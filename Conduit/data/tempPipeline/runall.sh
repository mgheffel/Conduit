#!/bin/bash
#SBATCH --nodes=1
#SBATCH --time=48:00:00
pipePath=/homes/mgheffel/SDP
dataParentDir=/bulk/mgheffel/data/SDP
alldoneflag=false
doneflags=(false)
stepflags=(false)
stages=(1)
while ( ! $alldoneflag )
	do
	if [ ${stages[0]} == 1 ] ; then
		if [ ${stepflags[0]} == false ] ; then
			sbatch /homes/mgheffel/SDP/0-1_M.sh 0-1
			stepflags[0]=true
		elif [ -e $dataParentDir/0-1.done ] ; then
			stages[0]=$((${stages[0]}+1))
			stepflags[0]=false
			echo "0-1 done"
		fi
	else
		doneflags[0]=true
	fi
	alldoneflag=true
	for i in ${doneflags[@]}
		do
		if [ "$i" == false ] ; then
			alldoneflag=false
		fi
	done
	sleep 60
done
touch $dataParentDir/all.done