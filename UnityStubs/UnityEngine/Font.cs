using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityStubs.Gui;
using UnityStubs.Internal;
using Waylith.UnityCompat;

namespace UnityEngine;

public class Font : Object
{
	private int _gameFontIndex = 1;

	private byte[] _fontData;

	private float _importSize;

	private int _renderingMode;

	private static readonly int[] FallbackPixelSizes = new int[3] { 12, 15, 22 };

	private Material _material;

	public int GameFontIndex
	{
		get
		{
			if (_fontData != null)
			{
				TryRegisterFontData();
			}
			return _gameFontIndex;
		}
		set
		{
			_gameFontIndex = value;
			_fontData = null;
		}
	}

	internal FontFaceReader.FaceNames? FaceNames { get; private set; }

	public bool dynamic => true;

	public int fontSize
	{
		get
		{
			if (!TryGetMetrics(out var importSize, out var _, out var _))
			{
				if (_fontData == null)
				{
					return FallbackPixelSizes[Math.Clamp(GameFontIndex, 0, 2)];
				}
				return (int)MathF.Round(_importSize);
			}
			return (int)MathF.Round(importSize);
		}
	}

	public int ascent
	{
		get
		{
			if (!TryGetMetrics(out var _, out var x, out var _))
			{
				return fontSize;
			}
			return (int)MathF.Round(x);
		}
	}

	public int lineHeight
	{
		get
		{
			if (!TryGetMetrics(out var _, out var _, out var lineSpacing))
			{
				return fontSize + 6;
			}
			return (int)MathF.Round(lineSpacing);
		}
	}

	public Material material
	{
		get
		{
			Material obj = _material;
			if ((object)obj == null)
			{
				Material obj2 = new Material((Shader)null)
				{
					name = "Font Material",
					mainTexture = TextMeshProBackend.Current?.GetAtlasTexture(GameFontIndex)
				};
				Material material = obj2;
				_material = obj2;
				obj = material;
			}
			return obj;
		}
	}

	public static event Action<Font> textureRebuilt
	{
		add
		{
		}
		remove
		{
		}
	}

	internal void SetFontData(byte[] fontData, float importSize, int renderingMode)
	{
		_fontData = fontData;
		_importSize = importSize;
		_renderingMode = renderingMode;
		if (FontFaceReader.TryReadFaces(fontData, out var faces))
		{
			FaceNames = faces[0];
		}
	}

	private void TryRegisterFontData()
	{
		IGuiBackend current = GuiBackend.Current;
		if (current != null && UnityRuntime.Current?.App.MainThreadId == Environment.CurrentManagedThreadId)
		{
			int num = current.RegisterFont(base.name, _fontData, _importSize, _renderingMode);
			if (num >= 0)
			{
				_gameFontIndex = num;
			}
			else
			{
				Debug.LogWarning("Font '" + base.name + "': its font data couldn't be loaded, using the default font.");
			}
			_fontData = null;
		}
	}

	public Font()
	{
	}

	public Font(string name)
	{
		base.name = name;
	}

	public static string[] GetPathsToOSFonts()
	{
		List<string> list = new List<string>();
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
		if (OperatingSystem.IsWindows())
		{
			list.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Fonts"));
			list.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft", "Windows", "Fonts"));
		}
		else if (OperatingSystem.IsMacOS())
		{
			list.Add("/System/Library/Fonts");
			list.Add("/Library/Fonts");
			list.Add(Path.Combine(folderPath, "Library", "Fonts"));
		}
		else
		{
			list.Add("/usr/share/fonts");
			list.Add("/usr/local/share/fonts");
			list.Add(Path.Combine(folderPath, ".fonts"));
			list.Add(Path.Combine(folderPath, ".local", "share", "fonts"));
			list.Add("/run/host/fonts");
			list.Add("/run/host/user-fonts");
		}
		List<string> list2 = new List<string>();
		HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
		foreach (string item in LinuxFontconfig.FontFiles())
		{
			if (IsSfntFont(item) && hashSet.Add(item))
			{
				list2.Add(item);
			}
		}
		foreach (string item2 in list)
		{
			if (!Directory.Exists(item2))
			{
				continue;
			}
			try
			{
				foreach (string item3 in Directory.EnumerateFiles(item2, "*", SearchOption.AllDirectories))
				{
					if (IsSfntFont(item3) && hashSet.Add(item3))
					{
						list2.Add(item3);
					}
				}
			}
			catch (Exception ex) when (((ex is IOException || ex is UnauthorizedAccessException) ? 1 : 0) != 0)
			{
			}
		}
		return list2.ToArray();
	}

