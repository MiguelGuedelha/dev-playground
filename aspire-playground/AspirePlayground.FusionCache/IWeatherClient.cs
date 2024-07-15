using Refit;

namespace AspirePlayground.FusionCache;

public interface IWeatherClient
{
    [Get("/weatherforecast")]
    Task<object> GetWeather(CancellationToken cancellationToken);
}