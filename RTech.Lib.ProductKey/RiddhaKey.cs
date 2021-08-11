using System;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;

namespace RTech.Lib.ProductKey
{
    public class RiddhaKey
    {
        private string Machinekey { get; set; }
        public string SecretKey { get; set; }
        public string MacAddress { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool validMac { get; set; }
        public bool validSecrete { get; set; }
        public RiddhaKey()
        {

        }
        public RiddhaKey(string secretKey, DateTime expiryDate)
        {
            SecretKey = secretKey;
            ExpiryDate = expiryDate;
        }
        public RiddhaKey(string secretKey, DateTime expiryDate, string address)
        {
            SecretKey = secretKey;
            ExpiryDate = expiryDate;
            MacAddress = address;
        }
        public string getProductKey()
        {
            return Encrypt(MergePrimaryInformation());
        }
        public string GetMacAddress()
        {
            if (string.IsNullOrEmpty(MacAddress))
            {
                return (from nic in NetworkInterface.GetAllNetworkInterfaces()
                        where string.IsNullOrEmpty(nic.GetPhysicalAddress().ToString()) == false
                        select nic.GetPhysicalAddress().ToString()
                       ).FirstOrDefault();
            }
            else
                return MacAddress;


        }
        private string MergePrimaryInformation()
        {
            return string.Format("{0}|{1}|{2}", SecretKey, GetMacAddress(), ExpiryDate.ToString("yyyy/MM/dd"));
        }
        private string Encrypt(string source)
        {
            //byte[] unhashedBytes = Encoding.Unicode.GetBytes(String.Concat(source));

            //SHA256Managed sha256 = new SHA256Managed();
            //byte[] hashedBytes = sha256.ComputeHash(unhashedBytes);

            //return System.Text.Encoding.Default.GetString(hashedBytes);

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(source);
            byte[] keyBytes = new Rfc2898DeriveBytes("ProductKey", Encoding.ASCII.GetBytes("riddhasoft")).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged()
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.Zeros
            };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes("@a23dgr6she6hdy6"));

            byte[] cipherTextByte;
            using (var memoryStream = new MemoryStream())
            {
                using (var cryptostream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptostream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptostream.FlushFinalBlock();
                    cipherTextByte = memoryStream.ToArray();
                    cryptostream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextByte);
        }
        private string Decrypt(string source)
        {
            try
            {
                //if (source.Length != 64)
                //{
                //    validMac = false;
                //    validSecrete = false;
                //    ExpiryDate = System.DateTime.Now.AddDays(-1);
                //    return "";
                //}
                byte[] cipherTextBytes = Convert.FromBase64String(source);
                byte[] keyBytes = new Rfc2898DeriveBytes("ProductKey", Encoding.ASCII.GetBytes("riddhasoft")).GetBytes(256 / 8);
                var symmetricKey = new RijndaelManaged()
                {
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.Zeros
                };

                var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes("@a23dgr6she6hdy6"));
                byte[] plainTextByte = new byte[cipherTextBytes.Length];
                using (var memoryStream = new MemoryStream(cipherTextBytes))
                {
                    using (var cryptostream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        int decryptedByteCount = cryptostream.Read(plainTextByte, 0, plainTextByte.Length);
                        cryptostream.Close();
                        memoryStream.Close();
                        return Encoding.UTF8.GetString(plainTextByte, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
                    }
                }
            }
            catch (Exception)
            {
                validMac = false;
                validSecrete = false;
                ExpiryDate = System.DateTime.Now.AddDays(-1);
                return "";
            }

        }
        public void validateProduct(string ProductKey, string secretKey)
        {
            this.SecretKey = secretKey;
            var stringKey = Decrypt(ProductKey);
            string[] array = stringKey.Split('|');
            if (array.Length == 3)
            {
                validMac = GetMacAddress() == array[1];
                MacAddress = array[1];
                validSecrete = SecretKey == array[0];
                try
                {
                    var dateArray = array[2].Split('/');
                    ExpiryDate = DateTime.Parse(array[2]);
                }
                catch (Exception)
                {
                    ExpiryDate = System.DateTime.Now.AddDays(-1);
                }
            }
        }
    }
}
