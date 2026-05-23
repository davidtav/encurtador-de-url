using Carter;
namespace EncurtadorDeUrl.CarterModules;

public class PagesModule:CarterModule
{
    public PagesModule()
    {
        Get("/", async (req, res) =>
        {
            await res.WriteAsync("Hello, World!");
        });
    }
}