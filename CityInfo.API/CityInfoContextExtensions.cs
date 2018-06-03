using CityInfo.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API
{
    public static class CityInfoExtensions
    {
        public static void EnsureSeedDataForContext(this CityInfoContext context)
        {
            //If we already have data, we don't insert anything. 
            if (context.Cities.Any())
            {
                return;
            }

            // init seed data
            var cities = new List<City>()
            {
                new City()
                {
                     Name = "Buenos Aires",
                     Description = "The Paris of South America",
                     PointOfInterest = new List<PointOfInterest>()
                     {
                         new PointOfInterest() {
                             Name = "Obelisco",
                             Description = "Big Obelisk in the city center"
                         },
                          new PointOfInterest() {
                              Name = "Pink House",
                              Description = "The government house"
                          },
                     }
                },
                new City()
                {
                    Name = "Resistencia",
                    Description = "The City of Scultures",
                    PointOfInterest = new List<PointOfInterest>()
                     {
                         new PointOfInterest() {
                             Name = "Cathedral",
                             Description = "A modern style cathedral"
                         },
                          new PointOfInterest() {
                             Name = "Democracy Park",
                             Description = "Green space perfect for open activities"
                          },
                     }
                },
                new City()
                {
                    Name = "Corrientes",
                    Description = "The City with 'Paje' ",
                    PointOfInterest = new List<PointOfInterest>()
                     {
                         new PointOfInterest() {
                             Name = "Costanera",
                             Description =  "A space near the river"
                         }
                     }
                }
            };

            context.Cities.AddRange(cities);
            context.SaveChanges();
        }
    }
}
