namespace BlazorApp1.Models
{
    public class Game
    {
        public List<Player> Players { get; private set; }
        private Stack<Card> Deck;
        private Stack<Card> DiscardPile;
        private int CurrentPlayerIndex;

        public Game(int numberOfPlayers)
        {
            Players = new List<Player>();
            Deck = new Stack<Card>();
            DiscardPile = new Stack<Card>();
            CurrentPlayerIndex = 0;

            InitializeDeck();
            ShuffleDeck();
        }

        private void InitializeDeck()
        {
            // Adicionar cartas ao baralho (exemplo básico, ajuste conforme necessário)
            for (int i = 0; i < 4; i++) Deck.Push(new Card("Exploding Kitten", "Exploding Kitten"));
            for (int i = 0; i < 6; i++) Deck.Push(new Card("Defuse", "Defuse"));
            for (int i = 0; i < 5; i++) Deck.Push(new Card("Attack", "Attack"));
            for (int i = 0; i < 4; i++) Deck.Push(new Card("Skip", "Skip"));
            for (int i = 0; i < 4; i++) Deck.Push(new Card("Favor", "Favor"));
            for (int i = 0; i < 4; i++) Deck.Push(new Card("Shuffle", "Shuffle"));
            for (int i = 0; i < 4; i++) Deck.Push(new Card("See the Future", "See the Future"));
            for (int i = 0; i < 4; i++) Deck.Push(new Card("Nope", "Nope"));
        }

        private void ShuffleDeck()
        {
            var rnd = new Random();
            Deck = new Stack<Card>(Deck.OrderBy(card => rnd.Next()));
        }

        public void AddPlayer(Player player)
        {
            Players.Add(player);
        }

        public void StartGame()
        {
            foreach (var player in Players)
            {
                // Distribuir 7 cartas para cada jogador
                for (int i = 0; i < 7; i++)
                {
                    player.AddCard(Deck.Pop());
                }
                // Dar uma carta "Defuse" a cada jogador
                player.AddCard(new Card("Defuse", "Defuse"));
            }

            // Adicionar cartas "Exploding Kitten" ao baralho
            for (int i = 0; i < Players.Count - 1; i++)
            {
                Deck.Push(new Card("Exploding Kitten", "Exploding Kitten"));
            }

            ShuffleDeck();
        }

        public Player GetCurrentPlayer()
        {
            return Players.Count > 0 ? Players[CurrentPlayerIndex] : null;
        }

        public void PlayCard(Player player, Card card)
        {
            player.RemoveCard(card);
            DiscardPile.Push(card);

            // Lógica para cada tipo de carta
            switch (card.Type)
            {
                case "Attack":
                    // Implementar lógica de ataque
                    break;
                case "Skip":
                    // Implementar lógica de pular turno
                    break;
                case "Defuse":
                    // Implementar lógica de desarmar explosão
                    break;
                case "Exploding Kitten":
                    // Implementar lógica de explosão
                    break;
                case "Favor":
                    // Implementar lógica de pedir favor
                    break;
                case "Shuffle":
                    ShuffleDeck();
                    break;
                case "See the Future":
                    // Implementar lógica de ver o futuro
                    break;
                case "Nope":
                    // Implementar lógica de cancelar ação
                    break;
            }

            // Passar para o próximo jogador
            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % Players.Count;
        }

        public void EndTurn()
        {
            var currentPlayer = GetCurrentPlayer();
            var drawnCard = Deck.Pop();

            if (drawnCard.Type == "Exploding Kitten")
            {
                // Lógica de explosão
                if (currentPlayer.Hand.Any(c => c.Type == "Defuse"))
                {
                    // Usar carta "Defuse"
                    var defuseCard = currentPlayer.Hand.First(c => c.Type == "Defuse");
                    currentPlayer.RemoveCard(defuseCard);
                    DiscardPile.Push(defuseCard);
                    // Reposicionar a carta "Exploding Kitten" no baralho
                    Deck.Push(new Card("Exploding Kitten", "Exploding Kitten"));
                    ShuffleDeck();
                }
                else
                {
                    // Jogador perde
                    Players.Remove(currentPlayer);
                }
            }
            else
            {
                currentPlayer.AddCard(drawnCard);
            }

            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % Players.Count;
        }

        public bool IsGameOver()
        {
            return Players.Count <= 1;
        }
    }
}
