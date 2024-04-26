﻿// <auto-generated />
using BlazorChat.Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BlazorChat.Migrations
{
    [DbContext(typeof(CourseDbContext))]
    [Migration("20240416114756_RemovedCourseOutline")]
    partial class RemovedCourseOutline
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BlazorChat.Models.Course", b =>
                {
                    b.Property<string>("CourseTitleSlug")
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("CourseTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LearningGoals")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NumberOfDays")
                        .HasColumnType("int");

                    b.Property<string>("RecommendationInfo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TargetAudience")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CourseTitleSlug");

                    b.ToTable("Courses");
                });
#pragma warning restore 612, 618
        }
    }
}