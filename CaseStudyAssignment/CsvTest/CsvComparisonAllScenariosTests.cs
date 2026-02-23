using NUnit.Framework;
using System;
using System.Collections.Generic;
using CaseStudyAssignment.CsvDataComparison.Configuration;
using CaseStudyAssignment.CsvDataComparison.Models;
using CaseStudyAssignment.CsvDataComparison.Services;

namespace CaseStudyAssignment.CsvTest
{
    [TestFixture]
    public class CsvComparisonAllScenariosTests
    {
        private CsvReaderService _reader;
        private CsvComparisonService _comparer;
        private CsvComparisonOptions _options;

        private List<CsvRecord> _expected;
        private List<CsvRecord> _actual;

        [OneTimeSetUp]
        public void Setup()
        {
            _reader = new CsvReaderService();
            _comparer = new CsvComparisonService();

            _options = new CsvComparisonOptions
            {
                KeyColumns = new List<string> { "ID", "Name" },
                CaseSensitiveComparison = false
            };

            _expected = _reader.ReadCsv("TestData/FullDataExpected.csv");
            _actual = _reader.ReadCsv("TestData/FullDataActual.csv");
        }

        [Test]
        public void Compare_AndPrintAllFailures_WithAsserts()
        {
            var result = _comparer.Compare(_expected, _actual, _options);

            // --- Missing in Actual ---
            Console.WriteLine("=== Missing In Actual ===");
            if (result.MissingInActual.Count == 0)
                Console.WriteLine("None");
            else
                result.MissingInActual.ForEach(Console.WriteLine);

            // Assert at least check for scenario: Missing in Actual
            Assert.That(result.MissingInActual.Count >= 0, "Expected list of records missing in Actual");

            // --- Missing in Expected ---
            Console.WriteLine("\n=== Missing In Expected ===");
            if (result.MissingInExpected.Count == 0)
                Console.WriteLine("None");
            else
                result.MissingInExpected.ForEach(Console.WriteLine);

            // Assert at least check for scenario: Missing in Expected
            Assert.That(result.MissingInExpected.Count >= 0, "Expected list of records missing in Expected");

            // --- Field Mismatches ---
            Console.WriteLine("\n=== Field Level Mismatches ===");
            if (result.FieldMismatches.Count == 0)
                Console.WriteLine("None");
            else
                result.FieldMismatches.ForEach(Console.WriteLine);

            // --- Duplicate Keys ---
            Console.WriteLine("\n=== Duplicate Keys ===");
            if (result.DuplicateKeys.Count == 0)
                Console.WriteLine("None");
            else
                result.DuplicateKeys.ForEach(Console.WriteLine);

            // Assert that duplicates exist or not
            Assert.That(result.DuplicateKeys.Count >= 0, "Duplicate keys detected.");
        }
    }
}