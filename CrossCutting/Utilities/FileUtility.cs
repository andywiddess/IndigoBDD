using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using System.Security.AccessControl;

namespace Indigo.CrossCutting.Utilities.Utility
{
    public class FileUtility
    {
        /// <summary>
        /// Reads the stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static byte[] ReadStream(Stream stream)
        {
            try
            {
                stream.Position = 0;
            }
            catch
            {
            }

            byte[] readBuffer = new byte[1024];
            List<byte> outputBytes = new List<byte>();

            int offset = 0;
            while (true)
            {
                int bytesRead = stream.Read(readBuffer, 0, readBuffer.Length);
                if (bytesRead == 0)
                {
                    break;
                }
                else if (bytesRead == readBuffer.Length)
                {
                    outputBytes.AddRange(readBuffer);
                }
                else
                {
                    byte[] tempBuf = new byte[bytesRead];
                    Array.Copy(readBuffer, tempBuf, bytesRead);
                    outputBytes.AddRange(tempBuf);
                    break;
                }
                offset += bytesRead;
            }
            return outputBytes.ToArray();
        }

        /// <summary>
        /// Gets the type of the content.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static string GetContentType(string fileName)
        {
            string mime = "application/octetstream";
            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(Path.GetExtension(fileName));
            if (rk != null && rk.GetValue("Content Type") != null)
                mime = rk.GetValue("Content Type").ToString();

            return mime;
        }

        /// <summary>
        /// Copies the directory.
        /// </summary>
        /// <param name="Src">The SRC.</param>
        /// <param name="Dst">The DST.</param>
        public static void CopyDirectory(string Src, string Dst)
        {
            String[] Files;

            if (!Directory.Exists(Dst)) Directory.CreateDirectory(Dst);

            if (Dst[Dst.Length - 1] != Path.DirectorySeparatorChar)
                Dst += Path.DirectorySeparatorChar;

            Files = Directory.GetFileSystemEntries(Src);
            foreach (string Element in Files)
            {
                // Sub directories

                if (Directory.Exists(Element))
                    CopyDirectory(Element, Dst + Path.GetFileName(Element));
                // Files in directory

                else
                    File.Copy(Element, Dst + Path.GetFileName(Element), true);
            }
        }

        /// <summary>
        /// Checks the access right.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static bool CheckAccessRight(string path, FileSystemRights right)
        {
            if (Path.HasExtension(path))
            {
                var dirInfo = new FileInfo(path);
                return CheckAccessRight(dirInfo, right);
            }
            else
            {
                var dirInfo = new DirectoryInfo(path);
                return CheckAccessRight(dirInfo, right);
            }
        }

        /// <summary>
        /// Checks the access right.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static bool CheckAccessRight(FileInfo file, FileSystemRights right)
        {
            var user = WindowsIdentity.GetCurrent();
            var p = new WindowsPrincipal(user);
            AuthorizationRuleCollection acl =
            file.GetAccessControl().GetAccessRules(true, true, typeof(System.Security.Principal.SecurityIdentifier));
            return CheckAccessRight(user, p, acl, right);
        }
        /// <summary>
        /// Checks the access right.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static bool CheckAccessRight(DirectoryInfo directory, FileSystemRights right)
        {
            var user = WindowsIdentity.GetCurrent();
            var p = new WindowsPrincipal(user);
            AuthorizationRuleCollection acl =
            directory.GetAccessControl().GetAccessRules(true, true, typeof(System.Security.Principal.SecurityIdentifier));
            return CheckAccessRight(user, p, acl, right);
        }

        /// <summary>
        /// Checks the access right.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="acl">The acl.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static bool CheckAccessRight(WindowsIdentity user, WindowsPrincipal principal, AuthorizationRuleCollection acl, FileSystemRights right)
        {
            // These are set to true if either the allow or deny  access rights are set
            bool allow = false;
            bool deny = false;

            for (int x = 0; x < acl.Count; x++)
            {
                FileSystemAccessRule currentRule = (FileSystemAccessRule)acl[x];
                // If the current rule applies to the current user
                if (user.User.Equals(currentRule.IdentityReference) || principal.IsInRole((SecurityIdentifier)currentRule.IdentityReference))
                {
                    if
                    (currentRule.AccessControlType.Equals(AccessControlType.Deny))
                    {
                        if ((currentRule.FileSystemRights & right) == right)
                        {
                            deny = true;
                        }
                    }
                    else if
                    (currentRule.AccessControlType.Equals(AccessControlType.Allow))
                    {
                        if ((currentRule.FileSystemRights & right) == right)
                        {
                            allow = true;
                        }
                    }
                }
            }

            return (allow & !deny);
        }
    }
}