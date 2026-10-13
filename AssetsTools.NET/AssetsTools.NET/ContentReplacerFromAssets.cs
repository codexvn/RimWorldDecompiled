using System.IO;
using AssetsTools.NET.Extra;

namespace AssetsTools.NET;

public class ContentReplacerFromAssets : IContentReplacer
{
	public AssetsFile file;

	public ContentReplacerFromAssets(AssetsFile file)
	{
		this.file = file;
	}

	public ContentReplacerFromAssets(AssetsFileInstance inst)
	{
		file = inst.file;
	}

	public void Write(AssetsFileWriter writer)
	{
		AssetsFileWriter writer2 = new AssetsFileWriter(new SegmentStream(writer.BaseStream, writer.Position));
		file.Write(writer2, -1L);
		writer.Position = writer.BaseStream.Length;
	}

	public bool HasPreview()
	{
		return false;
	}

	public Stream GetPreviewStream()
	{
		return null;
	}

	public ContentReplacerType GetReplacerType()
	{
		return ContentReplacerType.AddOrModify;
	}
}
