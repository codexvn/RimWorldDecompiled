using System.IO;

namespace AssetsTools.NET;

public interface IContentReplacer
{
	void Write(AssetsFileWriter writer);

	bool HasPreview();

	Stream GetPreviewStream();

	ContentReplacerType GetReplacerType();
}
