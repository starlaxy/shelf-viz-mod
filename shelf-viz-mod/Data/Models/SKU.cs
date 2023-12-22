public class SKU
{
    public string JanCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public string ImageURL { get; set; } = string.Empty;
    public int Size { get; set; }
    public long TimeStamp { get; set; }
    public string Shape { get; set; }
}


// public enum SKUShape
// {
//     Bottle,
//     Can
// }