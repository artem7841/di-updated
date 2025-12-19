using System.Drawing;

namespace TagsCloudContainer.Classes;

public class RandomColorScheme : IColorScheme
{
    private readonly Random _random;
    
    public RandomColorScheme()
    {
        _random = new Random(10);
    }

    public Color GetColorForWord(string word, int frequency, int index, int totalWords)
    {
        return Color.FromArgb(
            _random.Next(256),  
            _random.Next(256), 
            _random.Next(256)); 
    }
}