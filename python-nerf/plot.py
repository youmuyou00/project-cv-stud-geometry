from pandas import *
import matplotlib.pyplot as plt
import numpy as np

font = {'family' : 'normal',
        'size'   : 22}

plt.rc('font', **font)

# reading CSV file
data = read_csv('./fine_densitymap.csv')
d = data["d"].tolist()

# plt.title("NeRF 4k 3D Points")
# plt.hist(d, bins=7, range=[-14, 0])
# plt.xlabel("density value")
# plt.xlim((-14,0))
# plt.ylim((0,1200))
# plt.ylabel("number of points")
# plt.show()

# plt.title("NeRF 4k 3D Points")
# plt.hist(d, bins=5, range=[0, 250])
# plt.xlabel("density value")
# plt.xlim((0,250))
# plt.ylim((0,45))
# plt.ylabel("number of points")
# plt.show()

count = []
for i in range(len(d)):
        count.append(d.count(d[i]))
plt.title("Density in fine network")
plt.bar(d, count, edgecolor = 'black')
plt.xlabel("density value")
plt.ylabel("number of points")
plt.show()