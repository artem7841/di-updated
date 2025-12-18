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

        public Composition(ApplicationOptions options, string excludedWordsPath = null)
        {
            _appOptions = options;
            _excludedWordsPath = excludedWordsPath;
            Setup();
        }
        
        private void Setup()
        {
            DI.Setup(nameof(Composition))
                .Bind<ApplicationOptions>().To(_ => _appOptions)
                .Bind<IWordSource>().To<WordSource>(_ => new WordSource(_appOptions.InputFile))
                .Bind<IFontMeasurer>().To(_ => new FontMeasurer(_appOptions.Font))
                .Bind<IImageDrawer>().To(_ => new RectanglesDrawer())
                .Bind<IWordsPreprocessor>().To<WordsPreprocessor>()
                .Bind<ICloudBuilder<SpiralParameters>>().To<SpiralCloudBuilder>()
                .Bind<IWordFilter>().To(_ => new CustomizableWordFilter(_excludedWordsPath))
                .Bind<CliProgram.ProgramService>().To<CliProgram.ProgramService>()
                .Bind<ICloudApplication>().To<CloudApplication>()
                .Root<ICloudApplication>("CloudApplication");
        }
    }
}