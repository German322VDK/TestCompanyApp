﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TestCompanyApp.Database.Context;

#nullable disable

namespace TestCompanyApp.Database.Migrations
{
    [DbContext(typeof(TestCompanyAppDbContext))]
    [Migration("20241031072313_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true);

            modelBuilder.Entity("TestCompanyApp.Domain.Entity.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsEmployed")
                        .HasColumnType("INTEGER");

                    b.Property<int>("JobTitle")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("LeaderId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Patronymic")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SurName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("LeaderId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("TestCompanyApp.Domain.Entity.Employee", b =>
                {
                    b.HasOne("TestCompanyApp.Domain.Entity.Employee", "Leader")
                        .WithMany("Subordinates")
                        .HasForeignKey("LeaderId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Leader");
                });

            modelBuilder.Entity("TestCompanyApp.Domain.Entity.Employee", b =>
                {
                    b.Navigation("Subordinates");
                });
#pragma warning restore 612, 618
        }
    }
}