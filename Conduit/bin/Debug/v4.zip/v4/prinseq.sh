#!/bin/bash
#
#
#       Originally written by Rylan Kasitz rylankasitz@ksu.edu
#       5/25/2018
#

# Get parent directory and script directory
HERE="/homes/bioinfo/vdl/v4/"
Prinseq_Run=${HERE}.other/prinseq_run.sh

# Comandline Arguments
while getopts ":i:s" o; do
	case "${o}" in
		#get raw reads directory (required)
		i) 
			InFolder=${OPTARG}
			;;
		*)
			usage
			;;
	esac
done

# Create prinseq directory
echo $InFolder
parentdir="$(dirname "$InFolder")"
IFS='/' read -r -a InFolderSplit <<< "$InFolder"
inFolder_name=${InFolderSplit[${#InFolderSplit[@]}-1]}
prinseqFolder=$parentdir'/'$inFolder_name'_prinseq'
mkdir -p -- $prinseqFolder

# Read in all fastq files and to file lists
i=0
for entry in "$InFolder"/*
do
	filePaths[$i]="$entry"
	IFS='/' read -r -a entrySplit <<< "$entry"
	entryLen=${#entrySplit[@]}
	fileNames[$i]=${entrySplit[$entrylen-1]}
	i=$i+1
done

# Get lengths for loop
fileLen=${#filePaths[@]}

# Run prinseq for orginal files
for (( i=0; i<$fileLen; i+=1 ))
do
	filename=${fileNames[i]}
  	sbatch $Prinseq_Run ${filePaths[i]} $prinseqFolder/'prinseq_'${filename::-5} $fileLen $parentdir
done
