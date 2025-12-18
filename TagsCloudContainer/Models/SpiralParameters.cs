namespace TagsCloudContainer;

public class SpiralParameters : AlgorithmParameters
{
    public int ACoef { get; }
    public int StartAngle { get; }

    public SpiralParameters(int aCoef, int startAngle)
    {
        ACoef = aCoef;
        StartAngle = startAngle;
    }
}