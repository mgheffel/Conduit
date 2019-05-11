#!/bin/bash

#SBATCH --mem=100G
#SBATCH --nodes=1
#SBATCH --ntasks-per-node=1
#SBATCH --time=24:00:00
#SBATCH --job-name=CtigRefAlign

usage() { echo "Usage: bash $0 -h data_directory(ex: /bulk/dmarth027/run_022118) id dataset-name -f files(delimited by commas)" 1>&2; exit 1;}

while getopts ":h:d:f:" o; do
        case "${o}" in
                        h)
                                HERE=${OPTARG}
                                if [ ! -d $HERE ]; then echo "Directory not found $HERE"; exit 1;fi
                                HERE=$(realpath -s $HERE)
                                ;;
			d)
				dataset=${OPTARG}
				;;
			f)
				filesString=${OPTARG}
				;;
                        *)
                                usage
                                ;;
        esac
done
if [ -z $HERE ] || [ -z $filesString ]; then usage; fi


module load Python/3.6.3-iomkl-2017beocatb
source /homes/mgheffel/virtualenvs/mypy/bin/activate
python /homes/bioinfo/vdl/v4/pyscripts/alignContigToBlast.py $HERE $dataset $filesString
