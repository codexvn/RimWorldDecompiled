using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using RimWorld;
using RimWorld.IO;

namespace Verse
{
	// Token: 0x02000399 RID: 921
	public static class TranslationFilesCleaner
	{
		// Token: 0x06001796 RID: 6038 RVA: 0x00076D30 File Offset: 0x00074F30
		public static void CleanupTranslationFiles()
		{
			LoadedLanguage curLang = LanguageDatabase.activeLanguage;
			IEnumerable<ModMetaData> activeModsInLoadOrder = ModsConfig.ActiveModsInLoadOrder;
			if (activeModsInLoadOrder.Any((ModMetaData x) => x.IsCoreMod))
			{
				if (!activeModsInLoadOrder.Any((ModMetaData x) => !x.Official))
				{
					if (LanguageDatabase.activeLanguage.AllDirectories.Any((Tuple<VirtualDirectory, ModContentPack, string> x) => x.Item1 is TarDirectory))
					{
						Messages.Message("MessageUnpackBeforeCleaningTranslationFiles".Translate(), MessageTypeDefOf.RejectInput, false);
						return;
					}
					LongEventHandler.QueueLongEvent(delegate()
					{
						if (curLang.anyKeyedReplacementsXmlParseError || curLang.anyDefInjectionsXmlParseError)
						{
							string value = curLang.lastKeyedReplacementsXmlParseErrorInFile ?? curLang.lastDefInjectionsXmlParseErrorInFile;
							Messages.Message("MessageCantCleanupTranslationFilesBeucaseOfXmlError".Translate(value), MessageTypeDefOf.RejectInput, false);
							return;
						}
						curLang.LoadData();
						Dialog_MessageBox dialog_MessageBox = Dialog_MessageBox.CreateConfirmation("ConfirmCleanupTranslationFiles".Translate(curLang.FriendlyNameNative), delegate()
						{
							LongEventHandler.QueueLongEvent(new Action(TranslationFilesCleaner.DoCleanupTranslationFiles), "CleaningTranslationFiles".Translate(), true, null, true, false, null);
						}, true, null, WindowLayer.Dialog);
						dialog_MessageBox.buttonAText = "ConfirmCleanupTranslationFiles_Confirm".Translate();
						Find.WindowStack.Add(dialog_MessageBox);
					}, null, false, null, true, false, null);
					return;
				}
			}
			Messages.Message("MessageDisableModsBeforeCleaningTranslationFiles".Translate(), MessageTypeDefOf.RejectInput, false);
		}

