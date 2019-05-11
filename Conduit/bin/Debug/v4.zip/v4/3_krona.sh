#!/bin/bash
#$ -cwd'
# 
#       Updated v1.1 by Reza Mazloom rmazloom@ksu.edu
#       10/19/2017
#
#       originally written by Majed Alsadhan
#
#SBATCH --mem-per-cpu=32G

HERE="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
KRONA_RUN="/homes/bioinfo/vdl/v4/.other/krona_run.sh"
log=$HERE/logs/

usage() { echo "Usage: bash $0 -k kraken_result_directory [-c submit_parameters]" 1>&2; exit 1;}

while getopts ":k:c:b:" o; do
        case "${o}" in
	                k)
	                        input=${OPTARG}
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
if [ -z $input ]; then usage; fi
input=$(realpath -s $input)

mkdir $debugDir

if [ ! -d $input ]; then echo "$input is not a valide directory!" ;fi

echo 
echo
echo "Processing files..."
echo
for f in $input/*.kraken
do
  echo $(basename $f)
  cut -f 3 $f | sort | uniq -c | sort -nr | awk ' { reads = $1; $1 = $2; $3 = reads; print; }' OFS=$'\t' > $input"/"$(basename $f .kraken)".krn"
done

krn=$(ls $input/*.krn)
echo 
echo
echo "Submitting a \"Krona\" job..."
#sbatch $config $KRONA_RUN $input $krn &>/dev/null
sbatch -o ${debugDir}/3_kronaRunDebug.txt $KRONA_RUN $input $krn
echo
echo "After the job is done, you can find the .html file in $input"
echo
echo
echo "Thank you!"
