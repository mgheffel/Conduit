#!/bin/bash -l
#       Updated v1.1 by Reza Mazloom rmazloom@ksu.edu
#       11/07/2017
#
#       originally written by Majed Alsadhan

#SBATCH --job-name=Krona
#SBATCH --nodes=1
#SBATCH --ntasks-per-node=1
#SBATCH --time=3:00:00   # Use the form DD-HH:MM:SS
#SBATCH --partition=killable.q   # Job may run as killable on owned nodes
#SBATCH --mem-per-cpu=3G   # Memory per core, use --mem= for memory per node

INPUT=$1
KRN=${@:2}
KRONA="/bulk/bioinfo/vdl/.dependencies/software/krona/scripts/ImportTaxonomy.pl"


perl $KRONA $KRN -m 3 -k -o $INPUT"/"$(basename $INPUT)"_krona.html"

module load Python/3.6.3-iomkl-2017beocatb
source /homes/mgheffel/virtualenvs/mypy/bin/activate
python /homes/bioinfo/vdl/v4/pyscripts/kronaToTable2.0.py $HERE $INPUT"/"$(basename $INPUT)"_krona.html"




touch $INPUT/../3_krona.done

rm -f $INPUT/*.krn
