using System;
using System.Collections.Generic;

namespace CaseStudyAssignment.CsvDataComparison.Configuration
{
    //  These three members (KeyColumns, CaseSensitiveComparison, IgnoreColumns) are the properties of CsvComparisonOptions class.
    //  They’re public, so they can be set by the caller to control how the comparison behaves.
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
