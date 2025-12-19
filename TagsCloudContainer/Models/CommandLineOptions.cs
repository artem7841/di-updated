using CommandLine;
using CommandLine.Text;

namespace TagsCloudContainer;

public class CommandLineOptions
{
    [Value(0, MetaName = "input", HelpText = "Input text file")]
    public string InputFile { get; set; }
    
    [Value(1, MetaName = "output", Default = "cloud.png", HelpText = "Output image file")]
    public string OutputFile { get; set; }
    
    [Option( "size", Default = "800x600", HelpText = "Image size (width x height)")]
    public string Size { get; set; }
    
    [Option( "font", Default = "Arial", HelpText = "Font name")]
    public string Font { get; set; }
    
    [Option("font-color", Default = "Blue", HelpText = "Font color name")]
    public string FontColor { get; set; }
    
    [Option("bg-color", Default = "White", HelpText = "Background color name")]
    public string BackgroundColor { get; set; }
    
    [Option( "algorithm", Default = "spiral", HelpText = "Layout algorithm")]
    public string Algorithm { get; set; }
    
    [Option("spiral-coef", Default = 3, HelpText = "Spiral coefficient")]
    public int SpiralCoefficient { get; set; }
    
    [Option("start-angle", Default = 0, HelpText = "Start angle for spiral")]
    public int StartAngle { get; set; }

    [Option("exclude-file", HelpText = "File with words to exclude")]
    public string ExcludeFile { get; set; }
    
    [Option("no-exclude", Default = false, HelpText = "Disable word exclusion")]
    public bool NoExclude { get; set; }

    [Option("color-scheme", Default = "single", HelpText = "Color scheme: single, random, gradient")]
    public string ColorScheme { get; set; }
    
}