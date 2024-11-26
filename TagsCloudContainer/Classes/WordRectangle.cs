using System.Drawing;

namespace TagsCloudContainer;

public class WordRectangle
{
    public Rectangle Rectangle { get; set; }
    public string Word { get; set; }
    public int FontSize { get; set; }
    public WordRectangle(Rectangle rectangle, string word, int fontSize)
    {
        this.Rectangle = rectangle;
        this.Word = word;
        this.FontSize = fontSize;
    }
}