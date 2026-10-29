using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Health_Care_System.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAuditFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Appointments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "Patients",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "Doctors",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedDate",
                table: "Appointments",
                type: "datetimeoffset",
                nullable: true);
        }
    }
}
