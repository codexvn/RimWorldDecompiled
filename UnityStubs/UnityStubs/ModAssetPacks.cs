using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml;
using Waylith.AssetBake;
using Waylith.Rendering;

namespace UnityStubs;

public static class ModAssetPacks
{
	[CompilerGenerated]
	private sealed class _003CRawSources_003Ed__9 : IEnumerable<RawSource>, IEnumerable, IEnumerator<RawSource>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private RawSource _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private string root;

		public string _003C_003E3__root;

		private string[] extensions;

		public string[] _003C_003E3__extensions;

		private string prefix;

		public string _003C_003E3__prefix;

		private IEnumerator<string> _003C_003E7__wrap1;

		RawSource IEnumerator<RawSource>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CRawSources_003Ed__9(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (!Directory.Exists(root))
					{
						return false;
					}
					_003C_003E7__wrap1 = Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					string current = _003C_003E7__wrap1.Current;
					string extension = Path.GetExtension(current);
					if (!extension.Equals(".meta", StringComparison.OrdinalIgnoreCase) && !extension.Equals(".stamp", StringComparison.OrdinalIgnoreCase) && (extensions.Length == 0 || ((ReadOnlySpan<string>)extensions).Contains(extension, (IEqualityComparer<string>?)StringComparer.OrdinalIgnoreCase)))
					{
						string text = Path.GetRelativePath(root, current).Replace('\\', '/');
						_003C_003E2__current = new RawSource((prefix.Length == 0) ? text : (prefix + "/" + text), current);
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<RawSource> IEnumerable<RawSource>.GetEnumerator()
		{
			_003CRawSources_003Ed__9 _003CRawSources_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CRawSources_003Ed__ = this;
			}
			else
			{
				_003CRawSources_003Ed__ = new _003CRawSources_003Ed__9(0);
			}
			_003CRawSources_003Ed__.root = _003C_003E3__root;
			_003CRawSources_003Ed__.prefix = _003C_003E3__prefix;
			_003CRawSources_003Ed__.extensions = _003C_003E3__extensions;
			return _003CRawSources_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<RawSource>)this).GetEnumerator();
		}
	}

	public const string TexturesFolder = "Textures";

	public const string SoundsFolder = "Sounds";

	public static readonly string[] AudioExtensions = new string[7] { ".wav", ".mp3", ".ogg", ".xm", ".it", ".mod", ".s3m" };

	private const string RecipeVersion = "rimworld-modpack-v1";

	public static bool HasBakeableContent(string loadFolder)
	{
		if (!Directory.Exists(Path.Combine(loadFolder, "Textures")))
		{
			return Directory.Exists(Path.Combine(loadFolder, "Sounds"));
		}
		return true;
	}

	public static TextureBakeSettings BakeSettings(string loadFolder)
	{
		List<string> borderMipMaps = BorderMipMapSuffixes(loadFolder);
		return new TextureBakeSettings
		{
			Target = BcEncodeTarget.Bc7,
			Quality = 1f,
			RawMaxSize = 16,
			RawPaths = Array.Empty<string>(),
			AllowBc1 = false,
			MipFilter = MipFilter.Kaiser,
			EncodeOnGpu = TextureBlockEncoder.IsConfigured,
			BorderMipMaps = ((borderMipMaps.Count == 0) ? null : ((Func<string, bool>)((string fullPath) => WantsBorderMipMaps(fullPath, borderMipMaps))))
		};
	}

	public static ContentBakeRequest Request(string loadFolder, string output, bool shipped, IReadOnlyList<RawSource> rawFiles = null)
	{
		string text = Path.TrimEndingDirectorySeparator(Path.GetFullPath(loadFolder));
		string text2 = Path.Combine(text, "Textures");
		ContentBakeRequest obj = new ContentBakeRequest
		{
			Output = output,
			Base = text
		};
		IReadOnlyList<string> roots;
		if (!Directory.Exists(text2))
		{
			IReadOnlyList<string> readOnlyList = Array.Empty<string>();
			roots = readOnlyList;
		}
		else
		{
			IReadOnlyList<string> readOnlyList = new _003C_003Ez__ReadOnlySingleElementList<string>(text2);
			roots = readOnlyList;
		}
		obj.Roots = roots;
		obj.Texture = BakeSettings(text);
		obj.RawFiles = rawFiles ?? SoundFiles(text);
		obj.WriteStamps = !shipped;
		obj.WriteRoles = !shipped;
		obj.CodeIdentity = ContentBakeRequest.DefaultCodeIdentity + " " + typeof(ModAssetPacks).Assembly.ManifestModule.ModuleVersionId.ToString() + " rimworld-modpack-v1";
		return obj;
	}

	public static ContentBakeResult Bake(string loadFolder, string output, bool shipped, IReadOnlyList<RawSource> rawFiles = null, Action<string> progress = null, CancellationToken cancellation = default(CancellationToken))
	{
		return ContentPackBaker.Bake(Request(loadFolder, output, shipped, rawFiles), progress, cancellation);
	}

	public static List<RawSource> SoundFiles(string loadFolder, params string[] extensions)
	{
		return RawSources(Path.Combine(loadFolder, "Sounds"), "Sounds", (extensions.Length == 0) ? AudioExtensions : extensions).ToList();
	}

	[IteratorStateMachine(typeof(_003CRawSources_003Ed__9))]
	public static IEnumerable<RawSource> RawSources(string root, string prefix, params string[] extensions)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRawSources_003Ed__9(-2)
		{
			_003C_003E3__root = root,
			_003C_003E3__prefix = prefix,
			_003C_003E3__extensions = extensions
		};
	}

	public static ContentBakeResult BakeForUpload(string loadFolder, Action<string> progress = null, CancellationToken cancellation = default(CancellationToken))
	{
		string text = Path.TrimEndingDirectorySeparator(Path.GetFullPath(loadFolder));
		ContentPacks.Unmount(text);
		try
		{
			return Bake(text, Path.Combine(text, "Assets.zip"), shipped: false, null, progress, cancellation);
		}
		finally
		{
			ContentPacks.MountLoadFolder(text);
		}
	}

	public static List<string> BorderMipMapSuffixes(string loadFolder)
	{
		List<string> list = new List<string>();
		string path = Path.Combine(loadFolder, "Defs");
		if (!Directory.Exists(path))
		{
			return list;
		}
		foreach (string item in Directory.EnumerateFiles(path, "*.xml", SearchOption.AllDirectories))
		{
			XmlDocument xmlDocument;
			try
			{
				xmlDocument = new XmlDocument();
				xmlDocument.Load(item);
			}
			catch (Exception)
			{
				continue;
			}
			foreach (XmlElement item2 in xmlDocument.SelectNodes("//borderMipMaps"))
			{
				if (string.Equals(item2.InnerText.Trim(), "true", StringComparison.OrdinalIgnoreCase))
				{
					string text = item2.ParentNode?["texPath"]?.InnerText.Trim();
					if (!string.IsNullOrEmpty(text))
					{
						list.Add("/Textures/" + text.Replace('\\', '/').Trim('/'));
					}
				}
			}
		}
		return list;
	}

	private static bool WantsBorderMipMaps(string fullPath, List<string> suffixes)
	{
		string text = fullPath.Replace('\\', '/');
		string text2 = text;
		int length = Path.GetExtension(text).Length;
		text = text2.Substring(0, text2.Length - length);
		foreach (string suffix in suffixes)
		{
			if (text.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}
		return false;
	}
}
