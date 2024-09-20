using BookingSystem.Domain.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Domain.Entities
{
    public class Floor
    {
        [Key]
        public int FloorID { get; set; }  // Уникальный идентификатор этажа

        [Required]
        public int OfficeID { get; set; }  // Идентификатор офиса, к которому принадлежит этаж

        [ForeignKey("OfficeID")]
        public virtual Office Office { get; set; }  // Связь с офисом

        // Название этажа
        [Required]
        [StringLength(100)]
        public string FloorName { get; set; }  // Убедитесь, что это свойство существует

        // Данные изображения этажа
        public byte[] ImageData { get; set; }  // Убедитесь, что это свойство существует

        // MIME-тип изображения
        public string ImageMimeType { get; set; }  // Убедитесь, что это свойство существует

        // Навигационные свойства для рабочих мест и парковочных мест
        public virtual ICollection<Workspace> Workspaces { get; set; }  // Связь с рабочими местами
        public virtual ICollection<ParkingSpace> ParkingSpaces { get; set; }  // Связь с парковочными местами

        // Конструктор для инициализации коллекций
        public Floor()
        {
            Workspaces = new List<Workspace>();  // Инициализация коллекции рабочих мест
            ParkingSpaces = new List<ParkingSpace>();  // Инициализация коллекции парковочных мест
        }
    }
}