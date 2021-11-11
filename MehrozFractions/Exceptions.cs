﻿using System;

namespace MehrozFractions
{
    /// <summary>
    ///     Exception class for Fraction, derived from System.Exception
    /// </summary>
    public class FractionException : Exception
    {
        /// <summary>
        ///     Constructs a FractionException
        /// </summary>
        /// <param name="Message">String associated with the error message</param>
        /// <param name="InnerException">Actual inner exception caught</param>
        public FractionException(string Message, Exception InnerException) : base(Message, InnerException) { }
    }
}