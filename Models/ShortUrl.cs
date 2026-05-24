namespace EncurtadorDeUrl.Models;

public class ShortUrl
{
    public Guid Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string Chunck { get; set; } = string.Empty;
}
