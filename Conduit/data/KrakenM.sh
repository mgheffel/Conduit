#!/bin/bash
#
#       Updated v1.1 by Reza Mazloom rmazloom@ksu.edu
#       10/19/2017
#
#       originally written by Majed Alsadhan
#
#


#input=/.../data/raw_cleaned
#database=/.../krakenDBs/BD21
pipePath=/homes/mgheffel/SDP
input=/bulk/mgheffel/data/SDP/raw_cleaned
database=/bulk/bioinfo/vdl/.dependencies/krakenDBs/DB21
runSoftware=$pipePath/.other/KrakenP.sh
chain=${HERE}/.other/chain.sh

results="$(echo $input | rev | cut -f 2- -d '_' | rev)_kraken"

echo
echo
echo "subbmitting a job with database: $database  and input directory: $input"
#qsub $config -N "Kraken" -l mem=14G,h_rt=3:00:00 -e $log"Kraken.log" -o $log"Kraken.log" -pe single 12 $KRAKEN_RUN "$input $DB $results" &>/dev/null
JID=$(sbatch $runSoftware $input $database $results $kraken_params)
JID=$(echo $JID | rev | cut -f 1 -d ' '|rev)
sbatch --dependency $JID $chain $input/../2_kraken.done &>/dev/null
echo
echo "The Job named \"Kraken\" was submitted. It would probably take from 10 to 50 minutes to finish..."
echo "You can find the results in the directory $results"
echo
echo "After that, you can run: bash 3_krona.sh -K $results"
echo
echo
echo "Thank you!"
