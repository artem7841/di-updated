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
        public Composition(string path)
        {
            _path = path;
        }
        private static void Setup()
        {
            DI.Setup(nameof(Composition))
                .Bind<IWordSource>().To<WordSource>(_ => new WordSource(_path))
                .Bind<IWordsPreprocessor>().To<WordsPreprocessor>()
                .Bind<ICloudLayouter>().To(_ => new CircularCloudLayouter(new Point(0, 0)))
                .Bind<IFontMeasurer>().To<FontMeasurer>()
                .Bind<ICloudBuilder>().To<CloudBuilder>()
                .Bind<IWordFilter>().To<WordFilter>()
                .Bind<IImageDrawer>().To(_ => new RectanglesDrawer(new Point(0, 0)))
                .Bind<Program.ProgramService>().To<Program.ProgramService>()
                .Root<Program.ProgramService>("ProgramService");
        }
    }
}