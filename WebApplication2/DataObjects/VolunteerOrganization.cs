using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApplication2.DataObjects
{
    [Table("volunteer_organizations")]
    [Display(Name = "Волонтерская организация")]
    public partial class VolunteerOrganization
    {
        public VolunteerOrganization()
        {
            VolunteerApplications = new HashSet<VolunteerApplication>();
            Volunteers = new HashSet<Volunteer>();
        }

        [Key]
        [Column("volunteer_org_id", TypeName = "int(11)")]
        [Display(Name = "ID Волонтерской организации")]
        public int VolunteerOrgId { get; set; }
        [Required(ErrorMessage = "Введите корректное название организации!")]
        [Column("org_name")]
        [Display(Name = "Название организации")]
        [StringLength(60)]
        public string OrgName { get; set; }
        [Required(ErrorMessage = "Введите корректный адрес!")]
        [Column("adress")]
        [Display(Name = "Адрес")]
        [StringLength(80)]
        public string Adress { get; set; }
        [Column("contact_number", TypeName = "bigint(20)")]
        [Display(Name = "Контактный телефон")]
        public long ContactNumber { get; set; }
        [Required(ErrorMessage = "Введите корректные данные о контактном лице!")]
        [Column("contact_person")]
        [Display(Name = "Контактное лицо")]
        [StringLength(125)]
        public string ContactPerson { get; set; }

        [InverseProperty(nameof(VolunteerApplication.VolunteerOrg))]
        public virtual ICollection<VolunteerApplication> VolunteerApplications { get; set; }
        [InverseProperty(nameof(Volunteer.VolunteerOrg))]
        public virtual ICollection<Volunteer> Volunteers { get; set; }
    }
}
