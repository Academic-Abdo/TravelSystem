﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TravelSystem;

#nullable disable

namespace TravelSystem.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240529071444_Init_Migration")]
    partial class Init_Migration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TravelSystem.Models.Bus", b =>
                {
                    b.Property<string>("Bus_Plate")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Bus_Plate");

                    b.ToTable("Buss");
                });

            modelBuilder.Entity("TravelSystem.Models.Clint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("DocumntPath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Clints");
                });

            modelBuilder.Entity("TravelSystem.Models.Driver", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Drivers");
                });

            modelBuilder.Entity("TravelSystem.Models.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ClintId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReservationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("TripId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClintId");

                    b.HasIndex("TripId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("TravelSystem.Models.Trip", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AvailableSeats")
                        .HasColumnType("int");

                    b.Property<int>("BusId")
                        .HasColumnType("int");

                    b.Property<string>("Bus_Plate")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("DriverId")
                        .HasColumnType("int");

                    b.Property<int>("ReservedSeats")
                        .HasColumnType("int");

                    b.Property<DateTime>("TripDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("TripKindId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Bus_Plate");

                    b.HasIndex("DriverId");

                    b.HasIndex("TripKindId");

                    b.ToTable("Trips");
                });

            modelBuilder.Entity("TravelSystem.Models.TripKind", b =>
                {
                    b.Property<int>("TripKindId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TripKindId"));

                    b.HasKey("TripKindId");

                    b.ToTable("TripKinds");
                });

            modelBuilder.Entity("TravelSystem.Models.Reservation", b =>
                {
                    b.HasOne("TravelSystem.Models.Clint", "Clint")
                        .WithMany("Reservations")
                        .HasForeignKey("ClintId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TravelSystem.Models.Trip", "Trip")
                        .WithMany()
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Clint");

                    b.Navigation("Trip");
                });

            modelBuilder.Entity("TravelSystem.Models.Trip", b =>
                {
                    b.HasOne("TravelSystem.Models.Bus", "Bus")
                        .WithMany("Trips")
                        .HasForeignKey("Bus_Plate");

                    b.HasOne("TravelSystem.Models.Driver", "Driver")
                        .WithMany("Trips")
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TravelSystem.Models.TripKind", "TripKind")
                        .WithMany("Trips")
                        .HasForeignKey("TripKindId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bus");

                    b.Navigation("Driver");

                    b.Navigation("TripKind");
                });

            modelBuilder.Entity("TravelSystem.Models.Bus", b =>
                {
                    b.Navigation("Trips");
                });

            modelBuilder.Entity("TravelSystem.Models.Clint", b =>
                {
                    b.Navigation("Reservations");
                });

            modelBuilder.Entity("TravelSystem.Models.Driver", b =>
                {
                    b.Navigation("Trips");
                });

            modelBuilder.Entity("TravelSystem.Models.TripKind", b =>
                {
                    b.Navigation("Trips");
                });
#pragma warning restore 612, 618
        }
    }
}
