using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using RimWorld;
using RimWorld.IO;

namespace Verse;

public static class BackstoryTranslationUtility
{
	[CompilerGenerated]
	private sealed class _003CBackstoryTranslationElements_003Ed__3 : IEnumerable<XElement>, IEnumerable, IEnumerator<XElement>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private XElement _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IEnumerable<Tuple<VirtualDirectory, ModContentPack, string>> folders;

		public IEnumerable<Tuple<VirtualDirectory, ModContentPack, string>> _003C_003E3__folders;

		private List<string> loadErrors;

		public List<string> _003C_003E3__loadErrors;

		private Dictionary<ModContentPack, HashSet<string>> _003CalreadyLoadedFiles_003E5__2;

		private IEnumerator<Tuple<VirtualDirectory, ModContentPack, string>> _003C_003E7__wrap2;

		private IEnumerator<XElement> _003C_003E7__wrap3;

		XElement IEnumerator<XElement>.Current
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
		public _003CBackstoryTranslationElements_003Ed__3(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || num == 1)
			{
				try
				{
					if (num == -4 || num == 1)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003CalreadyLoadedFiles_003E5__2 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E7__wrap3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			bool result;
			try
			{
				int num = _003C_003E1__state;
				if (num == 0)
				{
					_003C_003E1__state = -1;
					_003CalreadyLoadedFiles_003E5__2 = new Dictionary<ModContentPack, HashSet<string>>();
					_003C_003E7__wrap2 = folders.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_01d0;
				}
				if (num == 1)
				{
					_003C_003E1__state = -4;
					goto IL_01b6;
				}
				result = false;
				goto end_IL_0000;
				IL_01b6:
				if (!_003C_003E7__wrap3.MoveNext())
				{
					_003C_003Em__Finally2();
					_003C_003E7__wrap3 = null;
					goto IL_01d0;
				}
				XElement current = _003C_003E7__wrap3.Current;
				_003C_003E2__current = current;
				_003C_003E1__state = 1;
				result = true;
				goto end_IL_0000;
				IL_01d0:
				while (true)
				{
					if (_003C_003E7__wrap2.MoveNext())
					{
						Tuple<VirtualDirectory, ModContentPack, string> current2 = _003C_003E7__wrap2.Current;
						if (!_003CalreadyLoadedFiles_003E5__2.ContainsKey(current2.Item2))
						{
							_003CalreadyLoadedFiles_003E5__2[current2.Item2] = new HashSet<string>();
						}
						VirtualFile file = current2.Item1.GetFile("Backstories/Backstories.xml");
						if (!file.Exists)
						{
							continue;
						}
						if (!file.FullPath.StartsWith(current2.Item3))
						{
							Log.Error("Failed to get a relative path for a file: " + file.FullPath + ", located in " + current2.Item3);
							continue;
						}
						string item = file.FullPath.Substring(current2.Item3.Length);
						if (_003CalreadyLoadedFiles_003E5__2[current2.Item2].Contains(item))
						{
							continue;
						}
						_003CalreadyLoadedFiles_003E5__2[current2.Item2].Add(item);
						XDocument xDocument;
						try
						{
							xDocument = file.LoadAsXDocument();
						}
						catch (Exception ex)
						{
							if (loadErrors != null)
							{
								loadErrors.Add("Exception loading backstory translation data from file " + file?.ToString() + ": " + ex);
							}
							result = false;
							goto IL_01e8;
						}
						_003C_003E7__wrap3 = xDocument.Root.Elements().GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_01b6;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = null;
					result = false;
					break;
					IL_01e8:
					_003C_003Em__Finally1();
					break;
				}
				end_IL_0000:;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
			return result;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
			if (_003C_003E7__wrap3 != null)
			{
				_003C_003E7__wrap3.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<XElement> IEnumerable<XElement>.GetEnumerator()
		{
			_003CBackstoryTranslationElements_003Ed__3 _003CBackstoryTranslationElements_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CBackstoryTranslationElements_003Ed__ = this;
			}
			else
			{
				_003CBackstoryTranslationElements_003Ed__ = new _003CBackstoryTranslationElements_003Ed__3(0);
			}
			_003CBackstoryTranslationElements_003Ed__.folders = _003C_003E3__folders;
			_003CBackstoryTranslationElements_003Ed__.loadErrors = _003C_003E3__loadErrors;
			return _003CBackstoryTranslationElements_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<XElement>)this).GetEnumerator();
		}
	}

	public const string BackstoriesFolder = "Backstories";

	public const string BackstoriesFileName = "Backstories.xml";

	public const string BackstoriesFolderLegacy = "Backstories DELETE_ME";

	private static Regex regex = new Regex("^[^0-9]*");

	private static List<BackstoryDef> tmpAllBackstories = new List<BackstoryDef>();

	[IteratorStateMachine(typeof(_003CBackstoryTranslationElements_003Ed__3))]
	public static IEnumerable<XElement> BackstoryTranslationElements(IEnumerable<Tuple<VirtualDirectory, ModContentPack, string>> folders, List<string> loadErrors = null)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CBackstoryTranslationElements_003Ed__3(-2)
		{
			_003C_003E3__folders = folders,
			_003C_003E3__loadErrors = loadErrors
		};
	}

