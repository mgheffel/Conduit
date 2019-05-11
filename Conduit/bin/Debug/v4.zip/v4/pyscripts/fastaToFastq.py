import os
import sys
import shutil
#sys.argv[1] should be dataset directory
here=sys.argv[1]
if here=='-f':
    here='here'

#make output directory
if 'raw_fastqContigs' in os.listdir(here):
    shutil.rmtree(here+'/raw_fastqContigs')
os.mkdir(here+'/raw_fastqContigs')

for filename in os.listdir(here+'/raw_trimmedContigs'):
    if filename[-6:]!='.fasta':
        continue
    outFilename=here+'/raw_fastqContigs/'+filename[:-1]+'q'
    print(outFilename)
    filename=here+'/raw_trimmedContigs/'+filename
    print(filename)
    inf=open(filename,'r')
    lines=inf.readlines()
    inf.close()
    outf=open(outFilename,'w')
    for line in lines:
        if line[0]=='>':
            outf.write('@'+line[1:])
        else:
            outf.write(line)
            outf.write('+\n')
            qualLine='~'*(len(line)-1)
            outf.write(qualLine+'\n')
    outf.close()
