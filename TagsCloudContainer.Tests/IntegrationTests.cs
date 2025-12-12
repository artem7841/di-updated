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
    [SetUp]
    public void Setup()
    {
        _debugOutputDirectory = Directory.GetCurrentDirectory();
        
        _testDirectory = Path.Combine(_debugOutputDirectory, "TestCloudImages");
        Directory.CreateDirectory(_testDirectory);
            
        Console.WriteLine($"Test directory: {_testDirectory}");
        Console.WriteLine($"Debug output directory: {_debugOutputDirectory}");
    }

    [Test]
    public void Program_ValidInputFile_CreatesImageFile()
    {
        var inputFile = Path.Combine(_testDirectory, "input.txt");
        var outputFile = Path.Combine(_testDirectory, "output.png");
        File.WriteAllLines(inputFile, new[] { "привет", "мир", "привет", "тест", "привет", 
            "мир", "интеграция", "тест", "облако", "тег", "облако" });
        var composition = new Composition();
        var programService = composition.programService;
        
        programService.Run(inputFile, outputFile, "Arial", Color.Blue, Color.White);
        
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
        var composition = new Composition();
        var programService = composition.programService;
        
        programService.Run(inputFile, outputFile, "Times New Roman", Color.Red, Color.LightGray);
        
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

        var composition = new Composition();
        var programService = composition.programService;
        
        programService.Run(inputFile, outputFile, "Arial", Color.Black, Color.White);
        
        File.Exists(outputFile).Should().BeTrue();
    }
    
    [Test]
    public void Program_InputFileWithSpaces_ThrowsException()
    {
        var inputFile = Path.Combine(_testDirectory, "invalid_input.txt");
        var outputFile = Path.Combine(_testDirectory, "output.png");
        File.WriteAllLines(inputFile, new[]
        { "яблоко", "сл ива", "груша" });
        var composition = new Composition();
        var programService = composition.programService;
        
        Action act = () => programService.Run(inputFile, outputFile, "Arial", Color.Black, Color.White);

        act.Should().Throw<InvalidDataException>();
    }
    
    [Test]
    public void Program_InvalidFontName_UsesDefaultFont()
    {
        var inputFile = Path.Combine(_testDirectory, "input.txt");
        var outputFile = Path.Combine(_testDirectory, "output.png");
        File.WriteAllLines(inputFile, new[] { "test", "font" });
        var composition = new Composition();
        var programService = composition.programService;
        
        programService.Run(inputFile, outputFile, "NonExistentFont123", Color.Black, Color.White);
        
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

        var composition = new Composition();
        var programService = composition.programService;
        
        programService.Run(inputFile, outputFile, "Arial", Color.Black, Color.White);
        
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
        
        var composition = new Composition();
        var programService = composition.programService;
            
        programService.Run(
            inputFile: inputFile,
            outputFile: outputFile,
            fontStyle: "Arial",
            fontColor: Color.DarkBlue,
            backgroundColor: Color.AliceBlue);
            
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
}