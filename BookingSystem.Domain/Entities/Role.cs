using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Domain.Entities
{
    public class Role
    {
        [Key]
        public int RoleID { get; set; }  // Уникальный идентификатор роли

        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }  // Название роли (например, администратор, сотрудник)

        // Навигационное свойство для связи с пользователями
        public virtual ICollection<User> Users { get; set; }  // Связь с пользователями

        // Конструктор для инициализации коллекции пользователей
        public Role()
        {
            Users = new List<User>();  // Инициализация коллекции пользователей
        }
    }
}