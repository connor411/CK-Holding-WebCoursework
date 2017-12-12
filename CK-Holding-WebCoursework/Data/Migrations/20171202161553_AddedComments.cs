using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CK_Holding_WebCoursework.Data.Migrations
{
    public partial class AddedComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "title",
                table: "Annoucements",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Annoucements",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "dateAndTimeOfPost",
                table: "Annoucements",
                newName: "DateAndTimeOfPost");

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    MyAnnoucementId = table.Column<int>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Annoucements_MyAnnoucementId",
                        column: x => x.MyAnnoucementId,
                        principalTable: "Annoucements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_MyAnnoucementId",
                table: "Comments",
                column: "MyAnnoucementId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Annoucements",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Annoucements",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "DateAndTimeOfPost",
                table: "Annoucements",
                newName: "dateAndTimeOfPost");
        }
    }
}
