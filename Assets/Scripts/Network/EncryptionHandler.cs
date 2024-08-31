using System;
using UnityEngine;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Xml.Serialization;

namespace W3Labs.ViralRunner.Network
{

    public class EncryptionHandler
    {
        //public static EncryptionHandler Instance;

        static RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(2048);
        private static RSAParameters _privateKey;
        private static RSAParameters _publicKey;

        static string key = "A60A5770FE5E7AB200BA9CFC94E4E8B0"; //set any string of 32 chars
        static string iv = "1234567887654321"; //set any string of 16 cha


        //void Awake()
        //{
        //    if (Instance == null)
        //    {
        //        Instance = this;
        //    }
        //    else if (Instance != this)
        //    {
        //        Destroy(gameObject);
        //    }
        //}


        public static string AESEncryption(string inputData)
        {
            AesCryptoServiceProvider AEScryptoProvider = new AesCryptoServiceProvider();
            AEScryptoProvider.BlockSize = 128;
            AEScryptoProvider.KeySize = 256;
            AEScryptoProvider.Key = ASCIIEncoding.ASCII.GetBytes(key);
            AEScryptoProvider.IV = ASCIIEncoding.ASCII.GetBytes(iv);
            AEScryptoProvider.Mode = CipherMode.CBC;
            AEScryptoProvider.Padding = PaddingMode.PKCS7;

            byte[] txtByteData = ASCIIEncoding.ASCII.GetBytes(inputData);
            ICryptoTransform trnsfrm = AEScryptoProvider.CreateEncryptor(AEScryptoProvider.Key, AEScryptoProvider.IV);

            byte[] result = trnsfrm.TransformFinalBlock(txtByteData, 0, txtByteData.Length);
            return Convert.ToBase64String(result);
        }


        public static string AESDecryption(string inputData)
        {
            AesCryptoServiceProvider AEScryptoProvider = new AesCryptoServiceProvider();
            AEScryptoProvider.BlockSize = 128;
            AEScryptoProvider.KeySize = 256;
            AEScryptoProvider.Key = ASCIIEncoding.ASCII.GetBytes(key);
            AEScryptoProvider.IV = ASCIIEncoding.ASCII.GetBytes(iv);
            AEScryptoProvider.Mode = CipherMode.CBC;
            AEScryptoProvider.Padding = PaddingMode.PKCS7;

            byte[] txtByteData = Convert.FromBase64String(inputData);
            ICryptoTransform trnsfrm = AEScryptoProvider.CreateDecryptor();

            byte[] result = trnsfrm.TransformFinalBlock(txtByteData, 0, txtByteData.Length);
            return ASCIIEncoding.ASCII.GetString(result);
        }

        public string getRSAPublicKey()
        {
            _privateKey = RSA.ExportParameters(true);
            _publicKey = RSA.ExportParameters(false);
            var sw = new StringWriter();
            var xs = new XmlSerializer(typeof(RSAParameters));
            xs.Serialize(sw, _publicKey);

            return sw.ToString();
        }

        public static string RSAEncryption(string inputData)
        {
            RSA = new RSACryptoServiceProvider();
            RSA.ImportParameters(_publicKey);
            var data = Encoding.Unicode.GetBytes(inputData);
            var cypher = RSA.Encrypt(data, false);

            return Convert.ToBase64String(cypher);
        }

        public string RSADecryption(string cypherText)
        {
            var dataBytes = Convert.FromBase64String(cypherText);
            RSA.ImportParameters(_privateKey);
            var plainText = RSA.Decrypt(dataBytes, false);

            return Encoding.Unicode.GetString(plainText);
        }

    }
}