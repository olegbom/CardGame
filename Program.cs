using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGame
{
    class Program
    {

        static void Main(string[] args)
        {

            const int numberOfPlayers = 4;
            const int numberOfCards = numberOfPlayers * 10;
          
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
                foreach (var p in players) Console.WriteLine(p);

                foreach (var p in players)
                {
                    if(p.Cards.Count == 0) continue;
                    
                    int index = r.Next(p.Cards.Count);
                    Console.WriteLine($"Player {p.Index}: {p.Cards[index]}");
                    p.SelectCard(index);
                }

                foreach (var p in players) p.GiveAwayCards();
                foreach (var p in players) p.CompleteExchange();

            } while (players.Any(p => p.Cards.Count > 0));

            foreach (var p in players) p.Scoring();
            foreach (var p in players) Console.WriteLine(p);


            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
