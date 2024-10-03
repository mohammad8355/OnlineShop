using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.DiscountCodeGenerator;

namespace Utility.ProductCodeGenerator
{
    public class ProductCodeGenerator
    {
        public ProductCodeGenerator() { }

        /// <summary>
        /// if you add speceficString to your code ,it is added to the first of th code and if you set
        /// HaveDash true between the speceficString and the random code be a dash  
        /// the example of code  === > LionComputer-1234fghk*
        /// </summary>
        /// <param name="speceficString"></param>
        /// <param name="haveRandomChar"></param>
        /// <param name="StringRandomlength"></param>
        /// <param name="SpecificChar"></param>
        /// <param name="countOfNumber"></param>
        /// <param name="HaveDash"></param>
        /// <returns>a random product code generator that you can change configs base on the params</returns>
        public string CodeGenerator(string speceficString, bool haveRandomChar,int StringRandomlength, bool SpecificChar,int countOfNumber, bool HaveDash)
        {
            StringBuilder code = new StringBuilder();
            if (!String.IsNullOrEmpty(speceficString))
            {
                code.Append(speceficString);
            }
            if (HaveDash)
            {
                code.Append("-");
            }
            if(countOfNumber != 0)
            {
                code.Append(RandomNumberGenerator(countOfNumber));
            }
            if (haveRandomChar)
            {
                code.Append(RandomCharachterGenerator(StringRandomlength,SpecificChar));
            }
            return code.ToString();
        }
        public string RandomNumberGenerator(int count)
        {
            Random random = new Random();
            StringBuilder code = new StringBuilder();
            for (int i = 0; i < count; i++)
            {

                code.Append(random.Next(1, 9));
            }
            return code.ToString();
        }
        public string RandomCharachterGenerator(int length,bool haveSpecial)
        {
            Random random = new Random();
            var charachterwidthSpecific = "ABCDEFGHIJKLMNOPQRSTUVWXYZ*%$#@!^";
            var charachter = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            StringBuilder code = new StringBuilder();
            if (haveSpecial)
            {
                for (int i = 0; i < length; i++)
                {
                    var index = random.Next(charachterwidthSpecific.Length);
                    code.Append(charachter[index]);
                }
                return code.ToString();
            }
            else
            {
                for (int i = 0; i < length; i++)
                {
                    var index = random.Next(charachter.Length);
                    code.Append(charachter[index]);
                }
                return code.ToString();
            }
        }
    }
}

