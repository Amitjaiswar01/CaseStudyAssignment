using CaseStudyAssignment.CsvDataComparison.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace CaseStudyAssignment.CsvDataComparison.Services
{
    /// <summary>
    /// Reads CSV file in streaming manner.
    /// No external libraries used.
    /// </summary>
    public class CsvReaderService
    {
        public List<string> BadDataRows { get; private set; } = new List<string>();
        /// <summary>
        /// Reads CSV and returns all records
        /// </summary>
        /// <param name="filePath">Path to the CSV file.</param>
        /// <param name="delimiter">Character used to separate fields (default is comma).</param>
        /// <returns>List of CsvRecord objects representing each row in the CSV.</returns>
        public List<CsvRecord> ReadCsv(string filePath, char delimiter = ',')
        {
            var records = new List<CsvRecord>(); // Initialize a list to hold all CSV records

            using (var reader = new StreamReader(filePath))
            {
                string headerLine = reader.ReadLine();

                // If the header line is missing or empty, throw an exception
                if (string.IsNullOrWhiteSpace(headerLine))
                    throw new Exception("CSV file has no header.");

                // Split the header line into individual column names based on delimiter
                string[] headers = headerLine.Split(delimiter);

                string line;

                // Read each line until the end of the file
                while ((line = reader.ReadLine()) != null)
                {
                    // Skip empty or whitespace-only lines
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string[] values = line.Split(delimiter);

                    // Create a dictionary to store column name -> value for this row
                    var fieldDict = new Dictionary<string, string>();

                    for (int i = 0; i < headers.Length; i++)
                    {
                        fieldDict[headers[i].Trim()] = values[i].Trim();
                    }

                    // Create a CsvRecord object for this row and add it to the records list
                    records.Add(new CsvRecord(fieldDict));
                }
            }

            return records;
        }
    }
}