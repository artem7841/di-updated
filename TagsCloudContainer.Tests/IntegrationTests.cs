using System.Drawing;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudContainer.Tests;

[TestFixture] 
public class integrationTests
{
    private string _testDirectory;
    private string _debugOutputDirectory;
    private Size _size;
    [SetUp]
    public void Setup()
    {
        _debugOutputDirectory = Directory.GetCurrentDirectory();
        _size = new Size(800, 600);
        
        _testDirectory = Path.Combine(_debugOutputDirectory, "TestCloudImages");
        Directory.CreateDirectory(_testDirectory);
            
        Console.WriteLine($"Test directory: {_testDirectory}");
        Console.WriteLine($"Debug output directory: {_debugOutputDirectory}");
    }
    
    private ApplicationOptions CreateTestOptions(string inputFile, string outputFile)
    {
        return new ApplicationOptions
        {
            InputFile = inputFile,
            OutputFile = outputFile,
            Size = _size,
            Font = "Arial",
            FontColor = Color.Black,
            BackgroundColor = Color.White,
            Algorithm = "spiral",
            AlgorithmParameters = new Dictionary<string, object>
            {
                ["SpiralCoefficient"] = 3,
                ["StartAngle"] = 0
            }
        };
    }

    [Test]
    public void Program_ValidInputFile_CreatesImageFile()
    {
        var inputFile = Path.Combine(_testDirectory, "input.txt");
        var outputFile = Path.Combine(_testDirectory, "output.png");
        File.WriteAllLines(inputFile, new[] { "привет", "мир", "привет", "тест", "привет", 
            "мир", "интеграция", "тест", "облако", "тег", "облако" });
        var options = CreateTestOptions(inputFile, outputFile);
        var composition = new Composition(options);
        var cloudApp = composition.CloudApplication;
        
        cloudApp.Run(options);
        
        File.Exists(outputFile).Should().BeTrue();
        new FileInfo(outputFile).Length.Should().BeGreaterThan(0);
    }
    
    [Test]
    public void Program_WithDifferentFontAndColors_CreatesImage()
    {
        var inputFile = Path.Combine(_testDirectory, "input.txt");
        var outputFile = Path.Combine(_testDirectory, "styled.png");
        File.WriteAllLines(inputFile, new[] { "красный", "зеленый", "синий", "желтый", 
            "фиолетовый", "оранжевый", "розовый", "черный", "серый", "белый" });
        var options = new ApplicationOptions
        {
            InputFile = inputFile,
            OutputFile = outputFile,
            Size = _size,
            Font = "Times New Roman",
            FontColor = Color.Red,
            BackgroundColor = Color.LightGray,
            Algorithm = "spiral",
            AlgorithmParameters = new Dictionary<string, object>
            {
                ["SpiralCoefficient"] = 3,
                ["StartAngle"] = 0
            }
        };
        
        var composition = new Composition(options);
        var cloudApp = composition.CloudApplication;
        
        cloudApp.Run(options);
        
        File.Exists(outputFile).Should().BeTrue();
    }
    
    [Test]
    public void Program_WithLargeInput_CreatesImageSuccessfully()
    {
        // Arrange
        var inputFile = Path.Combine(_testDirectory, "large_input.txt");
        var outputFile = Path.Combine(_testDirectory, "large_output.png");
        
        var lines = Enumerable.Range(1, 10)
            .SelectMany(i => Enumerable.Repeat($"word{i}", i))
            .ToList();
            
        File.WriteAllLines(inputFile, lines);

        var options = CreateTestOptions(inputFile, outputFile);
        var composition = new Composition(options);
        var cloudApp = composition.CloudApplication;

        cloudApp.Run(options);
        
        File.Exists(outputFile).Should().BeTrue();
    }
    
    [Test]
    public void Program_InputFileWithSpaces_ThrowsException()
    {
        var inputFile = Path.Combine(_testDirectory, "invalid_input.txt");
        var outputFile = Path.Combine(_testDirectory, "output.png");
        File.WriteAllLines(inputFile, new[]
        { "яблоко", "сл ива", "груша" });
        var options = CreateTestOptions(inputFile, outputFile);
        var composition = new Composition(options);
        var cloudApp = composition.CloudApplication;
        
        Action act = () => cloudApp.Run(options);

        act.Should().Throw<InvalidDataException>();
    }
    
