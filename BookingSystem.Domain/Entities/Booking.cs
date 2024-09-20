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
        public int UserID { get; set; }  // Идентификатор пользователя, который сделал бронирование

        [ForeignKey("Workspace")]
        public int? WorkspaceID { get; set; }  // Идентификатор рабочего места (может быть NULL)

        [ForeignKey("ParkingSpace")]
        public int? ParkingSpaceID { get; set; }  // Идентификатор парковочного места (может быть NULL)

        public DateTime BookingDate { get; set; }  // Дата начала бронирования
        public DateTime StartDateTime { get; set; }  // Время начала бронирования
        public DateTime EndDateTime { get; set; }  // Время окончания бронирования

        [Required]
        public int BookingStatusID { get; set; }  // Идентификатор статуса бронирования

        public string? AdditionalRequirements { get; set; }  // Дополнительные требования

        // Навигационные свойства
        public virtual User User { get; set; }  // Связь с пользователем
        public virtual Workspace Workspace { get; set; }  // Связь с рабочим местом
        public virtual ParkingSpace ParkingSpace { get; set; }  // Связь с парковочным местом

        // Связь со статусом бронирования
        [ForeignKey("BookingStatusID")] // Указываем, что это свойство связано с BookingStatusID
        public virtual BookingStatus BookingStatus { get; set; }
    }
}