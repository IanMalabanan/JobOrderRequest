using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace ClsCommon
{
    public class ClsGenerateRandomString
    {
        public string RandomAlphanumericString(int Size)
        {
            Random random = new Random();

            string input = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789abcdefghijklmnopqrstuvwxyz";

            StringBuilder builder = new StringBuilder();

            char ch;

            for (int i = 1; i <= Size; i++)
            {
                ch = input[random.Next(1, input.Length)];

                builder.Append(ch);
            }

            return builder.ToString();
        }

        public string GenerateRandomCode(int beginnum, int endnum)
        {
            string code = string.Empty;

            Random r = new Random();

            int rInt = r.Next(beginnum, endnum);

            code = RandomAlphanumericString(rInt);

            return code;
        }

    }
}
