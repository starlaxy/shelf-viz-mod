namespace shelf_viz_mod.Data.Models{
public class SKU
{
    public string JanCode { get; set; } // Unique identifier for the SKU
    public string Name { get; set; }    // Name of the SKU
    public string DrinkSize { get; set; } // Volume or size of the drink (e.g., 355ml, 500ml)
    public ProductSize ProductSize { get; set; } // Physical dimensions of the product
    public string ShapeType { get; set; } // Shape of the product (e.g., Can, Bottle, Box)
    public string ImageUrl { get; set; }  // URL to an image of the SKU
    public DateTime TimeStamp { get; set; } // Time stamp when registered
}

public class ProductSize
{
    public double Width { get; set; }  // Width of the product
    public double Depth { get; set; }  // Depth of the product
    public double Height { get; set; } // Height of the product
}
}