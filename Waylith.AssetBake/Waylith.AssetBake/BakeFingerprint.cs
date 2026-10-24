using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Hashing;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using Waylith.Assets;

namespace Waylith.AssetBake;

internal sealed record BakeFingerprint(string Settings, string Textures, string Shaders, string Layout)
{
	private static readonly string[] EngineAssemblies = new string[5] { "Waylith.dll", "Waylith.AssetBake.dll", "Waylith.NoesisIntegration.dll", "Waylith.Profiling.UI.dll", "NoesisManaged.dll" };

	public const string NoShaders = "none";

	public static string PathFor(string packPath)
	{
		return packPath + ".fingerprint";
	}

	public static BakeFingerprint Compute(AssetBakeOptions options, AssetBakeHost host, IReadOnlyList<string> rootFiles, IReadOnlyList<Source> sources)
	{
		string[] shaderPlanAssemblies = host.ShaderPlanAssemblies.ToArray();
		List<string> list = new List<string>();
		list.AddRange(EngineAssemblies);
		list.AddRange(host.VersionedAssemblies);
		list.AddRange(shaderPlanAssemblies);
		string[] versionedAssemblies = list.ToArray();
		string text = $"{options.TextureSettings} {host.Settings(options)} rdo:{(options.Rdo ? string.Join('|', host.RdoRules) : "off")}";
		List<(string, string)> source = ToolBinaries();
		string item = Hash(source.Select<(string, string), string>(((string Name, string Identity) binary) => binary.Name + " " + binary.Identity));
		string text2 = Hash(from binary in source
			where !((ReadOnlySpan<string>)shaderPlanAssemblies).Contains(binary.Name)
			select binary.Name + " " + binary.Identity);
		List<string> list2 = new List<string>();
		list2.Add($"textures:v{23}");
		list2.AddRange(from binary in source
			where !((ReadOnlySpan<string>)versionedAssemblies).Contains(binary.Name)
			select binary.Name + " " + binary.Identity);
		string text3 = Hash(new _003C_003Ez__ReadOnlyList<string>(list2));
		List<string> first = rootFiles.Where((string path) => path.EndsWith(".tyd", StringComparison.OrdinalIgnoreCase)).ToList();
		object obj;
		if (!host.ResolvesAtlasSources)
		{
			obj = new string[1] { text2 };
		}
		else
		{
			List<string> list3 = new List<string>();
			list3.Add(item);
			list3.AddRange(first.Concat(host.AtlasSourceInputs(options)).Order<string>(StringComparer.Ordinal).Select(Stamped));
			obj = list3.ToArray();
		}
		string[] collection = (string[])obj;
		List<string> list4 = new List<string>();
		list4.Add(text);
		list4.AddRange(collection);
		list4.AddRange(sources.Select(Stamped).Order<string>(StringComparer.Ordinal));
		string textures = Hash(new _003C_003Ez__ReadOnlyList<string>(list4));
		IEnumerable<string> second = options.ShaderDirectories.SelectMany((string directory) => Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories));
		object obj2;
		if (!options.NoShaders)
		{
			List<string> list5 = new List<string>();
			list5.Add($"{options.Codec} {options.Level}");
			list5.Add(item);
			list5.AddRange(first.Concat(second).Concat(host.ShaderSourceFiles).Order<string>(StringComparer.Ordinal)
				.Select(Stamped));
			obj2 = Hash(new _003C_003Ez__ReadOnlyList<string>(list5));
		}
		else
		{
			obj2 = "none";
		}
		string shaders = (string)obj2;
		return new BakeFingerprint(Hash(new global::_003C_003Ez__ReadOnlyArray<string>(new string[2] { text, text3 })), textures, shaders, $"align:{options.Alignment}");
	}

	public static BakeFingerprint ComputeContent(string settings, string codeIdentity, IReadOnlyList<Source> sources, int alignment)
	{
		string settings2 = Hash(new global::_003C_003Ez__ReadOnlyArray<string>(new string[2] { settings, codeIdentity }));
		List<string> list = new List<string>();
		list.Add(settings);
		list.Add(codeIdentity);
		list.AddRange(sources.Select(Stamped).Order<string>(StringComparer.Ordinal));
		return new BakeFingerprint(settings2, Hash(new _003C_003Ez__ReadOnlyList<string>(list)), "none", $"align:{alignment}");
	}

	public static BakeFingerprint? Read(string packPath)
	{
		string path = PathFor(packPath);
		if (!File.Exists(path))
		{
			return null;
		}
		string[] array = File.ReadAllText(path).Split('\n');
		if (array.Length < 4)
		{
			return null;
		}
		return new BakeFingerprint(array[0], array[1], array[2], array[3]);
	}

	public void Write(string packPath)
	{
		File.WriteAllText(PathFor(packPath), $"{Settings}\n{Textures}\n{Shaders}\n{Layout}");
	}

	private static string Stamped(string path)
	{
		return path + " " + AssetPackManifest.Stamp(new FileInfo(path));
	}

	private static string Stamped(Source source)
	{
		if (source.RawName != null)
		{
			return source.RawName + " <- " + Stamped(source.Path);
		}
		return Stamped(source.Path);
	}

	private static List<(string Name, string Identity)> ToolBinaries()
	{
		string directory = AppContext.BaseDirectory;
		return (from path in Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories).Where(delegate(string path)
			{
				switch (Path.GetExtension(path))
				{
				case ".dll":
				case ".so":
				case ".dylib":
					return true;
				default:
					return false;
				}
			})
			select (Name: Path.GetRelativePath(directory, path).Replace('\\', '/'), Identity: Identity(path))).OrderBy<(string, string), string>(((string Name, string Identity) binary) => binary.Name, StringComparer.Ordinal).ToList();
	}

	private static string Identity(string path)
	{
		using FileStream fileStream = File.OpenRead(path);
		if (Path.GetExtension(path) == ".dll")
		{
			using PEReader pEReader = new PEReader(fileStream, PEStreamOptions.LeaveOpen);
			if (pEReader.HasMetadata)
			{
				MetadataReader metadataReader = pEReader.GetMetadataReader();
				return metadataReader.GetGuid(metadataReader.GetModuleDefinition().Mvid).ToString();
			}
			fileStream.Position = 0L;
		}
		XxHash3 xxHash = new XxHash3();
		xxHash.Append(fileStream);
		return xxHash.GetCurrentHashAsUInt64().ToString("x16");
	}

	private static string Hash(IEnumerable<string> parts)
	{
		using IncrementalHash incrementalHash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
		foreach (string part in parts)
		{
			incrementalHash.AppendData(Encoding.UTF8.GetBytes(part));
			incrementalHash.AppendData("\n"u8);
		}
		return Convert.ToHexString(incrementalHash.GetHashAndReset());
	}

	[CompilerGenerated]
	private BakeFingerprint(BakeFingerprint original)
	{
		Settings = original.Settings;
		Textures = original.Textures;
		Shaders = original.Shaders;
		Layout = original.Layout;
	}
}
