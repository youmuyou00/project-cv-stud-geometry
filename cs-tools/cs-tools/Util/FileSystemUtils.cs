using System.IO;

namespace IPF.ProjectCV.Util {
    public static class FileSystemUtils {
        public static void CleanDirectory(string directory) {

            DirectoryInfo directoryInfo = new DirectoryInfo(directory);

            foreach (FileInfo file in directoryInfo.GetFiles()) {
                file.Delete();
            }
            foreach (DirectoryInfo subDirectory in directoryInfo.GetDirectories()) {
                subDirectory.Delete(true);
            }
        }
    }
}
