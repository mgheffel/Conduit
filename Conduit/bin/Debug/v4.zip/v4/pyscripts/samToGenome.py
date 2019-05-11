import sys
import os
import re
import pickle
import shutil
from Bio import Entrez
from Bio import SeqIO
import time
Entrez.email = "mgheffel@ksu.edu"
#dataset='02900'
dataset='03267'
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


#add Reads
f=open(readSamFile,'r')
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
    readID=lineInfo[0]
      
    
    #pos-1 to start indexing at 0 rather than 1
    pos=int(lineInfo[3])-1
    #if pos>10:
    #    line=f.readline()
    #    count+=1
    #    continue
    readSeq=lineInfo[9]
    readQual=lineInfo[10]
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
                refAssemblyDict[refID].addReadNuc(readSeq[0],pos)
                #print(pos)
                #print(refDict[refID]['assemblyStruct'][pos])
                
                #raise Exception('I know Python!')
                readSeq=readSeq[1:]
                pos+=1
        elif mode=='I':
            insertionStruct=refAssemblyDict[refID].readInsertionList
            for i in range(num):
                key=str(pos)+'-'+str(i)
                if key in insertionStruct.keys():
                    insertionStruct[key][readSeq[0]]+=1
                else:
                    insertionStruct[key]={'A':0,'T':0,'G':0,'C':0,'N':0}
                    insertionStruct[key][readSeq[0]]+=1
                readSeq=readSeq[1:]
        elif mode=='D':
            #refDict[refID]['assemblyStruct'][pos].addNuc('D')
            for i in range(num):
                refAssemblyDict[refID].addReadNuc('-',pos)
                pos+=1
        else:
            raise Exception('Unexpected cigar tag')
    
    
    
    line = f.readline()


f.close()

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
datasetFilename=referenceFile[19:-6]
f=open(dataDir+'/raw_assembledGenomes/'+datasetFilename+'NewGenomes.fasta','w')
refIDs=refAssemblyDict.keys()
#refIDs=['KU159364','MH043955']
for refID in refIDs:
    refGenome=refAssemblyDict[refID].refGenome
    weightedReadsGenome=refAssemblyDict[refID].weightedReadsGenome
    weightedContigsGenome=refAssemblyDict[refID].weightedContigsGenome
    
    #noCoverageLocs=[]
    #totalCoverageCount=0
    lowCoverageLocs=[]
    lowCoverageCount=0
    totalCoverageCount=0
    totalIdentityCount=0
    totalPositionMagnitude=0
    #list of tuples for mismatches to reference genome (index, reference, new)
    mismatchTuples=[]
    #list of tuples for ambiguous nucleotides (index, totalCoverage, info)
    ambiquityTuples=[]
    #iterate through each weighted genome position to assemble new genome
    newGenome=''
    for i in range(len(refGenome)):
        weightedReadsNuc=weightedReadsGenome[i]
        totalReadsCoverage=weightedReadsNuc.getTotal()
        totalPositionMagnitude+=totalReadsCoverage
        if totalReadsCoverage>0:
            totalCoverageCount+=1
            if totalReadsCoverage>=25:
                percentOrder=weightedReadsNuc.getPercentOrder()
                nuc1=percentOrder[0][0]
                nuc2perc=percentOrder[1][1]
                if nuc2perc>.2:
                    nuc2=percentOrder[1][0]
                    nuc3perc=percentOrder[2][1]
                    if nuc3perc>.1:
                        nuc3=percentOrder[2][0]
                        nuc4perc=percentOrder[3][1]
                        if nuc4perc>.1:
                            newGenome+='N'
                            ambiquityTuples.append((i,totalReadsCoverage,percentOrder))
                        else:
                            nuc3=percentOrder[2][0]
                            finalNuc=getAmbiguousNuc(nuc1,nuc2,nuc3)
                            ambiquityTuples.append((i,totalReadsCoverage,percentOrder))
                    else:
                        finalNuc=getAmbiguousNuc(nuc1,nuc2)
                        ambiquityTuples.append((i,totalReadsCoverage,percentOrder))
                else:
                    finalNuc=nuc1
                if finalNuc!='-':
                    newGenome+=finalNuc
                if refGenome[i]!=finalNuc:
                    mismatchTuples.append((i,refGenome[i],finalNuc))
                else:
                    totalIdentityCount+=1
            else:
                lowCoverageCount+=1
                lowCoverageLocs.append(i)
                newGenome+=refGenome[i]
        else:
            lowCoverageCount+=1
            lowCoverageLocs.append(i)
            newGenome+=refGenome[i]
    
    
    
    highCoveragePercent=(1-(lowCoverageCount/len(refGenome)))*100
    totalCoveragePercent=(totalCoverageCount/len(refGenome))*100
    if totalCoveragePercent>=90 and highCoveragePercent>80:
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
                flag=False
            except:
                time.sleep(10)
        f.write('>Assembly_from_'+refID+': '+name+' : '+str(totalCoveragePercent)+'\n')
        f.write(newGenome+'\n')
        print('|')
        print(refID)
        print("ref length: "+str(len(refGenome)))
        print("Total ID count: "+str(totalIdentityCount))
        print("Total from ref: "+str(lowCoverageCount) + "   -(low coverage)")
        print("High coverage percent: "+str(highCoveragePercent))
        print("Total coverage percent: "+str(totalCoveragePercent))
        print("Average coverage per position: "+str(totalPositionMagnitude/len(refGenome)))
        print("Total mismatches: "+str(len(mismatchTuples)))
        print("Total mismatch percent: "+str(len(mismatchTuples)*100/len(refGenome)))
        print("Mismatch percent (assembly only): "+str(len(mismatchTuples)*100/(len(refGenome)-lowCoverageCount)))
        print("Total ambiguous nucleotides: "+str(len(ambiquityTuples)))

f.close()
