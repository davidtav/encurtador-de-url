using System.Net;
using Carter;
using Carter.ModelBinding;
using EncurtadorDeUrl.Models;
using LiteDB;

namespace EncurtadorDeUrl.CarterModules;

public class UrlsModule : CarterModule
{
    public UrlsModule(ILiteDatabase db, IConfiguration config) : base("/urls")
    {
        Post("/", async (req, res) =>
        {
            var shortUrl = await req.Bind<ShortUrl>();

            if (string.IsNullOrWhiteSpace(shortUrl.Url))
            {
                res.StatusCode = (int)HttpStatusCode.BadRequest;
                await res.WriteAsJsonAsync(new
                {
                    error = new { code = "INVALID_URL", message = "A URL informada é obrigatória." }
                });
                return;
            }

            if (!Uri.TryCreate(shortUrl.Url, UriKind.Absolute, out var uriParsed) ||
                (uriParsed.Scheme != Uri.UriSchemeHttp && uriParsed.Scheme != Uri.UriSchemeHttps))
            {
                res.StatusCode = (int)HttpStatusCode.BadRequest;
                await res.WriteAsJsonAsync(new
                {
                    error = new { code = "INVALID_URL", message = "Informe uma URL válida com http:// ou https://." }
                });
                return;
            }

            shortUrl.Url = uriParsed.ToString();
            shortUrl.Chunck = Nanoid.Nanoid.Generate(size: 9);
            db.GetCollection<ShortUrl>(BsonAutoId.Guid).Insert(shortUrl);

            var baseUrl = config["BaseUrl"] ?? $"{req.Scheme}://{req.Host}";
            var rawShortUrl = $"{baseUrl}/{shortUrl.Chunck}";

            res.StatusCode = (int)HttpStatusCode.Created;
            res.Headers.Location = rawShortUrl;
            await res.WriteAsJsonAsync(new
            {
                shortUrl = rawShortUrl,
                chunk = shortUrl.Chunck
            });
        });
    }
}
