#!/bin/bash

#SBATCH --mem=20G
#SBATCH --nodes=1
#SBATCH --ntasks-per-node=1
#SBATCH --time=1:00:00
#SBATCH --job-name=TrimContigs

usage() { echo "Usage: bash $0 -h data_directory(ex: /bulk/dmarth027/run_022118)" 1>&2; exit 1;}

while getopts ":h:b:" o; do
        case "${o}" in
                        h)
                                HERE=${OPTARG}
                                if [ ! -d $HERE ]; then echo "Directory not found $HERE"; exit 1;fi
                                HERE=$(realpath -s $HERE)
                                ;;
			b)
				debugDir=${OPTARG}
				;;
                        *)
                                usage
                                ;;
        esac
done
if [ -z $HERE ]; then usage; fi

mkdir $debugDir

module load Python/3.6.3-foss-2017beocatb
source /homes/mgheffel/virtualenvs/mypy/bin/activate
python /homes/bioinfo/vdl/v4/pyscripts/lessContigs.py $HERE
