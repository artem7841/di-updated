using System.Drawing;

namespace TagsCloudContainer;

public class CloudBuilder : ICloudBuilder
{
    private readonly IWordsFrequencyCounter _wordsCounter;
    private readonly ICloudLayouter _layouter;
    private readonly IFontMeasurer _fontMeasurer; 

    public CloudBuilder(IWordsFrequencyCounter wordsCounter, ICloudLayouter layouter, IFontMeasurer fontMeasurer)
    {
        _wordsCounter = wordsCounter;
        _layouter = layouter;
        _fontMeasurer = fontMeasurer;
    }

    public IEnumerable<WordRectangle> BuildCloud(IEnumerable<string> words, Size size)
    {
        var wordFreq = _wordsCounter.GetDictionary(words);
        var rectangles = new List<WordRectangle>();
        
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