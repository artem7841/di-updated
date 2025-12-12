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