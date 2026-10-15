using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using RimWorld.IO;
using UnityEngine;

namespace Verse;

public class LoadedLanguage
{
	public class KeyedReplacement
	{
		public string key;

		public string value;

		public string fileSource;

		public int fileSourceLine;

		public string fileSourceFullPath;

		public bool isPlaceholder;
	}

	[CompilerGenerated]
	private sealed class _003Cget_AllDirectories_003Ed__31 : IEnumerable<Tuple<VirtualDirectory, ModContentPack, string>>, IEnumerable, IEnumerator<Tuple<VirtualDirectory, ModContentPack, string>>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Tuple<VirtualDirectory, ModContentPack, string> _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public LoadedLanguage _003C_003E4__this;

		private IEnumerator<ModContentPack> _003C_003E7__wrap1;

		private ModContentPack _003Cmod_003E5__3;

		private List<string>.Enumerator _003C_003E7__wrap3;

		Tuple<VirtualDirectory, ModContentPack, string> IEnumerator<Tuple<VirtualDirectory, ModContentPack, string>>.Current
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
		public _003Cget_AllDirectories_003Ed__31(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || (uint)(num - 1) <= 1u)
			{
				try
				{
					if (num == -4 || (uint)(num - 1) <= 1u)
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
			_003C_003E7__wrap1 = null;
			_003Cmod_003E5__3 = null;
			_003C_003E7__wrap3 = default(List<string>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				LoadedLanguage loadedLanguage = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = LoadedModManager.RunningMods.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_014e;
				case 1:
					_003C_003E1__state = -4;
					goto IL_0125;
				case 2:
					{
						_003C_003E1__state = -4;
						goto IL_0125;
					}
					IL_014e:
					if (_003C_003E7__wrap1.MoveNext())
					{
						_003Cmod_003E5__3 = _003C_003E7__wrap1.Current;
						_003C_003E7__wrap3 = _003Cmod_003E5__3.foldersToLoadDescendingOrder.GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_0125;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					return false;
					IL_0125:
					while (_003C_003E7__wrap3.MoveNext())
					{
						string current = _003C_003E7__wrap3.Current;
						string path = Path.Combine(current, "Languages");
						VirtualDirectory directory = AbstractFilesystem.GetDirectory(Path.Combine(path, loadedLanguage.folderName));
						if (directory.Exists)
						{
							_003C_003E2__current = new Tuple<VirtualDirectory, ModContentPack, string>(directory, _003Cmod_003E5__3, current);
							_003C_003E1__state = 1;
							return true;
						}
						directory = AbstractFilesystem.GetDirectory(Path.Combine(path, loadedLanguage.legacyFolderName));
						if (directory.Exists)
						{
							_003C_003E2__current = new Tuple<VirtualDirectory, ModContentPack, string>(directory, _003Cmod_003E5__3, current);
							_003C_003E1__state = 2;
							return true;
						}
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap3 = default(List<string>.Enumerator);
					_003Cmod_003E5__3 = null;
					goto IL_014e;
				}
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
			((IDisposable)_003C_003E7__wrap3/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Tuple<VirtualDirectory, ModContentPack, string>> IEnumerable<Tuple<VirtualDirectory, ModContentPack, string>>.GetEnumerator()
		{
			_003Cget_AllDirectories_003Ed__31 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_AllDirectories_003Ed__31(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Tuple<VirtualDirectory, ModContentPack, string>>)this).GetEnumerator();
		}
	}

	public string folderName;

	public LanguageInfo info;

	private LanguageWorker workerInt;

	private LanguageWordInfo wordInfo = new LanguageWordInfo();

	private bool dataIsLoaded;

	public List<string> loadErrors = new List<string>();

	public bool anyKeyedReplacementsXmlParseError;

	public string lastKeyedReplacementsXmlParseErrorInFile;

	public bool anyDefInjectionsXmlParseError;

	public string lastDefInjectionsXmlParseErrorInFile;

	public bool anyError;

	private string legacyFolderName;

	private Dictionary<ModContentPack, HashSet<string>> tmpAlreadyLoadedFiles = new Dictionary<ModContentPack, HashSet<string>>();

	public Texture2D icon = BaseContent.BadTex;

	public Dictionary<string, KeyedReplacement> keyedReplacements = new Dictionary<string, KeyedReplacement>();

	public List<DefInjectionPackage> defInjections = new List<DefInjectionPackage>();

	public Dictionary<string, List<string>> stringFiles = new Dictionary<string, List<string>>();

	public const string OldKeyedTranslationsFolderName = "CodeLinked";

	public const string KeyedTranslationsFolderName = "Keyed";

	public const string OldDefInjectionsFolderName = "DefLinked";

	public const string DefInjectionsFolderName = "DefInjected";

	public const string LanguagesFolderName = "Languages";

	public const string PlaceholderText = "TODO";

	private bool infoIsRealMetadata;

	public string DisplayName => GenText.SplitCamelCase(folderName);

	public string FriendlyNameNative
	{
		get
		{
			if (info == null || info.friendlyNameNative.NullOrEmpty())
			{
				return folderName;
			}
			return info.friendlyNameNative;
		}
	}

	public string FriendlyNameEnglish
	{
		get
		{
			if (info == null || info.friendlyNameEnglish.NullOrEmpty())
			{
				return folderName;
			}
			return info.friendlyNameEnglish;
		}
	}

	public IEnumerable<Tuple<VirtualDirectory, ModContentPack, string>> AllDirectories
	{
		[IteratorStateMachine(typeof(_003Cget_AllDirectories_003Ed__31))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_AllDirectories_003Ed__31(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public LanguageWorker Worker
	{
		get
		{
			if (workerInt == null)
			{
				workerInt = (LanguageWorker)Activator.CreateInstance(info.languageWorkerClass);
			}
			return workerInt;
		}
	}

	public LanguageWordInfo WordInfo => wordInfo;

	public string LegacyFolderName => legacyFolderName;

	public LoadedLanguage(string folderName)
	{
		this.folderName = folderName;
		legacyFolderName = (folderName.Contains("(") ? folderName.Substring(0, folderName.IndexOf("(") - 1) : folderName).Trim();
	}

	public void LoadMetadata()
	{
		if (info != null && infoIsRealMetadata)
		{
			return;
		}
		infoIsRealMetadata = true;
		foreach (ModContentPack runningMod in LoadedModManager.RunningMods)
		{
			foreach (string item in runningMod.foldersToLoadDescendingOrder)
			{
				string text = Path.Combine(item, "Languages");
				if (!new DirectoryInfo(text).Exists)
				{
					continue;
				}
				foreach (VirtualDirectory directory in AbstractFilesystem.GetDirectories(text, "*", SearchOption.TopDirectoryOnly))
				{
					if (directory.Name == folderName || directory.Name == legacyFolderName)
					{
						info = DirectXmlLoader.ItemFromXmlFile<LanguageInfo>(directory, "LanguageInfo.xml", resolveCrossRefs: false);
						if (info.friendlyNameNative.NullOrEmpty() && directory.FileExists("FriendlyName.txt"))
						{
							info.friendlyNameNative = directory.ReadAllText("FriendlyName.txt");
						}
						if (info.friendlyNameNative.NullOrEmpty())
						{
							info.friendlyNameNative = folderName;
						}
						if (info.friendlyNameEnglish.NullOrEmpty())
						{
							info.friendlyNameEnglish = folderName;
						}
						return;
					}
				}
			}
		}
	}

	public void InitMetadata(VirtualDirectory directory)
	{
		infoIsRealMetadata = false;
		info = new LanguageInfo();
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
		info.friendlyNameEnglish = friendlyNameEnglish;
		info.friendlyNameNative = friendlyNameNative;
	}

	public void LoadData()
	{
		if (dataIsLoaded)
		{
			return;
		}
		dataIsLoaded = true;
		DeepProfiler.Start("Loading language data: " + folderName);
		try
		{
			tmpAlreadyLoadedFiles.Clear();
			foreach (Tuple<VirtualDirectory, ModContentPack, string> allDirectory in AllDirectories)
			{
				Tuple<VirtualDirectory, ModContentPack, string> localDirectory = allDirectory;
				if (!tmpAlreadyLoadedFiles.ContainsKey(localDirectory.Item2))
				{
					tmpAlreadyLoadedFiles[localDirectory.Item2] = new HashSet<string>();
				}
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					if (icon == BaseContent.BadTex)
					{
						VirtualFile file = localDirectory.Item1.GetFile("LangIcon.png");
						if (file.Exists)
						{
							icon = ModContentLoader<Texture2D>.LoadItem(file).contentItem;
						}
					}
				});
				VirtualDirectory directory = localDirectory.Item1.GetDirectory("CodeLinked");
				if (directory.Exists)
				{
					loadErrors.Add("Translations aren't called CodeLinked any more. Please rename to Keyed: " + directory);
				}
				else
				{
					directory = localDirectory.Item1.GetDirectory("Keyed");
				}
				if (directory.Exists)
				{
					List<VirtualFile> list = new List<VirtualFile>();
					foreach (VirtualFile file2 in directory.GetFiles("*.xml", SearchOption.AllDirectories))
					{
						if (TryRegisterFileIfNew(localDirectory, file2.FullPath))
						{
							list.Add(file2);
						}
					}
					List<string> list2 = (from x in list.AsParallel()
						select x.ReadAllText()).ToList();
					for (int i = 0; i < list2.Count; i++)
					{
						LoadFromFile_Keyed(list[i], list2[i]);
					}
				}
				VirtualDirectory directory2 = localDirectory.Item1.GetDirectory("DefLinked");
				if (directory2.Exists)
				{
					loadErrors.Add("Translations aren't called DefLinked any more. Please rename to DefInjected: " + directory2);
				}
				else
				{
					directory2 = localDirectory.Item1.GetDirectory("DefInjected");
				}
				if (directory2.Exists)
				{
					foreach (VirtualDirectory directory4 in directory2.GetDirectories("*", SearchOption.TopDirectoryOnly))
					{
						string name = directory4.Name;
						Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(name);
						if (typeInAnyAssembly == null && name.Length > 3)
						{
							typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(name.Substring(0, name.Length - 1));
						}
						if (typeInAnyAssembly == null)
						{
							loadErrors.Add("Error loading language from " + allDirectory?.ToString() + ": dir " + directory4.Name + " doesn't correspond to any def type. Skipping...");
							continue;
						}
						List<VirtualFile> list3 = new List<VirtualFile>();
						foreach (VirtualFile file3 in directory4.GetFiles("*.xml", SearchOption.AllDirectories))
						{
							if (TryRegisterFileIfNew(localDirectory, file3.FullPath))
							{
								list3.Add(file3);
							}
						}
						List<string> list4 = (from x in list3.AsParallel()
							select x.ReadAllText()).ToList();
						for (int j = 0; j < list4.Count; j++)
						{
							LoadFromFile_DefInject(list3[j], typeInAnyAssembly, list4[j]);
						}
					}
				}
				EnsureAllDefTypesHaveDefInjectionPackage();
				VirtualDirectory directory3 = localDirectory.Item1.GetDirectory("Strings");
				if (directory3.Exists)
				{
					foreach (VirtualDirectory directory5 in directory3.GetDirectories("*", SearchOption.TopDirectoryOnly))
					{
						foreach (VirtualFile file4 in directory5.GetFiles("*.txt", SearchOption.AllDirectories))
						{
							if (TryRegisterFileIfNew(localDirectory, file4.FullPath))
							{
								LoadFromFile_Strings(file4, directory3);
							}
						}
					}
				}
				wordInfo.LoadFrom(localDirectory, this);
			}
		}
		catch (Exception ex)
		{
			Log.Error("Exception loading language data. Rethrowing. Exception: " + ex);
			throw;
		}
		finally
		{
			DeepProfiler.End();
		}
	}

	public bool TryRegisterFileIfNew(Tuple<VirtualDirectory, ModContentPack, string> dir, string filePath)
	{
		if (!filePath.StartsWith(dir.Item3))
		{
			Log.Error("Failed to get a relative path for a file: " + filePath + ", located in " + dir.Item3);
			return false;
		}
		string item = filePath.Substring(dir.Item3.Length);
		if (!tmpAlreadyLoadedFiles.ContainsKey(dir.Item2))
		{
			tmpAlreadyLoadedFiles[dir.Item2] = new HashSet<string>();
		}
		else if (tmpAlreadyLoadedFiles[dir.Item2].Contains(item))
		{
			return false;
		}
		tmpAlreadyLoadedFiles[dir.Item2].Add(item);
		return true;
	}

	private void LoadFromFile_Strings(VirtualFile file, VirtualDirectory stringsTopDir)
	{
		string text;
		try
		{
			text = file.ReadAllText();
		}
		catch (Exception ex)
		{
			loadErrors.Add("Exception loading from strings file " + file?.ToString() + ": " + ex);
			return;
		}
		string text2 = file.FullPath;
		if (stringsTopDir != null)
		{
			text2 = text2.Substring(stringsTopDir.FullPath.Length + 1);
		}
		text2 = text2.Substring(0, text2.Length - Path.GetExtension(text2).Length);
		text2 = text2.Replace('\\', '/');
		List<string> list = new List<string>();
		foreach (string item in GenText.LinesFromString(text))
		{
			list.Add(item);
		}
		if (stringFiles.TryGetValue(text2, out var value))
		{
			foreach (string item2 in list)
			{
				value.Add(item2);
			}
			return;
		}
		stringFiles.Add(text2, list);
	}

	private void LoadFromFile_Keyed(VirtualFile file, string preloadedFileContents)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
		try
		{
			foreach (DirectXmlLoaderSimple.XmlKeyValuePair item in DirectXmlLoaderSimple.ValuesFromXmlFile(preloadedFileContents))
			{
				if (dictionary.ContainsKey(item.key))
				{
					loadErrors.Add("Duplicate keyed translation key: " + item.key + " in language " + folderName);
					continue;
				}
				dictionary.Add(item.key, item.value);
				dictionary2.Add(item.key, item.lineNumber);
			}
		}
		catch (Exception ex)
		{
			loadErrors.Add("Exception loading from translation file " + file?.ToString() + ": " + ex);
			dictionary.Clear();
			dictionary2.Clear();
			anyKeyedReplacementsXmlParseError = true;
			lastKeyedReplacementsXmlParseErrorInFile = file.Name;
		}
		foreach (KeyValuePair<string, string> item2 in dictionary)
		{
			string text = item2.Value;
			KeyedReplacement keyedReplacement = new KeyedReplacement();
			if (text == "TODO")
			{
				keyedReplacement.isPlaceholder = true;
				text = "";
			}
			keyedReplacement.key = item2.Key;
			keyedReplacement.value = text;
			keyedReplacement.fileSource = file.Name;
			keyedReplacement.fileSourceLine = dictionary2[item2.Key];
			keyedReplacement.fileSourceFullPath = file.FullPath;
			keyedReplacements.SetOrAdd(item2.Key, keyedReplacement);
		}
	}

	public void LoadFromFile_DefInject(VirtualFile file, Type defType, string preloadedFileContents)
	{
		DefInjectionPackage defInjectionPackage = defInjections.Where((DefInjectionPackage di) => di.defType == defType).FirstOrDefault();
		if (defInjectionPackage == null)
		{
			defInjectionPackage = new DefInjectionPackage(defType);
			defInjections.Add(defInjectionPackage);
		}
		defInjectionPackage.AddDataFromFile(file, out var xmlParseError, preloadedFileContents);
		if (xmlParseError)
		{
			anyDefInjectionsXmlParseError = true;
			lastDefInjectionsXmlParseErrorInFile = file.Name;
		}
	}

	private void EnsureAllDefTypesHaveDefInjectionPackage()
	{
		foreach (Type defType in GenDefDatabase.AllDefTypesWithDatabases())
		{
			if (!defInjections.Any((DefInjectionPackage x) => x.defType == defType))
			{
				defInjections.Add(new DefInjectionPackage(defType));
			}
		}
	}

	public bool HaveTextForKey(string key, bool allowPlaceholders = false)
	{
		if (!dataIsLoaded)
		{
			LoadData();
		}
		if (key == null)
		{
			return false;
		}
		if (!keyedReplacements.TryGetValue(key, out var value))
		{
			return false;
		}
		if (!allowPlaceholders)
		{
			return !value.isPlaceholder;
		}
		return true;
	}

	public bool TryGetTextFromKey(string key, out TaggedString translated)
	{
		if (!dataIsLoaded)
		{
			LoadData();
		}
		if (key == null)
		{
			translated = key;
			return false;
		}
		if (!keyedReplacements.TryGetValue(key, out var value) || value.isPlaceholder)
		{
			translated = key;
			return false;
		}
		translated = value.value;
		return true;
	}

	public bool TryGetStringsFromFile(string fileName, out List<string> stringsList)
	{
		if (!dataIsLoaded)
		{
			LoadData();
		}
		if (!stringFiles.TryGetValue(fileName, out stringsList))
		{
			stringsList = null;
			return false;
		}
		return true;
	}

	public string GetKeySourceFileAndLine(string key)
	{
		if (!keyedReplacements.TryGetValue(key, out var value))
		{
			return "unknown";
		}
		return value.fileSource + ":" + value.fileSourceLine;
	}

	public Gender ResolveGender(string str, string fallback = null, Gender defaultGender = Gender.Male)
	{
		return wordInfo.ResolveGender(str, fallback, defaultGender);
	}

	public void InjectIntoData_BeforeImpliedDefs()
	{
		if (!dataIsLoaded)
		{
			LoadData();
		}
		foreach (DefInjectionPackage defInjection in defInjections)
		{
			try
			{
				defInjection.InjectIntoDefs(errorOnDefNotFound: false);
			}
			catch (Exception ex)
			{
				Log.Error("Critical error while injecting translations into defs: " + ex);
			}
		}
	}

	public void InjectIntoData_AfterImpliedDefs()
	{
		if (!dataIsLoaded)
		{
			LoadData();
		}
		int num = loadErrors.Count;
		foreach (DefInjectionPackage defInjection in defInjections)
		{
			try
			{
				defInjection.InjectIntoDefs(errorOnDefNotFound: true);
				num += defInjection.loadErrors.Count;
			}
			catch (Exception ex)
			{
				Log.Error("Critical error while injecting translations into defs: " + ex);
			}
		}
		if (num != 0)
		{
			anyError = true;
			Log.Warning("Translation data for language " + LanguageDatabase.activeLanguage.FriendlyNameEnglish + " has " + num + " errors. Generate translation report for more info.");
		}
	}

	public override string ToString()
	{
		return info.friendlyNameEnglish;
	}
}
