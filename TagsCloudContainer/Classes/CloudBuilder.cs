using System.Drawing;
using TagsCloudContainer.Extensions;

namespace TagsCloudContainer;

sealed class CloudBuilder : ICloudBuilder
{
    private readonly ICloudLayouter _layouter;
    private readonly IFontMeasurer _fontMeasurer; 

    public CloudBuilder(ICloudLayouter layouter, IFontMeasurer fontMeasurer)
    {
        _layouter = layouter;
        _fontMeasurer = fontMeasurer;
    }

    public IEnumerable<WordRectangle> BuildCloud(IEnumerable<string> words, Size size)
    {
        Dictionary<string, int> wordFreq = new();
        var rectangles = new List<WordRectangle>();
        
        foreach (var word in words)
        {
            wordFreq.Increment(word);
        }
        
        foreach (var word in wordFreq)
        {
            int fontSize = CalculateFontSize(word.Value);
            Size wordSize = _fontMeasurer.MeasureWord(word.Key, fontSize);
            Rectangle rectangle = _layouter.PutNextRectangle(wordSize);
            
            rectangles.Add(new WordRectangle(rectangle, word.Key, fontSize));
        }
        
        return rectangles;
    }
    
    private int CalculateFontSize(int frequency)
    {
        return Math.Min(10 + frequency * 2, 50);
    }
}