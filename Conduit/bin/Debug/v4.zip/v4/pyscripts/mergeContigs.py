import sys
import os
import re
import pickle
import shutil

dataDir=sys.argv[1]
#contigfile=dataDir+'\\raw_trimmedContigs\\E2358_S3_L001-contigs.fasta'
contigfile=dataDir+'/raw_trimmedContigs/'+sys.argv[2]
#contigfile='raw_trimmedContigs/E2358_S3_L001-contigs.fasta'
outff=dataDir+'/raw_mergedContigs/'+sys.argv[2]
outaf=dataDir+'/raw_mergedContigs/'+sys.argv[2][:-5]+'assm'
print(dataDir)
print(contigfile)
print(outff)
print(outaf)

print("Stopped here?")

contigs={}
cf=open(contigfile,'r')
line=cf.readline()
cname=''
while line:
    if line[:1]=='>':
        #remove first and last character (> and \n)
        cname=line[1:-1]
    else:
        contigs[cname]={'seq':line[:-1].upper(),'leftExtends':[],'rightExtends':[]}
    line=cf.readline()
    
#for key in contigs.keys(): print(key)
print("or here?")

overlapReq=50
cnames=contigs.keys()
#cnames=['contig.00006']
for cname in cnames:
    cSeq=contigs[cname]['seq']
    #find right based extendors
    tail50=cSeq[-overlapReq:]
    for cname2 in contigs.keys():
        if cname==cname2:
            continue
        cSeq2=contigs[cname2]['seq']
        overlapIndex=cSeq2.find(tail50)
        if overlapIndex!=-1 and overlapIndex<len(cSeq)+overlapReq:
            #print(cname)
            #print(tail50)
            #print(cSeq2[overlapIndex:overlapIndex+overlapReq])
            overlap1=cSeq[-(overlapIndex+overlapReq):]
            overlap2=cSeq2[:overlapIndex+overlapReq]
            #print(overlap1)
            #print(overlap2)
            if overlap1==overlap2:
                #print(cname)
                #print(cname2)
                contigs[cname]['rightExtends'].append(cname2)
                #print()
#add left based extendors
for cname in contigs.keys():
    rightExtendors=contigs[cname]['rightExtends']
    for cname2 in rightExtendors:
        contigs[cname2]['leftExtends'].append(cname)

#print(contigs['contig-3']['leftExtends'])

#Left based build
startPoints=[]
for cname in contigs:
    contig=contigs[cname]
    if len(contig['leftExtends'])==0 and len(contig['rightExtends'])==0:
        continue
    if len(contig['leftExtends'])==0:
        startPoints.append(cname)
print("start points")
print(startPoints)

finalAssemblyStrings=[]
#startPoints=['contig.00006','contig.00007']
assemblyStrings=startPoints
flag=True
while flag:
    flag=False
    newAssemblyStrings=[]
    for assemblyString in assemblyStrings:
        cname=assemblyString.split(',')[-1]
        #print(cname)
        contig=contigs[cname]
        #print(contig['rightExtends'])
        if len(contig['rightExtends'])==0:
            finalAssemblyStrings.append(assemblyString)
            continue
        for cname2 in contig['rightExtends']:
            newAssemblyStrings.append(assemblyString+','+cname2)
            flag=True
    assemblyStrings=newAssemblyStrings
    #print(assemblyStrings)

print()
print("Final assembly strings")
for s in finalAssemblyStrings:
    print(s)

fasSeqs=[]
for s in finalAssemblyStrings:
    cnames=s.split(',')
    #print(cnames[0])
    seq=contigs[cnames[0]]['seq']
    for cname in cnames[1:]:
        tail50=seq[-50:]
        cSeq=contigs[cname]['seq']
        startI=cSeq.index(tail50)
        seq+=cSeq[startI+50:]
    fasSeqs.append(seq)


af=open(outaf,'w')
ff=open(outff,'w')
for i in range(len(finalAssemblyStrings)):
    s=finalAssemblyStrings[i]
    seq=fasSeqs[i]
    cnames=s.split(',')
    annoh=' ' *len(seq)
    anno1=' ' *len(seq)
    anno2=' ' *len(seq)
    inAnno1=True
    for cname in cnames:
        cSeq=contigs[cname]['seq']
        head50=cSeq[:50]
        startI=seq.index(head50)
        if inAnno1:
            anno1=anno1[:startI]+cSeq+anno1[startI+len(cSeq)+1:]
            annoh=annoh[:startI]+cname+annoh[startI+len(cname)+1:]
            inAnno1=False
        else:
            anno2=anno2[:startI]+cSeq+anno2[startI+len(cSeq)+1:]
            annoh=annoh[:startI]+cname+annoh[startI+len(cname)+1:]
            inAnno1=True
    af.write('>assembly'+str(i)+'\n')
    ff.write('>assembly'+str(i)+'\n')
    af.write(seq+'\n')
    ff.write(seq+'\n')
    af.write(s+'\n')
    af.write(annoh+'\n')
    af.write(anno1+'\n')
    af.write(anno2+'\n')
    af.write('-\n')
    
af.close()
ff.close()
