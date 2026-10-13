using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AssetsTools.NET.Extra;

public class AssetsFileInstance
{
	public string path;

	public string name;

	public AssetsFile file;

	public BundleFileInstance parentBundle;

	internal Dictionary<int, AssetsFileInstance> dependencyCache;

	public Stream AssetsStream => file.Reader.BaseStream;

	public AssetsFileInstance(Stream stream, string filePath)
	{
		path = Path.GetFullPath(filePath);
		name = Path.GetFileName(path);
		file = new AssetsFile();
		file.Read(new AssetsFileReader(stream));
		dependencyCache = new Dictionary<int, AssetsFileInstance>();
	}

	public AssetsFileInstance(FileStream stream)
	{
		path = stream.Name;
		name = Path.GetFileName(path);
		file = new AssetsFile();
		file.Read(new AssetsFileReader(stream));
		dependencyCache = new Dictionary<int, AssetsFileInstance>();
	}

	public AssetsFileInstance GetDependency(AssetsManager am, int depIdx)
	{
		if (!dependencyCache.ContainsKey(depIdx) || dependencyCache[depIdx] == null)
		{
			string depPath = file.Metadata.Externals[depIdx].PathName;
			if (depPath == string.Empty)
			{
				return null;
			}
			if (depPath.StartsWith("archive:/"))
			{
				depPath = depPath.Substring(depPath.IndexOf('/', "archive:/".Length) + 1);
			}
			if (!am.FileLookup.TryGetValue(am.GetFileLookupKey(depPath), out var value))
			{
				string directoryName = Path.GetDirectoryName(path);
				string text = Path.Combine(directoryName, depPath);
				string text2 = Path.Combine(directoryName, Path.GetFileName(depPath));
				if (File.Exists(text))
				{
					dependencyCache[depIdx] = am.LoadAssetsFile(text, loadDeps: true);
				}
				else if (File.Exists(text2))
				{
					dependencyCache[depIdx] = am.LoadAssetsFile(text2, loadDeps: true);
				}
				else
				{
					if (parentBundle == null)
					{
						return null;
					}
					if (parentBundle.file.BlockAndDirInfo.DirectoryInfos.Any((AssetBundleDirectoryInfo di) => di.Name == depPath))
					{
						dependencyCache[depIdx] = am.LoadAssetsFileFromBundle(parentBundle, depPath, loadDeps: true);
					}
					else
					{
						string text3 = Path.Combine(Path.GetDirectoryName(directoryName), Path.GetFileName(depPath));
						if (!File.Exists(text3))
						{
							return null;
						}
						dependencyCache[depIdx] = am.LoadAssetsFile(text3, loadDeps: true);
					}
				}
			}
			else
			{
				dependencyCache[depIdx] = value;
			}
		}
		return dependencyCache[depIdx];
	}
}
