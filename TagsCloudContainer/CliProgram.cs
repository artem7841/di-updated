using System;
using System.Drawing;
using System.Linq;
using CommandLine;
using CommandLine.Text;
using TagsCloudContainer.Classes;

namespace TagsCloudContainer
{
    public class CliProgram
    {
        public static void Main(string[] args)
        {
            try
            {
                var parser = new Parser(with => with.HelpWriter = null);
                var result = parser.ParseArguments<CommandLineOptions>(args);
    
                result.WithParsed(options => RunApplication(options));
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
        private static void RunApplication(CommandLineOptions cliOptions)
        {
            try
            {
                var appOptions = new ApplicationOptions
                {
                    InputFile = cliOptions.InputFile,
                    OutputFile = cliOptions.OutputFile,
                    Size = ParseSize(cliOptions.Size),
                    Font = cliOptions.Font,
                    FontColor = ParseColor(cliOptions.FontColor),
                    BackgroundColor = ParseColor(cliOptions.BackgroundColor),
                    Algorithm = cliOptions.Algorithm,
                    AlgorithmParameters = new Dictionary<string, object>
                    {
                        ["SpiralCoefficient"] = cliOptions.SpiralCoefficient,
                        ["StartAngle"] = cliOptions.StartAngle
                    }
                };
                
                string filterConfig = GetFilterConfig(cliOptions);
                
                IColorScheme colorScheme = CreateColorSchemeFromCli(cliOptions);
                
                var composition = new Composition(appOptions, filterConfig, colorScheme);
                var app = composition.CloudApplication;
                app.Run(appOptions);
        
                Console.WriteLine($"Cloud saved to: {cliOptions.OutputFile}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Environment.Exit(1);
            }
        }
        
        private static IColorScheme CreateColorSchemeFromCli(CommandLineOptions cliOptions)
        {
            return cliOptions.ColorScheme?.ToLower() switch
            {
                "random" => new RandomColorScheme(),
                "single" => new SingleColorScheme(ParseColor(cliOptions.FontColor)),
                "gradient" => new GradientColorScheme(ParseColor(cliOptions.FontColor))
            };
        }
        
        private static string GetFilterConfig(CommandLineOptions options)
        {
            if (options.NoExclude)
            {
                return null; 
            }
            
            if (!string.IsNullOrEmpty(options.ExcludeFile))
            {
                if (!File.Exists(options.ExcludeFile))
                {
                    Console.WriteLine($"Warning: Exclude file '{options.ExcludeFile}' not found.");
                    return null;
                }
                return options.ExcludeFile;
            }
            return null;
        }
        
        private static Size ParseSize(string sizeStr)
        {
            var parts = sizeStr.Split('x', 'X');
            if (parts.Length != 2 || !int.TryParse(parts[0], out int width) || 
                !int.TryParse(parts[1], out int height))
            {
                throw new ArgumentException($"Invalid size format: {sizeStr} (should be WIDTHxHEIGHT)");
            }
            return new Size(width, height);
        }

        private static Color ParseColor(string colorName)
        {
            try
            {
                return Color.FromName(colorName);
            }
            catch
            {
                throw new ArgumentException($"Unknown color: {colorName}");
            }
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
            
        }
    }
}