#!/bin/bash

#SBATCH --mem=1G
#SBATCH --nodes=1
#SBATCH --ntasks-per-node=1
#SBATCH --time=3:00:00
#SBATCH --job-name=9_creatGenomes

usage() { echo "Usage: bash $0 -h data_directory(ex: /bulk/dmarth027/run_022118) -g refGenomesDirectory -r readsToRefSamsDirectory -c contigsToRefSamsDirectory -b debugDirectory" 1>&2; exit 1;}


while getopts ":h:g:r:c:b:" o; do
        case "${o}" in
                        h)
                                HERE=${OPTARG}
                                if [ ! -d $HERE ]; then echo "Directory not found $HERE"; exit 1;fi
                                HERE=$(realpath -s $HERE)
                                ;;
			g)
				refDir=${OPTARG}
				;;
			r)
				readSamDir=${OPTARG}
				;;
			c)
				contigSamDir=${OPTARG}
				;;
			b)
				debugDir=${OPTARG}
				;;
                        *)
                                usage
                                ;;
        esac
done
if [ -z $HERE ] || [ -z $refDir ] || [ -z $readSamDir ] || [ -z $contigSamDir ] || [ -z $debugDir ]; then usage; fi

#echo $HERE
#echo $refDir
#echo $readSamDir
#echo $contigSamDir
#echo $debugDir

mkdir $debugDir
rm -rf ${HERE}/raw_assembledGenomes
mkdir ${HERE}/raw_assembledGenomes

for filename in `ls $refDir`; do
	#echo $filename
	base=${filename%.fasta*}
	#echo $base
	refFile=${refDir}${filename}
	readSamFile=${readSamDir}trimmed_${base}_R1_001.sam
	contigSamFile=${contigSamDir}${base}.sam
	#echo $refFile
	#echo $readSamFile
	#echo $contigSamFile
	echo "sbatch -o "${debugDir}/9_${filename%.fasta*}.out" /homes/bioinfo/vdl/v4/.other/run_samToGenome.sh -h $HERE -g $refFile -r $readSamFile -c $contigSamFile"
	sbatch -o "${debugDir}/9_${filename%.fasta*}.out" /homes/bioinfo/vdl/v4/.other/run_samToGenome.sh -h $HERE -g $refFile -r $readSamFile -c $contigSamFile
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

touch ${HERE}/9_createGenomes.done
