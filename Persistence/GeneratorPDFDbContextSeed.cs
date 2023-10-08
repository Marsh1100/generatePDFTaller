using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Persistence;
   
public class GeneratorPDFDbContextSeed
{
    public static async Task SeedAsync(GeneratorPDFDbContext context, ILoggerFactory loggerFactory)
    {
        try
        {
            if(!context.Subjects.Any())
            {
                var subjects = new List<Subject>
                {
                    new() { NameSubject = "Music" },
                    new() { NameSubject = "Chemistry" },
                    new() { NameSubject = "Math" },
                    new() { NameSubject = "Biology" },
                    new() { NameSubject = "Physical" }

                };
                context.Subjects.AddRange(subjects);
                await context.SaveChangesAsync();
            }
            if(!context.Students.Any())
            {
                using (var reader = new StreamReader("../Persistence/Data/Csvs/students.csv"))
                {
                    using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HeaderValidated = null, // Esto deshabilita la validación de encabezados
                        MissingFieldFound = null
                    }))
                    {
                        // Resto de tu código para leer y procesar el archivo CSV
                        var list = csv.GetRecords<Student>();
                        List<Student> entidad = new List<Student>();
                        foreach (var item in list)
                        {
                            entidad.Add(new Student
                            {
                                Id = item.Id,
                                Profile = item.Profile,
                                StudentIdentification = item.StudentIdentification,
                                NameStudent = item.NameStudent,

                            });
                        }
                        context.Students.AddRange(entidad);
                        await context.SaveChangesAsync();
                    }
                }
                }

                if(!context.Notes.Any())
                {
                    using (var reader = new StreamReader("../Persistence/Data/Csvs/notes.csv"))
                    {
                        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                        {
                            HeaderValidated = null, // Esto deshabilita la validación de encabezados
                            MissingFieldFound = null
                        }))
                        {
                            // Resto de tu código para leer y procesar el archivo CSV
                            var list = csv.GetRecords<Notes>();
                            List<Notes> entidad = new List<Notes>();
                            foreach (var item in list)
                            {
                                entidad.Add(new Notes
                                {
                                    Id = item.Id,
                                    IdStudent = item.IdStudent,
                                    IdSubject = item.IdSubject,
                                    Note1 = item.Note1,
                                    Note2 = item.Note2,
                                    Note3 = item.Note3,
                                    Average = item.Average,
                                });
                            }
                            context.Notes.AddRange(entidad);
                            await context.SaveChangesAsync();
                        }
                    }
                    }

        }catch(Exception ex)
        {
            var logger = loggerFactory.CreateLogger<GeneratorPDFDbContext>();
            logger.LogError(ex.Message);
        }
    }
}