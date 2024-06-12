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

        // Tipos de gatos
        public static Card Cat1 = new Card("Tacocat", "Cat");
        public static Card Cat2 = new Card("Hairy Potato Cat", "Cat");
        public static Card Cat3 = new Card("Rainbow Ralphing Cat", "Cat");
        public static Card Cat4 = new Card("Beard Cat", "Cat");
        public static Card Cat5 = new Card("Cattermelon", "Cat");
    }
}
