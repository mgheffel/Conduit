#!/bin/bash
#
#
#       Originally written by Rylan Kasitz rylankasitz@ksu.edu
#       5/25/2018
#
#

# Global Variablsbes
declare adapters
declare filePaths
declare fileNames

# Get parent directory and script directory
HERE="/homes/bioinfo/vdl/v4"

# Set file locations
AdapterFile=$HERE/.other/illumina_adapters.fa
Trim_Run=$HERE/.other/trim_file.sh

usage() { echo "-i <Raw file directory> [-a <Adapter file>]" 1&>2; exit 1; }

# Comandline Arguments
while getopts ":i:a:c:b:" o; do
	case "${o}" in
		#get raw reads directory (required)
		i) 
			InFolder=${OPTARG}
			;;
		#get custom adapters see .other/illumina_adapters.fa for examples (optional) default=illumina_adapters.fa
		a) 
			AdapterFile=${OPTARG}
			;;
		b)
			debugDir=${OPTARG}
			;;
    # Temperary (don't know why its needed)
    c) 
      Config=${OPTARG}
      ;;
		*)
			usage
			;;
	esac
done

mkdir $debugDir

parentdir="$(dirname "$InFolder")"

# Create cleaned diretory
IFS='/' read -r -a InFolderSplit <<< "$InFolder"
inFolder_name=${InFolderSplit[${#InFolderSplit[@]}-1]}
OutFolder=$parentdir'/'$inFolder_name'_cleaned'
mkdir -p -- $OutFolder


# Read in all Adapters and add to list
i=0
while IFS='' read -r line || [[ -n "$line" ]]; do
  if [ ${line:0:1} != '>' ]
  then
    IFS='/' read -r -a splitLine <<< "$line"
    adapters[$i]=${splitLine[0]}
    adapters[$i+1]=${splitLine[1]}
    i=$i+2
  fi
done < "$AdapterFile"

# Read in all fastq files and to file lists
i=0
for entry in "$InFolder"/*
do
  if [[ $entry =~ \.fastq$ ]]
  then
    filePaths[$i]="$entry"
    IFS='/' read -r -a entrySplit <<< "$entry"
    entryLen=${#entrySplit[@]}
    fileNames[$i]=${entrySplit[$entrylen-1]}
    i=$i+1
  fi
done

# Get lengths for loop
fileLen=${#filePaths[@]}

# Loop through all fastq files
for (( i=0; i<$fileLen; i+=2 ))
do
  # Adapter List - Output Folder - file names forward/reverse - file paths foward/reverse - amount of files in directory - Parent directory
  sbatch -o ${debugDir}/1_trim_${fileNames[$i]}.out $Trim_Run $adapters $OutFolder ${fileNames[$i]} ${fileNames[$i+1]} ${filePaths[$i]} ${filePaths[$i+1]} $fileLen $parentdir
done
