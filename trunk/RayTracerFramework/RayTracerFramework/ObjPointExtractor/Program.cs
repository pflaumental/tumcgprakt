using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace ObjPointExtractor {
    class Program {
        private readonly static int reductionFactor = 1000;

        static void Main(string[] args) {
            //StreamWriter outputStreamWriter = new StreamWriter("out.point", true);
            FileStream outputFileStream = new FileStream("out.point" , FileMode.Create);
            Regex regex = new Regex(@"\s+");
            int iPoint = 0, readBlockCount = 0;
            foreach (string inputFilename in args) {
                FileStream inputFileStream = new FileStream(inputFilename, FileMode.Open);
                Console.WriteLine("Processing " + inputFilename);
                int blockSize = 1024 * 1024 * 100;
                byte[] inputBlock = new byte[blockSize];
                byte[] outputBlock = new byte[blockSize];
                MemoryStream inputMemStream = new MemoryStream(inputBlock);
                MemoryStream outputMemStream = new MemoryStream(outputBlock);
                StreamReader inputReader = new StreamReader(inputMemStream);
                StreamWriter inputWriter = new StreamWriter(inputMemStream);
                StreamWriter outputWriter = new StreamWriter(outputMemStream);
             
                int readCount, writeCount;
                int offset = 0;
                
                while (true) {
                    readCount = inputFileStream.Read(inputBlock, offset, blockSize - offset);
                    readBlockCount++;
                    if (readCount == 0)
                        break;                     

                    writeCount = 0;

                    string line = inputReader.ReadLine();                    

                    string nextLine = null;

                    while (true) {
                        nextLine = inputReader.ReadLine();
                        if (nextLine == null)
                            break; // line may be incomplete

                        string[] tokens = regex.Split(line);
                        if (tokens[0] == "v") {
                            if ((iPoint++ % reductionFactor) == 0) {
                                iPoint = 1;
                                string outputLine = tokens[1] + " " + tokens[2] + " " + tokens[3] + "\n";
                                outputWriter.Write(outputLine);
                                writeCount += outputLine.Length;
                            }
                        }
                        line = nextLine;
                    }                    
                    inputMemStream.Seek(0, SeekOrigin.Begin);
                    inputWriter.WriteLine(line);
                    inputWriter.Flush();
                    inputMemStream.Seek(0, SeekOrigin.Begin);
                    offset = line.Length;

                    outputWriter.Flush();
                    outputFileStream.Write(outputBlock, 0, writeCount); 
                    outputMemStream.Seek(0, SeekOrigin.Begin);

                    Console.WriteLine("Read block " + readBlockCount + " (blocksize " + readCount + " )");
                }
                inputMemStream.Close();
                outputMemStream.Close();
                inputFileStream.Close();
	        }

            outputFileStream.Close();
        }
    }
}
