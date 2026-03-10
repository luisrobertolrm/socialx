namespace SocialX.Core.Paginacao;

public static class CursorHelper
{
    public static string Serializar<T>(T obj)
    {
        string json = System.Text.Json.JsonSerializer.Serialize(obj);
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
        return Convert.ToBase64String(bytes);
    }

    public static T? Desserializar<T>(string? cursor)
    {
        if (string.IsNullOrWhiteSpace(cursor))
        {
            return default;
        }

        byte[] bytes = Convert.FromBase64String(cursor);
        string json = System.Text.Encoding.UTF8.GetString(bytes);
        return System.Text.Json.JsonSerializer.Deserialize<T>(json);
    }
}
