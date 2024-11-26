using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudContainer.Tests;

[TestFixture] 
public class Tests
{
    private WordSource _wordSource;
    private WordsPreprocessor _wordsPreprocessor;
    [SetUp]
    public void Setup()
    {
        _wordSource = new WordSource();
        _wordsPreprocessor = new WordsPreprocessor();
    }


}