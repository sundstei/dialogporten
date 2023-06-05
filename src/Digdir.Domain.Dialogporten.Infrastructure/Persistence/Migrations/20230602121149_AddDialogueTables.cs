﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Digdir.Domain.Dialogporten.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDialogueTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DialogueActivityType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DialogueActivityType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DialogueGuiActionType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DialogueGuiActionType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DialogueStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DialogueStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LocalizationSet",
                columns: table => new
                {
                    InternalId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "current_timestamp at time zone 'utc'"),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalizationSet", x => x.InternalId);
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessage",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
                    EventType = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    EventPayload = table.Column<string>(type: "jsonb", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessage", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "Dialogue",
                columns: table => new
                {
                    InternalId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "current_timestamp at time zone 'utc'"),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "current_timestamp at time zone 'utc'"),
                    UpdatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Org = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ServiceResourceIdentifier = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Party = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ExtendedStatus = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    VisibleFromUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DueAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ExpiresAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ReadAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    StatusId = table.Column<int>(type: "integer", nullable: false),
                    BodyId = table.Column<long>(type: "bigint", nullable: false),
                    TitleId = table.Column<long>(type: "bigint", nullable: false),
                    SenderNameId = table.Column<long>(type: "bigint", nullable: false),
                    SearchTitleId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dialogue", x => x.InternalId);
                    table.ForeignKey(
                        name: "FK_Dialogue_DialogueStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "DialogueStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dialogue_LocalizationSet_BodyId",
                        column: x => x.BodyId,
                        principalTable: "LocalizationSet",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dialogue_LocalizationSet_SearchTitleId",
                        column: x => x.SearchTitleId,
                        principalTable: "LocalizationSet",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dialogue_LocalizationSet_SenderNameId",
                        column: x => x.SenderNameId,
                        principalTable: "LocalizationSet",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dialogue_LocalizationSet_TitleId",
                        column: x => x.TitleId,
                        principalTable: "LocalizationSet",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Localization",
                columns: table => new
                {
                    CultureCode = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    LocalizationSetId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "current_timestamp at time zone 'utc'"),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "current_timestamp at time zone 'utc'"),
                    UpdatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localization", x => new { x.LocalizationSetId, x.CultureCode });
                    table.ForeignKey(
                        name: "FK_Localization_LocalizationSet_LocalizationSetId",
                        column: x => x.LocalizationSetId,
                        principalTable: "LocalizationSet",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessageConsumer",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConsumerName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessageConsumer", x => new { x.EventId, x.ConsumerName });
                    table.ForeignKey(
                        name: "FK_OutboxMessageConsumer_OutboxMessage_EventId",
                        column: x => x.EventId,
                        principalTable: "OutboxMessage",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DialogueActivity",
                columns: table => new
                {
                    InternalId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "current_timestamp at time zone 'utc'"),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    PerformedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ExtendedType = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    DescriptionInternalId = table.Column<long>(type: "bigint", nullable: false),
                    DetailsApiUrl = table.Column<string>(type: "character varying(1023)", maxLength: 1023, nullable: true),
                    DetailsGuiUrl = table.Column<string>(type: "character varying(1023)", maxLength: 1023, nullable: true),
                    TypeId = table.Column<int>(type: "integer", nullable: false),
                    DialogueId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DialogueActivity", x => x.InternalId);
                    table.ForeignKey(
                        name: "FK_DialogueActivity_DialogueActivityType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "DialogueActivityType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DialogueActivity_Dialogue_DialogueId",
                        column: x => x.DialogueId,
                        principalTable: "Dialogue",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DialogueActivity_LocalizationSet_DescriptionInternalId",
                        column: x => x.DescriptionInternalId,
                        principalTable: "LocalizationSet",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DialogueApiAction",
                columns: table => new
                {
                    InternalId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "current_timestamp at time zone 'utc'"),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "current_timestamp at time zone 'utc'"),
                    UpdatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Action = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Resource = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Url = table.Column<string>(type: "character varying(1023)", maxLength: 1023, nullable: false),
                    HttpMethod = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    DocumentationUrl = table.Column<string>(type: "character varying(1023)", maxLength: 1023, nullable: true),
                    RequestSchema = table.Column<string>(type: "character varying(1023)", maxLength: 1023, nullable: true),
                    ResponseSchema = table.Column<string>(type: "character varying(1023)", maxLength: 1023, nullable: true),
                    DialogueId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DialogueApiAction", x => x.InternalId);
                    table.ForeignKey(
                        name: "FK_DialogueApiAction_Dialogue_DialogueId",
                        column: x => x.DialogueId,
                        principalTable: "Dialogue",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DialogueAttachement",
                columns: table => new
                {
                    InternalId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "current_timestamp at time zone 'utc'"),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "current_timestamp at time zone 'utc'"),
                    UpdatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    DisplayNameInternalId = table.Column<long>(type: "bigint", nullable: false),
                    SizeInBytes = table.Column<long>(type: "bigint", nullable: false),
                    ContentType = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Url = table.Column<string>(type: "character varying(1023)", maxLength: 1023, nullable: false),
                    Resource = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    DialogueId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DialogueAttachement", x => x.InternalId);
                    table.ForeignKey(
                        name: "FK_DialogueAttachement_Dialogue_DialogueId",
                        column: x => x.DialogueId,
                        principalTable: "Dialogue",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DialogueAttachement_LocalizationSet_DisplayNameInternalId",
                        column: x => x.DisplayNameInternalId,
                        principalTable: "LocalizationSet",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DialogueGuiAction",
                columns: table => new
                {
                    InternalId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "current_timestamp at time zone 'utc'"),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "current_timestamp at time zone 'utc'"),
                    UpdatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Action = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    TitleInternalId = table.Column<long>(type: "bigint", nullable: false),
                    Url = table.Column<string>(type: "character varying(1023)", maxLength: 1023, nullable: false),
                    Resource = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IsBackChannel = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleteAction = table.Column<bool>(type: "boolean", nullable: false),
                    TypeId = table.Column<int>(type: "integer", nullable: false),
                    DialogueId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DialogueGuiAction", x => x.InternalId);
                    table.ForeignKey(
                        name: "FK_DialogueGuiAction_DialogueGuiActionType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "DialogueGuiActionType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DialogueGuiAction_Dialogue_DialogueId",
                        column: x => x.DialogueId,
                        principalTable: "Dialogue",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DialogueGuiAction_LocalizationSet_TitleInternalId",
                        column: x => x.TitleInternalId,
                        principalTable: "LocalizationSet",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DialogueTokenScope",
                columns: table => new
                {
                    InternalId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "current_timestamp at time zone 'utc'"),
                    CreatedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    DialogueId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DialogueTokenScope", x => x.InternalId);
                    table.ForeignKey(
                        name: "FK_DialogueTokenScope_Dialogue_DialogueId",
                        column: x => x.DialogueId,
                        principalTable: "Dialogue",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DialogueActivityType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Submission" },
                    { 2, "Feedback" },
                    { 3, "Information" },
                    { 4, "Error" },
                    { 5, "Closed" },
                    { 6, "Seen" },
                    { 7, "Forwarded" }
                });

            migrationBuilder.InsertData(
                table: "DialogueGuiActionType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Primary" },
                    { 2, "Secondary" },
                    { 3, "Tertiary" }
                });

            migrationBuilder.InsertData(
                table: "DialogueStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Unspecified" },
                    { 2, "InProgress" },
                    { 3, "Waiting" },
                    { 4, "Signing" },
                    { 5, "Cancelled" },
                    { 6, "Completed" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dialogue_BodyId",
                table: "Dialogue",
                column: "BodyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dialogue_Id",
                table: "Dialogue",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dialogue_SearchTitleId",
                table: "Dialogue",
                column: "SearchTitleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dialogue_SenderNameId",
                table: "Dialogue",
                column: "SenderNameId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dialogue_StatusId",
                table: "Dialogue",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Dialogue_TitleId",
                table: "Dialogue",
                column: "TitleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DialogueActivity_DescriptionInternalId",
                table: "DialogueActivity",
                column: "DescriptionInternalId");

            migrationBuilder.CreateIndex(
                name: "IX_DialogueActivity_DialogueId",
                table: "DialogueActivity",
                column: "DialogueId");

            migrationBuilder.CreateIndex(
                name: "IX_DialogueActivity_Id",
                table: "DialogueActivity",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DialogueActivity_TypeId",
                table: "DialogueActivity",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DialogueApiAction_DialogueId",
                table: "DialogueApiAction",
                column: "DialogueId");

            migrationBuilder.CreateIndex(
                name: "IX_DialogueApiAction_Id",
                table: "DialogueApiAction",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DialogueAttachement_DialogueId",
                table: "DialogueAttachement",
                column: "DialogueId");

            migrationBuilder.CreateIndex(
                name: "IX_DialogueAttachement_DisplayNameInternalId",
                table: "DialogueAttachement",
                column: "DisplayNameInternalId");

            migrationBuilder.CreateIndex(
                name: "IX_DialogueAttachement_Id",
                table: "DialogueAttachement",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DialogueGuiAction_DialogueId",
                table: "DialogueGuiAction",
                column: "DialogueId");

            migrationBuilder.CreateIndex(
                name: "IX_DialogueGuiAction_Id",
                table: "DialogueGuiAction",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DialogueGuiAction_TitleInternalId",
                table: "DialogueGuiAction",
                column: "TitleInternalId");

            migrationBuilder.CreateIndex(
                name: "IX_DialogueGuiAction_TypeId",
                table: "DialogueGuiAction",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DialogueTokenScope_DialogueId_Value",
                table: "DialogueTokenScope",
                columns: new[] { "DialogueId", "Value" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DialogueTokenScope_Id",
                table: "DialogueTokenScope",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LocalizationSet_Id",
                table: "LocalizationSet",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DialogueActivity");

            migrationBuilder.DropTable(
                name: "DialogueApiAction");

            migrationBuilder.DropTable(
                name: "DialogueAttachement");

            migrationBuilder.DropTable(
                name: "DialogueGuiAction");

            migrationBuilder.DropTable(
                name: "DialogueTokenScope");

            migrationBuilder.DropTable(
                name: "Localization");

            migrationBuilder.DropTable(
                name: "OutboxMessageConsumer");

            migrationBuilder.DropTable(
                name: "DialogueActivityType");

            migrationBuilder.DropTable(
                name: "DialogueGuiActionType");

            migrationBuilder.DropTable(
                name: "Dialogue");

            migrationBuilder.DropTable(
                name: "OutboxMessage");

            migrationBuilder.DropTable(
                name: "DialogueStatus");

            migrationBuilder.DropTable(
                name: "LocalizationSet");
        }
    }
}