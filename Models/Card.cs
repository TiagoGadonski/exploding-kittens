namespace BlazorApp1.Models
{
    public class Card
    {
        public string Name { get; set; }
        public string Type { get; set; }

        public Card(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}
