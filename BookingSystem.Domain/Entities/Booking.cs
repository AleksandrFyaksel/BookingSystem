using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Domain.Entities
{
    public class Booking
    {
        [Key]
        public int BookingID { get; set; }  // Уникальный идентификатор бронирования

        [ForeignKey("User")]
        [Required]
        public int UserID { get; set; }  // Идентификатор пользователя, который сделал бронирование

        [ForeignKey("Workspace")]
        [Required]
        public int WorkspaceID { get; set; }  // Идентификатор рабочего места, которое бронируется

        [ForeignKey("ParkingSpace")]
        public int? ParkingSpaceID { get; set; }  // Идентификатор парковочного места (может быть NULL)

        [ForeignKey("Floor")]
        [Required]
        public int FloorID { get; set; }  // Идентификатор этажа, на котором находится рабочее место

        [ForeignKey("BookingStatus")]
        [Required]
        public int BookingStatusID { get; set; }  // Идентификатор статуса бронирования

        public DateTime BookingDate { get; set; }  // Дата начала бронирования
        public DateTime StartDateTime { get; set; }  // Время начала бронирования
        public DateTime EndDateTime { get; set; }  // Время окончания бронирования

        public string AdditionalRequirements { get; set; }  // Дополнительные требования к бронированию

        public virtual User User { get; set; }  // Связь с пользователем
        public virtual Workspace Workspace { get; set; }  // Связь с рабочим местом
        public virtual ParkingSpace ParkingSpace { get; set; }  // Связь с парковочным местом
        public virtual Floor Floor { get; set; }  // Связь с этажом
        public virtual BookingStatus BookingStatus { get; set; }  // Связь со статусом бронирования

        /// <summary>
        /// Проверяет, является ли бронирование активным.
        /// </summary>
        /// <returns>true, если статус бронирования активен; иначе false.</returns>
        public bool IsActive()
        {
            return BookingStatus != null && BookingStatus.StatusName == "Active"; // Проверка статуса
        }
    }
}