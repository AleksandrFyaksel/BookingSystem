using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Domain.Entities
{
    public class Department
    {
        [Key]
        public int DepartmentID { get; set; }  // Уникальный идентификатор отдела

        [Required]
        [StringLength(100)]
        public string DepartmentName { get; set; }  // Название отдела

        // Навигационное свойство для связи с пользователями
        public virtual ICollection<User> Users { get; set; }  // Связь с пользователями

        // Конструктор для инициализации коллекции пользователей
        public Department()
        {
            Users = new List<User>();  // Инициализация коллекции пользователей
        }
    }
}