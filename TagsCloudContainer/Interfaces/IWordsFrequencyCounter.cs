namespace TagsCloudContainer;

public interface IWordsFrequencyCounter
{
    Dictionary<string, int> GetDictionary(IEnumerable<string> words);
}