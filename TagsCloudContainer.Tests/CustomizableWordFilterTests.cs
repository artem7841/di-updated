using TagsCloudContainer.Classes;

namespace TagsCloudContainer.Tests;

using FluentAssertions;
using NUnit.Framework;


[TestFixture]
public class CustomizableWordFilterTests
{
    private string _tempFilePath;
    
    [SetUp]
    public void SetUp() => _tempFilePath = Path.GetTempFileName();
    
    [TearDown]
    public void TearDown()
    {
        if (File.Exists(_tempFilePath))
            File.Delete(_tempFilePath);
    }
    
    [Test]
    public void Constructor_WithValidFile_ShouldLoadAndFilterWords()
    {
        File.WriteAllLines(_tempFilePath, new[] { "bad", "worse" });
        
        var filter = new CustomizableWordFilter(_tempFilePath);
        var result = filter.Filter(new[] { "good", "bad", "worse", "best" }).ToList();
        
        result.Should().BeEquivalentTo("good", "best");
    }
    
    [Test]
    public void Constructor_WhenFileNotFound_ShouldThrowFileNotFoundException()
    {
        var missingFile = "notfound.txt";
        
        Action act = () => new CustomizableWordFilter(missingFile);
        act.Should().Throw<FileNotFoundException>().WithMessage("File with excluded words not found");
    }
    
    [Test]
    public void Constructor_WithEmptyOrNullPath_ShouldCreateEmptyFilter()
    {
        var filter1 = new CustomizableWordFilter(null);
        filter1.Filter(new[] { "any", "word" }).Should().BeEquivalentTo("any", "word");
        
        var filter2 = new CustomizableWordFilter("");
        filter2.Filter(new[] { "any", "word" }).Should().BeEquivalentTo("any", "word");
    }
    
    [Test]
    public void Constructor_ShouldIgnoreWhitespaceAndBeCaseInsensitive()
    {
        File.WriteAllLines(_tempFilePath, new[] 
        {
            "",
            "  WORLD  ",
            "Hello",
            " ",
            "\tHELP\t"
        });
        
        var filter = new CustomizableWordFilter(_tempFilePath);
        var result = filter.Filter(new[] { "world", "HELLO", "Hello", "good" }).ToList();
        
        result.Should().ContainSingle().Which.Should().Be("good");
    }
    
    [Test]
    public void Constructor_WhenDirectoryNotFound_ShouldThrowDirectoryNotFoundException()
    {
        var missingDir = "missingDir";
        var fileInMissingDir = Path.Combine(missingDir, "words.txt");
        
        Action act = () => new CustomizableWordFilter(fileInMissingDir);
        act.Should().Throw<DirectoryNotFoundException>().WithMessage("Directory for excluded words file not found");
    }
}