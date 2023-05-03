using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GeminiDecrypt
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Drag and drop files into the .exe");
                Console.ReadKey();
                return;
            }
            byte[] data = null;
            foreach (var file in args)
            {
                if (!File.Exists(file))
                {
                    continue;
                }
                var info = new FileInfo(file);
                if (info.Length < 20)
                {
                    continue;
                }

                data = File.ReadAllBytes(file);
                var result = new byte[data.Length - 8];
                var xorKey = Md5(result.Length);

                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = (byte)(data[i + 8] ^ xorKey[i % xorKey.Length]);
                }

                string outName = $"{file}.decrypted";
                File.WriteAllBytes(outName, result);
            }

            Console.WriteLine("Done");
        }

        static byte[] Md5(int length)
        {
            using (MD5 md5 = MD5.Create())
            {
                return md5.ComputeHash(Encoding.UTF8.GetBytes($"hR89347kLjfdAy0u^lkdjadfj#jf--ie{length}"));
            }
        }
    }
}
