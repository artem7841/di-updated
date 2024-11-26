using System.Drawing;

namespace TagsCloudContainer;

public interface IFontMeasurer
{
    Size MeasureWord(string word, int fontSize);
}
