#!/bin/bash -l

#Assemble a set of pair-end reads using the IVA assembler
#Developed by Reza Mazloom rmazloom@ksu.edu
#version 1.0.0

#Usage:
#sbatch iva_iva.sh forward_read backward_read output_file extra_configurations

#SBATCH --mem-per-cpu=8G   # Memory per core, use --mem= for memory per node
#SBATCH --time=4-00:00:00   # Use the form DD-HH:MM:SS
#SBATCH --job-name=iva-denovo
#SBATCH --nodes=1
#SBATCH --ntasks-per-node=8
##SBATCH --partition=killable.q
set -x
#module load Python/3.6.3-iomkl-2017beocatb
module load Python/3.6.3-foss-2017beocatb
module load bzip2/1.0.6-GCCcore-7.2.0
module load ncurses/6.0
module load GLib/2.53.5-GCCcore-7.2.0

sample=$(basename $1 | cut -f 1 -d ".")
outdir=$(dirname $3)
r1=$1
r2=$2
out_file=$3
config=$4

#source /homes/bioinfo/bioinfo_software/iva/activate_iva_env.sh

BIOINFOSOFTWARE=/homes/bioinfo/bioinfo_software
#source $BIOINFOSOFTWARE/iva/iva_v3/bin/activate
#export PATH=$PATH:$BIOINFOSOFTWARE/samtools-1.7:$BIOINFOSOFTWARE/MUMmer3.23/:$BIOINFOSOFTWARE/kmc_2.1.1:$BIOINFOSOFTWARE/smalt-0.7.6/bin


iva=/homes/bioinfo/bioinfo_software/miniconda/miniconda3-cp2/bin/iva

#source /homes/bioinfo/bioinfo_software/miniconda/miniconda3-cp2/bin/activate
#source ~/.bashrc
#export PATH=$PATH:$BIOINFOSOFTWARE/samtools-1.7:$BIOINFOSOFTWARE/MUMmer3.23/:$BIOINFOSOFTWARE/kmc_2.1.1:$BIOINFOSOFTWARE/smalt-0.7.6/bin

#export PATH="$PATH:/homes/bioinfo/bioinfo_software/samtools-1.7"
#export PATH="$PATH:/homes/bioinfo/bioinfo_software/kmc_2.1.1"
#export PATH="$PATH:/homes/bioinfo/bioinfo_software/MUMmer3.23/"
#export PATH="$PATH:/homes/bioinfo/bioinfo_software//smalt-0.7.6/bin"

echo "attempt"
#iva -f $r1 -r $r2 $outdir/${sample}_$SLURM_JOB_ID -t 12 $config
$iva -f $r1 -r $r2 $outdir${sample}_${SLURM_JOB_ID}ivaTag -t 8
echo "$out_file"
mv $outdir${sample}_${SLURM_JOB_ID}ivaTag/contigs.fasta $out_file
rm -r $outdir${sample}_${SLURM_JOB_ID}ivaTag
echo "$outdir${sample}_$SLURM_JOB_ID"
