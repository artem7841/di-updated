using System.Drawing;

namespace TagsCloudContainer;

public interface ICloudBuilder<T> where T : AlgorithmParameters
{
    IEnumerable<WordRectangle> BuildCloud(
        IEnumerable<string> words, 
        Size targetSize, 
        T parameters);
}