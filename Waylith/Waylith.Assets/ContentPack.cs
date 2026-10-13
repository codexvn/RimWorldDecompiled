using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Waylith.Utils;

namespace Waylith.Assets;

public static class ContentPack
{
	public sealed class MountPoint
	{
		public required string Root { get; init; }

		public required AssetPack Pack { get; init; }

		public required IReadOnlyDictionary<string, string> Stamps { get; init; }

		public required Dictionary<string, AssetPackTextureInfo.Info> TextureInfo { get; init; }
	}

	public const string TextureSuffix = ".dds";

	public const string PremultipliedTextureSuffix = "-premul.dds";

	public const string MeshSuffix = ".nbmesh";

	private static readonly string[] Suffixes = new string[3] { "-premul.dds", ".dds", ".nbmesh" };

	private static readonly object MountLock = new object();

	private static volatile MountPoint[] mounts = Array.Empty<MountPoint>();

	private static volatile MountPoint? primary;

	private static readonly StringComparison RootComparison = (OperatingSystem.IsWindows() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);

	public static AssetPack? Pack => primary?.Pack;

	public static string? ContentRoot => primary?.Root;

	public static IReadOnlyList<MountPoint> Mounts => mounts;

	public static void Use(AssetPack? pack, string? contentRoot, IReadOnlyDictionary<string, string> stamps)
	{
		lock (MountLock)
		{
			if (primary != null)
			{
				Unmount(primary.Root, primary.Pack != pack);
			}
			if (pack != null && contentRoot != null)
			{
				primary = Mount(contentRoot, pack, stamps);
			}
		}
	}

	public static MountPoint Mount(string contentRoot, AssetPack pack, IReadOnlyDictionary<string, string> stamps)
	{
		string root = NormalizeRoot(contentRoot);
		MountPoint mountPoint = new MountPoint
		{
			Root = root,
			Pack = pack,
			Stamps = stamps,
			TextureInfo = AssetPackTextureInfo.Read(pack)
		};
		lock (MountLock)
		{
			MountPoint mountPoint2 = FindMount(root);
			if (mountPoint2 != null)
			{
				Unmount(root, mountPoint2.Pack != pack);
			}
			List<MountPoint> list = new List<MountPoint>(mounts);
			list.Add(mountPoint);
			list.Sort((MountPoint a, MountPoint b) => b.Root.Length.CompareTo(a.Root.Length));
			mounts = list.ToArray();
			ResourceFiles.AddVirtualFiles(root, pack.ContentHashes.Keys.Select((string name) => Path.Combine(root, SourcePathOf(name))));
			return mountPoint;
		}
	}

	public static void Unmount(string contentRoot)
	{
		Unmount(contentRoot, dispose: true);
	}

	private static void Unmount(string contentRoot, bool dispose)
	{
		string root = NormalizeRoot(contentRoot);
		lock (MountLock)
		{
			MountPoint mount = FindMount(root);
			if (mount != null)
			{
				mounts = mounts.Where((MountPoint m) => m != mount).ToArray();
				if (primary == mount)
				{
					primary = null;
				}
				ResourceFiles.RemoveVirtualFiles(root);
				if (dispose)
				{
					mount.Pack.Dispose();
				}
			}
		}
	}

	private static MountPoint? FindMount(string root)
	{
		return mounts.FirstOrDefault((MountPoint m) => string.Equals(m.Root, root, RootComparison));
	}

	private static string NormalizeRoot(string root)
	{
		return Path.TrimEndingDirectorySeparator(Path.GetFullPath(root));
	}

	private static bool TryGetMount(string fullPath, out MountPoint mount, out string relative)
	{
		string text = Path.TrimEndingDirectorySeparator(Path.GetFullPath(fullPath));
		MountPoint[] array = mounts;
		foreach (MountPoint mountPoint in array)
		{
			if (text.Length == mountPoint.Root.Length && string.Equals(text, mountPoint.Root, RootComparison))
			{
				mount = mountPoint;
				relative = string.Empty;
				return true;
			}
			if (text.Length > mountPoint.Root.Length && text[mountPoint.Root.Length] == Path.DirectorySeparatorChar && text.StartsWith(mountPoint.Root, RootComparison))
			{
				mount = mountPoint;
				relative = text.Substring(mountPoint.Root.Length + 1).Replace('\\', '/');
				return true;
			}
		}
		mount = null;
		relative = string.Empty;
		return false;
	}

	public static bool TryGetContentRelativePath(string fullPath, out string relative)
	{
		AssetPack pack;
		return TryGetContentRelativePath(fullPath, out pack, out relative);
	}

