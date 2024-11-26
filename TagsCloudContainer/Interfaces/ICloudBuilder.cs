using System.Drawing;

namespace TagsCloudContainer;

public interface ICloudBuilder
{
    IEnumerable<WordRectangle> BuildCloud(IEnumerable<string> words, Size targetSize);
}