using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClsCommon
{
    public class ClsBytesToBase64_Base64ToBytes
    {
        public string BytesToBase64(byte[] imagebyte)
        {
            string code = string.Empty;

            code = Convert.ToBase64String(imagebyte, 0, imagebyte.Length);

            return code;
        }

        public byte[] Base64ToByte(string base64string)
        {
            byte[] img = Convert.FromBase64String(base64string); ;
                        
            return img;
        }
    }
}
