using System;
using SimpleEFDbSeeder.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using System.Runtime.ConstrainedExecution;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection;
using System.Data;

namespace SimpleEFDbSeeder
{
    public class DbSeeder
    {
        private readonly DbContext _context;

        public DbSeeder(DbContext context)
        {
            _context = context;
        }



        public void RemoveData(List<string> entities)
        {
            if (entities.Count() == 1 && entities[0].ToLower() == "all")
            {
                RemoveDataFromAllTables();
            }
            else
            {


                var listOfTypes = entities.Select(x => Type.GetType(x.First().ToString().ToUpper() + x.Substring(1).ToLower())).Distinct().ToList();

                foreach (var entityType in listOfTypes)
                {
                    var tableName = _context.Model.FindEntityType(entityType).GetTableName();
                    var sqlDeleteCommand = $"DELETE FROM {tableName}";
                    var sqlUncheckConstraintsCommand = $"ALTER TABLE {tableName} NOCHECK CONSTRAINT ALL";

                    _context.Database.ExecuteSqlRaw(sqlUncheckConstraintsCommand);
                    _context.Database.ExecuteSqlRaw(sqlDeleteCommand);
                    _context.Database.ExecuteSqlRaw($"ALTER TABLE {tableName} CHECK CONSTRAINT ALL");
                }
            }
        }

        private void RemoveDataFromAllTables()
        {
            var entityTypes = _context.Model.GetEntityTypes();

            foreach (var entityType in entityTypes)
            {
                var tableName = entityType.GetTableName();
                var sqlUncheckConstraintsCommand = $"ALTER TABLE {tableName} NOCHECK CONSTRAINT ALL";
                var sqlDeleteCommand = $"DELETE FROM {tableName}";

                _context.Database.ExecuteSqlRaw(sqlUncheckConstraintsCommand);
                _context.Database.ExecuteSqlRaw(sqlDeleteCommand);
                _context.Database.ExecuteSqlRaw($"ALTER TABLE {tableName} CHECK CONSTRAINT ALL");
            }
        }

        public void ApplyMigrations()
        {
            _context.Database.Migrate();
        }

        public void AddSeedData(List<string> tables)
        {
            foreach (var table in tables)
            {
                var dataSetField = typeof(SampleData).GetProperty(table, BindingFlags.Static | BindingFlags.Public);
                if (dataSetField != null)
                {
                    var dataSet = dataSetField.GetValue(null);
                    if (dataSet != null)
                    {
                        var dataSetType = dataSet.GetType();
                        var elementType = dataSetType.GetGenericArguments()[0];
                        var addRangeMethod = _context.GetType().GetMethod("AddRange", new[] { typeof(IEnumerable<>).MakeGenericType(elementType) });
                        if (addRangeMethod != null)
                        {
                            addRangeMethod.Invoke(_context, new[] { dataSet });
                        }
                    }
                }
            }
            _context.SaveChanges();
        }

        public List<string> GetTableNames()
        {
            var tableNames = new List<string>();

            var connection = _context.Database.GetDbConnection();
            try
            {
                connection.Open();

                var schema = connection.GetSchema("Tables");
                foreach (DataRow row in schema.Rows)
                {
                    var tableName = row["TABLE_NAME"].ToString();
                    tableNames.Add(tableName);
                }
            }
            finally
            {
                connection.Close();
            }

            return tableNames;
        }
    }

}