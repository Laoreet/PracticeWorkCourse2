using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApplication2.DataObjects
{
    [Table("carrier_volunteers")]
    [Index(nameof(StationId), Name = "shelter_id")]
    public partial class CarrierVolunteer
    {
        [Key]
        [Column("carrier_volunteer_id", TypeName = "int(11)")]
        [Display(Name = "ID Волонтера-перевозчика")]
        public int CarrierVolunteerId { get; set; }
        [Column("station_id", TypeName = "int(11)")]
        [Display(Name = "ID Станции")]
        public int StationId { get; set; }
        [Required(ErrorMessage = "Выберите корректную станцию!")]
        [Column("fullname")]
        [Display(Name = "ФИО")]
        [StringLength(125)]
        public string Fullname { get; set; }
        [Column("phone_number", TypeName = "bigint(20)")]
        [Display(Name = "Номер телефона")]
        public long PhoneNumber { get; set; }
        [Column("vehicle")]
        [Display(Name = "Транспорт")]
        [StringLength(50)]
        public string Vehicle { get; set; }

        [ForeignKey(nameof(StationId))]
        [InverseProperty("CarrierVolunteers")]
        public virtual Station Station { get; set; }
    }
}
