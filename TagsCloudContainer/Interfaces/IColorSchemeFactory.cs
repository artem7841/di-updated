using System.Drawing;

namespace TagsCloudContainer;

public interface IColorSchemeFactory
{
    IColorScheme Create(string schemeName, Color color);
}
