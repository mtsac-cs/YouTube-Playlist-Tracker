using System;

namespace GiveBack_Hackathon.Lib
{
    public static class Guard
    {
        /// <summary>
        /// Check if an object is null and throw an Argument exception if it is
        /// </summary>
        /// <param name="obj">Object to check if null</param>
        public static void ThrowIfArgumentIsNull(object obj, string message, string argumentName)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(message, argumentName);
            }
        }


        public static void ThrowIfStringNullOrEmpty(string str, string message)
        {
            if (String.IsNullOrEmpty(str))
                throw new Exception(message);
        }
    }
}
