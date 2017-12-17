using System;
using System.Collections.Generic;
using System.Net;

namespace Indigo.CrossCutting.Utilities.Extensions
{
	public static class ExtensionsToUri
	{
		/// <summary>
		///   Appends a path to an existing Uri
		/// </summary>
		/// <param name = "uri"></param>
		/// <param name = "path"></param>
		/// <returns></returns>
		public static Uri AppendPath(this Uri uri, string path)
		{
			string absolutePath = uri.AbsolutePath.TrimEnd('/') + "/" + path;
			return new UriBuilder(uri.Scheme, uri.Host, uri.Port, absolutePath, uri.Query).Uri;
		}

		public static IEnumerable<IPEndPoint> ResolveHostName(this Uri uri)
		{
			if (uri.HostNameType == UriHostNameType.Dns)
			{
				IPAddress[] addresses = Dns.GetHostAddresses(uri.DnsSafeHost);
				if (addresses.Length == 0)
					throw new ArgumentException("The host could not be resolved: " + uri.DnsSafeHost, "uri");

				foreach (IPAddress address in addresses)
				{
					var endpoint = new IPEndPoint(address, uri.Port);
					yield return endpoint;
				}
			}
			else if (uri.HostNameType == UriHostNameType.IPv4)
			{
				IPAddress address = IPAddress.Parse(uri.Host);
				if (address == null)
					throw new ArgumentException("The IP address is invalid: " + uri.Host, "uri");

				var endpoint = new IPEndPoint(address, uri.Port);
				yield return endpoint;
			}
			else
				throw new ArgumentException("Could not determine host name type: " + uri.Host, "uri");
		}
	}
}