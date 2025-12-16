namespace TagsCloudContainer;

public interface IWordSource
{
    IEnumerable<string> GetWords();
}