using System.Threading.Tasks;
using AccountServer.Infrastructure.Services;
using AccountServer.ViewModels.InputParameters;
using CommonLibraries.Extensions;
using CommonLibraries.Response;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AccountServer.Controllers
{
  [EnableCors("AllowAllOrigin")]
  [Produces("application/json")]
  [Route("cities")]
  public class CitiesController : Controller //To get user's posts
  {
    private readonly ICityService _cityService;

    public CitiesController(ICityService cityService)
    {
      _cityService = cityService;
    }

    [HttpPost]
    public async Task<IActionResult> GetCities([FromBody] FindCitiesViewModel vm)
    {
      if (!ModelState.IsValid) return new BadResponseResult(ModelState);
      return vm.Pattern.IsNullOrEmpty()
        ? new OkResponseResult(await _cityService.GetPopularCities(vm.PageParams.Offset, vm.PageParams.Count))
        : new OkResponseResult(await _cityService.FindCities(vm.Pattern, vm.PageParams.Offset, vm.PageParams.Count));
    }
  }
}