﻿// <auto-generated />
using System;
using AngularApp2.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AngularApp2.Server.Migrations
{
    [DbContext(typeof(ApiTeleContext))]
    [Migration("20240920042333_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AngularApp2.Server.Models.ListMessageTable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ListMessageTables");
                });

            modelBuilder.Entity("AngularApp2.Server.Models.MessageTable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("media_path")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("media_type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("message_content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("message_id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("message_type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("sender_id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("sender_name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("timestamp")
                        .HasColumnType("datetime2");

                    b.Property<long?>("user_id")
                        .HasColumnType("bigint");

                    b.Property<string>("user_name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MessageTables");
                });
#pragma warning restore 612, 618
        }
    }
}