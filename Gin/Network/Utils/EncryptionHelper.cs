using System.Security.Cryptography;
using WindyFarm.Gin.SystemLog;

namespace WindyFarm.Gin.Network.Utils
{
    public class EncryptionHelper
    {
        public static byte[] Encrypt(byte[] plainText, byte[] key, byte[] iv)
        {
            /*
            using Aes aesAlg = Aes.Create();
            aesAlg.Key = key;
            aesAlg.IV = iv;
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;

            using ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            using MemoryStream msEncrypt = new();
            using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
            try
            {
                csEncrypt.Write(plainText, 0, plainText.Length);
                csEncrypt.FlushFinalBlock();
                return msEncrypt.ToArray();
            }
            catch (Exception ex)
            {
                GinLogger.Fatal(ex);
            }

            return [];
            */
            return plainText;
        }

        public static byte[] Decrypt(byte[] cipherText, byte[] key, byte[] iv)
        {
            /*
            using Aes aesAlg = Aes.Create();
            aesAlg.Key = key;
            aesAlg.IV = iv;
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;

            using ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            using MemoryStream msDecrypt = new(cipherText);
            using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
            using MemoryStream resultStream = new();
            try
            {
                csDecrypt.CopyTo(resultStream);
                return resultStream.ToArray();
            }
            catch (Exception ex)
            {
                GinLogger.Fatal(ex);
                return [];
            }
            */
            return cipherText;
        }

        public static byte[] GenerateRandomKey(BlockSize keyBlockSize)
        {
            int keySizeInBytes = TranslateBlockSize(keyBlockSize);
            byte[] key = new byte[keySizeInBytes];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(key);
            }
            return key;
        }

        public static byte[] GenerateRandomIV(BlockSize keyBlockSize)
        {
            int ivSizeInBytes = TranslateBlockSize(keyBlockSize);
            byte[] iv = new byte[ivSizeInBytes];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(iv);
            }
            return iv;
        }


        private static int TranslateBlockSize(BlockSize blockSize)
        {
            switch (blockSize)
            {
                case BlockSize.BitSize128:
                    return 16;
                case BlockSize.BitSize256:
                    return 32;
                default:
                    break;
            }

            return 8;
        }
    }

    public enum BlockSize
    {
        BitSize128,
        BitSize256
    }
}
