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
			touch $dataParentDir/1a.done
			stepflag=true
			echo $(date +"%T")
		elif [ -e $dataParentDir/1a.done ] ; then
			stages[0]=${stages[0]}+1
			stepflag=false		fi
	elif (( ${stages[0]} == 1 )) ; then
		if ( ! $stepflag ) ; then
			touch $dataParentDir/1.done
			echo $(date +"%T")
			stepflaf=true
		elif [ -e 1.done ] ; then
			stepflag=false
			stages[0]=${stages[0]}+1
		fi
	fi
	elif (( ${stages[0]} == 2 )) ; then
		if ( ! $stepflag ) ; then
			touch $dataParentDir/2.done
			echo $(date +"%T")
			stepflaf=true
		elif [ -e 2.done ] ; then
			stepflag=false
			stages[0]=${stages[0]}+1
		fi
	fi
	elif (( ${stages[0]} == 3 )) ; then
		if ( ! $stepflag ) ; then
			touch $dataParentDir/3a.done
			echo $(date +"%T")
			stepflaf=true
		elif [ -e 3.done ] ; then
			stepflag=false
			stages[0]=${stages[0]}+1
		fi
	fi
done