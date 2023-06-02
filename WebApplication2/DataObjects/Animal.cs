using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;

#nullable disable

namespace WebApplication2.DataObjects
{
    [Table("animals")]
    [Index(nameof(AnimalTypeId), Name = "ID Животного")]
    [Index(nameof(StationId), Name = "ID Станции")]
    [Display(Name = "Животные")]
    public partial class Animal
    {
        public Animal()
        {
            AnimalApplications = new HashSet<AnimalApplication>();
        }

        private byte[] photoArr = null;

        [Key]
        [Column("animal_id", TypeName = "int(11)")]
        [Display(Name = "ID Животного")]
        public int AnimalId { get; set; }
        [Column("station_id", TypeName = "int(11)")]
        [Display(Name = "ID Станции")]
        public int StationId { get; set; }
        [Column("animal_type_id", TypeName = "int(11)")]
        [Display(Name = "ID Вида животного")]
        public int AnimalTypeId { get; set; }
        [Required(ErrorMessage = "Необходимо ввести корректную кличку!")]
        [Column("name")]
        [Display(Name = "Кличка")]
        [StringLength(64)]
        public string Name { get; set; }
        [Column("photo", TypeName = "mediumblob")]
        [Display(Name = "Фотография")]
        public byte[] Photo { get; set; }
        [Required(ErrorMessage = "Необходимо добавить корректную фотографию!")]
        [Column("animal_sex")]
        [Display(Name = "Пол животного")]
        [StringLength(5)]
        public string AnimalSex { get; set; }
        [Display(Name = "Возраст")]
        [Column("age", TypeName = "tinyint(4)")]
        public byte Age { get; set; }
        [Column("coloring")]
        [Display(Name = "Окраска")]
        [StringLength(80)]
        public string Coloring { get; set; }
        [Column("breed")]
        [Display(Name = "Порода")]
        [StringLength(40)]
        public string Breed { get; set; }
        [Column("specificity")]
        [Display(Name = "Особенности")]
        [StringLength(200)]
        public string Specificity { get; set; }
        [Column("history")]
        [Display(Name = "История")]
        [StringLength(500)]
        public string History { get; set; }

        //I FORM FILE PHOTOTOT
        [Display(Name = "PhotoHelper")]
        [NotMapped]
        public IFormFile PhotoHelper { get; set; }

        [ForeignKey(nameof(AnimalTypeId))]
        [InverseProperty("Animals")]
        public virtual AnimalType AnimalType { get; set; }
        [ForeignKey(nameof(StationId))]
        [InverseProperty("Animals")]
        public virtual Station Station { get; set; }
        [InverseProperty(nameof(AnimalApplication.Animal))]
        public virtual ICollection<AnimalApplication> AnimalApplications { get; set; }
    }
}
