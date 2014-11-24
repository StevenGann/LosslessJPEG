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
        public static void encode(int[,] tempOriginalImage, int[,] tempPredictorImage, int method)
        {
            tempPredictorImage[0,0] = tempOriginalImage[0,0];
            for (int i = 1; i <= 15; i++)
            {
                tempPredictorImage[0,i] = tempOriginalImage[0,i] - tempOriginalImage[0,i-1];
                tempPredictorImage[i,0] = tempOriginalImage[i,0] - tempOriginalImage[i-1,0];
            }

            for (int i = 1; i <= 15; i++)
            {
                for (int j = 1; j <= 15; j++)
                {
                    switch (method)
                    {
                        case 1:
                            tempPredictorImage[j,i] = tempOriginalImage[j,i] - tempOriginalImage[j,i - 1];
                            break;
                        case 2:
                            tempPredictorImage[j,i] = tempOriginalImage[j,i] - tempOriginalImage[j - 1,i];
                            break;
                        case 3:
                            tempPredictorImage[j,i] = tempOriginalImage[j,i] - tempOriginalImage[j - 1,i - 1];
                            break;
                        case 4:
                            tempPredictorImage[j,i] = tempOriginalImage[j,i] - tempOriginalImage[j,i - 1] - tempOriginalImage[j - 1,i] + tempOriginalImage[j - 1,i - 1];
                            break;
                        case 5:
                            tempPredictorImage[j,i] = tempOriginalImage[j,i] - (tempOriginalImage[j,i - 1] + (tempOriginalImage[j - 1,i] - tempOriginalImage[j - 1,i - 1]) / 2);
                            break;
                        case 6:
                            tempPredictorImage[j,i] = tempOriginalImage[j,i] - (tempOriginalImage[j - 1,i] + (tempOriginalImage[j,i - 1] - tempOriginalImage[j - 1,i - 1]) / 2);
                            break;
                        case 7:
                            tempPredictorImage[j,i] = tempOriginalImage[j,i] - (tempOriginalImage[j,i - 1] + tempOriginalImage[j - 1,i]) / 2;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        //===========================================================================================

        //=====DECODE================================================================================
        public static void decoder(int[,] tempDecompressionImage, int[,] tempDecoderImage, int method)
        {
            tempDecompressionImage[0,0] = tempDecoderImage[0,0];
            for (int i = 1; i <= 15; i++)
            {
                tempDecompressionImage[0,i] = tempDecoderImage[0,i] + tempDecompressionImage[0,i - 1];
                tempDecompressionImage[i,0] = tempDecoderImage[i,0] + tempDecompressionImage[i - 1,0];
            }

            for (int i = 1; i <= 15; i++)
            {
                for (int j = 1; j <= 15; j++)
                {
                    switch (method)
                    {
                        case 1:
                            tempDecompressionImage[j,i] = tempDecoderImage[j,i] + tempDecompressionImage[j,i - 1];
                            break;
                        case 2:
                            tempDecompressionImage[j,i] = tempDecoderImage[j,i] + tempDecompressionImage[j - 1,i];
                            break;
                        case 3:
                            tempDecompressionImage[j,i] = tempDecoderImage[j,i] + tempDecompressionImage[j - 1,i - 1];
                            break;
                        case 4:
                            tempDecompressionImage[j,i] = tempDecoderImage[j,i] + tempDecompressionImage[j,i - 1] + tempDecompressionImage[j - 1,i] - tempDecompressionImage[j - 1,i - 1];
                            break;
                        case 5:
                            tempDecompressionImage[j,i] = tempDecoderImage[j,i] + (tempDecompressionImage[j,i - 1] + (tempDecompressionImage[j - 1,i] - tempDecompressionImage[j - 1,i - 1]) / 2);
                            break;
                        case 6:
                            tempDecompressionImage[j,i] = tempDecoderImage[j,i] + (tempDecompressionImage[j - 1,i] + (tempDecompressionImage[j,i - 1] - tempDecompressionImage[j - 1,i - 1]) / 2);
                            break;
                        case 7:
                            tempDecompressionImage[j,i] = tempDecoderImage[j,i] + (tempDecompressionImage[j,i - 1] + tempDecompressionImage[j - 1,i]) / 2;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        //===========================================================================================

        //=====HUFFMAN=ENCODING======================================================================
        public static void huffmanCodeEncoder(int[,] tempPredictorImage, String[,] tempHuffmanCode)
        {
            String tempCode = "";
            for (int i = 0; i <= 15; i++)
            {
                for (int j = 0; j <= 15; j++)
                {
                    switch (tempPredictorImage[i,j])
                    {
                        case 0:
                            tempCode = "1";
                            break;
                        case 1:
                            tempCode = "00";
                            break;
                        case -1:
                            tempCode = "011";
                            break;
                        case 2:
                            tempCode = "0100";
                            break;
                        case -2:
                            tempCode = "01011";
                            break;
                        case 3:
                            tempCode = "010100";
                            break;
                        case -3:
                            tempCode = "0101011";
                            break;
                        case 4:
                            tempCode = "01010100";
                            break;
                        case -4:
                            tempCode = "010101011";
                            break;
                        case 5:
                            tempCode = "0101010100";
                            break;
                        case -5:
                            tempCode = "01010101011";
                            break;
                        case 6:
                            tempCode = "010101010100";
                            break;
                        case -6:
                            tempCode = "0101010101011";
                            break;
                        default:
                            tempCode = "0" + IntToBinaryString(tempPredictorImage[0,0]);
                            break;
                    }
                    tempHuffmanCode[i,j] = tempCode;
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
        public static void huffmanCodeDecoder(int[,] tempDecoderImage, String[,] tempHuffmanCode)
        {
            int tempCode = 0;
            for (int i = 0; i <= 15; i++)
            {
                for (int j = 0; j <= 15; j++)
                {
                    switch (tempHuffmanCode[i,j])
                    {
                        case "1":
                            tempCode = 0;
                            break;
                        case "00":
                            tempCode = 1;
                            break;
                        case "011":
                            tempCode = -1;
                            break;
                        case "0100":
                            tempCode = 2;
                            break;
                        case "01011":
                            tempCode = -2;
                            break;
                        case "010100":
                            tempCode = 3;
                            break;
                        case "0101011":
                            tempCode = -3;
                            break;
                        case "01010100":
                            tempCode = 4;
                            break;
                        case "010101011":
                            tempCode = -4;
                            break;
                        case "0101010100":
                            tempCode = 5;
                            break;
                        case "01010101011":
                            tempCode = -5;
                            break;
                        case "010101010100":
                            tempCode = 6;
                            break;
                        case "0101010101011":
                            tempCode = -6;
                            break;
                        default:
                            tempCode = Convert.ToInt32(tempHuffmanCode[0,0]);
                            break;
                    }
                    tempDecoderImage[i,j] = tempCode;
                }
            }
        }
        //===========================================================================================

        //=====RMS=CALCULATOR================================================================================
        public static double RMSCalculator(int[,] tempOriginalImage, int[,] tempDecompressionImage)
        {
            double error = 0;
            double tempTotal = 0;

            for (int m = 0; m <= 15; m++)
            {
                for (int n = 0; n <= 15; n++)
                {
                    tempTotal = tempOriginalImage[m,n] - tempDecompressionImage[m,n];
                    tempTotal *= tempTotal;
                    error += tempTotal;
                }
            }
            error = error * (1 / (16 * 16));
            error = Math.Sqrt(error);
            return error;
        }
        //===========================================================================================

        //=====COMPRESSION=RATION====================================================================
        public static double compressionRatio(String[,] tempHuffmanCode)
        {
            double tempCr = 0;
            double totalBit = 0;
            for (int m = 0; m <= 15; m++)
            {
                for (int n = 0; n <= 15; n++)
                {
                    totalBit += tempHuffmanCode[m,n].Length;
                }
            }
            tempCr = (16 * 16 * 8) / totalBit;
            return tempCr;
        }
        //===========================================================================================

        //=====BIT=PIXEL=============================================================================
        public static double bitPixel(double tempC)
        {
            double tempPixel = 0;
            tempPixel = 8 / tempC;
            return tempPixel;
        }
        //===========================================================================================

        //=====PRINT=INT=ARRAY=======================================================================
        public static void printIntArray(int[,] tempArray) 
        {
            for (int x = 1; x < tempArray.GetLength(0); x++) 
            {
                for (int y = 1; y < tempArray.GetLength(1); y++)
                {
                    Console.Write(tempArray[x,y]);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        //===========================================================================================

        //=====PRINT=STRING=ARRAY====================================================================
        public static void printStringArray(String[,] tempArray)
        {
            for (int x = 1; x < tempArray.GetLength(0); x++)
            {
                for (int y = 1; y < tempArray.GetLength(1); y++)
                {
                    Console.Write(tempArray[x, y]);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        //===========================================================================================
    }
}
