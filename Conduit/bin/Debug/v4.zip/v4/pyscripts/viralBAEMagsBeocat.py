import sys
import os
from collections import OrderedDict
from pandas import DataFrame
import pandas as pd
#add usage
try:
	t1=sys.argv[1]
except:
	print("Usage: krakenMagsAtTaxLevel.py D180001745_S3_L001.translate")
	print('Tax levels:\n0: root\n1: organization type\n2: Domain\n3: Phylum\n4: Class\n5: Order\n6: Family\n7: Genus\n8: Species\n9: Strain\n10: Variant')
	sys.exit(0)
taxLevel=1

if not os.path.exists(t1+'/taxMags'):
    os.makedirs(t1+'/taxMags')

if not os.path.exists(t1+'/taxMags/viralBAE'):
    os.makedirs(t1+'/taxMags/viralBAE')

writer = pd.ExcelWriter(t1+'/taxMags/viralBAE/viralBAEMags.xlsx', engine='xlsxwriter')
for file in os.listdir(t1+'/kraken_translated'):
	if '.translate' in file or '.viralonly' in file:
		print(file)
	else: continue

	# with open(sys.argv[1]+'/'+file) as f:
		# lines=f.readlines()
	dataOrigin=[]
	dataKeys=[]
	data={}
	with open (t1+'/kraken_translated/'+file) as f:
		while True:
			line=f.readline()
			if not line: break
			item=line.split('\t')[1].strip()
			split=item.split(';')
			if len(split)>=taxLevel+1:
				#item=split[taxLevel].strip()
				if split[taxLevel]=='Viruses':
					item=split[taxLevel].strip()
				else:
					try:
						item=split[taxLevel+1].strip()
					except:
						item=split[taxLevel].strip()
			else:
				item=split[len(split)-1].strip()
			if item not in data:
				data[item]=1
				dataKeys.append(item)
				dataOrigin.append(line)
			else:
				data[item]+=1
	
	sortedKeys=sorted(dataKeys, reverse=True, key=data.__getitem__)
	sortedMags=[]
	for key in sortedKeys:
		#print('Key: ' + key + ' Magnitude: ' + str(data[key]))
		sortedMags.append(data[key])
	
	#load percent 
	sortedPercent=[]
	total=0
	for key in range(len(sortedKeys)):
		total+=sortedMags[key]
		sortedPercent.append(sortedMags[key])
	print(total)
	for key in range(len(sortedKeys)):
		sortedPercent[key]/=total*.01
		sortedPercent[key]=float(str(round(sortedPercent[key],2)))
	
	
	df=DataFrame({taxMags[i]: sortedKeys, 'Magnitude': sortedMags, 'Percent': sortedPercent})
	#df.to_excel(t1+'/taxMags/'+file.split('.')[0]+'.xlsx', sheet_name='sheet1', index=False)
	df.to_excel(writer, sheet_name=file.split('.')[0], index=False)

writer.save()
print('done')
# import numpy as np
# import matplotlib.pyplot as plt
# import pandas as pd
# import time
# import sys




# #TURN INTO HISTOGRAM AND TABLE
# objects=[]
# length=10
# if len(data)<10: length=len(data)
# yPos=np.arange(length)
# magnitudes=[]
# for i in range(length):
	# objects.append(sortedKeys[i])
	# magnitudes.append(data[sortedKeys[i]])

# plt.figure(1)
# plt.bar(yPos, magnitudes, align='center', alpha=.5)
# #plt.barh(yPos, magnitudes, align='center', alpha=.5)
# plt.xticks(yPos, objects, rotation='vertical')
# #plt.yticks(yPos, objects)
# plt.ylabel('Magnitude')
# plt.xlabel('Taxonomy')
# plt.title('Taxonomy Magnitudes')

# #annotate bars
# #for i,v in enumerate(magnitudes):
# #	ax.text(v+3,i+.25,str(v), color='black', fontweight='bold')


# try:
	# plt.tight_layout()
# except:
	# pass

# plt.show()

#ADD TABLE OR LIST OF ALL MAGNITUDES

# #create table
# plt.figure(2)
# fig,ax=plt.subplots()
# #hide axes
# fig.patch.set_visible(False)
# ax.axis('off')
# ax.axis('tight')

# df=pd.DataFrame([taxNames,taxMags])
# columns=['Taxonomy', 'Magnitude']
# ax.table(cellText=df.T.values, colLabels=df.columns, loc='center')
# fig.tight_layout()

# fig.show()
# plt.show()
