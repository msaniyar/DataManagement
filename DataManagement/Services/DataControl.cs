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
            post.Tree = Decrypt(post.Tree, secret);
            await _db.TreeListTable.AddAsync(post);
            await _db.SaveChangesAsync();

            return post.GetHashCode();

        }


        public static string Decrypt(string value, string password)
        {
            return Decrypt<AesManaged>(value, password);
        }
        public static string Decrypt<T>(string value, string password) where T : SymmetricAlgorithm, new()
        {

            var vectorBytes = Encoding.UTF8.GetBytes(Startup.StaticConfig.GetSection("AppConfiguration")["vector"]);
            var saltBytes = Encoding.UTF8.GetBytes(Startup.StaticConfig.GetSection("AppConfiguration")["salt"]);
            var valueBytes = Encoding.UTF8.GetBytes(value); 
            var iterations = Startup.StaticConfig.GetSection("AppConfiguration")["iteration"];
            var keySize = Startup.StaticConfig.GetSection("AppConfiguration")["keysize"];


            byte[] decrypted;
            var passwordBytes =
                new Rfc2898DeriveBytes(password, saltBytes, int.Parse(iterations));
            int decryptedByteCount = 0;


            using (T cipher = new T())
            {
                var keyBytes = passwordBytes.GetBytes(int.Parse(keySize) / 8);

                cipher.Mode = CipherMode.CBC;
                cipher.Padding = PaddingMode.PKCS7;

                try
                {
                    using (ICryptoTransform decryptor = cipher.CreateDecryptor(keyBytes, vectorBytes))
                    {
                        using (MemoryStream from = new MemoryStream(valueBytes))
                        {
                            using (CryptoStream reader = new CryptoStream(from, decryptor, CryptoStreamMode.Read))
                            {
                                decrypted = new byte[valueBytes.Length];
                                decryptedByteCount = reader.Read(decrypted, 0, decrypted.Length);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return String.Empty;
                }

                cipher.Clear();
            }
            return Encoding.UTF8.GetString(decrypted, 0, decryptedByteCount);
        }



    }
}