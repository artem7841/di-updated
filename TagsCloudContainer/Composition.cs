using System.Drawing;
using Pure.DI;
using TagsCloudContainer;
using TagsCloudContainer.Classes;
using TagsCloudVisualization;

namespace TagsCloudContainer
{
    public partial class Composition
    {
        private static string _path;
        private static string _font;
        public Composition(string path, string font)
        {
            _path = path;
            _font = font;
        }
        private static void Setup()
        {
            DI.Setup(nameof(Composition))
                .Bind<IWordSource>().To<WordSource>(_ => new WordSource(_path))
                .Bind<IWordsPreprocessor>().To<WordsPreprocessor>()
                .Bind<IFontMeasurer>().To(_ => new FontMeasurer(_font))
                .Bind<ICloudBuilder<SpiralParameters>>().To<SpiralCloudBuilder>()
                .Bind<IWordFilter>().To<WordFilter>()
                .Bind<IImageDrawer>().To(_ => new RectanglesDrawer())
                .Bind<Program.ProgramService>().To<Program.ProgramService>()
                .Root<Program.ProgramService>("ProgramService");
        }
    }
}