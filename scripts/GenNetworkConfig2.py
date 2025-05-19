#!/usr/bin/python3
# -*- coding: utf-8 -*-

# *************************************************************************
#    > File Name: GenNetworkConfig.py
#    > Author: xlzh
#    > Mail: xiaolongzhang2015@163.com 
#    > Created Time: 2021年12月02日 星期四 22时20分30秒
# *************************************************************************
# Modified By YY 2023/7/9

import sys
import csv
import random


# python GenNetworkConfig.py GenNetworkConfig_Test/test.gml GenNetworkConfig_Test/test_seq2hap.csv GenNetworkConfig_Test/test
# dist\all\GenNetworkConfig.exe GenNetworkConfig_Test/test.gml GenNetworkConfig_Test/test_seq2hap.csv GenNetworkConfig_Test/test


def gen_hap_config(hap_file):
    ''' func: generate the haplotype config
        hap_conf_list = [(sample1, China,hap1), (sample2, Korean,hap2), ...]
    '''
    hap_conf_list = []

    # Open the CSV file
    with open(hap_file, mode='r', newline='', encoding='utf-8') as file:
        reader = csv.reader(file)
        next(reader)  # Skip the header row
        for row in reader:
            # Create a tuple from id, hap, name, and trait, but only include relevant parts
            sample = row[2]
            hap = row[1].replace("Hap_", "H")
            group = row[3]
            hap_conf_list.append((sample, group, hap))

    return hap_conf_list


def _random_color(seed):
    ''' func: generate color randomly based on the specified seed
    '''
    random.seed(seed)

    colorArr = ['1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F']
    color_list = []

    for i in range(6):
        color_list.append(colorArr[random.randint(0, 14)])

    return '#' + ''.join(color_list)


def text_to_integer(text):
    if not text:
        return 0
    # 将文本中的每个字符映射成对应的ASCII码，并拼接在一起
    integer_value = int(''.join(str(ord(char)) for char in text))
    return integer_value


def gen_group_config(hap_conf_list):
    ''' func: generate the group config
        group_conf_list = [(China, '#FF011B', 'none'), (Korean, '2693FF', 'none'), ...]
    '''
    group_conf_list = []
    count_dict = {}

    for item in hap_conf_list:
        if item[1] != 'Default' and item[1] not in count_dict:
            rand_color = _random_color(text_to_integer(item[1]))
            print(text_to_integer(item[1]), rand_color)
            count_dict[item[1]] = rand_color
            group_conf_list.append((item[1], rand_color, 'none'))

    return group_conf_list


color = {3: ("#D85356", "#D7AA36", "#94BBAD"), 4: ("#F0776D", "#FBD2CB", "#29B4B6", "#C5E7E8"),
         5: ("#8386A8", "#D15C6B", "#F5CF36", "#8FB943", "#78B9D2"),
         6: ("#DA352A", "#FF8748", "#5BAA56", "#B8BB5B", "#4186B7", "#8679BE"),
         7: ("#794292", "#44599B", "#2C8FA0", "#40A93B", "#EFE644", "#E97124", "#DF4442"),
         8: ("#FF7F00", "#FDBF6F", "#E31A1C", "#FB9A99", "#33A02C", "#B2DF8A", "#1F78B4", "#A6CEE3"),
         9: ("#E71F19", "#2CB8BB", "#55B333", "#6A1B86", "#E2C9E1", "#A4C5E8", "#EF7A1C", "#F9CA9C", "#F7C3C4"),
         10: (
             "#009F72", "#D25F27", "#E49E21", "#EFE341", "#1F78B4", "#FB9A99", "#E21A1C", "#EAD59F", "#C9B1D4",
             "#52310F")}


# func: Generate a collection of different numbers of colors
def color_group_config(hap_conf_list):
    # Extracting unique group names
    group_name_list = list(set(group_name for _, group_name, _ in hap_conf_list))
    color_number = len(group_name_list)
    # Selecting colors for each group
    if color_number in color:
        return [(group_name, color[color_number][i], 'none') for i, group_name in enumerate(group_name_list)]
    else:
        # Handle cases where color_number is not in color.keys(), assuming gen_group_config is defined
        return gen_group_config(hap_conf_list)


def write_conf(hap_conf_list, group_conf_list, out_prefix):
    ''' func: write the config file
    '''
    # write the haplotype config file
    hap_fp = open(out_prefix + '_hapconf.csv', 'w')
    for hap in hap_conf_list:
        hap_fp.write("%s;%s;%s\n" % (hap[0], hap[1], hap[2]))

    # write the group config file
    group_fp = open(out_prefix + '_groupconf.csv', 'w')
    for group in group_conf_list:
        group_fp.write("%s;%s;%s\n" % (group[0], group[1], group[2]))

    hap_fp.close()
    group_fp.close()


def file2line(file_name):
    content = ""
    with open(file_name, 'r') as file_fp:
        content = file_fp.read()
        content = content.replace('\"', "\\\"").replace('\n', "\\n")
    return content


def main():
    args = sys.argv
    if len(args) != 4:
        sys.stderr.write("usage: python GetNetworkConfig.py <in.gml> <in.seq2hap.csv> <out_prefix>\n")
        sys.exit(-1)
    gml_file = args[1]
    hap_file = args[2]
    out_prefix = args[3]

    hap_conf_list = gen_hap_config(hap_file)
    group_conf_list = color_group_config(hap_conf_list)
    write_conf(hap_conf_list, group_conf_list, out_prefix)

    data_fp = open(out_prefix + '.js', 'w')
    data_fp.write("var gmlfile = {target: {files: [new File([\"")
    data_fp.write(file2line(gml_file))
    data_fp.write("\"], \".gml\")]}};\n")

    data_fp.write("var hapconffile = {target: {files: [new File([\"")
    data_fp.write(file2line(out_prefix + '_hapconf.csv'))
    data_fp.write("\"], \".gml\")]}};\n")

    data_fp.write("var groupconffile = {target: {files: [new File([\"")
    data_fp.write(file2line(out_prefix + '_groupconf.csv'))
    data_fp.write("\"], \".gml\")]}};\n")
    data_fp.close()


if __name__ == '__main__':
    main()
