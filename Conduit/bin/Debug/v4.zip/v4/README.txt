Marthaler Lab Bioinformatic Pipeline

Required Software:
	-PrinSeq
	-SeqPurge
	-Kraken
	-Krona
	-Python3

Scripts:
	-0_runall.sh:
		-runs all steps of the pipeline waiting on each sequencial step to finish before starting the next and branching off non dependent steps of the pipeline
	-1_trim.sh
		-Trims the adapters off the reads and removes low quality reads
	-2_kraken.sh
		-Loads up the Kraken database and maps reads to taxonomies based of k-mers
	-3_krona.sh
		-Visualizes the output of the Kraken Script