	public static bool AnyLegacyBackstoryFiles(IEnumerable<Tuple<VirtualDirectory, ModContentPack, string>> folders)
	{
		foreach (Tuple<VirtualDirectory, ModContentPack, string> folder in folders)
		{
			if (folder.Item1.GetFile("Backstories/Backstories.xml").Exists)
			{
				return true;
			}
		}
		return false;
	}

	public static void LoadAndInjectBackstoryData(IEnumerable<Tuple<VirtualDirectory, ModContentPack, string>> folderPaths, List<string> loadErrors = null)
	{
		foreach (XElement item in BackstoryTranslationElements(folderPaths, loadErrors))
		{
			string text = "[unknown]";
			try
			{
				text = item.Name.ToString();
				string text2 = GetText(item, "title");
				string text3 = GetText(item, "titleFemale");
				string text4 = GetText(item, "titleShort");
				string text5 = GetText(item, "titleShortFemale");
				string text6 = GetText(item, "desc");
				if (!TryGetWithIdentifier(text, out var backstory, closestMatchWarning: false))
				{
					throw new Exception("Backstory not found matching identifier " + text);
				}
				if (text2 == backstory.title && text3 == backstory.titleFemale && text4 == backstory.titleShort && text5 == backstory.titleShortFemale && text6 == backstory.description)
				{
					throw new Exception("Backstory translation exactly matches default data: " + text);
				}
				if (text2 != null)
				{
					backstory.SetTitle(text2, backstory.titleFemale);
				}
				if (text3 != null)
				{
					backstory.SetTitle(backstory.title, text3);
				}
				if (text4 != null)
				{
					backstory.SetTitleShort(text4, backstory.titleShortFemale);
				}
				if (text5 != null)
				{
					backstory.SetTitleShort(backstory.titleShort, text5);
				}
				if (text6 != null)
				{
					backstory.description = text6;
				}
			}
			catch (Exception ex)
			{
				loadErrors?.Add("Couldn't load backstory " + text + ": " + ex.Message + "\nFull XML text:\n\n" + item);
			}
		}
	}

