using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using RimWorld;
using RimWorld.IO;

namespace Verse
{
	// Token: 0x02000368 RID: 872
	public static class BackstoryTranslationUtility
	{
		// Token: 0x0600166F RID: 5743 RVA: 0x0006D904 File Offset: 0x0006BB04
		public static IEnumerable<XElement> BackstoryTranslationElements(IEnumerable<Tuple<VirtualDirectory, ModContentPack, string>> folders, List<string> loadErrors = null)
		{
			Dictionary<ModContentPack, HashSet<string>> alreadyLoadedFiles = new Dictionary<ModContentPack, HashSet<string>>();
			foreach (Tuple<VirtualDirectory, ModContentPack, string> tuple in folders)
			{
				if (!alreadyLoadedFiles.ContainsKey(tuple.Item2))
				{
					alreadyLoadedFiles[tuple.Item2] = new HashSet<string>();
				}
				VirtualFile file = tuple.Item1.GetFile("Backstories/Backstories.xml");
				if (file.Exists)
				{
					if (!file.FullPath.StartsWith(tuple.Item3))
					{
						Log.Error("Failed to get a relative path for a file: " + file.FullPath + ", located in " + tuple.Item3);
					}
					else
					{
						string item = file.FullPath.Substring(tuple.Item3.Length);
						if (!alreadyLoadedFiles[tuple.Item2].Contains(item))
						{
							alreadyLoadedFiles[tuple.Item2].Add(item);
							XDocument xdocument;
							try
							{
								xdocument = file.LoadAsXDocument();
							}
							catch (Exception ex)
							{
								if (loadErrors != null)
								{
									string str = "Exception loading backstory translation data from file ";
									VirtualFile virtualFile = file;
									string str2 = (virtualFile != null) ? virtualFile.ToString() : null;
									string str3 = ": ";
									Exception ex2 = ex;
									loadErrors.Add(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
								}
								yield break;
							}
							foreach (XElement xelement in xdocument.Root.Elements())
							{
								yield return xelement;
							}
							IEnumerator<XElement> enumerator2 = null;
						}
					}
				}
			}
			IEnumerator<Tuple<VirtualDirectory, ModContentPack, string>> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06001670 RID: 5744 RVA: 0x0006D91C File Offset: 0x0006BB1C
		public static bool AnyLegacyBackstoryFiles(IEnumerable<Tuple<VirtualDirectory, ModContentPack, string>> folders)
		{
			using (IEnumerator<Tuple<VirtualDirectory, ModContentPack, string>> enumerator = folders.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Item1.GetFile("Backstories/Backstories.xml").Exists)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001671 RID: 5745 RVA: 0x0006D97C File Offset: 0x0006BB7C
		public static void LoadAndInjectBackstoryData(IEnumerable<Tuple<VirtualDirectory, ModContentPack, string>> folderPaths, List<string> loadErrors = null)
		{
			foreach (XElement xelement in BackstoryTranslationUtility.BackstoryTranslationElements(folderPaths, loadErrors))
			{
				string text = "[unknown]";
				try
				{
					text = xelement.Name.ToString();
					string text2 = BackstoryTranslationUtility.GetText(xelement, "title");
					string text3 = BackstoryTranslationUtility.GetText(xelement, "titleFemale");
					string text4 = BackstoryTranslationUtility.GetText(xelement, "titleShort");
					string text5 = BackstoryTranslationUtility.GetText(xelement, "titleShortFemale");
					string text6 = BackstoryTranslationUtility.GetText(xelement, "desc");
					BackstoryDef backstoryDef;
					if (!BackstoryTranslationUtility.TryGetWithIdentifier(text, out backstoryDef, false))
					{
						throw new Exception("Backstory not found matching identifier " + text);
					}
					if (text2 == backstoryDef.title && text3 == backstoryDef.titleFemale && text4 == backstoryDef.titleShort && text5 == backstoryDef.titleShortFemale && text6 == backstoryDef.description)
					{
						throw new Exception("Backstory translation exactly matches default data: " + text);
					}
					if (text2 != null)
					{
						backstoryDef.SetTitle(text2, backstoryDef.titleFemale);
					}
					if (text3 != null)
					{
						backstoryDef.SetTitle(backstoryDef.title, text3);
					}
					if (text4 != null)
					{
						backstoryDef.SetTitleShort(text4, backstoryDef.titleShortFemale);
					}
					if (text5 != null)
					{
						backstoryDef.SetTitleShort(backstoryDef.titleShort, text5);
					}
					if (text6 != null)
					{
						backstoryDef.description = text6;
					}
				}
				catch (Exception ex)
				{
					if (loadErrors != null)
					{
						string[] array = new string[6];
						array[0] = "Couldn't load backstory ";
						array[1] = text;
						array[2] = ": ";
						array[3] = ex.Message;
						array[4] = "\nFull XML text:\n\n";
						int num = 5;
						XElement xelement2 = xelement;
						array[num] = ((xelement2 != null) ? xelement2.ToString() : null);
						loadErrors.Add(string.Concat(array));
					}
				}
			}
		}

		// Token: 0x06001672 RID: 5746 RVA: 0x0006DB68 File Offset: 0x0006BD68
		public static List<DefInjectionPackage.DefInjection> GetLegacyBackstoryTranslations(IEnumerable<Tuple<VirtualDirectory, ModContentPack, string>> folderPaths)
		{
			List<DefInjectionPackage.DefInjection> list = new List<DefInjectionPackage.DefInjection>();
			foreach (XElement xelement in BackstoryTranslationUtility.BackstoryTranslationElements(folderPaths, null))
			{
				try
				{
					string text = xelement.Name.ToString();
					BackstoryDef backstoryDef;
					if (BackstoryTranslationUtility.TryGetWithIdentifier(text, out backstoryDef, true))
					{
						string text2 = BackstoryTranslationUtility.GetText(xelement, "title");
						string text3 = BackstoryTranslationUtility.GetText(xelement, "titleFemale");
						string text4 = BackstoryTranslationUtility.GetText(xelement, "titleShort");
						string text5 = BackstoryTranslationUtility.GetText(xelement, "titleShortFemale");
						string text6 = BackstoryTranslationUtility.GetText(xelement, "desc");
						if (!text2.NullOrEmpty() && text2 != "TODO")
						{
							DefInjectionPackage.DefInjection defInjection = new DefInjectionPackage.DefInjection();
							defInjection.path = text + ".title";
							defInjection.suggestedPath = defInjection.path;
							defInjection.replacedString = backstoryDef.untranslatedTitle;
							defInjection.injection = text2;
							defInjection.injected = true;
							list.Add(defInjection);
						}
						if (!text3.NullOrEmpty() && text3 != "TODO")
						{
							DefInjectionPackage.DefInjection defInjection2 = new DefInjectionPackage.DefInjection();
							defInjection2.path = text + ".titleFemale";
							defInjection2.suggestedPath = defInjection2.path;
							defInjection2.replacedString = backstoryDef.untranslatedTitleFemale;
							defInjection2.injection = text3;
							defInjection2.injected = true;
							list.Add(defInjection2);
						}
						if (!text4.NullOrEmpty() && text4 != "TODO")
						{
							DefInjectionPackage.DefInjection defInjection3 = new DefInjectionPackage.DefInjection();
							defInjection3.path = text + ".titleShort";
							defInjection3.suggestedPath = defInjection3.path;
							defInjection3.replacedString = backstoryDef.untranslatedTitleShort;
							defInjection3.injection = text4;
							defInjection3.injected = true;
							list.Add(defInjection3);
						}
						if (!text5.NullOrEmpty() && text5 != "TODO")
						{
							DefInjectionPackage.DefInjection defInjection4 = new DefInjectionPackage.DefInjection();
							defInjection4.path = text + ".titleShortFemale";
							defInjection4.suggestedPath = defInjection4.path;
							defInjection4.replacedString = backstoryDef.untranslatedTitleShortFemale;
							defInjection4.injection = text5;
							defInjection4.injected = true;
							list.Add(defInjection4);
						}
						if (!text6.NullOrEmpty() && text6 != "TODO")
						{
							DefInjectionPackage.DefInjection defInjection5 = new DefInjectionPackage.DefInjection();
							defInjection5.path = text + ".description";
							defInjection5.suggestedPath = defInjection5.path;
							defInjection5.replacedString = backstoryDef.untranslatedDesc;
							defInjection5.injection = text6;
							defInjection5.injected = true;
							list.Add(defInjection5);
						}
					}
				}
				catch (Exception ex)
				{
					string str = "Error Getting legacy backstory translations: ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
			return list;
		}

		// Token: 0x06001673 RID: 5747 RVA: 0x0006DE5C File Offset: 0x0006C05C
		public static string StripNumericSuffix(string key)
		{
			return BackstoryTranslationUtility.regex.Match(key).Captures[0].Value;
		}

		// Token: 0x06001674 RID: 5748 RVA: 0x0006DE7C File Offset: 0x0006C07C
		private static bool TryGetWithIdentifier(string identifier, out BackstoryDef backstory, bool closestMatchWarning = true)
		{
			backstory = DefDatabase<BackstoryDef>.AllDefs.FirstOrDefault((BackstoryDef b) => b.defName == identifier);
			if (backstory == null)
			{
				string strippedDefName = BackstoryTranslationUtility.StripNumericSuffix(identifier);
				backstory = BackstoryTranslationUtility.tmpAllBackstories.FirstOrDefault((BackstoryDef b) => BackstoryTranslationUtility.StripNumericSuffix(b.defName) == strippedDefName);
				if (backstory != null && closestMatchWarning)
				{
					Log.Warning("Couldn't find exact match for backstory " + identifier + ", using closest match " + backstory.identifier);
				}
			}
			return backstory != null;
		}

		// Token: 0x06001675 RID: 5749 RVA: 0x0006DF10 File Offset: 0x0006C110
		public static List<string> MissingBackstoryTranslations(LoadedLanguage lang)
		{
			List<string> list = new List<string>();
			BackstoryTranslationUtility.tmpAllBackstories.Clear();
			BackstoryTranslationUtility.tmpAllBackstories.AddRange(DefDatabase<BackstoryDef>.AllDefs);
			foreach (XElement xelement in BackstoryTranslationUtility.BackstoryTranslationElements(lang.AllDirectories, null))
			{
				try
				{
					string text = xelement.Name.ToString();
					BackstoryDef backstoryDef;
					BackstoryTranslationUtility.TryGetWithIdentifier(text, out backstoryDef, true);
					if (backstoryDef == null)
					{
						list.Add("Translation doesn't correspond to any backstory: " + text);
					}
					else
					{
						BackstoryTranslationUtility.tmpAllBackstories.Remove(backstoryDef);
						string text2 = BackstoryTranslationUtility.GetText(xelement, "title");
						string text3 = BackstoryTranslationUtility.GetText(xelement, "titleFemale");
						string text4 = BackstoryTranslationUtility.GetText(xelement, "titleShort");
						string text5 = BackstoryTranslationUtility.GetText(xelement, "titleShortFemale");
						string text6 = BackstoryTranslationUtility.GetText(xelement, "desc");
						if (text2.NullOrEmpty())
						{
							list.Add(text + ".title missing");
						}
						if (!backstoryDef.titleFemale.NullOrEmpty() && text3.NullOrEmpty())
						{
							list.Add(text + ".titleFemale missing");
						}
						if (text4.NullOrEmpty())
						{
							list.Add(text + ".titleShort missing");
						}
						if (!backstoryDef.titleShortFemale.NullOrEmpty() && text5.NullOrEmpty())
						{
							list.Add(text + ".titleShortFemale missing");
						}
						if (text6.NullOrEmpty())
						{
							list.Add(text + ".desc missing");
						}
					}
				}
				catch (Exception ex)
				{
					List<string> list2 = list;
					string str = "Exception reading ";
					XName name = xelement.Name;
					list2.Add(str + ((name != null) ? name.ToString() : null) + ": " + ex.Message);
				}
			}
			foreach (BackstoryDef backstoryDef2 in BackstoryTranslationUtility.tmpAllBackstories)
			{
				list.Add("Missing backstory: " + backstoryDef2.defName);
			}
			BackstoryTranslationUtility.tmpAllBackstories.Clear();
			return list;
		}

		// Token: 0x06001676 RID: 5750 RVA: 0x0006E15C File Offset: 0x0006C35C
		public static List<string> BackstoryTranslationsMatchingEnglish(LoadedLanguage lang)
		{
			List<string> list = new List<string>();
			foreach (XElement xelement in BackstoryTranslationUtility.BackstoryTranslationElements(lang.AllDirectories, null))
			{
				try
				{
					string identifier = xelement.Name.ToString();
					BackstoryDef backstoryDef;
					if ((from b in DefDatabase<BackstoryDef>.AllDefs
					where b.defName == identifier
					select b).TryRandomElement(out backstoryDef))
					{
						string text = BackstoryTranslationUtility.GetText(xelement, "title");
						string text2 = BackstoryTranslationUtility.GetText(xelement, "titleFemale");
						string text3 = BackstoryTranslationUtility.GetText(xelement, "titleShort");
						string text4 = BackstoryTranslationUtility.GetText(xelement, "titleShortFemale");
						string text5 = BackstoryTranslationUtility.GetText(xelement, "desc");
						if (!text.NullOrEmpty() && text == backstoryDef.untranslatedTitle)
						{
							list.Add(identifier + ".title '" + text.Replace("\n", "\\n") + "'");
						}
						if (!text2.NullOrEmpty() && text2 == backstoryDef.untranslatedTitleFemale)
						{
							list.Add(identifier + ".titleFemale '" + text2.Replace("\n", "\\n") + "'");
						}
						if (!text3.NullOrEmpty() && text3 == backstoryDef.untranslatedTitleShort)
						{
							list.Add(identifier + ".titleShort '" + text3.Replace("\n", "\\n") + "'");
						}
						if (!text4.NullOrEmpty() && text4 == backstoryDef.untranslatedTitleShortFemale)
						{
							list.Add(identifier + ".titleShortFemale '" + text4.Replace("\n", "\\n") + "'");
						}
						if (!text5.NullOrEmpty() && text5 == backstoryDef.untranslatedDesc)
						{
							list.Add(identifier + ".desc '" + text5.Replace("\n", "\\n") + "'");
						}
					}
				}
				catch (Exception ex)
				{
					List<string> list2 = list;
					string str = "Exception reading ";
					XName name = xelement.Name;
					list2.Add(str + ((name != null) ? name.ToString() : null) + ": " + ex.Message);
				}
			}
			return list;
		}

		// Token: 0x06001677 RID: 5751 RVA: 0x0006E3E4 File Offset: 0x0006C5E4
		public static List<string> ObsoleteBackstoryTranslations(LoadedLanguage lang)
		{
			List<string> list = new List<string>();
			using (IEnumerator<XElement> enumerator = BackstoryTranslationUtility.BackstoryTranslationElements(lang.AllDirectories, null).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BackstoryDef backstoryDef;
					if (BackstoryTranslationUtility.TryGetWithIdentifier(enumerator.Current.Name.ToString(), out backstoryDef, true))
					{
						list.Add("Obsolete backstory format: " + backstoryDef.defName);
					}
				}
			}
			return list;
		}

		// Token: 0x06001678 RID: 5752 RVA: 0x0006E460 File Offset: 0x0006C660
		private static string GetText(XElement backstory, string fieldName)
		{
			XElement xelement = backstory.Element(fieldName);
			if (xelement == null || xelement.Value == "TODO")
			{
				return null;
			}
			return xelement.Value.Replace("\\n", "\n");
		}

		// Token: 0x0400147B RID: 5243
		public const string BackstoriesFolder = "Backstories";

		// Token: 0x0400147C RID: 5244
		public const string BackstoriesFileName = "Backstories.xml";

		// Token: 0x0400147D RID: 5245
		public const string BackstoriesFolderLegacy = "Backstories DELETE_ME";

		// Token: 0x0400147E RID: 5246
		private static Regex regex = new Regex("^[^0-9]*");

		// Token: 0x0400147F RID: 5247
		private static List<BackstoryDef> tmpAllBackstories = new List<BackstoryDef>();
	}
}
