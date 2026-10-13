using System;
using System.IO;
using System.Text;
using Waylith.Assets;

namespace UnityEngine;

public class TextAsset : Object
{
	private byte[] _bytes = Array.Empty<byte>();

	private string _text = string.Empty;

	public byte[] bytes => _bytes;

	public string text => _text;

	public TextAsset()
	{
	}

	public TextAsset(string text)
	{
		_text = text ?? string.Empty;
		_bytes = Encoding.UTF8.GetBytes(_text);
	}

	internal static TextAsset LoadFromFile(string filePath)
	{
		if (!ContentPack.TryReadFile(filePath, out byte[] data))
		{
			data = File.ReadAllBytes(filePath);
		}
		return FromBytes(Path.GetFileNameWithoutExtension(filePath), data);
	}

	internal static TextAsset FromBytes(string name, byte[] bytes)
	{
		TextAsset textAsset = new TextAsset
		{
			name = (name ?? string.Empty)
		};
		if (bytes.Length > 3 && bytes[0] == 239 && bytes[1] == 187 && bytes[2] == 191)
		{
			bytes = bytes[3..];
		}
		textAsset._bytes = bytes ?? Array.Empty<byte>();
		textAsset._text = ((textAsset._bytes.Length == 0) ? string.Empty : Encoding.UTF8.GetString(textAsset._bytes));
		return textAsset;
	}
}
