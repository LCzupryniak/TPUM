using DataModel;
using System.Xml.Serialization;

namespace DataModel;

[XmlRoot("Cart")]
public class Cart
{
    [XmlAttribute("Id")]
    public Guid Id { get; set; } = Guid.Empty;

    [XmlElement("Capacity")]
    public int Capacity { get; set; }

    [XmlArray("Items")]
    [XmlArrayItem("Item")]
    public List<Product> Items { get; set; } = new List<Product>();

    public Cart() { }

    public Cart(Guid id, int capacity)
    {
        Id = id;
        Capacity = capacity;
        Items = new List<Product>();
    }

    public Cart(Guid id, int capacity, List<Product> items)
    {
        Id = id;
        Capacity = capacity;
        Items = items;
    }
}