namespace TagsCloudContainer;

sealed class WordsPreprocessor : IWordsPreprocessor
{
    public IEnumerable<string> Process(IEnumerable<string> words)
    {
        return words.Select(w => w.ToLower());
    }
}