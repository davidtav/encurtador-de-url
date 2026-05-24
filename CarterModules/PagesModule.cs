using Carter;
using Carter.Request;
using EncurtadorDeUrl.Models;
using LiteDB;

namespace EncurtadorDeUrl.CarterModules;

public class PagesModule:CarterModule
{
    public PagesModule(ILiteDatabase db)
    {
        Get("/", async (req, res) =>
        {
            res.ContentType = "text/html";
            res.StatusCode = 200;
            await res.SendFileAsync("wwwroot/index.html");
        });
        Get("/{chunck}", (req, res) =>
        {
            var chunck = req.RouteValues.As<string>("chunck");
            var shortUrl = db.GetCollection<ShortUrl>().FindOne(x => x.Chunck == chunck);
            if (shortUrl == null)
            {
                res.Redirect("/");
            }

            if (shortUrl != null) res.Redirect(shortUrl.Url);
            return Task.CompletedTask;
        });
    }
}