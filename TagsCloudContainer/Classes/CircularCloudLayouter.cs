using System.Drawing;
using TagsCloudContainer;
using TagsCloudContainer.Classes;

namespace TagsCloudVisualization;

public class CircularCloudLayouter : ICloudLayouter
{
    private readonly Point _center;
    private List<Rectangle> _rectangles;
    private int _aCoef;
    private int _startAngle;
    
    public CircularCloudLayouter(Size targetSize, int aCoef, int startAngle)
    {
        _center = new Point(targetSize.Width / 2, targetSize.Height / 2);
        _rectangles = new List<Rectangle>();
        _aCoef = aCoef;
        _startAngle = startAngle;
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

        while (IsIntersectingWithOtherRectangles(rectangle))
        {
            _startAngle++;
            var nextPointOnSpiral = GetNextPointOnSpiral(_startAngle);
            var newRecPosition = GetLeftTopCornerFromCenter(nextPointOnSpiral, rectangleSize);
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
        
        MoveCoordinate(() => rectangle.GetCenter().X, _center.X, 
            value => rectangle.X = value,
            () => rectangle.X,
            value => rectangle.X = value,
            () => IsIntersectingWithOtherRectangles(rectangle));
        
        MoveCoordinate(() => rectangle.GetCenter().Y, _center.Y,
            value => rectangle.Y = value,
            () => rectangle.Y,
            value => rectangle.Y = value,
            () => IsIntersectingWithOtherRectangles(rectangle));
        
        return rectangle;
    }

    private void MoveCoordinate(
        Func<int> getCurrent,
        int target,
        Action<int> update,
        Func<int> getRaw,
        Action<int> setRaw,
        Func<bool> collisionCheck)
    {
        var prev = getRaw();
        while (getCurrent() != target)
        {
            prev = getRaw();
            var direction = CreateDirection(target, getCurrent());
            update(direction.GetNextValue(getRaw()));
            
            if (collisionCheck())
            {
                setRaw(prev);
                break;
            }
        }
    }

    private IDirection CreateDirection(int target, int current)
    {
        return target > current ? new PositiveDirection() : new NegativeDirection();
    }

    private Point GetNextPointOnSpiral(double angle)
    {
        var angleInRadians = Math.PI / 180 * angle;
        var cosine = Math.Cos(angleInRadians);
        var sine = Math.Sin(angleInRadians);

        var x = (int)(_aCoef * angleInRadians * cosine + _center.X);
        var y = (int)(_aCoef * angleInRadians * sine + _center.Y);
        
        return new Point(x, y);
    }
}


