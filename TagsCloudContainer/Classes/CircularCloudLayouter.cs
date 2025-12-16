using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudContainer;

namespace TagsCloudVisualization;

sealed class CircularCloudLayouter : ICloudLayouter
{
    private readonly Point _center;
    private List<Rectangle> _rectangles;
    private int aCoef = 30;
    
    public CircularCloudLayouter(Point center)
    {
        _center = center;
        _rectangles = new List<Rectangle>();
    }
    
    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
        {
            throw new ArgumentOutOfRangeException("rectangleSize is <= 0");
        }
        
        var leftTopCorner = GetLeftTopCornerFromCenter(_center, rectangleSize);
        var rectangle = new Rectangle(leftTopCorner, rectangleSize);
        
        if (_rectangles.Count == 0)
        {
            _rectangles.Add(rectangle);
            return rectangle;
        }
        
        var angle = 180;
        while (IsIntersectingWithOtherRectangles(rectangle))
        {
            angle++;
            var nextPointOnSpiral = GetNextPointOnSpiral(angle);
            var newRecPosition = GetLeftTopCornerFromCenter(nextPointOnSpiral,  rectangleSize);
            rectangle.X = newRecPosition.X;
            rectangle.Y = newRecPosition.Y;
        }

        var result = PushRectangleToCenter(rectangle);
        _rectangles.Add(result);
        return result;
    }

    private Point GetLeftTopCornerFromCenter(Point center, Size size)
    {
        var x = center.X - size.Width / 2;
        var y = center.Y - size.Height / 2;
        var result = new Point(x, y);
        return result;
    }

    private bool IsIntersectingWithOtherRectangles(Rectangle rectangle)
    {
        foreach (Rectangle otherRectangle in _rectangles)
        {
            if (otherRectangle.IntersectsWith(rectangle))
            {
                return true;
            }
        }
        return false;
    }

    private Rectangle PushRectangleToCenter(Rectangle rec)
    {
        var rectangle = rec;
        var prevX = rectangle.X;
        var prevY = rectangle.Y;
        while (rectangle.GetCenter().X != _center.X)
        {
            prevX = rectangle.X;
            if (_center.X - rectangle.GetCenter().X > 0)
            {
                rectangle.X++;
            }
            else
            {
                rectangle.X--;
            }
            
            if (IsIntersectingWithOtherRectangles(rectangle))
            {
                rectangle.X = prevX;
                break;
            }
        }
        while (rectangle.GetCenter().Y != _center.Y)
        {
            prevY = rectangle.Y;
            if (_center.Y - rectangle.GetCenter().Y > 0)
            {
                rectangle.Y++;
            }
            else
            {
                rectangle.Y--;
            }
            
            if (IsIntersectingWithOtherRectangles(rectangle))
            {
                rectangle.Y = prevY;
                break;
            }
        }
        return rectangle;
    }

    private Point GetNextPointOnSpiral(double angle)
    {
        var angleInRadians = Math.PI / 180 * angle;
        var cosine = Math.Cos(angleInRadians);
        var sine = Math.Sin(angleInRadians);

        var x = (int)(aCoef * angleInRadians * cosine + _center.X);
        var y = (int)(aCoef * angleInRadians * sine +  _center.Y);
        
        return new Point(x, y);
    }
}