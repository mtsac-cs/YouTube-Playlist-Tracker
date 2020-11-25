using System;

namespace YouTube_Playlist_Tracker.Lib
{
    /// <summary>
    /// This class contians common checks used throught the code
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Check if an object is null and throw an Argument exception if it is
        /// </summary>
        /// <param name="obj">Object to check if null</param>
        public static void ThrowIfArgumentIsNull(object obj,  string argumentName, string message = "")
        {
            if (obj is null)
            {
                if (string.IsNullOrEmpty(message))
                    throw new ArgumentNullException(argumentName);
                else
                    throw new ArgumentNullException(argumentName, message);
            }
        }


        public static void ThrowIfStringIsNull(string str, string message)
        {
            if (String.IsNullOrEmpty(str))
            {
                throw new Exception(message);
            }
        }
    }
}
