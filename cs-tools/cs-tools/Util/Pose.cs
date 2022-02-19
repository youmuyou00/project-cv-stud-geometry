using OpenTK.Mathematics;
using System;

namespace IPF.ProjectCV.Util {
    public class Pose {

        public static Pose Identity {
            get {
                return new Pose();
            }
        }

        public static Pose operator +(Pose left, Pose right) {
            return new Pose(left.matrix + right.matrix);
        }

        public static Pose operator -(Pose left, Pose right) {
            return new Pose(left.matrix - right.matrix);
        }

        public static Pose operator *(Pose left, Pose right) {
            return new Pose(left.matrix * right.matrix);
        }

        public static Pose operator *(Pose left, Matrix4d right) {
            return new Pose(left.matrix * right);
        }

        public static Pose operator *(Pose left, float right) {
            return new Pose(left.matrix * right);
        }

        public static Vector3 operator *(Pose left, Vector3 right) {
            return new Vector3(
                (float)(left.Rxx * right.X + left.Rxy * right.Y + left.Rxz * right.Z + left.X),
                (float)(left.Ryx * right.X + left.Ryy * right.Y + left.Ryz * right.Z + left.Y),
                (float)(left.Rzx * right.X + left.Rzy * right.Y + left.Rzz * right.Z + left.Z));
        }

        public static Vector3d operator *(Pose left, Vector3d right) {
            return new Vector3d(
                left.Rxx * right.X + left.Rxy * right.Y + left.Rxz * right.Z + left.X,
                left.Ryx * right.X + left.Ryy * right.Y + left.Ryz * right.Z + left.Y,
                left.Rzx * right.X + left.Rzy * right.Y + left.Rzz * right.Z + left.Z);
        }

        private Matrix4d matrix;

