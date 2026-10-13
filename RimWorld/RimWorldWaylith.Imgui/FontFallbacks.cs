using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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

	private static readonly Candidate[] LinuxSymbols = new Candidate[15]
	{
		"/usr/share/fonts/TTF/DejaVuSans.ttf", "/usr/share/fonts/truetype/dejavu/DejaVuSans.ttf", "/usr/share/fonts/dejavu/DejaVuSans.ttf", "/usr/share/fonts/noto/NotoSansSymbols-Regular.ttf", "/usr/share/fonts/truetype/noto/NotoSansSymbols-Regular.ttf", "/usr/share/fonts/noto/NotoSansSymbols2-Regular.ttf", "/usr/share/fonts/truetype/noto/NotoSansSymbols2-Regular.ttf", "/usr/share/fonts/TTF/Symbola.ttf", "/usr/share/fonts/truetype/ancient-scripts/Symbola_hint.ttf", "/usr/share/fonts/gdouros-symbola/Symbola.ttf",
		"/usr/share/fonts/liberation/LiberationSans-Regular.ttf", "/usr/share/fonts/truetype/liberation/LiberationSans-Regular.ttf", "/usr/share/fonts/TTF/LiberationSans-Regular.ttf", "/usr/share/fonts/misc/unifont.ttf", "/usr/share/fonts/truetype/unifont/unifont.ttf"
	};

	private static readonly string[] LinuxNotoCjk = new string[3] { "/usr/share/fonts/noto-cjk/NotoSansCJK-Regular.ttc", "/usr/share/fonts/opentype/noto/NotoSansCJK-Regular.ttc", "/usr/share/fonts/noto/NotoSansCJK-Regular.ttc" };

	private static readonly Candidate[] LinuxOtherCjk = new Candidate[4] { "/usr/share/fonts/truetype/droid/DroidSansFallbackFull.ttf", "/usr/share/fonts/droid/DroidSansFallbackFull.ttf", "/usr/share/fonts/wenquanyi/wqy-microhei/wqy-microhei.ttc", "/usr/share/fonts/truetype/wqy/wqy-microhei.ttc" };

	private static readonly Candidate[] LinuxHangulOnly = new Candidate[3] { "/usr/share/fonts/truetype/nanum/NanumGothic.ttf", "/usr/share/fonts/nanum/NanumGothic.ttf", "/usr/share/fonts/naver-nanum/NanumGothic.ttf" };

	private static readonly Candidate[] LinuxThai = new Candidate[4] { "/usr/share/fonts/noto/NotoSansThai-Regular.ttf", "/usr/share/fonts/truetype/noto/NotoSansThai-Regular.ttf", "/usr/share/fonts/truetype/tlwg/Garuda.ttf", "/usr/share/fonts/TTF/Garuda.ttf" };

	public static string? LocaleFor(string? languageFolderName)
	{
		return languageFolderName switch
		{
			"Japanese" => "JP", 
			"Korean" => "KR", 
			"ChineseSimplified" => "SC", 
			"ChineseTraditional" => "TC", 
			_ => null, 
		};
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
		List<Candidate> list = new List<Candidate>();
		string[] linuxNotoCjk = LinuxNotoCjk;
		foreach (string path in linuxNotoCjk)
		{
			list.Add(new Candidate(path, NotoFace(locale)));
		}
		list.AddRange(LinuxOtherCjk);
		return list.ToArray();
	}

	private static Candidate[] LinuxHangulFor()
	{
		List<Candidate> list = new List<Candidate>();
		list.AddRange(LinuxNotoCjk.Select((string p) => new Candidate(p, NotoFace("KR"))));
		list.AddRange(LinuxHangulOnly);
		return list.ToArray();
	}

	public static IReadOnlyList<Source> Resolve(string? locale)
	{
		string text = locale ?? "";
		List<Source> list = new List<Source>();
		string dir = null;
		Candidate[] candidates;
		Candidate[] candidates2;
		Candidate[] candidates3;
		Candidate[] candidates4;
		if (OperatingSystem.IsWindows())
		{
			dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Fonts");
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
			Candidate[] linuxSymbols = LinuxSymbols;
			Candidate[] array = LinuxHangulFor();
			Candidate[] linuxThai = LinuxThai;
			candidates = LinuxHanFor(text);
			candidates2 = linuxThai;
			candidates3 = array;
			candidates4 = linuxSymbols;
		}
		Add(list, candidates4, dir, firstOnly: false, ScriptRanges);
		Add(list, candidates3, dir, firstOnly: true, HanRanges);
		Add(list, candidates, dir, firstOnly: true, null);
		Add(list, candidates2, dir, firstOnly: true, null);
		Add(list, candidates4, dir, firstOnly: false, null);
		if (list.Count == 0)
		{
			Console.Error.WriteLine("[ImGuiHost] No OS fallback fonts found, non-latin text will render as '?'.");
		}
		else
		{
			VerboseLog.Log("ImGuiHost: fallback fonts (" + (locale ?? "default") + ") " + string.Join(", ", list.Select((Source f) => (f.FontNo != 0) ? $"{f.Path}#{f.FontNo}" : f.Path)));
		}
		return list;
	}

	private static void Add(List<Source> into, Candidate[] candidates, string? dir, bool firstOnly, uint[]? excludeRanges)
	{
		for (int i = 0; i < candidates.Length; i++)
		{
			Candidate candidate = candidates[i];
			string path = ((dir == null) ? candidate.Path : Path.Combine(dir, candidate.Path));
			if (File.Exists(path))
			{
				Source item = new Source(path, candidate.FontNo, excludeRanges);
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
