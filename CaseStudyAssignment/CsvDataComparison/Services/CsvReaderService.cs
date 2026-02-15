using CaseStudyAssignment.CsvDataComparison.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseStudyAssignment.CsvDataComparison.Services
{
    /// <summary>
    /// Reads CSV file in streaming manner.
    /// No external libraries used.
    /// </summary>
    public class CsvReaderService
    {
        /// <summary>
        /// Reads CSV file and returns dictionary of key -> CsvRecord
        /// </summary>
        public Dictionary<string, CsvRecord> ReadCsv(
            string filePath,
            List<string> keyColumns)
        {
            var records = new Dictionary<string, CsvRecord>();

            using (var reader = new StreamReader(filePath))
            {

                // Read header line
                string headerLine = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(headerLine))
                    throw new Exception("CSV file has no header.");

                string[] headers = headerLine.Split(',');

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string[] values = line.Split(',');

                    if (values.Length != headers.Length)
                        throw new Exception("Field count mismatch.");

                    var fieldDict = new Dictionary<string, string>();

                    for (int i = 0; i < headers.Length; i++)
                    {
                        fieldDict[headers[i].Trim()] = values[i].Trim();
                    }

                    var record = new CsvRecord(fieldDict);

                    // Generate composite key
                    string key = GenerateKey(record, keyColumns);

                    records[key] = record;
                }
            }

            return records;
        }

        private string GenerateKey(CsvRecord record, List<string> keyColumns)
        {
            var keyParts = new List<string>();

            foreach (var column in keyColumns)
            {
                keyParts.Add(record.GetValue(column));
            }

            return string.Join("|", keyParts);
        }
    }
}
