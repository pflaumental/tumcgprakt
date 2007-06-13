using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PointReducer {
    class Program {
        private readonly static int reductionFactor = 10;

        static void Main(string[] args) {
            string inputFileName = args[0];
            string outputFileName = "out.point";
            StreamReader reader = new StreamReader(inputFileName);
            StreamWriter writer = new StreamWriter(outputFileName);
            int count = 0;
            while (!reader.EndOfStream) {
                string line = reader.ReadLine();
                if ((count++ % reductionFactor) == 0) {
                    count = 1;
                    writer.WriteLine(line);
                }
            }
        }
    }
}
