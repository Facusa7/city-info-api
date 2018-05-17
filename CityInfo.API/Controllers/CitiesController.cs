using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    public class CitiesController : Controller
    {
        /// <summary>
        /// JsonResult class returns a JSONified version of whatever we pass into the constructor of JsonResult
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCities()
        {
            return new JsonResult(new List<object>()
            {
                new { id=1, name="Buenos Aires"},
                new { id=2, name="Prague"},
            });
        }
    }
}
