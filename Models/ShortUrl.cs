namespace EncurtadorDeUrl.Models;

public class ShortUrl
{
    public Guid Id { get; set; }
    public string Url { get; set; }
    public string Chunck { get; set; }
}