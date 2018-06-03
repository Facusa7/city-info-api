using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CityInfo.API.Controllers
{
    /// <summary>
    /// Since points of interest make sense only with a city, api/cities should be the first access to this controller.
    /// </summary>
    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _cityInfoRepository;
        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService localMailService, ICityInfoRepository cityInfoRepository)
        {
            _logger = logger;
            _mailService = localMailService;
            _cityInfoRepository = cityInfoRepository;
        }

        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {
                //var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
                if (!_cityInfoRepository.CityExists(cityId))
                {
                    _logger.LogInformation($"City with id: {cityId} wasn't found when accessing points of interest");
                    return NotFound();
                }
                var pointOfInterestForCity = _cityInfoRepository.GetPointOfInterestsForCity(cityId);
                var pointOfInterestForCityResults = pointOfInterestForCity.Select(poi => new PointsOfInterestDto()
                    {
                        Id = poi.Id,
                        Name = poi.Name,
                        Description = poi.Description
                    })
                    .ToList();

                return Ok(pointOfInterestForCityResults);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interests with city id: {cityId}", ex);
                //Since we are not handling exceptions yet, we just return a 500 error, with the minimum information to the consumer. 
                return StatusCode(500, "A problem happened while handling your request");
            }
        }
        [HttpGet("{cityId}/pointsofinterest/{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterest = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterest == null)
            {
                return NotFound();
            }

            var pointOfInterestDto = new PointsOfInterestDto()
            {
                Id = pointOfInterest.Id,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            return Ok(pointOfInterestDto);
            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
            //if (city == null)
            //{
            //    return NotFound();
            //}

            //var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            //if (pointOfInterest == null)
            //{
            //    return NotFound();
            //}

            //return Ok(pointOfInterest);
        }
        [HttpPost("{cityId}/pointsofinterest")]
        public IActionResult CreatePointOfInterest(int cityId, [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            if (pointOfInterest.Name == pointOfInterest.Description)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name");
            }
            //It does the validations that were set in the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);
            var finalPointOfInterest = new PointsOfInterestDto()
            {
                Id = ++maxPointOfInterestId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description

            };
            city.PointsOfInterest.Add(finalPointOfInterest);
            /* CreatedAtRout helper method allows us to return a response with a location header.
               That location header will contain the URI where the newly-created point of interest can be found. 
               Since GetPointOfInterest needs a cityId and the pointOfInterestId we pass that through, and also we send the object to show it. 
             */
            return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, id = finalPointOfInterest.Id}, finalPointOfInterest);
        }
        /// <summary>
        /// HTTP Standard says that the the 'put' verb is for updating all values, like a full update. 
        /// If the consumer doesn't provide a value for a field, that field should be put to its default value, which conveniently, it will
        /// have in the inputted object.
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="id"></param>
        /// <param name="pointOfInterest"></param>
        /// <returns></returns>
        [HttpPut("{cityId}/pointsofinterest/{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id, [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            //TODO: Add Fluent Validation
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            if (pointOfInterest.Name == pointOfInterest.Description)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name");
            }
            //It does the validations that were set in the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            pointOfInterestFromStore.Name = pointOfInterest.Name;
            pointOfInterestFromStore.Description = pointOfInterest.Description;

            //NoContent() means that the request completed successfully, but there is nothing to return.
            return NoContent();
        }
        /// <summary>
        /// JsonPatchDocument allows us to partially update an object
        /// A Patch request has this form 
        ///    [    {
        ///            "op":"replace", (operation)
        ///            "path": "/name", (name of the value to change)
        ///            "value": "updatedValue" (actual value) 
        ///          } 
        ///    ]
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
        [HttpPatch("{cityId}/pointsofinterest/{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id,
            [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = new PointOfInterestForUpdateDto()
                                            {
                                                Name = pointOfInterestFromStore.Name,
                                                Description = pointOfInterestFromStore.Description
                                            };
            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (pointOfInterestToPatch.Name == pointOfInterestToPatch.Description)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name");
            }
            /* This is to validate that the document that is being passed is valid.
             Since a delete operation could've been sent and this could cause invalid data in our models. */
            TryValidateModel(pointOfInterestToPatch);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

            //NoContent() means that the request completed successfully, but there is nothing to return.
            return NoContent();
        }

        [HttpDelete("{cityId}/pointsofinterest/{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            city.PointsOfInterest.Remove(pointOfInterestFromStore);
            _mailService.Send("Point of Interest Deleted ", $"Point of Interest {pointOfInterestFromStore.Name} with Id {pointOfInterestFromStore.Id} was deleted");
            return NoContent();
        }
    }
}