	public static List<DefInjectionPackage.DefInjection> GetLegacyBackstoryTranslations(IEnumerable<Tuple<VirtualDirectory, ModContentPack, string>> folderPaths)
	{
		List<DefInjectionPackage.DefInjection> list = new List<DefInjectionPackage.DefInjection>();
		foreach (XElement item in BackstoryTranslationElements(folderPaths))
		{
			try
			{
				string text = item.Name.ToString();
				if (TryGetWithIdentifier(text, out var backstory))
				{
					string text2 = GetText(item, "title");
					string text3 = GetText(item, "titleFemale");
					string text4 = GetText(item, "titleShort");
					string text5 = GetText(item, "titleShortFemale");
					string text6 = GetText(item, "desc");
					if (!text2.NullOrEmpty() && text2 != "TODO")
					{
						DefInjectionPackage.DefInjection defInjection = new DefInjectionPackage.DefInjection();
						defInjection.path = text + ".title";
						defInjection.suggestedPath = defInjection.path;
						defInjection.replacedString = backstory.untranslatedTitle;
						defInjection.injection = text2;
						defInjection.injected = true;
						list.Add(defInjection);
					}
					if (!text3.NullOrEmpty() && text3 != "TODO")
					{
						DefInjectionPackage.DefInjection defInjection2 = new DefInjectionPackage.DefInjection();
						defInjection2.path = text + ".titleFemale";
						defInjection2.suggestedPath = defInjection2.path;
						defInjection2.replacedString = backstory.untranslatedTitleFemale;
						defInjection2.injection = text3;
						defInjection2.injected = true;
						list.Add(defInjection2);
					}
					if (!text4.NullOrEmpty() && text4 != "TODO")
					{
						DefInjectionPackage.DefInjection defInjection3 = new DefInjectionPackage.DefInjection();
						defInjection3.path = text + ".titleShort";
						defInjection3.suggestedPath = defInjection3.path;
						defInjection3.replacedString = backstory.untranslatedTitleShort;
						defInjection3.injection = text4;
						defInjection3.injected = true;
						list.Add(defInjection3);
					}
					if (!text5.NullOrEmpty() && text5 != "TODO")
					{
						DefInjectionPackage.DefInjection defInjection4 = new DefInjectionPackage.DefInjection();
						defInjection4.path = text + ".titleShortFemale";
						defInjection4.suggestedPath = defInjection4.path;
						defInjection4.replacedString = backstory.untranslatedTitleShortFemale;
						defInjection4.injection = text5;
						defInjection4.injected = true;
						list.Add(defInjection4);
					}
					if (!text6.NullOrEmpty() && text6 != "TODO")
					{
						DefInjectionPackage.DefInjection defInjection5 = new DefInjectionPackage.DefInjection();
						defInjection5.path = text + ".description";
						defInjection5.suggestedPath = defInjection5.path;
						defInjection5.replacedString = backstory.untranslatedDesc;
						defInjection5.injection = text6;
						defInjection5.injected = true;
						list.Add(defInjection5);
					}
				}
			}
			catch (Exception ex)
			{
				Log.Error("Error Getting legacy backstory translations: " + ex);
			}
		}
		return list;
	}

	public static string StripNumericSuffix(string key)
	{
		return regex.Match(key).Captures[0].Value;
	}

	private static bool TryGetWithIdentifier(string identifier, out BackstoryDef backstory, bool closestMatchWarning = true)
	{
		backstory = DefDatabase<BackstoryDef>.AllDefs.FirstOrDefault((BackstoryDef b) => b.defName == identifier);
		if (backstory == null)
		{
			string strippedDefName = StripNumericSuffix(identifier);
			backstory = tmpAllBackstories.FirstOrDefault((BackstoryDef b) => StripNumericSuffix(b.defName) == strippedDefName);
			if (backstory != null && closestMatchWarning)
			{
				Log.Warning("Couldn't find exact match for backstory " + identifier + ", using closest match " + backstory.identifier);
			}
		}
		return backstory != null;
	}

