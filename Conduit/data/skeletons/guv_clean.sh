#!/bin/bash
#SBATCH --time=1:00:00
#SBATCH --mem-per-cpu=4G
#SBATCH --job-name=select


set -x

select=/homes/bioinfo/vdl/v2/.other/select_sequences.pl
sample_name=$1
p1=$(realpath $2)
p2=$(realpath $3)
output=$(realpath $4)

cat $output/$sample_name.selectu <(cut -f 1 $output/$sample_name.selectt)  > $output/$sample_name.select
perl $select $output/$sample_name.select $p1 $output/$(basename $p1 .fastq).fastq
perl $select $output/$sample_name.select $p2 $output/$(basename $p2 .fastq).fastq
rm $output/$sample_name.select*
