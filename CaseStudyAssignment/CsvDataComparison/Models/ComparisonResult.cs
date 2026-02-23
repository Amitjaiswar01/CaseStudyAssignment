
using System.Collections.Generic;

namespace CaseStudyAssignment.CsvDataComparison.Models
{
    /// <summary>
    /// Holds final comparison result.
    /// </summary>
    public class ComparisonResult
    {
        public List<string> MissingInActual { get; } = new List<string>();
        public List<string> MissingInExpected { get; } = new List<string>();
        public List<string> FieldMismatches { get; } = new List<string>();
        public List <string> DuplicateKeys { get;} = new List<string>();
    }
}
