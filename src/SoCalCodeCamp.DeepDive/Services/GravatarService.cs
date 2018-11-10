using System.Security.Cryptography;
using System.Text;

namespace SoCalCodeCamp.DeepDive.Services
{
    public class GravatarService : IGravatarService
    {
        private const string RequestUri = "https://www.gravatar.com/avatar/{0}?s=80&d=robohash";

        public string GetGravatarUri(string email) => string.Format(RequestUri, GetMd5Hash(email));

        private string GetMd5Hash(string str)
        {
            byte[] hash = null;
            using (MD5 md5 = MD5.Create())
            {
                hash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            }

            var sBuilder = new StringBuilder();

            if (hash != null)
            {
                for (int i = 0; i < hash.Length; i++)
                {
                    sBuilder.Append(hash[i].ToString("x2"));
                }
            }

            return sBuilder.ToString();
        }
    }
}
