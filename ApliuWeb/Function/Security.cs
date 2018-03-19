using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ApliuWeb
{
    public class Security
    {
        private static string key = "ApliuTool";

        /// <summary>
        /// 公钥加密
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string EncryptContent(Object Content)
        {
            string Value = Content.ToString();
            if (string.IsNullOrEmpty(Value)) return null;

            try
            {
                using (TripleDES provider = TripleDES.Create())
                {
                    key = key.Length >= 24 ? key : key.PadRight(24, '9');
                    provider.Key = Encoding.ASCII.GetBytes(key.Substring(0, 24));
                    provider.IV = Encoding.ASCII.GetBytes(key.Substring(8, 8));

                    byte[] encryptedBinary = EncryptTextToMemory(Value, provider.Key, provider.IV);
                    return Convert.ToBase64String(encryptedBinary);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 公钥解密
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        public static string DecryptConten(Object Content)
        {
            string Value = Content.ToString();
            if (string.IsNullOrEmpty(Value)) return Value;

            try
            {
                using (TripleDES provider = TripleDES.Create())
                {
                    key = key.Length >= 24 ? key : key.PadRight(24, '9');
                    provider.Key = Encoding.ASCII.GetBytes(key.Substring(0, 24));
                    provider.IV = Encoding.ASCII.GetBytes(key.Substring(8, 8));

                    byte[] buffer = Convert.FromBase64String(Value);
                    return DecryptTextFromMemory(buffer, provider.Key, provider.IV);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static byte[] EncryptTextToMemory(string data, byte[] key, byte[] iv)
        {
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, TripleDES.Create().CreateEncryptor(key, iv), CryptoStreamMode.Write))
                {
                    byte[] toEncrypt = Encoding.Unicode.GetBytes(data);
                    cs.Write(toEncrypt, 0, toEncrypt.Length);
                    cs.FlushFinalBlock();
                }

                return ms.ToArray();
            }
        }

        private static string DecryptTextFromMemory(byte[] data, byte[] key, byte[] iv)
        {
            using (var ms = new MemoryStream(data))
            {
                using (var cs = new CryptoStream(ms, TripleDES.Create().CreateDecryptor(key, iv), CryptoStreamMode.Read))
                {
                    using (var sr = new StreamReader(cs, Encoding.Unicode))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }
    }
}