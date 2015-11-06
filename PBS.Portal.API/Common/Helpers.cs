using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace PBS.Portal.API.Common
{
    public static  class Helpers
    {
        ///<summary>
        /// Base 64 Encoding with URL and Filename Safe Alphabet using UTF-8 character set.
        ///</summary>
        ///<param name="str">The original string</param>
        ///<returns>The Base64 encoded string</returns>
        public static string Base64ForUrlEncode(string str)
        {
            var byteArr = Encoding.UTF8.GetBytes(str);
            return HttpServerUtility.UrlTokenEncode(byteArr);
        }
        ///<summary>
        /// Decode Base64 encoded string with URL and Filename Safe Alphabet using UTF-8.
        ///</summary>
        ///<param name="str">Base64 code</param>
        ///<returns>The decoded string.</returns>
        public static string Base64ForUrlDecode(string str)
        {
            var byteArr = HttpServerUtility.UrlTokenDecode(str);
            return Encoding.UTF8.GetString(byteArr);
        }
    }
}