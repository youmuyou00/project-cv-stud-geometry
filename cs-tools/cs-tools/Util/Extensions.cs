using OpenCvSharp;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace IPF.ProjectCV.Util {
    public static class Extensions {
        public static double Floor(this double value) {
            return System.Math.Floor(value);
        }

        public static double Squared(this double value) {
            return value * value;
        }

        public static double Sqrt(this double value) {
            return System.Math.Sqrt(value);
        }

        public static double Sin(this double value) {
            return System.Math.Sin(value);
        }

        public static double Cos(this double value) {
            return System.Math.Cos(value);
        }

        public static double DegreeToRadian(this double degree) {
            return degree * System.Math.PI / 180.0;
        }

        public static double RadianToDegree(this double radian) {
            return radian * 180.0 / System.Math.PI;
        }

        public static double[] ToArray(this Vector3d vector) {
            return new double[] {
                vector.X, vector.Y, vector.Z
            };
        }

        public static double DistanceTo(
                this Vector3d v1,
                Vector3d v2) {
            return (v2 - v1).Length;
        }

        public static double[,] To2DArray(this Matrix3d matrix) {
            return new double[,] {
                { matrix[0, 0], matrix[0, 1], matrix[0, 2] },
                { matrix[1, 0], matrix[1, 1], matrix[1, 2] },
                { matrix[2, 0], matrix[2, 1], matrix[2, 2] }
            };
        }

        public static Matrix3d Clone(this Matrix3d matrix) {
            return new Matrix3d(
                matrix[0, 0], matrix[0, 1], matrix[0, 2],
                matrix[1, 0], matrix[1, 1], matrix[1, 2],
                matrix[2, 0], matrix[2, 1], matrix[2, 2]);
        }

        public static Matrix3d ToRotationMatrix(
                this Quaterniond quaternion) {

            quaternion = quaternion.Normalized();

            return new Matrix3d(
                1 - 2 * quaternion.Y.Squared() - 2 * quaternion.Z.Squared(),
                2 * quaternion.X * quaternion.Y - 2 * quaternion.Z * quaternion.W,
                2 * quaternion.X * quaternion.Z + 2 * quaternion.Y * quaternion.W,
                2 * quaternion.X * quaternion.Y + 2 * quaternion.Z * quaternion.W,
                1 - 2 * quaternion.X.Squared() - 2 * quaternion.Z.Squared(),
                2 * quaternion.Y * quaternion.Z - 2 * quaternion.X * quaternion.W,
                2 * quaternion.X * quaternion.Z - 2 * quaternion.Y * quaternion.W,
                2 * quaternion.Y * quaternion.Z + 2 * quaternion.X * quaternion.W,
                1 - 2 * quaternion.X.Squared() - 2 * quaternion.Y.Squared());
        }

        public static Vector3d RotationMatrixToRodriguesElements(
                this Matrix3d rotationMatrix) {

            Cv2.Rodrigues(
                rotationMatrix.To2DArray(),
                out double[] rodriguesElements,
                out _);

            return new Vector3d(
                rodriguesElements[0],
                rodriguesElements[1],
                rodriguesElements[2]);
        }

        public static Matrix3d RodriguesElementsToRotationMatrix(
                this Vector3d rodriguesElements) {

            Cv2.Rodrigues(
                rodriguesElements.ToArray(),
                out double[,] orientation,
                out _);

            return new Matrix3d(
                orientation[0, 0], orientation[0, 1], orientation[0, 2],
                orientation[1, 0], orientation[1, 1], orientation[1, 2],
                orientation[2, 0], orientation[2, 1], orientation[2, 2]);
        }

        public static Vector3d RotationMatrixToEulerAngles(
                this Matrix3d rotationMatrix) {

            return new Vector3d(
                System.Math.Atan2(
                    rotationMatrix[2, 1],
                    rotationMatrix[2, 2]),
                System.Math.Atan2(
                    -rotationMatrix[2, 0],
                    (rotationMatrix[2, 1].Squared() + rotationMatrix[2, 2].Squared()).Sqrt()),
                System.Math.Atan2(
                    rotationMatrix[1, 0],
                    rotationMatrix[0, 0]));
        }

        public static Matrix3d EulerAnglesToRotationMatrix(
                this Vector3d eulerAngles) {

            double cPitch = eulerAngles.X.Cos();
            double sPitch = eulerAngles.X.Sin();
            double cYaw = eulerAngles.Y.Cos();
            double sYaw = eulerAngles.Y.Sin();
            double cRoll = eulerAngles.Z.Cos();
            double sRoll = eulerAngles.Z.Sin();

            return new Matrix3d(
                cYaw * cRoll,
                    -cYaw * sRoll * cPitch + sYaw * sPitch,
                    cYaw * sRoll * sPitch + sYaw * cPitch,
                sRoll,
                    cRoll * cPitch,
                    -cRoll * sPitch,
                -sYaw * cRoll,
                    sYaw * sRoll * cPitch + cYaw * sPitch,
                    -sYaw * sRoll * sPitch + cYaw * cPitch);
        }

        public static string Join(
                this IEnumerable<string> strings,
                string seperator = ", ") {

            return string.Join(seperator, strings);
        }
    }
}