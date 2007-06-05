using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace ObjPointExtractor {
    class Program {
        private readonly static int reductionFactor = 8000;

        static void Main(string[] args) {
            StreamWriter outputStreamWriter = new StreamWriter("out.point", true);
            Regex regex = new Regex(@"\s+");
            int iPoint = 0;
            foreach (string inputFilename in args) {
                FileStream inputFileStream = new FileStream(inputFilename, FileMode.Open);
                int blockSize = 1024;
                byte[] block = new byte[blockSize];
                MemoryStream memStream = new MemoryStream(block);
                StreamReader reader = new StreamReader(memStream);
                StreamWriter writer = new StreamWriter(memStream);
             
                int readCount;
                int offset = 0;
                
                while (true) {
                    readCount = inputFileStream.Read(block, offset, blockSize - offset);
                    if (readCount == 0)
                        break;                     

                    string line = reader.ReadLine();                    

                    string nextLine = null;

                    while (true) {
                        nextLine = reader.ReadLine();
                        if (nextLine == null)
                            break; // line may be incomplete

                        string[] tokens = regex.Split(line);
                        if (tokens[0] == "v") {
                            if ((iPoint = (iPoint++ % reductionFactor)) == 0) {
                                outputStreamWriter.WriteLine(tokens[1] + " " + tokens[2] + " " + tokens[3]);
                            }
                        }
                        line = nextLine;
                    }

                    memStream.Seek(0, SeekOrigin.Begin);
                    writer.WriteLine(line);
                    writer.Flush();
                    memStream.Seek(0, SeekOrigin.Begin);

                    offset = line.Length;                    
                }
                memStream.Close();
                inputFileStream.Close();
	        }

            outputStreamWriter.Close();
        }
    }
}
