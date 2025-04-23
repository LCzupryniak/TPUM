using System.Xml.Serialization;

namespace DataModel
{
    [XmlRoot("Item")]
    public class Product
    {
        [XmlAttribute("Id")]
        public Guid Id { get; set; } = Guid.Empty;

        [XmlElement("Name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("Price")]
        public int Price { get; set; }

        [XmlElement("MaintenanceCost")]
        public int MaintenanceCost { get; set; }

        public Product() { }

        public Product(Guid id, string name, int price, int maintenanceCost)
        {
            Id = id;
            Name = name;
            Price = price;
            MaintenanceCost = maintenanceCost;
        }
    }
}
