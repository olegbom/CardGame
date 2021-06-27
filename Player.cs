using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardGame
{
    class Player
    {
        public int Index { get; init; }
        public Player LeftNeighbor { get; set; } 
        public Player RightNeighbor { get; set; }
        public List<Card> Cards { get; } = new List<Card>();

        public List<Card> Base { get; } = new List<Card>();





        public int Score { get; set; }

        private List<Card> _newCards = new List<Card>();

        private void TakeCards(IEnumerable<Card> newCards)
        {
            _newCards.AddRange(newCards);
        }

        public void SelectCard(int index)
        {
            if (index >= Cards.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            Base.Add(Cards[index]);
            Cards.RemoveAt(index);
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
                Base.AddRange(Cards);
                Cards.Clear();
            }
        }

        public void Scoring()
        {
            int leftCard = Base.Count(c => c.Direction == TransmissionDirection.Left);
            Score = Math.Min(Base.Count - leftCard, leftCard);
        }
        
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder($"Player {Index} Score {Score} ");
            sb.AppendJoin(" ", Cards);
            return sb.ToString();
        }
    }
}