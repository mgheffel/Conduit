#imports and setup
print('the very beginning')
from Bio import Entrez
Entrez.email = 'mgheffel@ksu.edu'
import os
import sys
from Bio import pairwise2

#current working directory
here=sys.argv[1]
dataset=sys.argv[2]
filesString=sys.argv[3]
print(here)
#sets of next gen sequencing data (forward and reverse reads)
datasets={}

print('making test file')

outf = open(here+'/raw_contigGenDiv/'+dataset+'.txt','w')
print('Making outfile ' + here+'/raw_contigGenDiv/'+dataset+'.txt')

#read in blast data from contigs
print('reading in blast data')
blastDir=here+'/raw_blast'
print('Blast Directory: ' + blastDir)
print('files used: ')
for file in os.listdir(blastDir):
    if dataset not in file:
        continue
    #isolate dataset name by pattern matching
    if '_a5_' in file:
        datasetName=file[8:file.index('_a5_')]
    elif '_ray_' in file:
        datasetName=file[8:file.index('_ray_')]
    elif '_iva_' in file:
        datasetName=file[8:file.index('_iva_')]
    print(file)
    #if dataset name isnt in the datasets dictionary yet, add it
    if datasetName in datasets.keys():
        pass
    else:
        datasets[datasetName]={'blasts': {}}
    #open and read data in file
    with open(blastDir+'/'+file,'r') as f:
        blastInput=f.readlines()
    #parse input blast data
    for input in blastInput:
        blastParse=input.split(',')
        contigName=blastParse[0]
        blastID=blastParse[2]
        blastName=blastParse[7]
        if blastID in datasets[datasetName]['blasts'].keys():
            datasets[datasetName]['blasts'][blastID]['contigs'][contigName]={}
        else:
            datasets[datasetName]['blasts'][blastID]={'name': blastName, 'sequence': 'seq', 'contigs': {contigName: {}}}




for datasetKey in datasets.keys():
    print('\t'+datasetKey)
    #outf.write('\t'+datasetKey+'\n')
    for blastID in datasets[datasetName]['blasts'].keys():
        handle = Entrez.efetch(db="nucleotide", id=blastID, rettype="fasta", retmode="text")
        record = handle.read()
        seq=record[record.index('\n')+1:].replace('\n','').upper()
        datasets[datasetName]['blasts'][blastID]['sequence']=seq
        print(blastID+': '+seq[:10])
        #outf.write(blastID+': '+seq[:10]+'\n')

print('reading in contigs')
print('files used')
denovoDir=here+'/raw_trimmedContigs'
for file in os.listdir(denovoDir):
    if dataset not in file:
        continue
    #isolate dataset name by pattern matching
    if '_a5_' in file:
        datasetName=file[8:file.index('_a5_')]
    elif '_ray_' in file:
        datasetName=file[8:file.index('_ray_')]
    elif '_iva_' in file:
        datasetName=file[8:file.index('_iva_')]
    print('\t'+file)
    with open(denovoDir+'/'+file,'r') as f:
        contigsInput=f.read()
    contigs=contigsInput.split('>')
    for i in range(1,len(contigs)):
        contig=contigs[i]
        contigName=contig[:contig.index('\n')].split(' ')[0]
        contigSeq=contig[contig.index('\n')+1:].replace('\n','').upper()
        for blastKey in datasets[datasetName]['blasts'].keys():
            if contigName in datasets[datasetName]['blasts'][blastKey]['contigs'].keys():
                datasets[datasetName]['blasts'][blastKey]['contigs'][contigName]['sequence']=contigSeq


for datasetKey in datasets.keys():
    for blastKey in datasets[datasetName]['blasts'].keys():
        outf.write(blastID+': '+datasets[datasetName]['blasts'][blastKey]['sequence'][:10]+'\n')
        for contigKey in datasets[datasetName]['blasts'][blastKey]['contigs'].keys():
            #outf.write('\t'+datasets[datasetName]['blasts'][blastKey]['contigs'][contigKey]+'\n')
            outf.write('\t'+contigKey+': ')
            outf.write(datasets[datasetName]['blasts'][blastKey]['contigs'][contigKey]['sequence'][:10]+'\n')

def AnnotateAlignment(s1,s2):
    ann=''
    for i in range(len(s1)):
        if s1[i]==s2[i]:
            ann+='|'
        elif s1[i]=='-' or s2[i]=='-':
            ann+='-'
        elif s1[i]=='N' or s2[i]=='N':
            ann+=':'
        else:
            ann+='X'
    return ann

print('aligning contigs to blast results')
from Bio import pairwise2
for datasetKey in datasets.keys():
    #print(datasetKey)
    for blastKey in datasets[datasetKey]['blasts'].keys():
        print(blastKey)
        outf.write('>' + blastKey+ '\n')
        blastSeq=datasets[datasetKey]['blasts'][blastKey]['sequence']
        for contigKey in datasets[datasetName]['blasts'][blastKey]['contigs'].keys():
            print('\t' + contigKey)
            contigSeq=datasets[datasetKey]['blasts'][blastKey]['contigs'][contigKey]['sequence']
            #match score=1, mismatch=-1, open gap=-5, extend gap=-1
            alignment=pairwise2.align.localms(blastSeq,contigSeq,1,-1,-5,-1)[0]
            outf.write(contigKey+'\n')
            outf.write('Blast Match: ' + alignment[0]+'\n')
            outf.write('Annotation:  ' + AnnotateAlignment(alignment[0],alignment[1])+'\n')
            outf.write('Contig:      ' + alignment[1]+'\n')
            break

outf.close()
