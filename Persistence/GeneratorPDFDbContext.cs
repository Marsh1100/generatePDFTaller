using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence;
    public class GeneratorPDFDbContext: DbContext
    {
        public GeneratorPDFDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Student> Students{ get; set; }
        public DbSet<Notes> Notes{ get; set; }
        public DbSet<Subject> Subjects{ get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
