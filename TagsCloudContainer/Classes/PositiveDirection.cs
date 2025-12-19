namespace TagsCloudContainer.Classes;

public class PositiveDirection :  IDirection
{
    public int GetNextValue(int current) => current + 1;
}