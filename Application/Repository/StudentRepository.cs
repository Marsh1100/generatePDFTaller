using System;
using System.IO;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Persistence;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Application.Repository;

public class StudentRepository : GenericRepository<Student>, IStudent
{
    private readonly GeneratorPDFDbContext _context;
        public StudentRepository(GeneratorPDFDbContext context) : base(context)
        {
            _context = context;
        }
    public override async Task<Student> GetByIdAsync(int id)
    {
        return await _context.Students
            .Include(p => p.Notes).ThenInclude(p => p.Subject)
            .FirstOrDefaultAsync(p => p.Id == id);

    }
    public override async Task<IEnumerable<Student>> GetAllAsync()
    {
        return await _context.Students
            .Include(p => p.Notes).ThenInclude(p => p.Subject)
            .ToListAsync();

    }

    public async Task<Dictionary<string, double>> GetBestAverages()
    {
        var students = await _context.Students.ToListAsync();
        var notes = await _context.Notes.ToListAsync();
        var studentsAverage = (from student in students
                                join note in notes on student.Id equals note.IdStudent
                            select note)
                            .GroupBy(w=> w.IdStudent)
                            .Select(s=> new{

                                NameStudent = s.Select(d=> d.Student.NameStudent).FirstOrDefault(),
                                AverageTotal = Math.Truncate(s.Sum(a=> a.Average)/s.Select(f=> f.IdStudent).Count()*100)/100
                            })
                            .OrderByDescending(o=> o.AverageTotal).Take(3);
        
        Dictionary<string,double> dicAverage = new();
        foreach (var st in  studentsAverage)
        {
            dicAverage[st.NameStudent] = st.AverageTotal;
        }
        
        return dicAverage;
    }
}


