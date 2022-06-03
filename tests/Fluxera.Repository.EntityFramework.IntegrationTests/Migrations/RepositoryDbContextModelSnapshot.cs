﻿// <auto-generated />
using System;
using Fluxera.Repository.EntityFrameworkCore.IntegrationTests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Fluxera.Repository.EntityFrameworkCore.IntegrationTests.Migrations
{
    [DbContext(typeof(RepositoryDbContext))]
    partial class RepositoryDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.5");

            modelBuilder.Entity("Fluxera.Repository.UnitTests.Core.CompanyAggregate.Company", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("Guid")
                        .HasColumnType("TEXT");

                    b.Property<string>("LegalType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("NullableGuid")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("Fluxera.Repository.UnitTests.Core.EmployeeAggregate.Employee", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<double>("SalaryDecimal")
                        .HasColumnType("REAL");

                    b.Property<double>("SalaryDouble")
                        .HasColumnType("REAL");

                    b.Property<float>("SalaryFloat")
                        .HasColumnType("REAL");

                    b.Property<int>("SalaryInt")
                        .HasColumnType("INTEGER");

                    b.Property<long>("SalaryLong")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("SalaryNullableDecimal")
                        .HasColumnType("REAL");

                    b.Property<double?>("SalaryNullableDouble")
                        .HasColumnType("REAL");

                    b.Property<float?>("SalaryNullableFloat")
                        .HasColumnType("REAL");

                    b.Property<int?>("SalaryNullableInt")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("SalaryNullableLong")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Fluxera.Repository.UnitTests.Core.PersonAggregate.Person", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("Age")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("People", (string)null);
                });

            modelBuilder.Entity("Fluxera.Repository.UnitTests.Core.PersonAggregate.Person", b =>
                {
                    b.OwnsOne("Fluxera.Repository.UnitTests.Core.PersonAggregate.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("PersonID")
                                .HasColumnType("TEXT");

                            b1.Property<string>("City")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Number")
                                .HasColumnType("TEXT");

                            b1.Property<string>("PostCode")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Street")
                                .HasColumnType("TEXT");

                            b1.HasKey("PersonID");

                            b1.ToTable("People");

                            b1.WithOwner()
                                .HasForeignKey("PersonID");
                        });

                    b.Navigation("Address");
                });
#pragma warning restore 612, 618
        }
    }
}
