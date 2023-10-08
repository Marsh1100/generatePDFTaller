using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Repository;
using DinkToPdf.Contracts;
using Domain.Interfaces;
using Persistence;

namespace Application.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly GeneratorPDFDbContext _context;
    private IStudent _students;

    public UnitOfWork(GeneratorPDFDbContext context)
    {
        _context = context;
    }

    public IStudent Students {
        get
        {
            if (_students == null)
            {
                _students = new StudentRepository(_context);
            }
            return _students;
        }
    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}