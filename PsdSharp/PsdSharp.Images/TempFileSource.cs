using System.IO;
using PsdSharp.Parsing;

namespace PsdSharp.Images;

internal class TempFileSource : ImageDataSource
{
	private readonly FileInfo _tempFile;

	public TempFileSource(BigEndianReader reader, long? dataLength = null)
	{
		string tempFileName = Path.GetTempFileName();
		_tempFile = new FileInfo(tempFileName);
		FileStream fileStream = _tempFile.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
		if (dataLength.HasValue)
		{
			reader.CopyTo(fileStream, dataLength.Value);
		}
		else
		{
			reader.CopyTo(fileStream);
		}
		fileStream.Close();
	}

	public override Stream GetData()
	{
		return _tempFile.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
	}
}
