using System;
using System.Collections.Generic;
using UnityStubs.Gui;

namespace UnityEngine;

public sealed class TextGenerator : IDisposable
{
	private readonly List<UICharInfo> _characters = new List<UICharInfo>();

	private readonly List<UILineInfo> _lines = new List<UILineInfo>();

	private readonly List<UIVertex> _verts = new List<UIVertex>();

	public IList<UICharInfo> characters => _characters;

	public IList<UILineInfo> lines => _lines;

	public IList<UIVertex> verts => _verts;

	public int characterCount => _characters.Count;

	public int characterCountVisible => _characters.Count;

	public int lineCount => _lines.Count;

	public int vertexCount => _verts.Count;

	public Rect rectExtents { get; private set; }

	public TextGenerator()
	{
	}

	public TextGenerator(int initialCapacity)
	{
	}

	public void Dispose()
	{
	}

	public bool Populate(string str, TextGenerationSettings settings)
	{
		Invalidate();
		ITextMeshProBackend current = TextMeshProBackend.Current;
		Font font = settings.font;
		if (current == null || font == null)
		{
			return false;
		}
		int gameFontIndex = font.GameFontIndex;
		int num = ((settings.fontSize > 0) ? settings.fontSize : font.fontSize);
		if (num <= 0 || !current.TryGetFontMetrics(gameFontIndex, out var metrics) || !metrics.found || metrics.bakedPixelSize <= 0f)
		{
			return false;
		}
		float num2 = (float)num / metrics.bakedPixelSize;
		float num3 = metrics.lineHeight * num2 * ((settings.lineSpacing > 0f) ? settings.lineSpacing : 1f);
		if (str == null)
		{
			str = string.Empty;
		}
		string[] array = str.Split('\n');
		float[] array2 = new float[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			float num4 = 0f;
			string text = array[i];
			foreach (char c in text)
			{
				if (c != '\r' && current.TryGetGlyphAtSize(gameFontIndex, c, num, out var glyph, out var _))
				{
					num4 += glyph.advanceX;
				}
			}
			array2[i] = num4;
		}
		float val = 0f;
		float[] array3 = array2;
		foreach (float val2 in array3)
		{
			val = Math.Max(val, val2);
		}
		float num5 = num3 * (float)array.Length;
		Vector2 generationExtents = settings.generationExtents;
		float num6 = (0f - settings.pivot.x) * generationExtents.x;
		float num7 = (0f - settings.pivot.y) * generationExtents.y;
		float num8 = num7 + generationExtents.y;
		int num9 = (int)settings.textAnchor % 3;
		float num10 = ((int)settings.textAnchor / 3) switch
		{
			0 => num8, 
			1 => num8 - (generationExtents.y - num5) * 0.5f, 
			_ => num7 + num5, 
		};
		rectExtents = new Rect(num6, num7, generationExtents.x, generationExtents.y);
		Color32 color = settings.color;
		int num11 = 0;
		for (int k = 0; k < array.Length; k++)
		{
			float num12 = num10 - (float)k * num3;
			float ascent = num6;
			float ascent2 = num9 switch
			{
				0 => 0f, 
				1 => (generationExtents.x - array2[k]) * 0.5f, 
				_ => generationExtents.x - array2[k], 
			};
			float num13 = ascent + ascent2;
			_lines.Add(new UILineInfo
			{
				startCharIdx = num11,
				height = (int)MathF.Round(num3),
				topY = num12,
				leading = 0f
			});
			string text = array[k];
			foreach (char c2 in text)
			{
				num11++;
				if (c2 == '\r' || !current.TryGetGlyphAtSize(gameFontIndex, c2, num, out var glyph2, out ascent2))
				{
					_characters.Add(new UICharInfo
					{
						cursorPos = new Vector2(num13, num12),
						charWidth = 0f
					});
					continue;
				}
				_characters.Add(new UICharInfo
				{
					cursorPos = new Vector2(num13, num12),
					charWidth = glyph2.advanceX
				});
				if (glyph2.visible)
				{
					float x = num13 + glyph2.x0;
					float x2 = num13 + glyph2.x1;
					float y = num12 - glyph2.y0;
					float y2 = num12 - glyph2.y1;
					AddVert(x, y, glyph2.u0, glyph2.v0, color);
					AddVert(x2, y, glyph2.u1, glyph2.v0, color);
					AddVert(x2, y2, glyph2.u1, glyph2.v1, color);
					AddVert(x, y2, glyph2.u0, glyph2.v1, color);
				}
				num13 += glyph2.advanceX;
			}
			num11++;
		}
		return true;
	}

	private void AddVert(float x, float y, float u, float v, Color32 color)
	{
		UIVertex simpleVert = UIVertex.simpleVert;
		simpleVert.position = new Vector3(x, y, 0f);
		simpleVert.uv0 = new Vector4(u, 1f - v, 0f, 0f);
		simpleVert.color = color;
		_verts.Add(simpleVert);
	}

	public bool PopulateWithErrors(string str, TextGenerationSettings settings, GameObject context)
	{
		return Populate(str, settings);
	}

	public void Invalidate()
	{
		_characters.Clear();
		_lines.Clear();
		_verts.Clear();
		rectExtents = default(Rect);
	}

	public float GetPreferredWidth(string str, TextGenerationSettings settings)
	{
		settings.horizontalOverflow = HorizontalWrapMode.Overflow;
		if (!Populate(str, settings) || _lines.Count == 0)
		{
			return 0f;
		}
		float num = float.MaxValue;
		float num2 = float.MinValue;
		foreach (UICharInfo character in _characters)
		{
			num = Math.Min(num, character.cursorPos.x);
			num2 = Math.Max(num2, character.cursorPos.x + character.charWidth);
		}
		if (!(num2 > num))
		{
			return 0f;
		}
		return num2 - num;
	}

	public float GetPreferredHeight(string str, TextGenerationSettings settings)
	{
		settings.verticalOverflow = VerticalWrapMode.Overflow;
		if (!Populate(str, settings))
		{
			return 0f;
		}
		return _lines.Count * ((_lines.Count > 0) ? _lines[0].height : 0);
	}

	public UICharInfo[] GetCharactersArray()
	{
		return _characters.ToArray();
	}

	public UILineInfo[] GetLinesArray()
	{
		return _lines.ToArray();
	}

	public UIVertex[] GetVerticesArray()
	{
		return _verts.ToArray();
	}

	public void GetCharacters(List<UICharInfo> characters)
	{
		characters.Clear();
		characters.AddRange(_characters);
	}

	public void GetLines(List<UILineInfo> lines)
	{
		lines.Clear();
		lines.AddRange(_lines);
	}

	public void GetVertices(List<UIVertex> vertices)
	{
		vertices.Clear();
		vertices.AddRange(_verts);
	}
}
