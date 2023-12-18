using System.Collections.Generic;
namespace shelf_viz_mod.Data.Models{
public class Cabinet
    {
        public int Number { get; set; }
        public List<Row> Rows { get; set; }
        public Position Position { get; set; }
        public Size Size { get; set; }
    }

    public class Row
    {
        public int Number { get; set; }
        public List<Lane> Lanes { get; set; }
        public int PositionZ { get; set; }
        public Size Size { get; set; }
    }

    public class Lane
    {
        public int Number { get; set; }
        public string JanCode { get; set; }
        public int Quantity { get; set; }
        public int PositionX { get; set; }
    }

    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
    }

    public class Size
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Depth { get; set; }
    }
}