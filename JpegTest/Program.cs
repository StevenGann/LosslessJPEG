using LosslessJPEG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JpegTest
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] originalImage = {
            {88, 88, 88, 89, 90, 91, 92, 93, 94, 95, 93, 95, 96, 98, 97, 94},
            {93, 91, 91, 90, 92, 93, 94, 94, 95, 95, 92, 93, 95, 95, 95, 96},
            {95, 95, 95, 95, 96, 97, 94, 96, 97, 96, 98, 97, 98, 99, 95, 97},
            {97, 96, 98, 97, 98, 94, 95, 97, 99, 100, 99, 101, 100, 100, 98, 98},
            {99, 100, 97, 99, 100, 100, 98, 98, 100, 101, 100, 99, 101, 102, 99, 100},
            {100, 101, 100, 99, 101, 102, 99, 100, 103, 102, 103, 101, 101, 100, 102, 101},
            {100, 102, 103, 101, 101, 100, 102, 103, 103, 105, 104, 104, 103, 104, 104, 103},
            {103, 105, 103, 105, 105, 104, 104, 104, 102, 101, 100, 100, 100, 101, 102, 103},
            {104, 104, 105, 105, 105, 104, 104, 106, 102, 103, 101, 101, 102, 101, 102, 102},
            {102, 105, 105, 105, 106, 104, 106, 104, 103, 101, 100, 100, 101, 102, 102, 103},
            {102, 105, 105, 105, 106, 104, 106, 104, 103, 101, 100, 100, 101, 102, 102, 103},
            {102, 105, 105, 105, 106, 104, 105, 104, 103, 101, 102, 100, 102, 102, 102, 103},
            {104, 105, 106, 105, 106, 104, 106, 103, 103, 102, 100, 100, 101, 102, 102, 103},
            {103, 105, 107, 107, 106, 104, 106, 104, 103, 101, 100, 100, 101, 102, 102, 103},
            {103, 105, 106, 108, 106, 104, 106, 105, 103, 101, 101, 100, 101, 103, 102, 105},
            {102, 105, 105, 105, 106, 104, 106, 107, 104, 103, 102, 100, 101, 104, 102, 104}};

        //I reused predictorImage, compressedImage, decoderImage, 
        //and DecompressionImage for all seven cases
        int[,] predictorImage = new int[16,16];//Image after encoder
        String[,] compressedImage = new String[16,16];//Image after huffman encoder
        int[,] decoderImage = new int[16,16];//Image after huffman decoder
        int[,] decompressionImage = new int[16,16];//Image after decoder

        double rms = 0;
        double cr = 0;
        double bitPerPixel = 0;

        //print original image
        Console.WriteLine("Original Image");
        JpegCodec.printIntArray(originalImage);

        for (int i = 1; i <= 7; i++) {
            Console.WriteLine("=====================================================================");
            Console.WriteLine("\nCase " + i + ":");
                        
            //encode the original method by different methods or cases
            JpegCodec.encode(originalImage, predictorImage, i);

            //print the encode image
            Console.WriteLine("Coefficients after the Predictor");
            JpegCodec.printIntArray(predictorImage);

            //encode by the huffman table 
            JpegCodec.huffmanCodeEncoder(predictorImage, compressedImage);

            //print the huffman encode image
            Console.WriteLine("Huffman Encoded compressed image in binary");
            JpegCodec.printStringArray(compressedImage);

            //decode by the huffman table
            JpegCodec.huffmanCodeDecoder(decoderImage, compressedImage);

            //print the huffman decode image
            Console.WriteLine("Huffman Decoded");
            JpegCodec.printIntArray(decoderImage);

            //decode the huffman image by its specific method
            JpegCodec.decoder(decompressionImage, decoderImage, i);

            //print the final decode image
            Console.WriteLine("Image after decompression");
            JpegCodec.printIntArray(decompressionImage);

            //calculate the compressed ratio
            cr = JpegCodec.compressionRatio(compressedImage);
            //print the compressed ratio
            Console.WriteLine("Compression Ratio: " + cr);
            
            //calculate the rms
            rms = JpegCodec.RMSCalculator(originalImage, decompressionImage);
            //print rms
            Console.WriteLine("Root Mean Square: " + rms);

            //calculate the bit per pixel
            bitPerPixel = JpegCodec.bitPixel(cr);
            //print bit per pixel
            Console.WriteLine("Bits Per Pixel: " + bitPerPixel);
            
            Console.WriteLine();
            //Console.Read();
            
        }
        Console.Read();
        }
    }
}
