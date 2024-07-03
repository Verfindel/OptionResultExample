using Microsoft.AspNetCore.Mvc;
using OptionResultExample.Models;
using OneOf;

namespace OptionResultExample.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController(ILogger<WeatherForecastController> logger) : ControllerBase
{
    private record struct VeryImportantThing(string Name, string Description);

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    
    [HttpGet("getWeatherForecast")]
    public IActionResult Get()
    {
        var lazyService = LazyWeatherService();

        return lazyService.Match<IActionResult>(
                Ok,
                exceptionWithReference => BadRequest(exceptionWithReference)
                );
    }

    private Either<HumanReadableExceptionWithReference, IEnumerable<WeatherForecast>> LazyWeatherService()
    {
        var testForecast = new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
            TemperatureC = Random.Shared.Next(-20, 55),
            //Summary = Summaries[Random.Shared.Next(Summaries.Length)],
            OptionSummary = Option<string>.Some(Summaries[Random.Shared.Next(Summaries.Length)])
        };

        //With nullable
        var veryImportant = new VeryImportantThing("Weather something, very important!", testForecast.Summary ?? "default");
        logger.Log(LogLevel.Information, "This is a very important thing: {VeryImportantThing}", veryImportant.Description);

        //With option
        return testForecast.OptionSummary.Match<Either<HumanReadableExceptionWithReference, IEnumerable<WeatherForecast>>>(
            summary =>
            {
                var veryImportantMatch = new VeryImportantThing("Weather something, very important!", summary);
                logger.Log(LogLevel.Information, "This is a very important thing: {VeryImportantThing}", veryImportantMatch);

                return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                })
                .ToArray();
            },
            () =>
            {
                var reference = Guid.NewGuid();
                logger.Log(LogLevel.Error, "WHERE'S THE IMPORTANT THING?! With reference: {reference}", reference);
                return new HumanReadableExceptionWithReference
                {
                    Reference = reference.ToString(),
                    Description = "The important thing is missing!"
                };
            }
        );
    }


    private bool DiscriminatedUnionsAreCool(OneOf<int, string, bool, double, HumanReadableExceptionWithReference, WeatherForecast> oneOfThese)
    {
        return oneOfThese.Match(
                myInt => {
                    if (myInt > 1)
                        return true;

                    return false;
                },
                myString => true,
                myBool => {
                    if (myBool == true)
                        return false;

                    return true;
                },
                myDouble => true,
                myException => true,
                myForecast => true
                );
    }
}