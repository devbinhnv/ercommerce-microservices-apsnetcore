﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ordering.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Orderaddstatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Order");
        }
    }
}
