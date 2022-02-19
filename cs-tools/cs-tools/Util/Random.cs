using System;

namespace IPF.ProjectCV.Util {
    public static class Random {
        private static readonly object LOCK = new object();

        [ThreadStatic]
        private static System.Random generator;
        private static System.Random globalGenerator = new System.Random(DateTime.Now.Millisecond);

        public static int GetInteger(
                int min, 
                int max) {

            return GetGenerator()
                .Next(
                    min, 
                    max + 1);
        }

        private static System.Random GetGenerator() {

            if (generator == null) {
                lock (LOCK) {
                    generator = new System.Random(
                        globalGenerator.Next());
                }
            }

            return generator;
        }
    }
}
