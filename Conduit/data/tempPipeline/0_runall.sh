#!/bin/bash
#SBATCH --nodes=1
#SBATCH --time=48:00:00
pipePath=/homes/mgheffel/SDP
dataParentDir=/bulk/mgheffel/data/SDP
alldoneflag=false
doneflags=(false false false false)
stepflags=(false false false false)
stages=(1 1 1 1)
while ( ! $alldoneflag )
	do
	if [ ${stages[0]} == 1 ] ; then
		if [ ${stepflags[0]} == false ] ; then
			touch /bulk/mgheffel/data/SDP/0-1.done
			stepflags[0]=true
		elif [ -e $dataParentDir/0-1.done ] ; then
			stages[0]=$((${stages[0]}+1))
			stepflags[0]=false
			echo "0-1 done"
		fi
	elif [ ${stages[0]} == 2 ] ; then
		if [ ${stepflags[0]} == false ] ; then
			touch /bulk/mgheffel/data/SDP/0-2.done
			stepflags[0]=true
		elif [ -e $dataParentDir/0-2.done ] ; then
			stages[0]=$((${stages[0]}+1))
			stepflags[0]=false
			echo "0-2 done"
		fi
	elif [ ${stages[0]} == 3 ] ; then
		if [ ${stepflags[0]} == false ] ; then
			touch /bulk/mgheffel/data/SDP/0-3.done
			stepflags[0]=true
		elif [ -e $dataParentDir/0-3.done ] ; then
			stages[0]=$((${stages[0]}+1))
			stepflags[0]=false
			echo "0-3 done"
		fi
	elif [ ${stages[0]} == 4 ] ; then
		if [ ${stepflags[0]} == false ] ; then
			touch /bulk/mgheffel/data/SDP/0-4.done
			stepflags[0]=true
		elif [ -e $dataParentDir/0-4.done ] ; then
			stages[0]=$((${stages[0]}+1))
			stepflags[0]=false
			echo "0-4 done"
		fi
	elif [ ${stages[0]} == 5 ] ; then
		if [ ${stepflags[0]} == false ] ; then
			touch /bulk/mgheffel/data/SDP/0-5.done
			stepflags[0]=true
		elif [ -e $dataParentDir/0-5.done ] ; then
			stages[0]=$((${stages[0]}+1))
			stepflags[0]=false
			echo "0-5 done"
		fi
	elif [ ${stages[0]} == 6 ] ; then
		if [ ${stepflags[0]} == false ] ; then
			touch /bulk/mgheffel/data/SDP/0-6.done
			stepflags[0]=true
		elif [ -e $dataParentDir/0-6.done ] ; then
			stages[0]=$((${stages[0]}+1))
			stepflags[0]=false
			echo "0-6 done"
		fi
	else
		doneflags[0]=true
	fi
	if [ ${stages[1]} == 1 ] ; then
		if [ ${stepflags[1]} == false ] ; then
			touch /bulk/mgheffel/data/SDP/1-1.done
			stepflags[1]=true
		elif [ -e $dataParentDir/1-1.done ] ; then
			stepflags[1]=false
			stages[1]=$((${stages[1]}+1))
			echo "1-1 done"
		fi
	else
		doneflags[1]=true
	fi
	if [ ${stages[2]} == 1 ] ; then
		if [ ${stepflags[2]} == false ] ; then
			touch /bulk/mgheffel/data/SDP/2-1.done
			stepflags[2]=true
		elif [ -e $dataParentDir/2-1.done ] ; then
			stepflags[2]=false
			stages[2]=$((${stages[2]}+1))
			echo "2-1 done"
		fi
	elif [ ${stages[2]} == 2 ] ; then
		if [ ${stepflags[2]} == false ] ; then
			touch /bulk/mgheffel/data/SDP/2-2.done
			stepflags[2]=true
		elif [ -e $dataParentDir/2-1.done ] ; then
			stepflags[2]=false
			stages[2]=$((${stages[2]}+1))
			echo "2-2 done"
		fi
	else
		doneflags[2]=true
	fi
	if [ ${stages[3]} == 1 ] ; then
		if [ ${stepflags[3]} == false ] ; then
			touch /bulk/mgheffel/data/SDP/3-1.done
			stepflags[3]=true
		elif [ -e $dataParentDir/3-1.done ] ; then
			stepflags[3]=false
			stages[3]=$((${stages[3]}+1))
			echo "3-1 done"
		fi
	else
		doneflags[3]=true
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