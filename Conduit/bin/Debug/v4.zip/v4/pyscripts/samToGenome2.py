mport sys
import os
import re
import pickle
import shutil
#for use on beocat
try:
    dataDir=sys.argv[1]
    if '.fasta' in sys.argv[2]:
        referenceFile=sys.argv[2]
    else:
        print('argv[1] should be in fasta format (ex: genomes.fasta)')
        exit()
    if '.sam' in sys.argv[3]:
        readSamFile=sys.argv[3]
    else:
        print('argv[3] should be in sam format (ex: remapReads.sam)')
        exit()
    if '.sam' in sys.argv[4]:
        contigSamFile=sys.argv[4]
    else:
        print('argv[4] should be in sam format (ex: remapContigs.sam)')
        exit()
except:
    print("invalid input parameters")
    exit()

print("dataDir")
print(dataDir)
print("referenceFile")
print(referenceFile)
print("readSamFile")
print(readSamFile)
print("ContigSamFile")
print(contigSamFile)

#gets abiguity code for multiple possible nucleotides
def getAmbiguousNuc(n1,n2,n3='-1'):
    if n1=='-' or n2=='-' or n3=='-':
        return '-'
    if n1=='A':
        if n2=='T':
            if n3=='-1':
                return 'W'
            elif n3=='G':
                return 'D'
            elif n3=='C':
                return 'H'
            else:
                print("n3: "+n3)
                raise Exception('Unexpected nucleotide')
        elif n2=='G':
            if n3=='-1':
                return 'R'
            elif n3=='C':
                return 'V'
            elif n3=='T':
                return 'D'
            else:
                print("n3: "+n3)
                raise Exception('Unexpected nucleotide')
        elif n2=='C':
            if n3=='-1':
                return 'M'
            elif n3=='T':
                return 'H'
            elif n3=='G':
                return 'V'
            else:
                print("n3: "+n3)
                raise Exception('Unexpected nucleotide')
        else:
            print("n2: "+n2)
            raise Exception('Unexpected nucleotide')
    elif n1=='T':
        if n2=='A':
            if n3=='-1':
                return 'W'
            elif n3=='G':
                return 'D'
            elif n3=='C':
                return 'H'
            else:
                print("n3: "+n3)
                raise Exception('Unexpected nucleotide')
        elif n2=='G':
            if n3=='-1':
                return 'K'
            elif n3=='A':
                return 'D'
            elif n3=='C':
                return 'B'
            else:
                print("n3: "+n3)
                raise Exception('Unexpected nucleotide')
        elif n2=='C':
            if n3=='-1':
                return 'Y'
            elif n3=='A':
                return 'H'
            elif n3=='G':
                return 'B'
            else:
                print(n3)
                raise Exception('Unexpected nucleotide')
        else:
            print("n3: "+n3)
            raise Exception('Unexpected nucleotide')
    elif n1=='G':
        if n2=='A':
            if n3=='-1':
                return 'R'
            elif n3=='C':
                return 'V'
            elif n3=='T':
                return 'D'
            else:
                print("n3: "+n3)
                raise Exception('Unexpected nucleotide')
        elif n2=='T':
            if n3=='-1':
                return 'K'
            elif n3=='A':
                return 'D'
            elif n3=='C':
                return 'B'
            else:
                print("n3: "+n3)
                raise Exception('Unexpected nucleotide')
        elif n2=='C':
            if n3=='-1':
                return 'S'
            elif n3=='A':
                return 'V'
            elif n3=='T':
                return 'B'
            else:
                print("n3: "+n3)
                raise Exception('Unexpected nucleotide')
        else:
            print("n2: "+n2)
            raise Exception('Unexpected nucleotide')
    elif n1=='C':
        if n2=='A':
            if n3=='-1':
                return 'M'
            elif n3=='T':
                return 'H'
            elif n3=='G':
                return 'V'
            else:
                print("n3: "+n3)
                raise Exception('Unexpected nucleotide')
        elif n2=='T':
            if n3=='-1':
                return 'Y'
            elif n3=='A':
                return 'H'
            elif n3=='G':
                return 'B'
            else:
                print("n3: "+n3)
                raise Exception('Unexpected nucleotide')
        elif n2=='G':
            if n3=='-1':
                return 'S'
            elif n3=='A':
                return 'V'
            elif n3=='T':
                return 'B'
            else:
                print("n3: "+n3)
                raise Exception('Unexpected nucleotide')
        else:
            print("n2: "+n2)
            raise Exception('Unexpected nucleotide')
    else:
        print("n1: "+n1)
        raise Exception('Unexpected nucleotide')

