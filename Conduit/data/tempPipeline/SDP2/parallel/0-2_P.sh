#!/bin/sh
#SBATCH --nodes=1
#SBATCH --ntasks-per-node=16
#SBATCH --mem-per-cpu=14G   # Memory per core, use --mem= for memory per node
#SBATCH --job-name=Kraken
#SBATCH --time=18:00:00


export PATH="$PATH:/bulk/bioinfo/vdl/.dependencies/software/jellyfish-1.1.11/bin"
export PATH="$PATH:/homes/bioinfo/bioinfo_software/kraken-1.0"
export LD_LIBRARY_PATH="/bulk/bioinfo/vdl/.dependencies/software/jellyfish-1.1.11/.libs"
export PKG_CONFIG_PATH="$PATH:/bulk/bioinfo/vdl/.dependencies/software/jellyfish-1.1.11/"
export KRAKEN_DB_PATH="/bulk/bioinfo/vdl/.dependencies/krakenDBs"

input=$1
db=$2
results=$3
N=16

rm -rf $results
mkdir $results

paste <(ls -1 $input/*_R1*) <(ls -1 $input/*_R2*) | while IFS="$(printf '\t')" read -r  p1 p2 ;
do
  out=$(basename $p1 .fastq | cut -f 2- -d "_" | awk -F'_R1' '{print $1}').kraken
  kraken --db $db --threads $N --preload --paired $p1 $p2 --output $results/$out

done 

