using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Persistence.Data.Configurations;
public class NotesConfiguration : IEntityTypeConfiguration<Notes>
{
    public void Configure(EntityTypeBuilder<Notes> builder)
    {
        builder.ToTable("notes");
   
        builder.Property(x=>x.Note1)
        .IsRequired();
        builder.Property(x=>x.Note2)
        .IsRequired();
        builder.Property(x=>x.Note3)
        .IsRequired();
        builder.Property(x=>x.Average)
        .IsRequired();
        
        builder.HasOne(x=>x.Student)
        .WithMany(s => s.Notes)
        .HasForeignKey(x=>x.IdStudent);
        
        builder.HasOne(x=>x.Subject)
        .WithMany(s => s.Notes)
        .HasForeignKey(x=>x.IdSubject);

    }
}