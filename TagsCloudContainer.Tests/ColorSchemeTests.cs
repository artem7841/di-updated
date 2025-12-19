using System.Drawing;
using FluentAssertions;
using TagsCloudContainer.Classes;

namespace TagsCloudContainer.Tests;
[TestFixture]
public class ColorSchemeTests
{
    [Test]
    public void RandomColorScheme_WithSameSeed_ShouldGenerateSameColors()
    {
        var scheme1 = new RandomColorScheme();
        var scheme2 = new RandomColorScheme();
    
        var color1 = scheme1.GetColorForWord("word", 1, 0, 10);
        var color2 = scheme2.GetColorForWord("word", 1, 0, 10);
    
        color1.Should().Be(color2);
    }

    [Test]
    public void SingleColorScheme_ShouldReturnSameColorForAllWords()
    {
        var expectedColor = Color.FromArgb(255, 0, 0, 255); 
        var scheme = new SingleColorScheme(expectedColor);
        
        var color1 = scheme.GetColorForWord("word1", frequency: 1, index: 0, totalWords: 10);
        var color2 = scheme.GetColorForWord("word2", frequency: 100, index: 99, totalWords: 100);
        var color3 = scheme.GetColorForWord("", frequency: 0, index: 0, totalWords: 1);
    
        color1.Should().Be(expectedColor);
        color2.Should().Be(expectedColor);
        color3.Should().Be(expectedColor);
    }
    
    [Test]
    public void GradientColorScheme_ColorsShouldFollowMonotonicGradient()
    {
        var scheme = new GradientColorScheme(Color.FromArgb(255, 50, 100, 150));
        var fontSizes = new[] { 10, 20, 30, 40, 50 };
        
        var colors = fontSizes
            .Select(fontSize => scheme.GetColorForWord("word", fontSize, 0, 5))
            .Select(CalculateBrightness)
            .ToList();
        
        for (int i = 0; i < colors.Count - 1; i++)
        {
            colors[i].Should().BeGreaterThan(colors[i + 1], 
                $"brightness should decrease as font size increases from {fontSizes[i]} to {fontSizes[i + 1]}");
        }
    }
    private static float CalculateBrightness(Color color)
    {
        return (color.R + color.G + color.B) / (3f * 255);
    }
}