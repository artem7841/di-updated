using System.Drawing;

namespace TagsCloudContainer;

public interface IColorScheme
{
    Color GetColorForWord(string word, int fontSize, int index, int totalWords);
}