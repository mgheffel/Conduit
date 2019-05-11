#!/bin/bash -l
#
#       Updated v1.1 by Reza Mazloom rmazloom@ksu.edu
#       10/18/2017
#
#       originally written by Majed Alsadhan



#SBATCH --nodes=1
#SBATCH --ntasks-per-node=16
#SBATCH --time=3:00:00   # Use the form DD-HH:MM:SS
#SBATCH --partition=killable.q   # Job may run as killable on owned nodes
#SBATCH --mem-per-cpu=14G   # Memory per core, use --mem= for memory per node
#SBATCH --job-name=Kraken

##module load libtool/2.4.6-GCCcore-7.2.0
##module load Anaconda3/5.0.1
#module load GCC/7.2.0-2.29
#module load GCCcore/7.2.0
#module load OpenMPI/2.1.1-iccifort-2018.0.128-GCC-7.2.0-2.29
#module load Perl/5.26.0-iompi-2017beocatb
##module load GLib/2.53.5-GCCcore-7.2.0
usage() { echo "sbatch $0 input_directory databse_name result_directory [kraken_parameters]" ; exit 1; }

export PATH="$PATH:/bulk/bioinfo/vdl/.dependencies/software/jellyfish-1.1.11/bin"
export PATH="$PATH:/homes/bioinfo/bioinfo_software/kraken-1.0"
export LD_LIBRARY_PATH="/bulk/bioinfo/vdl/.dependencies/software/jellyfish-1.1.11/.libs"
export PKG_CONFIG_PATH="$PATH:/bulk/bioinfo/vdl/.dependencies/software/jellyfish-1.1.11/"
export KRAKEN_DB_PATH="/bulk/bioinfo/vdl/.dependencies/krakenDBs"

#if (($# < 3 )) || (($# > 4)); then usage; fi
    
input=$1
db=$2
results=$3
params=$4
N=16

rm -rf $results
mkdir $results

paste <(ls -1 $input/*_R1*) <(ls -1 $input/*_R2*) | while IFS="$(printf '\t')" read -r  p1 p2 ;
do
  out=$(basename $p1 .fastq | cut -f 2- -d "_" | awk -F'_R1' '{print $1}').kraken
  kraken \
  $params \
  --db $db \
  --threads $N \
  --preload \
  --paired $p1 $p2 \
  --report $results/report.txt \
  --output $results/$out
done 
touch $input/../2_kraken.done
