using System;
using System.Net;

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
        public static void ThrowIfArgumentIsNull(object obj, string message, string argumentName)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(message, argumentName);
            }
        }
    }
}
