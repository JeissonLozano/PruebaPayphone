using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prueba.Payphone.Infraestructura.Migrations
{
    /// <inheritdoc />
    public partial class _100 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Nombre de usuario para autenticación"),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Dirección de correo electrónico del usuario"),
                    PasswordHash = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false, comment: "Contraseña hasheada"),
                    Salt = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false, comment: "Sal para el hash de la contraseña"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Fecha y hora de creación del usuario"),
                    LastAccess = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Fecha y hora del último acceso exitoso"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true, comment: "Indica si la cuenta de usuario está activa")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Documento de identidad del propietario de la billetera"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Nombre completo del propietario de la billetera"),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Saldo actual de la billetera en moneda base"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Fecha y hora de creación de la billetera"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Fecha y hora de última actualización de la billetera")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movements",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WalletId = table.Column<int>(type: "int", nullable: false, comment: "ID de la billetera asociada al movimiento"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "Monto del movimiento en la moneda base"),
                    Type = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, comment: "Tipo de movimiento: Débito o Crédito"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Fecha y hora de creación del movimiento")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movements_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalSchema: "dbo",
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movements_WalletId",
                schema: "dbo",
                table: "Movements",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "dbo",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                schema: "dbo",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_DocumentId",
                schema: "dbo",
                table: "Wallets",
                column: "DocumentId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movements",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Wallets",
                schema: "dbo");
        }
    }
}
