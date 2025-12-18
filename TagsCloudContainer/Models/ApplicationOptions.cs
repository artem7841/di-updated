using System.Drawing;

namespace TagsCloudContainer;

public class ApplicationOptions
{
    public string InputFile { get; set; }
    public string OutputFile { get; set; }
    public Size Size { get; set; }
    public string Font { get; set; }
    public Color FontColor { get; set; }
    public Color BackgroundColor { get; set; }
    public string Algorithm { get; set; }
    
    public Dictionary<string, object> AlgorithmParameters { get; set; } = new();
    
    public T GetParameter<T>(string key, T defaultValue = default)
    {
        return AlgorithmParameters.ContainsKey(key) && AlgorithmParameters[key] is T value
            ? value
            : defaultValue;
    }
}