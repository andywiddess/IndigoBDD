﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Indigo.CrossCutting.Utilities.Utility
{
    public static class PGen
    {
        #region Statics
        /// <summary>
        /// Define default password length.
        /// </summary>
        private static int DEFAULT_PASSWORD_LENGTH = 8;

        /// <summary>
        /// No characters that are confusing: i, I, l, L, o, O, 0, 1, u, v
        /// </summary>
        public static string PASSWORD_CHARS_ALPHA = "abcdefghjkmnpqrstwxyzABCDEFGHJKMNPQRSTWXYZ";

        /// <summary>
        /// Numbers
        /// </summary>
        public static string PASSWORD_CHARS_NUMERIC = "23456789";

        /// <summary>
        /// Special Characters
        /// </summary>
        public static string PASSWORD_CHARS_SPECIAL = "*$-+?_&=!%{}/";

        /// <summary>
        /// Accepted Characters
        /// </summary>
        public static string PASSWORD_CHARS_ALPHANUMERIC = PASSWORD_CHARS_ALPHA + PASSWORD_CHARS_NUMERIC;

        /// <summary>
        /// All Characters
        /// </summary>
        public static string PASSWORD_CHARS_ALL = PASSWORD_CHARS_ALPHANUMERIC + PASSWORD_CHARS_SPECIAL;
        #endregion

        #region Public Methods

        /// <summary>
        /// Generates a random password with the default length.
        /// </summary>
        /// <returns>Randomly generated password.</returns>
        public static string Generate()
        {
            return Generate(DEFAULT_PASSWORD_LENGTH, PASSWORD_CHARS_ALL);
        }

        /// <summary>
        /// Generates a random password with the default length.
        /// </summary>
        /// <returns>Randomly generated password.</returns>
        public static string Generate(string passwordChars)
        {
            return Generate(DEFAULT_PASSWORD_LENGTH, passwordChars);
        }

        /// <summary>
        /// Generates a random password with the default length.
        /// </summary>
        /// <returns>Randomly generated password.</returns>
        public static string Generate(int passwordLength)
        {
            return Generate(passwordLength, PASSWORD_CHARS_ALL);
        }

        /// <summary>
        /// Generates a random password.
        /// </summary>
        /// <returns>Randomly generated password.</returns>
        public static string Generate(int passwordLength, string passwordChars)
        {
            return GeneratePassword(passwordLength, passwordChars);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Generates the password.
        /// </summary>
        /// <returns></returns>
        private static string GeneratePassword(int passwordLength, string passwordCharacters)
        {
            if (passwordLength < 0)
                throw new ArgumentOutOfRangeException("Password Length");

            if (string.IsNullOrEmpty(passwordCharacters))
                throw new ArgumentOutOfRangeException("Password Characters");

            var password = new char[passwordLength];
            var random = GetRandom();

            for (int i = 0; i < passwordLength; i++)
                password[i] = passwordCharacters[random.Next(passwordCharacters.Length)];

            return new string(password);
        }

        /// <summary>
        /// Gets a random object with a real random seed
        /// </summary>
        /// <returns></returns>
        private static Random GetRandom()
        {
            // Use a 4-byte array to fill it with random bytes and convert it then
            // to an integer value.
            byte[] randomBytes = new byte[4];

            // Generate 4 random bytes.
            new RNGCryptoServiceProvider().GetBytes(randomBytes);

            // Convert 4 bytes into a 32-bit integer value.
            int seed = (randomBytes[0] & 0x7f) << 24 |
                        randomBytes[1] << 16 |
                        randomBytes[2] << 8 |
                        randomBytes[3];

            // Now, this is real randomization.
            return new Random(seed);
        }
        #endregion
    }
}