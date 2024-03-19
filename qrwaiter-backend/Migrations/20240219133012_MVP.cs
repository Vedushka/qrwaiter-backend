using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace qrwaiter_backend.Migrations
{
    /// <inheritdoc />
    public partial class MVP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_QrCodes",
                table: "QrCodes");

            migrationBuilder.RenameTable(
                name: "QrCodes",
                newName: "QrCode");

            migrationBuilder.RenameColumn(
                name: "Active",
                table: "QrCode",
                newName: "IsDeleted");

            migrationBuilder.AlterColumn<string>(
                name: "Link",
                table: "QrCode",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "IdTable",
                table: "QrCode",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "QrCode",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NotificationLifeTimeSecunds",
                table: "QrCode",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "QrCode",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QrCode",
                table: "QrCode",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "NotifyDevice",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeviceToken = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotifyDevice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Restaurant",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdIdentityUser = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaxNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsFilled = table.Column<bool>(type: "bit", nullable: false),
                    NotifyMe = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    TimeZoneMinutes = table.Column<int>(type: "int", nullable: false),
                    IdentityUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Restaurant_AspNetUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StatisticQrCode",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdQrCode = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatisticQrCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatisticQrCode_QrCode_IdQrCode",
                        column: x => x.IdQrCode,
                        principalTable: "QrCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotifyDeviceQrCode",
                columns: table => new
                {
                    NotifyDevicesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QrCodesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotifyDeviceQrCode", x => new { x.NotifyDevicesId, x.QrCodesId });
                    table.ForeignKey(
                        name: "FK_NotifyDeviceQrCode_NotifyDevice_NotifyDevicesId",
                        column: x => x.NotifyDevicesId,
                        principalTable: "NotifyDevice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotifyDeviceQrCode_QrCode_QrCodesId",
                        column: x => x.QrCodesId,
                        principalTable: "QrCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Table",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdResaurant = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdQrCode = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Table", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Table_QrCode_IdQrCode",
                        column: x => x.IdQrCode,
                        principalTable: "QrCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Table_Restaurant_IdResaurant",
                        column: x => x.IdResaurant,
                        principalTable: "Restaurant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QrCode_Link",
                table: "QrCode",
                column: "Link");

            migrationBuilder.CreateIndex(
                name: "IX_NotifyDevice_DeviceToken",
                table: "NotifyDevice",
                column: "DeviceToken");

            migrationBuilder.CreateIndex(
                name: "IX_NotifyDeviceQrCode_QrCodesId",
                table: "NotifyDeviceQrCode",
                column: "QrCodesId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_IdentityUserId",
                table: "Restaurant",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_StatisticQrCode_IdQrCode",
                table: "StatisticQrCode",
                column: "IdQrCode");

            migrationBuilder.CreateIndex(
                name: "IX_Table_IdQrCode",
                table: "Table",
                column: "IdQrCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Table_IdResaurant",
                table: "Table",
                column: "IdResaurant");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotifyDeviceQrCode");

            migrationBuilder.DropTable(
                name: "StatisticQrCode");

            migrationBuilder.DropTable(
                name: "Table");

            migrationBuilder.DropTable(
                name: "NotifyDevice");

            migrationBuilder.DropTable(
                name: "Restaurant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QrCode",
                table: "QrCode");

            migrationBuilder.DropIndex(
                name: "IX_QrCode_Link",
                table: "QrCode");

            migrationBuilder.DropColumn(
                name: "IdTable",
                table: "QrCode");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "QrCode");

            migrationBuilder.DropColumn(
                name: "NotificationLifeTimeSecunds",
                table: "QrCode");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "QrCode");

            migrationBuilder.RenameTable(
                name: "QrCode",
                newName: "QrCodes");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "QrCodes",
                newName: "Active");

            migrationBuilder.AlterColumn<string>(
                name: "Link",
                table: "QrCodes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QrCodes",
                table: "QrCodes",
                column: "Id");
        }
    }
}
