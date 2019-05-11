#!/bin/bash -l

#SBATCH --nodes=1
#SBATCH --ntasks-per-node=4
#SBATCH --mem-per-cpu=12G   # Memory per core, use --mem= for memory per node
#SBATCH --time=24:00:00   # Use the form DD-HH:MM:SS
##SBATCH --partition=killable.q   # Job may run as killable on owned nodes
#SBATCH --job-name=a5-denovo

module load Perl/5.26.0-iompi-2017beocatb
module load Java/1.8.0_144

outfile=$3
cwd=$SLURM_SUBMIT_DIR
sample=$(basename $1 | awk -F"_R1" '{print $1}')
wd=$(dirname $1)

mkdir $wd/$sample.$SLURM_JOB_ID
cd $wd/$sample.$SLURM_JOB_ID

/homes/bioinfo/bioinfo_software/a5_miseq_linux_20160825/bin/a5_pipeline.pl $1 $2 $sample

mv $wd/$sample.$SLURM_JOB_ID/$sample.contigs.fasta $outfile
rm -r $wd/$sample.$SLURM_JOB_ID

cd $cwd
