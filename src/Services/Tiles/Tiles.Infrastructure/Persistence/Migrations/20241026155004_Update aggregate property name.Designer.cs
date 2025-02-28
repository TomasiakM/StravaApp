﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tiles.Infrastructure.Persistence;

#nullable disable

namespace Tiles.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(ServiceDbContext))]
    [Migration("20241026155004_Update aggregate property name")]
    partial class Updateaggregatepropertyname
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.17")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Tiles.Domain.Aggregates.ActivityTiles.ActivityTilesAggregate", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("NewSquare")
                        .HasColumnType("int");

                    b.Property<long>("StravaActivityId")
                        .HasColumnType("bigint");

                    b.Property<long>("StravaUserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("StravaActivityId")
                        .IsUnique();

                    b.ToTable("ActivityTiles", (string)null);
                });

            modelBuilder.Entity("Tiles.Domain.Aggregates.Coordinates.CoordinatesAggregate", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LatLngs")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("StravaActivityId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("StravaActivityId")
                        .IsUnique();

                    b.ToTable("Coordinates", (string)null);
                });

            modelBuilder.Entity("Tiles.Domain.Aggregates.ActivityTiles.ActivityTilesAggregate", b =>
                {
                    b.OwnsMany("Tiles.Domain.Aggregates.ActivityTiles.ValueObjects.NewClusterTile", "NewClusterTiles", b1 =>
                        {
                            b1.Property<Guid>("ActivityTilesId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("X")
                                .HasColumnType("int");

                            b1.Property<int>("Y")
                                .HasColumnType("int");

                            b1.Property<int>("Z")
                                .HasColumnType("int");

                            b1.HasKey("ActivityTilesId", "X", "Y", "Z");

                            b1.ToTable("NewClusterTiles", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("ActivityTilesId");
                        });

                    b.OwnsMany("Tiles.Domain.Aggregates.ActivityTiles.ValueObjects.NewTile", "NewTiles", b1 =>
                        {
                            b1.Property<Guid>("ActivityTilesId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("X")
                                .HasColumnType("int");

                            b1.Property<int>("Y")
                                .HasColumnType("int");

                            b1.Property<int>("Z")
                                .HasColumnType("int");

                            b1.HasKey("ActivityTilesId", "X", "Y", "Z");

                            b1.ToTable("NewTiles", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("ActivityTilesId");
                        });

                    b.OwnsMany("Tiles.Domain.Aggregates.ActivityTiles.ValueObjects.Tile", "Tiles", b1 =>
                        {
                            b1.Property<Guid>("ActivityTilesId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("X")
                                .HasColumnType("int");

                            b1.Property<int>("Y")
                                .HasColumnType("int");

                            b1.Property<int>("Z")
                                .HasColumnType("int");

                            b1.HasKey("ActivityTilesId", "X", "Y", "Z");

                            b1.ToTable("Tiles", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("ActivityTilesId");
                        });

                    b.Navigation("NewClusterTiles");

                    b.Navigation("NewTiles");

                    b.Navigation("Tiles");
                });
#pragma warning restore 612, 618
        }
    }
}
