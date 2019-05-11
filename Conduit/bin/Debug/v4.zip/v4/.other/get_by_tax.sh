#!/bin/bash -l

#SBATCH --mem-per-cpu=2G   # Memory per core, use --mem= for memory per node
#SBATCH --time=1:00:00   # Use the form DD-HH:MM:SS
#SBATCH --partition=killable.q   # Job may run as killable on owned nodes
#SBATCH --nodes=1
#SBATCH --ntasks-per-node=2

set -x
dependencies=/bulk/bioinfo/vdl/.dependencies
translate=/homes/bioinfo/bioinfo_software/kraken-1.0/kraken-tax-translate
database_dir=$dependencies/krakenDBs

usage() { echo "Usage: sbatch $0 -k kraken_file.kraken -d corresponding_kraken_database -o output_file -t tax1[,tax2,...]" 1>&2; exit 1;}

while getopts ":k:d:t:o:" o; do
	case "${o}" in
		k)
			kraken_file=${OPTARG}
			if [ ! -e $kraken_file ]; then echo "File not found: $kraken_file";fi
			;;
		d)	
			database=${OPTARG}
			if [ ! -d $database_dir/$database ]; then echo "Database not found $database_dir/$database";fi
			;;
		t)
			taxonomies=${OPTARG}
			;;
		o)
			output=${OPTARG}
			;;
		*)
			usage
			;;
	esac
done
if [ -z $kraken_file ] || [ -z $database ] || [ -z $output ] || [ -z $taxonomies ]; then usage;fi

translate_output=$(dirname $output)/$(basename $kraken_file | rev | cut -f 2- -d '.' | rev).translate
$translate $kraken_file -db $database_dir/$database > $translate_output
taxonomy_set=(${taxonomies//,/ })
if [ -e $output ]; then rm $output; fi
touch $output.t
 
for t in "${taxonomy_set[@]}"; do
	grep -E ";$t(;|\$)" $translate_output  >> $output.t
done

sort $output.t | uniq -u | cut -f 1 > $output
rm $output.t $translate_output

