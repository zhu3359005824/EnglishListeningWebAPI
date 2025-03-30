using System.Security.Cryptography;
using System.Text;

namespace ZHZ.Tools
{
    public static class HashHelper
    {
        public static string ComputeSHA256HashUsingStream(Stream stream)
        {
            // 创建SHA256哈希算法实例
            using (SHA256 sha256 = SHA256.Create())
            {
                // 计算哈希值
                byte[] hashBytes = sha256.ComputeHash(stream);

                // 将哈希值字节数组转换为十六进制字符串
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2")); // "x2"表示以两位十六进制格式输出
                }

                return sb.ToString();
            }
        }


        private static string ToHashString(byte[] bytes)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }

        public static string ComputeSha256Hash(Stream stream)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(stream);
                return ToHashString(bytes);
            }
        }

        public static string ComputeSha256Hash(string input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                return ToHashString(bytes);
            }
        }

        public static string ComputeMd5Hash(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] bytes = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                return ToHashString(bytes);
            }
        }

        public static string ComputeMd5Hash(Stream input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] bytes = md5Hash.ComputeHash(input);
                return ToHashString(bytes);
            }
        }

    }
}
