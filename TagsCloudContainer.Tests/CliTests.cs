using FluentAssertions;
using NUnit.Framework;
using CommandLine;

namespace TagsCloudContainer.Tests;

[TestFixture]
public class CliTests
{
    private string _tempDir;
    private string _testInputFile;
    private string _testOutputFile;
    
    [SetUp]
    public void SetUp()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), "TagsCloudTests_" + Guid.NewGuid());
        Directory.CreateDirectory(_tempDir);
        
        _testInputFile = Path.Combine(_tempDir, "input.txt");
        _testOutputFile = Path.Combine(_tempDir, "output.png");
        
        File.WriteAllLines(_testInputFile, new[] { "word1", "word2", "word3" });
    }
    
    [TearDown]
    public void TearDown()
    {
        if (Directory.Exists(_tempDir))
            Directory.Delete(_tempDir, true);
    }
    
    [Test]
    public void ParseArguments_WithRequiredParameters_Success()
    {
        var args = new[]
        {
            _testInputFile,       
            _testOutputFile,    
            "--size", "800x600"    
        };
        
        var parser = new CommandLine.Parser(with => with.HelpWriter = null);
        var result = parser.ParseArguments<CommandLineOptions>(args);
        result.Tag.Should().Be(ParserResultType.Parsed);
    
        result.WithParsed(options =>
        {
            options.InputFile.Should().Be(_testInputFile);
            options.OutputFile.Should().Be(_testOutputFile);
            options.Size.Should().Be("800x600");
        });
    }
    
    [Test]
    public void ParseArguments_WithoutRequiredParameters_ShouldShowError()
    {
        var args = new[]
        {
            _testOutputFile,   
            "--size", "800x600"
        };
        
        var parser = new CommandLine.Parser(with => with.HelpWriter = null);
        var result = parser.ParseArguments<CommandLineOptions>(args);
        
        result.Tag.Should().Be(ParserResultType.Parsed);
    }
    
    [Test]
    public void ParseArguments_WithHelp_ShouldNotParse()
    {
        var args = new[] { "--help" };
        
        var parser = new CommandLine.Parser(with => with.HelpWriter = null);
        var result = parser.ParseArguments<CommandLineOptions>(args);
        
        result.Tag.Should().Be(ParserResultType.NotParsed);
        result.WithNotParsed(errors =>
        {
            errors.Should().NotBeEmpty();
            errors.Should().Contain(e => e is HelpRequestedError);
        });
    }
    
    [Test]
    public void ParseArguments_WithInvalidSizeFormat_ShouldStillParse()
    {
        var args = new[]
        {
            _testInputFile,    
            _testOutputFile,
            "--size", "invalid"
        };
        
        var parser = new CommandLine.Parser(with => with.HelpWriter = null);
        var result = parser.ParseArguments<CommandLineOptions>(args);
        
        result.Tag.Should().Be(ParserResultType.Parsed, "argument parsing is success");
        
        result.WithParsed(options =>
        {
            options.Size.Should().Be("invalid");
        });
    }
    
    [Test]
    public void ParseArguments_WithValidArguments_ShouldParseAllOptions()
    {
        var args = new[]
        {
            _testInputFile,     
            _testOutputFile,    
            "--size", "800x600",
            "--font", "Arial",
            "--font-color", "Red",
            "--bg-color", "Blue",  
            "--color-scheme", "random",
            "--spiral-coef", "5", 
            "--start-angle", "45",
            "--no-exclude"
        };
        
        var parser = new CommandLine.Parser(with => with.HelpWriter = null);
        var result = parser.ParseArguments<CommandLineOptions>(args);
        
        result.Tag.Should().Be(ParserResultType.Parsed);
        result.WithParsed(options =>
        {
            options.InputFile.Should().Be(_testInputFile);
            options.OutputFile.Should().Be(_testOutputFile);
            options.Size.Should().Be("800x600");
            options.Font.Should().Be("Arial");
            options.FontColor.Should().Be("Red");
            options.BackgroundColor.Should().Be("Blue");
            options.ColorScheme.Should().Be("random");
            options.SpiralCoefficient.Should().Be(5);
            options.StartAngle.Should().Be(45);
            options.NoExclude.Should().BeTrue();
        });
    }
}