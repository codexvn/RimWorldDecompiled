using RimWorld.IO;

namespace Verse;

public class LoadedContentItem<T> where T : class
{
	public VirtualFile internalFile;

	public T contentItem;

	public LoadedContentItem(VirtualFile internalFile, T contentItem)
	{
		this.internalFile = internalFile;
		this.contentItem = contentItem;
	}
}
