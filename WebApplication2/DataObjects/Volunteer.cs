using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApplication2.DataObjects
{
    [Table("volunteers")]
    [Index(nameof(VolunteerGroupId), Name = "volunteer_group_id")]
    [Index(nameof(VolunteerOrgId), Name = "volunteer_org_id")]
    public partial class Volunteer
    {
        [Key]
        [Column("volunteer_id", TypeName = "int(11)")]
        [Display(Name = "ID Волонтера")]
        public int VolunteerId { get; set; }
        [Column("volunteer_org_id", TypeName = "int(11)")]
        [Display(Name = "ID Волонтерской организации")]
        public int? VolunteerOrgId { get; set; }
        [Column("volunteer_group_id", TypeName = "int(11)")]
        [Display(Name = "ID Волонтерской группы")]
        public int VolunteerGroupId { get; set; }
        [Required(ErrorMessage = "Введите корректную фамилию!")]
        [Column("lastname")]
        [Display(Name = "Фамилия")]
        [StringLength(40)]
        public string Lastname { get; set; }
        [Required(ErrorMessage = "Введите корректное имя!")]
        [Column("firstname")]
        [Display(Name = "Имя")]
        [StringLength(40)]
        public string Firstname { get; set; }
        [Required(ErrorMessage = "Введите корректное отчество!")]
        [Column("patronymic")]
        [Display(Name = "Отчество")]
        [StringLength(40)]
        public string Patronymic { get; set; }
        [Required(ErrorMessage = "Введите корректный пол!")]
        [Column("sex")]
        [Display(Name = "Пол")]
        [StringLength(3)]
        public string Sex { get; set; }
        [Column("age", TypeName = "int(3)")]
        [Display(Name = "Возраст")]
        public int Age { get; set; }
        [Column("phone_number", TypeName = "bigint(20)")]
        [Display(Name = "Номер телефона")]
        public long PhoneNumber { get; set; }

        [ForeignKey(nameof(VolunteerGroupId))]
        [InverseProperty("Volunteers")]
        public virtual VolunteerGroup VolunteerGroup { get; set; }
        [ForeignKey(nameof(VolunteerOrgId))]
        [InverseProperty(nameof(VolunteerOrganization.Volunteers))]
        public virtual VolunteerOrganization VolunteerOrg { get; set; }
    }
}
