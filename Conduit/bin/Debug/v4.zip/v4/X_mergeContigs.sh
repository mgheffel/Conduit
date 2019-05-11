#!/bin/bash

#SBATCH --mem=1G
#SBATCH --nodes=1
#SBATCH --ntasks-per-node=1
#SBATCH --time=3:00:00
#SBATCH --job-name=mergeCtigs

usage() { echo "Usage: bash $0 -h data_directory(ex: /bulk/dmarth027/run_022118) -b debugDirectory" 1>&2; exit 1;}


while getopts ":h:b:" o; do
        case "${o}" in
                        h)
                                HERE=${OPTARG}
                                if [ ! -d $HERE ]; then echo "Directory not found $HERE"; exit 1;fi
                                HERE=$(realpath -s $HERE)
                                ;;
			b)
				debugDir=${OPTARG}
				;;
                        *)
                                usage
                                ;;
        esac
done
if [ -z $HERE ] || [ -z $debugDir ]; then usage; fi

#echo $HERE
#echo $debugDir

contigDir=${HERE}/raw_trimmedContigs

mkdir $debugDir
rm -rf ${HERE}/raw_mergedContigs
mkdir ${HERE}/raw_mergedContigs

for filename in `ls $contigDir`; do
	echo $HERE
	base=${filename%.fasta*}
	echo $filename
	#echo "sbatch -o "${debugDir}/9_${filename%.fasta*}.out" /homes/bioinfo/vdl/v4/.other/run_samToGenome.sh -h $HERE -g $refFile -r $readSamFile -c $contigSamFile"
	sbatch -o "${debugDir}/X_${filename%.fasta*}.out" /homes/bioinfo/vdl/v4/.other/run_mergeContigs.sh -h $HERE -c $filename
	echo ""
done

#waits on first blast cleaning script to start
#flag=0
#while [ $flag == 0 ]; do
#        dirlist=$(ls $out_dir/ | grep -v ".+")
#        for f in `ls $HERE/raw_blastSequences`;
#        do
#                endFour=${f:(-4)}
#                if [ $endFour == "flag" ]; then
#                        flag=1
#                fi
#                #echo "f $f"
#        done
#        sleep 30
#done


#waits on all blast cleaning scripts to end
#flag=0
#while [ $flag == 0 ]; do
#	flag=1
#        dirlist=$(ls $out_dir/ | grep -v ".+")
#        for f in `ls $HERE/raw_blastSequences`;
#        do
#                endFour=${f:(-4)}
#                if [ $endFour == "flag" ]; then
#                        flag=0
#                fi
#        done
#        sleep 60
#done

#touch ${HERE}/X_mergeContigs.done
