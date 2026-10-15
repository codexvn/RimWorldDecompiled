using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using Waylith.Assets;
using Waylith.Logging;

namespace RimWorldWaylith.Imgui;

internal static class FontFallbacks
{
	internal readonly record struct Source(string Path, uint FontNo, uint[]? ExcludeRanges);

	private readonly record struct Candidate(string Path, uint FontNo = 0u)
	{
		public static implicit operator Candidate(string path)
		{
			return new Candidate(path);
		}
	}

	private static readonly uint[] HangulRanges = Pin(new uint[9] { 4352u, 4607u, 12592u, 12687u, 43360u, 43391u, 44032u, 55295u, 0u });

	private static readonly uint[] HanRanges = Pin(new uint[21]
	{
		11904u, 12255u, 12288u, 12351u, 12352u, 12543u, 12736u, 12783u, 12784u, 12799u,
		13312u, 19903u, 19968u, 40959u, 63744u, 64255u, 65280u, 65519u, 131072u, 201551u,
		0u
	});

	private static readonly uint[] ThaiRanges = Pin(new uint[3] { 3584u, 3711u, 0u });

	private static readonly uint[] ScriptRanges = Pin(new uint[27]
	{
		3584u, 3711u, 4352u, 4607u, 11904u, 12255u, 12288u, 12543u, 12592u, 12687u,
		12736u, 12799u, 13312u, 19903u, 19968u, 40959u, 43360u, 43391u, 44032u, 55295u,
		63744u, 64255u, 65280u, 65519u, 131072u, 201551u, 0u
	});

	private const string ShippedHangul = "Pretendard-Regular.otf";

	private static readonly Candidate[] WindowsSymbols = new Candidate[5] { "seguisym.ttf", "seguiemj.ttf", "segoeui.ttf", "arialuni.ttf", "DejaVuSans.ttf" };

	private static readonly Candidate[] WindowsHangul = new Candidate[3] { "malgun.ttf", "gulim.ttc", "arialuni.ttf" };

	private static readonly Candidate[] WindowsThai = new Candidate[3] { "LeelawUI.ttf", "leelawad.ttf", "tahoma.ttf" };

	private static readonly Dictionary<string, Candidate[]> WindowsHan = new Dictionary<string, Candidate[]>
	{
		["JP"] = new Candidate[5] { "meiryo.ttc", "YuGothM.ttc", "msgothic.ttc", "msyh.ttc", "arialuni.ttf" },
		["KR"] = new Candidate[4] { "malgun.ttf", "gulim.ttc", "msyh.ttc", "arialuni.ttf" },
		["SC"] = new Candidate[3] { "msyh.ttc", "simsun.ttc", "arialuni.ttf" },
		["TC"] = new Candidate[4] { "msjh.ttc", "mingliu.ttc", "msyh.ttc", "arialuni.ttf" },
		[""] = new Candidate[6] { "msyh.ttc", "msyh.ttf", "meiryo.ttc", "msgothic.ttc", "simsun.ttc", "arialuni.ttf" }
	};

	private static readonly Candidate[] MacSymbols = new Candidate[4] { "/System/Library/Fonts/Apple Symbols.ttf", "/System/Library/Fonts/Supplemental/Arial Unicode.ttf", "/System/Library/Fonts/Supplemental/DejaVuSans.ttf", "/Library/Fonts/DejaVuSans.ttf" };

	private static readonly Candidate[] MacHangul = new Candidate[2] { "/System/Library/Fonts/AppleSDGothicNeo.ttc", "/System/Library/Fonts/Supplemental/AppleGothic.ttf" };

	private static readonly Candidate[] MacThai = new Candidate[3] { "/System/Library/Fonts/Supplemental/Thonburi.ttc", "/System/Library/Fonts/Thonburi.ttc", "/System/Library/Fonts/Supplemental/Ayuthaya.ttf" };

