using System.Collections.Generic;
namespace shelf_viz_mod.Data.Models{
public class Shelf
{
    public Cabinet Cabinet { get; set; } // Cabinet information
    public string Number { get; set; }   // Unique identifier for the shelf
    public List<Row> Rows { get; set; }  // Rows in the shelf
    public Position Position { get; set; } // Origin position based on a coordinate system
    public ProductSize Size { get; set; }  // Size of the shelf
}

public class Cabinet
{
    public string Number { get; set; } // Unique identifier for the cabinet
    // Add additional properties as needed
}

public class Row
{
    public string Number { get; set; } // Unique identifier for the row
    public List<Lane> Lanes { get; set; } // Lanes in the row
    public double PositionZ { get; set; } // Z-coordinate of the row
    public double Height { get; set; }    // Height of the row
}

public class Lane
{
    public string Number { get; set; }   // Unique identifier for the lane
    public List<string> JanCodes { get; set; } // JanCodes of SKUs in the lane
    public int Quantity { get; set; }    // Number of SKUs that can be placed in the lane
    public double PositionX { get; set; } // X-coordinate of the lane
}

public class Position
{
    public double X { get; set; } // X-coordinate
    public double Y { get; set; } // Y-coordinate
    public double Z { get; set; } // Z-coordinate
}
}