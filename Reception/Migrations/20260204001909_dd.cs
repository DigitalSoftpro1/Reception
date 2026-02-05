using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Reception.Migrations
{
    /// <inheritdoc />
    public partial class dd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Treatments_VisitTreatments_VisitTreatmentId",
                table: "Treatments");

            migrationBuilder.DropForeignKey(
                name: "FK_Visits_Clinics_ClinicId",
                table: "Visits");

            migrationBuilder.DropForeignKey(
                name: "FK_Visits_VisitTreatments_VisitTreatmentId",
                table: "Visits");

            migrationBuilder.DropIndex(
                name: "IX_Visits_VisitTreatmentId",
                table: "Visits");

            migrationBuilder.DropIndex(
                name: "IX_Treatments_VisitTreatmentId",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "VisitTreatmentId",
                table: "Visits");

            migrationBuilder.DropColumn(
                name: "VisitTreatmentId",
                table: "Treatments");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Visits",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "ClinicId",
                table: "Visits",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_VisitTreatments_TreatmentId",
                table: "VisitTreatments",
                column: "TreatmentId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitTreatments_VisitId",
                table: "VisitTreatments",
                column: "VisitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Visits_Clinics_ClinicId",
                table: "Visits",
                column: "ClinicId",
                principalTable: "Clinics",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VisitTreatments_Treatments_TreatmentId",
                table: "VisitTreatments",
                column: "TreatmentId",
                principalTable: "Treatments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VisitTreatments_Visits_VisitId",
                table: "VisitTreatments",
                column: "VisitId",
                principalTable: "Visits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Visits_Clinics_ClinicId",
                table: "Visits");

            migrationBuilder.DropForeignKey(
                name: "FK_VisitTreatments_Treatments_TreatmentId",
                table: "VisitTreatments");

            migrationBuilder.DropForeignKey(
                name: "FK_VisitTreatments_Visits_VisitId",
                table: "VisitTreatments");

            migrationBuilder.DropIndex(
                name: "IX_VisitTreatments_TreatmentId",
                table: "VisitTreatments");

            migrationBuilder.DropIndex(
                name: "IX_VisitTreatments_VisitId",
                table: "VisitTreatments");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Visits",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClinicId",
                table: "Visits",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VisitTreatmentId",
                table: "Visits",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VisitTreatmentId",
                table: "Treatments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Visits_VisitTreatmentId",
                table: "Visits",
                column: "VisitTreatmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_VisitTreatmentId",
                table: "Treatments",
                column: "VisitTreatmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Treatments_VisitTreatments_VisitTreatmentId",
                table: "Treatments",
                column: "VisitTreatmentId",
                principalTable: "VisitTreatments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Visits_Clinics_ClinicId",
                table: "Visits",
                column: "ClinicId",
                principalTable: "Clinics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Visits_VisitTreatments_VisitTreatmentId",
                table: "Visits",
                column: "VisitTreatmentId",
                principalTable: "VisitTreatments",
                principalColumn: "Id");
        }
    }
}
