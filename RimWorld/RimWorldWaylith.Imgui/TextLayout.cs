using System;
using System.Collections.Generic;
using Brutal.ImGuiApi;
using UnityEngine;

namespace RimWorldWaylith.Imgui;

internal sealed class TextLayout
{
	internal readonly struct Piece
	{
		public readonly int Start;

		public readonly int Length;

		public readonly Color Color;

		public readonly FontStyle Style;

		public readonly float Size;

		public readonly float X;

		public readonly float Width;

		public readonly bool IsSpace;

		public Piece(int start, int length, Color color, FontStyle style, float size, float x, float width, bool isSpace)
		{
			Start = start;
			Length = length;
			Color = color;
			Style = style;
			Size = size;
			X = x;
			Width = width;
			IsSpace = isSpace;
		}

		public Piece Shifted(float dx)
		{
			return new Piece(Start, Length, Color, Style, Size, X - dx, Width, IsSpace);
		}
	}

	internal readonly struct Line
	{
		public readonly int PieceStart;

		public readonly int PieceCount;

		public readonly float Width;

		public readonly float Top;

		public readonly float Ascent;

		public readonly float FirstLine;

		public readonly float Pitch;

		public Line(int pieceStart, int pieceCount, float width, float top, LineMetrics metrics)
		{
			PieceStart = pieceStart;
			PieceCount = pieceCount;
			Width = width;
			Top = top;
			Ascent = metrics.Ascent;
			FirstLine = metrics.FirstLine;
			Pitch = metrics.Pitch;
		}
	}

	private sealed class GlyphAdvances(ImFontPtr font, float sizePx, float scale)
	{
		private readonly float[] bmp = CreateTable();

		private static float[] CreateTable()
		{
			float[] array = new float[65536];
			Array.Fill(array, float.NaN);
			return array;
		}

		public float Measure(string s, int start, int length)
		{
			float num = 0f;
			int num2 = start + length;
			int num3 = start;
			while (num3 < num2)
			{
				num3 += CodePoint(s, num3, out var width);
				num += width;
			}
			return num;
		}

		public int CodePoint(string s, int i, out float width)
		{
			char c = s[i];
			if (char.IsHighSurrogate(c) && i + 1 < s.Length && char.IsLowSurrogate(s[i + 1]))
			{
				width = MeasureRaw(s.AsSpan(i, 2));
				return 2;
			}
			float num = bmp[(uint)c];
			if (float.IsNaN(num))
			{
				num = (bmp[(uint)c] = MeasureRaw(s.AsSpan(i, 1)));
			}
			width = num;
			return 1;
		}

		private float MeasureRaw(ReadOnlySpan<char> text)
		{
			uint c = (uint)((text.Length == 2) ? char.ConvertToUtf32(text[0], text[1]) : text[0]);
			ImFontGlyphPtr imFontGlyphPtr = font.GetFontBaked(sizePx, scale).FindGlyph(c);
			if (!imFontGlyphPtr.IsNull())
			{
				return MathF.Floor(imFontGlyphPtr.AdvanceX * scale + 0.5f) / scale;
			}
			return 0f;
		}
	}

	private const float WrapEpsilon = 0.01f;

	private readonly ImGuiHost host;

	private int fontGeneration;

	private readonly Dictionary<float, GlyphAdvances[]> baseAdvances = new Dictionary<float, GlyphAdvances[]>();

	private readonly Dictionary<(int font, float size, FontStyle style, float scale), GlyphAdvances> sizedAdvances = new Dictionary<(int, float, FontStyle, float), GlyphAdvances>();

	private const int MaxSizedAdvances = 32;

	private readonly List<Piece> pieces = new List<Piece>();

	private readonly List<Line> lines = new List<Line>();

	private readonly Stack<Color> colorStack = new Stack<Color>();

	private readonly Stack<float> sizeStack = new Stack<float>();

	private string source = string.Empty;

	private int fontIndex;

	private GlyphAdvances adv;

	private FontStyle style;

	private FontStyle baseStyle;

	private float size;

	private float baseSize;

	private int boldDepth;

	private int italicDepth;

	private LineMetrics lineMetrics;

	private float grownSize;

	private float lineTop;

	private bool wrap;

	private float limit;

	private float cursorX;

	private float lineWidth;

	private int lineStartPiece;

	private int wordStart;

	private bool afterWrap;

	private GlyphAdvances pieceAdv;

	private FontStyle pieceStyle;

	private float pieceSize;

	public string Source => source;

	public IReadOnlyList<Piece> Pieces => pieces;

