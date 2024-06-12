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
            for (int j = 0; j < 4; j++) Deck.Push(new Card("Exploding Kitten", "Exploding Kitten"));
            for (int j = 0; j < 6; j++) Deck.Push(new Card("Defuse", "Defuse"));
            for (int j = 0; j < 5; j++) Deck.Push(new Card("Attack", "Attack"));
            for (int j = 0; j < 4; j++) Deck.Push(new Card("Skip", "Skip"));
            for (int j = 0; j < 4; j++) Deck.Push(new Card("Favor", "Favor"));
            for (int j = 0; j < 4; j++) Deck.Push(new Card("Shuffle", "Shuffle"));
            for (int j = 0; j < 4; j++) Deck.Push(new Card("See the Future", "See the Future"));
            for (int j = 0; j < 4; j++) Deck.Push(new Card("Nope", "Nope"));

            // Adicionar cartas de gatos
            for (int j = 0; j < 4; j++) Deck.Push(Card.Cat1);
            for (int j = 0; j < 4; j++) Deck.Push(Card.Cat2);
            for (int j = 0; j < 4; j++) Deck.Push(Card.Cat3);
            for (int j = 0; j < 4; j++) Deck.Push(Card.Cat4);
            for (int j = 0; j < 4; j++) Deck.Push(Card.Cat5);
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
                for (int j = 0; j < 7; j++)
                {
                    player.AddCard(Deck.Pop());
                }
                // Dar uma carta "Defuse" a cada jogador
                player.AddCard(new Card("Defuse", "Defuse"));
            }

            // Adicionar cartas "Exploding Kitten" ao baralho
            for (int j = 0; j < Players.Count - 1; j++)
            {
                Deck.Push(new Card("Exploding Kitten", "Exploding Kitten"));
            }

            ShuffleDeck();

            // Selecionar um jogador inicial aleatoriamente
            var rnd = new Random();
            CurrentPlayerIndex = rnd.Next(Players.Count);
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
                case "Cat":
                    // Implementar lógica de cartas de gato
                    CheckCatPairs(player);
                    break;
            }

            // Passar para o próximo jogador
            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % Players.Count;
        }

        private void CheckCatPairs(Player player)
        {
            var catGroups = player.Hand.Where(c => c.Type == "Cat")
                                       .GroupBy(c => c.Name)
                                       .Where(g => g.Count() >= 2);

            foreach (var group in catGroups)
            {
                if (group.Count() >= 2)
                {
                    // Remove a carta do par do jogador
                    var cardToRemove = group.First();
                    player.RemoveCard(cardToRemove);
                    player.RemoveCard(cardToRemove);

                    // Roubar uma carta aleatória de outro jogador
                    var random = new Random();
                    var otherPlayers = Players.Where(p => p != player).ToList();
                    if (otherPlayers.Count > 0)
                    {
                        var targetPlayer = otherPlayers[random.Next(otherPlayers.Count)];
                        if (targetPlayer.Hand.Count > 0)
                        {
                            var stolenCard = targetPlayer.Hand[random.Next(targetPlayer.Hand.Count)];
                            player.AddCard(stolenCard);
                            targetPlayer.RemoveCard(stolenCard);
                        }
                    }
                }
            }
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
