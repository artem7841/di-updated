using System.Drawing;

namespace TagsCloudContainer.Classes;

public class SingleColorScheme : IColorScheme
{
    private readonly Color _color;
        
    public SingleColorScheme(Color color)
    {
        _color = color;
    }
    
    public Color GetColorForWord(string word, int frequency, int index, int totalWords)
    {
        return _color;
    }
}