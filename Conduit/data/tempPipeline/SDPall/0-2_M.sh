#!/bin/bash


pipelinePath=/homes/mgheffel/SDPall
parentDir=/bulk/mgheffel/data/SDP
runSoftware=$pipelinePath/parallel/0-2_P.sh
readsInput=/bulk/mgheffel/data/SDP/raw_cleaned
chain=${pipelinePath}/parallel/chain.sh
krakenOutDir=/bulk/mgheffel/data/SDP/raw_kraken
database=/bulk/bioinfo/vdl/.dependencies/krakenDBs/DB21

results="$(echo $input | rev | cut -f 2- -d '_' | rev)_kraken"


#qsub $config -N "Kraken" -l mem=14G,h_rt=3:00:00 -e $log"Kraken.log" -o $log"Kraken.log" -pe single 12 $KRAKEN_RUN "$input $DB $results" &>/dev/null
JID=$(sbatch $runSoftware $readsInput $database $krakenOutDir)
JID=$(echo $JID | rev | cut -f 1 -d ' '|rev)
sbatch --dependency $JID $chain $parentDir/$1 &>/dev/null

