﻿// <auto-generated />
using System;
using Achievements.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Achievements.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ServiceDbContext))]
    [Migration("20240804192529_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.17")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Achievements.Domain.Aggregates.Achievement.Achievement", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AchievementType")
                        .HasColumnType("int");

                    b.Property<long>("StravaUserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("StravaUserId", "AchievementType")
                        .IsUnique();

                    b.ToTable("Achievements", (string)null);

                    b.HasDiscriminator<int>("AchievementType");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Achievements.Domain.Aggregates.Achievement.AchevementTypes.DistanceAchievements.CumulativeDistanceAchievement", b =>
                {
                    b.HasBaseType("Achievements.Domain.Aggregates.Achievement.Achievement");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("Achievements.Domain.Aggregates.Achievement.AchevementTypes.DistanceAchievements.MaxDistanceAchievement", b =>
                {
                    b.HasBaseType("Achievements.Domain.Aggregates.Achievement.Achievement");

                    b.HasDiscriminator().HasValue(2);
                });

            modelBuilder.Entity("Achievements.Domain.Aggregates.Achievement.AchevementTypes.DistanceAchievements.MonthlyCumulativeDistanceAchievement", b =>
                {
                    b.HasBaseType("Achievements.Domain.Aggregates.Achievement.Achievement");

                    b.HasDiscriminator().HasValue(3);
                });

            modelBuilder.Entity("Achievements.Domain.Aggregates.Achievement.AchevementTypes.DistanceAchievements.YearlyCumulativeDistanceAchievement", b =>
                {
                    b.HasBaseType("Achievements.Domain.Aggregates.Achievement.Achievement");

                    b.HasDiscriminator().HasValue(4);
                });

            modelBuilder.Entity("Achievements.Domain.Aggregates.Achievement.Achievement", b =>
                {
                    b.OwnsMany("Achievements.Domain.Aggregates.Achievement.Entities.AchievementLevel", "AchievementLevels", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"));

                            b1.Property<DateTimeOffset>("AchievedAt")
                                .HasColumnType("datetimeoffset");

                            b1.Property<Guid>("AchievementId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Level")
                                .HasColumnType("int");

                            b1.HasKey("Id");

                            b1.HasIndex("AchievementId");

                            b1.ToTable("AchievementLevels", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("AchievementId");
                        });

                    b.Navigation("AchievementLevels");
                });
#pragma warning restore 612, 618
        }
    }
}
