using System.Collections.Generic;
namespace CaseStudyAssignment.CsvDataComparison.Models
{
    /// <summary>
    /// Represents a single CSV record with field name and value mapping.
    /// </summary>
    public class CsvRecord
    {
        public Dictionary<string, string> Fields { get; }

        public CsvRecord(Dictionary<string, string> fields)
        {
            Fields = fields;
        }

        /// <summary>
        /// Returns value by column name safely.
        /// </summary>
        public string GetValue(string columnName)
        {
            return Fields.ContainsKey(columnName) ? Fields[columnName] : string.Empty;
        }
    }
}
