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

        /// <summary>
        /// Adding information to the database with decryption.
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public async Task<Guid> AddPostAsync(TreeListTable post)
        {
            if (_db == null) return Guid.Empty;
            var secret = Startup.StaticConfig.GetSection("AppConfiguration")["SecretKey"];
            var vector = Startup.StaticConfig.GetSection("AppConfiguration")["vector"];

            post.Tree = DecryptAesManaged(post.Tree, secret, vector);
            await _db.TreeListTable.AddAsync(post);
            await _db.SaveChangesAsync();

            return post.Id;

        }

        /// <summary>
        /// Calling decryption method.
        /// </summary>
        /// <param name="encryptedText"></param>
        /// <param name="key"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static string DecryptAesManaged(string encryptedText, string key, string vector )
        {
            try
            {
                // Create Aes that generates a new key and initialization vector (IV).    
                // Same key must be used in encryption and decryption    
                using (var aes = new AesManaged())
                {
                    aes.Mode = CipherMode.ECB;
                    // Decrypt the bytes to a string.    
                    return Decrypt(Convert.FromBase64String(encryptedText), Convert.FromBase64String(key), Convert.FromBase64String(vector));
                }
            }
            catch (Exception)
            {
                return  string.Empty;
            }
        }

        /// <summary>
        /// Decrypt incoming text with using AES method.
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="Key"></param>
        /// <param name="IV"></param>
        /// <returns></returns>
        private static string Decrypt(byte[] cipherText, byte[] Key, byte[] IV)
        {
            string plaintext = null;
            // Create AesManaged    
            using (var aes = new AesManaged())
            {
                // Create a decryptor    
                var decryptor = aes.CreateDecryptor(Key, IV);
                // Create the streams used for decryption.    
                using (var ms = new MemoryStream(cipherText))
                {
                    // Create crypto stream    
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        // Read crypto stream    
                        using (var reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }



    }
}