﻿// <auto-generated />
using System;
using Anoroc_User_Management.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Anoroc_User_Management.Migrations
{
    [DbContext(typeof(AnorocDbContext))]
    [Migration("20200729094321_AddModelEntities")]
    partial class AddModelEntities
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Anoroc_User_Management.Models.Area", b =>
                {
                    b.Property<long>("Area_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Province")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Suburb")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Area_ID");

                    b.ToTable("Areas");
                });

            modelBuilder.Entity("Anoroc_User_Management.Models.User", b =>
                {
                    b.Property<long>("User_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Access_Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Anoroc_Log_In")
                        .HasColumnType("bit");

                    b.Property<bool>("Carrier_Status")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Facebook_Log_In")
                        .HasColumnType("bit");

                    b.Property<string>("Firebase_Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("First_Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Google_Log_In")
                        .HasColumnType("bit");

                    b.Property<string>("Last_Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("User_ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Anoroc_User_Management.Services.Cluster", b =>
                {
                    b.Property<long>("Cluster_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Cluster_Created")
                        .HasColumnType("datetime2");

                    b.Property<double>("Cluster_Radius")
                        .HasColumnType("float");

                    b.HasKey("Cluster_ID");

                    b.ToTable("Clusters");
                });

            modelBuilder.Entity("GeoCoordinatePortable.GeoCoordinate", b =>
                {
                    b.Property<double>("Altitude")
                        .HasColumnType("float");

                    b.Property<double>("Course")
                        .HasColumnType("float");

                    b.Property<double>("HorizontalAccuracy")
                        .HasColumnType("float");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<double>("Speed")
                        .HasColumnType("float");

                    b.Property<double>("VerticalAccuracy")
                        .HasColumnType("float");

                    b.ToTable("GeoCoordinate");
                });
#pragma warning restore 612, 618
        }
    }
}
