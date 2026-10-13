using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using RimWorld.IO;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000392 RID: 914
	public class LoadedLanguage
	{
		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x06001768 RID: 5992 RVA: 0x00075917 File Offset: 0x00073B17
		public string DisplayName
		{
			get
			{
				return GenText.SplitCamelCase(this.folderName);
			}
		}

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x06001769 RID: 5993 RVA: 0x00075924 File Offset: 0x00073B24
		public string FriendlyNameNative
		{
			get
			{
				if (this.info == null || this.info.friendlyNameNative.NullOrEmpty())
				{
					return this.folderName;
				}
				return this.info.friendlyNameNative;
			}
		}

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x0600176A RID: 5994 RVA: 0x00075952 File Offset: 0x00073B52
		public string FriendlyNameEnglish
		{
			get
			{
				if (this.info == null || this.info.friendlyNameEnglish.NullOrEmpty())
				{
					return this.folderName;
				}
				return this.info.friendlyNameEnglish;
			}
		}

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x0600176B RID: 5995 RVA: 0x00075980 File Offset: 0x00073B80
		public IEnumerable<Tuple<VirtualDirectory, ModContentPack, string>> AllDirectories
		{
			get
			{
				foreach (ModContentPack mod in LoadedModManager.RunningMods)
				{
					foreach (string text in mod.foldersToLoadDescendingOrder)
					{
						string path = Path.Combine(text, "Languages");
						VirtualDirectory directory = AbstractFilesystem.GetDirectory(Path.Combine(path, this.folderName));
						if (directory.Exists)
						{
							yield return new Tuple<VirtualDirectory, ModContentPack, string>(directory, mod, text);
						}
						else
						{
							directory = AbstractFilesystem.GetDirectory(Path.Combine(path, this.legacyFolderName));
							if (directory.Exists)
							{
								yield return new Tuple<VirtualDirectory, ModContentPack, string>(directory, mod, text);
							}
						}
					}
					List<string>.Enumerator enumerator2 = default(List<string>.Enumerator);
					mod = null;
				}
				IEnumerator<ModContentPack> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x0600176C RID: 5996 RVA: 0x00075990 File Offset: 0x00073B90
		public LanguageWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (LanguageWorker)Activator.CreateInstance(this.info.languageWorkerClass);
				}
				return this.workerInt;
			}
		}

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x0600176D RID: 5997 RVA: 0x000759BB File Offset: 0x00073BBB
		public LanguageWordInfo WordInfo
		{
			get
			{
				return this.wordInfo;
			}
		}

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x0600176E RID: 5998 RVA: 0x000759C3 File Offset: 0x00073BC3
		public string LegacyFolderName
		{
			get
			{
				return this.legacyFolderName;
			}
		}

		// Token: 0x0600176F RID: 5999 RVA: 0x000759CC File Offset: 0x00073BCC
		public LoadedLanguage(string folderName)
		{
			this.folderName = folderName;
			this.legacyFolderName = (folderName.Contains("(") ? folderName.Substring(0, folderName.IndexOf("(") - 1) : folderName).Trim();
		}

		// Token: 0x06001770 RID: 6000 RVA: 0x00075A64 File Offset: 0x00073C64
		public void LoadMetadata()
		{
			if (this.info != null && this.infoIsRealMetadata)
			{
				return;
			}
			this.infoIsRealMetadata = true;
			foreach (ModContentPack modContentPack in LoadedModManager.RunningMods)
			{
				foreach (string path in modContentPack.foldersToLoadDescendingOrder)
				{
					string text = Path.Combine(path, "Languages");
					if (new DirectoryInfo(text).Exists)
					{
						foreach (VirtualDirectory virtualDirectory in AbstractFilesystem.GetDirectories(text, "*", SearchOption.TopDirectoryOnly, false))
						{
							if (virtualDirectory.Name == this.folderName || virtualDirectory.Name == this.legacyFolderName)
							{
								this.info = DirectXmlLoader.ItemFromXmlFile<LanguageInfo>(virtualDirectory, "LanguageInfo.xml", false);
								if (this.info.friendlyNameNative.NullOrEmpty() && virtualDirectory.FileExists("FriendlyName.txt"))
								{
									this.info.friendlyNameNative = virtualDirectory.ReadAllText("FriendlyName.txt");
								}
								if (this.info.friendlyNameNative.NullOrEmpty())
								{
									this.info.friendlyNameNative = this.folderName;
								}
								if (this.info.friendlyNameEnglish.NullOrEmpty())
								{
									this.info.friendlyNameEnglish = this.folderName;
								}
								return;
							}
						}
					}
				}
			}
		}

		// Token: 0x06001771 RID: 6001 RVA: 0x00075C4C File Offset: 0x00073E4C
		public void InitMetadata(VirtualDirectory directory)
		{
			this.infoIsRealMetadata = false;
			this.info = new LanguageInfo();
			string text = Regex.Replace(directory.Name, "(\\B[A-Z]+?(?=[A-Z][^A-Z])|\\B[A-Z]+?(?=[^A-Z]))", " $1");
			string friendlyNameEnglish = text;
			string friendlyNameNative = text;
			int num = text.FirstIndexOf((char c) => c == '(');
			int num2 = text.LastIndexOf(")");
			if (num >= 0 && num2 >= 0 && num2 > num)
			{
				friendlyNameEnglish = text.Substring(0, num - 1);
				friendlyNameNative = text.Substring(num + 1, num2 - num - 1);
			}
			this.info.friendlyNameEnglish = friendlyNameEnglish;
			this.info.friendlyNameNative = friendlyNameNative;
		}

		// Token: 0x06001772 RID: 6002 RVA: 0x00075CFC File Offset: 0x00073EFC
		public void LoadData()
		{
			if (this.dataIsLoaded)
			{
				return;
			}
			this.dataIsLoaded = true;
			DeepProfiler.Start("Loading language data: " + this.folderName);
			try
			{
				this.tmpAlreadyLoadedFiles.Clear();
				foreach (Tuple<VirtualDirectory, ModContentPack, string> tuple in this.AllDirectories)
				{
					Tuple<VirtualDirectory, ModContentPack, string> localDirectory = tuple;
					if (!this.tmpAlreadyLoadedFiles.ContainsKey(localDirectory.Item2))
					{
						this.tmpAlreadyLoadedFiles[localDirectory.Item2] = new HashSet<string>();
					}
					LongEventHandler.ExecuteWhenFinished(delegate
					{
						if (this.icon == BaseContent.BadTex)
						{
							VirtualFile file = localDirectory.Item1.GetFile("LangIcon.png");
							if (file.Exists)
							{
								this.icon = ModContentLoader<Texture2D>.LoadItem(file).contentItem;
							}
						}
					});
					VirtualDirectory directory = localDirectory.Item1.GetDirectory("CodeLinked");
					if (directory.Exists)
					{
						List<string> list = this.loadErrors;
						string str = "Translations aren't called CodeLinked any more. Please rename to Keyed: ";
						VirtualDirectory virtualDirectory = directory;
						list.Add(str + ((virtualDirectory != null) ? virtualDirectory.ToString() : null));
					}
					else
					{
						directory = localDirectory.Item1.GetDirectory("Keyed");
					}
					if (directory.Exists)
					{
						List<VirtualFile> list2 = new List<VirtualFile>();
						foreach (VirtualFile virtualFile in directory.GetFiles("*.xml", SearchOption.AllDirectories))
						{
							if (this.TryRegisterFileIfNew(localDirectory, virtualFile.FullPath))
							{
								list2.Add(virtualFile);
							}
						}
						List<string> list3 = (from x in list2.AsParallel<VirtualFile>()
						select x.ReadAllText()).ToList<string>();
						for (int i = 0; i < list3.Count; i++)
						{
							this.LoadFromFile_Keyed(list2[i], list3[i]);
						}
					}
					VirtualDirectory directory2 = localDirectory.Item1.GetDirectory("DefLinked");
					if (directory2.Exists)
					{
						List<string> list4 = this.loadErrors;
						string str2 = "Translations aren't called DefLinked any more. Please rename to DefInjected: ";
						VirtualDirectory virtualDirectory2 = directory2;
						list4.Add(str2 + ((virtualDirectory2 != null) ? virtualDirectory2.ToString() : null));
					}
					else
					{
						directory2 = localDirectory.Item1.GetDirectory("DefInjected");
					}
					if (directory2.Exists)
					{
						foreach (VirtualDirectory virtualDirectory3 in directory2.GetDirectories("*", SearchOption.TopDirectoryOnly))
						{
							string name = virtualDirectory3.Name;
							Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(name, null);
							if (typeInAnyAssembly == null && name.Length > 3)
							{
								typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(name.Substring(0, name.Length - 1), null);
							}
							if (typeInAnyAssembly == null)
							{
								List<string> list5 = this.loadErrors;
								string[] array = new string[5];
								array[0] = "Error loading language from ";
								int num = 1;
								Tuple<VirtualDirectory, ModContentPack, string> tuple2 = tuple;
								array[num] = ((tuple2 != null) ? tuple2.ToString() : null);
								array[2] = ": dir ";
								array[3] = virtualDirectory3.Name;
								array[4] = " doesn't correspond to any def type. Skipping...";
								list5.Add(string.Concat(array));
							}
							else
							{
								List<VirtualFile> list6 = new List<VirtualFile>();
								foreach (VirtualFile virtualFile2 in virtualDirectory3.GetFiles("*.xml", SearchOption.AllDirectories))
								{
									if (this.TryRegisterFileIfNew(localDirectory, virtualFile2.FullPath))
									{
										list6.Add(virtualFile2);
									}
								}
								List<string> list7 = (from x in list6.AsParallel<VirtualFile>()
								select x.ReadAllText()).ToList<string>();
								for (int j = 0; j < list7.Count; j++)
								{
									this.LoadFromFile_DefInject(list6[j], typeInAnyAssembly, list7[j]);
								}
							}
						}
					}
					this.EnsureAllDefTypesHaveDefInjectionPackage();
					VirtualDirectory directory3 = localDirectory.Item1.GetDirectory("Strings");
					if (directory3.Exists)
					{
						foreach (VirtualDirectory virtualDirectory4 in directory3.GetDirectories("*", SearchOption.TopDirectoryOnly))
						{
							foreach (VirtualFile virtualFile3 in virtualDirectory4.GetFiles("*.txt", SearchOption.AllDirectories))
							{
								if (this.TryRegisterFileIfNew(localDirectory, virtualFile3.FullPath))
								{
									this.LoadFromFile_Strings(virtualFile3, directory3);
								}
							}
						}
					}
					this.wordInfo.LoadFrom(localDirectory, this);
				}
			}
			catch (Exception ex)
			{
				string str3 = "Exception loading language data. Rethrowing. Exception: ";
				Exception ex2 = ex;
				Log.Error(str3 + ((ex2 != null) ? ex2.ToString() : null));
				throw;
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06001773 RID: 6003 RVA: 0x00076284 File Offset: 0x00074484
		public bool TryRegisterFileIfNew(Tuple<VirtualDirectory, ModContentPack, string> dir, string filePath)
		{
			if (!filePath.StartsWith(dir.Item3))
			{
				Log.Error("Failed to get a relative path for a file: " + filePath + ", located in " + dir.Item3);
				return false;
			}
			string item = filePath.Substring(dir.Item3.Length);
			if (!this.tmpAlreadyLoadedFiles.ContainsKey(dir.Item2))
			{
				this.tmpAlreadyLoadedFiles[dir.Item2] = new HashSet<string>();
			}
			else if (this.tmpAlreadyLoadedFiles[dir.Item2].Contains(item))
			{
				return false;
			}
			this.tmpAlreadyLoadedFiles[dir.Item2].Add(item);
			return true;
		}

		// Token: 0x06001774 RID: 6004 RVA: 0x00076330 File Offset: 0x00074530
		private void LoadFromFile_Strings(VirtualFile file, VirtualDirectory stringsTopDir)
		{
			string text;
			try
			{
				text = file.ReadAllText();
			}
			catch (Exception ex)
			{
				List<string> list = this.loadErrors;
				string str = "Exception loading from strings file ";
				string str2 = (file != null) ? file.ToString() : null;
				string str3 = ": ";
				Exception ex2 = ex;
				list.Add(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
				return;
			}
			string text2 = file.FullPath;
			if (stringsTopDir != null)
			{
				text2 = text2.Substring(stringsTopDir.FullPath.Length + 1);
			}
			text2 = text2.Substring(0, text2.Length - Path.GetExtension(text2).Length);
			text2 = text2.Replace('\\', '/');
			List<string> list2 = new List<string>();
			foreach (string item in GenText.LinesFromString(text))
			{
				list2.Add(item);
			}
			List<string> list3;
			if (this.stringFiles.TryGetValue(text2, out list3))
			{
				using (List<string>.Enumerator enumerator2 = list2.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						string item2 = enumerator2.Current;
						list3.Add(item2);
					}
					return;
				}
			}
			this.stringFiles.Add(text2, list2);
		}

		// Token: 0x06001775 RID: 6005 RVA: 0x0007647C File Offset: 0x0007467C
		private void LoadFromFile_Keyed(VirtualFile file, string preloadedFileContents)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
			try
			{
				foreach (DirectXmlLoaderSimple.XmlKeyValuePair xmlKeyValuePair in DirectXmlLoaderSimple.ValuesFromXmlFile(preloadedFileContents))
				{
					if (dictionary.ContainsKey(xmlKeyValuePair.key))
					{
						this.loadErrors.Add("Duplicate keyed translation key: " + xmlKeyValuePair.key + " in language " + this.folderName);
					}
					else
					{
						dictionary.Add(xmlKeyValuePair.key, xmlKeyValuePair.value);
						dictionary2.Add(xmlKeyValuePair.key, xmlKeyValuePair.lineNumber);
					}
				}
			}
			catch (Exception ex)
			{
				List<string> list = this.loadErrors;
				string str = "Exception loading from translation file ";
				string str2 = (file != null) ? file.ToString() : null;
				string str3 = ": ";
				Exception ex2 = ex;
				list.Add(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
				dictionary.Clear();
				dictionary2.Clear();
				this.anyKeyedReplacementsXmlParseError = true;
				this.lastKeyedReplacementsXmlParseErrorInFile = file.Name;
			}
			foreach (KeyValuePair<string, string> keyValuePair in dictionary)
			{
				string text = keyValuePair.Value;
				LoadedLanguage.KeyedReplacement keyedReplacement = new LoadedLanguage.KeyedReplacement();
				if (text == "TODO")
				{
					keyedReplacement.isPlaceholder = true;
					text = "";
				}
				keyedReplacement.key = keyValuePair.Key;
				keyedReplacement.value = text;
				keyedReplacement.fileSource = file.Name;
				keyedReplacement.fileSourceLine = dictionary2[keyValuePair.Key];
				keyedReplacement.fileSourceFullPath = file.FullPath;
				this.keyedReplacements.SetOrAdd(keyValuePair.Key, keyedReplacement);
			}
		}

		// Token: 0x06001776 RID: 6006 RVA: 0x00076654 File Offset: 0x00074854
		public void LoadFromFile_DefInject(VirtualFile file, Type defType, string preloadedFileContents)
		{
			DefInjectionPackage defInjectionPackage = (from di in this.defInjections
			where di.defType == defType
			select di).FirstOrDefault<DefInjectionPackage>();
			if (defInjectionPackage == null)
			{
				defInjectionPackage = new DefInjectionPackage(defType);
				this.defInjections.Add(defInjectionPackage);
			}
			bool flag;
			defInjectionPackage.AddDataFromFile(file, out flag, preloadedFileContents);
			if (flag)
			{
				this.anyDefInjectionsXmlParseError = true;
				this.lastDefInjectionsXmlParseErrorInFile = file.Name;
			}
		}

		// Token: 0x06001777 RID: 6007 RVA: 0x000766C8 File Offset: 0x000748C8
		private void EnsureAllDefTypesHaveDefInjectionPackage()
		{
			using (IEnumerator<Type> enumerator = GenDefDatabase.AllDefTypesWithDatabases().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Type defType = enumerator.Current;
					if (!this.defInjections.Any((DefInjectionPackage x) => x.defType == defType))
					{
						this.defInjections.Add(new DefInjectionPackage(defType));
					}
				}
			}
		}

		// Token: 0x06001778 RID: 6008 RVA: 0x00076748 File Offset: 0x00074948
		public bool HaveTextForKey(string key, bool allowPlaceholders = false)
		{
			if (!this.dataIsLoaded)
			{
				this.LoadData();
			}
			LoadedLanguage.KeyedReplacement keyedReplacement;
			return key != null && this.keyedReplacements.TryGetValue(key, out keyedReplacement) && (allowPlaceholders || !keyedReplacement.isPlaceholder);
		}

		// Token: 0x06001779 RID: 6009 RVA: 0x00076788 File Offset: 0x00074988
		public bool TryGetTextFromKey(string key, out TaggedString translated)
		{
			if (!this.dataIsLoaded)
			{
				this.LoadData();
			}
			if (key == null)
			{
				translated = key;
				return false;
			}
			LoadedLanguage.KeyedReplacement keyedReplacement;
			if (!this.keyedReplacements.TryGetValue(key, out keyedReplacement) || keyedReplacement.isPlaceholder)
			{
				translated = key;
				return false;
			}
			translated = keyedReplacement.value;
			return true;
		}

		// Token: 0x0600177A RID: 6010 RVA: 0x000767EC File Offset: 0x000749EC
		public bool TryGetStringsFromFile(string fileName, out List<string> stringsList)
		{
			if (!this.dataIsLoaded)
			{
				this.LoadData();
			}
			if (!this.stringFiles.TryGetValue(fileName, out stringsList))
			{
				stringsList = null;
				return false;
			}
			return true;
		}

		// Token: 0x0600177B RID: 6011 RVA: 0x00076814 File Offset: 0x00074A14
		public string GetKeySourceFileAndLine(string key)
		{
			LoadedLanguage.KeyedReplacement keyedReplacement;
			if (!this.keyedReplacements.TryGetValue(key, out keyedReplacement))
			{
				return "unknown";
			}
			return keyedReplacement.fileSource + ":" + keyedReplacement.fileSourceLine.ToString();
		}

		// Token: 0x0600177C RID: 6012 RVA: 0x00076852 File Offset: 0x00074A52
		public Gender ResolveGender(string str, string fallback = null, Gender defaultGender = Gender.Male)
		{
			return this.wordInfo.ResolveGender(str, fallback, defaultGender);
		}

		// Token: 0x0600177D RID: 6013 RVA: 0x00076864 File Offset: 0x00074A64
		public void InjectIntoData_BeforeImpliedDefs()
		{
			if (!this.dataIsLoaded)
			{
				this.LoadData();
			}
			foreach (DefInjectionPackage defInjectionPackage in this.defInjections)
			{
				try
				{
					defInjectionPackage.InjectIntoDefs(false);
				}
				catch (Exception ex)
				{
					string str = "Critical error while injecting translations into defs: ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
		}

		// Token: 0x0600177E RID: 6014 RVA: 0x000768F4 File Offset: 0x00074AF4
		public void InjectIntoData_AfterImpliedDefs()
		{
			if (!this.dataIsLoaded)
			{
				this.LoadData();
			}
			int num = this.loadErrors.Count;
			foreach (DefInjectionPackage defInjectionPackage in this.defInjections)
			{
				try
				{
					defInjectionPackage.InjectIntoDefs(true);
					num += defInjectionPackage.loadErrors.Count;
				}
				catch (Exception ex)
				{
					string str = "Critical error while injecting translations into defs: ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
			if (num != 0)
			{
				this.anyError = true;
				Log.Warning(string.Concat(new string[]
				{
					"Translation data for language ",
					LanguageDatabase.activeLanguage.FriendlyNameEnglish,
					" has ",
					num.ToString(),
					" errors. Generate translation report for more info."
				}));
			}
		}

		// Token: 0x0600177F RID: 6015 RVA: 0x000769E8 File Offset: 0x00074BE8
		public override string ToString()
		{
			return this.info.friendlyNameEnglish;
		}

		// Token: 0x040014EE RID: 5358
		public string folderName;

		// Token: 0x040014EF RID: 5359
		public LanguageInfo info;

		// Token: 0x040014F0 RID: 5360
		private LanguageWorker workerInt;

		// Token: 0x040014F1 RID: 5361
		private LanguageWordInfo wordInfo = new LanguageWordInfo();

		// Token: 0x040014F2 RID: 5362
		private bool dataIsLoaded;

		// Token: 0x040014F3 RID: 5363
		public List<string> loadErrors = new List<string>();

		// Token: 0x040014F4 RID: 5364
		public bool anyKeyedReplacementsXmlParseError;

		// Token: 0x040014F5 RID: 5365
		public string lastKeyedReplacementsXmlParseErrorInFile;

		// Token: 0x040014F6 RID: 5366
		public bool anyDefInjectionsXmlParseError;

		// Token: 0x040014F7 RID: 5367
		public string lastDefInjectionsXmlParseErrorInFile;

		// Token: 0x040014F8 RID: 5368
		public bool anyError;

		// Token: 0x040014F9 RID: 5369
		private string legacyFolderName;

		// Token: 0x040014FA RID: 5370
		private Dictionary<ModContentPack, HashSet<string>> tmpAlreadyLoadedFiles = new Dictionary<ModContentPack, HashSet<string>>();

		// Token: 0x040014FB RID: 5371
		public Texture2D icon = BaseContent.BadTex;

		// Token: 0x040014FC RID: 5372
		public Dictionary<string, LoadedLanguage.KeyedReplacement> keyedReplacements = new Dictionary<string, LoadedLanguage.KeyedReplacement>();

		// Token: 0x040014FD RID: 5373
		public List<DefInjectionPackage> defInjections = new List<DefInjectionPackage>();

		// Token: 0x040014FE RID: 5374
		public Dictionary<string, List<string>> stringFiles = new Dictionary<string, List<string>>();

		// Token: 0x040014FF RID: 5375
		public const string OldKeyedTranslationsFolderName = "CodeLinked";

		// Token: 0x04001500 RID: 5376
		public const string KeyedTranslationsFolderName = "Keyed";

		// Token: 0x04001501 RID: 5377
		public const string OldDefInjectionsFolderName = "DefLinked";

		// Token: 0x04001502 RID: 5378
		public const string DefInjectionsFolderName = "DefInjected";

		// Token: 0x04001503 RID: 5379
		public const string LanguagesFolderName = "Languages";

		// Token: 0x04001504 RID: 5380
		public const string PlaceholderText = "TODO";

		// Token: 0x04001505 RID: 5381
		private bool infoIsRealMetadata;

		// Token: 0x02000393 RID: 915
		public class KeyedReplacement
		{
			// Token: 0x04001506 RID: 5382
			public string key;

			// Token: 0x04001507 RID: 5383
			public string value;

			// Token: 0x04001508 RID: 5384
			public string fileSource;

			// Token: 0x04001509 RID: 5385
			public int fileSourceLine;

			// Token: 0x0400150A RID: 5386
			public string fileSourceFullPath;

			// Token: 0x0400150B RID: 5387
			public bool isPlaceholder;
		}
	}
}
