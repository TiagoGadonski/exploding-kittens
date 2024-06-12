namespace BlazorApp1.Models
{
    public class Player
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Card> Hand { get; set; }

        public Player(string id, string name)
        {
            Id = id;
            Name = name;
            Hand = new List<Card>();
        }

        public void AddCard(Card card)
        {
            Hand.Add(card);
        }

        public void RemoveCard(Card card)
        {
            Hand.Remove(card);
        }
    }
}
