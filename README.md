The project is about comparing two CSV files — one “Expected” file and one “Actual” file — to find differences. It answers three main questions:
1.	Missing Records
o	Which records are present in Expected but not in Actual?
o	Which records are present in Actual but not in Expected?
2.	Field Level Mismatches
o	For records that exist in both files, are there any differences in their field values?
o	Example: Expected has SCOPE = 533.57 but Actual has SCOPE = 533.97.
3.	Flexibility & Efficiency
o	The program works efficiently even for very large files (100MB+).
o	It allows customization: you can define which columns are keys, whether comparisons are case sensitive, and which columns to ignore.
So in simple terms: It’s a tool that tells you what’s missing, what’s extra, and what’s mismatched between two CSV files.
