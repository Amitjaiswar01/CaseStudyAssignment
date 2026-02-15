using CaseStudyAssignment.CsvDataComparison.Configuration;
using CaseStudyAssignment.CsvDataComparison.Services;
using NUnit.Framework;
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
            _expectedPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Expected.csv");
            _actualPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData", "Actual.csv");
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

            Assert.That(result.MissingInActual.Count > 0);
            Assert.That(result.MissingInExpected.Count > 0);
            Assert.That(result.FieldMismatches.Count > 0);
        }
    }
}