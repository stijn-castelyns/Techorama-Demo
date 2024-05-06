using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using U2U_AI.Infra.Entities;

namespace U2U_AI.Infra;

public partial class CourseDbContext : DbContext
{
  public DbSet<Course> Courses => Set<Course>();
  public DbSet<Session> Sessions => Set<Session>();
  public DbSet<Booking> Bookings => Set<Booking>();
  public CourseDbContext()
  {
  }

  public CourseDbContext(DbContextOptions<CourseDbContext> options)
      : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    OnModelCreatingPartial(modelBuilder);

    modelBuilder.Entity<Course>()
      .HasKey(c => c.CourseTitleSlug);

    modelBuilder.Entity<Course>()
      .Property(c => c.CourseTitleSlug)
      .HasColumnType("VARCHAR(100)");

    var dateListConverter = new ValueConverter<List<DateOnly>, string>(
            v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
            v => JsonSerializer.Deserialize<List<DateOnly>>(v, new JsonSerializerOptions()));

    // Apply the converter to the OrganisationDays property
    modelBuilder
        .Entity<Session>()
        .Property(s => s.OrganisationDays)
        .HasConversion(dateListConverter)
        .HasColumnType("nvarchar(max)"); // Using nvarchar(max) to store JSON data

    modelBuilder
        .Entity<Session>()
        .HasOne(c => c.Course)
        .WithMany()
        .HasForeignKey(c => c.CourseId);

    modelBuilder
        .Entity<Session>()
        .HasData(
          [
            new Session() { Id = 1, CourseId = "building-aspnet-web-apis", OrganisationDays = [new DateOnly(2024, 5, 13), new DateOnly(2024, 5, 14), new DateOnly(2024, 5, 15), new DateOnly(2024, 5, 16), new DateOnly(2024, 5, 17)] },
            new Session() { Id = 2, CourseId = "building-aspnet-web-apis", OrganisationDays = [new DateOnly(2024, 5, 20), new DateOnly(2024, 5, 21), new DateOnly(2024, 5, 22), new DateOnly(2024, 5, 23), new DateOnly(2024, 5, 24)] },
          ]);

    modelBuilder
        .Entity<Booking>()
        .HasOne(b => b.Session)
        .WithMany(s => s.Bookings);
  }

  partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
