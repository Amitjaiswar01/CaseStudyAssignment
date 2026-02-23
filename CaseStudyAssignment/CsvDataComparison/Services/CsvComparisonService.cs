using CaseStudyAssignment.CsvDataComparison.Configuration;
using CaseStudyAssignment.CsvDataComparison.Models;
using System;
using System.Collections.Generic;

namespace CaseStudyAssignment.CsvDataComparison.Services
{
    /// <summary>
    /// Performs CSV comparison logic.
    /// </summary>
    public class CsvComparisonService
    {
        public ComparisonResult Compare(
            List<CsvRecord> expectedRecords,
            List<CsvRecord> actualRecords,
            CsvComparisonOptions options)
        {
            var result = new ComparisonResult();

            var expectedDict = new Dictionary<string, CsvRecord>();
            var actualDict = new Dictionary<string, CsvRecord>();

            // Build Expected dictionary and detect duplicates
            foreach (var record in expectedRecords)
            {
                string key = GenerateKey(record, options.KeyColumns);

                if (expectedDict.ContainsKey(key))
                {
                    result.DuplicateKeys.Add($"Duplicate in Expected: {key}");
                    continue;
                }

                expectedDict[key] = record;
            }

            // Build Actual dictionary and detect duplicates
            foreach (var record in actualRecords)
            {
                string key = GenerateKey(record, options.KeyColumns);

                if (actualDict.ContainsKey(key))
                {
                    result.DuplicateKeys.Add($"Duplicate in Actual: {key}");
                    continue;
                }

                actualDict[key] = record;
            }

            // 1. Missing in Actual
            foreach (var key in expectedDict.Keys)
            {
                if (!actualDict.ContainsKey(key))
                    result.MissingInActual.Add(key);
            }

            // 2. Missing in Expected
            foreach (var key in actualDict.Keys)
            {
                if (!expectedDict.ContainsKey(key))
                    result.MissingInExpected.Add(key);
            }

            // 3. Field-level comparison
            foreach (var key in expectedDict.Keys)
            {
                if (!actualDict.ContainsKey(key))
                    continue;

                var expected = expectedDict[key];
                var actual = actualDict[key];

                foreach (var field in expected.Fields.Keys)
                {
                    if (options.IgnoreColumns.Contains(field))
                        continue;

                    var expectedValue = expected.GetValue(field);
                    var actualValue = actual.GetValue(field);

                    if (!ValuesEqual(expectedValue, actualValue, options))
                    {
                        string message =
                            $"Failed Fieldname: {field} | " +
                            $"Expected Input Value: \"{expectedValue}\" | " +
                            $"Actual Value: \"{actualValue}\" | " +
                            $"For record key: {key}";

                        result.FieldMismatches.Add(message);
                    }
                }
            }

            return result;
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

        private bool ValuesEqual(string expected, string actual, CsvComparisonOptions options)
        {
            if (options.CaseSensitiveComparison)
                return expected == actual;

            return string.Equals(expected, actual, StringComparison.OrdinalIgnoreCase);
        }
    }
}