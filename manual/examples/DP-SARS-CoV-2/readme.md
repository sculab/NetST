DP dataset are generated from the following work:
Sekizuka T, Itokawa K, Kageyama T, Saito S, Takayama I, Asanuma H, Nao N, Tanaka R, Hashino M, Takahashi T, et al. 2020. Haplotype networks of SARS-CoV-2 infections in the Diamond Princess cruise ship outbreak. Proc. Natl. Acad. Sci. U.S.A. 117:20198–20201.
Please cite the above reference if you use the DP dataset.


•	meta.tsv: A tab-separated text file containing metadata for the samples, including Virus name, GISAID ID, Date, and Location.
•	format_seq.fasta: A FASTA file containing sample sequences and related information, organized based on the data in meta.tsv and snv.tsv. Each sequence ID includes the sample’s Virus name, quality, Date, and Location, with the Date represented as the number of days since the discovery of Wuhan-Hu-1.
•	ref_seq.fasta: The reference sequence of the first SARS-CoV-2 sample, Wuhan-Hu-1.
•	snv.tsv: A tab-separated text file containing SNV sequences for all samples.