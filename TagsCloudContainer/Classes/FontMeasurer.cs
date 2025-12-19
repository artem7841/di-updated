using System.Drawing;
using System.Drawing.Text;

namespace TagsCloudContainer;

public class FontMeasurer : IFontMeasurer
{
    private readonly string _fontFamily;
    
    public FontMeasurer(string fontFamily = "Arial")
    {
        _fontFamily = fontFamily;
    }
    
    public Size MeasureWord(string word, int fontSize)
    {
        using var bitmap = new Bitmap(1000, 1000);
        using var graphics = Graphics.FromImage(bitmap);

        graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
        graphics.PageUnit = GraphicsUnit.Pixel;
        
        using var font = new Font(_fontFamily, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
        
        var stringFormat = new StringFormat(StringFormat.GenericTypographic)
        {
            FormatFlags = StringFormatFlags.NoClip,
        };
        
        var size = graphics.MeasureString(word, font, PointF.Empty, stringFormat);

        var padding = fontSize / 4;  
        var widthWithPadding = (int)Math.Ceiling(size.Width) + (padding * 2);
        var height = (int)Math.Ceiling(size.Height) ;
    
        return new Size(widthWithPadding, height);
    }
}