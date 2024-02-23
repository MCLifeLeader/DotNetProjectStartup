using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Startup.Api.Models.Controllers;

namespace Startup.Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class RandomizeController : BaseController
{
    private readonly ILogger<RandomizeController> _logger;

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
    public async Task<IList<GiftExchange>> GenerateRandomPairList([FromBody] IList<string> namesList)
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(GenerateRandomPairList));

        if (namesList == null || namesList.Count <= 2)
        {
            throw new ArgumentException(nameof(namesList));
        }

        // Get a randomized names list.
        namesList = namesList.OrderBy(_ => Random.Shared.Next()).ToList();

        // Create a list of pairs between each name and a randomized name.
        var randomizeGiftList = new List<GiftExchange>();

        if (namesList.Count % 2 == 0)
        {
            for (int ii = 0; ii < namesList.Count; ii += 2)
            {
                // Add the names to the list of paired participants.
                randomizeGiftList.Add(new GiftExchange
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
                    randomizeGiftList.Add(new GiftExchange
                    {
                        Person = namesList[ii],
                        HasName = namesList[Random.Shared.Next(0, namesList.Count - 1)]
                    });
                }
                else
                {
                    // Add the names to the list of paired participants.
                    randomizeGiftList.Add(new GiftExchange
                    {
                        Person = namesList[ii],
                        HasName = namesList[ii + 1]
                    });
                }
            }
        }

        // Return the list of randomly paired names.
        return randomizeGiftList;
    }
}