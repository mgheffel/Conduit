
# coding: utf-8

# In[21]:


import os
from pandas import DataFrame
import pandas as pd
import sys




try:
    with open('raw_kraken_krona.html','r')as f:
        kronaInput=f.read()
except:
   with open(sys.argv[1],'r')as f:
        kronaInput=f.read()
dsStartIndex=kronaInput.index('<dataset>')
dsEndIndex=kronaInput.index('</datasets>',dsStartIndex)
datasetLines=kronaInput[dsStartIndex:dsEndIndex].split('\n')
#a dictionary containing datasetName and the corresponding taxTree
datasets=[]
for i in range(len(datasetLines)-1):
    line=datasetLines[i]
    dsNameStart=line.index('>')+1
    dsNameEnd=line.index('</')
    print("Adding " + line[dsNameStart:dsNameEnd] + " to datasets")
    datasets.append({'datasetName': line[dsNameStart:dsNameEnd], 'taxTree': {}})


# In[23]:


kronaLines=kronaInput[dsEndIndex:]
virusLines=kronaLines[kronaLines.find('<node name="Viruses"'):]



#---temp insertion for deadline
df=open(sys.argv[1][:-21]+'0_domain.txt','w')
kronaLines=kronaInput[dsEndIndex:]
coLines=kronaLines[kronaLines.find('<node name="cellular organisms"'):kronaLines.find('<node name="Viruses"')]
#pull select data
euStart=coLines.find('<node name="Eukaryota">')
euEnd=coLines.find('</magnitude>',euStart)
magLineSplit=coLines[coLines.find('<magnitude>',euStart):euEnd].split('<val>')
#print(magLineSplit)
for i in range(len(datasets)):
    try:
        mag=magLineSplit[i+1]
        magInt=int(mag[:mag.find('<')])
    except:
        magInt=0
    datasets[i]['eukaryoteMag']=magInt

bacStart=coLines.find('<node name="Bacteria">',euEnd)
bacEnd=coLines.find('</magnitude>',bacStart)
magLineSplit=coLines[coLines.find('<magnitude>',bacStart):bacEnd].split('<val>')
for i in range(len(datasets)):
    try:
        mag=magLineSplit[i+1]
        magInt=int(mag[:mag.find('<')])
    except:
        magInt=0
    datasets[i]['bacteriaMag']=magInt

dsRNAStart=kronaLines.find('<node name="dsRNA viruses">',bacEnd)
dsRNAEnd=kronaLines.find('</magnitude>',dsRNAStart)
magLineSplit=kronaLines[kronaLines.find('<magnitude>',dsRNAStart):dsRNAEnd].split('<val>')
#print(magLineSplit)
for i in range(len(datasets)):
    try:
        mag=magLineSplit[i+1]
        magInt=int(mag[:mag.find('<')])
    except:
        magInt=0
    datasets[i]['dsRNAMag']=magInt
    
for i in range(len(datasets)):
    df.write(datasets[i]['datasetName']+'\n')
    df.write('\tEukaryote Magnitude: '+str(datasets[i]['eukaryoteMag'])+'\n')
    df.write('\tBacteria Magnitude: '+str(datasets[i]['bacteriaMag'])+'\n')
    df.write('\tdsRNA Virus Magnitude: '+str(datasets[i]['dsRNAMag'])+'\n')
    df.write('\n')

df.close()
#----------------END temp insertion



#print(virusLines[:100])
tempDsDicts=[]
for i in range(len(datasets)):
    tempDsDicts.append(datasets[i]['taxTree'])
def buildTrees(dsDicts, level):
    global virusLines
    
    spaceBar=''
    for i in range(level):
        spaceBar+=' '
    
    nameStart=virusLines.find('<node name')
    nameEnd=virusLines.find('">')
    name=virusLines[nameStart+12:nameEnd]
    virusLines=virusLines[nameEnd:]
    
    magLineStart=virusLines.find('magnitude')
    magLineEnd=virusLines.find('</magnitude',magLineStart)
    magLineSplit=virusLines[magLineStart:magLineEnd].split('<val>')
    virusLines=virusLines[magLineEnd:]
    #print(magLineSplit)
    
    rankStart=virusLines.find('<rank')
    rankEnd=virusLines.find('</val',rankStart)
    rank=virusLines[rankStart+11:rankEnd]
    if rank=='no rank' and level==1:
        if name[:5]!='ssRNA':
            rank='group'
    if rank=='no rank' and name[:5]=='ssRNA':
        rank='group'
    virusLines=virusLines[rankEnd:]
    #print(rank)
    
    #print('Name: ' + name)
    for i in range(len(datasets)):
        try:
            mag=magLineSplit[i+1]
            magInt=int(mag[:mag.find('<')])
        except:
            magInt=0
        dsDicts[i][name]={'rank': rank, 'magnitude': magInt, 'tree': {}}
    
    
    nextLevelDicts=[]
    for i in range(len(dsDicts)):
        nextLevelDicts.append(dsDicts[i][name]['tree'])

    while True:
        #print(spaceBar +name + ' : ' + str(level))
        #print(spaceBar+virusLines[:100])
        nextNodeEnd=virusLines.find('/node')
        nextNodeStart=virusLines.find('<node')
        #print(spaceBar+'start: ' + str(nextNodeStart) + ' ||| ' + 'end: ' + str(nextNodeEnd))
        
        #print(spaceBar+virusLines[:100])
        
        if nextNodeEnd>nextNodeStart and nextNodeStart!=-1:
            virusLines=virusLines[nextNodeStart:]
            buildTrees(nextLevelDicts, level+1)
        else:
            virusLines=virusLines[nextNodeEnd+1:]
            break
        
        
        #print(spaceBar+virusLines[:100])
    #print('-')
    return
    #if nextNodeEnd<nextNodeStart or nextNodeStart==-1:
    #    return
    #else:
    #    for i in range(len(dsDicts)):
    #        dsDicts[i]=dsDicts[i][name]['tree']
    #    buildTrees(dsDicts, level+1)

