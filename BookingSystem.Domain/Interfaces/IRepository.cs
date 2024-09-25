using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookingSystem.Domain.Interfaces
{
    /// <summary>
    /// Интерфейс для репозитория сущностей.
    /// </summary>
    /// <typeparam name="T">Тип сущности.</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Добавляет новую сущность.
        /// </summary>
        /// <param name="entity">Сущность для добавления.</param>
        void Add(T entity);

        /// <summary>
        /// Асинхронно добавляет новую сущность.
        /// </summary>
        /// <param name="entity">Сущность для добавления.</param>
        Task AddAsync(T entity);

        /// <summary>
        /// Удаляет сущность по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности.</param>
        /// <returns>True, если удаление прошло успешно; иначе - false.</returns>
        bool Delete(int id);

        /// <summary>
        /// Асинхронно удаляет сущность по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности.</param>
        /// <returns>True, если удаление прошло успешно; иначе - false.</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Находит сущности по предикату.
        /// </summary>
        /// <param name="predicate">Предикат для фильтрации.</param>
        /// <returns>Запрос для получения сущностей.</returns>
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Получает сущность по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности.</param>
        /// <param name="includes">Навигационные свойства для включения.</param>
        /// <returns>Сущность или null, если не найдена.</returns>
        T Get(int id, params string[] includes);

        /// <summary>
        /// Асинхронно получает сущность по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности.</param>
        /// <param name="includes">Навигационные свойства для включения.</param>
        /// <returns>Сущность или null, если не найдена.</returns>
        Task<T> GetAsync(int id, params string[] includes);

        /// <summary>
        /// Получает все сущности.
        /// </summary>
        /// <returns>Запрос для получения всех сущностей.</returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Получает все сущности с пагинацией.
        /// </summary>
        /// <param name="pageNumber">Номер страницы.</param>
        /// <param name="pageSize">Размер страницы.</param>
        /// <returns>Запрос для получения всех сущностей.</returns>
        IQueryable<T> GetAll(int pageNumber, int pageSize);

        /// <summary>
        /// Получает все сущности, отсортированные по указанному полю.
        /// </summary>
        /// <param name="orderBy">Поле для сортировки.</param>
        /// <param name="ascending">True для сортировки по возрастанию; иначе - по убыванию.</param>
        /// <returns>Запрос для получения всех сущностей.</returns>
        IQueryable<T> GetAll(Expression<Func<T, object>> orderBy, bool ascending = true);

        /// <summary>
        /// Получает все сущности с фильтрацией и сортировкой.
        /// </summary>
        /// <param name="filter">Предикат для фильтрации.</param>
        /// <param name="orderBy">Поле для сортировки.</param>
        /// <param name="ascending">True для сортировки по возрастанию; иначе - по убыванию.</param>
        /// <param name="pageNumber">Номер страницы.</param>
        /// <param name="pageSize">Размер страницы.</param>
        /// <returns>Запрос для получения всех сущностей.</returns>
        IQueryable<T> GetAll(Expression<Func<T, bool>> filter, Expression<Func<T, object>> orderBy, bool ascending = true, int pageNumber = 1, int pageSize = 10);

        /// <summary>
        /// Обновляет существующую сущность.
        /// </summary>
        /// <param name="entity">Сущность для обновления.</param>
        void Update(T entity);

        /// <summary>
        /// Асинхронно обновляет существующую сущность.
        /// </summary>
        /// <param name="entity">Сущность для обновления.</param>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Асинхронно находит сущности по предикату.
        /// </summary>
        /// <param name="predicate">Предикат для фильтрации.</param>
        /// <returns>Список найденных сущностей.</returns>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Асинхронно находит первую сущность, соответствующую предикату.
        /// </summary>
        /// <param name="predicate">Предикат для фильтрации.</param>
        /// <returns>Первая найденная сущность или null, если не найдена.</returns>
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Удаляет сущность.
        /// </summary>
        /// <param name="entity">Сущность для удаления.</param>
        void Remove(T entity);

        /// <summary>
        /// Асинхронно удаляет сущность.
        /// </summary>
        /// <param name="entity">Сущность для удаления.</param>
        Task<bool> RemoveAsync(T entity);
    }
}