using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using MimeKit;
using MimeKit.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;


namespace CRMAudax.Tools
{
    public class Converts
    {
		public static byte[] ConverteStreamToByteArray(Stream stream)
		{
			byte[] byteArray = new byte[16 * 1024];
			using (MemoryStream mStream = new MemoryStream())
			{
				int bit;
				while ((bit = stream.Read(byteArray, 0, byteArray.Length)) > 0)
				{
					mStream.Write(byteArray, 0, bit);
				}
				return mStream.ToArray();
			}
		}
	}
}
