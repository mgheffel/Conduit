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
#SBATCH --job-name=CtigSubs

HERE="/homes/bioinfo/vdl/v4"

usage() { echo "-h <dataset file directory> (example: /bulk/dmarth027/data/run_021218" 1>&2; exit 1;}

# Comandline Arguments
while getopts ":h:" o; do
        case "${o}" in
                #get raw reads directory (required)
                h)
                        InFolder=${OPTARG}
                        ;;
                *)
                        usage
                        ;;
        esac
done

echo $InFolder
contigDir=${InFolder}/raw_trimmedContigs
echo $contigDir

mkdir raw_contigGenDiv

datasetArr=()
a5Flag="false"
ivaFlag="false"
rayFlag="false"
for file in "$contigDir"/*
do
	#check for assebler type in file name
	if [[ "$file" =~ "_a5_" ]]; then
		temp=${file#*trimmed_}
		if [ ivaFlag=="false" ] && [ rayFlag=="false" ]; then
			temp=${temp%_a5_*}
			datasetArr+=($temp)
			a5Flag="true"
		fi
	fi
	if [[ "$file" =~ "_iva__" ]]; then
                temp=${file#*trimmed_}
		if [ a5Flag=="false" ] && [ rayFlag=="false" ]; then
	                temp=${temp%_iva_*}
			dataset+=($temp)
			ivaFlag="true"
		fi
        fi
	if [[ "$file" =~ "_ray_" ]]; then
                temp=${file#*trimmed_}
		if [ a5Flag=="false" ] && [ ivaFlag=="false" ]; then
	                temp=${temp%_ray_*}
			dataset+=($temp)
			rayflag="true"
		fi
        fi
done

echo ${datasetArr[@]}

for dataset in ${datasetArr[@]}
do
	echo $dataset
	filesString=""
	for file in "$contigDir"/*
	do
		if [[ "$file" =~ "$dataset" ]]; then
			filesString+=${file},
		fi
	done
	sbatch ${HERE}/.other/X2.sh -h $InFolder -d $dataset -f $filesString
	echo ${filesString::-1}
done
