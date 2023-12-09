namespace AdventOfCode;

struct HandWithJoker : IComparable<HandWithJoker>
{
    private HandWithJoker(HandType type, uint bid, string cards)
    {
        Type = type;
        Bid = bid;
        Cards = cards;
    }

    public HandType Type { get; }
    public uint Bid { get; }
    public string Cards { get; }
    
    public static HandWithJoker Parse(ReadOnlySpan<char> hand, uint bid)
    {
        Span<uint> cards = stackalloc uint[13];

        foreach (var card in hand)
        {
            cards[GetCardId(card)]++;
        }

        var foundThree = false;
        var foundPair = false;

        var numberOfJokers = cards[GetCardId('J')];
        
        // Skip Jokers and sort descending
        cards = cards[..12];
        cards.Sort();
        cards.Reverse();

        foreach (var numberOfCardsOfType in cards)
        {
            var numberOfCards = numberOfCardsOfType + numberOfJokers;
            
            switch (numberOfCards)
            {
                case 5:
                    return new HandWithJoker(HandType.FiveOfKind, bid, hand.ToString());
                case 4:
                    return new HandWithJoker(HandType.FourOfKind, bid, hand.ToString());
                case 3 when foundPair:
                    return new HandWithJoker(HandType.FullHouse, bid, hand.ToString());
                case 3:
                    foundThree = true;
                    numberOfJokers = 0;
                    continue;
                case 2 when foundThree:
                    return new HandWithJoker(HandType.FullHouse, bid, hand.ToString());
                case 2 when foundPair:
                    return new HandWithJoker(HandType.TwoPair, bid, hand.ToString());
                case 2:
                    foundPair = true;
                    numberOfJokers = 0;
                    continue;
            }
        }

        if (foundThree)
        {
            return new HandWithJoker(HandType.ThreeOfKind, bid, hand.ToString());
        }

        if (foundPair)
        {
            return new HandWithJoker(HandType.OnePair, bid, hand.ToString());
        }

        return new HandWithJoker(HandType.HighCard, bid, hand.ToString());
    }

    // Weaker first, Stronger second
    public int CompareTo(HandWithJoker other)
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
        'T' => 3,
        '9' => 4,
        '8' => 5,
        '7' => 6,
        '6' => 7,
        '5' => 8,
        '4' => 9,
        '3' => 10,
        '2' => 11,
        'J' => 12,
        _ => throw new Exception("Invalid Card!")
    };
}