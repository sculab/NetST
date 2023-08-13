import os
from Bio import SeqIO
import argparse

def merge_sequences(input_folder, output_file, exts, missingchar):
    species_sequences = {}
    sequence_index = 0
    total_count = 0
    # Traverse through all FASTA files in the folder
    for filename in os.listdir(input_folder):
        if os.path.splitext(filename)[-1].lower() in exts.split(","):
            total_count += 1
    sequence_length = [0] * total_count
    for filename in os.listdir(input_folder):
        if os.path.splitext(filename)[-1].lower() in exts.split(","):
            filepath = os.path.join(input_folder, filename)
            for record in SeqIO.parse(filepath, "fasta"):
                sequence = str(record.seq)
                species = record.name
                if species not in species_sequences:
                    species_sequences[species] = [""] * total_count
                species_sequences[species][sequence_index] = sequence
                sequence_length[sequence_index] = max(sequence_length[sequence_index], len(sequence))
            sequence_index += 1

    # Fill in missing sequences
    for species, sequences in species_sequences.items():
        for i in range(len(sequences)):
            if sequences[i] == "":
                sequences[i] = missingchar * sequence_length[i]

    # Write merged sequences to a new FASTA file
    with open(output_file, "w") as output_handle:
        for species, sequences in species_sequences.items():
            output_handle.write(f">{species}\n{''.join(sequences)}\n")

if __name__ == "__main__":
    pars = argparse.ArgumentParser(formatter_class=argparse.RawDescriptionHelpFormatter, description=''' Merge Sequences YY ''')
    pars.add_argument('-input', metavar='<str>', type=str, help='''input folder.''', required=False, default='./out_gb/alignment')
    pars.add_argument('-output', metavar='<str>', type=str, help='''output folder''', required=False, default='merge.fasta')
    pars.add_argument('-exts', metavar='<str>', type=str, help='''file extensions''', required=False, default='.fasta,.fas,.fa')
    pars.add_argument('-missing', metavar='<str>', type=str, help='''character to fill missing sequences''', required=False, default='N')
    args = pars.parse_args()
    input_folder = args.input  # Replace with the folder path containing FASTA files
    output_file = args.output
    exts = args.exts
    missingchar = args.missing
    merge_sequences(input_folder, output_file, exts, missingchar)
    print("Merging completed.")
