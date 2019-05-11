#!/bin/bash
#$ -cwd'
#
# 	Updated by Reza Mazloom rmazloom@ksu.edu
#	created by Majed Alsadhan
#	Version 1.0.1	
#
#SBATCH --nodes=1
#SBATCH --time=24:00:00
#SBATCH --job-name=Denovo
#

CWD="/homes/bioinfo/vdl/v4"
declare -a assemblies=()
#A new assembly can be made available here with a variable name callable in the parameter -c
ray=$CWD/.other/ray_run.sh
iva=$CWD/.other/iva_run.sh
a5=$CWD/.other/a5_run.sh
LOG="$CWD/logs/"
chain=$CWD/.other/chain.sh
JOBS=""


#print help
usage() { echo "Usage: $0 -i reads_directory_to_assemble -a assembly_names(iva,...) [-c job_configuration]" 1>&2; exit 1;}

while getopts ":i:a:c:b:" o; do
	case "${o}" in
		i)
			input=${OPTARG}
			if [ ! -d "$input" ]; then usage; fi
			;;
		a)
			software=${OPTARG}
			;;
		c) 
			config=${OPTARG}
			;;
		b)
			debugDir=${OPTARG}
			;;
		*)
			usage
			;;
	esac
done

if [ -z "$input" ] || [ -z "$software" ]; then
	usage
fi

mkdir $debugDir
input=$(realpath -s $input)

#softarr=',' read -ra array <<< "$software"

softarr=(${software//,/ })

for i in "${softarr[@]}"; do
	if [ -z ${!i} ];then
		echo "This assembler is not available: $i"
	else
		assemblies+=(${!i})
	fi
done
#declare -a assemblies=($D1 $D2)
echo 
echo "Submitting \"DeNovo\" jobs..."
echo
for A in "${assemblies[@]}"
do
	echo "using $A"
	assembly_name=$(echo $(basename $A) | cut -f 1 -d '_')
	FINALRESULTS="$(echo $input | rev | cut -f 2- -d '_' | rev)_denovo"

	#rm -rf $FINALRESULTS
	mkdir $FINALRESULTS

	paste <(ls -1 $input/*_R1*) <(ls -1 $input/*_R2*) | while IFS="$(printf '\t')" read -r  p1 p2 ;
	do
		log="$LOG/$(basename $p1)_$assembly_name.log"
		JID=$(sbatch $config $A $p1 $p2 $FINALRESULTS/$(echo $(basename $p1) | sed 's/^\(.*\)_R1.*$/\1/')_${assembly_name}_contigs.fasta)
		JID=$(echo $JID | rev | cut -f 1 -d ' '|rev)

		#JOBS="${JOBS}${JID},"
	done
done
#JOBS=$(echo $JOBS | sed 's/,\([^,]*\)$/ \1/')
sbatch $config --dependency $JOBS $chain $input/../5_denovo.done
sleep 120
HERE=$FINALRESULTS
echo "here $HERE"
echo "$FINALRESULTS"
echo
echo "After the job is done, you can find the the contigs in $input""_denovo"
echo 
echo "After that, you can run: bash 6_blast.sh -I $input""_denovo"
echo
echo
echo "Thank you!" 

#waits on Ray or a5 assembler to finish
flag=0
while [ $flag == 0 ]; do
        flag=1
	dirlist=$(ls $FINALRESULTS/ | grep -v ".+")
        for f in $dirlist
        do
                #echo "f:-6 ${f:(-6)}"
                endSix=${f:(-6)}
                #echo "endSix $endSix"
                if [ $endSix != ".fasta" ]; then
                        #echo "in dolsf $f"
                        flag=0
                fi
                #echo "f $f"
        done
        sleep 30
done
#waits on IVA assembler to finish
flag=0
while [ $flag == 0 ]; do
	flag=1
	dirlist=$(ls ${FINALRESULTS::-10} | grep -E "raw_denov.+")
	#echo $dirlist
	for f in $dirlist
	do
		endSix=${f:(-6)}
		if [ $endSix != "denovo" ]; then
			#echo $endSix
			flag=0
		fi
	done
	sleep 30
done
donefile=${FINALRESULTS::-10}
touch ${donefile}5_denovo.done
