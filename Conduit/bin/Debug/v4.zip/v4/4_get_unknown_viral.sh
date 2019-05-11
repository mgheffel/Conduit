#!/bin/bash
#$ -cwd'
#
#       Originally written by Reza Mazloom rmazloom@ksu.edu
#       10/23/2017
#
#SBATCH --nodes=1
#SBATCH --time=10:00:00
#SBATCH --job-name=viralUK
#SBATCH --mem=4G
#
 

HERE="/homes/bioinfo/vdl/v4"
select=$HERE/.other/select_sequences.pl
get_by_tax=$HERE/.other/get_by_tax.sh
krakendb_dir=/bulk/bioinfo/vdl/.dependencies/krakenDBs
chain=$HERE/.other/chain.sh
clean=$HERE/.other/guv_clean.sh
LOG="$HERE/logs/"
JOBS=""

usage() { echo "Usage: bash $0 -i trimmed_reads_directory -k kraken_result_directory -d kraken_database_name [-c submit_parameters]" 1>&2; exit 1;}

while getopts ":i:k:c:d:b:" o; do
        case "${o}" in
			k)
				kinput=${OPTARG}
				if [ ! -d $kinput ]; then echo "Directory not found $kinput"; exit 1;fi
				kinput=$(realpath -s $kinput)
				;;
	                i)
	                        input=${OPTARG}
				if [ ! -d $input ]; then echo "Directory not found $input"; exit 1;fi
				input=$(realpath -s $input)
	                        ;;
			d)
				database=${OPTARG}
				if [ ! -d $krakendb_dir/$database ]; then echo "Database not found: $krakendb_dir/$database"; exit 1;fi
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
if [ -z $kinput ] || [ -z $database ] || [ -z $input ]; then usage; fi

mkdir $debugDir

output=$(echo $kinput | rev | cut -f 2- -d '_' | rev )"_unknown-viral"

rm -rf $output
mkdir $output

echo
echo
paste <(ls -1 $input/*_R1*) <(ls -1 $input/*_R2*) | while IFS="$(printf '\t')" read -r  p1 p2 ;
do
		
	echo "Checking: "$(basename $p1) " and " $(basename $p2)  
	X=$(echo $(basename $p1 .fastq) | awk -F'_R1' '{print $1}' | cut -f 2- -d "_")

	awk -F"\t" '{if ($1 == "U") print $2}' $kinput/$X.kraken > $output/$X.selectu
	JID=$(sbatch -o ${debugDir}/4_uv_g$X.kraken  $config -J tax_$X.kraken $get_by_tax -k $kinput/$X.kraken -d $database -o $output/$X.selectt -t 10239)
	JID=$(echo $JID | rev | cut -f 1 -d ' '|rev)
	sleep 5	
	JID=$(sbatch -o ${debugDir}/4_uv_c$X.kraken $config --dependency $JID -J clean_$X.kraken $clean $X $p1 $p2 $output)
	JID=$(echo $JID | rev | cut -f 1 -d ' '|rev)
	
	#echo -e "#!/bin/bash\n -l\
	#SBATCH --time=1:00:00\n \
	#SBATCH --mem=2G\n \
	#SBATCH -J select\n \
	#set -x\n \
	#cat $output/$X.selectu <(cut -f 1 $output/$X.selectt)  > $output/$X.select
	#perl $select $output/$X.select $p1 $output/$(basename $p1 .fastq).fastq\n \
	#perl $select $output/$X.select $p2 $output/$(basename $p2 .fastq).fastq\n \
	#rm $output/$X.select*
	#rm $output/$X.sh " > $output/$X.sh
	#
	#JID=$(sbatch $config --dependency $JID -J guv.kraken $output/$X.sh)
	
	JOBS="${JOBS}$JID,"
#
done 
sleep 10 
JOBS=$(echo $JOBS | sed 's/,\([^,]*\)$/ \1/')
echo "sbatch sbatch $config --dependency $JOBS $chain $(dirname $output)/4_get_unknown_viral.done  > /dev/null"
sbatch -o ${debugDir}/4_uv_$output.txt $config --dependency $JOBS $chain $(dirname $output)
curdir=${kinput::-10}
sleep 900
echo "curdir $curdir"
touch ${curdir}4_unknown_viral.done
echo 
echo
echo "Done submitting jobs! once the jobs finish running (5 to 10 min)"
echo "You should be able to find the results in $output"
echo
echo "After that, you can run: bash 5_denovo.sh -i $output -a iva,... "
echo
echo
echo "Thank you!"
