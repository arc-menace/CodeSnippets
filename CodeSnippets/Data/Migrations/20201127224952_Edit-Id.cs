using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeSnippets.Data.Migrations
{
    public partial class EditId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SnippetTag_Snippets_SnippetsId",
                table: "SnippetTag");

            migrationBuilder.DropForeignKey(
                name: "FK_SnippetTag_Tags_TagsId",
                table: "SnippetTag");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Tags",
                newName: "TagId");

            migrationBuilder.RenameColumn(
                name: "TagsId",
                table: "SnippetTag",
                newName: "TagsTagId");

            migrationBuilder.RenameColumn(
                name: "SnippetsId",
                table: "SnippetTag",
                newName: "SnippetsSnippetId");

            migrationBuilder.RenameIndex(
                name: "IX_SnippetTag_TagsId",
                table: "SnippetTag",
                newName: "IX_SnippetTag_TagsTagId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Snippets",
                newName: "SnippetId");

            migrationBuilder.AddForeignKey(
                name: "FK_SnippetTag_Snippets_SnippetsSnippetId",
                table: "SnippetTag",
                column: "SnippetsSnippetId",
                principalTable: "Snippets",
                principalColumn: "SnippetId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SnippetTag_Tags_TagsTagId",
                table: "SnippetTag",
                column: "TagsTagId",
                principalTable: "Tags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SnippetTag_Snippets_SnippetsSnippetId",
                table: "SnippetTag");

            migrationBuilder.DropForeignKey(
                name: "FK_SnippetTag_Tags_TagsTagId",
                table: "SnippetTag");

            migrationBuilder.RenameColumn(
                name: "TagId",
                table: "Tags",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "TagsTagId",
                table: "SnippetTag",
                newName: "TagsId");

            migrationBuilder.RenameColumn(
                name: "SnippetsSnippetId",
                table: "SnippetTag",
                newName: "SnippetsId");

            migrationBuilder.RenameIndex(
                name: "IX_SnippetTag_TagsTagId",
                table: "SnippetTag",
                newName: "IX_SnippetTag_TagsId");

            migrationBuilder.RenameColumn(
                name: "SnippetId",
                table: "Snippets",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SnippetTag_Snippets_SnippetsId",
                table: "SnippetTag",
                column: "SnippetsId",
                principalTable: "Snippets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SnippetTag_Tags_TagsId",
                table: "SnippetTag",
                column: "TagsId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