def listToRanges(ls):
    if len(ls)==0:
        return "[]"
    elif len(ls)==1:
        return "["+str(ls[0])+"]"
    
    curIndex=ls[0]
    start=str(ls[0])
    rangeString="["+str(curIndex)
    inRange=True
    for i in range(1,len(ls)):
        if curIndex+1==int(ls[i]):
            curIndex=int(ls[i])
            continue
        else:
            if start==str(curIndex):
                rangeString+=', '
            else:
                rangeString+='-'+str(curIndex)+', '
            curIndex=int(ls[i])
            rangeString+=str(curIndex)
            start=str(curIndex)
    if start!=str(ls[len(ls)-1]):
        rangeString+='-'+str(ls[len(ls)-1]) 
    rangeString+=']'
    return rangeString

class nucWeight:
    def __init__(self):
        self.aCount=0
        self.tCount=0
        self.gCount=0
        self.cCount=0
        self.dCount=0
    def addNuc(self, nuc):
        if nuc=='A':
            self.aCount+=1
        elif nuc=='T':
            self.tCount+=1
        elif nuc=='G':
            self.gCount+=1
        elif nuc=='C':
            self.cCount+=1
        #ambiguous nucleotide, dont add
        elif nuc=='N':
            return
        elif nuc=='-':
            self.dCount+=1
        else:
            print(nuc)
            raise Exception('Unexpected nucleotide')
    def getTotal(self):
        return self.aCount+self.tCount+self.gCount+self.cCount+self.dCount
    def getNucOrder(self):
        total=self.aCount+self.tCount+self.gCount+self.cCount+self.dCount
        nucTuples=[('A',self.aCount),('T',self.tCount),('G',self.gCount),('C',self.cCount),('-',self.dCount)]
        nucList=sorted(nucTuples, key=lambda nuc: nuc[1], reverse=True)
        return nucList
    def getPercentOrder(self):
        total=self.getTotal()
        if total==0:
            return -1
        percentTuples=[('A',(self.aCount/total)),('T',(self.tCount/total)),('G',(self.gCount/total)),('C',(self.cCount/total)),('-',(self.dCount)/total)]
        percentList=sorted(percentTuples, key=lambda nuc: nuc[1], reverse=True)
        return percentList
    def toString(self):
        return 'A: '+str(self.aCount)+', T: '+str(self.tCount)+', G: '+str(self.gCount)+', C: '+str(self.cCount)+', -: '+str(self.dCount)

class assemblyStruct:
    def __init__(self, refID, refGenome):
        self.refID=refID
        self.refGenome=refGenome
        self.weightedReadsGenome=[nucWeight() for x in range(len(refGenome))]
        self.readInsertionList={}
        self.weightedContigsGenome=[nucWeight() for x in range(len(refGenome))]
        self.contigInsertionList={}
    def addReadNuc(self,nuc,index):
        self.weightedReadsGenome[index].addNuc(nuc)
    def addContigNuc(self,nuc,index):
        self.weightedContigsGenome[index].addNuc(nuc)
    def headReadString(self, x=5):
        outStr=''
        #add catch for the literally zero case senario where a genome is less than 5 bp
        for i in range(x):
            outStr+=self.weightedReadsGenome[i].toString()+'\n'
        return outStr

#create dictionaries of nessecary data structures with all reference genomes as keys
refAssemblyDict={}
with open(referenceFile,'r') as f:
    line=f.readline()
    while line:
        if line[0]=='>':
            refID=line[1:-1].split('_')[0]
            #print(refID)
        else:
            genome=line[:-1]
            refAssemblyDict[refID]=assemblyStruct(refID,genome)
            #refDict[refID]={'refSeq':genome, 'assemblyStruct':[nucWeight() for i in range(len(genome))], 'insertionStruct':{}}
        line=f.readline()

