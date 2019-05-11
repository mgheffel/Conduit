import os
import sys
from Bio import Entrez
from Bio import SeqIO
import time
Entrez.email = "mgheffel@ksu.edu"
#sys.argv[1]=dataset directory
here=sys.argv[1]
#sys.argv[2]=input file
inputFile=sys.argv[2]
outputFile=here+'/raw_blastSequences/'+inputFile[:inputFile.rindex('-')]+'.fasta'
flagfileName=outputFile[:-5]+'flag'
print(flagfileName)
flagfile=open(flagfileName,'w')
flagfile.write(' ')
flagfile.close



with open(here+'/raw_blast/'+inputFile,'r') as f:
    lines=f.readlines()
#dictionary with key=refID and value={info}
blastInfos={}
genomeRemoved=[]
taxonRemoved=[]
for line in lines:
    count=0
    split=line.split(',')
    name=split[7]
    #collect only complete genomes
    #if not split[8][:16]==' complete genome':
        #if name not in genomeRemoved:
        #    genomeRemoved.append(name)
        #print("Removed: "+name+' --incomplete genome')
        #continue
    if 'VIRUS' not in name.upper():
        if name not in taxonRemoved:
            taxonRemoved.append(name)
        #print("Removed: "+name+' --not a virus')
        continue
    refID=split[2]
    if refID not in blastInfos.keys():
        blastInfos[refID]={'name':name}
#print removals
for removal in genomeRemoved:
    print("Removed: "+removal+' --incomplete genome')
print()
for removal in taxonRemoved:
    print("Removed: "+removal+' --not a virus')
outf=open(outputFile,'w')
for refID in blastInfos.keys():
    flag=True
    while flag:
        try:
            handle = Entrez.efetch(db="nucleotide", id=refID, rettype="gb", retmode="text")
            record = SeqIO.read(handle,"genbank")
            handle.close()
            seq=record.seq
            try:
                name=record.description
            except:
                name='name-not-found'
                print('nnn')
            outf.write('>'+refID+'\n')
            outf.write(str(seq)+'\n')
            flag=False
        except:
            time.sleep(10)
print('sequences written to: '+outputFile)
os.remove(flagfileName)
