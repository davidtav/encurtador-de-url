using System.Net;
using System.Reflection.Emit;
using Carter;
using Carter.ModelBinding;
using Carter.Request;
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
            if (Uri.TryCreate(shortUrl.Url, UriKind.Absolute, out var uriParsed))
            {
                shortUrl.Chunck = Nanoid.Nanoid.Generate(size: 9);
                db.GetCollection<ShortUrl>(BsonAutoId.Guid).Insert(shortUrl);
                res.StatusCode = (int)HttpStatusCode.OK;

                var baseUrl = config["BaseUrl"] ?? $"{req.Scheme}://{req.Host}";
                var rawShortUrl = $"{baseUrl}/{shortUrl.Chunck}";

                await res.WriteAsJsonAsync(new { shortUrl = rawShortUrl });
            }
            else
            {
                res.StatusCode = (int)HttpStatusCode.BadRequest;
                await res.WriteAsJsonAsync(new { error = "Url inválida" });
            }
        });
    }
}