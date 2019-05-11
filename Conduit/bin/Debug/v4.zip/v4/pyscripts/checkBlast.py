import sys
import os
here=sys.argv[1]
contigDir=sys.argv[2]
blastDir=sys.argv[3]

outdir=here+'/raw_noBlastHits/'
#dictionary with dataset names as keys and dictionaries of {contig:sequence} as values
datasets={}
#read input files of contigs
print(here+contigDir)
for filename in os.listdir(here+contigDir):
    if filename[-6:]!='.fasta':
        continue
    fastaName=filename
    blastName=filename[:-5]+'blast'
    blastedNames=[]
    bf=open(here+blastDir+blastName,'r')
    line=bf.readline()
    while line:
        cName=line.split(',')[0]
        if cName not in blastedNames:
            blastedNames.append(cName)
        line=bf.readline()
    bf.close()
    noHitContigs={}
    ff=open(here+contigDir+fastaName,'r')
    line=ff.readline()
    flag=False
    while line:
        if line[:1]=='>':
            contigName=line[1:].strip().split(' ')[0].strip()
            if contigName not in blastedNames:
                key=contigName
                flag=True
        elif flag:
            noHitContigs[key]=line[:-1]
            flag=False
        line=ff.readline()
    ff.close()
    outf=open(outdir+fastaName,'w')
    print(filename[:-6])
    for c in noHitContigs.keys():
        print('\t'+c)
        outf.write('>'+c+'\n')
        outf.write(noHitContigs[c]+'\n')
    outf.close()
