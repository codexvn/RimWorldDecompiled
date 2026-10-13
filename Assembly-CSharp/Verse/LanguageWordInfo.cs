using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using RimWorld.IO;

namespace Verse
{
	// Token: 0x0200037C RID: 892
	public class LanguageWordInfo
	{
		// Token: 0x060016D6 RID: 5846 RVA: 0x0007230C File Offset: 0x0007050C
		public void LoadFrom(Tuple<VirtualDirectory, ModContentPack, string> dir, LoadedLanguage lang)
		{
			VirtualDirectory directory = dir.Item1.GetDirectory("WordInfo").GetDirectory("Gender");
			this.TryLoadFromFile(directory.GetFile("Male.txt"), Gender.Male, dir, lang);
			this.TryLoadFromFile(directory.GetFile("Female.txt"), Gender.Female, dir, lang);
			this.TryLoadFromFile(directory.GetFile("Neuter.txt"), Gender.None, dir, lang);
		}

		// Token: 0x060016D7 RID: 5847 RVA: 0x00072370 File Offset: 0x00070570
		public Gender ResolveGender(string str, string fallback = null, Gender defaultGender = Gender.Male)
		{
			if (str == null)
			{
				return defaultGender;
			}
			Gender result;
			if (this.TryResolveGender(str, out result))
			{
				return result;
			}
			if (fallback != null && this.TryResolveGender(str, out result))
			{
				return result;
			}
			return defaultGender;
		}

		// Token: 0x060016D8 RID: 5848 RVA: 0x000723A0 File Offset: 0x000705A0
		private bool TryResolveGender(string str, out Gender gender)
		{
			LanguageWordInfo.tmpLowercase.Length = 0;
			for (int i = 0; i < str.Length; i++)
			{
				LanguageWordInfo.tmpLowercase.Append(char.ToLower(str[i]));
			}
			string key = LanguageWordInfo.tmpLowercase.ToString();
			if (this.genders.TryGetValue(key, out gender))
			{
				return true;
			}
			gender = Gender.Male;
			return false;
		}

		// Token: 0x060016D9 RID: 5849 RVA: 0x00072400 File Offset: 0x00070600
		private void TryLoadFromFile(VirtualFile file, Gender gender, Tuple<VirtualDirectory, ModContentPack, string> dir, LoadedLanguage lang)
		{
			string[] array;
			try
			{
				array = file.ReadAllLines();
			}
			catch (DirectoryNotFoundException)
			{
				return;
			}
			catch (FileNotFoundException)
			{
				return;
			}
			if (!lang.TryRegisterFileIfNew(dir, file.FullPath))
			{
				return;
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].NullOrEmpty() && !this.genders.ContainsKey(array[i]))
				{
					this.genders.Add(array[i], gender);
				}
			}
		}

		// Token: 0x060016DA RID: 5850 RVA: 0x00072480 File Offset: 0x00070680
		public void RegisterLut(string name)
		{
			if (this.lookupTables.ContainsKey(name.ToLower()))
			{
				Log.Error("Tried registering language look up table named " + name + " twice.");
				return;
			}
			Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			try
			{
				foreach (Tuple<VirtualDirectory, ModContentPack, string> tuple in activeLanguage.AllDirectories)
				{
					VirtualFile file = tuple.Item1.GetDirectory("WordInfo").GetFile(name + ".txt");
					if (file.Exists)
					{
						foreach (string text in GenText.LinesFromString(file.ReadAllText()))
						{
							string[] array;
							if (GenText.TryGetSeparatedValues(text, ';', out array))
							{
								string key = array[0].ToLower();
								if (!dictionary.ContainsKey(key))
								{
									dictionary.Add(key, array);
								}
							}
							else
							{
								Log.ErrorOnce(string.Concat(new string[]
								{
									"Failed parsing lookup items from line ",
									text,
									" in ",
									file.FullPath,
									". Line: ",
									text
								}), name.GetHashCode() ^ 1857221523);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				string str = "Exception parsing a language lookup table: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
			this.lookupTables.Add(name.ToLower(), dictionary);
		}

		// Token: 0x060016DB RID: 5851 RVA: 0x00072648 File Offset: 0x00070848
		public Dictionary<string, string[]> GetLookupTable(string name)
		{
			string text = name.ToLower();
			if (this.lookupTables.ContainsKey(text))
			{
				return this.lookupTables[text];
			}
			this.RegisterLut(text);
			if (this.lookupTables.ContainsKey(text))
			{
				return this.lookupTables[text];
			}
			return null;
		}

		// Token: 0x040014C7 RID: 5319
		private Dictionary<string, Gender> genders = new Dictionary<string, Gender>();

		// Token: 0x040014C8 RID: 5320
		private Dictionary<string, Dictionary<string, string[]>> lookupTables = new Dictionary<string, Dictionary<string, string[]>>();

		// Token: 0x040014C9 RID: 5321
		private const string FolderName = "WordInfo";

		// Token: 0x040014CA RID: 5322
		private const string GendersFolderName = "Gender";

		// Token: 0x040014CB RID: 5323
		private const string MaleFileName = "Male.txt";

		// Token: 0x040014CC RID: 5324
		private const string FemaleFileName = "Female.txt";

		// Token: 0x040014CD RID: 5325
		private const string NeuterFileName = "Neuter.txt";

		// Token: 0x040014CE RID: 5326
		private static StringBuilder tmpLowercase = new StringBuilder();
	}
}
