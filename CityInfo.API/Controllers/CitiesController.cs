using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// By adding this annotation, we are saying that all requests with api/cities will be addressed in this controller. 
    /// The annotation could be like this [Route("api/[controller]")], and for web sites is a good idea, but for APIs is not. 
    /// Since the name of the controller could change if we did a refactor, it could affect to the consumers of this info, and that is not important to them.
    /// </summary>
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        /// <summary>
        /// JsonResult class returns a JSONified version of whatever we pass into the constructor of JsonResult, 
        /// but returning an IActionResult gives us more flexibility so not always we return a JSON.
        /// We don't have a 404 case here, because even the empty list is a valid result. 
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public IActionResult GetCities()
        {
            return Ok(CitiesDataStore.Current.Cities);
        }
        [HttpGet("{id}")]
        public IActionResult Getcity(int id)
        {
            /*
             * If an exception happens, the server will return a 500 error automatically.
             */
            var cityToReturn = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == id);
            if (cityToReturn == null)
            {
                return NotFound();
            }

            return Ok(cityToReturn);
        }
    }
}
