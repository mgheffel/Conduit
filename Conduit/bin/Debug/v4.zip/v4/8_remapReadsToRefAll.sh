#!/bin/bash
#$ -cwd'
#
# 	Originally Created by Matthew G. Heffel, mgheffel@ksu.edu
#	1/2/2019
#
#SBATCH --time=4:00:00

CWD="/homes/bioinfo/vdl/v4"
remap=$CWD/.other/remap_bowtieAll.sh
LOG="$CWD/logs"
chain=$CWD/.other/chain.sh
JOBS=""


#print help
usage() { echo "Usage: bash $0 -r reference_directory -m mapping_reads_directory [-c job_configuration]" 1>&2; exit 1;}

while getopts ":r:m:c:b:" o; do
	case "${o}" in
		r)
			ref=${OPTARG}
			if [ ! -d "$ref" ]; then echo "Directory does not exist $ref";usage; fi
			ref=$(realpath $ref)
			;;
		m)
			map=${OPTARG}
			if [ ! -d "$map" ]; then "Directory does not exist $ref";usage; fi
			map=$(realpath $map)
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

if [ -z "$ref" ] || [ -z "$map" ]; then	echo "One or more mandatory parameters were not satisfied";usage; fi

mkdir $debugDir
echo "debugDir: $debugDir"

echo 'Submitting "Remapping" jobs...'
echo
out_dir="$(echo $ref | rev | cut -f 2- -d '_' | rev)_remapReadsToRef"
rm -rf $out_dir
mkdir $out_dir
for reference in `ls $ref/*.fasta`;
do
	echo $reference
	sample=${reference#*blastSequences/}
	echo $sample
	sample=${sample%.fasta*}
	echo $sample
	sbatch -o ${debugDir}/8_remapReads-${sample}.out -J ${sample}-remap $remap -d $(realpath $reference) -f $map/*${sample}_R1*.fastq -r $map/*${sample}_R2*.fastq -o $out_dir
	#sbatch -o ${debugDir}/6_remapStep-${contig#*trimmedContigs/}.out $config -J ${sample}-remap $remap -d $(realpath $contig) -f $map/*${sample}_R1*.fastq -r $map/*${sample}_R2*.fastq -o $out_dir
	echo "sbatch $config -J ${sample}-remap $remap -d $(realpath $reference) -f $map/*${sample}_R1*.fastq -r $map/*${sample}_R2*.fastq -o $out_dir"
	echo
	#JID=$(echo $JID | rev | cut -f 1 -d ' '|rev)
	##-o $log -e $log 
done
#JOBS="${JOBS},${JID}"
#JOBS=$(echo $JOBS | sed 's/,\([^,]*\)$/ \1/')
#sbatch $config --dependency $JOBS "$chain $ref/../7_remap.done" & >/dev/null
echo
echo "After the job is done, you can find the the mappings in $(realpath $ref | rev | cut -f 2- -d "_"| rev)""_remap"
echo 
echo "Thank you!" 



#waits on first remapping script to start
flag=0
while [ $flag == 0 ]; do
        dirlist=$(ls $out_dir/ | grep -v ".+")
        for f in $dirlist
        do
                #echo "f:-6 ${f:(-6)}"
                endFour=${f:(-4)}
                #echo "endSix $endSix"
                if [ $endFour == ".bam" ]; then
                        #echo "in dolsf $f"
                        flag=1
                fi
                #echo "f $f"
        done
        sleep 30
done


#waits on all remapping scripts to end
flag=0
while [ $flag == 0 ]; do
        flag=1
        dirlist=$(ls $out_dir/ | grep -v ".+")
        for f in $dirlist
        do
                #echo "f:-6 ${f:(-6)}"
                endFive=${f:(-5)}
                #echo "endSix $endSix"
                if [ $endFive == ".hold" ]; then
                        #echo "in dolsf $f"
                        flag=0
                fi
                #echo "f $f"
        done
        sleep 60
done

touch ${outdir%/raw_remap*}8_remapReads.done
