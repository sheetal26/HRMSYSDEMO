using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMSystem
{
    /// <summary>
    /// Summary description for ValueConvert
    /// </summary>
    public class ValueConvert
    {
        public ValueConvert()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        //public string ConvertDate(string input)
        //{

        //    DateTime ValidDate;
        //    //string strValidDate="";
        //    System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
        //    dateInfo.ShortDatePattern = "dd/MM/yyyy";

        //    try
        //    {
        //        ValidDate = Convert.ToDateTime(input, dateInfo);
        //        return ValidDate.ToString("MM/dd/yyyy");
        //    }
        //    catch (FormatException ex)
        //    {
        //        throw ex;
        //    }
        //}

        public static double ValDouble(string input)
        {
            double returnvalue = 0;
            bool IsNull = string.IsNullOrEmpty(input);
            if (IsNull == true)
            {
                returnvalue = 0;
            }
            else
            {
                double i;
                bool IsNumeric = double.TryParse(input, out i);
                if (IsNumeric == true)
                {
                    returnvalue = Convert.ToDouble(input);
                }
                else
                {
                    returnvalue = 0;
                }
            }
            return returnvalue;
        }

        public static Int16 ValInt16(string input)
        {
            Int16 returnvalue = 0;
            bool IsNull = string.IsNullOrEmpty(input);
            if (IsNull == true)
            {
                returnvalue = 0;
            }
            else
            {
                Int16 i;
                bool IsNumeric = Int16.TryParse(input, out i);
                if (IsNumeric == true)
                {
                    returnvalue = Convert.ToInt16(input);
                }
                else
                {
                    returnvalue = 0;
                }
            }
            return returnvalue;
        }

        public static Int32 ValInt32(string input)
        {
            Int32 returnvalue = 0;
            bool IsNull = string.IsNullOrEmpty(input);
            if (IsNull == true)
            {
                returnvalue = 0;
            }
            else
            {
                Int32 i;
                bool IsNumeric = Int32.TryParse(input, out i);
                if (IsNumeric == true)
                {
                    returnvalue = Convert.ToInt32(input);
                }
                else
                {
                    returnvalue = 0;
                }
            }
            return returnvalue;
        }

        public static decimal ValDecimal(string input)
        {
            decimal returnvalue = 0;
            bool IsNull = string.IsNullOrEmpty(input);
            if (IsNull == true)
            {
                returnvalue = 0;
            }
            else
            {
                decimal i;
                bool IsNumeric = Decimal.TryParse(input, out i);
                if (IsNumeric == true)
                {
                    returnvalue = Convert.ToDecimal(input);
                }
                else
                {
                    returnvalue = 0;
                }
            }
            return returnvalue;
        }

        public static double Val(string input)
        {
            double returnvalue = 0;
            bool IsNull = string.IsNullOrEmpty(input);
            if (IsNull == true)
            {
                returnvalue = 0;
            }
            else
            {
                double i;
                bool IsNumeric = double.TryParse(input, out i);
                if (IsNumeric == true)
                {
                    returnvalue = Convert.ToDouble(input);
                }
                else
                {
                    returnvalue = 0;
                }
            }
            return returnvalue;
        }

        public static bool IsValidDate(string input)
        {
            bool IsValid = false;

            System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
            dateInfo.ShortDatePattern = "dd/MM/yyyy";

            try
            {
                DateTime ValidDate = Convert.ToDateTime(input, dateInfo);
                IsValid = true;
            }
            catch (FormatException)
            {
                IsValid = false;
            }

            return IsValid;
        }

        public static string ConvertDate(string input)
        {

            DateTime ValidDate;
            //string strValidDate="";
            System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
            dateInfo.ShortDatePattern = "dd/MM/yyyy";

            try
            {
                ValidDate = Convert.ToDateTime(input, dateInfo);
                return ValidDate.ToString("MM/dd/yyyy");
            }
            catch (FormatException fx)
            {
                throw fx;
            }

        }

        public static DateTime ToDate(string input)
        {

            DateTime ValidDate;
            //string strValidDate="";
            System.Globalization.DateTimeFormatInfo dateInfo = new System.Globalization.DateTimeFormatInfo();
            dateInfo.ShortDatePattern = "dd/MM/yyyy";
            // dateInfo.ShortDatePattern = "MM/dd/yyyy";

            try
            {
                ValidDate = Convert.ToDateTime(input, dateInfo);
                return ValidDate;
            }
            catch (FormatException fx)
            {
                throw fx;
            }

        }

        public static string ToPositive(string input)
        {
            double returnvalue;
            bool IsNull = string.IsNullOrEmpty(input);
            if (IsNull == true)
            {
                returnvalue = 0;
            }
            else
            {
                double i;
                bool IsNumeric = double.TryParse(input, out i);
                if (IsNumeric == true)
                {
                    returnvalue = Math.Abs(Convert.ToDouble(input));
                }
                else
                {
                    returnvalue = 0;
                }
            }
            return returnvalue.ToString();
        }
    }
}