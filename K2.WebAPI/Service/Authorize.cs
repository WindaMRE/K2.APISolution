using Dapper;
using K2.WebAPI.Models;
using K2.WebAPI.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Web;

namespace K2.WebAPI.Service
{
    public class Authorize
    {
        public Authorize()
        { }
        public static string Token(string wsuid, string wspass)
        {
            return Md5(wsuid + wspass + DateTime.Now.ToString("dd/MM/yyyy"));
        }
        public static string Decrypt(string cipher)
        {
            string key = "0cc0f505308d0c301d04305d06b0ab01405c0920400410d8";
            using (var md5 = new MD5CryptoServiceProvider())
            {
                using (var tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    using (var transform = tdes.CreateDecryptor())
                    {
                        byte[] cipherBytes = Convert.FromBase64String(cipher);
                        byte[] bytes = transform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                        return UTF8Encoding.UTF8.GetString(bytes);
                    }
                }
            }
        }

        public static string Md5(string str)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(Decrypt(str));
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x3"));
            }
            return sb.ToString();
        }
        public static bool GetAuthorize(string username, string passwordParam)
        {
            Repository test = new Repository();
            DynamicParameters Dparam = new();
            try
            {
                Dparam.Add("@Username", username);
                var data = test.DynamicParamQueryAsync<Users>(@"sp_tbl_AuthenticAPI_CheckUser_Getrows", Dparam, null, true);

                if (data.Result.List.Count > 0)
                {
                    var passwordCrypt = Md5(data.Result.List[0].Password).ToString();
                    if (passwordParam == passwordCrypt)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Validate(string vldprm1, string vldprm2, string vldprm3, string RoleCode)
        {
            if (GetAuthorize(vldprm1, vldprm2) && vldprm3 == Token(vldprm1, vldprm2))
                return true;
            else
                return false;
        }
    }
}