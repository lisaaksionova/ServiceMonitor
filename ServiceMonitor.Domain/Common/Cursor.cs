using System.Text;
using System.Text.Json;

namespace ServiceMonitor.Domain.Common;

public sealed record Cursor(int LastId)
{
    public static string Encode(int lastId)
    {
        var json = JsonSerializer.Serialize(new Cursor(lastId));

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
        catch (FormatException)
        {
            return null;
        }
        catch (JsonException)
        {
            return null;
        }
    }
}
