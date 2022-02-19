using IPF.ProjectCV.Util;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Random = IPF.ProjectCV.Util.Random;

namespace IPF.ProjectCV.IO {
    public static class COLMAPConverter {
        public static void Convert(
                double testFraction,
                double validationFraction,
                string colmapResultsDirectory,
                string imageDirectory,
                string destinationDirectory) {

            double focalLength;
            double principalPointX;
            double principalPointY;
            double nearBound;
            double farBound;
            HashSet<int> testImageIndices;
            HashSet<int> validationImageIndices;
            List<(string, Pose)> namedPoses;

            if (!Directory.Exists(destinationDirectory)) {
                Directory.CreateDirectory(destinationDirectory);
            }
            FileSystemUtils.CleanDirectory(destinationDirectory);

            ReadCameraParameters(
                $"{colmapResultsDirectory}/cameras.txt",
                out focalLength,
                out principalPointX,
                out principalPointY);

            DetermineSceneBounds(
                $"{colmapResultsDirectory}/points3D.txt",
                $"{colmapResultsDirectory}/images.txt",
                out nearBound,
                out farBound);

            namedPoses = COLMAPPoseFileReader.ReadNamedPoses(
                $"{colmapResultsDirectory}/images.txt");

            namedPoses.Split(
                testFraction,
                validationFraction,
                out testImageIndices,
                out validationImageIndices);

            Write(
                focalLength,
                principalPointX,
                principalPointY,
                nearBound,
                farBound,
                imageDirectory,
                destinationDirectory,
                "test",
                testImageIndices,
                namedPoses);

            Write(
                focalLength,
                principalPointX,
                principalPointY,
                nearBound,
                farBound,
                imageDirectory,
                destinationDirectory,
                "val",
                validationImageIndices,
                namedPoses);

            Write(
                focalLength,
                principalPointX,
                principalPointY,
                nearBound,
                farBound,
                imageDirectory,
                destinationDirectory,
                "train",
                Enumerable
                    .Range(0, namedPoses.Count)
                    .Where(index => !testImageIndices.Contains(index) 
                        && !validationImageIndices.Contains(index)),
                namedPoses);
        }

        private static void Split(
                this List<(string, Pose)> namedPoses,
                double testFraction,
                double validationFraction,
                out HashSet<int> testImageIndices,
                out HashSet<int> validationImageIndices) {

            int imageIndex;

            testImageIndices = new HashSet<int>();
            validationImageIndices = new HashSet<int>();

            while (testImageIndices.Count < (testFraction * namedPoses.Count).Floor()) {
                testImageIndices.Add(
                    Random.GetInteger(0, namedPoses.Count - 1));
            }

            while (validationImageIndices.Count < (validationFraction * namedPoses.Count).Floor()) {
                imageIndex = Random.GetInteger(0, namedPoses.Count - 1);
                if (!testImageIndices.Contains(imageIndex)) {
                    validationImageIndices.Add(imageIndex);
                }
            }
        }

        private static void ReadCameraParameters(
                string cameraFile,
                out double focalLength,
                out double principalPointX,
                out double principalPointY) {

            string[] lines;
            string[] values;

            lines = File
                .ReadLines(cameraFile)
                .Where(line => !line.StartsWith("#"))
                .Where(line => line.Length != 0)
                .ToArray();

            if (lines.Length > 1) {
                throw new ApplicationException(
                    "Currently, we expect to have exactly on camera for the whole scene.");
            }

            values = lines[0].Split(' ');

            if (values[1] != "SIMPLE_RADIAL") {
                throw new ApplicationException(
                    "Currently, we only support the SIMPLE_RADIAL format.");
            }

            focalLength = double.Parse(values[4]);
            principalPointX = double.Parse(values[5]);
            principalPointY = double.Parse(values[6]);
        }

        private static void DetermineSceneBounds(
                string pointsFile,
                string imagesFile,
                out double nearBound,
                out double farBound) {

            int i = 0;
            int j;
            int pointId;
            double distance;
            string[] values;
            Vector3d cameraPosition = new Vector3d();
            Dictionary<int, Vector3d> points = ReadPoints(pointsFile);

            nearBound = double.MaxValue;
            farBound = double.MinValue;

            foreach (string line in File.ReadLines(imagesFile)) {

                if (line.StartsWith("#")) {
                    continue;
                }

                i++;
                values = line.Split(' ');

                if (i % 2 == 1) {
                    cameraPosition = new Vector3d(
                        double.Parse(values[5]),
                        double.Parse(values[6]),
                        double.Parse(values[7]));
                    continue;
                }

                for (j = 2; j < values.Length; j += 3) {
                    
                    pointId = int.Parse(values[j]);
                    if (pointId == -1) {
                        continue;
                    }

                    distance = points[pointId].DistanceTo(cameraPosition);
                    if (distance < nearBound) {
                        nearBound = distance;
                    }
                    if (distance > farBound) {
                        farBound = distance;
                    }
                }
            }
        }

        private static Dictionary<int, Vector3d> ReadPoints(
                string pointsFile) {

            string[] values;
            Dictionary<int, Vector3d> points = new Dictionary<int, Vector3d>();

            foreach (string line in File.ReadLines(pointsFile)) {

                if (line.StartsWith("#")) {
                    continue;
                }

                values = line.Split(' ');
                if (values.Length == 0) {
                    continue;
                }

                points.Add(
                    int.Parse(values[0]),
                    new Vector3d(
                        double.Parse(values[1]),
                        double.Parse(values[2]),
                        double.Parse(values[3])));
            }

            return points;
        }

        private static void Write(
                double focalLength,
                double principalPointX,
                double principalPointY,
                double nearBound,
                double farBound,
                string imageDirectory,
                string destinationDirectory,
                string destinationSubDirectoryName,
                IEnumerable<int> imageIndices,
                List<(string, Pose)> namedPoses) {

            bool first = true;
            string destinationSubDirectory = $"{destinationDirectory}/{destinationSubDirectoryName}";

            Directory.CreateDirectory(destinationSubDirectory);

            using (NeRFPoseFileWriter writer = new NeRFPoseFileWriter(
                    focalLength,
                    principalPointX,
                    principalPointY,
                    nearBound,
                    farBound,
                    $"{destinationDirectory}/transforms_{destinationSubDirectoryName}.json")) {

                foreach (int index in imageIndices) {

                    File.Copy(
                        $"{imageDirectory}/{namedPoses[index].Item1}",
                        $"{destinationSubDirectory}/{namedPoses[index].Item1}");

                    writer.Write(
                        first,
                        $"./{destinationSubDirectoryName}/{namedPoses[index].Item1}",
                        namedPoses[index].Item2);

                    first = false;
                }
            }
        }
    }
}
