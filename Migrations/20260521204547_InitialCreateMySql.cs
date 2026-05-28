using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace AspNetCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateMySql : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                CREATE TABLE IF NOT EXISTS `Alunos` (
                    `Id` int NOT NULL AUTO_INCREMENT,
                    `Matricula` longtext NOT NULL,
                    `Curso` longtext NOT NULL,
                    `Nome` varchar(100) NOT NULL,
                    `Sobrenome` longtext NOT NULL,
                    `Cpf` longtext NOT NULL,
                    `Email` longtext NOT NULL,
                    `DataNascimento` datetime(6) NOT NULL,
                    PRIMARY KEY (`Id`)
                ) CHARACTER SET utf8mb4;
                """);

            migrationBuilder.Sql("""
                CREATE TABLE IF NOT EXISTS `Professores` (
                    `Id` int NOT NULL AUTO_INCREMENT,
                    `Siap` longtext NOT NULL,
                    `Area` longtext NOT NULL,
                    `Nome` varchar(100) NOT NULL,
                    `Sobrenome` longtext NOT NULL,
                    `Cpf` longtext NOT NULL,
                    `Email` longtext NOT NULL,
                    `DataNascimento` datetime(6) NOT NULL,
                    PRIMARY KEY (`Id`)
                ) CHARACTER SET utf8mb4;
                """);

            migrationBuilder.Sql("""
                CREATE TABLE IF NOT EXISTS `Usuarios` (
                    `Id` int NOT NULL AUTO_INCREMENT,
                    `Email` varchar(150) NOT NULL,
                    `SenhaHash` varchar(255) NOT NULL,
                    PRIMARY KEY (`Id`),
                    UNIQUE KEY `IX_Usuarios_Email` (`Email`)
                ) CHARACTER SET utf8mb4;
                """);

            migrationBuilder.Sql("""
                CREATE TABLE IF NOT EXISTS `Disciplinas` (
                    `Id` int NOT NULL AUTO_INCREMENT,
                    `Nome` varchar(100) NOT NULL,
                    `CargaHoraria` int NOT NULL,
                    `Periodo` longtext NOT NULL,
                    `ProfessorId` int NULL,
                    PRIMARY KEY (`Id`),
                    CONSTRAINT `FK_Disciplinas_Professores_ProfessorId`
                        FOREIGN KEY (`ProfessorId`) REFERENCES `Professores` (`Id`)
                ) CHARACTER SET utf8mb4;
                """);

            migrationBuilder.Sql("""
                CREATE TABLE IF NOT EXISTS `AlunoDisciplina` (
                    `AlunosId` int NOT NULL,
                    `DisciplinasId` int NOT NULL,
                    PRIMARY KEY (`AlunosId`, `DisciplinasId`),
                    CONSTRAINT `FK_AlunoDisciplina_Alunos_AlunosId`
                        FOREIGN KEY (`AlunosId`) REFERENCES `Alunos` (`Id`) ON DELETE CASCADE,
                    CONSTRAINT `FK_AlunoDisciplina_Disciplinas_DisciplinasId`
                        FOREIGN KEY (`DisciplinasId`) REFERENCES `Disciplinas` (`Id`) ON DELETE CASCADE
                ) CHARACTER SET utf8mb4;
                """);

            migrationBuilder.Sql("""
                CREATE TABLE IF NOT EXISTS `Notas` (
                    `Id` int NOT NULL AUTO_INCREMENT,
                    `AlunoId` int NOT NULL,
                    `DisciplinaId` int NOT NULL,
                    `Valor` decimal(18,2) NOT NULL,
                    `Descricao` longtext NOT NULL,
                    PRIMARY KEY (`Id`),
                    CONSTRAINT `FK_Notas_Alunos_AlunoId`
                        FOREIGN KEY (`AlunoId`) REFERENCES `Alunos` (`Id`) ON DELETE CASCADE,
                    CONSTRAINT `FK_Notas_Disciplinas_DisciplinaId`
                        FOREIGN KEY (`DisciplinaId`) REFERENCES `Disciplinas` (`Id`) ON DELETE CASCADE
                ) CHARACTER SET utf8mb4;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlunoDisciplina");

            migrationBuilder.DropTable(
                name: "Notas");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Alunos");

            migrationBuilder.DropTable(
                name: "Disciplinas");

            migrationBuilder.DropTable(
                name: "Professores");
        }
    }
}