		// Token: 0x06001797 RID: 6039 RVA: 0x00076E20 File Offset: 0x00075020
		private static void DoCleanupTranslationFiles()
		{
			try
			{
				try
				{
					TranslationFilesCleaner.CleanupKeyedTranslations();
				}
				catch (Exception ex)
				{
					string str = "Could not cleanup keyed translations: ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
				try
				{
					TranslationFilesCleaner.CleanupDefInjections();
				}
				catch (Exception ex3)
				{
					string str2 = "Could not cleanup def-injections: ";
					Exception ex4 = ex3;
					Log.Error(str2 + ((ex4 != null) ? ex4.ToString() : null));
				}
				try
				{
					TranslationFilesCleaner.CleanupBackstories();
				}
				catch (Exception ex5)
				{
					string str3 = "Could not cleanup backstories: ";
					Exception ex6 = ex5;
					Log.Error(str3 + ((ex6 != null) ? ex6.ToString() : null));
				}
				string value = string.Join("\n", (from x in ModsConfig.ActiveModsInLoadOrder
				select TranslationFilesCleaner.GetLanguageFolderPath(LanguageDatabase.activeLanguage, x.RootDir.FullName)).ToArray<string>());
				Messages.Message("MessageTranslationFilesCleanupDone".Translate(value), MessageTypeDefOf.TaskCompletion, false);
			}
			catch (Exception ex7)
			{
				string str4 = "Could not cleanup translation files: ";
				Exception ex8 = ex7;
				Log.Error(str4 + ((ex8 != null) ? ex8.ToString() : null));
			}
		}

		// Token: 0x06001798 RID: 6040 RVA: 0x00076F54 File Offset: 0x00075154
		private static void CleanupKeyedTranslations()
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			LoadedLanguage english = LanguageDatabase.defaultLanguage;
			List<LoadedLanguage.KeyedReplacement> list = (from x in activeLanguage.keyedReplacements
			where !x.Value.isPlaceholder && !english.HaveTextForKey(x.Key, false)
			select x.Value).ToList<LoadedLanguage.KeyedReplacement>();
			HashSet<LoadedLanguage.KeyedReplacement> writtenUnusedKeyedTranslations = new HashSet<LoadedLanguage.KeyedReplacement>();
			foreach (ModMetaData modMetaData in ModsConfig.ActiveModsInLoadOrder)
			{
				string languageFolderPath = TranslationFilesCleaner.GetLanguageFolderPath(activeLanguage, modMetaData.RootDir.FullName);
				string text = Path.Combine(languageFolderPath, "CodeLinked");
				string text2 = Path.Combine(languageFolderPath, "Keyed");
				DirectoryInfo directoryInfo = new DirectoryInfo(text);
				if (directoryInfo.Exists)
				{
					if (!Directory.Exists(text2))
					{
						Directory.Move(text, text2);
						Thread.Sleep(1000);
						directoryInfo = new DirectoryInfo(text2);
					}
				}
				else
				{
					directoryInfo = new DirectoryInfo(text2);
				}
				DirectoryInfo directoryInfo2 = new DirectoryInfo(Path.Combine(TranslationFilesCleaner.GetLanguageFolderPath(english, modMetaData.RootDir.FullName), "Keyed"));
				if (!directoryInfo2.Exists)
				{
					if (modMetaData.IsCoreMod)
					{
						Log.Error("English keyed translations folder doesn't exist.");
					}
					if (!directoryInfo.Exists)
					{
						continue;
					}
				}
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				if (activeLanguage != english)
				{
					foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.xml", SearchOption.AllDirectories))
					{
						try
						{
							fileInfo.Delete();
						}
						catch (Exception ex)
						{
							string str = "Could not delete ";
							string name = fileInfo.Name;
							string str2 = ": ";
							Exception ex2 = ex;
							Log.Error(str + name + str2 + ((ex2 != null) ? ex2.ToString() : null));
						}
					}
					foreach (FileInfo fileInfo2 in directoryInfo2.GetFiles("*.xml", SearchOption.AllDirectories))
					{
						try
						{
							string path = new Uri(directoryInfo2.FullName + Path.DirectorySeparatorChar.ToString()).MakeRelativeUri(new Uri(fileInfo2.FullName)).ToString();
							string text3 = Path.Combine(directoryInfo.FullName, path);
							Directory.CreateDirectory(Path.GetDirectoryName(text3));
							fileInfo2.CopyTo(text3);
						}
						catch (Exception ex3)
						{
							string str3 = "Could not copy ";
							string name2 = fileInfo2.Name;
							string str4 = ": ";
							Exception ex4 = ex3;
							Log.Error(str3 + name2 + str4 + ((ex4 != null) ? ex4.ToString() : null));
						}
					}
				}
				foreach (FileInfo fileInfo3 in directoryInfo.GetFiles("*.xml", SearchOption.AllDirectories))
				{
					try
					{
						XDocument xdocument = XDocument.Load(fileInfo3.FullName, LoadOptions.PreserveWhitespace);
						XElement xelement = xdocument.DescendantNodes().OfType<XElement>().FirstOrDefault<XElement>();
						if (xelement != null)
						{
							try
							{
								foreach (XNode xnode in xelement.DescendantNodes().ToArray<XNode>())
								{
									XElement xelement2 = xnode as XElement;
									if (xelement2 != null)
									{
										foreach (XNode xnode2 in xelement2.DescendantNodes().ToArray<XNode>())
										{
											try
											{
												XText xtext = xnode2 as XText;
												if (xtext != null && !xtext.Value.NullOrEmpty())
												{
													string comment = " EN: " + xtext.Value + " ";
													xnode.AddBeforeSelf(new XComment(TranslationFilesCleaner.SanitizeXComment(comment)));
													xnode.AddBeforeSelf(Environment.NewLine);
													xnode.AddBeforeSelf("  ");
												}
											}
											catch (Exception ex5)
											{
												string str5 = "Could not add comment node in ";
												string name3 = fileInfo3.Name;
												string str6 = ": ";
												Exception ex6 = ex5;
												Log.Error(str5 + name3 + str6 + ((ex6 != null) ? ex6.ToString() : null));
											}
											xnode2.Remove();
										}
										try
										{
											TaggedString taggedString;
											if (activeLanguage.TryGetTextFromKey(xelement2.Name.ToString(), out taggedString))
											{
												if (!taggedString.NullOrEmpty())
												{
													xelement2.Add(new XText(taggedString.Replace("\n", "\\n").RawText));
												}
											}
											else
											{
												xelement2.Add(new XText("TODO"));
											}
										}
										catch (Exception ex7)
										{
											string str7 = "Could not add existing translation or placeholder in ";
											string name4 = fileInfo3.Name;
											string str8 = ": ";
											Exception ex8 = ex7;
											Log.Error(str7 + name4 + str8 + ((ex8 != null) ? ex8.ToString() : null));
										}
									}
								}
								bool flag = false;
								foreach (LoadedLanguage.KeyedReplacement keyedReplacement in list)
								{
									if (new Uri(fileInfo3.FullName).Equals(new Uri(keyedReplacement.fileSourceFullPath)))
									{
										if (!flag)
										{
											xelement.Add("  ");
											xelement.Add(new XComment(" UNUSED "));
											xelement.Add(Environment.NewLine);
											flag = true;
										}
										XElement xelement3 = new XElement(keyedReplacement.key);
										if (keyedReplacement.isPlaceholder)
										{
											xelement3.Add(new XText("TODO"));
										}
										else if (!keyedReplacement.value.NullOrEmpty())
										{
											xelement3.Add(new XText(keyedReplacement.value.Replace("\n", "\\n")));
										}
										xelement.Add("  ");
										xelement.Add(xelement3);
										xelement.Add(Environment.NewLine);
										writtenUnusedKeyedTranslations.Add(keyedReplacement);
									}
								}
								if (flag)
								{
									xelement.Add(Environment.NewLine);
								}
							}
							finally
							{
								TranslationFilesCleaner.SaveXMLDocumentWithProcessedNewlineTags(xdocument.Root, fileInfo3.FullName);
							}
						}
					}
					catch (Exception ex9)
					{
						string str9 = "Could not process ";
						string name5 = fileInfo3.Name;
						string str10 = ": ";
						Exception ex10 = ex9;
						Log.Error(str9 + name5 + str10 + ((ex10 != null) ? ex10.ToString() : null));
					}
				}
			}
			IEnumerable<LoadedLanguage.KeyedReplacement> source = list;
			Func<LoadedLanguage.KeyedReplacement, bool> <>9__2;
			Func<LoadedLanguage.KeyedReplacement, bool> predicate;
			if ((predicate = <>9__2) == null)
			{
				predicate = (<>9__2 = ((LoadedLanguage.KeyedReplacement x) => !writtenUnusedKeyedTranslations.Contains(x)));
			}
			foreach (IGrouping<string, LoadedLanguage.KeyedReplacement> grouping in from x in source.Where(predicate)
			group x by x.fileSourceFullPath)
			{
				try
				{
					if (File.Exists(grouping.Key))
					{
						Log.Error("Could not save unused keyed translations to " + grouping.Key + " because this file already exists.");
					}
					else
					{
						object[] array3 = new object[1];
						int num = 0;
						XName name6 = "LanguageData";
						object[] array4 = new object[4];
						array4[0] = new XComment("NEWLINE");
						array4[1] = new XComment(" UNUSED ");
						array4[2] = grouping.Select(delegate(LoadedLanguage.KeyedReplacement x)
						{
							string text4 = x.isPlaceholder ? "TODO" : x.value;
							return new XElement(x.key, new XText(text4.NullOrEmpty() ? "" : text4.Replace("\n", "\\n")));
						});
						array4[3] = new XComment("NEWLINE");
						array3[num] = new XElement(name6, array4);
						TranslationFilesCleaner.SaveXMLDocumentWithProcessedNewlineTags(new XDocument(array3), grouping.Key);
					}
				}
				catch (Exception ex11)
				{
					string str11 = "Could not save unused keyed translations to ";
					string key = grouping.Key;
					string str12 = ": ";
					Exception ex12 = ex11;
					Log.Error(str11 + key + str12 + ((ex12 != null) ? ex12.ToString() : null));
				}
			}
		}

