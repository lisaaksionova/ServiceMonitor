using System.Text;
using System.Text.Json;

namespace ServiceMonitor.Domain.Common;

public sealed record Cursor(int LastId, DateTime CreatedAt)
{
    public static string Encode(int lastId, DateTime createdAt)
    {
        var json = JsonSerializer.Serialize(new Cursor(lastId, createdAt));

        return Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
    }

    public static Cursor? Decode(string? cursor)
    {
        if (string.IsNullOrWhiteSpace(cursor))
        {
            return null;
        }

        try
        {
            var json = Encoding.UTF8.GetString(
                Convert.FromBase64String(cursor));

            return JsonSerializer.Deserialize<Cursor>(json);
        }
        catch (FormatException formatException)
        {
            throw new FormatException("Invalid cursor string", formatException);
        }
        catch (JsonException jsonException)
        {
            throw new JsonException("Invalid cursor string", jsonException);
        }
    }
}
