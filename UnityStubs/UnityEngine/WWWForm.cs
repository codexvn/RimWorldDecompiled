using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace UnityEngine;

public class WWWForm
{
	private const string BoundaryChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

	private readonly List<byte[]> formData = new List<byte[]>();

	private readonly List<string> fieldNames = new List<string>();

	private readonly List<string> fileNames = new List<string>();

	private readonly List<string> types = new List<string>();

	private readonly string boundary;

	private bool containsFiles;

	public Dictionary<string, string> headers => new Dictionary<string, string> { ["Content-Type"] = (containsFiles ? ("multipart/form-data; boundary=\"" + boundary + "\"") : "application/x-www-form-urlencoded") };

	public byte[] data
	{
		get
		{
			if (!containsFiles)
			{
				return UrlEncodedBody();
			}
			return MultipartBody();
		}
	}

	public WWWForm()
	{
		char[] array = new char[40];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"[System.Random.Shared.Next("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".Length)];
		}
		boundary = new string(array);
	}

	public void AddField(string fieldName, string value)
	{
		AddField(fieldName, value, Encoding.UTF8);
	}

	public void AddField(string fieldName, int i)
	{
		AddField(fieldName, i.ToString());
	}

	public void AddField(string fieldName, string value, Encoding e)
	{
		fieldNames.Add(fieldName);
		fileNames.Add(null);
		formData.Add(e.GetBytes(value));
		types.Add("text/plain; charset=\"" + e.WebName + "\"");
	}

	public void AddBinaryData(string fieldName, byte[] contents)
	{
		AddBinaryData(fieldName, contents, null, null);
	}

	public void AddBinaryData(string fieldName, byte[] contents, string fileName)
	{
		AddBinaryData(fieldName, contents, fileName, null);
	}

	public void AddBinaryData(string fieldName, byte[] contents, string fileName, string mimeType)
	{
		containsFiles = true;
		fieldNames.Add(fieldName);
		fileNames.Add(fileName ?? (fieldName + ".dat"));
		formData.Add(contents);
		types.Add(mimeType ?? "application/octet-stream");
	}

	private byte[] UrlEncodedBody()
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < formData.Count; i++)
		{
			if (i > 0)
			{
				stringBuilder.Append('&');
			}
			stringBuilder.Append(WebUtility.UrlEncode(fieldNames[i])).Append('=').Append(Encoding.ASCII.GetString(WebUtility.UrlEncodeToBytes(formData[i], 0, formData[i].Length)));
		}
		return Encoding.UTF8.GetBytes(stringBuilder.ToString());
	}

	private byte[] MultipartBody()
	{
		MemoryStream body = new MemoryStream();
		try
		{
			for (int i = 0; i < formData.Count; i++)
			{
				Write("--" + boundary + "\r\n");
				Write("Content-Type: " + types[i] + "\r\n");
				Write("Content-disposition: form-data; name=\"" + fieldNames[i] + "\"");
				if (fileNames[i] != null)
				{
					Write("; filename=\"" + fileNames[i] + "\"");
				}
				Write("\r\n\r\n");
				body.Write(formData[i]);
				Write("\r\n");
			}
			Write("--" + boundary + "--");
			return body.ToArray();
		}
		finally
		{
			if (body != null)
			{
				((IDisposable)body).Dispose();
			}
		}
		void Write(string text)
		{
			body.Write(Encoding.UTF8.GetBytes(text));
		}
	}
}
