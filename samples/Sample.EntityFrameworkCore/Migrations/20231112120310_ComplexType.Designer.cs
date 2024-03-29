﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sample.EntityFrameworkCore;

#nullable disable

namespace Sample.EntityFrameworkCore.Migrations
{
    [DbContext(typeof(SampleDbContext))]
    [Migration("20231112120310_ComplexType")]
    partial class ComplexType
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Sample.Domain.Company.Company", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CompanyID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LegalType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("CompanyID");

                    b.ToTable("Companies", (string)null);
                });

            modelBuilder.Entity("Sample.Domain.Company.Company", b =>
                {
                    b.HasOne("Sample.Domain.Company.Company", null)
                        .WithMany("Partners")
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.OwnsOne("Sample.Domain.Company.Address", "Address", b1 =>
                        {
                            b1.Property<string>("CompanyID")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Number")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("CompanyID");

                            b1.ToTable("Companies");

                            b1.WithOwner()
                                .HasForeignKey("CompanyID");
                        });

                    b.Navigation("Address");
                });

            modelBuilder.Entity("Sample.Domain.Company.Company", b =>
                {
                    b.Navigation("Partners");
                });
#pragma warning restore 612, 618
        }
    }
}
