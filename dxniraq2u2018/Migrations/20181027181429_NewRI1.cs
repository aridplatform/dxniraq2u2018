using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace dxniraq2u2018.Migrations
{
    public partial class NewRI1 : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                          name: "RegisterIntention",
                          columns: table => new
                          {
                              Id = table.Column<int>(nullable: false)
                                  .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                              ApplicationUserId = table.Column<string>(nullable: true)
                          },
                          constraints: table =>
                          {
                              table.PrimaryKey("PK_RegisterIntention", x => x.Id);
                              table.ForeignKey(
                                  name: "FK_RegisterIntention_AspNetUsers_ApplicationUserId",
                                  column: x => x.ApplicationUserId,
                                  principalTable: "AspNetUsers",
                                  principalColumn: "Id",
                                  onDelete: ReferentialAction.Restrict);
                          });


            migrationBuilder.CreateIndex(
                         name: "IX_RegisterIntention_ApplicationUserId",
                         table: "RegisterIntention",
                         column: "ApplicationUserId");
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
              name: "RegisterIntention");
        }

       
    }
}
