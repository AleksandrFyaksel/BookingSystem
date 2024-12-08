using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Domain.Entities
{
    public class Office
    {
        [Key]
        public int OfficeID { get; set; }  // Уникальный идентификатор офиса

        [Required]
        [StringLength(100)]
        public string OfficeName { get; set; }  // Название офиса

        [Required]
        [StringLength(200)]
        public string Location { get; set; }  // Местоположение офиса

        [Range(1, int.MaxValue)]
        public int Capacity { get; set; }  // Максимальное количество рабочих мест

        [StringLength(15)]
        public string Phone { get; set; }  // Номер телефона офиса

        // Навигационное свойство для этажей
        public virtual ICollection<Floor> Floors { get; set; }  // Связь с этажами

        // Конструктор для инициализации коллекции этажей
        public Office()
        {
            Floors = new List<Floor>();  // Инициализация коллекции этажей
        }
    }
}