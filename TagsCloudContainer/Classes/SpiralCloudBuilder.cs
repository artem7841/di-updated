using System.Drawing;
using TagsCloudContainer;
using TagsCloudContainer.Extensions;
using TagsCloudVisualization;

public class SpiralCloudBuilder : ICloudBuilder<SpiralParameters>
{
    private readonly IFontMeasurer _fontMeasurer;
    
    public SpiralCloudBuilder(IFontMeasurer fontMeasurer)
    {
        _fontMeasurer = fontMeasurer;
    }
    
    public IEnumerable<WordRectangle> BuildCloud(IEnumerable<string> words, Size targetSize, SpiralParameters spiralParameters)
    {
        var layouter = new CircularCloudLayouter(targetSize, spiralParameters.ACoef, spiralParameters.StartAngle);
        
        Dictionary<string, int> wordFreq = new();
        var rectangles = new List<WordRectangle>();
        
        foreach (var word in words)
        {
            wordFreq.Increment(word);
        }
        var sortedWords = wordFreq.OrderByDescending(pair => pair.Value).ToList();
        
        foreach (var word in sortedWords)
        {
            int fontSize = CalculateFontSize(word.Value, targetSize, sortedWords.Count);
            Size wordSize = _fontMeasurer.MeasureWord(word.Key, fontSize);

            float maxRatio = 0.5f; 
            while (wordSize.Width > targetSize.Width * maxRatio || wordSize.Height > targetSize.Height * maxRatio)
            {
                fontSize = (int)(fontSize * 0.7);
                wordSize = _fontMeasurer.MeasureWord(word.Key, fontSize);
            }
            
            Rectangle rectangle = layouter.PutNextRectangle(wordSize);
            
            rectangles.Add(new WordRectangle(rectangle, word.Key, fontSize));
        }
        
        return rectangles;
    }
    
    private int CalculateFontSize(int frequency, Size targetSize, int totalWords)
    {
        const float baseSize = 20f;
        const float referenceSize = 400f; 
        const float referenceWords = 10f; 
        const float frequencyMultiplier = 2f;
        
        float wordsFactor = referenceWords / totalWords;
        float sizeFactor = Math.Min(targetSize.Width, targetSize.Height) / referenceSize;
        int baseFont = (int)(baseSize * sizeFactor * wordsFactor);
        float fontSize = baseFont + frequency * frequencyMultiplier;
        
        return (int)fontSize;
    }
}