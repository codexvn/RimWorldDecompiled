using System;
using System.Collections.Generic;
using System.IO;
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
		}
		List<string> list2 = new List<string>();
		foreach (string item in list)
		{
			if (!Directory.Exists(item))
			{
				continue;
			}
			try
			{
				foreach (string item2 in Directory.EnumerateFiles(item, "*", SearchOption.AllDirectories))
				{
					string extension = Path.GetExtension(item2);
					if (extension.Equals(".ttf", StringComparison.OrdinalIgnoreCase) || extension.Equals(".otf", StringComparison.OrdinalIgnoreCase) || extension.Equals(".ttc", StringComparison.OrdinalIgnoreCase))
					{
						list2.Add(item2);
					}
				}
			}
			catch (Exception ex) when (((ex is IOException || ex is UnauthorizedAccessException) ? 1 : 0) != 0)
			{
			}
		}
		return list2.ToArray();
	}

	public static Font CreateDynamicFontFromOSFont(string fontname, int size)
	{
		StubWarner.LogStub("CreateDynamicFontFromOSFont", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Font.cs", 101);
		return new Font
		{
			name = fontname
		};
	}

	public static Font CreateDynamicFontFromOSFont(string[] fontnames, int size)
	{
		StubWarner.LogStub("CreateDynamicFontFromOSFont", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Font.cs", 102);
		return new Font
		{
			name = ((fontnames != null && fontnames.Length > 0) ? fontnames[0] : string.Empty)
		};
	}

	public static string[] GetOSInstalledFontNames()
	{
		StubWarner.LogStub("GetOSInstalledFontNames", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Font.cs", 103);
		return Array.Empty<string>();
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
