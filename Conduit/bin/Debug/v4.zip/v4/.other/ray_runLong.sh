#!/bin/bash -l

#Assemble pair-end reads with the Ray assembly
#Developed by Reza Mazloom rmazloom@ksu.edu
#version 1.0.0
#adapted from Majed Alsadhan

#Usage:
#qsub ray_run.sh forward_read backward_read result_file

#SBATCH --nodes=1
#SBATCH --ntasks-per-node=4 
#SBATCH --mem-per-cpu=10G   # Memory per core, use --mem= for memory per node
#SBATCH --time=2-00:00:00   # Use the form DD-HH:MM:SS
##SBATCH --partition=killable.q   # Job may run as killable on owned nodes
#SBATCH --job-name=ray-denovo
#SBATCH --gres=fabric:ib:1


module load Python/3.6.3-iomkl-2017beocatb
RAYDIR="/bulk/dmarth027/QA_QC_pipeline/software/ray"
export PATH="$PATH:$RAYDIR"
export OMP_NUM_THREADS=2

sample=$(basename $1 | cut -f 1 -d ".")
outdir=$(dirname $3)
r1=$1
r2=$2
out_file=$3
config=$4

mpirun -np $SLURM_NTASKS Ray -p $r1 $r2 -o $outdir/${sample}_$SLURM_JOB_ID $config < /dev/null

mv $outdir/${sample}_$SLURM_JOB_ID/Contigs.fasta $out_file
rm -r $outdir/${sample}_$SLURM_JOB_ID
