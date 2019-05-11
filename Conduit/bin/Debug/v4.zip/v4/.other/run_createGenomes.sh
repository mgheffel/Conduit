#!/bin/bash

#SBATCH --mem=16G
#SBATCH --nodes=1
#SBATCH --ntasks-per-node=1
#SBATCH --time=24:00:00
#SBATCH --job-name=sortCtigs

usage() { echo "Usage: bash $0 -h data_directory(ex: /bulk/dmarth027/run_022118) id dataset-name -f files(delimited by commas)" 1>&2; exit 1;}

while getopts ":h:r:s:" o; do
        case "${o}" in
                        h)
                                HERE=${OPTARG}
                                if [ ! -d $HERE ]; then echo "Directory not found $HERE"; exit 1;fi
                                HERE=$(realpath -s $HERE)
                                ;;
			r)
				reference=${OPTARG}
				;;
			s)
				samFile=${OPTARG}
				;;
                        *)
                                usage
                                ;;
        esac
done
if [ -z $HERE ] || [ -z $reference ]; then usage; fi

module load Python/3.6.3-iomkl-2017beocatb
source /homes/mgheffel/virtualenvs/mypy/bin/activate
python /homes/bioinfo/vdl/v4/pyscripts/createGenomes.py $reference $samFile
