using System.Reflection.Metadata;

namespace TagsCloudContainer;

public class WordSource : IWordSource
{
    private string _filePath;
    public WordSource(string filePath)
    {
        _filePath = filePath;
    }

    public IEnumerable<string> GetWords()
    {
        using var reader = new StreamReader(_filePath);
        List<string> words = new List<string>();
        
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            if (line.Contains(' '))
            {
                throw new InvalidDataException($"The line in the file contains spaces");
            }
            
            if (string.IsNullOrWhiteSpace(line))
            {
                throw new InvalidDataException($"The line in the file is empty or contains only spaces.");
            }
            words.Add(line);
        }
        return words;
    }
    
}