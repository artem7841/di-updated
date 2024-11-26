using System.Collections;
using System.Drawing;

namespace TagsCloudContainer;

public interface IImageDrawer
{
    void GenerateImage(IEnumerable<WordRectangle> rectangles, String pathToSave, string fontStyle, Color fontColor, Color BackgroundColor);
}