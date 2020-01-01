using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class InitialDbCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameSettingses",
                columns: table => new
                {
                    GameSettingsId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameName = table.Column<string>(maxLength: 255, nullable: false, defaultValue: "Connect 4"),
                    BoardHeight = table.Column<int>(nullable: false, defaultValue: 6)
                        .Annotation("Sqlite:Autoincrement", false),
                    BoardWidth = table.Column<int>(nullable: false, defaultValue: 7)
                        .Annotation("Sqlite:Autoincrement", false),
                    SaveName = table.Column<string>(maxLength: 255, nullable: true),
                    SerializedBoard = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameSettingses", x => x.GameSettingsId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameSettingses");
        }
    }
}
