using System;
using System.IO;
using UnityStubs.Internal;

namespace UnityEngine.TextCore.LowLevel;

public static class FontEngine
{
	private static FontFaceReader.FaceNames? _loadedFace;

	private static int _loadedPointSize;

	public static FontEngineError InitializeFontEngine()
	{
		return FontEngineError.Success;
	}

	public static FontEngineError DestroyFontEngine()
	{
		_loadedFace = null;
		return FontEngineError.Success;
	}

	public static FontEngineError LoadFontFace(string filePath)
	{
		return LoadFontFace(filePath, 0);
	}

	public static FontEngineError LoadFontFace(string filePath, int pointSize)
	{
		if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
		{
			return FontEngineError.Invalid_File_Path;
		}
		try
		{
			return LoadFontFace(File.ReadAllBytes(filePath), pointSize);
		}
		catch (Exception ex) when (((ex is IOException || ex is UnauthorizedAccessException) ? 1 : 0) != 0)
		{
			return FontEngineError.Invalid_File_Path;
		}
	}

	public static FontEngineError LoadFontFace(byte[] sourceFontFile)
	{
		return LoadFontFace(sourceFontFile, 0);
	}

	public static FontEngineError LoadFontFace(byte[] sourceFontFile, int pointSize)
	{
		if (sourceFontFile == null || !FontFaceReader.TryReadFaces(sourceFontFile, out var faces))
		{
			return FontEngineError.Invalid_File_Format;
		}
		_loadedFace = faces[0];
		_loadedPointSize = pointSize;
		return FontEngineError.Success;
	}

	public static FontEngineError LoadFontFace(Font font)
	{
		return LoadFontFace(font, 0);
	}

	public static FontEngineError LoadFontFace(Font font, int pointSize)
	{
		if ((object)font == null || !font.FaceNames.HasValue)
		{
			return FontEngineError.Invalid_Face;
		}
		_loadedFace = font.FaceNames;
		_loadedPointSize = pointSize;
		return FontEngineError.Success;
	}

	public static FontEngineError UnloadFontFace()
	{
		_loadedFace = null;
		return FontEngineError.Success;
	}

	public static FaceInfo GetFaceInfo()
	{
		FontFaceReader.FaceNames? loadedFace = _loadedFace;
		if (loadedFace.HasValue)
		{
			FontFaceReader.FaceNames valueOrDefault = loadedFace.GetValueOrDefault();
			FaceInfo result = default(FaceInfo);
			result.familyName = valueOrDefault.Family;
			result.styleName = valueOrDefault.Style;
			result.pointSize = _loadedPointSize;
			result.scale = 1f;
			return result;
		}
		return default(FaceInfo);
	}

	public static FontEngineError SetFaceSize(int pointSize)
	{
		StubWarner.LogStub("SetFaceSize", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\TextCore.cs", 119);
		return FontEngineError.Invalid_Face;
	}

	public static bool TryGetGlyphIndex(uint unicode, out uint glyphIndex)
	{
		StubWarner.LogStub("TryGetGlyphIndex", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\TextCore.cs", 120);
		glyphIndex = 0u;
		return false;
	}

	public static bool TryGetGlyphWithIndexValue(uint glyphIndex, GlyphLoadFlags flags, out Glyph glyph)
	{
		StubWarner.LogStub("TryGetGlyphWithIndexValue", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\TextCore.cs", 121);
		glyph = null;
		return false;
	}

	public static bool TryGetGlyphWithUnicodeValue(uint unicode, GlyphLoadFlags flags, out Glyph glyph)
	{
		StubWarner.LogStub("TryGetGlyphWithUnicodeValue", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\TextCore.cs", 122);
		glyph = null;
		return false;
	}
}
