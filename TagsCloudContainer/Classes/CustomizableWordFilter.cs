using System.Collections.Frozen;

namespace TagsCloudContainer.Classes;

public class CustomizableWordFilter : IWordFilter
{
    private readonly FrozenSet<string> _excludedWords;
    
    public IEnumerable<string> Filter(IEnumerable<string> words)
    {
        return words.Where(x => !_excludedWords.Contains(x));
    }

    public CustomizableWordFilter(string excludedWordsFilePath)
    {
        if (string.IsNullOrWhiteSpace(excludedWordsFilePath))
        {
            _excludedWords = FrozenSet<string>.Empty;
            return;
        }
        
        try
        {
            var excludedWords = File.ReadLines(excludedWordsFilePath)
                .Select(line => line.Trim())
                .Where(word => !string.IsNullOrWhiteSpace(word))
                .Distinct(StringComparer.OrdinalIgnoreCase);
            
            _excludedWords = excludedWords.ToFrozenSet(StringComparer.OrdinalIgnoreCase);
        }
        catch (FileNotFoundException e)
        {
            throw new FileNotFoundException($"File with excluded words not found", e);
        }
        catch (DirectoryNotFoundException e)
        {
            throw new DirectoryNotFoundException("Directory for excluded words file not found", e);
        }
        catch (IOException e)
        {
            throw new IOException($"Error reading excluded words file", e);
        }
        catch (ArgumentException e)
        {
            throw; 
        }
        catch (Exception e)
        {
            throw new InvalidOperationException($"Failed to initialize word filter from file ", e);
        }
    }
}