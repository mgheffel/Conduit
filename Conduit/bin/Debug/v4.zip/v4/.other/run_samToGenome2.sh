#!/bin/bash

#SBATCH --mem=32G
#SBATCH --nodes=1
#SBATCH --ntasks-per-node=1
#SBATCH --time=12:00:00
#SBATCH --job-name=samToGenome

usage() { echo "Usage: bash $0 -h data_directory(ex: /bulk/dmarth027/run_022118) -g refGenomesFile.fasta -r readsToRef.sam -c contigsToRef.sam" 1>&2; exit 1;}

while getopts ":h:g:r:c:" o; do
        case "${o}" in
                        h)
                                HERE=${OPTARG}
                                if [ ! -d $HERE ]; then echo "Directory not found $HERE"; exit 1;fi
                                HERE=$(realpath -s $HERE)
                                ;;
			g)
				refFile=${OPTARG}
				;;
			r)
				readSamFile=${OPTARG}
				;;
			c)
				contigSamFile=${OPTARG}
				;;
                        *)
                                usage
                                ;;
        esac
done
if [ -z $HERE ] || [ -z $refFile ] || [ -z $readSamFile ] || [ -z $contigSamFile ]; then usage; fi

module load Python/3.6.3-iomkl-2017beocatb
source /homes/mgheffel/virtualenvs/mypy/bin/activate
python /homes/bioinfo/vdl/v4/pyscripts/samToGenome.py $HERE $refFile $readSamFile $contigSamFile
