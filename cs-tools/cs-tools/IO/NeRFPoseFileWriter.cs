using IPF.ProjectCV.Util;
using System;
using System.IO;

namespace IPF.ProjectCV.IO {
    public class NeRFPoseFileWriter : IDisposable {
        private StreamWriter writer;

        public NeRFPoseFileWriter(
                double focalLength,
                double principalPointX,
                double principalPointY,
                double nearBound,
                double farBound,
                string file) {

            writer = new StreamWriter(file);
            writer.WriteLine("{");
            writer.WriteLine("    \"camera_parameters\": {");
            writer.WriteLine($"      \"focal_length\": {focalLength},");
            writer.WriteLine($"      \"principal_point_x\": {principalPointX},");
            writer.WriteLine($"      \"principal_point_y\": {principalPointY}");
            writer.WriteLine("    },");
            writer.WriteLine("    \"scene_bounds\": {");
            writer.WriteLine($"      \"near_bound\": {nearBound},");
            writer.WriteLine($"      \"far_bound\": {farBound}");
            writer.WriteLine("    },");
            writer.WriteLine("    \"frames\": [");
        }

        public void Write(
                bool isFirstPose,
                string imageFilePath,
                Pose pose) {

            if (!isFirstPose) {
                writer.Write($",{Environment.NewLine}");
            }
            writer.WriteLine("        {");
            writer.WriteLine($"            \"file_path\": \"{imageFilePath}\",");
            writer.WriteLine("             \"transform_matrix\": [");
            writer.WriteLine("                [");
            writer.WriteLine($"                    {pose.Rxx},");
            writer.WriteLine($"                    {pose.Rxy},");
            writer.WriteLine($"                    {pose.Rxz},");
            writer.WriteLine($"                    {pose.X}");
            writer.WriteLine("                ],");
            writer.WriteLine("                [");
            writer.WriteLine($"                    {pose.Ryx},");
            writer.WriteLine($"                    {pose.Ryy},");
            writer.WriteLine($"                    {pose.Ryz},");
            writer.WriteLine($"                    {pose.Y}");
            writer.WriteLine("                ],");
            writer.WriteLine("                [");
            writer.WriteLine($"                    {pose.Rzx},");
            writer.WriteLine($"                    {pose.Rzy},");
            writer.WriteLine($"                    {pose.Rzz},");
            writer.WriteLine($"                    {pose.Z}");
            writer.WriteLine("                ],");
            writer.WriteLine("                [");
            writer.WriteLine("                    0.0,");
            writer.WriteLine("                    0.0,");
            writer.WriteLine("                    0.0,");
            writer.WriteLine("                    1.0");
            writer.WriteLine("                ]");
            writer.WriteLine("            ]");
            writer.Write("        }");
        }

        public void Dispose() {

            writer.Write(Environment.NewLine);
            writer.WriteLine("    ]");
            writer.WriteLine("}");
            writer.Flush();
            writer.Close();
            writer.Dispose();
        }
    }
}