	private static readonly Dictionary<string, Candidate[]> MacHan = new Dictionary<string, Candidate[]>
	{
		["JP"] = new Candidate[4] { "/System/Library/Fonts/ヒラギノ角ゴシック W3.ttc", "/System/Library/Fonts/Hiragino Sans GB.ttc", "/System/Library/Fonts/PingFang.ttc", "/System/Library/Fonts/Supplemental/Arial Unicode.ttf" },
		["KR"] = new Candidate[3] { "/System/Library/Fonts/AppleSDGothicNeo.ttc", "/System/Library/Fonts/PingFang.ttc", "/System/Library/Fonts/Supplemental/Arial Unicode.ttf" },
		["SC"] = new Candidate[4] { "/System/Library/Fonts/PingFang.ttc", "/System/Library/Fonts/Hiragino Sans GB.ttc", "/System/Library/Fonts/STHeiti Light.ttc", "/System/Library/Fonts/Supplemental/Arial Unicode.ttf" },
		["TC"] = new Candidate[2] { "/System/Library/Fonts/PingFang.ttc", "/System/Library/Fonts/Supplemental/Arial Unicode.ttf" },
		[""] = new Candidate[4] { "/System/Library/Fonts/PingFang.ttc", "/System/Library/Fonts/Hiragino Sans GB.ttc", "/System/Library/Fonts/Supplemental/Arial Unicode.ttf", "/System/Library/Fonts/STHeiti Light.ttc" }
	};

	private static readonly Candidate[] LinuxSymbols = new Candidate[7] { "DejaVuSans.ttf", "NotoSansSymbols-Regular.ttf", "NotoSansSymbols2-Regular.ttf", "Symbola.ttf", "Symbola_hint.ttf", "LiberationSans-Regular.ttf", "unifont.ttf" };

	private const string LinuxNotoCjk = "NotoSansCJK-Regular.ttc";

	private static readonly Candidate[] LinuxOtherCjk = new Candidate[2] { "DroidSansFallbackFull.ttf", "wqy-microhei.ttc" };

	private static readonly Candidate[] LinuxHangulOnly = new Candidate[1] { "NanumGothic.ttf" };

	private static readonly Candidate[] LinuxThai = new Candidate[2] { "NotoSansThai-Regular.ttf", "Garuda.ttf" };

	private static Dictionary<string, string>? linuxFontIndex;

	public static string? LocaleFor(string? languageFolderName)
	{
		if (languageFolderName == null)
		{
			return null;
		}
		int num = languageFolderName.IndexOf('(');
		return ((num < 0) ? languageFolderName : languageFolderName.Substring(0, num)).Trim() switch
		{
			"Japanese" => "JP", 
			"Korean" => "KR", 
			"ChineseSimplified" => "SC", 
			"ChineseTraditional" => "TC", 
			_ => null, 
		};
	}

	private static string? ShippedFont(string name)
	{
		string text = Path.Combine(Application.dataPath, "Resources", "Fonts", name);
		if (!File.Exists(text) && !ContentPack.HasFile(text))
		{
			return null;
		}
		return text;
	}

	private static uint NotoFace(string locale)
	{
		return locale switch
		{
			"KR" => 1u, 
			"SC" => 2u, 
			"TC" => 3u, 
			_ => 0u, 
		};
	}

	private static Candidate[] LinuxHanFor(string locale)
	{
		Candidate candidate = new Candidate("NotoSansCJK-Regular.ttc", NotoFace(locale));
		Candidate[] linuxOtherCjk = LinuxOtherCjk;
		int num = 0;
		Candidate[] array = new Candidate[1 + linuxOtherCjk.Length];
		array[num] = candidate;
		num++;
		ReadOnlySpan<Candidate> readOnlySpan = new ReadOnlySpan<Candidate>(linuxOtherCjk);
		readOnlySpan.CopyTo(new Span<Candidate>(array).Slice(num, readOnlySpan.Length));
		num += readOnlySpan.Length;
		return array;
	}

