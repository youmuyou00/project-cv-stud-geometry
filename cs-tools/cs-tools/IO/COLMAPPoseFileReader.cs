using IPF.ProjectCV.Util;
using OpenTK.Mathematics;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IPF.ProjectCV.IO {
    public static class COLMAPPoseFileReader {
        public static List<(string, Pose)> ReadNamedPoses(
                string file) {

            int i = 0;
            string[] values;
            List<(string, Pose)> namedPoses = new List<(string, Pose)>();

            foreach (string line in File.ReadLines(file)) {

                if (line.StartsWith("#")) {
                    continue;
                }

                i++;
                if (i % 2 == 0) {
                    continue;
                }

                values = line.Split(' ');

                namedPoses.Add((
                    Enumerable
                        .Range(9, values.Length - 9)
                        .Select(index => values[index])
                        .Join(" "),
                    new Pose() { 
                        Position = new Vector3d(
                            double.Parse(values[5]),
                            double.Parse(values[6]),
                            double.Parse(values[7])),
                        Quaternion = new Quaterniond(
                            double.Parse(values[2]),
                            double.Parse(values[3]),
                            double.Parse(values[4]),
                            double.Parse(values[1]))
                    }));
            }

            return namedPoses;
        }
    }
}