	private static bool IsSfntFont(string file)
	{
		string extension = Path.GetExtension(file);
		if (!extension.Equals(".ttf", StringComparison.OrdinalIgnoreCase) && !extension.Equals(".otf", StringComparison.OrdinalIgnoreCase))
		{
			return extension.Equals(".ttc", StringComparison.OrdinalIgnoreCase);
		}
		return true;
	}

	public static Font CreateDynamicFontFromOSFont(string fontname, int size)
	{
		Font font = new Font
		{
			name = fontname
		};
		if (!FontFaceReader.TryFindOsFont(fontname, out var face))
		{
			Debug.LogWarning("Font '" + fontname + "' is not installed, using the default font.");
			return font;
		}
		try
		{
			font.SetFontData(FontFaceReader.ExtractFace(File.ReadAllBytes(face.Path), face.FaceIndex), size, 0);
		}
		catch (Exception ex) when (((ex is IOException || ex is UnauthorizedAccessException) ? 1 : 0) != 0)
		{
			Debug.LogWarning($"Font '{fontname}' could not be read from '{face.Path}': {ex.Message}");
		}
		return font;
	}

	public static Font CreateDynamicFontFromOSFont(string[] fontnames, int size)
	{
		if (fontnames != null)
		{
			foreach (string fontname in fontnames)
			{
				if (FontFaceReader.TryFindOsFont(fontname, out var _))
				{
					return CreateDynamicFontFromOSFont(fontname, size);
				}
			}
		}
		return CreateDynamicFontFromOSFont((fontnames != null && fontnames.Length > 0) ? fontnames[0] : string.Empty, size);
	}

	public static string[] GetOSInstalledFontNames()
	{
		HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		foreach (FontFaceReader.OsFace value in FontFaceReader.OsFonts.Values)
		{
			hashSet.Add(value.Names.FullName);
		}
		string[] array = hashSet.ToArray();
		Array.Sort(array, (IComparer<string>?)StringComparer.OrdinalIgnoreCase);
		return array;
	}

	private bool TryGetMetrics(out float importSize, out float ascent, out float lineSpacing)
	{
		importSize = (ascent = (lineSpacing = 0f));
		int gameFontIndex = GameFontIndex;
		IGuiBackend current = GuiBackend.Current;
		if (current == null || _fontData != null)
		{
			return false;
		}
		current.FontMetrics(gameFontIndex, out importSize, out ascent, out lineSpacing);
		return true;
	}

	public void RequestCharactersInTexture(string characters)
	{
	}

	public void RequestCharactersInTexture(string characters, int size)
	{
	}

	public void RequestCharactersInTexture(string characters, int size, FontStyle style)
	{
	}

	public bool HasCharacter(char c)
	{
		TmpGlyphMetrics glyph;
		return TextMeshProBackend.Current?.TryGetGlyph(GameFontIndex, c, out glyph) ?? false;
	}

	public bool GetCharacterInfo(char ch, out CharacterInfo info)
	{
		return GetCharacterInfo(ch, out info, 0, FontStyle.Normal);
	}

	public bool GetCharacterInfo(char ch, out CharacterInfo info, int size)
	{
		return GetCharacterInfo(ch, out info, size, FontStyle.Normal);
	}

	public bool GetCharacterInfo(char ch, out CharacterInfo info, int size, FontStyle style)
	{
		info = default(CharacterInfo);
		ITextMeshProBackend current = TextMeshProBackend.Current;
		if (current == null)
		{
			return false;
		}
		TmpGlyphMetrics glyph;
		float num;
		if (size > 0)
		{
			if (!current.TryGetGlyphAtSize(GameFontIndex, ch, size, out glyph, out num))
			{
				return false;
			}
		}
		else
		{
			if (!current.TryGetGlyph(GameFontIndex, ch, out glyph) || !glyph.found || !current.TryGetFontMetrics(GameFontIndex, out var metrics))
			{
				return false;
			}
			num = metrics.ascent;
		}
		info.index = ch;
		info.size = size;
		info.style = style;
		info.width = glyph.advanceX;
		info.vert = new Rect(glyph.x0, num - glyph.y0, glyph.x1 - glyph.x0, 0f - (glyph.y1 - glyph.y0));
		info.uv = new Rect(glyph.u0, 1f - glyph.v1, glyph.u1 - glyph.u0, glyph.v1 - glyph.v0);
		return true;
	}
}