#ADD CONTIGS
f=open(contigSamFile,'r')
line = f.readline()
count=0
while line:
    #skip header section of sam file
    if line[0]=="@":
        line=f.readline()
        continue
    lineInfo=line.split('\t')

    refID=lineInfo[2].split('_')[0]
    #if read didn't map to a reference, skip it
    if refID=='*':
        line = f.readline()
        count+=1
        continue
    #cigar string annotates the read's map to reference (matches and indels)
    cigar=lineInfo[5]
    #dont include reads with * as cigar string
    if cigar=='*':
        line=f.readline()
        count+=1
        continue
    contigID=lineInfo[0]
      
    
    #pos-1 to start indexing at 0 rather than 1
    pos=int(lineInfo[3])-1
    #if pos>10:
    #    line=f.readline()
    #    count+=1
    #    continue
    seq=lineInfo[9]
    qual=lineInfo[10]
    #add read to assembly
    
    m=re.findall('\d+[A-Z]+',cigar)
    #index=0
    cigarRead=''
    for item in m:
        num=int(item[:-1])
        mode=item[-1:]
        if mode=='M':
            for i in range(num):
                #refDict[refID]['assemblyStruct'][pos].addNuc(readSeq[0])
                refAssemblyDict[refID].addContigNuc(seq[0],pos)
                #print(pos)
                #print(refDict[refID]['assemblyStruct'][pos])
                
                #raise Exception('I know Python!')
                seq=seq[1:]
                pos+=1
        elif mode=='I':
            insertionStruct=refAssemblyDict[refID].contigInsertionList
            for i in range(num):
                key=str(pos)+'-'+str(i)
                if key in insertionStruct.keys():
                    insertionStruct[key][seq[0]]+=1
                else:
                    insertionStruct[key]={'A':0,'T':0,'G':0,'C':0,'N':0}
                    insertionStruct[key][seq[0]]+=1
                seq=seq[1:]
        elif mode=='D':
            #refDict[refID]['assemblyStruct'][pos].addNuc('D')
            for i in range(num):
                refAssemblyDict[refID].addContigNuc('-',pos)
                pos+=1
        else:
            raise Exception('Unexpected cigar tag')
    

    line = f.readline()


f.close()

for key in refAssemblyDict.keys():
    assemblyStruct=refAssemblyDict[key]
    print(key)
    weightedContigsGenome=assemblyStruct.weightedContigsGenome
    #for i in range(len(weightedContigsGenome)):
    for i in range(10):    
        weightedNuc=weightedContigsGenome[i]
        print(weightedNuc.getNucOrder())
    break

refContigLists={}
#align contigs
f=open(contigSamFile,'r')
line = f.readline()
while line:
    #skip header section of sam file
    if line[0]=="@":
        line=f.readline()
        continue
    lineInfo=line.split('\t')

    refID=lineInfo[2].split('_')[0]
    if refID not in refContigLists:
        refContigLists[refID]=[]
    #if read didn't map to a reference, skip it
    #if refID=='*':
    if refID!='*':
        line = f.readline()
        continue
    #cigar string annotates the read's map to reference (matches and indels)
    cigar=lineInfo[5]
    #dont include reads with * as cigar string
    if cigar=='*':
        line=f.readline()
        continue
    contigID=lineInfo[0]
    
            
    pos=int(lineInfo[3])
    
    m=re.findall('\d+[A-Z]+',cigar)
    #print(contigID)
    #print(cigar)
    #print(pos)
    endIndex=pos
    for item in m:
        num=int(item[:-1])
        #print('num: '+str(num))
        mode=item[-1:]
        if mode!='I':
            endIndex+=num
    #print(endIndex)
    #print()
    
    seq=lineInfo[9]
    refContigLists[refID].append({'ID':contigID,'startIndex':pos,'endIndex':endIndex,'seq':seq,'cigar':cigar})
    line=f.readline()

refID='KU159364'

