using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Entities
{
    public class City
    {
        /// <summary>
        /// When we declare an Id (City_Id will work as well) field in the EntityClass, the DB would assume that is the primary key, but
        /// it won't hurt add the [Key] annotation here.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        public ICollection<PointOfInterest> PointOfInterest { get; set; } = new List<PointOfInterest>();
    }
}
