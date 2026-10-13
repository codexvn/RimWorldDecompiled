using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace Verse
{
	// Token: 0x02000560 RID: 1376
	public static class LoadedModManager
	{
		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x060023BD RID: 9149 RVA: 0x000B959F File Offset: 0x000B779F
		public static List<ModContentPack> RunningModsListForReading
		{
			get
			{
				return LoadedModManager.runningMods;
			}
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x060023BE RID: 9150 RVA: 0x000B959F File Offset: 0x000B779F
		public static IEnumerable<ModContentPack> RunningMods
		{
			get
			{
				return LoadedModManager.runningMods;
			}
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x060023BF RID: 9151 RVA: 0x000B95A6 File Offset: 0x000B77A6
		public static List<Def> PatchedDefsForReading
		{
			get
			{
				return LoadedModManager.patchedDefs;
			}
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x060023C0 RID: 9152 RVA: 0x000B95AD File Offset: 0x000B77AD
		public static IEnumerable<Mod> ModHandles
		{
			get
			{
				return LoadedModManager.runningModClasses.Values;
			}
		}

		// Token: 0x060023C1 RID: 9153 RVA: 0x000B95BC File Offset: 0x000B77BC
		public static void LoadAllActiveMods(bool hotReload = false)
		{
			DeepProfiler.Start("XmlInheritance.Clear()");
			try
			{
				XmlInheritance.Clear();
			}
			finally
			{
				DeepProfiler.End();
			}
			if (!hotReload)
			{
				DeepProfiler.Start("InitializeMods()");
				try
				{
					LoadedModManager.InitializeMods();
				}
				finally
				{
					DeepProfiler.End();
				}
			}
			DeepProfiler.Start("LoadModContent()");
			try
			{
				LoadedModManager.LoadModContent(hotReload);
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("CreateModClasses()");
			try
			{
				LoadedModManager.CreateModClasses();
			}
			finally
			{
				DeepProfiler.End();
			}
			List<LoadableXmlAsset> xmls = null;
			DeepProfiler.Start("LoadModXML()");
			try
			{
				xmls = LoadedModManager.LoadModXML(hotReload);
			}
			finally
			{
				DeepProfiler.End();
			}
			Dictionary<XmlNode, LoadableXmlAsset> assetlookup = new Dictionary<XmlNode, LoadableXmlAsset>();
			XmlDocument xmlDocument = null;
			DeepProfiler.Start("CombineIntoUnifiedXML()");
			try
			{
				xmlDocument = LoadedModManager.CombineIntoUnifiedXML(xmls, assetlookup);
			}
			finally
			{
				DeepProfiler.End();
			}
			if (!hotReload)
			{
				TKeySystem.Clear();
				DeepProfiler.Start("TKeySystem.Parse()");
				try
				{
					TKeySystem.Parse(xmlDocument);
				}
				finally
				{
					DeepProfiler.End();
				}
			}
			if (!hotReload)
			{
				DeepProfiler.Start("ErrorCheckPatches()");
				try
				{
					LoadedModManager.ErrorCheckPatches();
				}
				finally
				{
					DeepProfiler.End();
				}
			}
			DeepProfiler.Start("ApplyPatches()");
			try
			{
				LoadedModManager.ApplyPatches(xmlDocument, assetlookup);
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("ParseAndProcessXML()");
			try
			{
				LoadedModManager.ParseAndProcessXML(xmlDocument, assetlookup, hotReload);
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("ClearCachedPatches()");
			try
			{
				LoadedModManager.ClearCachedPatches();
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("XmlInheritance.Clear()");
			try
			{
				XmlInheritance.Clear();
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x060023C2 RID: 9154 RVA: 0x000B9798 File Offset: 0x000B7998
		public static void InitializeMods()
		{
			int num = 0;
			foreach (ModMetaData modMetaData in ModsConfig.ActiveModsInLoadOrder.ToList<ModMetaData>())
			{
				string str = "Initializing ";
				ModMetaData modMetaData2 = modMetaData;
				DeepProfiler.Start(str + ((modMetaData2 != null) ? modMetaData2.ToString() : null));
				try
				{
					if (!modMetaData.RootDir.Exists)
					{
						ModsConfig.SetActive(modMetaData.PackageId, false);
						string[] array = new string[6];
						array[0] = "Failed to find active mod ";
						array[1] = modMetaData.Name;
						array[2] = "(";
						array[3] = modMetaData.PackageIdPlayerFacing;
						array[4] = ") at ";
						int num2 = 5;
						DirectoryInfo rootDir = modMetaData.RootDir;
						array[num2] = ((rootDir != null) ? rootDir.ToString() : null);
						Log.Warning(string.Concat(array));
					}
					else
					{
						ModContentPack item = new ModContentPack(modMetaData.RootDir, modMetaData.PackageId, modMetaData.PackageIdPlayerFacing, num, modMetaData.Name, modMetaData.Official);
						num++;
						LoadedModManager.runningMods.Add(item);
						GenTypes.ClearCache();
					}
				}
				catch (Exception ex)
				{
					string str2 = "Error initializing mod: ";
					Exception ex2 = ex;
					Log.Error(str2 + ((ex2 != null) ? ex2.ToString() : null));
					ModsConfig.SetActive(modMetaData.PackageId, false);
				}
				finally
				{
					DeepProfiler.End();
				}
			}
		}

		// Token: 0x060023C3 RID: 9155 RVA: 0x000B9920 File Offset: 0x000B7B20
		public static void LoadModContent(bool hotReload = false)
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				DeepProfiler.Start("LoadModContent");
			});
			for (int i = 0; i < LoadedModManager.runningMods.Count; i++)
			{
				ModContentPack modContentPack = LoadedModManager.runningMods[i];
				string str = "Loading ";
				ModContentPack modContentPack2 = modContentPack;
				DeepProfiler.Start(str + ((modContentPack2 != null) ? modContentPack2.ToString() : null) + " content");
				try
				{
					modContentPack.ReloadContent(hotReload);
				}
				catch (Exception ex)
				{
					string str2 = "Could not reload mod content for mod ";
					string packageIdPlayerFacing = modContentPack.PackageIdPlayerFacing;
					string str3 = ": ";
					Exception ex2 = ex;
					Log.Error(str2 + packageIdPlayerFacing + str3 + ((ex2 != null) ? ex2.ToString() : null));
				}
				finally
				{
					DeepProfiler.End();
				}
			}
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				DeepProfiler.End();
				for (int j = 0; j < LoadedModManager.runningMods.Count; j++)
				{
					ModContentPack modContentPack3 = LoadedModManager.runningMods[j];
					if (!modContentPack3.AnyContentLoaded())
					{
						Log.Error("Mod " + modContentPack3.Name + " did not load any content. Following load folders were used:\n" + modContentPack3.foldersToLoadDescendingOrder.ToLineList("  - "));
					}
				}
			});
		}

		// Token: 0x060023C4 RID: 9156 RVA: 0x000B9A0C File Offset: 0x000B7C0C
		public static void CreateModClasses()
		{
			using (IEnumerator<Type> enumerator = typeof(Mod).InstantiableDescendantsAndSelf().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Type type = enumerator.Current;
					string str = "Loading ";
					Type type3 = type;
					DeepProfiler.Start(str + ((type3 != null) ? type3.ToString() : null) + " mod class");
					try
					{
						if (!LoadedModManager.runningModClasses.ContainsKey(type))
						{
							ModContentPack modContentPack = (from modpack in LoadedModManager.runningMods
							where modpack.assemblies.loadedAssemblies.Contains(type.Assembly)
							select modpack).FirstOrDefault<ModContentPack>();
							LoadedModManager.runningModClasses[type] = (Mod)Activator.CreateInstance(type, new object[]
							{
								modContentPack
							});
						}
					}
					catch (Exception ex)
					{
						string str2 = "Error while instantiating a mod of type ";
						Type type2 = type;
						string str3 = (type2 != null) ? type2.ToString() : null;
						string str4 = ": ";
						Exception ex2 = ex;
						Log.Error(str2 + str3 + str4 + ((ex2 != null) ? ex2.ToString() : null));
					}
					finally
					{
						DeepProfiler.End();
					}
				}
			}
		}

		// Token: 0x060023C5 RID: 9157 RVA: 0x000B9B48 File Offset: 0x000B7D48
		public static List<LoadableXmlAsset> LoadModXML(bool hotReload = false)
		{
			List<LoadableXmlAsset> list = new List<LoadableXmlAsset>();
			for (int i = 0; i < LoadedModManager.runningMods.Count; i++)
			{
				ModContentPack modContentPack = LoadedModManager.runningMods[i];
				string str = "Loading ";
				ModContentPack modContentPack2 = modContentPack;
				DeepProfiler.Start(str + ((modContentPack2 != null) ? modContentPack2.ToString() : null));
				try
				{
					list.AddRange(modContentPack.LoadDefs(hotReload));
				}
				catch (Exception ex)
				{
					string str2 = "Could not load defs for mod ";
					string packageIdPlayerFacing = modContentPack.PackageIdPlayerFacing;
					string str3 = ": ";
					Exception ex2 = ex;
					Log.Error(str2 + packageIdPlayerFacing + str3 + ((ex2 != null) ? ex2.ToString() : null));
				}
				finally
				{
					DeepProfiler.End();
				}
			}
			return list;
		}

		// Token: 0x060023C6 RID: 9158 RVA: 0x000B9BF4 File Offset: 0x000B7DF4
		public static void ErrorCheckPatches()
		{
			foreach (ModContentPack modContentPack in LoadedModManager.runningMods)
			{
				foreach (PatchOperation patchOperation in modContentPack.Patches)
				{
					try
					{
						foreach (string text in patchOperation.ConfigErrors())
						{
							string[] array = new string[6];
							array[0] = "Config error in ";
							array[1] = modContentPack.Name;
							array[2] = " patch ";
							int num = 3;
							PatchOperation patchOperation2 = patchOperation;
							array[num] = ((patchOperation2 != null) ? patchOperation2.ToString() : null);
							array[4] = ": ";
							array[5] = text;
							Log.Error(string.Concat(array));
						}
					}
					catch (Exception ex)
					{
						string[] array2 = new string[6];
						array2[0] = "Exception in ConfigErrors() of ";
						array2[1] = modContentPack.Name;
						array2[2] = " patch ";
						int num2 = 3;
						PatchOperation patchOperation3 = patchOperation;
						array2[num2] = ((patchOperation3 != null) ? patchOperation3.ToString() : null);
						array2[4] = ": ";
						int num3 = 5;
						Exception ex2 = ex;
						array2[num3] = ((ex2 != null) ? ex2.ToString() : null);
						Log.Error(string.Concat(array2));
					}
				}
			}
		}

		// Token: 0x060023C7 RID: 9159 RVA: 0x000B9D98 File Offset: 0x000B7F98
		public static void ApplyPatches(XmlDocument xmlDoc, Dictionary<XmlNode, LoadableXmlAsset> assetlookup)
		{
			foreach (PatchOperation patchOperation in LoadedModManager.runningMods.SelectMany((ModContentPack rm) => rm.Patches))
			{
				try
				{
					patchOperation.Apply(xmlDoc);
				}
				catch (Exception ex)
				{
					string str = "Error in patch.Apply(): ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
		}

		// Token: 0x060023C8 RID: 9160 RVA: 0x000B9E38 File Offset: 0x000B8038
		public static XmlDocument CombineIntoUnifiedXML(List<LoadableXmlAsset> xmls, Dictionary<XmlNode, LoadableXmlAsset> assetlookup)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.AppendChild(xmlDocument.CreateElement("Defs"));
			foreach (LoadableXmlAsset loadableXmlAsset in xmls)
			{
				if (loadableXmlAsset.xmlDoc == null || loadableXmlAsset.xmlDoc.DocumentElement == null)
				{
					Log.Error(string.Format("{0}: unknown parse failure", loadableXmlAsset.fullFolderPath + "/" + loadableXmlAsset.name));
				}
				else
				{
					if (loadableXmlAsset.xmlDoc.DocumentElement.Name != "Defs")
					{
						Log.Error(string.Format("{0}: root element named {1}; should be named Defs", loadableXmlAsset.fullFolderPath + "/" + loadableXmlAsset.name, loadableXmlAsset.xmlDoc.DocumentElement.Name));
					}
					foreach (object obj in loadableXmlAsset.xmlDoc.DocumentElement.ChildNodes)
					{
						XmlNode node = (XmlNode)obj;
						XmlNode xmlNode = xmlDocument.ImportNode(node, true);
						assetlookup[xmlNode] = loadableXmlAsset;
						xmlDocument.DocumentElement.AppendChild(xmlNode);
					}
				}
			}
			return xmlDocument;
		}

		// Token: 0x060023C9 RID: 9161 RVA: 0x000B9FB8 File Offset: 0x000B81B8
		public static void ParseAndProcessXML(XmlDocument xmlDoc, Dictionary<XmlNode, LoadableXmlAsset> assetlookup, bool hotReload = false)
		{
			XmlNodeList childNodes = xmlDoc.DocumentElement.ChildNodes;
			List<XmlNode> list = new List<XmlNode>();
			foreach (object obj in childNodes)
			{
				list.Add(obj as XmlNode);
			}
			DeepProfiler.Start("Loading asset nodes " + list.Count.ToString());
			try
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].NodeType == XmlNodeType.Element)
					{
						LoadableXmlAsset loadableXmlAsset = null;
						DeepProfiler.Start("assetlookup.TryGetValue");
						try
						{
							assetlookup.TryGetValue(list[i], out loadableXmlAsset);
						}
						finally
						{
							DeepProfiler.End();
						}
						DeepProfiler.Start("XmlInheritance.TryRegister");
						try
						{
							XmlInheritance.TryRegister(list[i], (loadableXmlAsset != null) ? loadableXmlAsset.mod : null);
						}
						finally
						{
							DeepProfiler.End();
						}
					}
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("XmlInheritance.Resolve()");
			try
			{
				XmlInheritance.Resolve();
			}
			finally
			{
				DeepProfiler.End();
			}
			LoadedModManager.runningMods.FirstOrDefault<ModContentPack>();
			if (hotReload)
			{
				foreach (ModContentPack modContentPack in LoadedModManager.runningMods)
				{
					modContentPack.ClearDefs();
				}
			}
			LoadedModManager.patchedDefs.Clear();
			List<ValueTuple<Def, Def>> toHotReloadCopy = new List<ValueTuple<Def, Def>>();
			bool flag = !GenCommandLine.CommandLineArgPassed("legacy-xml-deserializer");
			DeepProfiler.Start("Loading defs for " + list.Count.ToString() + " nodes");
			try
			{
				foreach (XmlNode xmlNode in list)
				{
					XmlAttributeCollection attributes = xmlNode.Attributes;
					string text;
					if (attributes == null)
					{
						text = null;
					}
					else
					{
						XmlAttribute xmlAttribute = attributes["MayRequire"];
						text = ((xmlAttribute != null) ? xmlAttribute.Value.ToLower() : null);
					}
					string text2 = text;
					if (text2 == null || ModsConfig.AreAllActive(text2))
					{
						XmlAttributeCollection attributes2 = xmlNode.Attributes;
						string[] array;
						if (attributes2 == null)
						{
							array = null;
						}
						else
						{
							XmlAttribute xmlAttribute2 = attributes2["MayRequireAnyOf"];
							array = ((xmlAttribute2 != null) ? xmlAttribute2.Value.ToLower().Split(',', StringSplitOptions.None) : null);
						}
						string[] array2 = array;
						if (array2.NullOrEmpty<string>() || ModsConfig.IsAnyActiveOrEmpty(array2, false))
						{
							LoadableXmlAsset loadableXmlAsset2 = assetlookup.TryGetValue(xmlNode, null);
							Def def = flag ? DirectXmlToObjectNew.DefFromNodeNew(xmlNode, loadableXmlAsset2) : DirectXmlLoader.DefFromNode(xmlNode, loadableXmlAsset2);
							if (def != null)
							{
								if (hotReload)
								{
									Def defSilentFail = GenDefDatabase.GetDefSilentFail(def.GetType(), def.defName, true);
									if (defSilentFail != null)
									{
										toHotReloadCopy.Add(new ValueTuple<Def, Def>(def, defSilentFail));
										def = defSilentFail;
									}
								}
								ModContentPack modContentPack2 = (loadableXmlAsset2 != null) ? loadableXmlAsset2.mod : null;
								if (modContentPack2 != null)
								{
									modContentPack2.AddDef(def, loadableXmlAsset2.name);
								}
								else
								{
									LoadedModManager.patchedDefs.Add(def);
								}
							}
						}
					}
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			if (toHotReloadCopy.Count != 0)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					Parallel.ForEach<ValueTuple<Def, Def>>(toHotReloadCopy, delegate(ValueTuple<Def, Def> toCopy)
					{
						Def item = toCopy.Item1;
						Def item2 = toCopy.Item2;
						ushort shortHash = item2.shortHash;
						ushort index = item2.index;
						ushort debugRandomId = item2.debugRandomId;
						ModContentPack modContentPack3 = item2.modContentPack;
						ThingCategoryDef thingCategoryDef = item2 as ThingCategoryDef;
						TreeNode_ThingCategory treeNode_ThingCategory = (thingCategoryDef != null) ? thingCategoryDef.treeNode : null;
						Gen.MemberwiseShallowCopy(item, item2);
						item2.shortHash = shortHash;
						item2.index = index;
						item2.debugRandomId = debugRandomId;
						item2.modContentPack = modContentPack3;
						if (treeNode_ThingCategory != null)
						{
							((ThingCategoryDef)item2).treeNode = treeNode_ThingCategory;
						}
						item.defName += "_HotReloadedThrowaway";
						item.ResolveDefNameHash();
						item.ClearCachedData();
					});
				});
			}
		}

		// Token: 0x060023CA RID: 9162 RVA: 0x000BA380 File Offset: 0x000B8580
		public static void ClearCachedPatches()
		{
			foreach (ModContentPack modContentPack in LoadedModManager.runningMods)
			{
				foreach (PatchOperation patchOperation in modContentPack.Patches)
				{
					try
					{
						patchOperation.Complete(modContentPack.Name);
					}
					catch (Exception ex)
					{
						string str = "Error in patch.Complete(): ";
						Exception ex2 = ex;
						Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
				modContentPack.ClearPatchesCache();
			}
		}

		// Token: 0x060023CB RID: 9163 RVA: 0x000BA444 File Offset: 0x000B8644
		public static void ClearDestroy()
		{
			foreach (ModContentPack modContentPack in LoadedModManager.runningMods)
			{
				try
				{
					modContentPack.ClearDestroy();
				}
				catch (Exception ex)
				{
					string str = "Error in mod.ClearDestroy(): ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
			LoadedModManager.runningMods.Clear();
			GenTypes.ClearCache();
		}

		// Token: 0x060023CC RID: 9164 RVA: 0x000BA4D4 File Offset: 0x000B86D4
		public static T GetMod<T>() where T : Mod
		{
			return LoadedModManager.GetMod(typeof(T)) as T;
		}

		// Token: 0x060023CD RID: 9165 RVA: 0x000BA4F0 File Offset: 0x000B86F0
		public static Mod GetMod(Type type)
		{
			if (LoadedModManager.runningModClasses.ContainsKey(type))
			{
				return LoadedModManager.runningModClasses[type];
			}
			return (from kvp in LoadedModManager.runningModClasses
			where type.IsAssignableFrom(kvp.Key)
			select kvp).FirstOrDefault<KeyValuePair<Type, Mod>>().Value;
		}

		// Token: 0x060023CE RID: 9166 RVA: 0x000BA550 File Offset: 0x000B8750
		private static string GetSettingsFilename(string modIdentifier, string modHandleName)
		{
			return Path.Combine(GenFilePaths.ConfigFolderPath, GenText.SanitizeFilename(string.Format("Mod_{0}_{1}.xml", modIdentifier, modHandleName)));
		}

		// Token: 0x060023CF RID: 9167 RVA: 0x000BA570 File Offset: 0x000B8770
		public static T ReadModSettings<T>(string modIdentifier, string modHandleName) where T : ModSettings, new()
		{
			string settingsFilename = LoadedModManager.GetSettingsFilename(modIdentifier, modHandleName);
			T t = default(T);
			try
			{
				if (File.Exists(settingsFilename))
				{
					Scribe.loader.InitLoading(settingsFilename);
					try
					{
						Scribe_Deep.Look<T>(ref t, "ModSettings", Array.Empty<object>());
					}
					finally
					{
						Scribe.loader.FinalizeLoading();
					}
				}
			}
			catch (Exception ex)
			{
				Log.Warning(string.Format("Caught exception while loading mod settings data for {0}. Generating fresh settings. The exception was: {1}", modIdentifier, ex.ToString()));
				t = default(T);
			}
			if (t == null)
			{
				t = Activator.CreateInstance<T>();
			}
			return t;
		}

		// Token: 0x060023D0 RID: 9168 RVA: 0x000BA60C File Offset: 0x000B880C
		public static void WriteModSettings(string modIdentifier, string modHandleName, ModSettings settings)
		{
			Scribe.saver.InitSaving(LoadedModManager.GetSettingsFilename(modIdentifier, modHandleName), "SettingsBlock");
			try
			{
				Scribe_Deep.Look<ModSettings>(ref settings, "ModSettings", Array.Empty<object>());
			}
			finally
			{
				Scribe.saver.FinalizeSaving();
			}
		}

		// Token: 0x04001DAE RID: 7598
		private static List<ModContentPack> runningMods = new List<ModContentPack>();

		// Token: 0x04001DAF RID: 7599
		private static Dictionary<Type, Mod> runningModClasses = new Dictionary<Type, Mod>();

		// Token: 0x04001DB0 RID: 7600
		private static List<Def> patchedDefs = new List<Def>();
	}
}
