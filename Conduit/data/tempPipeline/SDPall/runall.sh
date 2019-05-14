#!/bin/bash
#SBATCH --nodes=1
#SBATCH --time=48:00:00
pipePath=/homes/mgheffel/SDPall
dataParentDir=/bulk/mgheffel/data/SDP
alldoneflag=false
doneflags=(false false)
stepflags=(false false)
stages=(1 0)
while ( ! $alldoneflag )
	do
	if [ ${stages[0]} == 1 ] ; then
		if [ ${stepflags[0]} == false ] ; then
			sbatch /homes/mgheffel/SDPall/0-1_M.sh 0-1.done
			stepflags[0]=true
		elif [ -e $dataParentDir/0-1.done ] ; then
			stages[0]=$((${stages[0]}+1))
			stepflags[0]=false
			echo "0-1 done"
		fi
	elif [ ${stages[0]} == 2 ] ; then
		if [ ${stepflags[0]} == false ] ; then
			sbatch /homes/mgheffel/SDPall/0-2_M.sh 0-2.done
			stepflags[0]=true
		elif [ -e $dataParentDir/0-2.done ] ; then
			stages[0]=$((${stages[0]}+1))
			stepflags[0]=false
			echo "0-2 done"
		fi
	elif [ ${stages[0]} == 3 ] ; then
		if [ ${stepflags[0]} == false ] ; then
			sbatch /homes/mgheffel/SDPall/0-3_M.sh 0-3.done
			stepflags[0]=true
		elif [ -e $dataParentDir/0-3.done ] ; then
			stages[0]=$((${stages[0]}+1))
			stages[1]=1
			stepflags[0]=false
			echo "0-3 done"
		fi
	elif [ ${stages[0]} == 4 ] ; then
		if [ ${stepflags[0]} == false ] ; then
			sbatch /homes/mgheffel/SDPall/0-4_M.sh 0-4.done
			stepflags[0]=true
		elif [ -e $dataParentDir/0-4.done ] ; then
			stages[0]=$((${stages[0]}+1))
			stepflags[0]=false
			echo "0-4 done"
		fi
	elif [ ${stages[0]} == 5 ] ; then
		if [ ${stepflags[0]} == false ] ; then
			sbatch /homes/mgheffel/SDPall/0-5_M.sh 0-5.done
			stepflags[0]=true
		elif [ -e $dataParentDir/0-5.done ] ; then
			stages[0]=$((${stages[0]}+1))
			stepflags[0]=false
			echo "0-5 done"
		fi
	else
		doneflags[0]=true
	fi
	if [ ${stages[1]} == 1 ] ; then
		if [ ${stepflags[1]} == false ] ; then
			sbatch /homes/mgheffel/SDPall/1-1_M.sh 1-1.done
			stepflags[1]=true
		elif [ -e $dataParentDir/1-1.done ] ; then
			stepflags[1]=false
			stages[1]=$((${stages[1]}+1))
			echo "1-1 done"
		fi
	else
		doneflags[1]=true
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