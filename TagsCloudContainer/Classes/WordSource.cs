using System.Reflection.Metadata;

namespace TagsCloudContainer;

public class WordSource : IWordSource
{
    public IEnumerable<string> GetWords(string path)
    {
        using var reader = new StreamReader(path);
        List<string> words = new List<string>();
        
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            words.Add(line);
        }

        return words;
    }
    
}