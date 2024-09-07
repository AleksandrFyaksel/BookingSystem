using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookingSystem.Domain.Interfaces
{
    public interface IRepository<T>
    {
        /// <summary>
        /// Добавляет новую сущность.
        /// </summary>
        void Add(T entity);

        /// <summary>
        /// Асинхронно добавляет новую сущность.
        /// </summary>
        Task AddAsync(T entity);

        /// <summary>
        /// Удаляет сущность по идентификатору.
        /// </summary>
        bool Delete(int id);

        /// <summary>
        /// Асинхронно удаляет сущность по идентификатору.
        /// </summary>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Находит сущности по заданному условию.
        /// </summary>
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Получает сущность по идентификатору с возможностью включения связанных данных.
        /// </summary>
        T Get(int id, params string[] includes);

        /// <summary>
        /// Асинхронно получает сущность по идентификатору.
        /// </summary>
        Task<T> GetAsync(int id, params string[] includes);

        /// <summary>
        /// Получает все сущности.
        /// </summary>
        IQueryable<T> GetAll();

        /// <summary>
        /// Получает все сущности с пагинацией.
        /// </summary>
        IQueryable<T> GetAll(int pageNumber, int pageSize);

        /// <summary>
        /// Получает все сущности с сортировкой.
        /// </summary>
        IQueryable<T> GetAll(Expression<Func<T, object>> orderBy, bool ascending = true);

        /// <summary>
        /// Обновляет существующую сущность.
        /// </summary>
        void Update(T entity);

        /// <summary>
        /// Асинхронно обновляет существующую сущность.
        /// </summary>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Асинхронно находит сущности по заданному условию.
        /// </summary>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    }
}