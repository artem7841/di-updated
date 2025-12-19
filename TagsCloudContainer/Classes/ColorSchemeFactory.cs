using System.Drawing;

namespace TagsCloudContainer.Classes;

public class ColorSchemeFactory : IColorSchemeFactory
{
    private readonly Dictionary<string, Func<Color, IColorScheme>> _schemes = new();
    
    public ColorSchemeFactory()
    {
        _schemes = new Dictionary<string, Func<Color, IColorScheme>>(StringComparer.OrdinalIgnoreCase)
        {
            ["single"] = color => new SingleColorScheme(color),
            ["random"] = color => new RandomColorScheme(),
            ["gradient"] = color => new GradientColorScheme(color),
        };
    }
    
    public IColorScheme Create(string schemeName, Color color)
    {
        var creator = _schemes.GetValueOrDefault(schemeName) 
                      ?? (color => new SingleColorScheme(color));
        
        return creator(color);
    }
}