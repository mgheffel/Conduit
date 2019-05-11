#!/bin/bash -l

#Remap reads to a contig using bowtie
#Developed by Reza Mazloom rmazloom@ksu.edu
#version 1.0.0


#SBATCH --mem=16G
#SBATCH --time=2:00:00
#SBATCH --nodes=1
#SBATCH --ntasks-per-node=4
##SBATCH --partition=killable.q,batch.q
#SBATCH --job-name=remap_bowtie

module load bzip2/1.0.6-GCCcore-7.2.0
module load ncurses/6.0

PATH=$PATH:/homes/bioinfo/bioinfo_software/bowtie2-2.3.4.3:/homes/bioinfo/bioinfo_software/samtools-1.7/bin

usage() { echo "sbatch $0 -r reference -m map -o output_directory" &1>2;exit 1;}

while getopts ":r:m:o:" o;do
	case "${o}" in
		r)
			ref=${OPTARG}
			if [ ! -e $ref ]; then echo "File does not exist $ref";usage;fi
			ref=$(realpath $ref)
			;;
		m)
			map=${OPTARG}
			;;
		o)	
			outdir=${OPTARG}
			;;
		*)
			echo "Parameter not supported : ${OPTARG}"
			usage
			;;

	esac
done
if [ -z $ref ] || [ -z $map ] || [ -z $outdir ];then echo "Requeired parameter missing";usage;fi

dir=$(dirname $r1)
mkdir $outdir
outdir=$(realpath $outdir)
basename=${ref%.fa*}
basename=${basename#*Sequences}

mkdir -p ${outdir}/${basename}_index
echo "build: bowtie2-buildd -f $ref ${outdir}/${basename}_index/$basename"
bowtie2-build -f $ref ${outdir}/${basename}_index/$basename
echo
echo "map: bowtie22 -x ${sample}_index/$(basename $sample) -1 $r1 -2 $r2 --un-conc ${sample}_unmapped.fastq -p 4 > $sample.sam"
bowtie2 -x ${outdir}/${basename}_index/$basename -U $map -p 4 > ${outdir}/${basename}.sam

sample=${outdir}/${basename}
samtools view -bS $sample.sam > $sample.bam
samtools sort $sample.bam -o $sample.sorted.bam
samtools index $sample.sorted.bam
