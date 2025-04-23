using System.Xml.Serialization;

namespace DataModel;

[XmlRoot("Customer")] 
public class Customer 
{
    [XmlAttribute("Id")] 
    public Guid Id { get; set; } = Guid.Empty; 

    [XmlElement("Name")]
    public string Name { get; set; } = string.Empty;

    [XmlElement("Money")]
    public float Money { get; set; }

    [XmlElement("Cart")]
    public Cart Cart { get; set; } = new Cart();

    public Customer() { }

    public Customer(Guid id, string name, float money, Cart cart)
    {
        Id = id;
        Name = name;
        Money = money;
        Cart = cart;
    }
}