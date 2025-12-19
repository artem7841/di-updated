using TagsCloudContainer.Extensions;

namespace TagsCloudContainer.Classes;

public class CloudApplication : ICloudApplication
{
    private readonly IWordSource _wordSource;
    private readonly IWordsPreprocessor _preprocessor;
    private readonly IWordFilter _wordFilter;
    private readonly ICloudBuilder<SpiralParameters> _cloudBuilder;
    private readonly IImageDrawer _drawer;
    
    public CloudApplication(
        IWordSource wordSource,
        IWordsPreprocessor preprocessor,
        IWordFilter wordFilter,
        ICloudBuilder<SpiralParameters> cloudBuilder,
        IImageDrawer drawer)
    {
        _wordSource = wordSource;
        _preprocessor = preprocessor;
        _wordFilter = wordFilter;
        _cloudBuilder = cloudBuilder;
        _drawer = drawer;
    }
    
    public void Run(ApplicationOptions options)
    {
        var words = _wordSource.GetWords();
        words = _preprocessor.Process(words);
        words = _wordFilter.Filter(words);
        var parameters = options.AlgorithmParameters.ToSpiralParameters();
            
        var wordRectangles = _cloudBuilder.BuildCloud(words, options.Size, parameters);
        ImageDrawerOptions drawerOptions = new ImageDrawerOptions(
            wordRectangles, 
            options.OutputFile, 
            options.Font, 
            options.FontColor, 
            options.BackgroundColor, 
            options.Size);
                
        _drawer.GenerateImage(drawerOptions);
    }
}
