﻿// <auto-generated />
using System;
using Fluxera.Repository.EntityFrameworkCore.IntegrationTests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
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
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Fluxera.Repository.EntityFrameworkCore.IntegrationTests.InvoiceAggregate.Invoice", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CustomerName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("Fluxera.Repository.EntityFrameworkCore.IntegrationTests.InvoiceAggregate.InvoiceItem", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<string>("InvoiceID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("InvoiceID");

                    b.ToTable("InvoiceItem");
                });

            modelBuilder.Entity("Fluxera.Repository.UnitTests.Core.CompanyAggregate.Company", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("Guid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LegalType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("NullableGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ReferenceID")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ID");

                    b.HasIndex("ReferenceID");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("Fluxera.Repository.UnitTests.Core.EmployeeAggregate.Employee", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReferenceID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("SalaryDecimal")
                        .HasColumnType("float");

                    b.Property<double>("SalaryDouble")
                        .HasColumnType("float");

                    b.Property<float>("SalaryFloat")
                        .HasColumnType("real");

                    b.Property<int>("SalaryInt")
                        .HasColumnType("int");

                    b.Property<long>("SalaryLong")
                        .HasColumnType("bigint");

                    b.Property<double?>("SalaryNullableDecimal")
                        .HasColumnType("float");

                    b.Property<double?>("SalaryNullableDouble")
                        .HasColumnType("float");

                    b.Property<float?>("SalaryNullableFloat")
                        .HasColumnType("real");

                    b.Property<int?>("SalaryNullableInt")
                        .HasColumnType("int");

                    b.Property<long?>("SalaryNullableLong")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.HasIndex("ReferenceID");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Fluxera.Repository.UnitTests.Core.PersonAggregate.Person", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReferenceID")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ID");

                    b.HasIndex("ReferenceID");

                    b.ToTable("People", (string)null);
                });

            modelBuilder.Entity("Fluxera.Repository.UnitTests.Core.ReferenceAggregate.Reference", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CompanyID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("EmployeeID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid?>("PersonID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ID");

                    b.HasIndex("CompanyID");

                    b.HasIndex("EmployeeID");

                    b.HasIndex("PersonID");

                    b.ToTable("References");
                });

            modelBuilder.Entity("Fluxera.Repository.EntityFrameworkCore.IntegrationTests.InvoiceAggregate.InvoiceItem", b =>
                {
                    b.HasOne("Fluxera.Repository.EntityFrameworkCore.IntegrationTests.InvoiceAggregate.Invoice", "Invoice")
                        .WithMany("InvoiceItems")
                        .HasForeignKey("InvoiceID");

                    b.Navigation("Invoice");
                });

            modelBuilder.Entity("Fluxera.Repository.UnitTests.Core.CompanyAggregate.Company", b =>
                {
                    b.HasOne("Fluxera.Repository.UnitTests.Core.ReferenceAggregate.Reference", null)
                        .WithMany("Companies")
                        .HasForeignKey("ReferenceID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Fluxera.Repository.UnitTests.Core.EmployeeAggregate.Employee", b =>
                {
                    b.HasOne("Fluxera.Repository.UnitTests.Core.ReferenceAggregate.Reference", null)
                        .WithMany("Employees")
                        .HasForeignKey("ReferenceID")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Fluxera.Repository.UnitTests.Core.PersonAggregate.Person", b =>
                {
                    b.HasOne("Fluxera.Repository.UnitTests.Core.ReferenceAggregate.Reference", null)
                        .WithMany("People")
                        .HasForeignKey("ReferenceID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.OwnsOne("Fluxera.Repository.UnitTests.Core.PersonAggregate.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("PersonID")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("City")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Number")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("PostCode")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Street")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("PersonID");

                            b1.ToTable("People");

                            b1.WithOwner()
                                .HasForeignKey("PersonID");
                        });

                    b.Navigation("Address");
                });

            modelBuilder.Entity("Fluxera.Repository.UnitTests.Core.ReferenceAggregate.Reference", b =>
                {
                    b.HasOne("Fluxera.Repository.UnitTests.Core.CompanyAggregate.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Fluxera.Repository.UnitTests.Core.EmployeeAggregate.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Fluxera.Repository.UnitTests.Core.PersonAggregate.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Company");

                    b.Navigation("Employee");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("Fluxera.Repository.EntityFrameworkCore.IntegrationTests.InvoiceAggregate.Invoice", b =>
                {
                    b.Navigation("InvoiceItems");
                });

            modelBuilder.Entity("Fluxera.Repository.UnitTests.Core.ReferenceAggregate.Reference", b =>
                {
                    b.Navigation("Companies");

                    b.Navigation("Employees");

                    b.Navigation("People");
                });
#pragma warning restore 612, 618
        }
    }
}
