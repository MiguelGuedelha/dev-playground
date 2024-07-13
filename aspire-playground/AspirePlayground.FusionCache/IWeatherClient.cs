using Refit;

namespace AspirePlayground.FusionCache;

public interface IWeatherClient
{
    [Get("/weatherforecast")]
    object GetWeather(CancellationToken cancellationToken);
}