        public Vector3d Position {
            get {
                return new Vector3d(X, Y, Z);
            }
            set {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }

        public double X {
            get { return matrix[0, 3]; }
            set { matrix[0, 3] = value; }
        }

        public double Y {
            get { return matrix[1, 3]; }
            set { matrix[1, 3] = value; }
        }

        public double Z {
            get { return matrix[2, 3]; }
            set { matrix[2, 3] = value; }
        }

        public Matrix3d OrientationMatrix {
            get {
                return new Matrix3d(
                    Rxx, Rxy, Rxz,
                    Ryx, Ryy, Ryz,
                    Rzx, Rzy, Rzz);
            }
            set {
                Rxx = value[0, 0];
                Rxy = value[0, 1];
                Rxz = value[0, 2];
                Ryx = value[1, 0];
                Ryy = value[1, 1];
                Ryz = value[1, 2];
                Rzx = value[2, 0];
                Rzy = value[2, 1];
                Rzz = value[2, 2];
            }
        }

        public Vector3d RodriguesElements {
            get {
                return OrientationMatrix.RotationMatrixToRodriguesElements();
            }
            set {
                OrientationMatrix = value.RodriguesElementsToRotationMatrix();
            }
        }

        public Vector3d EulerAngles {
            get {
                return OrientationMatrix.RotationMatrixToEulerAngles();
            }
            set {
                OrientationMatrix = value.EulerAnglesToRotationMatrix();
            }
        }

        public Quaterniond Quaternion {
            get {
                return Quaterniond.FromMatrix(OrientationMatrix);
            }
            set {
                OrientationMatrix = value.ToRotationMatrix();
            }
        }

        public double Rxx {
            get { return matrix[0, 0]; }
            private set { matrix[0, 0] = value; }
        }

        public double Rxy {
            get { return matrix[0, 1]; }
            private set { matrix[0, 1] = value; }
        }

        public double Rxz {
            get { return matrix[0, 2]; }
            private set { matrix[0, 2] = value; }
        }

        public double Ryx {
            get { return matrix[1, 0]; }
            private set { matrix[1, 0] = value; }
        }

        public double Ryy {
            get { return matrix[1, 1]; }
            private set { matrix[1, 1] = value; }
        }

        public double Ryz {
            get { return matrix[1, 2]; }
            private set { matrix[1, 2] = value; }
        }

        public double Rzx {
            get { return matrix[2, 0]; }
            private set { matrix[2, 0] = value; }
        }

        public double Rzy {
            get { return matrix[2, 1]; }
            private set { matrix[2, 1] = value; }
        }

        public double Rzz {
            get { return matrix[2, 2]; }
            private set { matrix[2, 2] = value; }
        }

        public Pose() :
            this(Matrix4d.Identity) { }

        public Pose(
            double m00, double m01, double m02, double m03,
            double m10, double m11, double m12, double m13,
            double m20, double m21, double m22, double m23) :
                this(new Matrix4d(
                    m00, m01, m02, m03,
                    m10, m11, m12, m13,
                    m20, m21, m22, m23,
                    0, 0, 0, 1)) { }

        private Pose(
                Matrix4d matrix) {
            this.matrix = matrix;
        }
        public override string ToString() {
            return
                $"{matrix[0, 0]} {matrix[0, 1]} {matrix[0, 2]} {matrix[0, 3]}{Environment.NewLine}" +
                $"{matrix[1, 0]} {matrix[1, 1]} {matrix[1, 2]} {matrix[1, 3]}{Environment.NewLine}" +
                $"{matrix[2, 0]} {matrix[2, 1]} {matrix[2, 2]} {matrix[2, 3]}{Environment.NewLine}" +
                $"{matrix[3, 0]} {matrix[3, 1]} {matrix[3, 2]} {matrix[3, 3]}";
        }

        public Vector3d GetXAxisDirection() {
            return ((this * new Vector3d(1, 0, 0)) - Position).Normalized();
        }

        public Vector3d GetYAxisDirection() {
            return ((this * new Vector3d(0, 1, 0)) - Position).Normalized();
        }

        public Vector3d GetZAxisDirection() {
            return ((this * new Vector3d(0, 0, 1)) - Position).Normalized();
        }

        public void RotateX(double angle) {
            matrix = Matrix4d.CreateRotationX(angle) * matrix;
        }

        public void RotateY(double angle) {
            matrix = Matrix4d.CreateRotationY(angle) * matrix;
        }

        public void RotateZ(double angle) {
            matrix = Matrix4d.CreateRotationZ(angle) * matrix;
        }

        public void LookAt(Vector3d target, Vector3d up) {
            matrix = Matrix4d.LookAt(Position, target, up);
        }

        public Pose RotatedX(double angle) {
            Pose pose = new Pose(matrix);
            pose.RotateX(angle);
            return pose;
        }

        public Pose RotatedY(double angle) {
            Pose pose = new Pose(matrix);
            pose.RotateY(angle);
            return pose;
        }

        public Pose RotatedZ(double angle) {
            Pose pose = new Pose(matrix);
            pose.RotateZ(angle);
            return pose;
        }

        public Pose LookingAt(Vector3d target, Vector3d up) {
            Pose pose = new Pose(matrix);
            pose.LookAt(target, up);
            return pose;
        }

        public Vector3d DirectionTo(Pose pose) {
            return DirectionTo(pose.Position);
        }

        public Vector3d DirectionTo(Vector3d point) {
            return (point - Position).Normalized();
        }

        public double DistanceTo(Pose pose) {
            return DistanceTo(pose.Position);
        }

        public double DistanceTo(Vector3d point) {
            return (point - Position).Length;
        }

        public void Invert() {
            matrix.Invert();
        }

        public Pose Inverted() {
            return new Pose(matrix.Inverted());
        }

        public void InvertXAxis() {
            X = -X;
            Quaterniond quaternion = Quaternion;
            Quaternion = new Quaterniond(
                -quaternion.X,
                quaternion.Y,
                quaternion.Z,
                -quaternion.W);
        }

        public void InvertYAxis() {
            Y = -Y;
            Quaterniond quaternion = Quaternion;
            Quaternion = new Quaterniond(
                quaternion.X,
                -quaternion.Y,
                quaternion.Z,
                -quaternion.W);
        }

        public void InvertZAxis() {
            Z = -Z;
            Quaterniond quaternion = Quaternion;
            Quaternion = new Quaterniond(
                quaternion.X,
                quaternion.Y,
                -quaternion.Z,
                -quaternion.W);
        }
    }
}