newContigs=[]
for contig1 in refContigLists[refID]:
    id1=contig1['ID']
    start1=contig1['startIndex']
    if start1>511:
        continue
    else:
        print(id1)
    end1=contig1['endIndex']
    cigar1=contig1['cigar']
    seq1=contig1['seq']
    for contig2 in refContigLists[refID]:
        id2=contig2['ID']
        if id1==id2:
            continue
        start2=contig2['startIndex']
        end2=contig2['endIndex']
        cigar2=contig2['cigar']
        seq2=contig2['seq']
        
        if start2<end1 and end2>end1:
            startDiff=start2-start1
            #get overlapping seq for contig1
            m=re.findall('\d+[A-Z]+',cigar1)
            startFlag=False
            frontCut=0
            lenOverlap=0
            for item in m:
                if startDiff==0:
                    break
                num=int(item[:-1])
                mode=item[-1:]
                if not startFlag:
                    if startDiff-num>=0:
                        if mode !='I':
                            startDiff-=num
                        frontCut+=num
                    else:
                        num-=startDiff
                        frontCut+=startDiff
                        startDiff=0
                        startFlag=True
            overlap1=seq1[frontCut:]
            if len(overlap1)<50:
                continue
            lenOverlap=len(overlap1)
            #print(len(overlap1))
            #get overlapping seq for contig2
            m=re.findall('\d+[A-Z]+',cigar2)
            frontKeep=0
            combinedCigar=cigar1
            for item in m:
                if lenOverlap<=0:
                    combinedCigar+=item
                    continue
                num=int(item[:-1])
                mode=item[-1:]
                if lenOverlap-num>=0:
                    #print('in1')
                    lenOverlap-=num
                    frontKeep+=num
                else:
                    #print('in2')
                    frontKeep+=lenOverlap
                    combinedCigar+=str(num-lenOverlap)+mode
                    lenOverlap=0
            overlap2=seq2[:frontKeep]
            
            if overlap1==overlap2:
            #    print(id1)
            #    print(id2)
            #    print(cigar1)
            #    print(cigar2)
            #    print(overlap1)
            #    print(overlap2)
            #    print(combinedCigar)
                
                #break
                newContigs.append({'ID':id1+id2,'startIndex':start1,'endIndex':end2,'seq':seq1+seq2[frontKeep:],'cigar':combinedCigar})
            #    #print(seq1+'   '+seq2[frontKeep:])
            #    print(len(seq1+seq2[frontKeep:]))
            #    print(len(newContigs[0]['seq']))
            #    print(newContigs[0]['endIndex'])
            #    print()
    #break

refID='KU159364'
finalContigs=[]

flag=True
while len(newContigs)!=0:
    oldContigs=newContigs
    newContigs=[]
    for contig1 in oldContigs:
        id1=contig1['ID']
        start1=contig1['startIndex']
        end1=contig1['endIndex']
        cigar1=contig1['cigar']
        seq1=contig1['seq']
        for contig2 in refContigLists[refID]:
            id2=contig2['ID']
            if id1==id2:
                continue
            start2=contig2['startIndex']
            end2=contig2['endIndex']
            cigar2=contig2['cigar']
            seq2=contig2['seq']

            if start2<end1 and end2>end1+300:
                startDiff=start2-start1
                #get overlapping seq for contig1
                m=re.findall('\d+[A-Z]+',cigar1)
                startFlag=False
                frontCut=0
                lenOverlap=0
                for item in m:
                    if startDiff==0:
                        break
                    num=int(item[:-1])
                    mode=item[-1:]
                    if not startFlag:
                        if startDiff-num>=0:
                            if mode !='I':
                                startDiff-=num
                            frontCut+=num
                        else:
                            num-=startDiff
                            frontCut+=startDiff
                            startDiff=0
                            startFlag=True
                overlap1=seq1[frontCut:]
                lenOverlap=len(overlap1)
                if len(overlap1)<100:
                    continue
                #print(len(overlap1))
                #get overlapping seq for contig2
                m=re.findall('\d+[A-Z]+',cigar2)
                frontKeep=0
                combinedCigar=cigar1
                for item in m:
                    if lenOverlap<=0:
                        combinedCigar+=item
                        continue
                    num=int(item[:-1])
                    mode=item[-1:]
                    if lenOverlap-num>=0:
                        #print('in1')
                        lenOverlap-=num
                        frontKeep+=num
                    else:
                        #print('in2')
                        frontKeep+=lenOverlap
                        combinedCigar+=str(num-lenOverlap)+mode
                        lenOverlap=0
                overlap2=seq2[:frontKeep]

                if overlap1==overlap2:
                    #print(end1)
                    #print(start2)
                    #print(len(seq1))
                    #print(frontCut)
                    #print(id1)
                    #print(id2)
                    #print(cigar1)
                    #print(cigar2)
                    #print(overlap1)
                    #print(overlap2)
                    #print(combinedCigar)
                    #print()
                    #break
                    newContigs.append({'ID':id1+id2,'startIndex':start1,'endIndex':end2,'seq':seq1+seq2[frontKeep:],'cigar':combinedCigar})
                    #print(seq1+'   '+seq2[frontKeep:])
                    #print(len(seq1+seq2[frontKeep:]))
                    if len(seq1+seq2[frontKeep:])>6500:
                        finalContigs.append({'ID':id1+id2,'startIndex':start1,'endIndex':end2,'seq':seq1+seq2[frontKeep:],'cigar':combinedCigar})
                    #print('-------------------------------------------------------')

with open('sdkfjn.fasta','w') as f:
    i=0
    for item in finalContigs:
        print('contig'+str(i))
        print(item['ID'])
        i+=1
        print()
        #print(item['seq'])
        f.write('>contig'+str(i)+'\n')
        f.write(item['seq']+'\n')