		// Token: 0x06001799 RID: 6041 RVA: 0x00077780 File Offset: 0x00075980
		private static void CleanupDefInjections()
		{
			foreach (ModMetaData modMetaData in ModsConfig.ActiveModsInLoadOrder)
			{
				string languageFolderPath = TranslationFilesCleaner.GetLanguageFolderPath(LanguageDatabase.activeLanguage, modMetaData.RootDir.FullName);
				string text = Path.Combine(languageFolderPath, "DefLinked");
				string text2 = Path.Combine(languageFolderPath, "DefInjected");
				DirectoryInfo directoryInfo = new DirectoryInfo(text);
				if (directoryInfo.Exists)
				{
					if (!Directory.Exists(text2))
					{
						Directory.Move(text, text2);
						Thread.Sleep(1000);
						directoryInfo = new DirectoryInfo(text2);
					}
				}
				else
				{
					directoryInfo = new DirectoryInfo(text2);
				}
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.xml", SearchOption.AllDirectories))
				{
					try
					{
						fileInfo.Delete();
					}
					catch (Exception ex)
					{
						string str = "Could not delete ";
						string name = fileInfo.Name;
						string str2 = ": ";
						Exception ex2 = ex;
						Log.Error(str + name + str2 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
				foreach (Type type in GenDefDatabase.AllDefTypesWithDatabases())
				{
					try
					{
						TranslationFilesCleaner.CleanupDefInjectionsForDefType(type, directoryInfo.FullName, modMetaData);
					}
					catch (Exception ex3)
					{
						string str3 = "Could not process def-injections for type ";
						string name2 = type.Name;
						string str4 = ": ";
						Exception ex4 = ex3;
						Log.Error(str3 + name2 + str4 + ((ex4 != null) ? ex4.ToString() : null));
					}
				}
			}
		}

		// Token: 0x0600179A RID: 6042 RVA: 0x00077964 File Offset: 0x00075B64
		private static void CleanupDefInjectionsForDefType(Type defType, string defInjectionsFolderPath, ModMetaData mod)
		{
			LoadedLanguage activeLanguage = LanguageDatabase.activeLanguage;
			List<KeyValuePair<string, DefInjectionPackage.DefInjection>> list = (from x in (from x in activeLanguage.defInjections
			where x.defType == defType
			select x).SelectMany((DefInjectionPackage x) => x.injections)
			where !x.Value.isPlaceholder && x.Value.ModifiesDefFromModOrNullCore(mod, defType)
			select x).ToList<KeyValuePair<string, DefInjectionPackage.DefInjection>>();
			Dictionary<string, DefInjectionPackage.DefInjection> dictionary = new Dictionary<string, DefInjectionPackage.DefInjection>();
			foreach (KeyValuePair<string, DefInjectionPackage.DefInjection> keyValuePair in list)
			{
				if (!dictionary.ContainsKey(keyValuePair.Value.normalizedPath))
				{
					dictionary.Add(keyValuePair.Value.normalizedPath, keyValuePair.Value);
				}
			}
			if (defType == typeof(BackstoryDef))
			{
				foreach (DefInjectionPackage.DefInjection defInjection in BackstoryTranslationUtility.GetLegacyBackstoryTranslations(activeLanguage.AllDirectories))
				{
					if (!dictionary.ContainsKey(defInjection.path))
					{
						dictionary.Add(defInjection.path, defInjection);
					}
				}
			}
			List<TranslationFilesCleaner.PossibleDefInjection> possibleDefInjections = new List<TranslationFilesCleaner.PossibleDefInjection>();
			DefInjectionUtility.ForEachPossibleDefInjection(defType, delegate(string suggestedPath, string normalizedPath, bool isCollection, string str, IEnumerable<string> collection, bool translationAllowed, bool fullListTranslationAllowed, FieldInfo fieldInfo, Def def)
			{
				if (translationAllowed)
				{
					TranslationFilesCleaner.PossibleDefInjection possibleDefInjection2 = new TranslationFilesCleaner.PossibleDefInjection();
					possibleDefInjection2.suggestedPath = suggestedPath;
					possibleDefInjection2.normalizedPath = normalizedPath;
					possibleDefInjection2.isCollection = isCollection;
					possibleDefInjection2.fullListTranslationAllowed = fullListTranslationAllowed;
					possibleDefInjection2.curValue = str;
					possibleDefInjection2.curValueCollection = collection;
					possibleDefInjection2.fieldInfo = fieldInfo;
					possibleDefInjection2.def = def;
					possibleDefInjections.Add(possibleDefInjection2);
				}
			}, mod);
			if (!possibleDefInjections.Any<TranslationFilesCleaner.PossibleDefInjection>() && !list.Any<KeyValuePair<string, DefInjectionPackage.DefInjection>>())
			{
				return;
			}
			List<KeyValuePair<string, DefInjectionPackage.DefInjection>> source = (from x in list
			where !x.Value.injected
			select x).ToList<KeyValuePair<string, DefInjectionPackage.DefInjection>>();
			using (IEnumerator<string> enumerator3 = (from x in possibleDefInjections
			select TranslationFilesCleaner.GetSourceFile(x.def)).Concat(from x in source
			select x.Value.fileSource).Distinct<string>().GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					string fileName = enumerator3.Current;
					try
					{
						XDocument xdocument = new XDocument();
						bool flag = false;
						try
						{
							XElement xelement = new XElement("LanguageData");
							xdocument.Add(xelement);
							xelement.Add(new XComment("NEWLINE"));
							List<TranslationFilesCleaner.PossibleDefInjection> source2 = (from x in possibleDefInjections
							where TranslationFilesCleaner.GetSourceFile(x.def) == fileName
							select x).ToList<TranslationFilesCleaner.PossibleDefInjection>();
							List<KeyValuePair<string, DefInjectionPackage.DefInjection>> source3 = (from x in source
							where x.Value.fileSource == fileName
							select x).ToList<KeyValuePair<string, DefInjectionPackage.DefInjection>>();
							using (IEnumerator<string> enumerator4 = (from x in (from x in source2
							select x.def.defName).Concat(from x in source3
							select x.Value.DefName).Distinct<string>()
							orderby x
							select x).GetEnumerator())
							{
								while (enumerator4.MoveNext())
								{
									string defName = enumerator4.Current;
									try
									{
										IEnumerable<TranslationFilesCleaner.PossibleDefInjection> enumerable = from x in source2
										where x.def.defName == defName
										select x;
										IEnumerable<KeyValuePair<string, DefInjectionPackage.DefInjection>> enumerable2 = from x in source3
										where x.Value.DefName == defName
										select x;
										if (enumerable.Any<TranslationFilesCleaner.PossibleDefInjection>())
										{
											bool flag2 = false;
											foreach (TranslationFilesCleaner.PossibleDefInjection possibleDefInjection in enumerable)
											{
												if (possibleDefInjection.isCollection)
												{
													IEnumerable<string> englishList = TranslationFilesCleaner.GetEnglishList(possibleDefInjection.normalizedPath, possibleDefInjection.curValueCollection, dictionary);
													bool flag3 = false;
													if (englishList != null)
													{
														int num = 0;
														foreach (string text in englishList)
														{
															if (dictionary.ContainsKey(possibleDefInjection.normalizedPath + "." + num.ToString()))
															{
																flag3 = true;
																break;
															}
															num++;
														}
													}
													if (flag3 || !possibleDefInjection.fullListTranslationAllowed)
													{
														if (englishList == null)
														{
															continue;
														}
														int num2 = -1;
														using (IEnumerator<string> enumerator6 = englishList.GetEnumerator())
														{
															while (enumerator6.MoveNext())
															{
																string text2 = enumerator6.Current;
																num2++;
																string text3 = possibleDefInjection.normalizedPath + "." + num2.ToString();
																string text4 = possibleDefInjection.suggestedPath + "." + num2.ToString();
																string text5;
																if (TKeySystem.TrySuggestTKeyPath(text3, out text5, null))
																{
																	text4 = text5;
																}
																DefInjectionPackage.DefInjection defInjection2;
																if (!dictionary.TryGetValue(text3, out defInjection2))
																{
																	defInjection2 = null;
																}
																if (defInjection2 != null || DefInjectionUtility.ShouldCheckMissingInjection(text2, possibleDefInjection.fieldInfo, possibleDefInjection.def))
																{
																	flag2 = true;
																	flag = true;
																	try
																	{
																		if (!text2.NullOrEmpty())
																		{
																			xelement.Add(new XComment(TranslationFilesCleaner.SanitizeXComment(" EN: " + text2.Replace("\n", "\\n") + " ")));
																		}
																	}
																	catch (Exception ex)
																	{
																		string str11 = "Could not add comment node in ";
																		string fileName5 = fileName;
																		string str2 = ": ";
																		Exception ex2 = ex;
																		Log.Error(str11 + fileName5 + str2 + ((ex2 != null) ? ex2.ToString() : null));
																	}
																	if (LanguageDatabase.activeLanguage == LanguageDatabase.defaultLanguage)
																	{
																		xelement.Add(new XElement(text4, text2));
																	}
																	else
																	{
																		xelement.Add(TranslationFilesCleaner.GetDefInjectableFieldNode(text4, defInjection2));
																	}
																}
															}
															continue;
														}
													}
													bool flag4 = false;
													if (englishList != null)
													{
														using (IEnumerator<string> enumerator6 = englishList.GetEnumerator())
														{
															while (enumerator6.MoveNext())
															{
																if (DefInjectionUtility.ShouldCheckMissingInjection(enumerator6.Current, possibleDefInjection.fieldInfo, possibleDefInjection.def))
																{
																	flag4 = true;
																	break;
																}
															}
														}
													}
													DefInjectionPackage.DefInjection defInjection3;
													if (!dictionary.TryGetValue(possibleDefInjection.normalizedPath, out defInjection3))
													{
														defInjection3 = null;
													}
													if (defInjection3 != null || flag4)
													{
														flag2 = true;
														flag = true;
														try
														{
															string text6 = TranslationFilesCleaner.ListToLiNodesString(englishList);
															if (!text6.NullOrEmpty())
															{
																xelement.Add(new XComment(TranslationFilesCleaner.SanitizeXComment(" EN:\n" + text6.Indented("    ") + "\n  ")));
															}
														}
														catch (Exception ex3)
														{
															string str3 = "Could not add comment node in ";
															string fileName2 = fileName;
															string str4 = ": ";
															Exception ex4 = ex3;
															Log.Error(str3 + fileName2 + str4 + ((ex4 != null) ? ex4.ToString() : null));
														}
														if (LanguageDatabase.activeLanguage == LanguageDatabase.defaultLanguage)
														{
															xelement.Add(TranslationFilesCleaner.ListToXElement(englishList, possibleDefInjection.suggestedPath, (defInjection3 != null) ? defInjection3.fullListInjectionComments : null));
														}
														else
														{
															xelement.Add(TranslationFilesCleaner.GetDefInjectableFieldNode(possibleDefInjection.suggestedPath, defInjection3));
														}
													}
												}
												else
												{
													DefInjectionPackage.DefInjection defInjection4;
													if (!dictionary.TryGetValue(possibleDefInjection.normalizedPath, out defInjection4))
													{
														defInjection4 = null;
													}
													string text7 = (defInjection4 != null && defInjection4.injected) ? defInjection4.replacedString : possibleDefInjection.curValue;
													if (defInjection4 != null || DefInjectionUtility.ShouldCheckMissingInjection(text7, possibleDefInjection.fieldInfo, possibleDefInjection.def))
													{
														flag2 = true;
														flag = true;
														try
														{
															if (!text7.NullOrEmpty())
															{
																xelement.Add(new XComment(TranslationFilesCleaner.SanitizeXComment(" EN: " + text7.Replace("\n", "\\n") + " ")));
															}
														}
														catch (Exception ex5)
														{
															string str5 = "Could not add comment node in ";
															string fileName3 = fileName;
															string str6 = ": ";
															Exception ex6 = ex5;
															Log.Error(str5 + fileName3 + str6 + ((ex6 != null) ? ex6.ToString() : null));
														}
														if (LanguageDatabase.activeLanguage == LanguageDatabase.defaultLanguage)
														{
															xelement.Add(new XElement(possibleDefInjection.suggestedPath, text7));
														}
														else
														{
															xelement.Add(TranslationFilesCleaner.GetDefInjectableFieldNode(possibleDefInjection.suggestedPath, defInjection4));
														}
													}
												}
											}
											if (flag2)
											{
												xelement.Add(new XComment("NEWLINE"));
											}
										}
										if (enumerable2.Any<KeyValuePair<string, DefInjectionPackage.DefInjection>>())
										{
											flag = true;
											xelement.Add(new XComment(" UNUSED "));
											foreach (KeyValuePair<string, DefInjectionPackage.DefInjection> keyValuePair2 in enumerable2)
											{
												xelement.Add(TranslationFilesCleaner.GetDefInjectableFieldNode(keyValuePair2.Value.path, keyValuePair2.Value));
											}
											xelement.Add(new XComment("NEWLINE"));
										}
									}
									catch (Exception ex7)
									{
										string str7 = "Could not process def-injections for def ";
										string defName2 = defName;
										string str8 = ": ";
										Exception ex8 = ex7;
										Log.Error(str7 + defName2 + str8 + ((ex8 != null) ? ex8.ToString() : null));
									}
								}
							}
						}
						finally
						{
							if (flag)
							{
								string text8 = Path.Combine(defInjectionsFolderPath, defType.Name);
								Directory.CreateDirectory(text8);
								TranslationFilesCleaner.SaveXMLDocumentWithProcessedNewlineTags(xdocument, Path.Combine(text8, fileName));
							}
						}
					}
					catch (Exception ex9)
					{
						string str9 = "Could not process def-injections for file ";
						string fileName4 = fileName;
						string str10 = ": ";
						Exception ex10 = ex9;
						Log.Error(str9 + fileName4 + str10 + ((ex10 != null) ? ex10.ToString() : null));
					}
				}
			}
		}

		// Token: 0x0600179B RID: 6043 RVA: 0x000783D4 File Offset: 0x000765D4
		private static void CleanupBackstories()
		{
			string activeLanguageCoreModFolderPath = TranslationFilesCleaner.GetActiveLanguageCoreModFolderPath();
			string text = Path.Combine(activeLanguageCoreModFolderPath, "Backstories");
			if (Directory.Exists(text))
			{
				string text2 = Path.Combine(activeLanguageCoreModFolderPath, "Backstories DELETE_ME");
				Directory.Move(text, text2);
				string value = Path.Combine(text, "Backstories.xml");
				string value2 = Path.Combine(text2, "Backstories.xml");
				Find.WindowStack.Add(new Dialog_MessageBox("RestartAfterImportingLegacyBackstoryTanslations".Translate(value, value2), null, new Action(GenCommandLine.Restart), null, null, null, false, null, null, WindowLayer.Dialog));
			}
		}

		// Token: 0x0600179C RID: 6044 RVA: 0x00078464 File Offset: 0x00076664
		private static string GetActiveLanguageCoreModFolderPath()
		{
			ModContentPack modContentPack = LoadedModManager.RunningMods.FirstOrDefault((ModContentPack x) => x.IsCoreMod);
			return TranslationFilesCleaner.GetLanguageFolderPath(LanguageDatabase.activeLanguage, modContentPack.RootDir);
		}

		// Token: 0x0600179D RID: 6045 RVA: 0x000784AB File Offset: 0x000766AB
		public static string GetLanguageFolderPath(LoadedLanguage language, string modRootDir)
		{
			return Path.Combine(Path.Combine(modRootDir, "Languages"), language.folderName);
		}

		// Token: 0x0600179E RID: 6046 RVA: 0x000784C3 File Offset: 0x000766C3
		private static void SaveXMLDocumentWithProcessedNewlineTags(XNode doc, string path)
		{
			File.WriteAllText(path, "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" + doc.ToString().Replace("<!--NEWLINE-->", "").Replace("&gt;", ">"), Encoding.UTF8);
		}

		// Token: 0x0600179F RID: 6047 RVA: 0x00078500 File Offset: 0x00076700
		private static string ListToLiNodesString(IEnumerable<string> list)
		{
			if (list == null)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string text in list)
			{
				stringBuilder.Append("<li>");
				if (!text.NullOrEmpty())
				{
					stringBuilder.Append(text.Replace("\n", "\\n"));
				}
				stringBuilder.Append("</li>");
				stringBuilder.AppendLine();
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x060017A0 RID: 6048 RVA: 0x0007859C File Offset: 0x0007679C
		private static XElement ListToXElement(IEnumerable<string> list, string name, List<Pair<int, string>> comments)
		{
			XElement xelement = new XElement(name);
			if (list != null)
			{
				int num = 0;
				foreach (string text in list)
				{
					if (comments != null)
					{
						for (int i = 0; i < comments.Count; i++)
						{
							if (comments[i].First == num)
							{
								xelement.Add(new XComment(comments[i].Second));
							}
						}
					}
					XElement xelement2 = new XElement("li");
					if (!text.NullOrEmpty())
					{
						xelement2.Add(new XText(text.Replace("\n", "\\n")));
					}
					xelement.Add(xelement2);
					num++;
				}
				if (comments != null)
				{
					for (int j = 0; j < comments.Count; j++)
					{
						if (comments[j].First == num)
						{
							xelement.Add(new XComment(comments[j].Second));
						}
					}
				}
			}
			return xelement;
		}

		// Token: 0x060017A1 RID: 6049 RVA: 0x000786CC File Offset: 0x000768CC
		private static string AppendXmlExtensionIfNotAlready(string fileName)
		{
			if (!fileName.ToLower().EndsWith(".xml"))
			{
				return fileName + ".xml";
			}
			return fileName;
		}

		// Token: 0x060017A2 RID: 6050 RVA: 0x000786ED File Offset: 0x000768ED
		private static string GetSourceFile(Def def)
		{
			if (!def.fileName.NullOrEmpty())
			{
				return TranslationFilesCleaner.AppendXmlExtensionIfNotAlready(def.fileName);
			}
			return "Unknown.xml";
		}

		// Token: 0x060017A3 RID: 6051 RVA: 0x00078710 File Offset: 0x00076910
		private static IEnumerable<string> GetEnglishList(string normalizedPath, IEnumerable<string> curValue, Dictionary<string, DefInjectionPackage.DefInjection> injectionsByNormalizedPath)
		{
			DefInjectionPackage.DefInjection defInjection;
			if (injectionsByNormalizedPath.TryGetValue(normalizedPath, out defInjection) && defInjection.injected)
			{
				return defInjection.replacedList;
			}
			if (curValue == null)
			{
				return null;
			}
			List<string> list = curValue.ToList<string>();
			if (LanguageDatabase.activeLanguage == LanguageDatabase.defaultLanguage)
			{
				return list;
			}
			for (int i = 0; i < list.Count; i++)
			{
				string key = normalizedPath + "." + i.ToString();
				DefInjectionPackage.DefInjection defInjection2;
				if (injectionsByNormalizedPath.TryGetValue(key, out defInjection2) && defInjection2.injected)
				{
					list[i] = defInjection2.replacedString;
				}
			}
			return list;
		}

		// Token: 0x060017A4 RID: 6052 RVA: 0x00078798 File Offset: 0x00076998
		private static XElement GetDefInjectableFieldNode(string suggestedPath, DefInjectionPackage.DefInjection existingInjection)
		{
			if (existingInjection == null || existingInjection.isPlaceholder)
			{
				return new XElement(suggestedPath, new XText("TODO"));
			}
			if (existingInjection.IsFullListInjection)
			{
				return TranslationFilesCleaner.ListToXElement(existingInjection.fullListInjection, suggestedPath, existingInjection.fullListInjectionComments);
			}
			XElement xelement;
			if (!existingInjection.injection.NullOrEmpty())
			{
				if (existingInjection.suggestedPath.EndsWith(".slateRef") && ConvertHelper.IsXml(existingInjection.injection))
				{
					try
					{
						return XElement.Parse(string.Concat(new string[]
						{
							"<",
							suggestedPath,
							">",
							existingInjection.injection,
							"</",
							suggestedPath,
							">"
						}));
					}
					catch (Exception ex)
					{
						string str = "Could not parse XML: ";
						string injection = existingInjection.injection;
						string str2 = ". Exception: ";
						Exception ex2 = ex;
						Log.Warning(str + injection + str2 + ((ex2 != null) ? ex2.ToString() : null));
						xelement = new XElement(suggestedPath);
						xelement.Add(existingInjection.injection);
						return xelement;
					}
				}
				xelement = new XElement(suggestedPath);
				xelement.Add(new XText(existingInjection.injection.Replace("\n", "\\n")));
			}
			else
			{
				xelement = new XElement(suggestedPath);
			}
			return xelement;
		}

		// Token: 0x060017A5 RID: 6053 RVA: 0x000788E8 File Offset: 0x00076AE8
		private static string SanitizeXComment(string comment)
		{
			while (comment.Contains("-----"))
			{
				comment = comment.Replace("-----", "- - -");
			}
			while (comment.Contains("--"))
			{
				comment = comment.Replace("--", "- -");
			}
			return comment;
		}

		// Token: 0x0400151B RID: 5403
		private const string NewlineTag = "NEWLINE";

		// Token: 0x0400151C RID: 5404
		private const string NewlineTagFull = "<!--NEWLINE-->";

		// Token: 0x0200039A RID: 922
		private class PossibleDefInjection
		{
			// Token: 0x0400151D RID: 5405
			public string suggestedPath;

			// Token: 0x0400151E RID: 5406
			public string normalizedPath;

			// Token: 0x0400151F RID: 5407
			public bool isCollection;

			// Token: 0x04001520 RID: 5408
			public bool fullListTranslationAllowed;

			// Token: 0x04001521 RID: 5409
			public string curValue;

			// Token: 0x04001522 RID: 5410
			public IEnumerable<string> curValueCollection;

			// Token: 0x04001523 RID: 5411
			public FieldInfo fieldInfo;

			// Token: 0x04001524 RID: 5412
			public Def def;
		}
	}
}
