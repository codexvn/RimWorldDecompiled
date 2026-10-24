using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Waylith.Logging;

namespace UnityStubs.Internal;

internal static class LinuxFontconfig
{
	private struct FcFontSet
	{
		public int nfont;

		public int sfont;

		public unsafe nint* fonts;
	}

	private const string Library = "libfontconfig.so.1";

	private const int FcSetSystem = 0;

	private const int FcResultMatch = 0;

	internal unsafe static List<string> FontFiles()
	{
		List<string> list = new List<string>();
		if (!OperatingSystem.IsLinux())
		{
			return list;
		}
		nint num = 0;
		try
		{
			num = FcInitLoadConfigAndFonts();
			if (num == 0)
			{
				return list;
			}
			FcFontSet* ptr = FcConfigGetFonts(num, 0);
			if (ptr == null)
			{
				return list;
			}
			fixed (byte* obj = (ReadOnlySpan<byte>)new byte[5] { 102, 105, 108, 101, 0 })
			{
				byte* ptr2 = default(byte*);
				for (int i = 0; i < ptr->nfont; i++)
				{
					if (FcPatternGetString(ptr->fonts[i], obj, 0, &ptr2) == 0 && ptr2 != null)
					{
						list.Add(Marshal.PtrToStringUTF8((nint)ptr2));
					}
				}
			}
		}
		catch (Exception ex) when (((ex is DllNotFoundException || ex is EntryPointNotFoundException) ? 1 : 0) != 0)
		{
		}
		finally
		{
			if (num != 0)
			{
				FcConfigDestroy(num);
			}
		}
		VerboseLog.Log($"Located {list.Count} fonts from libfontconfig");
		return list;
	}

	[DllImport("libfontconfig.so.1", ExactSpelling = true)]
	[LibraryImport("libfontconfig.so.1")]
	private static extern nint FcInitLoadConfigAndFonts();

	[DllImport("libfontconfig.so.1", ExactSpelling = true)]
	[LibraryImport("libfontconfig.so.1")]
	private unsafe static extern FcFontSet* FcConfigGetFonts(nint config, int set);

	[DllImport("libfontconfig.so.1", ExactSpelling = true)]
	[LibraryImport("libfontconfig.so.1")]
	private unsafe static extern int FcPatternGetString(nint pattern, byte* obj, int n, byte** s);

	[DllImport("libfontconfig.so.1", ExactSpelling = true)]
	[LibraryImport("libfontconfig.so.1")]
	private static extern void FcConfigDestroy(nint config);
}
