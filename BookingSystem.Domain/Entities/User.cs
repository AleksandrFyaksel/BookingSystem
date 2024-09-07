using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingSystem.Domain.Entities
{
    public class User
    {
        [Key]
        public int UserID { get; set; }  // Уникальный идентификатор пользователя

        [Required]
        [StringLength(100)]
        public string Name { get; set; }  // Имя пользователя

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }  // Адрес электронной почты

        [Required]
        [StringLength(256)]
        public string PasswordHash { get; set; }  // Хеш пароля

        [StringLength(15)]
        public string PhoneNumber { get; set; }  // Номер телефона

        [ForeignKey("Role")]
        public int RoleID { get; set; }  // Идентификатор роли пользователя

        // Навигационные свойства
        public virtual Role Role { get; set; }  // Связь с ролью
        public virtual ICollection<Booking> Bookings { get; set; }  // Связь с бронированиями
        public virtual ICollection<UserPassword> UserPasswords { get; set; }  // Связь с паролями пользователя

        // Конструктор для инициализации коллекций
        public User()
        {
            Bookings = new List<Booking>();  // Инициализация коллекции бронирований
            UserPasswords = new List<UserPassword>();  // Инициализация коллекции паролей
        }
    }
}