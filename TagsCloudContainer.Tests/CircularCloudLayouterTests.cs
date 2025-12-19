using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using TagsCloudVisualization;

namespace TagsCloudContainer.Tests;

[TestFixture]
public class CircularCloudLayouterTests
{
    private Size _targetSize;
    private ICloudLayouter _layouter;
    private Point _center;
    private Random _random;
    
    [SetUp]
    public void Setup()
    {
        _targetSize = new Size(1920, 1080);
        _center = new Point(_targetSize.Width / 2, _targetSize.Height / 2);
        _layouter = new CircularCloudLayouter(_targetSize, aCoef: 5, startAngle: 0);
        _random = new Random(10); 
    }
    
    [Test]
    public void PutNextRectangle_WithInvalidSize_ShouldThrowException()
    {
        var invalidSizes = new[]
        {
            new Size(-30, 10),
            new Size(30, -10),
            new Size(0, 10),
            new Size(10, 0),
            new Size(0, 0),
            new Size(-1, -1)
        };
        
        foreach (var size in invalidSizes)
        {
            Action act = () => _layouter.PutNextRectangle(size);
            act.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
    
    [Test]
    public void PutNextRectangle_FirstRectangle_ShouldBeCentered()
    {
        var rectangleSize = new Size(100, 50);
        
        var rectangle = _layouter.PutNextRectangle(rectangleSize);
        
        var rectangleCenter = GetRectangleCenter(rectangle);
        rectangleCenter.X.Should().Be(_center.X);
        rectangleCenter.Y.Should().Be(_center.Y);
        rectangle.Size.Should().Be(rectangleSize);
    }
    
    [Test]
    public void PutNextRectangle_MultipleRectangles_ShouldNotIntersect()
    {
        var rectangles = new List<Rectangle>();
        int rectangleCount = 50;
        
        for (int i = 0; i < rectangleCount; i++)
        {
            var size = new Size(_random.Next(20, 100), _random.Next(15, 60));
            var rectangle = _layouter.PutNextRectangle(size);
            rectangles.Add(rectangle);
        }
        
        for (int i = 0; i < rectangles.Count; i++)
        {
            for (int j = i + 1; j < rectangles.Count; j++)
            {
                rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse(
                    $"Rectangles {i} ({rectangles[i]}) and {j} ({rectangles[j]}) intersected");
            }
        }
    }
    
    [Test]
    public void PutNextRectangle_Rectangles_ShouldBeWithinBounds()
    {
        int rectangleCount = 30;
        var rectangles = new List<Rectangle>();
        
        for (int i = 0; i < rectangleCount; i++)
        {
            var size = new Size(_random.Next(30, 80), _random.Next(20, 50));
            rectangles.Add(_layouter.PutNextRectangle(size));
        }
        
        foreach (var rectangle in rectangles)
        {
            rectangle.Left.Should().BeGreaterThanOrEqualTo(0, $"Rectangle {rectangle} extends beyond the left border");
            rectangle.Top.Should().BeGreaterThanOrEqualTo(0, $"Rectangle {rectangle} extends beyond the top border");
            rectangle.Right.Should().BeLessThanOrEqualTo(_targetSize.Width, $"Rectangle {rectangle} extends beyond the right border");
            rectangle.Bottom.Should().BeLessThanOrEqualTo(_targetSize.Height, $"Rectangle {rectangle} extends beyond the bottom border");
        }
    }
    
    [Test]
    public void PutNextRectangle_WithSameSizes_ShouldCreateCompactLayout()
    {
        int rectangleCount = 20;
        var size = new Size(60, 40);
        var rectangles = new List<Rectangle>();
        
        for (int i = 0; i < rectangleCount; i++)
        {
            rectangles.Add(_layouter.PutNextRectangle(size));
        }
        
        var totalArea = rectangles.Sum(r => r.Width * r.Height);
        var boundingCircle = GetBoundingCircle(rectangles, _center);
        var circleArea = Math.PI * boundingCircle * boundingCircle;
        
        var packingDensity = totalArea / circleArea;
        packingDensity.Should().BeGreaterThan(0.5, "The packing density is small");
    }
    
    [Test]
    public void PutNextRectangle_ShouldHandleManyRectangles()
    {
        int rectangleCount = 200;
        
        for (int i = 0; i < rectangleCount; i++)
        {
            var size = new Size(_random.Next(10, 40), _random.Next(8, 25));
            var rectangle = _layouter.PutNextRectangle(size);
            
            rectangle.Left.Should().BeGreaterThanOrEqualTo(0);
            rectangle.Top.Should().BeGreaterThanOrEqualTo(0);
        }
    }
    
    [Test]
    public void PutNextRectangle_WithDifferentParameters_ShouldWork()
    {
        var testCases = new[]
        {
            new { Size = _targetSize, ACoef = 2, StartAngle = 0 },
            new { Size = _targetSize, ACoef = 10, StartAngle = 0 },
            new { Size = _targetSize, ACoef = 5, StartAngle = 90 },
            new { Size = new Size(800, 600), ACoef = 3, StartAngle = 45 }
        };
        
        foreach (var testCase in testCases)
        {
            ICloudLayouter layouter = new CircularCloudLayouter(testCase.Size, testCase.ACoef, testCase.StartAngle);
            
            var rectangles = new List<Rectangle>();
            for (int i = 0; i < 10; i++)
            {
                rectangles.Add(layouter.PutNextRectangle(new Size(50, 30)));
            }
            
            for (int i = 0; i < rectangles.Count; i++)
            {
                for (int j = i + 1; j < rectangles.Count; j++)
                {
                    rectangles[i].IntersectsWith(rectangles[j]).Should().BeFalse();
                }
            }
        }
    }
    
    [Test]
    public void PutNextRectangle_RectanglesShouldTouchAtLeastOneOther()
    {
        int rectangleCount = 30;
        var rectangles = new List<Rectangle>();
        
        for (int i = 0; i < rectangleCount; i++)
        {
            rectangles.Add(_layouter.PutNextRectangle(new Size(40, 25)));
        }
        
        for (int i = 0; i < rectangles.Count; i++)
        {
            var touchesOther = false;
            for (int j = 0; j < rectangles.Count; j++)
            {
                if (i != j && RectanglesTouch(rectangles[i], rectangles[j]))
                {
                    touchesOther = true;
                    break;
                }
            }
            
            if (i > 0)
            {
                touchesOther.Should().BeTrue($"Rectangle {i} does not touch others");
            }
        }
    }
    
    [Test]
    public void CircularCloudLayouter_ShouldHandleExtremeSizeRatios()
    {
        var targetSize = new Size(1000, 1000);
        var layouter = new CircularCloudLayouter(targetSize, 5, 0);
        
        var wideRect = layouter.PutNextRectangle(new Size(800, 10));
        var tallRect = layouter.PutNextRectangle(new Size(10, 800));
    
        wideRect.IntersectsWith(tallRect).Should().BeFalse();
        wideRect.Right.Should().BeLessThanOrEqualTo(targetSize.Width);
        tallRect.Bottom.Should().BeLessThanOrEqualTo(targetSize.Height);
    }
    
    private Point GetRectangleCenter(Rectangle rectangle)
    {
        return new Point(
            rectangle.X + rectangle.Width / 2,
            rectangle.Y + rectangle.Height / 2);
    }
    
    private double GetBoundingCircle(List<Rectangle> rectangles, Point center)
    {
        double maxDistance = 0;
        
        foreach (var rectangle in rectangles)
        {
            var corners = new[]
            {
                new Point(rectangle.Left, rectangle.Top),
                new Point(rectangle.Right, rectangle.Top),
                new Point(rectangle.Left, rectangle.Bottom),
                new Point(rectangle.Right, rectangle.Bottom)
            };
            
            foreach (var corner in corners)
            {
                var distance = GetDistance(center, corner);
                maxDistance = Math.Max(maxDistance, distance);
            }
        }
        
        return maxDistance;
    }
    
    private double GetDistance(Point p1, Point p2)
    {
        var dx = p1.X - p2.X;
        var dy = p1.Y - p2.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
    
    private bool RectanglesTouch(Rectangle r1, Rectangle r2)
    {
        return (r1.Right == r2.Left || r2.Right == r1.Left) && 
               (r1.Bottom >= r2.Top && r1.Top <= r2.Bottom) ||
               (r1.Bottom == r2.Top || r2.Bottom == r1.Top) && 
               (r1.Right >= r2.Left && r1.Left <= r2.Right);
    }
    
}