    [Test]
    public void Program_InvalidFontName_UsesDefaultFont()
    {
        var inputFile = Path.Combine(_testDirectory, "input.txt");
        var outputFile = Path.Combine(_testDirectory, "output.png");
        File.WriteAllLines(inputFile, new[] { "test", "font" });
        var options = new ApplicationOptions
        {
            InputFile = inputFile,
            OutputFile = outputFile,
            Size = _size,
            Font = "Font123", 
            FontColor = Color.Black,
            BackgroundColor = Color.White,
            Algorithm = "spiral",
            AlgorithmParameters = new Dictionary<string, object>
            {
                ["SpiralCoefficient"] = 3,
                ["StartAngle"] = 0
            }
        };
        
        var composition = new Composition(options);
        var cloudApp = composition.CloudApplication;
        
        cloudApp.Run(options);
        
        File.Exists(outputFile).Should().BeTrue();
    }
    
    [Test]
    public void Program_WithUnicodeCharacters_ProcessesCorrectly()
    {
        var inputFile = Path.Combine(_testDirectory, "unicode.txt");
        var outputFile = Path.Combine(_testDirectory, "unicode_output.png");
        File.WriteAllLines(inputFile, new[]
        {
            "привет", "мир", "こんにちは", "世界",
        });

        var options = CreateTestOptions(inputFile, outputFile);
        var composition = new Composition(options);
        var cloudApp = composition.CloudApplication;

        cloudApp.Run(options);
        
        File.Exists(outputFile).Should().BeTrue();
    }

    [Test]
    public void Program_With30UniqueRussianWords_GeneratesValidCloud()
    {
        var inputFile = Path.Combine(_testDirectory, "test_words.txt");
        var outputFile = Path.Combine(_testDirectory, "test_cloud.png");
            
        var russianWords = new List<string> { "солнце", "луна", "звезда", "небо", "облако", "ветер", "дождь", "снег", "град", "туман",
            "лес", "гора", "река", "озеро", "море", "город", "деревня", "улица", "дом", "окно",
            "книга", "стол", "стул", "дверь", "цветок", "птица", "рыба", "зверь", "человек", "друг"
        };
        
        var random = new Random(); 
        var allWords = new List<string>();
            
        foreach (var word in russianWords)
        {
            var count = random.Next(1, 11);
            allWords.AddRange(Enumerable.Repeat(word, count));
        }
            
        allWords = allWords.OrderBy(x => random.Next()).ToList();
            
        File.WriteAllLines(inputFile, allWords);
        
        var options = new ApplicationOptions
        {
            InputFile = inputFile,
            OutputFile = outputFile,
            Size = _size,
            Font = "Arial",
            FontColor = Color.DarkBlue,
            BackgroundColor = Color.AliceBlue,
            Algorithm = "spiral",
            AlgorithmParameters = new Dictionary<string, object>
            {
                ["SpiralCoefficient"] = 3,
                ["StartAngle"] = 0
            }
        };
        
        var composition = new Composition(options);
        var cloudApp = composition.CloudApplication;
            
        cloudApp.Run(options);
            
        File.Exists(outputFile).Should().BeTrue($"The file {outputFile} should be created");
            
        var fileInfo = new FileInfo(outputFile);
        fileInfo.Length.Should().BeGreaterThan(0, $"The file should not be empty");
            
        var linesFromFile = File.ReadAllLines(inputFile);
        linesFromFile.Length.Should().Be(allWords.Count, "Count lines must match");
            
        foreach (var word in russianWords)
        {
            linesFromFile.Should().Contain(word, $"The file must contain the word '{word}'");
        }
    }    
    
    [Test]
    public void Program_EmptyInputFile_CreatesEmptyImage()
    {
        var inputFile = Path.Combine(_testDirectory, "empty.txt");
        var outputFile = Path.Combine(_testDirectory, "empty_output.png");
        
        File.WriteAllText(inputFile, string.Empty); 
        
        var options = CreateTestOptions(inputFile, outputFile);
        var composition = new Composition(options);
        var cloudApp = composition.CloudApplication;
        
        var act = () => cloudApp.Run(options);

        act.Should().Throw<InvalidOperationException>();
    }
    
    [Test]
    public void Program_WithNonexistentInputFile_ShouldThrowFileNotFoundException()
    {
        var nonexistentFile = Path.Combine(_testDirectory, "nonexistent_" + Guid.NewGuid() + ".txt");
        var outputFile = Path.Combine(_testDirectory, "output.png");

        if (File.Exists(nonexistentFile))
        {
            File.Delete(nonexistentFile);
        }
    
        var options = CreateTestOptions(nonexistentFile, outputFile);
        var composition = new Composition(options);
        var cloudApp = composition.CloudApplication;
        
        Action act = () => cloudApp.Run(options);
        
        act.Should().Throw<FileNotFoundException>().WithMessage($"*{nonexistentFile}*");
    }
}