#!/bin/bash
#
#
#       Originally written by Rylan Kasitz rylankasitz@ksu.edu
#       5/25/2018
#
#
#SBATCH --time=4
#SBATCH --mem-per-cpu=8


#SBATCH --job-name=SeqPurge
export PATH="/homes/mgheffel/miniconda3/bin:$PATH"

# Commandline Args
adapters=$1
OutFolder=$2
fileNames_f=$3
fileNames_r=$4
filePath_f=$5
filePath_r=$6
fileCount_o=$7
parentdir=$8

# Create the output files (reverse/foward)
outFile_F=$OutFolder/'trimmed_'$fileNames_f'.gz'
outFile_R=$OutFolder/'trimmed_'$fileNames_r'.gz'
touch $outFile_F
touch $outFile_R

# Get adapter length
adapterLen=${#adapters[@]}
  
# Loop through the adapter list
for (( j=0; j<$adapterLen; j+=2 ))
do
    # Check if there are foward and reverse adapters
    if [ "${adapters[$j]}" == "" ]
    then
        SeqPurge -a2 ${adapters[$j+1]} -in1 $filePath_f -in2 $filePath_r -out1 $outFile_F -out2 $outFile_R
    elif [ "${adapters[$j+1]}" == "" ]
    then
        SeqPurge -a1 ${adapters[$j]} -in1 $filePath_f -in2 $filePath_r -out1 $outFile_F -out2 $outFile_R
    else
        SeqPurge -a1 ${adapters[$j]} -a2 ${adapters[$j+1]} -in1 $filePath_f -in2 $filePath_r -out1 $outFile_F -out2 $outFile_R
    fi

# Reset the input files to newly trimmed files
filePath_f=$outFile_F
filePath_r=$outFile_R
done

# Unzip files (-d) and force file override (-f)
gzip -f -d $outFile_F
gzip -f -d $outFile_R