	public IReadOnlyList<Line> Lines => lines;

	public float Width { get; private set; }

	public float Height { get; private set; }

	public float Scale { get; set; } = 1f;

	private FontStyle CurrentStyle => (FontStyle)((int)((uint)baseStyle | ((boldDepth > 0) ? 1u : 0u)) | ((italicDepth > 0) ? 2 : 0));

	public TextLayout(ImGuiHost host)
	{
		this.host = host;
	}

	private GlyphAdvances AdvancesFor(int fontIndex, float size, FontStyle style)
	{
		if (fontGeneration != host.FontGeneration)
		{
			fontGeneration = host.FontGeneration;
			baseAdvances.Clear();
			sizedAdvances.Clear();
		}
		if (style == FontStyle.Normal && size == host.ImportSize(fontIndex))
		{
			if (!baseAdvances.TryGetValue(Scale, out GlyphAdvances[] value) || value.Length < host.Fonts.Count)
			{
				if (baseAdvances.Count >= 32)
				{
					baseAdvances.Clear();
				}
				GlyphAdvances[] array = new GlyphAdvances[host.Fonts.Count];
				value?.CopyTo(array, 0);
				for (int i = ((value != null) ? value.Length : 0); i < array.Length; i++)
				{
					array[i] = new GlyphAdvances(host.Font(i), host.ImportSize(i), Scale);
				}
				value = (baseAdvances[Scale] = array);
			}
			return value[fontIndex];
		}
		if (!sizedAdvances.TryGetValue((fontIndex, size, style, Scale), out GlyphAdvances value2))
		{
			if (sizedAdvances.Count >= 32)
			{
				sizedAdvances.Clear();
			}
			value2 = (sizedAdvances[(fontIndex, size, style, Scale)] = new GlyphAdvances(host.StyledFont(fontIndex, style), size, Scale));
		}
		return value2;
	}

	private GlyphAdvances AdvancesOf(in Piece piece)
	{
		if (piece.Style == style && piece.Size == size)
		{
			return adv;
		}
		if (pieceAdv == null || piece.Style != pieceStyle || piece.Size != pieceSize)
		{
			pieceStyle = piece.Style;
			pieceSize = piece.Size;
			pieceAdv = AdvancesFor(fontIndex, piece.Size, piece.Style);
		}
		return pieceAdv;
	}

	private void SetRun(FontStyle s, float px)
	{
		if (s != style || px != size)
		{
			style = s;
			size = px;
			adv = AdvancesFor(fontIndex, px, s);
		}
	}

	public int CodePointAt(in Piece piece, int i, out float width)
	{
		return AdvancesOf(in piece).CodePoint(source, i, out width);
	}

