namespace CardGame
{
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