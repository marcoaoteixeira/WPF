// Cake Build Automation System.
// Learn more: https://cakebuild.net

//////////////////////////////////////////////////////////////////////
// INITIALIZATION
//////////////////////////////////////////////////////////////////////

Information("*** Starting Build Process ***");
Information(string.Empty);

// Set the working directory to the current directory
if (HasArgument("working-dir")) {
    var argument = Argument<DirectoryPath>("working-dir");
    var workingDir = !System.IO.Path.IsPathRooted(argument.FullPath)
        ? Context.Environment.WorkingDirectory.Combine(argument)
        : argument;

    Context.Environment.WorkingDirectory = workingDir;
}
Information($"\t- Working directory: {Context.Environment.WorkingDirectory}");

//////////////////////////////////////////////////////////////////////
// GLOBAL ARGUMENTS
//////////////////////////////////////////////////////////////////////

var solutionPath = Argument("solution", string.Empty);
if (string.IsNullOrEmpty(solutionPath)) {
    Information("\t\t!!! Solution was not provided. Searching for the first valid match: *.sln...");
    var solutionFile = Context
        .FileSystem.GetDirectory(Context.Environment.WorkingDirectory)
        .GetFiles("*.sln", SearchScope.Current)
        .FirstOrDefault();
    if (solutionFile is null) {
        Error("!!!!! NO SOLUTION FILE FOUND !!!!");
        Environment.Exit(1);
    }

    solutionPath = solutionFile.Path.FullPath;
    Information($"\t- Solution: {solutionPath}");
}

var configuration = Argument("configuration", "Release");
Information($"\t- Configuration: {configuration}");

var verbosity = Argument("verbosity", DotNetVerbosity.Normal);
Information($"\t- Verbosity: {verbosity}");

var testFilter = Argument("test-filter", string.Empty);
Information($"\t- Test filters: {testFilter}");

var testLogger = Argument("test-logger", "console;verbosity=normal");
Information($"\t- Test logger: {testLogger}");

var testCollector = Argument("test-collector", "XPlat Code Coverage");
Information($"\t- Test collector: {testCollector}");

var codeCoverageDir = Argument<DirectoryPath>("code-coverage-dir", "code_coverage");
Information($"\t- Code coverage output directory: {codeCoverageDir}");

Information($"\t- Open code coverage report? {HasArgument("open-code-coverage")}");

var nupkgOutputDir = Argument<DirectoryPath>("nupkg-output-dir", "nupkgs");
Information($"\t- NuGet packages output directory: {nupkgOutputDir}");

var nugetSource = Argument("nuget-source", "https://api.nuget.org/v3/index.json");
Information($"\t- NuGet source: {nugetSource}");

var nugetApiKey = Argument("nuget-api-key", string.Empty);
Information($"\t- Is NuGet API key present? {!string.IsNullOrWhiteSpace(nugetApiKey)}");

var releaseVersion = Argument("release-version", "1.0.0");
if (!SemVersion.TryParse(releaseVersion, out var _)) {
    Information("\t\t!!! Release version is not a valid SemVersion. Using default version: 1.0.0");
    releaseVersion = "1.0.0";
}
Information($"\t- Release version: {releaseVersion}");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

// Cleans the solution
Task("Clean")
    .Does(() => {
        DotNetClean(solutionPath, new DotNetCleanSettings {
            Configuration = configuration,
            NoLogo = true,
            Verbosity = verbosity
        });

        // Delete previous NuGet packages files
        Information("*** Cleaning NuGet Packages Directory ***");
        CleanDirectory(nupkgOutputDir, new CleanDirectorySettings {
            Force = true
        });

        // Delete previous code coverage results
        Information("*** Cleaning Code Coverage Report Directory ***");
        CleanDirectory(codeCoverageDir, new CleanDirectorySettings {
            Force = true
        });
    });

// Restores the solution
Task("Restore")
    .Does(() => {
        DotNetRestore(solutionPath, new DotNetRestoreSettings {
            Verbosity = verbosity
        });
    });

// Executes the vanilla build process
Task("Build")
    .IsDependentOn("Restore")
    .Does(() => {
        DotNetBuild(solutionPath, new DotNetBuildSettings {
            Configuration = configuration,
            MSBuildSettings = new DotNetMSBuildSettings {
                AssemblyVersion = releaseVersion,
                FileVersion = releaseVersion,
                Version = releaseVersion,
            },
            NoLogo = true,
            NoRestore = true,
            Verbosity = verbosity
        });
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() => {
        DotNetTest(solutionPath, new DotNetTestSettings {
            Configuration = configuration,
            NoLogo = true,
            NoBuild = true,
            NoRestore = true,
            Filter = testFilter,
            Collectors = [testCollector],
            Loggers = [testLogger],
            ResultsDirectory = codeCoverageDir.Combine("test_results"),
            Verbosity = verbosity
        });
    });

Task("CodeCoverage")
    .IsDependentOn("Test")
    .Does(() => {
        // the '**' in the path is a wildcard that matches any number of directories
        FilePath coberturaFilePath = codeCoverageDir.Combine("**").CombineWithFilePath("coverage.cobertura.xml");
        DirectoryPath reportDirectoryPath = codeCoverageDir.Combine("coverage_report");

        Information($"*** Generating Code Coverage Report ***");
        ReportGenerator(coberturaFilePath, reportDirectoryPath, new ReportGeneratorSettings {
            ReportTypes = [ReportGeneratorReportType.Html],
            Verbosity = ReportGeneratorVerbosity.Verbose
        });

        if (!HasArgument("open-code-coverage")) { return; }
        Information($"*** Opening Code Coverage Report ***");

        FilePath reportFile = reportDirectoryPath.GetFilePath("index.html");
        if (!Context.FileSystem.Exist(reportFile)) {
            Information($"\t\t !!!Code coverage report not found: {reportFile}.");

            return;
        }

        Information($"\t- Code coverage report file: {reportFile}");
        switch (Context.Environment.Platform.Family) {
            case PlatformFamily.Windows:
                StartProcess("cmd.exe", $"/C start {reportFile}");
                break;
            case PlatformFamily.Linux:
                StartProcess("xdg-open", reportFile.FullPath);
                break;
            case PlatformFamily.OSX:
                StartProcess("open", reportFile.FullPath);
                break;
        }
    });

Task("NuGetPack")
    .IsDependentOn("Test")
    .Does(() => {
        DotNetPack(solutionPath, new DotNetPackSettings {
            Configuration = configuration,
            IncludeSymbols = true,
            OutputDirectory = nupkgOutputDir,
            MSBuildSettings = new DotNetMSBuildSettings {
                PackageVersion = releaseVersion,
            },
            Verbosity = verbosity,
            NoLogo = true,
            NoBuild = true,
            NoRestore = true,
        });
    });

Task("NuGetPublish")
    .IsDependentOn("Clean")
    .IsDependentOn("NuGetPack")
    .Does(() => {
        if (!HasArgument("nuget-api-key")) {
            Error("!!!!! MISSING NUGET API KEY !!!!!");

            Environment.Exit(1);
        }

        var settings = new DotNetNuGetPushSettings {
            ApiKey = nugetApiKey,
            Source = nugetSource,
            SkipDuplicate = true,
        };

        var nupkgGlobPattern = System.IO.Path.Combine(nupkgOutputDir.ToString(), "**", "*.nupkg");
        var packages = GetFiles(nupkgGlobPattern);
        
        foreach (var package in packages) {
            DotNetNuGetPush(package, settings);
        }
    });

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Clean")
    .IsDependentOn("CodeCoverage");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(Argument("target", "Default"));