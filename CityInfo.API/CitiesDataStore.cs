using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }
        /// <summary>
        ///This make sure that we can keep working on the same data as long as we don't restart.
        /// </summary>
        public static CitiesDataStore Current { get; } = new CitiesDataStore(); 

        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "Buenos Aires",
                    Description = "The Paris of South America"
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Resistencia",
                    Description = "The City of Scultures"
                },
                new CityDto()
                {
                    Id = 3,
                    Name = "Corrientes",
                    Description = "The City with Paye"
                }
            };
        }
    }
}