	public void Layout(int fontIndex, float fontSize, FontStyle fontStyle, string text, float wrapWidth, Color baseColor, bool richText = true)
	{
		source = text ?? string.Empty;
		this.fontIndex = fontIndex;
		pieceAdv = null;
		int end = (boldDepth = (italicDepth = 0));
		baseStyle = (style = fontStyle);
		float num = (baseSize = (size = host.PixelSizeFor(fontIndex, fontSize)));
		adv = AdvancesFor(fontIndex, size, style);
		wrap = wrapWidth > 0f;
		limit = (wrap ? (wrapWidth + 0.01f) : float.MaxValue);
		pieces.Clear();
		lines.Clear();
		colorStack.Clear();
		sizeStack.Clear();
		lineMetrics = default(LineMetrics);
		grownSize = 0f;
		lineTop = 0f;
		cursorX = 0f;
		lineWidth = 0f;
		lineStartPiece = 0;
		wordStart = -1;
		afterWrap = false;
		Color color = baseColor;
		int length = source.Length;
		int num2 = 0;
		while (num2 < length)
		{
			char c = source[num2];
			if (c == '<' && richText)
			{
				int end2;
				Color color2;
				float px;
				switch (RichText.Parse(source, num2, out end2, out color2, out px))
				{
				case RichText.Tag.ColorOpen:
					colorStack.Push(color);
					color = color2;
					goto default;
				case RichText.Tag.ColorOpenInvalid:
					colorStack.Push(color);
					goto default;
				case RichText.Tag.ColorClose:
					color = ((colorStack.Count > 0) ? colorStack.Pop() : baseColor);
					goto default;
				case RichText.Tag.BoldOpen:
					boldDepth++;
					SetRun(CurrentStyle, size);
					goto default;
				case RichText.Tag.BoldClose:
					boldDepth = Math.Max(0, boldDepth - 1);
					SetRun(CurrentStyle, size);
					goto default;
				case RichText.Tag.ItalicOpen:
					italicDepth++;
					SetRun(CurrentStyle, size);
					goto default;
				case RichText.Tag.ItalicClose:
					italicDepth = Math.Max(0, italicDepth - 1);
					SetRun(CurrentStyle, size);
					goto default;
				case RichText.Tag.SizeOpen:
					sizeStack.Push(size);
					SetRun(style, px);
					goto default;
				case RichText.Tag.SizeOpenInvalid:
					sizeStack.Push(size);
					goto default;
				case RichText.Tag.SizeClose:
					SetRun(style, (sizeStack.Count > 0) ? sizeStack.Pop() : baseSize);
					goto default;
				default:
					num2 = end2;
					continue;
				case RichText.Tag.None:
					break;
				}
			}
			switch (c)
			{
			case '\n':
				EndLine(wrapped: false);
				num2++;
				continue;
			case '\r':
				num2++;
				continue;
			case ' ':
			{
				int i;
				for (i = num2 + 1; i < length && source[i] == ' '; i++)
				{
				}
				if (!afterWrap || pieces.Count != lineStartPiece)
				{
					float num3 = adv.Measure(source, num2, i - num2);
					pieces.Add(new Piece(num2, i - num2, color, style, size, cursorX, num3, isSpace: true));
					GrowLine(size);
					cursorX += num3;
				}
				wordStart = -1;
				num2 = i;
				continue;
			}
			}
			int j;
			for (j = num2 + 1; j < length; j++)
			{
				switch (source[j])
				{
				case '<':
				{
					if (RichText.Parse(source, j, out end, out var _, out num) == RichText.Tag.None)
					{
						continue;
					}
					break;
				}
				default:
					continue;
				case '\n':
				case '\r':
				case ' ':
					break;
				}
				break;
			}
			AddWord(num2, j - num2, color);
			num2 = j;
		}
		CloseLine(pieces.Count - lineStartPiece, lineWidth, newline: false);
		float num4 = 0f;
		foreach (Line line2 in lines)
		{
			num4 = Math.Max(num4, line2.Width);
		}
		Width = num4;
		List<Line> list = lines;
		Line line = list[list.Count - 1];
		Height = line.Top + line.FirstLine;
	}

	public int LineAt(float y)
	{
		int i;
		for (i = 0; i + 1 < lines.Count && y >= lines[i + 1].Top; i++)
		{
		}
		return i;
	}

	public int LineOf(int index)
	{
		int result = 0;
		for (int i = 1; i < lines.Count && ((lines[i].PieceCount > 0) ? pieces[lines[i].PieceStart].Start : LineStartIndex(i)) <= index; i++)
		{
			result = i;
		}
		return result;
	}

	public int IndexAt(Vector2 p)
	{
		if (lines.Count == 0 || source.Length == 0)
		{
			return 0;
		}
		int num = LineAt(p.y);
		Line line = lines[num];
		if (line.PieceCount == 0)
		{
			return LineStartIndex(num);
		}
		if (p.x <= 0f)
		{
			return pieces[line.PieceStart].Start;
		}
		for (int i = line.PieceStart; i < line.PieceStart + line.PieceCount; i++)
		{
			Piece piece = pieces[i];
			if (p.x >= piece.X + piece.Width)
			{
				continue;
			}
			float num2 = piece.X;
			int j = piece.Start;
			int num3 = piece.Start + piece.Length;
			GlyphAdvances glyphAdvances = AdvancesOf(in piece);
			int num4;
			for (; j < num3; j += num4)
			{
				num4 = glyphAdvances.CodePoint(source, j, out var width);
				if (p.x < num2 + width * 0.5f)
				{
					return j;
				}
				num2 += width;
			}
			return num3;
		}
		Piece piece2 = pieces[line.PieceStart + line.PieceCount - 1];
		return piece2.Start + piece2.Length;
	}

	public Vector2 PositionOf(int index)
	{
		if (lines.Count == 0)
		{
			return Vector2.zero;
		}
		int index2 = LineOf(index);
		Line line = lines[index2];
		float x = 0f;
		for (int i = line.PieceStart; i < line.PieceStart + line.PieceCount; i++)
		{
			Piece piece = pieces[i];
			if (index <= piece.Start)
			{
				x = piece.X;
				break;
			}
			x = ((index >= piece.Start + piece.Length) ? (piece.X + piece.Width) : (piece.X + AdvancesOf(in piece).Measure(source, piece.Start, index - piece.Start)));
			if (index <= piece.Start + piece.Length)
			{
				break;
			}
		}
		return new Vector2(x, line.Top);
	}

