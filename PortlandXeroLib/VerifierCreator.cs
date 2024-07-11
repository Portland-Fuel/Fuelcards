using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PortlandXeroLib
{
    public class VerifierCreator
    {
        private readonly string _verifier;
        private readonly string _codeChallenge;
        public string Verifier { get { return _verifier; } }
        public string CodeChallenge { get { return _codeChallenge; } }

        public VerifierCreator()
        {
            // Generate a random 50 character string
            _verifier = EncodeTo64(RandomString(50));
            _codeChallenge = HashTheVerifier();
        }

        public VerifierCreator(int stringLength)
        {
            // Generate a random 50 character string
            _verifier = EncodeTo64(RandomString(stringLength));
        }

        private string HashTheVerifier()
        {
            //generate the code challenge based on the verifier
            string codeChallenge;
            using (var sha256 = SHA256.Create())
            {
                var challengeBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(_verifier));
                codeChallenge = Convert.ToBase64String(challengeBytes)
                    .TrimEnd('=')
                    .Replace('+', '-')
                    .Replace('/', '_');
            }
            return codeChallenge;
        }

        public string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz-._~";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        static public string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue = Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }
    }
}
