using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Verse;

public class LoadableXmlAsset
{
	public readonly string name;

	public readonly string fullFolderPath;

	public readonly XmlDocument xmlDoc;

	public readonly ModContentPack mod;

	private const int BufferSize = 16000;

	private static readonly XmlReaderSettings Settings = new XmlReaderSettings
	{
		IgnoreComments = true,
		IgnoreWhitespace = true,
		CheckCharacters = false,
		Async = false
	};

	public string FullFilePath => fullFolderPath + Path.DirectorySeparatorChar + name;

	public LoadableXmlAsset(string name, string text)
	{
		this.name = name;
		fullFolderPath = string.Empty;
		try
		{
			using StringReader input = new StringReader(text);
			using XmlReader reader = XmlReader.Create(input, Settings);
			xmlDoc = new XmlDocument();
			xmlDoc.Load(reader);
		}
		catch (Exception value)
		{
			Log.Warning($"Exception reading {name} as XML: {value}");
			xmlDoc = null;
		}
	}

	public LoadableXmlAsset(FileInfo file, ModContentPack mod)
	{
		this.mod = mod;
		name = file.Name;
		fullFolderPath = file.Directory.FullName;
		try
		{
			using MemoryMappedFileSpanWrapper memoryMappedFileSpanWrapper = new MemoryMappedFileSpanWrapper(file);
			ReadOnlySpan<byte> bytes = memoryMappedFileSpanWrapper.GetReadOnlySpan(0L, memoryMappedFileSpanWrapper.FileSize);
			if (bytes.Length >= 3 && bytes[0] == 239 && bytes[1] == 187 && bytes[2] == 191)
			{
				bytes = bytes.Slice(3);
			}
			using StringReader input = new StringReader(Encoding.UTF8.GetString(bytes));
			using XmlReader reader = XmlReader.Create(input, Settings);
			xmlDoc = new XmlDocument();
			xmlDoc.Load(reader);
		}
		catch (Exception value)
		{
			Log.Warning($"Exception reading {name} as XML: {value}");
			xmlDoc = null;
		}
	}

	public override string ToString()
	{
		return name;
	}
}
