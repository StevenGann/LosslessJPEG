using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosslessJPEG
{
    public class JpegCodec
    {
        //=====ENCODE================================================================================
        public static void encode(int[,] inputOriginal, int[,] inputPredictor, int method)
        {
            inputPredictor[0,0] = inputOriginal[0,0];
            for (int i = 1; i <= 15; i++)
            {
                inputPredictor[0,i] = inputOriginal[0,i] - inputOriginal[0,i-1];
                inputPredictor[i,0] = inputOriginal[i,0] - inputOriginal[i-1,0];
            }

            for (int i = 1; i <= 15; i++)
            {
                for (int j = 1; j <= 15; j++)
                {
                    switch (method)
                    {
                        case 1:
                            inputPredictor[j,i] = inputOriginal[j,i] - inputOriginal[j,i - 1];
                            break;
                        case 2:
                            inputPredictor[j,i] = inputOriginal[j,i] - inputOriginal[j - 1,i];
                            break;
                        case 3:
                            inputPredictor[j,i] = inputOriginal[j,i] - inputOriginal[j - 1,i - 1];
                            break;
                        case 4:
                            inputPredictor[j,i] = inputOriginal[j,i] - inputOriginal[j,i - 1] - inputOriginal[j - 1,i] + inputOriginal[j - 1,i - 1];
                            break;
                        case 5:
                            inputPredictor[j,i] = inputOriginal[j,i] - (inputOriginal[j,i - 1] + (inputOriginal[j - 1,i] - inputOriginal[j - 1,i - 1]) / 2);
                            break;
                        case 6:
                            inputPredictor[j,i] = inputOriginal[j,i] - (inputOriginal[j - 1,i] + (inputOriginal[j,i - 1] - inputOriginal[j - 1,i - 1]) / 2);
                            break;
                        case 7:
                            inputPredictor[j,i] = inputOriginal[j,i] - (inputOriginal[j,i - 1] + inputOriginal[j - 1,i]) / 2;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        //===========================================================================================

        //=====DECODE================================================================================
        public static void decoder(int[,] inputDecompress, int[,] inputDecode, int method)
        {
            //inputDecompress[0,0] = inputDecode[0,0];
            for (int i = 1; i <= 15; i++)
            {
                inputDecompress[0,i] = inputDecode[0,i] + inputDecompress[0,i - 1];
                inputDecompress[i,0] = inputDecode[i,0] + inputDecompress[i - 1,0];
            }

            for (int x = 1; x <= 15; x++)
            {
                for (int y = 1; y <= 15; y++)
                {
                    switch (method)
                    {
                        case 1:
                            inputDecompress[y,x] = inputDecode[y,x] + inputDecompress[y,x - 1];
                            break;
                        case 2:
                            inputDecompress[y,x] = inputDecode[y,x] + inputDecompress[y - 1,x];
                            break;
                        case 3:
                            inputDecompress[y,x] = inputDecode[y,x] + inputDecompress[y - 1,x - 1];
                            break;
                        case 4:
                            inputDecompress[y,x] = inputDecode[y,x] + inputDecompress[y,x - 1] + inputDecompress[y - 1,x] - inputDecompress[y - 1,x - 1];
                            break;
                        case 5:
                            inputDecompress[y,x] = inputDecode[y,x] + (inputDecompress[y,x - 1] + (inputDecompress[y - 1,x] - inputDecompress[y - 1,x - 1]) / 2);
                            break;
                        case 6:
                            inputDecompress[y,x] = inputDecode[y,x] + (inputDecompress[y - 1,x] + (inputDecompress[y,x - 1] - inputDecompress[y - 1,x - 1]) / 2);
                            break;
                        case 7:
                            inputDecompress[y,x] = inputDecode[y,x] + (inputDecompress[y,x - 1] + inputDecompress[y - 1,x]) / 2;
                            break;
                        default:
                            break;
                    }
                }
            }

            for (int x = 1; x <= 15; x++)
            {
                for (int y = 1; y <= 15; y++)
                {
                    inputDecompress[y, x] += 88;
                }
            }
        }
        //===========================================================================================

        //=====HUFFMAN=ENCODING======================================================================
        public static void huffmanCodeEncoder(int[,] inputPredictor, String[,] inputHuffman)
        {
            String code = "";
            for (int x = 0; x <= 15; x++)
            {
                for (int y = 0; y <= 15; y++)
                {
                    switch (inputPredictor[x,y])
                    {
                        case 0:
                            code = "1";
                            break;
                        case 1:
                            code = "00";
                            break;
                        case -1:
                            code = "011";
                            break;
                        case 2:
                            code = "0100";
                            break;
                        case -2:
                            code = "01011";
                            break;
                        case 3:
                            code = "010100";
                            break;
                        case -3:
                            code = "0101011";
                            break;
                        case 4:
                            code = "01010100";
                            break;
                        case -4:
                            code = "010101011";
                            break;
                        case 5:
                            code = "0101010100";
                            break;
                        case -5:
                            code = "01010101011";
                            break;
                        case 6:
                            code = "010101010100";
                            break;
                        case -6:
                            code = "0101010101011";
                            break;
                        default:
                            code = "0" + IntToBinaryString(inputPredictor[0,0]);
                            break;
                    }
                    inputHuffman[x,y] = code;
                }
            }
        }
        //===========================================================================================

        //=====INTO=TO=BINARY=STRING=================================================================
        public static String IntToBinaryString(int number)
        {
            //const int mask = 1;
            var binary = String.Empty;
            while (number > 0)
            {
                // Logical AND the number and prepend it to the result string
                binary = (number & 1) + binary;
                number = number >> 1;
            }

            return binary;
        }
        //===========================================================================================

        //=====HUFFMAN=DECODING======================================================================
        public static void huffmanCodeDecoder(int[,] inputDecoder, String[,] inputHuffman)
        {
            int code = 0;
            for (int x = 0; x <= 15; x++)
            {
                for (int y = 0; y <= 15; y++)
                {
                    switch (inputHuffman[x,y])
                    {
                        case "1":
                            code = 0;
                            break;
                        case "00":
                            code = 1;
                            break;
                        case "011":
                            code = -1;
                            break;
                        case "0100":
                            code = 2;
                            break;
                        case "01011":
                            code = -2;
                            break;
                        case "010100":
                            code = 3;
                            break;
                        case "0101011":
                            code = -3;
                            break;
                        case "01010100":
                            code = 4;
                            break;
                        case "010101011":
                            code = -4;
                            break;
                        case "0101010100":
                            code = 5;
                            break;
                        case "01010101011":
                            code = -5;
                            break;
                        case "010101010100":
                            code = 6;
                            break;
                        case "0101010101011":
                            code = -6;
                            break;
                        default:
                            code = Convert.ToInt32(inputHuffman[0,0]);
                            break;
                    }
                    inputDecoder[x,y] = code;
                }
            }
        }
        //===========================================================================================

        //=====RMS=CALCULATOR========================================================================
        public static double RMSCalculator(int[,] inputOriginal, int[,] inputDecompress)
        {
            double error = 0;
            double total = 0;

            for (int x = 0; x <= 15; x++)
            {
                for (int y = 0; y <= 15; y++)
                {
                    total = inputOriginal[x,y] - inputDecompress[x,y];
                    total *= total;
                    error += total;
                }
            }
            error = error * (1 / (16 * 16));
            error = Math.Sqrt(error);
            return error;
        }
        //===========================================================================================

        //=====COMPRESSION=RATION====================================================================
        public static double compressionRatio(String[,] input)
        {
            double ratio = 0;
            double totalBits = 0;
            for (int x = 0; x <= 15; x++)
            {
                for (int y = 0; y <= 15; y++)
                {
                    totalBits += input[x,y].Length;
                }
            }
            ratio = (16 * 16 * 8) / totalBits;
            return ratio;
        }
        //===========================================================================================

        //=====BIT=PIXEL=============================================================================
        public static double bitPixel(double input)
        {
            double pixel = 0;
            pixel = 8 / input;
            return pixel;
        }
        //===========================================================================================

        //=====PRINT=INT=ARRAY=======================================================================
        public static void printIntArray(int[,] input) 
        {
            for (int x = 1; x < input.GetLength(0); x++) 
            {
                for (int y = 1; y < input.GetLength(1); y++)
                {
                    Console.Write(input[x,y]);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        //===========================================================================================

        //=====PRINT=STRING=ARRAY====================================================================
        public static void printStringArray(String[,] input)
        {
            for (int x = 1; x < input.GetLength(0); x++)
            {
                for (int y = 1; y < input.GetLength(1); y++)
                {
                    Console.Write(input[x, y]);
                    //Console.Write(" ");
                }
                //Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine();
        }
        //===========================================================================================
    }
}
