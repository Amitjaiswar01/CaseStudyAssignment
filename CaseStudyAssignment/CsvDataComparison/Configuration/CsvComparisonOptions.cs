using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseStudyAssignment.CsvDataComparison.Configuration
{
    /// <summary>
    /// Configuration for CSV comparison.
    /// Allows future customization.
    /// </summary>
    public class CsvComparisonOptions
    {
        /// <summary>
        /// List of primary/composite key columns.
        /// </summary>
        public List<string> KeyColumns { get; set; } = new List<string>();

        /// <summary>
        /// Case sensitivity option.
        /// </summary>
        public bool CaseSensitiveComparison { get; set; } = false;

        /// <summary>
        /// Optional columns to ignore during comparison.
        /// </summary>
        public List<string> IgnoreColumns { get; set; } = new List<string>();
    }
}
