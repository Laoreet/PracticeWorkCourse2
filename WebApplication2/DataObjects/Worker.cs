using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace WebApplication2.DataObjects
{
    [Table("workers")]
    [Index(nameof(StationId), Name = "workers_ibfk_1")]
    public partial class Worker
    {
        [Key]
        [Column("worker_id", TypeName = "int(11)")]
        [Display(Name = "ID Сотрудника")]
        public int WorkerId { get; set; }
        [Column("station_id", TypeName = "int(11)")]
        [Display(Name = "ID Станции")]
        public int StationId { get; set; }
        [Required(ErrorMessage = "Введите корректную фамилию!")]
        [Column("secondname")]
        [Display(Name = "Фамилия")]
        [StringLength(40)]
        public string Secondname { get; set; }
        [Required(ErrorMessage = "Введите корректное имя!")]
        [Column("firstname")]
        [Display(Name = "Имя")]
        [StringLength(40)]
        public string Firstname { get; set; }
        [Required(ErrorMessage = "Введите корректное отчество!")]
        [Column("patronymic")]
        [Display(Name = "Отчество")]
        [StringLength(50)]
        public string Patronymic { get; set; }
        [Required(ErrorMessage = "Введите корректную должность!")]
        [Column("position")]
        [Display(Name = "Должность")]
        [StringLength(40)]
        public string Position { get; set; }
        [Required(ErrorMessage = "Введите корректный пол!")]
        [Column("sex")]
        [Display(Name = "Пол")]
        [StringLength(3)]
        public string Sex { get; set; }
        [Column("age", TypeName = "int(3)")]
        [Display(Name = "Возраст")]
        public int? Age { get; set; }
        [Column("phone_number", TypeName = "int(11)")]
        [Display(Name = "Номер телефона")]
        public int PhoneNumber { get; set; }
        [Column("salary", TypeName = "int(11)")]
        [Display(Name = "Зарплата")]
        public int Salary { get; set; }

        [ForeignKey(nameof(StationId))]
        [InverseProperty("Workers")]
        public virtual Station Station { get; set; }
    }
}
