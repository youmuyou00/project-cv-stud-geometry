# project-cv-stud-geometry

- __python-nerf__: 
	- slightly modified code of nerf
	- train: *python run\_nerf.py --config path/to/config_file.txt*
	- render: *python run\_nerf.py --config path/to/config_file.txt --render_only*
	
- __cs-tools__:
	- C#-Tools for:
		- converting COLMAP results to the format nerf expects
		- randomly splitting images in train/validation/test
		- determining the scene bounds (t_near, t_far)


- __runnerf.py__:
Find the boundary in whole scene Lego: (lines 439-573) this process will takes very long times, so if not necessary, need not repeat again. The boundary's results save in ./xyz_minmax/xyz_minmax.txt.
- __extractDensity from single points__:
function extractDensity (lines 1185-1246)
- __extractDensity from all scene boundary__:
function get_density_al (lines 1249-1273)
output density points save as .csv in ./density_points
- __plot density point cloud__:
./density_map.py
