#!/bin/bash 
#
#       Written by Matthew G. Heffel, mgheffel@ksu.edu
#	1/2/2019
#
#

#SBATCH --nodes=1
#SBATCH --time=48:00:00
#SBATCH --job-name=runPipe
start=$SECONDS

export PATH="$PATH:/homes/bioinfo/vdl/v4"
pipePath="/homes/bioinfo/vdl/v4"
KRAKENDB=/bulk/bioinfo/vdl/.dependencies/krakenDBs
ASSEMBLY=$CMD/.other
usage() { echo "Usage: sbatch $0 -i raw_reads_directory -d Kraken_database_name(DB21) -a assembly_list(ray,iva,...) [-t trimming_adapters] [-c job_configurations]" 1>&2; exit 1; }

while getopts ":i:d:a:t:c:" o; do
	case "${o}" in
		i)
			input=${OPTARG}
			if [ ! -d $input ]; then echo "Directory not found: $input";fi
			input=$(realpath -s $input)
			;;
		d)
			database=${OPTARG}
			if [ ! -d $KRAKENDB/$database ]; then echo "Directory not found: $KRAKENDB/$database";fi
			;;
		a)
			assemblies=${OPTARG}
			;;
		t)
			adapters=${OPTARG}
			;;
		c)	
			config=${OPTARG}
			;;
		*)
			usage
			;;
	esac
done
#shift $((OPTIND-1))
if [ -z $input ] || [ -z $database ] || [ -z $assemblies ]; then echo "Please input mandatory parameters"; usage; fi

debugDir=${input::-3}debug
mkdir $debugDir

#while : 
for (( ; ; ))
	do
	if [ -e $input/../8_remapReads.done ]; then
		echo "remap to reference done"
		sbatch -o ${debugDir}/9_createGenomes.out ${pipePath}/9_createGeneoms.sh -h ${input::-3} -g ${input}_blastSequences -r ${input}_remapReadsToRef -c ${input}_remapCtigsToRef -b $debugDir
		echo "Submitted create genomes job"
	elif [ -e $input/../7B_cleanBlast.done ]; then
        	echo "blast done"
        	sbatch -o ${debugDir}/8_remapCtigs.out ${pipePath}/8_remapContigsToRef.sh -h ${input::-3}
		sbatch -o ${debugDir}/8_remapReads.out ${pipePath}/8_remapReadsToRef.sh -r ${input}_blastSequences -m ${input}_cleaned -b $debugDir
		echo Submitted remap to reference jobs
	elif [ -e $input/../7_blast.done ]; then
		echo "blast done"
		sbatch -o ${debugDir}/7B_cleanBlast.out ${pipePath}/7B_cleanBlast.sh -h ${input::-3} -b $debugDir
		echo "submitted clean blast job"
	elif [ -e $input/../6_remap.done ]; then
		echo "remap done"
		#6C will have to be moved to after 6B once it is implemented
        	sbatch -o ${debugDir}/6C_contigsToFastq.out ${pipepath}/6C_contigsToFastq.sh -h ${input::-3}
		echo "submitted contigsToFastq job"
		sbatch -o ${debugDir}/7_blastStep.out ${pipePath}/7_blast.sh -i ${input}_trimmedContigs
		echo "Submitted blast job"
	elif [ -e $input/../5BtrimContigs.done ]; then
		echo "Trim Contigs done"
		sbatch -o ${debugDir}/6_remapStep.out ${pipePath}/6_remapReadsToContigs.sh -r ${input}_trimmedContigs -m ${input}_cleaned -b $debugDir
        	echo "Submitted remap job"
	elif [ -e $input/../5_denovo.done ]; then
		echo "get_unknown_viral done"
		sbatch -o ${debugDir}/5_denovoStep.out ${pipePath}/5_denovo.sh -i ${input}_unknown-viral -a $assemblies -b "$debugDir"
		echo "Submitted Denovo job"
	elif [ -e $input/../4_unknown_viral.done ]; then
		echo "get_unknown_viral done"
		sbatch -o ${debugDir}/5_denovoStep.out ${pipePath}/5_denovo.sh -i ${input}_unknown-viral -a $assemblies -b "$debugDir"
		echo "Submitted Denovo job"
	elif [ -e $input/../debug/2_kraken.done ]; then
		echo "Kraken done"
		sbatch -o ${debugDir}/3_kronaStep.out ${pipePath}/3_krona.sh -k ${input}_kraken -c "$config" -b "$debugDir"
		echo "Submitted Krona job"
		sbatch -o ${debugDir}/4_unknownViralStep.out ${pipePath}/4_get_unknown_viral.sh -i ${input}_cleaned -k ${input}_kraken -d $database -b "$debugDir"
		echo "Submitted unknown-viral job"
	elif [ -e $input/../1_trim.done ]; then
		echo "Trim job done"
		#sbatch prinseq.sh -i ${input}_cleaned
		#echo "Submitted prinseq job of cleaned files"
		echo "Submitting kraken job"
		sbatch -o ${debugDir}/2_krakenStep.out ${pipePath}/2_kraken.sh -i ${input}_cleaned -d $database -c "$config" -b "$debugDir"
	else
		echo "Submitted prinseq job of initial files"
		if [ ! -z $adapters  ]; then
			sbatch -o ${debugDir}/1_trimStep.out ${pipePath}/1_trim.sh -i $input -a "$adapters" -c "$config" -b "$debugDir"
		else
			sbatch -o ${debugDir}/1_trimStep.out ${pipePath}/1_trim.sh -i $input -c "$config" -b "$debugDir"
		echo "Submitted trim job"
		fi
	fi
	sleep 30


done
duration=$(( SECONDS - start ))
echo "Pipeline ran in " $duration

