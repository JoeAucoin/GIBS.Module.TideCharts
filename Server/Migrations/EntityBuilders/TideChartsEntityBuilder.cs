using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;
using System.Net;

namespace GIBS.Module.TideCharts.Migrations.EntityBuilders
{
    public class TideChartsEntityBuilder : AuditableBaseEntityBuilder<TideChartsEntityBuilder>
    {
        private const string _entityTableName = "GIBSTideCharts";
        private readonly PrimaryKey<TideChartsEntityBuilder> _primaryKey = new("PK_GIBSTideCharts", x => x.TideChartsId);
        private readonly ForeignKey<TideChartsEntityBuilder> _moduleForeignKey = new("FK_GIBSTideCharts_Module", x => x.ModuleId, "Module", "ModuleId", ReferentialAction.Cascade);

        public TideChartsEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_moduleForeignKey);
        }

        protected override TideChartsEntityBuilder BuildTable(ColumnsBuilder table)
        {
            TideChartsId = AddAutoIncrementColumn(table,"TideChartsId");
            ModuleId = AddIntegerColumn(table,"ModuleId");
            StationName = table.Column<string>(name: "StationName", maxLength: 256, nullable: false);
            StationId = table.Column<string>(name: "StationId", maxLength: 10, nullable: false);
            State = table.Column<string>(name: "State", maxLength: 2, nullable: true);
            TimeZoneCorrection = table.Column<string>(name: "TimeZoneCorrection", maxLength: 50, nullable: true);
            Latitude = table.Column<double>(name: "Latitude", nullable: false, defaultValue: 0.0);
            Longitude = table.Column<double>(name: "Longitude", nullable: false, defaultValue: 0.0);
            Slug = table.Column<string>(name: "Slug", maxLength: 500, nullable: true);
            IsActive = table.Column<bool>(name: "IsActive", nullable: false, defaultValue: true);
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> TideChartsId { get; set; }
        public OperationBuilder<AddColumnOperation> ModuleId { get; set; }
        public OperationBuilder<AddColumnOperation> StationName { get; set; }
        public OperationBuilder<AddColumnOperation> StationId { get; set; }
        public OperationBuilder<AddColumnOperation> State { get; set; }
        public OperationBuilder<AddColumnOperation> TimeZoneCorrection { get; set; }
        public OperationBuilder<AddColumnOperation> Latitude { get; set; }
        public OperationBuilder<AddColumnOperation> Longitude { get; set; }
        public OperationBuilder<AddColumnOperation> Slug { get; set; }
        public OperationBuilder<AddColumnOperation> IsActive { get; set; }
    }
}
