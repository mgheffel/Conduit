#!/bin/bash
#SBATCH --mem-per-cpu=32G

pipelinePath=/homes/mgheffel/SDPall
parentDir=/bulk/mgheffel/data/SDPall
runSoftware=$pipelinePath/parallel/1-1_P.sh
chain=${pipelinePath}/parallel/chain.sh
krakenInput=/bulk/mgheffel/data/SDP/raw_kraken
kronaOutDir=*&%@kronaOurDirTag

for f in $krakenInput/*.kraken
do
  echo $(basename $f)
  cut -f 3 $f | sort | uniq -c | sort -nr | awk ' { reads = $1; $1 = $2; $3 = reads; print; }' OFS=$'\t' > $krakenInput"/"$(basename $f .kraken)".krn"
done

krn=$(ls $input/*.krn)

#sbatch $config $KRONA_RUN $input $krn &>/dev/null
JID=$(sbatch $runSoftware $kronaOurDirTag $krakenInput $krn)
JID=$(echo $JID | rev | cut -f 1 -d ' '|rev)

sbatch --dependency $JID $chain $parentDir/$1 &>/dev/null