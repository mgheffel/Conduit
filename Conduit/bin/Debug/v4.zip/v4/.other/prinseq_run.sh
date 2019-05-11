#!/bin/bash
#
#
#       Originally written by Rylan Kasitz rylankasitz@ksu.edu
#       5/25/2018
#
#SBATCH --mem=8g

PRINSEQ="/homes/rylankasitz/bin/prinseq"
prinseq_run=$PRINSEQ/prinseq.pl
prinseq_graph=$PRINSEQ/prinseq-graph.pl

input=$1
output=$2
len_o=$3
parentdir=$4
echo "test"

perl $prinseq_run -fastq $input -graph_data $output.gd -out_good null -out_bad null

len_n=$(find $cd -type f -name "*.gd" | wc -l)
echo $len_n
echo $len_o
if [ $len_n == $len_o ]
then
    #touch $parentdir'/prinseq.done'
fi
