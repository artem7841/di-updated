namespace TagsCloudContainer.Extensions;

public static class AlgorithmParametersExtensions
{
    public static SpiralParameters ToSpiralParameters(this Dictionary<string, object> dict)
    {
        return new SpiralParameters(
            dict.GetValue("SpiralCoefficient", 3),
            dict.GetValue("StartAngle", 0)
        );
    }
    
    private static T GetValue<T>(this Dictionary<string, object> dict, string key, T defaultValue)
    {
        return dict.ContainsKey(key) && dict[key] is T value ? value : defaultValue;
    }
}