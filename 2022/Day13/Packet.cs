using System.Text;

namespace AdventOfCode;

public record Packet : PacketList
{
    private Packet(IEnumerable<PacketComponent> content) : base(content)
    {
        
    }

    public static Packet Parse(string input)
    {
        var pos = 0;
        return new Packet(PacketList.Parse(input, ref pos).Content);
    }

    public bool IsDividerKey()
    {
        var str = ToString();
        return str is "[[6]]" or "[[2]]";
    }
}

public abstract record PacketComponent
    : IComparable<PacketComponent>, IComparable<PacketInteger>, IComparable<PacketList>
{
    public int CompareTo(PacketComponent? other)
    {
        return other switch
        {
            PacketInteger integer => CompareTo(integer),
            PacketList list => CompareTo(list),
            _ => throw new InvalidOperationException()
        };
    }

    public abstract int CompareTo(PacketInteger? other);

    public abstract int CompareTo(PacketList? other);
}

public record PacketInteger(int Content) : PacketComponent
{
    public static PacketInteger Parse(string input, ref int pos)
    {
        var builder = new StringBuilder();
        while (pos < input.Length)
        {
            if (char.IsDigit(input[pos]))
            {
                builder.Append(input[pos]);
                pos++;
            }
            else
            {
                return new PacketInteger(int.Parse(builder.ToString()));
            }
        }

        throw new InvalidOperationException();
    }
    
    public override int CompareTo(PacketInteger? other)
    {
        if (other is null)
        {
            throw new ArgumentNullException(nameof(other));
        }
        
        return Content.CompareTo(other.Content);
    }

    public override int CompareTo(PacketList? other)
    {
        if (other is null)
        {
            throw new ArgumentNullException(nameof(other));
        }
        
        return -1 * other.CompareTo(this);
    }

    public sealed override string ToString()
    {
        return Content.ToString();
    }
}

public record PacketList : PacketComponent
{
    public PacketList(IEnumerable<PacketComponent> content)
    {
        _content = content.ToArray();
    }

    private readonly PacketComponent[] _content;
    public IEnumerable<PacketComponent> Content => _content.AsReadOnly();

    protected static PacketList Parse(string input, ref int pos)
    {
        pos++; //skip over the first '['
        var contents = new List<PacketComponent>();
        
        while (pos < input.Length)
        {
            if (input[pos] == '[')
            {
                contents.Add(Parse(input, ref pos));
                pos++; //move over the last ']'
            }else if (input[pos] == ']')
            {
                break;
            }else if (char.IsDigit(input[pos]))
            {
                contents.Add(PacketInteger.Parse(input, ref pos));
            }else if (input[pos] == ',')
            {
                pos++;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        return new PacketList(contents);
    }

    public override int CompareTo(PacketInteger? other)
    {
        if (other is null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        return CompareTo(new PacketList(new[] { other }));
    }

    public override int CompareTo(PacketList? other)
    {
        if (other is null)
        {
            throw new ArgumentNullException(nameof(other));
        }
        
        for (var i = 0; i < Math.Max(_content.Length, other._content.Length); i++)
        {
            if (i >= _content.Length)
            {
                return -1;
            }

            if (i >= other._content.Length)
            {
                return 1;
            }
            
            var diff = _content[i].CompareTo(other._content[i]);
            
            if (diff != 0)
            {
                return diff;
            }
        }

        return 0;
    }

    public sealed override string ToString()
    {
        return $"[{string.Join(',', Content.Select(x => x.ToString()))}]";
    }
}