using System.ComponentModel.DataAnnotations;

namespace ZeroGravity.Db.Models
{
    public class EducationalInfoData : ModelBase
    {
        [Required]
        public string Category { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public string Tittle { get; set; }
        public string Description { get; set; }
    }
}