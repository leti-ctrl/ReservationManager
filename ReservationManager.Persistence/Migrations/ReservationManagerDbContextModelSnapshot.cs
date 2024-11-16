﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ReservationManager.Persistence;

#nullable disable

namespace ReservationManager.Persistence.Migrations
{
    [DbContext(typeof(ReservationManagerDbContext))]
    partial class ReservationManagerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ReservationManager.DomainModel.Meta.ReservationType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<TimeOnly>("End")
                        .HasColumnType("time without time zone");

                    b.Property<DateTime?>("IsDeleted")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<TimeOnly>("Start")
                        .HasColumnType("time without time zone");

                    b.HasKey("Id");

                    b.ToTable("ReservationType", (string)null);
                });

            modelBuilder.Entity("ReservationManager.DomainModel.Meta.ResourceType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<DateTime?>("IsDeleted")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("ResourceType", (string)null);
                });

            modelBuilder.Entity("ReservationManager.DomainModel.Meta.UserType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<DateTime?>("IsDeleted")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("UserType", (string)null);
                });

            modelBuilder.Entity("ReservationManager.DomainModel.Operation.ClosingCalendar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("Day")
                        .HasColumnType("date");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTime?>("IsDeleted")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ResourceId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ResourceId");

                    b.ToTable("ClosingCalendar", (string)null);
                });

            modelBuilder.Entity("ReservationManager.DomainModel.Operation.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("Day")
                        .HasColumnType("date");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<TimeOnly>("End")
                        .HasColumnType("time without time zone");

                    b.Property<DateTime?>("IsDeleted")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ResourceId")
                        .HasColumnType("integer");

                    b.Property<TimeOnly>("Start")
                        .HasColumnType("time without time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<int>("TypeId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ResourceId");

                    b.HasIndex("TypeId");

                    b.HasIndex("UserId");

                    b.ToTable("Reservations", (string)null);
                });

            modelBuilder.Entity("ReservationManager.DomainModel.Operation.Resource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<DateTime?>("IsDeleted")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("TypeId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TypeId");

                    b.ToTable("Resource", (string)null);
                });

            modelBuilder.Entity("ReservationManager.DomainModel.Operation.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime?>("IsDeleted")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("TypeId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TypeId");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("ReservationManager.DomainModel.Operation.ClosingCalendar", b =>
                {
                    b.HasOne("ReservationManager.DomainModel.Operation.Resource", "Resource")
                        .WithMany()
                        .HasForeignKey("ResourceId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Resource");
                });

            modelBuilder.Entity("ReservationManager.DomainModel.Operation.Reservation", b =>
                {
                    b.HasOne("ReservationManager.DomainModel.Operation.Resource", "Resource")
                        .WithMany()
                        .HasForeignKey("ResourceId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ReservationManager.DomainModel.Meta.ReservationType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ReservationManager.DomainModel.Operation.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Resource");

                    b.Navigation("Type");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ReservationManager.DomainModel.Operation.Resource", b =>
                {
                    b.HasOne("ReservationManager.DomainModel.Meta.ResourceType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Type");
                });

            modelBuilder.Entity("ReservationManager.DomainModel.Operation.User", b =>
                {
                    b.HasOne("ReservationManager.DomainModel.Meta.UserType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Type");
                });
#pragma warning restore 612, 618
        }
    }
}
