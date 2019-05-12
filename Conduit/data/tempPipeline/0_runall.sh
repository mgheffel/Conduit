#!/bin/bash
#SBATCH --nodes=1
#SBATCH --time=48:00:00
pipePath=/homes/mgheffel/SDP
dataParentDir=/bulk/mgheffel/data/SDP
doneflag=false
stepflag=false
stages=(0)
while ( ! $doneflag )
	do
	if (( ${stages[0]} == 0 )) ; then
		if ( ! $stepflag ) ; then
			sbatch /homes/mgheffel/SDP/1a_SeqPurgeM.sh
			stepflag=true
		elif [ -e $dataParentDir/1a.done ] ; then
			stages[0]=${stages[0]}+1
		fi
	fi
done