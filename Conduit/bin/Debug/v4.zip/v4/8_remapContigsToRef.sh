#!/bin/bash
#
#
#       Originally written by Matthew G Heffel mgheffel@ksu.edu
#       11/02/2018
#
#
# Get parent directory and script directory

#SBATCH --mem=8G
#SBATCH --nodes=1
#SBATCH --ntasks-per-node=1
#SBATCH --time=1:00:00
#SBATCH --job-name=CtigToRef

HERE="/homes/bioinfo/vdl/v4"

usage() { echo "-h <dataset file directory> (example: /bulk/dmarth027/data/run_021218" 1>&2; exit 1;}


# Comandline Arguments
while getopts ":h:b:" o; do
        case "${o}" in
                #get raw reads directory (required)
                h)
                        InFolder=${OPTARG}
                        ;;
		b)
			debugDir=${OPTARG}
			;;
                *)
                        usage
                        ;;
        esac
done


mkdir $debugDir
echo $InFolder
sortedDir=${InFolder}/raw_blastSequences
echo $sortedDir
#make output directory for aligned files
outdir=${InFolder}/raw_remapCtigsToRef
echo $outdir
mkdir $outdir

for refFile in "$sortedDir"/*
do
	mapFile=${refFile#*blastSequences/}
	mapFile=${InFolder}/raw_fastqContigs/${mapFile}-contigs.fastq
	outFile=${refFile#*blastSequences/}
	debugFile=${outFile::-5}.out
	outFile=${outdir}/${outFile::-5}-contigs.fasta
	echo "refFile"
	echo "$refFile"
	sbatch -o ${debugDir}/8_remapCtigs-${debugFile} ${HERE}/.other/unpairedRemap.sh -r $refFile -m $mapFile -o $outdir
	echo "mapFile"
	echo "$mapFile"
done

