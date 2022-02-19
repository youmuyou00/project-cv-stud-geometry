using IPF.ProjectCV.IO;
using System.Globalization;

namespace IPF.ProjectCV {
    class Program {
        static void Main(string[] args) {

            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

            COLMAPConverter.Convert(
                0.2, // testFraction
                0.1, // validationFraction
                @"Y:\fuer_Dennis\Ettlingen\Internet\herzjesu-P25\colmap", // colmapResultsDirectory
                @"Y:\fuer_Dennis\Ettlingen\Internet\herzjesu-P25", // imageDirectory
                @"Y:\fuer_Dennis\Ettlingen\Internet\herzjesu-P25\nerf-input"); // destinationDirectory
        }
    }
}