buildTrees(tempDsDicts,0)


# In[24]:


def printTree(taxDict, node, level):
    #space for formatting
    spaceBar=''
    for i in range(level):
        spaceBar+=' '
    print(spaceBar + node + ' : '+taxDict['rank']+' : '+str(taxDict['magnitude']))
    #print(spaceBar+node)
    if len(taxDict.keys())==0:
        return
    for key in taxDict['tree'].keys():
        #print(key)
        printTree(taxDict['tree'][key], key, level+1)
    
#printTree(datasets[0]['taxTree']['Viruses'], 'Viruses', 0)


# In[25]:


finalDicts=[]
usefulDicts=[]
for i in range(len(datasets)):
    finalDicts.append({'datasetName': datasets[i]['datasetName'], 'taxTree': {'group':{},'order':{},'family':{},'genus':{},'species':{}}})
    usefulDicts.append(finalDicts[i]['taxTree'])



def extractUsefulTree(taxDict, node, level, useDict, group='',order='',family='',genus='',species=''):
    #space for formatting
    spaceBar=''
    for i in range(level):
        spaceBar+=' '
    rank=taxDict['rank']
    mag=taxDict['magnitude']
    if mag!=0:
        if rank=='group':
            group=node
            useDict['group'][node]=mag
        elif rank=='order':
            order=node
            useDict['order'][node]=mag
            if group=='':
                useDict['group'][node+' (from order)']=mag
        elif rank=='family':
            family=node
            useDict['family'][node]=taxDict['magnitude']
            if order=='':
                useDict['order'][node+' (from family)']=mag
                if group=='':
                    useDict['group'][node+' (from family)']=mag
        elif rank=='genus':
            genus=node
            useDict['genus'][node]=taxDict['magnitude']
            if family=='':
                useDict['family'][node+' (from genus)']=mag
                if order=='':
                    useDict['order'][node+' (from genus)']=mag
                    if group=='':
                        useDict['group'][node+' (from genus)']=mag
        elif rank=='species':
            species=node
            useDict['species'][node]=mag
            if genus=='':
                useDict['genus'][node+' (from species)']=mag
                if family=='':
                    useDict['family'][node+' (from species)']=mag
                    if order=='':
                        useDict['order'][node+' (from species)']=mag
                        if group=='':
                            useDict['group'][node+' (from species)']=mag
        
        
        
    #if rank=='group' or rank=='order' or rank=='family' or rank=='genus' or rank=='species':
        #print(spaceBar+group+':'+order+':'+family+':'+genus+':'+species)
        #print(spaceBar + node + ' : '+taxDict['rank']+' : '+str(taxDict['magnitude']))
    #print(spaceBar+node)
    if len(taxDict.keys())==0:
        return
    for key in taxDict['tree'].keys():
        #print(key)
        extractUsefulTree(taxDict['tree'][key], key, level+1,useDict,group,order,family,genus,species)

for i in range(len(datasets)):
    extractUsefulTree(datasets[i]['taxTree']['Viruses'], 'Viruses', 0, usefulDicts[i])


# In[26]:


#for key in usefulDicts[0].keys():
#    print(key)
#    for key2 in usefulDicts[0][key].keys():
#        print('\t'+key2 + ' : ' + str(usefulDicts[0][key][key2]))


# In[27]:


for key in finalDicts[0]['taxTree'].keys():
    print(key)


# In[28]:


taxLevel=1
for rank in finalDicts[0]['taxTree'].keys():
    writer = pd.ExcelWriter(sys.argv[1][:-21]+str(taxLevel) + '-' + rank + '.xlsx', engine='xlsxwriter')
    for ds in finalDicts:
        #sort keys by rank
        totalMags=0
        for key in ds['taxTree'][rank].keys():
            totalMags+=ds['taxTree'][rank][key]
        sortedKeys=[]
        sortedMags=[]
        sortedPercents=[]
        for elem in sorted(ds['taxTree'][rank].items(),reverse=True, key=lambda x: x[1]):
            if elem[1]==0:
                continue
            sortedKeys.append(elem[0])
            sortedMags.append(elem[1])
            sortedPercents.append(round((elem[1]*100/totalMags),4))
        count=0
        total=0
        for i in range(len(sortedKeys)):
            if total>99:
                break
            total+=sortedPercents[i]
            count=i
        #this line for 99%
        #df=DataFrame({rank: sortedKeys[:count], 'magnitude': sortedMags[:count], 'percent': sortedPercents[:count]})
        #this line for all
        df=DataFrame({rank: sortedKeys, 'magnitude': sortedMags, 'percent': sortedPercents})
        df.to_excel(writer, sheet_name=ds['datasetName'], index=False, columns=[rank,'magnitude','percent'])
    writer.save()
    taxLevel+=1

