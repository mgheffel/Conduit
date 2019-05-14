#!/bin/bash -l
#       Updated v1.1 by Reza Mazloom rmazloom@ksu.edu
#       11/07/2017
#
#       originally written by Majed Alsadhan

#SBATCH --job-name=Krona
#SBATCH --nodes=1
#SBATCH --ntasks-per-node=1
#SBATCH --time=3:00:00

#SBATCH --partition=killable.q   # Job may run as killable on owned nodes
#SBATCH --mem-per-cpu=3G   # Memory per core, use --mem= for memory per node

INPUT=$1
outdir=$2
KRN=${@:3}
KRONA="/bulk/bioinfo/vdl/.dependencies/software/krona/scripts/ImportTaxonomy.pl"


perl $KRONA $KRN -m 3 -k -o $outdir/"krona.html"

module load Python/3.6.3-iomkl-2017beocatb
source /homes/mgheffel/virtualenvs/mypy/bin/activate
python /homes/bioinfo/vdl/v4/pyscripts/kronaToTable2.0.py $HERE $outdir/"krona.html"

rm -f $INPUT/*.krn
