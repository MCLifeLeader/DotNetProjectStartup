using Api.Startup.Example.Models.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Startup.Example.Controllers.V1;

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
    /// <param name="namesList">The list of participants to be randomly paired with another.</param>
    /// <returns>A list of pairs of people, each of whom is assigned to someone else.</returns>
    [AllowAnonymous]
    [HttpPost("GenerateSecretGiftExchangeList")]
    public async Task<IList<GiftExchange>> GenerateSecretGiftExchangeList([FromBody] IList<string> namesList)
    {
        _logger.LogDebug("'{Class}.{Method}' called", GetType().Name, nameof(GenerateSecretGiftExchangeList));

        // Randomize the items in the names list.
        var randomList = await Task.Run(() =>
        {
            List<string> preRandomList;
            while (true)
            {
                // Get a randomized names list.
                preRandomList = namesList.OrderBy(_ => Random.Shared.Next()).ToList();

                // Check if the random list is valid and no two people have the same name
                bool isValid = true;
                for (int ii = 0; ii < namesList.Count; ii++)
                {
                    if (preRandomList[ii] == namesList[ii])
                    {
                        isValid = false;
                        break;
                    }
                }

                // If the random list is valid, break out of the loop.
                if (isValid)
                {
                    break;
                }
            }

            return preRandomList;
        });

        // Create a list of pairs between each name and a randomized name.
        var randomizeGiftList = new List<GiftExchange>();
        for (int ii = 0; ii < namesList.Count; ii++)
        {
            // Add the names to the list of paired participants.
            randomizeGiftList.Add(new GiftExchange
            {
                Person = namesList[ii],
                HasName = randomList[ii]
            });
        }

        // Return the list of randomly paired names.
        return randomizeGiftList;
    }
}