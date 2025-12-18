using System.Drawing;

namespace TagsCloudContainer;

public class ImageDrawerOptions
{
    public IEnumerable<WordRectangle> Rectangles { get; }
    public string PathToSave { get; }
    public string FontStyle { get; }
    public Color FontColor { get; }
    public Color BackgroundColor{ get; }
    public Size Size{ get; }

    public ImageDrawerOptions(IEnumerable<WordRectangle> rectangles, string pathToSave, 
        string fontStyle, Color fontColor, Color backgroundColor, Size size)
    {
        Rectangles = rectangles;
        PathToSave = pathToSave;
        FontStyle = fontStyle;
        FontColor = fontColor;
        BackgroundColor = backgroundColor;
        Size = size;
    }
}