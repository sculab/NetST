import sys
import os
import csv
import gzip
import argparse
pars = argparse.ArgumentParser(formatter_class=argparse.RawDescriptionHelpFormatter, description=''' MakeData YY ''')
pars.add_argument('-input', metavar='<str>', type=str, help='''input file.''', required=False, default = 'GetType_test/target.fasta')
pars.add_argument('-db', metavar='<str>', type=str, help='''input file.''', required=False, default = 'GetType_test/21.gz')
pars.add_argument('-k', metavar='<int>', type=int, help='''k value''', required=False, default = 21)
pars.add_argument('-ref', metavar='<str>', type=str, help='''reference file''', required=False, default = 'GetType_test/input.fasta')
pars.add_argument('-cut', metavar='<int>', type=int, help='''max number of sequence''', required=False, default = 85)
pars.add_argument('-output', metavar='<str>', type=str, help='''output folder''', required=False, default = 'GetType_test/all')
args = pars.parse_args()
def generate_kmers(sequence, k, trans = False):
    kmers = []
    if trans:
        sequence = sequence.translate(str.maketrans('ACGT', '0110'))
    for i in range(len(sequence) - k + 1):
        kmer = sequence[i:i+k]
        kmers.append(kmer)
    return kmers

def count_kmers_from_file(fasta_file):
    print("Loading k-mer database ...")
    kmer_dict = {}
    with gzip.open(fasta_file, "rt") as file:  # Open the file in gzip mode for reading
        for line in file:
            line = line.strip()
            kmer, sequence_name = line.split("\t")
            kmer_dict[kmer] = sequence_name
    return kmer_dict

def count_kmers(fasta_file, k, output_file):
    if os.path.isfile(output_file):
        kmer_dict  = count_kmers_from_file(output_file)
    else:
        kmer_dict = {}
        current_type = ""
        count = 0
        with open(fasta_file, "r") as file:
            for line in file:
                line = line.strip()
                if line.startswith(">"):
                    current_type = line[1:].split('|')[1]
                    count += 1
                    print(count, end='\r')
                else:
                    kmers = generate_kmers(line, k)
                    for kmer in kmers:
                        if kmer in kmer_dict:
                            kmer_dict[kmer].add(current_type)
                        else:
                            kmer_dict[kmer] = {current_type}

        for kmer in list(kmer_dict.keys()):
            if len(kmer_dict[kmer]) >= 2:
                kmer_dict.pop(kmer)
            else:
                kmer_dict[kmer] = list(kmer_dict[kmer])[0]
        print("writing kmer dict ...")
        with gzip.open(output_file, "wt") as file:  # Open the file in gzip mode for writing
            for kmer, sequence_name in kmer_dict.items():
                file.write(f"{kmer}\t{sequence_name}\n")

    return kmer_dict

def count_kmer_varieties(target_dict, ref_dict):
    variety_dict = {}
    for kmer in target_dict.keys():
        if kmer in ref_dict:
            my_type = ref_dict[kmer]
            if my_type in variety_dict:
                variety_dict[my_type] += 1
            else:
                variety_dict[my_type] = 1
    return variety_dict,  len(target_dict),

def sort_varieties(variety_dict):
    sorted_varieties = sorted(variety_dict.items(), key=lambda x: x[1], reverse=True)
    return sorted_varieties

