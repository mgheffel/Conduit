#!/bin/bash

#SBATCH --mem=1G
#SBATCH --nodes=1
#SBATCH --ntasks-per-node=1
#SBATCH --time=3:00:00
#SBATCH --job-name=clnBlastM

usage() { echo "Usage: bash $0 -h data_directory(ex: /bulk/dmarth027/run_022118)" 1>&2; exit 1;}

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
if [ -z $HERE ]; then usage; fi

mkdir $debugDir
mkdir ${HERE}/raw_blastSequences

for filename in `ls $HERE/raw_blast/`; do
    sbatch -o "${debugDir}/7B_${filename%-contigs*}.out" /homes/bioinfo/vdl/v4/.other/run_cleanBlast.sh -h $HERE -i $filename
done

#waits on first blast cleaning script to start
flag=0
while [ $flag == 0 ]; do
        dirlist=$(ls $out_dir/ | grep -v ".+")
        for f in `ls $HERE/raw_blastSequences`;
        do
                endFour=${f:(-4)}
                if [ $endFour == "flag" ]; then
                        flag=1
                fi
                #echo "f $f"
        done
        sleep 30
done


#waits on all blast cleaning scripts to end
flag=0
while [ $flag == 0 ]; do
	flag=1
        dirlist=$(ls $out_dir/ | grep -v ".+")
        for f in `ls $HERE/raw_blastSequences`;
        do
                endFour=${f:(-4)}
                if [ $endFour == "flag" ]; then
                        flag=0
                fi
        done
        sleep 60
done

touch ${HERE}/7B_cleanBlast.done
