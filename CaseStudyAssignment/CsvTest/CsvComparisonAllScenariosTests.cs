using CaseStudyAssignment.CsvDataComparison.Configuration;
using CaseStudyAssignment.CsvDataComparison.Services;
using NUnit.Framework;
using System.Collections.Generic;

namespace CaseStudyAssignment.CsvTest
{
    [TestFixture]
    public class CsvComparisonAllScenariosTests
    {
        private CsvReaderService _reader;
        private CsvComparisonService _comparer;
        private CsvComparisonOptions _options;

        // Shared data for all tests
        private Dictionary<string, CsvDataComparison.Models.CsvRecord> _expected;
        private Dictionary<string, CsvDataComparison.Models.CsvRecord> _actual;

        [OneTimeSetUp]
        public void GlobalSetup()
        {
            _reader = new CsvReaderService();
            _comparer = new CsvComparisonService();
            _options = new CsvComparisonOptions
            {
                KeyColumns = new List<string> { "ID", "Name" }
            };

            // Load once for all tests
            _expected = _reader.ReadCsv("TestData/FullDataExpected.csv", _options.KeyColumns);
            _actual = _reader.ReadCsv("TestData/FullDataActual.csv", _options.KeyColumns);
        }

        private void PrintResults(CsvDataComparison.Models.ComparisonResult result)
        {
            TestContext.WriteLine("=== Missing in Actual ===");
            foreach (var key in result.MissingInActual)
                TestContext.WriteLine(key);

            TestContext.WriteLine("=== Missing in Expected ===");
            foreach (var key in result.MissingInExpected)
                TestContext.WriteLine(key);

            TestContext.WriteLine("=== Field Mismatches ===");
            foreach (var mismatch in result.FieldMismatches)
                TestContext.WriteLine(mismatch);
        }

        [Test]
        public void Scenario_MissingInActual()
        {
            var result = _comparer.Compare(_expected, _actual, _options);
            PrintResults(result);

            Assert.That(result.MissingInActual.Count > 0, "Expected some records missing in Actual");
            Assert.That(result.MissingInExpected.Count == 0, "Expected no records missing in Expected for this scenario");
            Assert.That(result.FieldMismatches.Count == 0, "Expected no field mismatches for this scenario");
        }

        [Test]
        public void Scenario_MissingInExpected()
        {
            var result = _comparer.Compare(_expected, _actual, _options);
            PrintResults(result);

            Assert.That(result.MissingInActual.Count == 0, "Expected no records missing in Actual for this scenario");
            Assert.That(result.MissingInExpected.Count > 0, "Expected some records missing in Expected");
            Assert.That(result.FieldMismatches.Count == 0, "Expected no field mismatches for this scenario");
        }

        [Test]
        public void Scenario_FieldMismatch()
        {
            var result = _comparer.Compare(_expected, _actual, _options);
            PrintResults(result);

            Assert.That(result.MissingInActual.Count == 0, "Expected no records missing in Actual for this scenario");
            Assert.That(result.MissingInExpected.Count == 0, "Expected no records missing in Expected for this scenario");
            Assert.That(result.FieldMismatches.Count > 0, "Expected some field mismatches");
        }

        [Test]
        public void Scenario_AllFailuresTogether()
        {
            var result = _comparer.Compare(_expected, _actual, _options);
            PrintResults(result);

            Assert.That(result.MissingInActual.Count > 0, "Expected some records missing in Actual");
            Assert.That(result.MissingInExpected.Count > 0, "Expected some records missing in Expected");
            Assert.That(result.FieldMismatches.Count > 0, "Expected some field mismatches");
        }

        [Test]
        public void Scenario_CaseSensitivity()
        {
            _options.CaseSensitiveComparison = true;

            var result = _comparer.Compare(_expected, _actual, _options);
            PrintResults(result);

            Assert.That(result.FieldMismatches.Count > 0, "Expected mismatch due to case sensitivity (SIGMA vs sigma)");
        }
    }
}
