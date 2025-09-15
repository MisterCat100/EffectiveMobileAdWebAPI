using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace EffectiveMobileAdWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdvertisementController : ControllerBase
{
    readonly string path = Path.Combine(Directory.GetCurrentDirectory(), "addresses.txt");
    static readonly Dictionary<string, string> platformsAdresses = new();
    static readonly Dictionary<string, string> addressesPlatforms = new();

    [HttpGet("search-by-address/{address}")]
    public IActionResult SearchByAddress(string address)
    {
        address = HttpUtility.UrlDecode(address);

        List<string> adPlatforms = new();

        if (platformsAdresses.Count == 0)
            return BadRequest("Addresses are empty");

        foreach (string platform in platformsAdresses.Keys)
        {
            if (platformsAdresses[platform].Contains(address))
                adPlatforms.Add(platform);
        }

        foreach (string addressToCompare in addressesPlatforms.Keys)
        {
            if (addressToCompare.Contains(address))
                adPlatforms.Add(addressesPlatforms[addressToCompare]);
        }

        return Ok(adPlatforms);
    }

    [HttpPost]
    public async Task<IActionResult> LoadPlatformsFromFile(IFormFile formFile)
    {
        if (Path.GetExtension(formFile.FileName) != ".txt")
            return BadRequest("Invalid format of the file");

        if (formFile.Length > 1024 * 1024 * 256)
            return BadRequest("File can't be bigger, than 256 MB");

        using StreamReader reader = new(formFile.OpenReadStream(), System.Text.Encoding.UTF8);

        string? line;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            string[] platformAdresses = line.Split(':');

            platformsAdresses[platformAdresses[0]] = platformAdresses[1];
        }

        return Ok("File sucessfully readed");
    }
}
