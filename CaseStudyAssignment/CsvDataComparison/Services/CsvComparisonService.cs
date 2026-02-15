using CaseStudyAssignment.CsvDataComparison.Configuration;
using CaseStudyAssignment.CsvDataComparison.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseStudyAssignment.CsvDataComparison.Services
{
    /// <summary>
    /// Performs CSV comparison logic.
    /// </summary>
    public class CsvComparisonService
    {
        public ComparisonResult Compare(
            Dictionary<string, CsvRecord> expected,
            Dictionary<string, CsvRecord> actual,
            CsvComparisonOptions options)
        {
            var result = new ComparisonResult();

            // 1. Missing in Actual
            foreach (var key in expected.Keys)
            {
                if (!actual.ContainsKey(key))
                {
                    result.MissingInActual.Add(key);
                }
            }

            // 2. Missing in Expected
            foreach (var key in actual.Keys)
            {
                if (!expected.ContainsKey(key))
                {
                    result.MissingInExpected.Add(key);
                }
            }

            // 3. Field level comparison
            foreach (var key in expected.Keys)
            {
                if (!actual.ContainsKey(key))
                    continue;

                var expectedRecord = expected[key];
                var actualRecord = actual[key];

                foreach (var field in expectedRecord.Fields.Keys)
                {
                    if (options.IgnoreColumns.Contains(field))
                        continue;

                    var expectedValue = expectedRecord.GetValue(field);
                    var actualValue = actualRecord.GetValue(field);

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

        private bool ValuesEqual(string expected, string actual, CsvComparisonOptions options)
        {
            if (options.CaseSensitiveComparison)
                return expected == actual;

            return string.Equals(expected, actual, StringComparison.OrdinalIgnoreCase);
        }
    }
}
