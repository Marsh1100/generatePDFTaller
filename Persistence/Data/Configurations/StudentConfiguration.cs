using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistence.Data.Configurations;
    public class StudentConfiguration: IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("student");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.StudentIdentification)
        .IsRequired();
        builder.Property(x => x.NameStudent)
        .IsRequired()
        .HasMaxLength(120);
        builder.Property(x=>x.Profile);
    }
}