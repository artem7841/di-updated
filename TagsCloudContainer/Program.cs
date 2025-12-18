using System;
using System.Drawing;
using System.Linq;

namespace TagsCloudContainer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var composition = new Composition("test.txt", "Arial");
            var programService = composition.ProgramService;
            var size = new  Size(800, 600);
            
            programService.Run( "image.png", "Arial", Color.Blue, Color.AliceBlue, size);
            programService.Run( "image2.png", "Arial", Color.Aqua, Color.AliceBlue, size);
        }
        
        public class ProgramService
        {
            private readonly IWordSource _wordSource;
            private readonly IWordsPreprocessor _preprocessor;
            private readonly ICloudBuilder<SpiralParameters> _cloudBuilder;
            private readonly IImageDrawer _drawer;
            private readonly IWordFilter _wordFilter;
        
            public ProgramService(
                IWordSource wordSource,
                IWordsPreprocessor preprocessor,
                ICloudBuilder<SpiralParameters> cloudBuilder,
                IImageDrawer drawer,
                IWordFilter wordFilter)
            {
                _wordSource = wordSource;
                _preprocessor = preprocessor;
                _cloudBuilder = cloudBuilder;
                _drawer = drawer;
                _wordFilter = wordFilter;
            }
        
            public void Run(string outputFile, string fontStyle, Color fontColor, Color backgroundColor, Size size)
            {
                var words = _wordSource.GetWords();
                words = _preprocessor.Process(words);
                words = _wordFilter.Filter(words);
                SpiralParameters parameters = new SpiralParameters(3, 0);
                var wordRectangles = _cloudBuilder.BuildCloud(words, size, parameters);
                ImageDrawerOptions options = new ImageDrawerOptions(wordRectangles, outputFile, 
                    fontStyle, fontColor, backgroundColor, size);
                
                _drawer.GenerateImage(options);
            }
        }
    }
}