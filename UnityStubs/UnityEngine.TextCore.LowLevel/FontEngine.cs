using UnityStubs.Internal;

namespace UnityEngine.TextCore.LowLevel;

public static class FontEngine
{
	public static FontEngineError InitializeFontEngine()
	{
		StubWarner.LogStub("InitializeFontEngine", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\TextCore.cs", 79);
		return FontEngineError.Invalid_Library;
	}

	public static FontEngineError DestroyFontEngine()
	{
		StubWarner.LogStub("DestroyFontEngine", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\TextCore.cs", 80);
		return FontEngineError.Success;
	}

	public static FontEngineError LoadFontFace(string filePath)
	{
		StubWarner.LogStub("LoadFontFace", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\TextCore.cs", 81);
		return FontEngineError.Invalid_File_Path;
	}

	public static FontEngineError LoadFontFace(string filePath, int pointSize)
	{
		StubWarner.LogStub("LoadFontFace", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\TextCore.cs", 82);
		return FontEngineError.Invalid_File_Path;
	}

	public static FontEngineError LoadFontFace(byte[] sourceFontFile)
	{
		StubWarner.LogStub("LoadFontFace", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\TextCore.cs", 83);
		return FontEngineError.Invalid_File;
	}

	public static FontEngineError LoadFontFace(byte[] sourceFontFile, int pointSize)
	{
		StubWarner.LogStub("LoadFontFace", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\TextCore.cs", 84);
		return FontEngineError.Invalid_File;
	}

	public static FontEngineError LoadFontFace(Font font)
	{
		StubWarner.LogStub("LoadFontFace", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\TextCore.cs", 85);
		return FontEngineError.Invalid_Face;
	}

	public static FontEngineError LoadFontFace(Font font, int pointSize)
	{
		StubWarner.LogStub("LoadFontFace", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\TextCore.cs", 86);
		return FontEngineError.Invalid_Face;
	}

	public static FontEngineError UnloadFontFace()
	{
		StubWarner.LogStub("UnloadFontFace", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\TextCore.cs", 87);
		return FontEngineError.Success;
	}

	public static FaceInfo GetFaceInfo()
	{
		StubWarner.LogStub("GetFaceInfo", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\TextCore.cs", 88);
		return default(FaceInfo);
	}

	public static FontEngineError SetFaceSize(int pointSize)
	{
		StubWarner.LogStub("SetFaceSize", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\TextCore.cs", 89);
		return FontEngineError.Invalid_Face;
	}

	public static bool TryGetGlyphIndex(uint unicode, out uint glyphIndex)
	{
		StubWarner.LogStub("TryGetGlyphIndex", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\TextCore.cs", 90);
		glyphIndex = 0u;
		return false;
	}

	public static bool TryGetGlyphWithIndexValue(uint glyphIndex, GlyphLoadFlags flags, out Glyph glyph)
	{
		StubWarner.LogStub("TryGetGlyphWithIndexValue", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\TextCore.cs", 91);
		glyph = null;
		return false;
	}

	public static bool TryGetGlyphWithUnicodeValue(uint unicode, GlyphLoadFlags flags, out Glyph glyph)
	{
		StubWarner.LogStub("TryGetGlyphWithUnicodeValue", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\TextCore.cs", 92);
		glyph = null;
		return false;
	}
}
