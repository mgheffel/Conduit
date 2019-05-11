import sys
import os
here=sys.argv[1]

#create directory for output
if not os.path.exists(here+'/raw_trimmedContigs'):
    os.makedirs(here+'/raw_trimmedContigs')

#dictionary with dataset names as keys and dictionaries of {contig:sequence} as values
datasets={}
#read input files of contigs
for f in os.listdir(here+'/raw_denovo'):
    datasetName=f[8:f.index('_contigs')]
    datasetName=datasetName[:datasetName.rindex('_')]
    #if new dataset name found adds empty contig dictionary to datasets{} with key datasetName
    if datasetName not in datasets.keys():
        datasets[datasetName]={}
    print('trimming ' + f)
    with open(here+'/raw_denovo/'+f, 'r') as cf:
        input=cf.read()
    clines=input.split('>')
    for i in range(1,len(clines)):
        cName=clines[i][:clines[i].find("\n")].split(' ')[0]
        contig=clines[i][clines[i].find("\n"):].replace("\n","")
        #add contig to dataset if length grater than 400
        if len(contig)>=400:
            datasets[datasetName][cName]=contig
        else:
            print("Removed contig " + cName + " -shorter that 400bp")

for datasetName in datasets.keys():
    with open(here+'/raw_trimmedContigs/'+datasetName+'-contigs.fasta','w') as outf:
        for contig in datasets[datasetName]:
            outf.write('>'+contig+'\n')
            outf.write(datasets[datasetName][contig]+'\n')
with open(here+"/5BtrimContigs.done", 'w') as f:
    f.write(" ")