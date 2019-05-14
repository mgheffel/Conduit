#!/bin/bash

#SBATCH --nodes=1
#SBATCH --time,:00:00=10
#SBATCH --mem,G=4

#SBATCH --job-name=getTaxUK

pipelinePath=/homes/mgheffel/SDPall
parentDir=/bulk/mgheffel/data/SDP
chain=${pipelinePath}/parallel/chain.sh
database=/bulk/bioinfo/vdl/.dependencies/krakenDBs/DB21
cleanedReadsDir=/bulk/mgheffel/data/SDP/raw_cleaned
krakenDir=/bulk/mgheffel/data/SDP/raw_kraken
taxonomyReadsDir=/bulk/mgheffel/data/SDP/vraw_iralUK

select=/homes/bioinfo/vdl/v4/.other/select_sequences.pl
get_by_tax=/homes/bioinfo/vdl/v4/.other/get_by_tax.sh
clean=homes/bioinfo/vdl/v4/.other/guv_clean.sh

JOBS=""


rm -rf $taxonomyReadsDir
mkdir $taxonomyReadsDir

paste <(ls -1 $cleanedReadsDir/*_R1*) <(ls -1 $cleanedReadsDir/*_R2*) | while IFS="$(printf '\t')" read -r  p1 p2 ;
do
		
	echo "Checking: "$(basename $p1) " and " $(basename $p2)  
	X=$(echo $(basename $p1 .fastq) | awk -F'_R1' '{print $1}' | cut -f 2- -d "_")

	awk -F"\t" '{if ($1 == "U") print $2}' $krakenDir/$X.kraken > $taxonomyReadsDir/$X.selectu
	JID=$(sbatch $config -J tax_$X.kraken $get_by_tax -k $kinput/$X.kraken -d $database -o $taxonomyReadsDir/$X.selectt -t 10239)
	JID=$(echo $JID | rev | cut -f 1 -d ' '|rev)
	sleep 5	
	JID=$(sbatch $config --dependency $JID -J clean_$X.kraken $clean $X $p1 $p2 $taxonomyReadsDir)
	JID=$(echo $JID | rev | cut -f 1 -d ' '|rev)
	
	
	JOBS="${JOBS}$JID,"
#
done 
sleep 10 
JOBS=$(echo $JOBS | sed 's/,\([^,]*\)$/ \1/')

sbatch --dependency $JOBS $chain $parentDir/$1 &>/dev/null

