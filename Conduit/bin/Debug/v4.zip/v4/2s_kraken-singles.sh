#!/bin/bash
#
#       Written by Matthew G. Heffel, mgheffel@ksu.edu
#       11/13/2018
#
#

#SBATCH --nodes=1
#SBATCH --ntasks-per-node=16
#SBATCH --time=18:00:00   # Use the form DD-HH:MM:SS
#SBATCH --partition=killable.q   # Job may run as killable on owned nodes
#SBATCH --mem-per-cpu=14G   # Memory per core, use --mem= for memory per node
#SBATCH --job-name=sKraken

##module load libtool/2.4.6-GCCcore-7.2.0
##module load Anaconda3/5.0.1
#module load GCC/7.2.0-2.29
#module load GCCcore/7.2.0
#module load OpenMPI/2.1.1-iccifort-2018.0.128-GCC-7.2.0-2.29
#module load Perl/5.26.0-iompi-2017beocatb
##module load GLib/2.53.5-GCCcore-7.2.0


HERE="/homes/bioinfo/vdl/v4"
KRAKENDB_DIR=/bulk/bioinfo/vdl/.dependencies/krakenDBs
KRAKEN_RUN=$HERE/.other/kraken_run.sh
log=$HERE/logs/

usage() { echo "Usage: bash $0 -i trimmed_reads_directory -d kraken_database [-k extra_kraken_parameters] [-c job_submit_configuation]" 1>&2; exit 1;}

while getopts ":i:c:d:k:" o; do
        case "${o}" in
                i)
                        input=${OPTARG}
                        if [ ! -d "$input" ]; then usage; fi
			input=$(realpath -s $input)
                        ;;
                d)
                        database=${OPTARG}
			if [ ! -d $KRAKENDB_DIR/$database ]; then echo "The database $database does not exist"; fi
	                ;;
		k)
			kraken_params=${OPTARG}
			;;
		c)	
			config=${OPTARG}
			;;
		*)
			usage
			;;
	esac
done

if [ -z $input ]; then usage;fi

results="${input%raw_cleaned*}raw_kraken"
mkdir $results

export PATH="$PATH:/bulk/bioinfo/vdl/.dependencies/software/jellyfish-1.1.11/bin"
export PATH="$PATH:/homes/bioinfo/bioinfo_software/kraken-1.0"
export LD_LIBRARY_PATH="/bulk/bioinfo/vdl/.dependencies/software/jellyfish-1.1.11/.libs"
export PKG_CONFIG_PATH="$PATH:/bulk/bioinfo/vdl/.dependencies/software/jellyfish-1.1.11/"
export KRAKEN_DB_PATH="/bulk/bioinfo/vdl/.dependencies/krakenDBs"

for file in $input/*
do
	echo "Classifying reads from $file"
	filename=${file#*raw_cleaned/}
	kraken --db $DB $database --threads 12 --output ${input%raw_cleaned*}raw_kraken/${filename%.fastq*}.kraken $file
done
#sbatch --dependency $JID $chain $input/../2_kraken.done &>/dev/null

touch 2s_kraken.done
echo
echo "The Job named \"Kraken\" was submitted. It would probably take from 10 to 50 minutes to finish..."
echo "You can find the results in the directory $results"
echo
echo "After that, you can run: bash 3_krona.sh -K $results"
echo
echo
echo "Thank you!"
