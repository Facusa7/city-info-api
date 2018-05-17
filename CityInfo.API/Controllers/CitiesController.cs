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
        /// JsonResult class returns a JSONified version of whatever we pass into the constructor of JsonResult
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public JsonResult GetCities()
        {
            return new JsonResult(CitiesDataStore.Current.Cities);
        }
        [HttpGet("{id}")]
        public JsonResult Getcity(int id)
        {
            return new JsonResult(CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == id));
        }
    }
}
