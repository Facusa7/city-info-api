using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Entities
{
    public class PointOfInterest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        /// <summary>
        /// This is a convention-based to point a foreign key. A property is considered a navigation one if
        /// the declaration here cannot be mapped as a scalar type by the current database provider. 
        /// </summary>
        [ForeignKey("CityId")]
        public City City { get; set; }
        /// <summary>
        /// This is the way to explicit the foreign key. Even though is not necessary, it is a good way to make it clear. 
        /// </summary>
        public int CityId { get; set; }
    }
}
