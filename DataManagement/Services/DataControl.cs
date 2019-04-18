using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DataManagement.Models;
using Microsoft.Extensions.Configuration;

namespace DataManagement.Services
{
    public class DataControl : IDataControl
    {
        private readonly DataManagementContext _db;        

        public DataControl(DataManagementContext db)
        {
            _db = db;
        }

        public async Task<int> AddPostAsync(TreeListTable post)
        {
            if (_db == null) return 0;
            var secret = Startup.StaticConfig.GetSection("AppConfiguration")["SecretKey"];
            var vector = Startup.StaticConfig.GetSection("AppConfiguration")["vector"];

            post.Tree = DecryptAesManaged(post.Tree, secret, vector);
            await _db.TreeListTable.AddAsync(post);
            await _db.SaveChangesAsync();

            return post.GetHashCode();

        }

        public static string DecryptAesManaged(string encryptedText, string key, string vector )
        {
            try
            {
                // Create Aes that generates a new key and initialization vector (IV).    
                // Same key must be used in encryption and decryption    
                using (AesManaged aes = new AesManaged())
                {
                    aes.Mode = CipherMode.ECB;
                    // Decrypt the bytes to a string.    
                    return Decrypt(Convert.FromBase64String(encryptedText), Convert.FromBase64String(key), Convert.FromBase64String(vector));
                }
            }
            catch (Exception)
            {
                return  String.Empty;
            }
        }

        private static string Decrypt(byte[] cipherText, byte[] Key, byte[] IV)
        {
            string plaintext = null;
            // Create AesManaged    
            using (AesManaged aes = new AesManaged())
            {
                // Create a decryptor    
                ICryptoTransform decryptor = aes.CreateDecryptor(Key, IV);
                // Create the streams used for decryption.    
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    // Create crypto stream    
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        // Read crypto stream    
                        using (StreamReader reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }



    }
}