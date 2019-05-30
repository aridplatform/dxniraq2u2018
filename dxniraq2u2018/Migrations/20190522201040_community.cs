using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace dxniraq2u2018.Migrations
{
    public partial class community : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
         
            migrationBuilder.CreateTable(
                name: "Communities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApplicationUserId = table.Column<string>(maxLength: 450, nullable: true),
                    BgImage = table.Column<string>(maxLength: 100, nullable: true),
                    CommunityType = table.Column<int>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsApproved = table.Column<bool>(nullable: false),
                    IsCommentsAllowed = table.Column<bool>(nullable: false),
                    IsFeatured = table.Column<bool>(nullable: false),
                    IsSuspended = table.Column<bool>(nullable: false),
                    Logo = table.Column<string>(maxLength: 100, nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    SecurityLevel = table.Column<int>(nullable: false),
                    ShortName = table.Column<string>(maxLength: 50, nullable: false),
                    SpecialityId = table.Column<int>(nullable: false),
                    Tags = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Communities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Communities_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CommunityFollowers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ApplicationUserId = table.Column<string>(maxLength: 450, nullable: true),
                    CommunityId = table.Column<int>(nullable: false),
                    IsAdmin = table.Column<bool>(nullable: false),
                    NotifyMe = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityFollowers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunityFollowers_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommunityFollowers_Communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Communities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ApplicationUserId = table.Column<string>(maxLength: 450, nullable: true),
                    Body = table.Column<string>(maxLength: 5000, nullable: false),
                    CommunityId = table.Column<int>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    File = table.Column<string>(maxLength: 100, nullable: true),
                    Image = table.Column<string>(maxLength: 100, nullable: true),
                    IsApproved = table.Column<bool>(nullable: false),
                    IsCommentsAllowed = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsFeatured = table.Column<bool>(nullable: false),
                    IsGifted = table.Column<bool>(nullable: false),
                    IsHidden = table.Column<bool>(nullable: false),
                    IsPublishRequest = table.Column<bool>(nullable: false),
                    PostType = table.Column<int>(nullable: false),
                    PublishRequestStatus = table.Column<bool>(nullable: false),
                    Reads = table.Column<int>(nullable: false),
                    Tags = table.Column<string>(maxLength: 100, nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Posts_Communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Communities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostComments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(maxLength: 2000, nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    File = table.Column<string>(maxLength: 100, nullable: true),
                    IsBestAnswer = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsFeatured = table.Column<bool>(nullable: false),
                    IsHidden = table.Column<bool>(nullable: false),
                    PostId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostComments_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostComments_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostMetrics",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: false),
                    NotifyMe = table.Column<bool>(nullable: false),
                    PostId = table.Column<Guid>(nullable: false),
                    ReportType = table.Column<int>(nullable: false),
                    VoteValue = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostMetrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostMetrics_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostMetrics_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostRevisions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Body = table.Column<string>(maxLength: 5000, nullable: false),
                    EditorDateTime = table.Column<DateTime>(nullable: false),
                    EditorUserId = table.Column<string>(maxLength: 450, nullable: true),
                    PostId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostRevisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostRevisions_AspNetUsers_EditorUserId",
                        column: x => x.EditorUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostRevisions_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentMetrics",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    PostCommentId = table.Column<Guid>(nullable: false),
                    ReportType = table.Column<int>(nullable: false),
                    VoteValue = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentMetrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentMetrics_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommentMetrics_PostComments_PostCommentId",
                        column: x => x.PostCommentId,
                        principalTable: "PostComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentMetrics_ApplicationUserId",
                table: "CommentMetrics",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentMetrics_PostCommentId",
                table: "CommentMetrics",
                column: "PostCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Communities_ApplicationUserId",
                table: "Communities",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityFollowers_ApplicationUserId",
                table: "CommunityFollowers",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityFollowers_CommunityId",
                table: "CommunityFollowers",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_PostComments_ApplicationUserId",
                table: "PostComments",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostComments_PostId",
                table: "PostComments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostMetrics_ApplicationUserId",
                table: "PostMetrics",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostMetrics_PostId",
                table: "PostMetrics",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostRevisions_EditorUserId",
                table: "PostRevisions",
                column: "EditorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PostRevisions_PostId",
                table: "PostRevisions",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_ApplicationUserId",
                table: "Posts",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CommunityId",
                table: "Posts",
                column: "CommunityId");
        }

   
    }
}
