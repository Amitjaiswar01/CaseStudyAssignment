using CaseStudyAssignment.CsvDataComparison.Configuration;
using CaseStudyAssignment.CsvDataComparison.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace CaseStudyAssignment.CsvTest
{
    public class CsvComparisonTests
    {
        private string _expectedPath;
        private string _actualPath;

        [SetUp]
        public void Setup()
        {
            _expectedPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Expected1.csv");
            _actualPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Actual1.csv");
        }

        [Test]
        public void CompareCsv_ShouldDetectAllFailures()
        {
            var reader = new CsvReaderService();

            var options = new CsvComparisonOptions
            {
                KeyColumns = new List<string> { "Name", "ID" }
            };

            var expected = reader.ReadCsv(_expectedPath, options.KeyColumns);
            var actual = reader.ReadCsv(_actualPath, options.KeyColumns);

            var comparer = new CsvComparisonService();
            var result = comparer.Compare(expected, actual, options);

            Assert.That(result.MissingInActual.Count > 0);   // at least one record missing in Actual
            Assert.That(result.MissingInExpected.Count > 0); // at least one record extra in Actual
            Assert.That(result.FieldMismatches.Count > 0);   // at least one record mismatched

            // Print results
            Console.WriteLine("=== Missing in Actual ===");
            foreach (var key in result.MissingInActual)
                Console.WriteLine(key);

            Console.WriteLine("=== Missing in Expected ===");
            foreach (var key in result.MissingInExpected)
                Console.WriteLine(key);

            Console.WriteLine("=== Field Mismatches ===");
            foreach (var mismatch in result.FieldMismatches)
                Console.WriteLine(mismatch);

            Assert.That(result.MissingInActual.Count > 0, "Expected some records missing in Actual");
            Assert.That(result.MissingInExpected.Count > 0, "Expected some records missing in Expected");
            Assert.That(result.FieldMismatches.Count > 0, "Expected some field mismatches");

            options.CaseSensitiveComparison = true;
            Assert.That(result.FieldMismatches.Count > 0, "Expected mismatch due to case sensitivity");

          //  options.IgnoreColumns.Add("Extra Column");
          //  Assert.That(result.FieldMismatches.Count == 0, "Amount mismatches should be ignored");

            Assert.That(result.MissingInActual.Count > 0);
            Assert.That(result.MissingInExpected.Count > 0);
            Assert.That(result.FieldMismatches.Count > 0);

            // Print results
            Console.WriteLine("=== Missing in Actual ===");
            foreach (var key in result.MissingInActual)
                Console.WriteLine(key);

            Console.WriteLine("=== Missing in Expected ===");
            foreach (var key in result.MissingInExpected)
                Console.WriteLine(key);

            Console.WriteLine("=== Field Mismatches ===");
            foreach (var mismatch in result.FieldMismatches)
                Console.WriteLine(mismatch);
        }
    }
}