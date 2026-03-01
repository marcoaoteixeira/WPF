using Lucene.Net.Search;
using Nameless.Bootstrap;
using Nameless.Bootstrap.Infrastructure;
using Nameless.Bootstrap.Notification;
using Nameless.Lucene.Infrastructure;

namespace Nameless.WPF.Bootstrap;

public class InitializeLuceneIndexStep : StepBase {
    private readonly IIndexProvider _indexProvider;

    public override string Name => "Initializing Lucene Index";

    public InitializeLuceneIndexStep(IIndexProvider indexProvider) {
        _indexProvider = indexProvider;
    }

    public override Task ExecuteAsync(FlowContext context, IProgress<StepProgress> progress, CancellationToken cancellationToken) {
        progress.ReportInformation(Name, "Initializing default index...");

        using var index = _indexProvider.Get(Constants.Lucene.UniqueIndexName);

        _ = index.Search(new MatchAllDocsQuery(), TopFieldCollector.Create(
            Sort.RELEVANCE,
            numHits: 1,
            fillFields: false,
            trackDocScores: false,
            trackMaxScore: false,
            docsScoredInOrder: false
        ));

        return Task.CompletedTask;
    }
}
