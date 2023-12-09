namespace AdventOfCode;

struct Hand : IComparable<Hand>
{
    private Hand(HandType type, uint bid, string cards)
    {
        Type = type;
        Bid = bid;
        Cards = cards;
    }

    public HandType Type { get; }
    public uint Bid { get; }
    public string Cards { get; }
    
    public static Hand Parse(ReadOnlySpan<char> hand, uint bid)
    {
        Span<uint> cards = stackalloc uint[13];

        foreach (var card in hand)
        {
            cards[GetCardId(card)]++;
        }

        var foundThree = false;
        var foundPair = false;

        foreach (var numberOfCards in cards)
        {
            switch (numberOfCards)
            {
                case 5:
                    return new Hand(HandType.FiveOfKind, bid, hand.ToString());
                case 4:
                    return new Hand(HandType.FourOfKind, bid, hand.ToString());
                case 3 when foundPair:
                    return new Hand(HandType.FullHouse, bid, hand.ToString());
                case 3:
                    foundThree = true;
                    continue;
                case 2 when foundThree:
                    return new Hand(HandType.FullHouse, bid, hand.ToString());
                case 2 when foundPair:
                    return new Hand(HandType.TwoPair, bid, hand.ToString());
                case 2:
                    foundPair = true;
                    continue;
            }
        }

        if (foundThree)
        {
            return new Hand(HandType.ThreeOfKind, bid, hand.ToString());
        }

        if (foundPair)
        {
            return new Hand(HandType.OnePair, bid, hand.ToString());
        }

        return new Hand(HandType.HighCard, bid, hand.ToString());
    }

    // Weaker first, Stronger second
    public int CompareTo(Hand other)
    {
        if (Type > other.Type)
        {
            return 1;
        }

        if (Type < other.Type)
        {
            return -1;
        }

        for (var i = 0; i < Cards.Length; i++)
        {
            if (GetCardId(Cards[i]) < GetCardId(other.Cards[i]))
            {
                return 1;
            }

            if (GetCardId(Cards[i]) > GetCardId(other.Cards[i]))
            {
                return -1;
            }
        }

        return 0;
    }
    
    private static int GetCardId(char card) => card switch
    {
        'A' => 0,
        'K' => 1,
        'Q' => 2,
        'J' => 3,
        'T' => 4,
        '9' => 5,
        '8' => 6,
        '7' => 7,
        '6' => 8,
        '5' => 9,
        '4' => 10,
        '3' => 11,
        '2' => 12,
        _ => throw new Exception("Invalid Card!")
    };
}