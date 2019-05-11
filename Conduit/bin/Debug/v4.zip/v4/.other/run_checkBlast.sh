#!/bin/bash

#SBATCH --mem=8G
#SBATCH --nodes=1
#SBATCH --ntasks-per-node=1
#SBATCH --time=2:00:00
#SBATCH --job-name=checkBlast

usage() { echo "Usage: bash $0 -h data_directory(ex: /bulk/dmarth027/run_022118) -b blast_directory -c contigs_directory" 1>&2; exit 1;}

while getopts ":h:c:b:" o; do
        case "${o}" in
                        h)
                                HERE=${OPTARG}
                                if [ ! -d $HERE ]; then echo "Directory not found $HERE"; exit 1;fi
                                HERE=$(realpath -s $HERE)
                                ;;
			c)
				contigDir=${OPTARG}
				;;
			b)
				blastDir=${OPTARG}
				;;
                        *)
                                usage
                                ;;
        esac
done
if [ -z $HERE ] || [ -z $contigDir ] || [ -z $blastDir ]; then usage; fi

mkdir $HERE/raw_noBlastHits/

module load Python/3.6.3-iomkl-2017beocatb
source /homes/mgheffel/virtualenvs/mypy/bin/activate
python /homes/bioinfo/vdl/v4/pyscripts/checkBlast.py $HERE/ $contigDir $blastDir
