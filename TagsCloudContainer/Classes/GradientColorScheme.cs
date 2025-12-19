using System.Drawing;

namespace TagsCloudContainer.Classes;

public class GradientColorScheme : IColorScheme
{
    private readonly Color _baseColor;
    private readonly float _minBrightness;
    private readonly float _maxBrightness;
    private readonly float _fontSizePower = 0.9f; 
    
    public GradientColorScheme(Color baseColor, float minBrightness = 0.2f, float maxBrightness = 1.0f, float fontSizePower = 0.9f)
    {
        _baseColor = baseColor;
        _minBrightness = Math.Clamp(minBrightness, 0f, 0.2f);
        _maxBrightness = Math.Clamp(maxBrightness, 0.9f, 1.0f);
        _fontSizePower = Math.Clamp(fontSizePower, 0.1f, 2f);
    }
    
    public Color GetColorForWord(string word, int fontSize, int index, int totalWords)
    {
        float minFontSize = 8f;
        float maxFontSize = 50f;
        
        float clampedFontSize = Math.Clamp(fontSize, minFontSize, maxFontSize);
        float normalized = (clampedFontSize - minFontSize) / (maxFontSize - minFontSize);
        
        normalized = (float)Math.Pow(normalized, _fontSizePower);
        float brightness = _maxBrightness - normalized * (_maxBrightness - _minBrightness);
        
        brightness = Math.Clamp(brightness, _minBrightness, _maxBrightness);
        
        return AdjustColorBrightness(_baseColor, brightness);
    }
    
    private Color AdjustColorBrightness(Color color, float brightness)
    {
        int r = (int)(color.R * brightness); 
        int g = (int)(color.G * brightness);
        int b = (int)(color.B * brightness);
        
        r = Math.Clamp(r, 0, 255);
        g = Math.Clamp(g, 0, 255);
        b = Math.Clamp(b, 0, 255);
        
        return Color.FromArgb(color.A, r, g, b);
    }
}