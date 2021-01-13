﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using dotnet5_webapp.Data;

namespace dotnet5_webapp.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("dotnet5_webapp.Models.Contact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NickName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Place")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("dotnet5_webapp.Models.Minigame", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("MinigameId")
                        .HasColumnType("int");

                    b.Property<int>("Rank")
                        .HasColumnType("int");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.Property<int>("StatRecordId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StatRecordId");

                    b.ToTable("Minigame");
                });

            modelBuilder.Entity("dotnet5_webapp.Models.Skill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<int>("Rank")
                        .HasColumnType("int");

                    b.Property<int>("SkillId")
                        .HasColumnType("int");

                    b.Property<int>("StatRecordId")
                        .HasColumnType("int");

                    b.Property<long>("Xp")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("StatRecordId");

                    b.ToTable("Skill");
                });

            modelBuilder.Entity("dotnet5_webapp.Models.StatRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("StatRecord");
                });

            modelBuilder.Entity("dotnet5_webapp.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("DisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("dotnet5_webapp.Models.Minigame", b =>
                {
                    b.HasOne("dotnet5_webapp.Models.StatRecord", null)
                        .WithMany("Minigames")
                        .HasForeignKey("StatRecordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("dotnet5_webapp.Models.Skill", b =>
                {
                    b.HasOne("dotnet5_webapp.Models.StatRecord", null)
                        .WithMany("Skills")
                        .HasForeignKey("StatRecordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("dotnet5_webapp.Models.StatRecord", b =>
                {
                    b.HasOne("dotnet5_webapp.Models.User", null)
                        .WithMany("StatRecords")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("dotnet5_webapp.Models.StatRecord", b =>
                {
                    b.Navigation("Minigames");

                    b.Navigation("Skills");
                });

            modelBuilder.Entity("dotnet5_webapp.Models.User", b =>
                {
                    b.Navigation("StatRecords");
                });
#pragma warning restore 612, 618
        }
    }
}
