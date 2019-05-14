#!/bin/bash

#SBATCH --nodes=1
#SBATCH --time=24:00:00
#SBATCH --job-name=a5

pipelinePath=/homes/mgheffel/SDPall
parentDir=/bulk/mgheffel/data/SDP
runSoftware=$pipelinePath/parallel/0-4_P.sh
chain=${pipelinePath}/parallel/chain.sh
readsInputDir=/bulk/mgheffel/data/SDP/vraw_iralUK
contigsOutDir=/bulk/mgheffel/data/SDP/raw_denovo


ray=$pipelinePath/parallel/ray_run.sh

JOBS=""


mkdir $contigsOutDir

paste <(ls -1 $readsInput/*_R1*) <(ls -1 $readsInput/*_R2*) | while IFS="$(printf '\t')" read -r  p1 p2 ;
do
	log="$LOG/$(basename $p1)_$assembly_name.log"
	JID=$(sbatch $config $runSoftware $p1 $p2 $contigsOutDir/$(echo $(basename $p1) | sed 's/^\(.*\)_R1.*$/\1/')_$ray_contigs.fasta)
	JID=$(echo $JID | rev | cut -f 1 -d ' '|rev)
	JOBS="${JOBS}${JID},"
done

JOBS=$(echo $JOBS | sed 's/,\([^,]*\)$/ \1/')
sbatch --dependency $JOBS $chain $parentDir/$1 &>/dev/null

