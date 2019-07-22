using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminInmuebles.Extensions
{
    public static class Querys
    {
        public static string ToUpdate(this Models.Campo[] fields, string tableName, JObject value)
        {
            var index = 0;
            var query = $"UPDATE {tableName} SET ";
            fields.Where(c => !c.IsIdentity).ToList().ForEach(column =>
            {
                if (index > 0)
                    query += ", ";
                query += $"{column.Nombre} = '{value[column.Nombre]}'";
                index++;
            });
            index = 0;
            query += " WHERE ";
            fields.Where(c => c.IsIdentity).ToList().ForEach(column =>
            {
                if (index > 0)
                    query += " and ";
                query += $"{column.Nombre} = {value[column.Nombre]}";
                index++;
            });
            return query;
        }

        public static string ToCreate(this Models.Campo[] fields, string tableName, JObject value)
        {
            var index = 0;
            var query = $"INSERT INTO {tableName} (";
            fields.Where(c => !c.IsIdentity).ToList().ForEach(column =>
            {
                if (index > 0)
                    query += ", ";
                query += $"{column.Nombre}";
                index++;
            });
            index = 0;
            query += ") VALUES (";
            fields.Where(c => !c.IsIdentity).ToList().ForEach(column =>
            {
                if (index > 0)
                    query += ", ";
                query += $"'{value[column.Nombre]}'";
                index++;
            });
            query += ")";
            return query;
        }

        public static string ToDelete(this Models.Campo[] fields, string tableName, JObject value)
        {
            var index = 0;
            var query = $"DELETE {tableName} WHERE ";
            fields.Where(c => c.IsIdentity).ToList().ForEach(column =>
            {
                if (index > 0)
                    query += " and ";
                query += $"{column.Nombre} = {value[column.Nombre]}";
                index++;
            });
            return query;
        }
    }
}
