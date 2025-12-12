using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudContainer.Tests;

[TestFixture] 
public class Tests
{
    private WordSource _wordSource;
    private string _testDirectory;
    [SetUp]
    public void Setup()
    {
        _wordSource = new WordSource();
        _testDirectory = Path.Combine(Path.GetTempPath(), $"WordSourceTests_{Guid.NewGuid()}");
        Directory.CreateDirectory(_testDirectory);
    }

    [Test]
    public void GetWords_CorrectFile_ShouldReadWordsFromFile()
    {
        var testFile = Path.Combine(_testDirectory, "with_spaces.txt");
        File.WriteAllLines(testFile, new[] { "яблоко", "слива", "вишня" });
        
        var words = _wordSource.GetWords(testFile).ToList();

        words.Should().HaveCount(3);
        words.Should().Contain("яблоко");
        words.Should().Contain("слива");
        words.Should().Contain("вишня");
    }
    
    [Test]
    public void GetWords_FileWithSpaces_ThrowsInvalidDataException()
    {
        var testFile = Path.Combine(_testDirectory, "with_spaces.txt");
        File.WriteAllLines(testFile, new[] { "яблоко", "сли ва", "вишня" });
        
        Action act = () => _wordSource.GetWords(testFile).ToList();

        act.Should().Throw<InvalidDataException>();
    }
    
    [Test]
    public void GetWords_FileWithEmptyLines_ThrowsInvalidDataException()
    {
        var testFile = Path.Combine(_testDirectory, "with_spaces.txt");
        File.WriteAllLines(testFile, new[] { "яблоко", "", "вишня" });
        
        Action act = () => _wordSource.GetWords(testFile).ToList();

        act.Should().Throw<InvalidDataException>();
    }
    

}