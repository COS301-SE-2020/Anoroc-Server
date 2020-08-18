﻿// <auto-generated />
using System;
using Anoroc_User_Management.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Anoroc_User_Management.Migrations
{
    [DbContext(typeof(AnorocDbContext))]
    partial class AnorocDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("Anoroc_User_Management.Models.ItineraryFolder.PrimitiveItineraryRisk", b =>
                {
                    b.Property<string>("AccessToken")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("LocationItineraryRisks")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalItineraryRisk")
                        .HasColumnType("int");

                    b.HasKey("AccessToken");

                    b.ToTable("ItineraryRisks");
                });

            modelBuilder.Entity("Anoroc_User_Management.Models.Location", b =>
                {
                    b.Property<long>("Location_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccessToken")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("Carrier_Data_Point")
                        .HasColumnType("bit");

                    b.Property<long?>("ClusterReferenceID")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<long?>("Old_ClusterReferenceID")
                        .HasColumnType("bigint");

                    b.Property<long>("RegionArea_ID")
                        .HasColumnType("bigint");

                    b.HasKey("Location_ID");

                    b.HasIndex("AccessToken")
                        .IsUnique()
                        .HasFilter("[AccessToken] IS NOT NULL");

                    b.HasIndex("ClusterReferenceID");

                    b.HasIndex("Old_ClusterReferenceID");

                    b.HasIndex("RegionArea_ID");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("Anoroc_User_Management.Models.OldCluster", b =>
                {
                    b.Property<long>("Old_Cluster_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("Center_LocationLocation_ID")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Cluster_Created")
                        .HasColumnType("datetime2");

                    b.Property<double>("Cluster_Radius")
                        .HasColumnType("float");

                    b.HasKey("Old_Cluster_Id");

                    b.HasIndex("Center_LocationLocation_ID");

                    b.ToTable("OldClusters");
                });

            modelBuilder.Entity("Anoroc_User_Management.Models.OldLocation", b =>
                {
                    b.Property<long>("OldLocation_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccessToken")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("AreaReferenceID")
                        .HasColumnType("bigint");

                    b.Property<bool>("Carrier_Data_Point")
                        .HasColumnType("bit");

                    b.Property<long?>("Cluster_Id")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<long?>("Old_ClusterReferenceID")
                        .HasColumnType("bigint");

                    b.Property<long?>("RegionArea_ID")
                        .HasColumnType("bigint");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OldLocation_ID");

                    b.HasIndex("AccessToken")
                        .IsUnique()
                        .HasFilter("[AccessToken] IS NOT NULL");

                    b.HasIndex("Cluster_Id");

                    b.HasIndex("RegionArea_ID");

                    b.ToTable("OldLocations");
                });

            modelBuilder.Entity("Anoroc_User_Management.Models.User", b =>
                {
                    b.Property<string>("AccessToken")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Firebase_Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserID")
                        .HasColumnType("bigint");

                    b.Property<string>("UserSurname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("carrierStatus")
                        .HasColumnType("bit");

                    b.Property<bool>("currentlyLoggedIn")
                        .HasColumnType("bit");

                    b.Property<bool>("loggedInAnoroc")
                        .HasColumnType("bit");

                    b.Property<bool>("loggedInFacebook")
                        .HasColumnType("bit");

                    b.Property<bool>("loggedInGoogle")
                        .HasColumnType("bit");

                    b.HasKey("AccessToken");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Anoroc_User_Management.Services.Cluster", b =>
                {
                    b.Property<long>("Cluster_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("Center_LocationLocation_ID")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Cluster_Created")
                        .HasColumnType("datetime2");

                    b.Property<double>("Cluster_Radius")
                        .HasColumnType("float");

                    b.HasKey("Cluster_Id");

                    b.HasIndex("Center_LocationLocation_ID");

                    b.ToTable("Clusters");
                });

            modelBuilder.Entity("Anoroc_User_Management.Models.ItineraryFolder.PrimitiveItineraryRisk", b =>
                {
                    b.HasOne("Anoroc_User_Management.Models.User", "User")
                        .WithOne("PrimitiveItineraryRisk")
                        .HasForeignKey("Anoroc_User_Management.Models.ItineraryFolder.PrimitiveItineraryRisk", "AccessToken")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Anoroc_User_Management.Models.Location", b =>
                {
                    b.HasOne("Anoroc_User_Management.Models.User", "User")
                        .WithOne("Location")
                        .HasForeignKey("Anoroc_User_Management.Models.Location", "AccessToken");

                    b.HasOne("Anoroc_User_Management.Services.Cluster", "Cluster")
                        .WithMany("Coordinates")
                        .HasForeignKey("ClusterReferenceID");

                    b.HasOne("Anoroc_User_Management.Models.OldCluster", null)
                        .WithMany("Coordinates")
                        .HasForeignKey("Old_ClusterReferenceID");

                    b.HasOne("Anoroc_User_Management.Models.Area", "Region")
                        .WithMany()
                        .HasForeignKey("RegionArea_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Anoroc_User_Management.Models.OldCluster", b =>
                {
                    b.HasOne("Anoroc_User_Management.Models.Location", "Center_Location")
                        .WithMany()
                        .HasForeignKey("Center_LocationLocation_ID");
                });

            modelBuilder.Entity("Anoroc_User_Management.Models.OldLocation", b =>
                {
                    b.HasOne("Anoroc_User_Management.Models.User", "User")
                        .WithOne("OldLocation")
                        .HasForeignKey("Anoroc_User_Management.Models.OldLocation", "AccessToken");

                    b.HasOne("Anoroc_User_Management.Services.Cluster", "Cluster")
                        .WithMany()
                        .HasForeignKey("Cluster_Id");

                    b.HasOne("Anoroc_User_Management.Models.Area", "Region")
                        .WithMany()
                        .HasForeignKey("RegionArea_ID");
                });

            modelBuilder.Entity("Anoroc_User_Management.Services.Cluster", b =>
                {
                    b.HasOne("Anoroc_User_Management.Models.Location", "Center_Location")
                        .WithMany()
                        .HasForeignKey("Center_LocationLocation_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