	public static List<string> MissingBackstoryTranslations(LoadedLanguage lang)
	{
		List<string> list = new List<string>();
		tmpAllBackstories.Clear();
		tmpAllBackstories.AddRange(DefDatabase<BackstoryDef>.AllDefs);
		foreach (XElement item in BackstoryTranslationElements(lang.AllDirectories))
		{
			try
			{
				string text = item.Name.ToString();
				TryGetWithIdentifier(text, out var backstory);
				if (backstory == null)
				{
					list.Add("Translation doesn't correspond to any backstory: " + text);
					continue;
				}
				tmpAllBackstories.Remove(backstory);
				string text2 = GetText(item, "title");
				string text3 = GetText(item, "titleFemale");
				string text4 = GetText(item, "titleShort");
				string text5 = GetText(item, "titleShortFemale");
				string text6 = GetText(item, "desc");
				if (text2.NullOrEmpty())
				{
					list.Add(text + ".title missing");
				}
				if (!backstory.titleFemale.NullOrEmpty() && text3.NullOrEmpty())
				{
					list.Add(text + ".titleFemale missing");
				}
				if (text4.NullOrEmpty())
				{
					list.Add(text + ".titleShort missing");
				}
				if (!backstory.titleShortFemale.NullOrEmpty() && text5.NullOrEmpty())
				{
					list.Add(text + ".titleShortFemale missing");
				}
				if (text6.NullOrEmpty())
				{
					list.Add(text + ".desc missing");
				}
			}
			catch (Exception ex)
			{
				list.Add("Exception reading " + item.Name?.ToString() + ": " + ex.Message);
			}
		}
		foreach (BackstoryDef tmpAllBackstory in tmpAllBackstories)
		{
			list.Add("Missing backstory: " + tmpAllBackstory.defName);
		}
		tmpAllBackstories.Clear();
		return list;
	}

	public static List<string> BackstoryTranslationsMatchingEnglish(LoadedLanguage lang)
	{
		List<string> list = new List<string>();
		foreach (XElement item in BackstoryTranslationElements(lang.AllDirectories))
		{
			try
			{
				string identifier = item.Name.ToString();
				if (DefDatabase<BackstoryDef>.AllDefs.Where((BackstoryDef b) => b.defName == identifier).TryRandomElement(out var result))
				{
					string text = GetText(item, "title");
					string text2 = GetText(item, "titleFemale");
					string text3 = GetText(item, "titleShort");
					string text4 = GetText(item, "titleShortFemale");
					string text5 = GetText(item, "desc");
					if (!text.NullOrEmpty() && text == result.untranslatedTitle)
					{
						list.Add(identifier + ".title '" + text.Replace("\n", "\\n") + "'");
					}
					if (!text2.NullOrEmpty() && text2 == result.untranslatedTitleFemale)
					{
						list.Add(identifier + ".titleFemale '" + text2.Replace("\n", "\\n") + "'");
					}
					if (!text3.NullOrEmpty() && text3 == result.untranslatedTitleShort)
					{
						list.Add(identifier + ".titleShort '" + text3.Replace("\n", "\\n") + "'");
					}
					if (!text4.NullOrEmpty() && text4 == result.untranslatedTitleShortFemale)
					{
						list.Add(identifier + ".titleShortFemale '" + text4.Replace("\n", "\\n") + "'");
					}
					if (!text5.NullOrEmpty() && text5 == result.untranslatedDesc)
					{
						list.Add(identifier + ".desc '" + text5.Replace("\n", "\\n") + "'");
					}
				}
			}
			catch (Exception ex)
			{
				list.Add("Exception reading " + item.Name?.ToString() + ": " + ex.Message);
			}
		}
		return list;
	}

	public static List<string> ObsoleteBackstoryTranslations(LoadedLanguage lang)
	{
		List<string> list = new List<string>();
		foreach (XElement item in BackstoryTranslationElements(lang.AllDirectories))
		{
			if (TryGetWithIdentifier(item.Name.ToString(), out var backstory))
			{
				list.Add("Obsolete backstory format: " + backstory.defName);
			}
		}
		return list;
	}

	private static string GetText(XElement backstory, string fieldName)
	{
		XElement xElement = backstory.Element(fieldName);
		if (xElement == null || xElement.Value == "TODO")
		{
			return null;
		}
		return xElement.Value.Replace("\\n", "\n");
	}
}