def count_kmers_target(fasta_file, ref_dict, result_file, cut_value):
    k = len(next(iter(ref_dict)))
    kmer_dict = {}
    sequence_name = ""
    sequence_seq = ""
    seq_length = 0
    my_id = 0
    with open(fasta_file, "r") as file:
        for line in file:
            line = line.strip()
            if line.startswith(">"):
                my_id += 1
                if sequence_name:
                    kmer_varieties, total_no = count_kmer_varieties(kmer_dict, ref_dict)
                    sorted_varieties = sort_varieties(kmer_varieties)
                    total_count = 0
                    for _, count in sorted_varieties:
                        total_count += count
                    with open(result_file + ".csv", "a", newline='') as rf:
                        writer = csv.writer(rf, delimiter=',')
                        for my_type, count in sorted_varieties:
                            writer.writerow([my_id, sequence_name, seq_length, my_type, 
                                            f"{int(count / total_count * 100)}%",
                                            f"{kmer_varieties[my_type]}",
                                            f"{total_no}",
                                            ])
                    if len(sorted_varieties) > 0:
                        if sorted_varieties[0][1] / total_count * 100 < float(cut_value):
                            with open(result_file + "_mix.csv", "a", newline='') as rf:
                                writer = csv.writer(rf, delimiter=',')
                                my_types = []
                                supports = []
                                for my_type, count in sorted_varieties:
                                    my_types.append(my_type)
                                    supports.append(str(int(count / total_count * 100)))
                                writer.writerow([my_id, sequence_name, seq_length, '|'.join(my_types),
                                '|'.join(supports), sequence_seq])
                    else:
                        with open(result_file + "_null.csv", "a", newline='') as rf:
                            writer = csv.writer(rf, delimiter=',')
                            writer.writerow([my_id, sequence_name, seq_length, sequence_seq])
                    kmer_dict = {}
                sequence_name = line[1:]
                print(sequence_name,end='\r')
            else:
                kmers = generate_kmers(line, k)
                sequence_seq = line
                seq_length = len(line)
                for kmer in kmers:
                    if kmer in kmer_dict:
                        kmer_dict[kmer] += 1
                    else:
                        kmer_dict[kmer] = 1

    kmer_varieties, total_no = count_kmer_varieties(kmer_dict, ref_dict)
    sorted_varieties = sort_varieties(kmer_varieties)
    total_count = 0
    for _, count in sorted_varieties:
        total_count += count
    with open(result_file + ".csv", "a", newline='') as rf:
        writer = csv.writer(rf, delimiter=',')
        for my_type, count in sorted_varieties:
            writer.writerow([my_id, sequence_name, seq_length, my_type, 
                            f"{int(count / total_count * 100)}%",
                            f"{kmer_varieties[my_type]}",
                            f"{total_no}",
                            ])
        
    if len(sorted_varieties) > 0:
        if sorted_varieties[0][1] / total_count * 100 < float(cut_value):
            with open(result_file + "_mix.csv", "a", newline='') as rf:
                writer = csv.writer(rf, delimiter=',')
                my_types = []
                supports = []
                for my_type, count in sorted_varieties:
                    my_types.append(my_type)
                    supports.append(str(int(count / total_count * 100)))
                writer.writerow([my_id, sequence_name, seq_length, '|'.join(my_types),
                '|'.join(supports), sequence_seq])
    else:
        with open(result_file + "_null.csv", "a", newline='') as rf:
            writer = csv.writer(rf, delimiter=',')
            writer.writerow([my_id, sequence_name, seq_length, sequence_seq])

if __name__ == "__main__":
    k_value = args.k
    dict_file = args.db
    ref_file = args.ref
    target_file = args.input
    result_file = args.output
    cut_value = args.cut
    kmer_dict_ref = count_kmers(ref_file, k_value, dict_file)
    with open(result_file + ".csv", "w", newline='') as rf:  
        writer = csv.writer(rf, delimiter=',')
        writer.writerow(["ID", "name", "length", "Type", "Support", "hit no","total no"])

    with open(result_file + "_mix.csv", "w", newline='') as rf:  
        writer = csv.writer(rf, delimiter=',')
        writer.writerow(["ID", "name", "length", "Type", "Support", "sequence"])
    
    with open(result_file + "_null.csv", "w", newline='') as rf:  
        writer = csv.writer(rf, delimiter=',')
        writer.writerow(["ID", "name", "length", "sequence"])

    count_kmers_target(target_file, kmer_dict_ref, result_file, cut_value)
