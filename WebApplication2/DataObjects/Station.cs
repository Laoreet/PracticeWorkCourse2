using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApplication2.DataObjects
{
    [Table("stations")]
    [Display(Name = "Станция")]

    public partial class Station
    {
        public Station()
        {
            Animals = new HashSet<Animal>();
            CarrierVolunteers = new HashSet<CarrierVolunteer>();
            Donations = new HashSet<Donation>();
            VolunteerGroups = new HashSet<VolunteerGroup>();
            Workers = new HashSet<Worker>();
        }

        [Key]
        [Column("station_id", TypeName = "int(11)")]
        [Display(Name = "ID Станции")]
        public int StationId { get; set; }
        [Required(ErrorMessage = "Введите корректное название станции!")]
        [Column("station_name")]
        [Display(Name = "Название станции")]
        [StringLength(128)]
        public string StationName { get; set; }
        [Required(ErrorMessage = "Введите корректный адрес станции!")]
        [Column("address")]
        [Display(Name = "Адрес")]
        [StringLength(64)]
        public string Address { get; set; }
        [Column("contact_number", TypeName = "int(11)")]
        [Display(Name = "Контактный телефон")]
        public int ContactNumber { get; set; }
        [Required(ErrorMessage = "Введите корректные данные контактного лица!")]
        [Column("contact_person")]
        [Display(Name = "Контактное лицо")]
        [StringLength(64)]
        public string ContactPerson { get; set; }

        [InverseProperty(nameof(Animal.Station))]
        public virtual ICollection<Animal> Animals { get; set; }
        [InverseProperty(nameof(CarrierVolunteer.Station))]
        public virtual ICollection<CarrierVolunteer> CarrierVolunteers { get; set; }
        [InverseProperty(nameof(Donation.Station))]
        public virtual ICollection<Donation> Donations { get; set; }
        [InverseProperty(nameof(VolunteerGroup.Station))]
        public virtual ICollection<VolunteerGroup> VolunteerGroups { get; set; }
        [InverseProperty(nameof(Worker.Station))]
        public virtual ICollection<Worker> Workers { get; set; }
    }
}
