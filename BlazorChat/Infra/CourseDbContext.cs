using System;
using System.Collections.Generic;
using BlazorChat.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorChat.Infra;

public partial class CourseDbContext : DbContext
{
  public DbSet<Course> Courses => Set<Course>();
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
  }

  partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
