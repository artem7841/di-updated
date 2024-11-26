namespace TagsCloudContainer;

public class WordsFrequencyCounter :  IWordsFrequencyCounter
{
    private Dictionary<string, int> wordsDictionary;
    
    public Dictionary<string, int> GetDictionary(IEnumerable<string> words)
    {
        wordsDictionary = new();
        foreach (var word in words)
        {
            if (wordsDictionary.ContainsKey(word))
            {
                wordsDictionary[word]++;
            }
            else
            {
                wordsDictionary.Add(word, 1);
            }
        }
        return wordsDictionary;
    }
}