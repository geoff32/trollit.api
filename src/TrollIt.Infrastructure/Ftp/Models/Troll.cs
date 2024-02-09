namespace TrollIt.Infrastructure.Ftp.Models;

internal record Troll(int Id, string Name, string Breed, int Level, int? GuildId, string Avatar)
{
    public Troll(string row)
        : this(row.Split(";"))
    {

    }
    public Troll(string[] columns)
        : this(int.Parse(columns[0]), columns[1], columns[2], int.Parse(columns[3]), int.TryParse(columns[7], out var guildId) ? guildId : null, GetAvatar(columns))
    {
    }

    private static string GetAvatar(string[] columns)
    {
        return string.IsNullOrEmpty(columns[14])
            ? "https://blason.mountyhall.com/Blason_PJ/MyNameIsNobody.gif"
            : columns[14].Replace("http://www.mountyhall.com/images/Blasons/", "https://blason.mountyhall.com/");
    }
}
