import sys
import csv
import os
import argparse
import multiprocessing
from multiprocessing import Pool



def generate_kmers(sequence, k):
    return [sequence[i:i+k] for i in range(len(sequence) - k + 1)]

def get_similar_seq(sequence, k, kmer_set):
    count = 0
    for i in range(len(sequence) - k + 1):
        if sequence[i:i+k] in kmer_set:
            count += 1
    return count

def process_row(row, k_value, out_dir, ref_dir, max_sequence):
    seq_id = row[0]
    seq_name = row[1]
    type_list = row[3].split('|')
    support_list = row[4].split('|')
    seq_seq = row[5]
    kmer_set = set(generate_kmers(seq_seq, k_value))
    print(str(seq_id), seq_name, '\t'*8 ,end='\r')
    with open(os.path.join(out_dir, seq_id + ".fasta"), 'w') as output_file:
        output_file.write(f">{seq_name}|unknown|1\n{seq_seq}\n")
        for i in range(len(type_list)):
            file_name = os.path.join(ref_dir, f"{type_list[i]}.fasta")
            sequences_list = []
            with open(file_name, "r") as current_file:
                for line in current_file:
                    line = line.strip()
                    if line.startswith(">"):
                        current_name = line[1:]
                    else:
                        line = line.upper()
                        count = get_similar_seq(line, k_value, kmer_set)
                        if count > 0:
                            sequences_list.append([current_name, line, count])
            sequences_list = sorted(sequences_list, key=lambda x: x[2], reverse=True)
            for i in range(min(max_sequence,len(sequences_list))):
                output_file.write(f">{sequences_list[i][0]}|{sequences_list[i][2]}\n{sequences_list[i][1]}\n")

if __name__ == "__main__":
    if sys.platform.startswith('win'):
        multiprocessing.freeze_support()
    # Extract the file name from the command-line arguments
    pars = argparse.ArgumentParser(formatter_class=argparse.RawDescriptionHelpFormatter, description=''' MakeData YY ''')
    pars.add_argument('-input', metavar='<str>', type=str, help='''input file.''', required=False, default='GetType_test/test_mix.csv')
    pars.add_argument('-ref_dir', metavar='<str>', type=str, help='''reference file''', required=False, default='GetType_test/HA')
    pars.add_argument('-k', metavar='<int>', type=int, help='''k value''', required=False, default=21)
    pars.add_argument('-max', metavar='<int>', type=int, help='''max number of sequence''', required=False, default=100)
    pars.add_argument('-out_dir', metavar='<str>', type=str, help='''output folder''', required=False, default='GetType_test/mix')
    pars.add_argument('-t', metavar='<int>', type=int, help='''number of processes''', required=False, default=10)
    args = pars.parse_args()
    k_value = args.k
    input_file = args.input
    ref_dir = args.ref_dir
    out_dir = args.out_dir
    if not os.path.exists(out_dir):
        os.makedirs(out_dir)
    max_sequence = args.max
    rows = []
    with open(input_file, 'r') as file:
        # Create a CSV reader
        reader = csv.reader(file, delimiter=',')
        next(reader)
        # Iterate over each row in the CSV file
        for row in reader:
            rows.append(row)
            # Print the row
    
    num_processes = args.t
    pool = Pool(processes=num_processes)
    pool.starmap(process_row, [(row, k_value, out_dir, ref_dir, max_sequence) for row in rows])
    pool.close()
    pool.join()
