using System.Collections;
using System.Drawing;

namespace TagsCloudContainer;

public interface IImageDrawer
{
    void GenerateImage(ImageDrawerOptions options);
}