	public static bool TryGetContentRelativePath(string fullPath, out AssetPack pack, out string relative)
	{
		MountPoint mount;
		bool flag = TryGetMount(fullPath, out mount, out relative);
		pack = (flag ? mount.Pack : null);
		return flag;
	}

	public static bool TryGetEntryName(string sourcePath, string suffix, out string name)
	{
		AssetPack pack;
		return TryGetEntryName(sourcePath, suffix, out pack, out name);
	}

	public static bool TryGetEntryName(string sourcePath, string suffix, out AssetPack pack, out string name)
	{
		MountPoint mount;
		bool flag = TryGetMount(sourcePath, out mount, out name);
		pack = (flag ? mount.Pack : null);
		if (flag)
		{
			name += suffix;
		}
		return flag;
	}

	public static bool TryGetTextureContentHash(string sourcePath, out string hash)
	{
		hash = string.Empty;
		InlineArray2<string> buffer = default(InlineArray2<string>);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<string>, string>(ref buffer, 0) = ".dds";
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<string>, string>(ref buffer, 1) = "-premul.dds";
		ReadOnlySpan<string> readOnlySpan = global::_003CPrivateImplementationDetails_003E.InlineArrayAsReadOnlySpan<InlineArray2<string>, string>(in buffer, 2);
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			string suffix = readOnlySpan[i];
			if (TryGetCurrentEntry(sourcePath, suffix, out AssetPack pack, out string name) && pack.ContentHashes.TryGetValue(name, out string value))
			{
				hash = value;
				return true;
			}
		}
		return false;
	}

	public static bool TryGetCurrentEntry(string sourcePath, string suffix, out string name)
	{
		MountPoint mount;
		return TryGetCurrentMount(sourcePath, suffix, out mount, out name);
	}

	public static bool TryGetCurrentEntry(string sourcePath, string suffix, out AssetPack pack, out string name)
	{
		MountPoint mount;
		bool flag = TryGetCurrentMount(sourcePath, suffix, out mount, out name);
		pack = (flag ? mount.Pack : null);
		return flag;
	}

	public static bool TryGetCurrentMount(string sourcePath, string suffix, out MountPoint mount, out string name)
	{
		if (!TryGetMount(sourcePath, out mount, out name))
		{
			return false;
		}
		name += suffix;
		if (!mount.Pack.Contains(name))
		{
			return false;
		}
		if (!mount.Stamps.TryGetValue(name, out string value))
		{
			return true;
		}
		FileInfo fileInfo = new FileInfo(sourcePath);
		if (fileInfo.Exists)
		{
			return value == AssetPackManifest.Stamp(fileInfo);
		}
		return true;
	}

	public static bool HasFile(string fullPath)
	{
		MountPoint mount;
		string name;
		return TryGetCurrentMount(fullPath, string.Empty, out mount, out name);
	}

	public static bool TryReadFile(string fullPath, out byte[] data)
	{
		data = Array.Empty<byte>();
		if (TryGetCurrentMount(fullPath, string.Empty, out MountPoint mount, out string name))
		{
			return mount.Pack.TryRead(name, out data);
		}
		return false;
	}

	public static bool TryGetStoredFile(string fullPath, out ReadOnlySpan<byte> data)
	{
		data = default(ReadOnlySpan<byte>);
		if (TryGetCurrentMount(fullPath, string.Empty, out MountPoint mount, out string name))
		{
			return mount.Pack.TryGetStoredPayload(name, out data);
		}
		return false;
	}

	public static bool TryGetStoredFile(string fullPath, out nint pointer, out int length)
	{
		pointer = IntPtr.Zero;
		length = 0;
		if (TryGetCurrentMount(fullPath, string.Empty, out MountPoint mount, out string name))
		{
			return mount.Pack.TryGetStoredPayload(name, out pointer, out length);
		}
		return false;
	}

	public static bool TryGetFileLength(string fullPath, out long length)
	{
		length = 0L;
		if (!TryGetCurrentMount(fullPath, string.Empty, out MountPoint mount, out string name) || !mount.Pack.TryGetCompressedInfo(name, out var _, out var length2, out var _, out var _))
		{
			return false;
		}
		length = length2;
		return true;
	}

	private static string SourcePathOf(string entryName)
	{
		string[] suffixes = Suffixes;
		foreach (string text in suffixes)
		{
			if (entryName.EndsWith(text, StringComparison.Ordinal))
			{
				int length = text.Length;
				return entryName.Substring(0, entryName.Length - length);
			}
		}
		return entryName;
	}
}
