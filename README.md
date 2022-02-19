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