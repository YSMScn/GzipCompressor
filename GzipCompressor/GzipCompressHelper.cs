﻿using System.IO.Compression;
using System.Text;

namespace GzipCompressor
{
    public static class GzipCompressHelper
    {
        public static async Task<string> CompressStringAsync(string inputString)
        {
            return await Task.Run(() =>
            {
               return CompressString(inputString);
            });
        }
        public static async Task<string> DecompressStringAsync(string inputString)
        {
            return await Task.Run(() =>
            {
                return DecompressString(inputString);
            });
        }
        public static async Task<MemoryStream> CompressStreamAsync(Stream inputStream)
        {
            return await Task.Run(() =>
            {
                return CompressStream(inputStream);
            });
        }
        public static async Task<MemoryStream> DecompressStreamAsync(Stream inputStream)
        {
            return await Task.Run(() =>
            {
                return DecompressStream(inputStream);
            });
        }
        public static string CompressString(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                throw new ArgumentNullException(nameof(inputString));
            }
            var inputByteArray = Encoding.UTF8.GetBytes(inputString);
            var outputStream = new MemoryStream();
            using (var gzipStream = new GZipStream(outputStream, CompressionMode.Compress))
            {
                gzipStream.Write(inputByteArray, 0, inputByteArray.Length);
            }
            return Convert.ToBase64String(outputStream.ToArray());
        }

        public static string DecompressString(string inputString)
        {
            if (string.IsNullOrWhiteSpace(inputString))
            {
                throw new ArgumentNullException(nameof(inputString));
            }
            var inputByteArray = Convert.FromBase64String(inputString);
            var inputStream = new MemoryStream(inputByteArray);
            using (var gzipStream = new GZipStream(inputStream, CompressionMode.Decompress))
            {
                using var sr = new StreamReader(gzipStream);
                return sr.ReadToEnd();
            }
        }

        public static MemoryStream CompressStream(Stream inputStream)
        {
            var decompressedStream = new MemoryStream();
            MemoryStream outputStream = new();
            using (var gzipStream = new GZipStream(decompressedStream, CompressionMode.Compress,true))
            {
                inputStream.CopyTo(gzipStream);
                //decompressedStream.WriteTo(outputStream);
            }
            decompressedStream.Position=0;
            return decompressedStream;

        }

        public static MemoryStream DecompressStream(Stream inputStream)
        {
            var outputStream = new MemoryStream();
            using (var gzipStream = new GZipStream(inputStream, CompressionMode.Decompress))
            {
                gzipStream.CopyTo(outputStream);
            }
            outputStream.Position = 0;
            return outputStream;
        }
        
    }
}