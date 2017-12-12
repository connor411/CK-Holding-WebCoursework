using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CK_Holding_WebCoursework.Data.Migrations
{
    public partial class Image1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageName",
                table: "Annoucements",
                newName: "ImageLocation");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageLocation",
                table: "Annoucements",
                newName: "ImageName");
        }
    }
}
