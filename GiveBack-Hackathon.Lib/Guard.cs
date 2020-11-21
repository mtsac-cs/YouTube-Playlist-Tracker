﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}