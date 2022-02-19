from mpl_toolkits.mplot3d import Axes3D
import matplotlib.pyplot as plt
import pandas
'''
show all 4d information x,y,z,density as point cloud.
'''
# points = pandas.read_csv('densitymap_20w.csv')
points = pandas.read_csv('./density_point/17.csv')
fig = plt.figure()
ax = fig.add_subplot(111, projection='3d')

x = points['x'].values
y = points['y'].values
z = points['z'].values
d = points['d'].values

# ax.scatter(x, y, z, c=d, cmap='YlOrRd', marker='o')
cb=ax.scatter(x, y, z, c=d, cmap='Accent', marker='o')
# plt.title("Density greater than 0 of Lego")
ax.set_xlabel("x")
ax.set_ylabel("y")
ax.set_zlabel("z")
cb = plt.colorbar(cb)
cb.set_label('density')

'''
show one ray Z
'''

points = pandas.read_csv('./ray_fine_position_27421.csv')



x2 = points['x'].values
y2 = points['y'].values
z2 = points['z'].values

# ax.scatter(x, y, z, c=d, cmap='YlOrRd', marker='o')
ax.scatter(x2, y2, z2)

plt.show()