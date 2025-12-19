using System.Drawing;
using Pure.DI;
using TagsCloudContainer;
using TagsCloudContainer.Classes;
using TagsCloudVisualization;

namespace TagsCloudContainer
{
    public partial class Composition
    {
        private readonly ApplicationOptions _appOptions;
        private readonly string _excludedWordsPath;
        private readonly IColorScheme _colorScheme;
        private readonly IImageDrawer _imageDrawer;

        public Composition(ApplicationOptions options, string excludedWordsPath = null,
            IColorScheme colorScheme = null, IImageDrawer imageDrawer = null)
        {
            _appOptions = options;
            _excludedWordsPath = excludedWordsPath;
            _colorScheme = colorScheme ?? new SingleColorScheme(options.FontColor);
            _imageDrawer = imageDrawer;
            Setup();
        }
        
        private void Setup()
        {
            DI.Setup(nameof(Composition))
                .Bind<ApplicationOptions>().To(_ => _appOptions)
                .Bind<IWordSource>().To<WordSource>(_ => new WordSource(_appOptions.InputFile))
                .Bind<IFontMeasurer>().To(_ => new FontMeasurer(_appOptions.Font))
                
                .Bind<IColorScheme>().To(_ => _colorScheme)
                .Bind<IImageDrawer>().To(_ => _imageDrawer ?? new RectanglesDrawer(_colorScheme))
                
                .Bind<IWordsPreprocessor>().To<WordsPreprocessor>()
                .Bind<ICloudBuilder<SpiralParameters>>().To<SpiralCloudBuilder>()
                .Bind<IWordFilter>().To(_ => new CustomizableWordFilter(_excludedWordsPath))
                .Bind<CliProgram.ProgramService>().To<CliProgram.ProgramService>()
                .Bind<ICloudApplication>().To<CloudApplication>()
                .Root<ICloudApplication>("CloudApplication");
        }
    }
}