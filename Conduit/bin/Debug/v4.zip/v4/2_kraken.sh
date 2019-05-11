#!/bin/bash
#
#       Updated v1.1 by Reza Mazloom rmazloom@ksu.edu
#       10/19/2017
#
#       originally written by Majed Alsadhan
#
#

HERE="/homes/bioinfo/vdl/v4"
KRAKENDB_DIR=/bulk/bioinfo/vdl/.dependencies/krakenDBs
KRAKEN_RUN=$HERE/.other/kraken_run.sh
log=$HERE/logs/

usage() { echo "Usage: bash $0 -i trimmed_reads_directory -d kraken_database [-k extra_kraken_parameters] [-c job_submit_configuation]" 1>&2; exit 1;}

while getopts ":i:c:d:k:b:" o; do
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
		b)
			debugDir=${OPTARG}
			;;
		*)
			usage
			;;
	esac
done

if [ -z $input ]; then usage;fi

mkdir $debugDir

results="$(echo $input | rev | cut -f 2- -d '_' | rev)_kraken"

echo
echo
echo "subbmitting a job with database: $database  and input directory: $input"
#qsub $config -N "Kraken" -l mem=14G,h_rt=3:00:00 -e $log"Kraken.log" -o $log"Kraken.log" -pe single 12 $KRAKEN_RUN "$input $DB $results" &>/dev/null
JID=$(sbatch -o ${debugDir}/krakenDebug.out $config $KRAKEN_RUN $input $database $results $kraken_params)
JID=$(echo $JID | rev | cut -f 1 -d ' '|rev)
sbatch --dependency $JID $chain $input/../2_kraken.done &>/dev/null
echo
echo "The Job named \"Kraken\" was submitted. It would probably take from 10 to 50 minutes to finish..."
echo "You can find the results in the directory $results"
echo
echo "After that, you can run: bash 3_krona.sh -K $results"
echo
echo
echo "Thank you!"
