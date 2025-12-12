using System;
using System.Drawing;
using System.Linq;

namespace TagsCloudContainer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var composition = new Composition();
            var programService = composition.programService;
            
            programService.Run("test.txt", "image.png", "Arial", Color.Blue, Color.AliceBlue);
        }
        
        public class ProgramService
        {
            private readonly IWordSource _wordSource;
            private readonly IWordsPreprocessor _preprocessor;
            private readonly ICloudBuilder _cloudBuilder;
            private readonly IImageDrawer _drawer;
            private readonly IWordFilter _wordFilter;
        
            public ProgramService(
                IWordSource wordSource,
                IWordsPreprocessor preprocessor,
                ICloudBuilder cloudBuilder,
                IImageDrawer drawer,
                IWordFilter wordFilter)
            {
                _wordSource = wordSource;
                _preprocessor = preprocessor;
                _cloudBuilder = cloudBuilder;
                _drawer = drawer;
                _wordFilter = wordFilter;
            }
        
            public void Run(string inputFile, string outputFile, string fontStyle, Color fontColor, Color backgroundColor)
            {
                var words = _wordSource.GetWords(inputFile);
                words = _preprocessor.Process(words);
                words = _wordFilter.Filter(words);
                Size size = new Size(1920, 1080);
                
                var wordRectangles = _cloudBuilder.BuildCloud(words, size);
                _drawer.GenerateImage(wordRectangles.ToList(), outputFile, fontStyle, fontColor, backgroundColor);
            }
        }
    }
}