using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardGame
{
    class Program
    {

        static void Main(string[] args)
        {
            const int numberOfPlayers = 4;
            const int numberOfCards = numberOfPlayers * 7;
          
            List<Card> deck = new List<Card>();
            for (int i = 0; i < numberOfCards; i++)
            {
                deck.Add(new Card()
                {
                    Index = i,
                    Direction = (i % 2 == 1)
                        ? TransmissionDirection.Left
                        : TransmissionDirection.Right
                });
            }

            Player[] players = new Player[numberOfPlayers];
            for (int i = 0; i < numberOfPlayers; i++)
                players[i] = new Player(){Index = i};
            for (int i = 0; i < numberOfPlayers; i++)
            {
                players[i].LeftNeighbor = players[(numberOfPlayers + i - 1 )% numberOfPlayers];
                players[i].RightNeighbor = players[(i + 1) % numberOfPlayers];
            }

            Random r = new Random();
            while (deck.Any())
            {
                for (int i = 0; i < numberOfPlayers; i++)
                {
                    int index = r.Next(deck.Count);
                    players[i].Cards.Add(deck[index]);
                    deck.RemoveAt(index);
                }
            }


            do
            {
                foreach (var p in players)
                {
                    Console.WriteLine(p);
                }

                foreach (var p in players)
                {
                    if(p.Cards.Count == 0) continue;
                    
                    int index = r.Next(p.Cards.Count);
                    Console.WriteLine($"Player {p.Index}: {p.Cards[index]}");
                    p.Cards.RemoveAt(index);
                }

                foreach (var p in players) p.GiveAwayCards();
                foreach (var p in players) p.CompleteExchange();

            } while (players.Any(p => p.Cards.Count > 0));



            Console.WriteLine("Hello World!");
        }
    }


    class Player
    {
        public int Index { get; init; }
        public Player LeftNeighbor { get; set; } 
        public Player RightNeighbor { get; set; }
        public List<Card> Cards { get; } = new List<Card>();

        public int Score { get; set; }

        private List<Card> _newCards = new List<Card>();

        private void TakeCards(IEnumerable<Card> newCards)
        {
            _newCards.AddRange(newCards);
        }

        public void GiveAwayCards()
        {
            LeftNeighbor.TakeCards(Cards.Where(c => c.Direction == TransmissionDirection.Left));
            RightNeighbor.TakeCards(Cards.Where(c => c.Direction == TransmissionDirection.Right));
        }

        public void CompleteExchange()
        {
            Cards.Clear(); 
            Cards.AddRange(_newCards);
            _newCards.Clear();

            if (Cards.Count == 2 && Cards.Count(c => c.Direction == TransmissionDirection.Left) == 1)
            {
                Score++;
                Cards.Clear();
            }
        
        }
        
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder($"Player {Index} Score {Score} ");
            sb.AppendJoin(" ", Cards);
            return sb.ToString();
        }
    }


    enum TransmissionDirection
    {
        Left,
        Right
    }

    class Card
    {
        public TransmissionDirection Direction { get; init; }

        public int Index { get; init; }

        public override string ToString()
        {
            return $"{Direction,-5} {Index,2}";
        }
    }


}
