using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Startup.Api.Models.Controllers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Startup.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class RandomizeController : BaseController
{
    private readonly ILogger<RandomizeController> _logger;

    // ReSharper disable once ConvertToPrimaryConstructor
    public RandomizeController(ILogger<RandomizeController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Generate a secret gift exchange list, pairing each person with another randomly.
    /// </summary>
    /// <param name="namesList">The list of participants to be randomly paired with another. Count >= 3</param>
    /// <returns>A list of pairs of people, each of whom is assigned to someone else.</returns>
    [AllowAnonymous]
    [HttpPost("GenerateRandomPairList")]
    [Produces("application/json")]
    [SwaggerResponse((int)HttpStatusCode.OK, "A string of characters", typeof(IList<PairedName>))]
    public async Task<ActionResult> GenerateRandomPairList([FromBody] IList<string> namesList)
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(GenerateRandomPairList));

        await Task.Yield();

        if (namesList == null || namesList.Count <= 2)
        {
            throw new ArgumentException(nameof(namesList));
        }

        // Get a randomized names list.
        namesList = namesList.OrderBy(_ => Random.Shared.Next()).ToList();

        // Create a list of pairs between each name and a randomized name.
        var randomizeNamesList = new List<PairedName>();

        if (namesList.Count % 2 == 0)
        {
            for (int ii = 0; ii < namesList.Count; ii += 2)
            {
                // Add the names to the list of paired participants.
                randomizeNamesList.Add(new PairedName
                {
                    Person = namesList[ii],
                    HasName = namesList[ii + 1]
                });
            }
        }
        else
        {
            for (int ii = 0; ii < namesList.Count; ii += 2)
            {
                if (ii + 1 >= namesList.Count)
                {
                    // Pick one person at random to have a double exchange
                    randomizeNamesList.Add(new PairedName
                    {
                        Person = namesList[ii],
                        HasName = namesList[Random.Shared.Next(0, namesList.Count - 1)]
                    });
                }
                else
                {
                    // Add the names to the list of paired participants.
                    randomizeNamesList.Add(new PairedName
                    {
                        Person = namesList[ii],
                        HasName = namesList[ii + 1]
                    });
                }
            }
        }

        // Return the list of randomly paired names.
        return new OkObjectResult(randomizeNamesList);
    }
}