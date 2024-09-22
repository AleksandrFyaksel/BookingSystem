﻿using BookingSystem.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookingSystem.Domain.Entities
{
    public class Workspace
    {
        [Key]
        public int WorkspaceID { get; set; }  // Уникальный идентификатор рабочего места

        [ForeignKey("Floor")]
        public int FloorID { get; set; }  // Идентификатор этажа, на котором находится рабочее место

        public string Label { get; set; }  // Название рабочего места
        public string Position { get; set; }  // Позиция рабочего места
        public bool IsAvailable { get; set; }  // Статус доступности (свободно/занято)

        // Навигационные свойства
        public virtual Floor Floor { get; set; }  // Связь с этажом
        public virtual ICollection<Booking> Bookings { get; set; }  // Связь с бронированиями

        // Конструктор для инициализации коллекции бронирований
        public Workspace()
        {
            Bookings = new List<Booking>();  // Инициализация коллекции бронирований
            IsAvailable = true; // По умолчанию рабочее место доступно
        }

        // Метод для установки статуса рабочего места
        public void SetAvailability(bool isAvailable)
        {
            IsAvailable = isAvailable;  // Устанавливает статус доступности
        }

        // Метод для бронирования рабочего места
        public void Book()
        {
            if (IsAvailable)
            {
                IsAvailable = false; // Устанавливаем статус как занятое
                // Логика для добавления бронирования в коллекцию Bookings
            }
            else
            {
                throw new InvalidOperationException("Рабочее место уже забронировано.");
            }
        }
    }
}