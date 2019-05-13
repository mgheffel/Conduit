#!/bin/bash
#SBATCH --time=4:00:00

pipelinePath=jdhgfddj
parentDir=dsnnv
runSoftware=$pipelinePath/parallel/0-1_P.sh
chain=${pipelinePath}/parallel/chain.sh
referencesToMapToDir=/bulk/mgheffel/data/SDP/raw_denovo
pairedReadsToMapDir=/bulk/mgheffel/data/SDP/raw_cleaned
remappedOutDir=/bulk/mgheffel/data/SDP/raw_remap


mkdir $remappedOutDir
JOBS=""

rm -rf $remappedOutDir
mkdir $remappedOutDir
for contig in `ls $referencesToMapToDir/*-contigs.fasta`;
do
	sample=${contig#*trimmedContigs/}
	echo $sample
	sample=${sample%-contigs*}
	JID=$(sbatch $runSoftware $(realpath $contig) $pairedReadsToMapDir/*${sample}_R1*.fastq $pairedReadsToMapDir/*${sample}_R2*.fastq $remappedOutDir)
	JID=$(echo $JID | rev | cut -f 1 -d ' '|rev)
	JOBS="${JOBS}$JID,"
	
done

JOBS=$(echo $JOBS | sed 's/,\([^,]*\)$/ \1/')
sbatch --dependency $JOBS $chain $parentDir/$1 &>/dev/null
