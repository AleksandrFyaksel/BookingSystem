using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BookingSystem.Domain.Entities
{
    public class UserPassword
    {
        [Key]
        public int UserPasswordID { get; set; }  // Уникальный идентификатор пароля

        [Required]
        [StringLength(256)]
        public string PasswordHash { get; set; }  // Хеш пароля пользователя

        [Required]
        [StringLength(256)]
        public string Salt { get; set; }  // Соль для хеширования пароля

        [ForeignKey("User")]
        public int UserID { get; set; }  // Идентификатор пользователя

        // Навигационное свойство
        public virtual User User { get; set; }  // Связь с пользователем

        // Конструктор
        public UserPassword() { }
    }
}