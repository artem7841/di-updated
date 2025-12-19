using System.Drawing;
using FluentAssertions;

namespace TagsCloudContainer.Tests;

public class CloudeBuilderTests
{
    [Test]
    public void BuildCloud_WhenWordLargerThanTargetSize_ShouldThrowException()
    {
        var fontMeasurer = new FontMeasurer(); 
        var builder = new SpiralCloudBuilder(fontMeasurer);
        
        var targetSize = new Size(10, 10); 
        var spiralParams = new SpiralParameters(5, 0);
        var words = new[] { "оченьдлинноесловооченьдлинноесловооченьдлинноесловооченьдлинноеслово" };
        
        Action act = () => builder.BuildCloud(words, targetSize, spiralParams).ToList();

        act.Should().Throw<InvalidOperationException>().WithMessage("*exceeds the image area size*");
    }
    
    [Test]
    public void BuildCloud_WithSmallWords_ShouldWorkCorrectly()
    {
        var fontMeasurer = new FontMeasurer();
        var builder = new SpiralCloudBuilder(fontMeasurer);
        var targetSize = new Size(500, 500);
        var spiralParams = new SpiralParameters(5, 0);
        var words = new[] { "мама", "мыла", "раму", "привет", "мир" };
        
        var result = builder.BuildCloud(words, targetSize, spiralParams).ToList();
        
        result.Should().HaveCount(5);
    
        foreach (var wordRect in result)
        {
            wordRect.Word.Should().NotBeNullOrEmpty();
            wordRect.FontSize.Should().BeGreaterThan(0);
            wordRect.Rectangle.Width.Should().BeGreaterThan(0);
            wordRect.Rectangle.Height.Should().BeGreaterThan(0);
        }
    }

    [Test]
    public void BuildCloud_ShouldSortWordsByFrequency()
    {
        var fontMeasurer = new FontMeasurer();
        var builder = new SpiralCloudBuilder(fontMeasurer);
        var targetSize = new Size(500, 500);
        var spiralParams = new SpiralParameters(5, 0);
        var words = new[] { "раз", "два", "раз", "три", "раз", "два", "раз" };
        
        var result = builder.BuildCloud(words, targetSize, spiralParams).ToList();
        
        result.Should().HaveCount(3); 
        result[0].Word.Should().Be("раз"); 
        result[1].Word.Should().Be("два"); 
        result[2].Word.Should().Be("три"); 
        result[0].FontSize.Should().BeGreaterThan(result[1].FontSize);
        result[1].FontSize.Should().BeGreaterThan(result[2].FontSize);
    }

    [Test]
    public void BuildCloud_WithEmptyWords_ShouldReturnEmptyCollection()
    {
        var fontMeasurer = new FontMeasurer();
        var builder = new SpiralCloudBuilder(fontMeasurer);
        var targetSize = new Size(500, 500);
        var spiralParams = new SpiralParameters(5, 0);
        var words = Array.Empty<string>();
        
        var result = builder.BuildCloud(words, targetSize, spiralParams).ToList();

        result.Should().BeEmpty();
    }
    
}