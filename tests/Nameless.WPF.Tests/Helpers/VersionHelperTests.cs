using Nameless.Testing.Tools.Attributes;

namespace Nameless.WPF.Helpers;

[UnitTest]
public class VersionHelperTests {
    [Fact]
    public void WhenParsing_WhenValueIsSemVer_ThenReturnsVersion() {
        // arrange
        const string SemVer = "1.2.3";
        var expected = new Version(1, 2, 3);

        // act
        var actual = VersionHelper.Parse(SemVer);

        // assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void WhenParsing_WhenValueIsSemVer_WithRevision_ThenReturnsVersion() {
        // arrange
        const string SemVer = "1.2.3.4-alpha";
        var expected = new Version(1, 2, 3, 4);

        // act
        var actual = VersionHelper.Parse(SemVer);

        // assert
        Assert.Equal(expected, actual);
    }
}
