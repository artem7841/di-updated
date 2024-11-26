namespace TagsCloudContainer;

public interface IWordFilter
{
    IEnumerable<string> Filter(IEnumerable<string> words);
}