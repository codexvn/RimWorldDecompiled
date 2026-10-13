using System;
using System.Text;

namespace UnityEngine.Networking;

public class MultipartFormFileSection : IMultipartFormSection
{
	private string name;

	private byte[] data;

	private string file;

	private string content;

	public string sectionName => name;

	public byte[] sectionData => data;

	public string fileName => file;

	public string contentType => content;

	private void Init(string name, byte[] data, string fileName, string contentType)
	{
		this.name = name;
		this.data = data;
		file = fileName;
		content = contentType;
	}

	public MultipartFormFileSection(string name, byte[] data, string fileName, string contentType)
	{
		if (data == null || data.Length < 1)
		{
			throw new ArgumentException("Cannot create a multipart form file section without body data");
		}
		if (string.IsNullOrEmpty(fileName))
		{
			fileName = "file.dat";
		}
		if (string.IsNullOrEmpty(contentType))
		{
			contentType = "application/octet-stream";
		}
		Init(name, data, fileName, contentType);
	}

	public MultipartFormFileSection(byte[] data)
		: this(null, data, null, null)
	{
	}

	public MultipartFormFileSection(string fileName, byte[] data)
		: this(null, data, fileName, null)
	{
	}

	public MultipartFormFileSection(string name, string data, Encoding dataEncoding, string fileName)
	{
		if (string.IsNullOrEmpty(data))
		{
			throw new ArgumentException("Cannot create a multipart form file section without body data");
		}
		if (dataEncoding == null)
		{
			dataEncoding = Encoding.UTF8;
		}
		if (string.IsNullOrEmpty(fileName))
		{
			fileName = "file.txt";
		}
		Init(name, dataEncoding.GetBytes(data), fileName, "text/plain; charset=" + dataEncoding.WebName);
	}

	public MultipartFormFileSection(string data, Encoding dataEncoding, string fileName)
		: this(null, data, dataEncoding, fileName)
	{
	}

	public MultipartFormFileSection(string data, string fileName)
		: this(data, null, fileName)
	{
	}
}
