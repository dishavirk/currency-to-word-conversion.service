using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace currency_to_word_conversion.service
{
    public class ConversionService : IConversionService
    {
        public string convertNumbersToWords(string num)
        {
            num = num.Replace(" ", string.Empty);
            bool isValid = validateInput(num);

            if (isValid)
            {
                try {
                    string[] decimalSeparated = num.Split(',');

                    string dollars = String.IsNullOrEmpty(decimalSeparated[0]) ? "" : NumberToWords(decimalSeparated[0]);

                    string cents = "";
                    if (decimalSeparated.Length > 1) {
                        string trailingzero =  decimalSeparated[1].Length == 1 ? "0" : "";
                        cents = String.IsNullOrEmpty(decimalSeparated[1]) ? "" : NumberToWords("0" + decimalSeparated[1] + trailingzero);
                    }

                    if (String.IsNullOrEmpty(dollars + cents))
                        return "zero";

                    return dollars + (String.IsNullOrEmpty(dollars) ? "" : "dollars ") + cents + (String.IsNullOrEmpty(cents) ? "" : "cents");
                }
                catch (Exception ex) {
                    throw new FaultException<UnexpectedServiceFault>(
                       new UnexpectedServiceFault
                       {
                           Message = ex.Message,
                           Description = ex.InnerException.ToString(),
                           Result = "fail"
                       },
                       new FaultReason(string.Format(CultureInfo.InvariantCulture,
                       "{0}", "Service fault exception")));
                }
            }
            else {
                throw new FaultException<ValidationFault>(
                   new ValidationFault
                   {
                       Message = "Invalid input",
                       Description = "Invalid input provided by the user",
                       Result = "fail"
                   },
                   new FaultReason(string.Format(CultureInfo.InvariantCulture,
                   "{0}", "Service fault exception")));
            }
        }

        public static bool validateInput(string input)
        {
            Regex reg = new Regex("^[0-9]*[,]?[0-9]{0,2}$");
            if (String.IsNullOrEmpty(input) ||
            !reg.IsMatch(input) ||
            input.Equals(",") ||
            input.Split(',')[0].Length > 30 ||
            (input.Split(',').Length > 1 && input.Split(',')[1].Length > 2))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static string NumberToWords(string numInput)
        {
            int numLength = numInput.Length;
            int lengthWithPadding = numLength % 3 > 0 ? ((numLength / 3) + 1) * 3 : numLength;
            string numInputWithPadding = numInput.PadLeft(lengthWithPadding, '0');

            int tripletPairLength = (int)Math.Ceiling((Decimal)(numInputWithPadding.Length / 3));
            string numOutput = "";
            for (int i = 0; i < tripletPairLength; i++)
            {
                string multiplier = GetMultipliers(tripletPairLength - i - 1);
                string triplet = TripletToWord(numInputWithPadding.Substring(i * 3, 3));
                numOutput = numOutput + triplet + (String.IsNullOrEmpty(triplet) ? "" : multiplier);
            }

            return numOutput;
        }

        public static string TripletToWord(string input)
        {
            int triplet = Int16.Parse(input);

            int remainder100 = triplet % 100;
            int quotient100 = (int)Math.Floor((Decimal)(triplet / 100));
            int remainder10 = remainder100 % 10;
            int quotient10 = (int)Math.Floor((Decimal)(remainder100 / 10));

            string tens = remainder100 > 19 ?
                            GetNumberString(quotient10 * 10) + GetNumberString(remainder10) :
                            GetNumberString(remainder100);
            string hundreds = GetNumberString(quotient100);

            return hundreds + (String.IsNullOrEmpty(hundreds) ? "" : "hundred ") + tens;
        }

        public static string GetNumberString(int num)
        {
            string output = "";
            switch (num)
            {
                case 1:
                    output = "one ";
                    break;
                case 2:
                    output = "two ";
                    break;
                case 3:
                    output = "three ";
                    break;
                case 4:
                    output = "four ";
                    break;
                case 5:
                    output = "five ";
                    break;
                case 6:
                    output = "six ";
                    break;
                case 7:
                    output = "seven ";
                    break;
                case 8:
                    output = "eight ";
                    break;
                case 9:
                    output = "nine ";
                    break;
                case 10:
                    output = "ten ";
                    break;
                case 11:
                    output = "eleven ";
                    break;
                case 12:
                    output = "twelve ";
                    break;
                case 13:
                    output = "thirteen ";
                    break;
                case 14:
                    output = "fourteen ";
                    break;
                case 15:
                    output = "fifteen ";
                    break;
                case 16:
                    output = "sixteen ";
                    break;
                case 17:
                    output = "seventeen ";
                    break;
                case 18:
                    output = "eighteen ";
                    break;
                case 19:
                    output = "nineteen ";
                    break;
                case 20:
                    output = "twenty-";
                    break;
                case 30:
                    output = "thirty-";
                    break;
                case 40:
                    output = "forty-";
                    break;
                case 50:
                    output = "fifty-";
                    break;
                case 60:
                    output = "sixty-";
                    break;
                case 70:
                    output = "seventy-";
                    break;
                case 80:
                    output = "eighty-";
                    break;
                case 90:
                    output = "ninety-";
                    break;
                default:
                    output = "";
                    break;
            }

            return output;
        }

        public static string GetMultipliers(int num)
        {

            string output = "";

            switch (num)
            {
                case 1:
                    output = "thousand ";
                    break;
                case 2:
                    output = "million ";
                    break;
                case 3:
                    output = "billion ";
                    break;
                case 4:
                    output = "trillion ";
                    break;
                case 5:
                    output = "quadrillion ";
                    break;
                case 6:
                    output = "quintillion ";
                    break;
                case 7:
                    output = "sextillion ";
                    break;
                case 8:
                    output = "septillion ";
                    break;
                case 9:
                    output = "octillion ";
                    break;
                default:
                    break;

            }

            return output;
        }
    }
}
