using System.Drawing;

namespace TagsCloudContainer;

public record WordRectangle
{
    public Rectangle Rectangle { get;  }
    public string Word { get;  }
    public int FontSize { get;  }
    public WordRectangle(Rectangle rectangle, string word, int fontSize)
    {
        this.Rectangle = rectangle;
        this.Word = word;
        this.FontSize = fontSize;
    }
}