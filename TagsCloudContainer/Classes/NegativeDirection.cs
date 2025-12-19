using System.Collections;

namespace TagsCloudContainer.Classes;

public class NegativeDirection : IDirection
{
    public int GetNextValue(int current) => current - 1;
}