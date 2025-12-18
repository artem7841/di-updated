using System.Collections.Frozen;

namespace TagsCloudContainer.Classes;

public class CustomizableWordFilter : IWordFilter
{
    private readonly HashSet<string> _excludedWords;
    
    public IEnumerable<string> Filter(IEnumerable<string> words)
    {
        return words.Where(x => !_excludedWords.Contains(x));
    }

    public CustomizableWordFilter(string excludedWordsFilePath)
    {
        _excludedWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        
        if (!string.IsNullOrEmpty(excludedWordsFilePath) && File.Exists(excludedWordsFilePath))
        {
            var lines = File.ReadAllLines(excludedWordsFilePath);
            foreach (var line in lines)
            {
                var word = line.Trim();
                if (!string.IsNullOrEmpty(word))
                {
                    _excludedWords.Add(word);
                }
            }
        }
    }
}