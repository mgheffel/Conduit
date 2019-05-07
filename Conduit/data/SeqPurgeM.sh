declare adapters
declare filePaths
declare fileNames

HERE="/homes/mgheffel/SDP"
runSoftware=$HERE/.other/SeqPurgeP.sh
adapterFile=$HERE/dependencies/illumina_adapters.fa
readsInput=/bulk/mgheffel/data/SDP/raw
chain=${HERE}/.other/chain.sh

parentdir="$(dirname "$readsInput")"

# Create cleaned diretory
IFS='/' read -r -a readsInputSplit <<< "$readsInput"
readsInput_name=${readsInputSplit[${#readsInputSplit[@]}-1]}
OutFolder=$parentdir'/'$readsInput_name'_cleaned'
rm -rf $OutFolder
mkdir $OutFolder


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
done < "$adapterFile"

# Read in all fastq files and to file lists
i=0
for entry in "$readsInput"/*
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
  JID=$(sbatch $runSoftware $adapters $OutFolder ${fileNames[$i]} ${fileNames[$i+1]} ${filePaths[$i]} ${filePaths[$i+1]} $fileLen $parentdir)
  JID=$(echo $JID | rev | cut -f 1 -d ' '|rev)
  JOBS="${JOBS}$JID,"
done

JOBS=$(echo $JOBS | sed 's/,\([^,]*\)$/ \1/')
sbatch --dependency $JOBS $chain $parentdir/1_SeqPurge.done &>/dev/null
