
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Moss.Hospital.Data.Dao
{
    public class Cipher
    {
        private static byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;
            var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }

                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }
        private static byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            try
            {
                byte[] decryptedBytes = null;
                var saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
                using (MemoryStream ms = new MemoryStream())
                {
                    using (RijndaelManaged AES = new RijndaelManaged())
                    {
                        var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);

                        AES.KeySize = 256;
                        AES.BlockSize = 128;
                        AES.Key = key.GetBytes(AES.KeySize / 8);
                        AES.IV = key.GetBytes(AES.BlockSize / 8);
                        AES.Mode = CipherMode.CBC;

                        using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                            cs.Close();
                        }

                        decryptedBytes = ms.ToArray();
                    }
                }

                return decryptedBytes;
            }
            catch
            {
                return null;
            }
        }

        //public static string Encrypt(string plainText, string password)
        //{
        //    if (plainText == null)
        //    {
        //        return null;
        //    }

        //    if (password == null)
        //    {
        //        password = String.Empty;
        //    }

        //    // Get the bytes of the string
        //    var bytesToBeEncrypted = Encoding.UTF8.GetBytes(plainText);
        //    var passwordBytes = Encoding.UTF8.GetBytes(password);

        //    // Hash the password with SHA256
        //    passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

        //    var bytesEncrypted = Cipher.Encrypt(bytesToBeEncrypted, passwordBytes);

        //    return Convert.ToBase64String(bytesEncrypted);
        //}
        //public static string Decrypt(string encryptedText, string password)
        //{
        //    try
        //    {
        //        if (encryptedText == null)
        //        {
        //            return null;
        //        }

        //        if (password == null)
        //        {
        //            password = String.Empty;
        //        }

        //        // Get the bytes of the string
        //        var bytesToBeDecrypted = Convert.FromBase64String(encryptedText);
        //        var passwordBytes = Encoding.UTF8.GetBytes(password);

        //        passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

        //        var bytesDecrypted = Cipher.Decrypt(bytesToBeDecrypted, passwordBytes);

        //        return Encoding.UTF8.GetString(bytesDecrypted);
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        public static string Encrypt(string key, string password)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(key));
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;
            byte[] DataToEncrypt = UTF8.GetBytes(password);
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }
            return Convert.ToBase64String(Results);
        }
        public static string Decrypt(string key, string password)
        {
            try
            {
                byte[] Results;
                System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
                MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
                byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(key));
                TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();
                TDESAlgorithm.Key = TDESKey;
                TDESAlgorithm.Mode = CipherMode.ECB;
                TDESAlgorithm.Padding = PaddingMode.PKCS7;
                byte[] DataToDecrypt = Convert.FromBase64String(password);
                try
                {
                    ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                    Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
                }
                finally
                {
                    TDESAlgorithm.Clear();
                    HashProvider.Clear();
                }
                return UTF8.GetString(Results);
            }
            catch
            {
                return null;
            }
        }
    }
}