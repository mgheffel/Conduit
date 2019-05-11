
# coding: utf-8

# In[1]:


import sys
import os
import re
print(sys.argv[1]=='-f')
if '.fasta' in sys.argv[1]:
    referenceFile=sys.argv[1]
else:
    referenceFile='00265-06_RCSW_S3.fasta'
    print('default')
if '.sam' in sys.argv[2]:
    samFile=sys.argv[2]
else:
    samFile='toBlast_00265-06_RCSW_S3_R1_001.sam'
    print('default')


# In[2]:


for filename in os.listdir():
    print(filename)


# In[3]:




refDict={}
with open(referenceFile,'r') as f:
    line=f.readline()
    while line:
        if line[0]=='>':
            refID=line[1:-1]
        else:
            genome=line[:-1]
            refDict[refID]={'refSeq':genome, 'assemblyStruct':[{'A':0,'T':0,'G':0,'C':0,'N':0,'D':0} for i in range(len(genome))], 'insertionStruct':{}}
        line=f.readline()

for refID in refDict.keys():
    print(refID)
    print(refDict[refID]['refSeq'][:20])
    break
            


# In[ ]:


f=open(samFile,'r')
line = f.readline()
count=0
while line and count<1000000000:
    if count==11563110 or count==11563111:
        print(count)
        print('next')
        print(line)
        for i in range(10):
            print(str(i)+': '+str(refDict[refID]['assemblyStruct'][i]))
    if line[0]=="@":
        line=f.readline()
        continue
    
    else:
        lineInfo=line.split('\t')
        
        if count==11563110:
            print(count)
            print('mid')
            print(line)
            for i in range(10):
                print(str(i)+': '+str(refDict[refID]['assemblyStruct'][i]))
        
        
        if count==11563110:
            print(count)
            print('mid')
            print(line)
            for i in range(10):
                print(str(i)+': '+str(refDict[refID]['assemblyStruct'][i]))
        
        #refID goes here <-
        
        if count==11563110:
            print(count)
            print('mid')
            print(line)
            for i in range(10):
                print(str(i)+': '+str(refDict[refID]['assemblyStruct'][i]))
        
        #print(lineInfo)
        cigar=lineInfo[5]
        #dont include reads with * as cigar string
        if cigar=='*':
            line=f.readline()
            count+=1
            continue
        readID=lineInfo[0]
        
        if count==11563110:
            print(count)
            print('mid')
            print(line)
            for i in range(10):
                print(str(i)+': '+str(refDict[refID]['assemblyStruct'][i]))
        
        refID=lineInfo[2]
        if refID=='*':
            line = f.readline()
            count+=1
            continue
        #force small dataset
        if refID!='KM392232':
            line=f.readline()
            count+=1
            if count-1>11574298:
                print(line)
                print(count-1)
                for i in range(10):
                    print(str(i)+': '+str(refDict[refID]['assemblyStruct'][i]))
            continue
        #pos-1 to start indexing at 0 rather than 1
        pos=int(lineInfo[3])-1
        if pos>10:
            line=f.readline()
            count+=1
            continue
        readSeq=lineInfo[9]
        readQual=lineInfo[10]
        
    #add read to assembly
    
    #assemblyStruct=refDict[refID]['assemblyStruct']
    pattern='\d+[A-Z]+'
    m=re.findall(pattern,cigar)
    index=0
    cigarRead=''
    for item in m:
        num=int(item[:-1])
        mode=item[-1:]
        if mode=='M':
            for i in range(num):
                refDict[refID]['assemblyStruct'][pos][readSeq[0]]+=1
                #print(pos)
                #print(refDict[refID]['assemblyStruct'][pos])
                
                #raise Exception('I know Python!')
                readSeq=readSeq[1:]
                pos+=1
        elif mode=='I':
            insertionStruct=refDict[refID]['insertionStruct']
            for i in range(num):
                key=str(pos)+'-'+str(i)
                if key in insertionStruct.keys():
                    insertionStruct[key][readSeq[0]]+=1
                else:
                    insertionStruct[key]={'A':0,'T':0,'G':0,'C':0,'N':0}
                    insertionStruct[key][readSeq[0]]+=1
                readSeq=readSeq[1:]
        elif mode=='D':
            refDict[refID]['assemblyStruct'][pos]['D']+=1
        else:
            raise Exception('I know Python!')
    
    
    
    line = f.readline()
    count+=1
    highCount=count
    if count-1>11563006:
        print('Note')
        print(line)
        print(count-1)
        for i in range(10):
            print(str(i)+': '+str(refDict[refID]['assemblyStruct'][i]))
print('Total count: '+str(count))
print(highCount)
print()
for i in range(20):
    print(str(i)+': '+str(refDict[refID]['assemblyStruct'][i]))
    
print()
for struct in refDict['KM392232']['insertionStruct'].keys():
    total=0
    for nuc in refDict['KM392232']['insertionStruct'][struct].keys():
        total+=refDict['KM392232']['insertionStruct'][struct][nuc]
    if total>100:
        print(refDict['KM392232']['insertionStruct'][struct])
for i in range(20):
    print(str(i)+': '+str(refDict[refID]['assemblyStruct'][i]))
print(refDict.keys())

