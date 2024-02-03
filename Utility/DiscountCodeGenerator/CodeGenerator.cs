using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.DiscountCodeGenerator
{
    public class CodeGenerator
    {
        public string GenerateDiscountCode(int length)
        {
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder code = new StringBuilder();

            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                int index = random.Next(characters.Length);
                code.Append(characters[index]);
            }

            return code.ToString();
        }
    }
}
