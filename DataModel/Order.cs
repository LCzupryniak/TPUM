using System.Xml.Serialization;

namespace DataModel;

[XmlRoot("Order")]
public class Order
{
    [XmlAttribute("Id")]
    public Guid Id { get; set; } = Guid.Empty;

    [XmlElement("Buyer")]
    public Customer Buyer { get; set; } = new Customer();

    [XmlArray("ItemsToBuy")]
    [XmlArrayItem("Item")]
    public List<Product> ItemsToBuy { get; set; } = new List<Product>(); // Użyto List<Item>, dodano public set i inicjalizację

    public Order() { }

    public Order(Guid id, Customer buyer, List<Product> itemsToBuy)
    {
        Id = id;
        Buyer = buyer;
        ItemsToBuy = itemsToBuy;
    }
}