	private static Candidate[] LinuxHangulFor()
	{
		Candidate candidate = new Candidate("NotoSansCJK-Regular.ttc", NotoFace("KR"));
		Candidate[] linuxHangulOnly = LinuxHangulOnly;
		int num = 0;
		Candidate[] array = new Candidate[1 + linuxHangulOnly.Length];
		array[num] = candidate;
		num++;
		ReadOnlySpan<Candidate> readOnlySpan = new ReadOnlySpan<Candidate>(linuxHangulOnly);
		readOnlySpan.CopyTo(new Span<Candidate>(array).Slice(num, readOnlySpan.Length));
		num += readOnlySpan.Length;
		return array;
	}

	private static string? LinuxFont(string fileName)
	{
		if (linuxFontIndex == null)
		{
			linuxFontIndex = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			string[] pathsToOSFonts = Font.GetPathsToOSFonts();
			foreach (string text in pathsToOSFonts)
			{
				linuxFontIndex.TryAdd(Path.GetFileName(text), text);
			}
		}
		return linuxFontIndex.GetValueOrDefault(fileName);
	}

	public static IReadOnlyList<Source> Resolve(string? locale)
	{
		string text = locale ?? "";
		List<Source> list = new List<Source>();
		Func<string, string> resolve;
		Candidate[] candidates;
		Candidate[] candidates2;
		Candidate[] candidates3;
		Candidate[] candidates4;
		if (OperatingSystem.IsWindows())
		{
			string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Fonts");
			resolve = (string name) => Existing(Path.Combine(dir, name));
			Candidate[] windowsSymbols = WindowsSymbols;
			Candidate[] windowsHangul = WindowsHangul;
			Candidate[] windowsThai = WindowsThai;
			candidates = WindowsHan[text];
			candidates2 = windowsThai;
			candidates3 = windowsHangul;
			candidates4 = windowsSymbols;
		}
		else if (OperatingSystem.IsMacOS())
		{
			resolve = Existing;
			Candidate[] macSymbols = MacSymbols;
			Candidate[] macHangul = MacHangul;
			Candidate[] macThai = MacThai;
			candidates = MacHan[text];
			candidates2 = macThai;
			candidates3 = macHangul;
			candidates4 = macSymbols;
		}
		else
		{
			resolve = LinuxFont;
			Candidate[] linuxSymbols = LinuxSymbols;
			Candidate[] array = LinuxHangulFor();
			Candidate[] linuxThai = LinuxThai;
			candidates = LinuxHanFor(text);
			candidates2 = linuxThai;
			candidates3 = array;
			candidates4 = linuxSymbols;
		}
		Add(list, candidates4, resolve, firstOnly: false, ScriptRanges);
		Add(list, new Candidate[1] { "Pretendard-Regular.otf" }, ShippedFont, firstOnly: true, HanRanges);
		Add(list, candidates3, resolve, firstOnly: true, HanRanges);
		Add(list, candidates, resolve, firstOnly: true, null);
		Add(list, candidates2, resolve, firstOnly: true, null);
		Add(list, candidates4, resolve, firstOnly: false, null);
		if (list.Count == 0)
		{
			Console.Error.WriteLine("[ImGuiHost] No OS fallback fonts found, non-latin text will render as missing glyph boxes.");
		}
		else
		{
			VerboseLog.Log("ImGuiHost: fallback fonts (" + (locale ?? "default") + ") " + string.Join(", ", list.Select((Source f) => (f.FontNo != 0) ? $"{f.Path}#{f.FontNo}" : f.Path)));
		}
		return list;
	}

	private static string? Existing(string path)
	{
		if (!File.Exists(path))
		{
			return null;
		}
		return path;
	}

	private static void Add(List<Source> into, Candidate[] candidates, Func<string, string?> resolve, bool firstOnly, uint[]? excludeRanges)
	{
		for (int i = 0; i < candidates.Length; i++)
		{
			Candidate candidate = candidates[i];
			string text = resolve(candidate.Path);
			if (text != null)
			{
				Source item = new Source(text, candidate.FontNo, excludeRanges);
				if (!into.Contains(item))
				{
					into.Add(item);
				}
				if (firstOnly)
				{
					break;
				}
			}
		}
	}

	private static uint[] Pin(uint[] ranges)
	{
		GCHandle.Alloc(ranges, GCHandleType.Pinned);
		return ranges;
	}
}