	private int LineStartIndex(int li)
	{
		for (int i = li + 1; i < lines.Count; i++)
		{
			if (lines[i].PieceCount > 0)
			{
				return Math.Max(0, pieces[lines[i].PieceStart].Start - (i - li));
			}
		}
		return Math.Max(0, source.Length - (lines.Count - 1 - li));
	}

	private void AddWord(int start, int length, Color color)
	{
		float num = adv.Measure(source, start, length);
		bool flag = wrap && cursorX + num > limit;
		if (flag && pieces.Count > lineStartPiece)
		{
			if (wordStart > lineStartPiece)
			{
				MoveWordToNewLine();
			}
			else if (wordStart < 0)
			{
				EndLine(wrapped: true);
			}
			flag = cursorX + num > limit;
		}
		if (!flag)
		{
			AddWordPiece(start, length, color, num);
			return;
		}
		int num2 = start;
		int num3 = start + length;
		while (num2 < num3)
		{
			int i = num2;
			float num4 = 0f;
			int num5;
			for (; i < num3; i += num5)
			{
				num5 = adv.CodePoint(source, i, out var width);
				if (cursorX + num4 + width > limit && (i > num2 || cursorX > 0f))
				{
					break;
				}
				num4 += width;
			}
			if (i == num2)
			{
				EndLine(wrapped: true);
				continue;
			}
			AddWordPiece(num2, i - num2, color, num4);
			num2 = i;
			if (num2 < num3)
			{
				EndLine(wrapped: true);
			}
		}
	}

	private void AddWordPiece(int start, int length, Color color, float width)
	{
		pieces.Add(new Piece(start, length, color, style, size, cursorX, width, isSpace: false));
		GrowLine(size);
		if (wordStart < 0)
		{
			wordStart = pieces.Count - 1;
		}
		cursorX += width;
		lineWidth = cursorX;
	}

	private void GrowLine(float px)
	{
		if (px == grownSize)
		{
			return;
		}
		LineMetrics lineMetrics = host.LineMetricsFor(fontIndex, px);
		if (grownSize == 0f)
		{
			grownSize = px;
			this.lineMetrics = lineMetrics;
			return;
		}
		grownSize = px;
		if (!(lineMetrics.Ascent <= this.lineMetrics.Ascent) || !(lineMetrics.FirstLine - lineMetrics.Ascent <= this.lineMetrics.FirstLine - this.lineMetrics.Ascent) || !(lineMetrics.Pitch <= this.lineMetrics.Pitch))
		{
			float num = Math.Max(lineMetrics.Ascent, this.lineMetrics.Ascent);
			float num2 = Math.Max(lineMetrics.FirstLine - lineMetrics.Ascent, this.lineMetrics.FirstLine - this.lineMetrics.Ascent);
			this.lineMetrics = new LineMetrics(num, num + num2, Math.Max(lineMetrics.Pitch, this.lineMetrics.Pitch), Math.Max(lineMetrics.Spacing, this.lineMetrics.Spacing));
		}
	}

	private void CloseLine(int pieceCount, float width, bool newline)
	{
		if (newline || grownSize == 0f)
		{
			GrowLine(size);
		}
		lines.Add(new Line(lineStartPiece, pieceCount, width, lineTop, lineMetrics));
		lineTop += lineMetrics.Pitch;
		lineMetrics = default(LineMetrics);
		grownSize = 0f;
	}

	private void EndLine(bool wrapped)
	{
		CloseLine(pieces.Count - lineStartPiece, lineWidth, !wrapped);
		lineStartPiece = pieces.Count;
		cursorX = 0f;
		lineWidth = 0f;
		wordStart = -1;
		afterWrap = wrapped;
	}

	private void MoveWordToNewLine()
	{
		float x = pieces[wordStart].X;
		float width = 0f;
		lineMetrics = default(LineMetrics);
		grownSize = 0f;
		for (int i = lineStartPiece; i < wordStart; i++)
		{
			if (!pieces[i].IsSpace)
			{
				width = pieces[i].X + pieces[i].Width;
			}
			GrowLine(pieces[i].Size);
		}
		CloseLine(wordStart - lineStartPiece, width, newline: false);
		for (int j = wordStart; j < pieces.Count; j++)
		{
			pieces[j] = pieces[j].Shifted(x);
			GrowLine(pieces[j].Size);
		}
		lineStartPiece = wordStart;
		cursorX -= x;
		lineWidth = cursorX;
		afterWrap = true;
	}
}
