#!/bin/bash

#SBATCH --mem=16G
#SBATCH --nodes=1
#SBATCH --ntasks-per-node=1
#SBATCH --time=6:00:00
#SBATCH --job-name=cleanBlast

usage() { echo "Usage: bash $0 -h data_directory(ex: /bulk/dmarth027/run_022118) -i input_blastFile" 1>&2; exit 1;}

while getopts ":h:i:" o; do
        case "${o}" in
                        h)
                                HERE=${OPTARG}
                                if [ ! -d $HERE ]; then echo "Directory not found $HERE"; exit 1;fi
                                HERE=$(realpath -s $HERE)
                                ;;
			i)
				input=${OPTARG}
				;;
                        *)
                                usage
                                ;;
        esac
done
if [ -z $HERE ] || [ -z $input ]; then usage; fi

module load Python/3.6.3-iomkl-2017beocatb
source /homes/mgheffel/virtualenvs/mypy/bin/activate
python /homes/bioinfo/vdl/v4/pyscripts/cleanBlast.py $HERE $input
