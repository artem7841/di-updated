using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudContainer;

namespace TagsCloudVisualization;

public class RectanglesDrawer : IImageDrawer
{
    private Point _center;
    
    public RectanglesDrawer(Point center)
    {
        this._center = center;
    }
    
    public void GenerateImage(IEnumerable<WordRectangle> rectangles, string pathToSave, string fontStyle, Color fontColor, Color BackgroundColor)
    {
        var rectanglesList = rectangles.ToList();
        if (string.IsNullOrWhiteSpace(pathToSave))
        {
            throw new ArgumentNullException("pathToSave can not be null or empty" );
        }

        if (rectanglesList.Count == 0)
        {
            throw new InvalidOperationException("rectangles can not be empty" );
        }
        
        var imageSize = GetSizeForImage(rectanglesList);
        
        using var bitmap = new Bitmap(imageSize.Width, imageSize.Height);
        using var graphics = Graphics.FromImage(bitmap);
        
        graphics.Clear(BackgroundColor);
        
        
        var offsetX = imageSize.Width / 2 - _center.X;
        var offsetY = imageSize.Height / 2 - _center.Y;
        
        for (int i = 0; i < rectanglesList.Count; i++)
        {
            var offsetRectangle = rectanglesList[i].Rectangle;
            offsetRectangle.Offset(offsetX, offsetY);
            
            string word = rectanglesList[i].Word; 
            int fontSize = rectanglesList[i].FontSize;
            
            using var font = new Font(fontStyle, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            using var textBrush = new SolidBrush(fontColor);
            
            SizeF textSize = graphics.MeasureString(word, font);
            
            float textX = offsetRectangle.X + (offsetRectangle.Width - textSize.Width) / 2;
            float textY = offsetRectangle.Y + (offsetRectangle.Height - textSize.Height) / 2;
            
            graphics.DrawString(word, font, textBrush, textX, textY);
        }
        bitmap.Save(pathToSave, ImageFormat.Png);
    }
    

    
    private Size GetSizeForImage(List<WordRectangle> rectangles)
    {
        var maxHeight = 0;
        var maxWidth = 0;
        foreach (var rectangle in rectangles)
        {
            var maxDistYFromCenter = Math.Max(Math.Abs(_center.Y - rectangle.Rectangle.Top), Math.Abs(_center.Y - rectangle.Rectangle.Bottom));
            var maxDistXFromCenter = Math.Max(Math.Abs(_center.X - rectangle.Rectangle.Left), Math.Abs(_center.X - rectangle.Rectangle.Right));
            
            maxHeight = Math.Max(maxHeight, maxDistYFromCenter);
            maxWidth = Math.Max(maxWidth, maxDistXFromCenter);
        }
        return new Size(maxWidth*2, maxHeight*2);
    }
}