using BookingSystem.DAL.Data;
using BookingSystem.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

public class OfficeService
{
    private readonly BookingContext _context;

    public OfficeService(BookingContext context)
    {
        _context = context;
    }

    public void AddOffice(Office office)
    {
        if (office == null) throw new ArgumentNullException(nameof(office));
        _context.Offices.Add(office);
        _context.SaveChanges();
    }

    public IEnumerable<Office> GetAllOffices()
    {
        return _context.Offices.ToList();
    }

    public Office GetOfficeById(int id)
    {
        return _context.Offices.Find(id);
    }
}