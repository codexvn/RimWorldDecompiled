using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using RimWorld;
using RimWorld.IO;

namespace Verse
{
	// Token: 0x0200036E RID: 878
	public class DefInjectionPackage
	{
		// Token: 0x0600168A RID: 5770 RVA: 0x0006E853 File Offset: 0x0006CA53
		public DefInjectionPackage(Type defType)
		{
			this.defType = defType;
		}

		// Token: 0x0600168B RID: 5771 RVA: 0x0006E883 File Offset: 0x0006CA83
		private string ProcessedPath(string path)
		{
			if (path == null)
			{
				path = "";
			}
			if (!path.Contains('[') && !path.Contains(']'))
			{
				return path;
			}
			return path.Replace("]", "").Replace('[', '.');
		}

		// Token: 0x0600168C RID: 5772 RVA: 0x0006E8BE File Offset: 0x0006CABE
		private string ProcessedTranslation(string rawTranslation)
		{
			return rawTranslation.Replace("\\n", "\n");
		}

		// Token: 0x0600168D RID: 5773 RVA: 0x0006E8D0 File Offset: 0x0006CAD0
		public void AddDataFromFile(VirtualFile file, out bool xmlParseError, string preloadedFileContents)
		{
			xmlParseError = false;
			try
			{
				foreach (XElement xelement in VirtualFileInfoExt.LoadAsXDocument(preloadedFileContents).Root.Elements())
				{
					if (xelement.Name == "rep")
					{
						string key = this.ProcessedPath(xelement.Elements("path").First<XElement>().Value);
						string translation = this.ProcessedTranslation(xelement.Elements("trans").First<XElement>().Value);
						this.TryAddInjection(file, key, translation);
						this.usedOldRepSyntax = true;
					}
					else
					{
						string text = this.ProcessedPath(xelement.Name.ToString());
						if (text.EndsWith(".slateRef"))
						{
							if (xelement.HasElements)
							{
								this.TryAddInjection(file, text, xelement.GetInnerXml());
							}
							else
							{
								string translation2 = this.ProcessedTranslation(xelement.Value);
								this.TryAddInjection(file, text, translation2);
							}
						}
						else if (xelement.HasElements)
						{
							List<string> list = new List<string>();
							List<Pair<int, string>> list2 = null;
							bool flag = false;
							foreach (XNode xnode in xelement.DescendantNodes())
							{
								XElement xelement2 = xnode as XElement;
								if (xelement2 != null)
								{
									if (xelement2.Name == "li")
									{
										list.Add(this.ProcessedTranslation(xelement2.Value));
									}
									else if (!flag)
									{
										this.loadErrors.Add(text + " has elements which are not 'li' (" + file.Name + ")");
										flag = true;
									}
								}
								XComment xcomment = xnode as XComment;
								if (xcomment != null)
								{
									if (list2 == null)
									{
										list2 = new List<Pair<int, string>>();
									}
									list2.Add(new Pair<int, string>(list.Count, xcomment.Value));
								}
							}
							this.TryAddFullListInjection(file, text, list, list2);
						}
						else
						{
							string translation3 = this.ProcessedTranslation(xelement.Value);
							this.TryAddInjection(file, text, translation3);
						}
					}
				}
			}
			catch (Exception ex)
			{
				List<string> list3 = this.loadErrors;
				string str = "Exception loading translation data from file ";
				string name = file.Name;
				string str2 = ": ";
				Exception ex2 = ex;
				list3.Add(str + name + str2 + ((ex2 != null) ? ex2.ToString() : null));
				xmlParseError = true;
			}
		}

		// Token: 0x0600168E RID: 5774 RVA: 0x0006EB6C File Offset: 0x0006CD6C
		private void TryAddInjection(VirtualFile file, string key, string translation)
		{
			string text = key;
			key = this.BackCompatibleKey(key);
			if (this.CheckErrors(file, key, text, false))
			{
				return;
			}
			DefInjectionPackage.DefInjection defInjection = new DefInjectionPackage.DefInjection();
			if (translation == "TODO")
			{
				defInjection.isPlaceholder = true;
				translation = "";
			}
			defInjection.path = key;
			defInjection.injection = translation;
			defInjection.fileSource = file.Name;
			defInjection.nonBackCompatiblePath = text;
			this.injections.SetOrAdd(key, defInjection);
		}

		// Token: 0x0600168F RID: 5775 RVA: 0x0006EBE0 File Offset: 0x0006CDE0
		private void TryAddFullListInjection(VirtualFile file, string key, List<string> translation, List<Pair<int, string>> comments)
		{
			string text = key;
			key = this.BackCompatibleKey(key);
			if (this.CheckErrors(file, key, text, true))
			{
				return;
			}
			if (translation == null)
			{
				translation = new List<string>();
			}
			DefInjectionPackage.DefInjection defInjection = new DefInjectionPackage.DefInjection();
			defInjection.path = key;
			defInjection.fullListInjection = translation;
			defInjection.fullListInjectionComments = comments;
			defInjection.fileSource = file.Name;
			defInjection.nonBackCompatiblePath = text;
			this.injections.Add(key, defInjection);
		}

		// Token: 0x06001690 RID: 5776 RVA: 0x0006EC4C File Offset: 0x0006CE4C
		private string BackCompatibleKey(string key)
		{
			string[] array = key.Split('.', StringSplitOptions.None);
			if (array.Any<string>())
			{
				array[0] = BackCompatibility.BackCompatibleDefName(this.defType, array[0], true, null);
			}
			key = string.Join(".", array);
			if (this.defType == typeof(ConceptDef) && key.Contains(".helpTexts.0"))
			{
				key = key.Replace(".helpTexts.0", ".helpText");
			}
			return key;
		}

		// Token: 0x06001691 RID: 5777 RVA: 0x0006ECC4 File Offset: 0x0006CEC4
		private bool CheckErrors(VirtualFile file, string key, string nonBackCompatibleKey, bool replacingFullList)
		{
			if (!key.Contains('.'))
			{
				this.loadErrors.Add(string.Concat(new string[]
				{
					"Error loading DefInjection from file ",
					file.Name,
					": Key lacks a dot: ",
					key,
					(key == nonBackCompatibleKey) ? "" : (" (auto-renamed from " + nonBackCompatibleKey + ")"),
					" (",
					file.Name,
					")"
				}));
				return true;
			}
			DefInjectionPackage.DefInjection defInjection;
			if (this.injections.TryGetValue(key, out defInjection))
			{
				string text;
				if (key != nonBackCompatibleKey)
				{
					text = " (auto-renamed from " + nonBackCompatibleKey + ")";
				}
				else if (defInjection.path != defInjection.nonBackCompatiblePath)
				{
					text = string.Concat(new string[]
					{
						" (",
						defInjection.nonBackCompatiblePath,
						" was auto-renamed to ",
						defInjection.path,
						")"
					});
				}
				else
				{
					text = "";
				}
				this.loadErrors.Add(string.Concat(new string[]
				{
					"Duplicate def-injected translation key: ",
					key,
					text,
					" (",
					file.Name,
					")"
				}));
			}
			bool flag = false;
			if (replacingFullList)
			{
				if (this.injections.Any((KeyValuePair<string, DefInjectionPackage.DefInjection> x) => !x.Value.IsFullListInjection && x.Key.StartsWith(key + ".")))
				{
					flag = true;
				}
			}
			else if (key.Contains('.') && char.IsNumber(key[key.Length - 1]))
			{
				string key2 = key.Substring(0, key.LastIndexOf('.'));
				if (this.injections.ContainsKey(key2) && this.injections[key2].IsFullListInjection)
				{
					flag = true;
				}
			}
			if (flag)
			{
				this.loadErrors.Add(string.Concat(new string[]
				{
					"Replacing the whole list and individual elements at the same time doesn't make sense. Either replace the whole list or translate individual elements by using their indexes. key=",
					key,
					(key == nonBackCompatibleKey) ? "" : (" (auto-renamed from " + nonBackCompatibleKey + ")"),
					" (",
					file.Name,
					")"
				}));
				return true;
			}
			return false;
		}

		// Token: 0x06001692 RID: 5778 RVA: 0x0006EF38 File Offset: 0x0006D138
		public void InjectIntoDefs(bool errorOnDefNotFound)
		{
			this.loadSyntaxSuggestions.Clear();
			this.loadErrors.Clear();
			foreach (KeyValuePair<string, DefInjectionPackage.DefInjection> keyValuePair in this.injections)
			{
				if (!keyValuePair.Value.injected)
				{
					string normalizedPath;
					string suggestedPath;
					if (keyValuePair.Value.IsFullListInjection)
					{
						keyValuePair.Value.injected = this.SetDefFieldAtPath(this.defType, keyValuePair.Key, keyValuePair.Value.fullListInjection, typeof(List<string>), errorOnDefNotFound, keyValuePair.Value.fileSource, keyValuePair.Value.isPlaceholder, out normalizedPath, out suggestedPath, out keyValuePair.Value.replacedString, out keyValuePair.Value.replacedList);
					}
					else
					{
						keyValuePair.Value.injected = this.SetDefFieldAtPath(this.defType, keyValuePair.Key, keyValuePair.Value.injection, typeof(string), errorOnDefNotFound, keyValuePair.Value.fileSource, keyValuePair.Value.isPlaceholder, out normalizedPath, out suggestedPath, out keyValuePair.Value.replacedString, out keyValuePair.Value.replacedList);
					}
					keyValuePair.Value.normalizedPath = normalizedPath;
					keyValuePair.Value.suggestedPath = suggestedPath;
				}
			}
			GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), this.defType, "ClearCachedData");
		}

		// Token: 0x06001693 RID: 5779 RVA: 0x0006F0D8 File Offset: 0x0006D2D8
		private bool SetDefFieldAtPath(Type defType, string path, object value, Type ensureFieldType, bool errorOnDefNotFound, string fileSource, bool isPlaceholder, out string normalizedPath, out string suggestedPath, out string replacedString, out IEnumerable<string> replacedStringsList)
		{
			replacedString = null;
			replacedStringsList = null;
			string b = path;
			string text = path;
			bool flag = TKeySystem.TryGetNormalizedPath(path, out normalizedPath);
			if (flag)
			{
				text = text + " (" + normalizedPath + ")";
				suggestedPath = path;
				path = normalizedPath;
			}
			else
			{
				normalizedPath = path;
				suggestedPath = path;
			}
			string text2 = path.Split('.', StringSplitOptions.None)[0];
			text2 = BackCompatibility.BackCompatibleDefName(defType, text2, true, null);
			if (GenDefDatabase.GetDefSilentFail(defType, text2, false) == null)
			{
				if (errorOnDefNotFound)
				{
					this.loadErrors.Add(string.Concat(new string[]
					{
						"Found no ",
						(defType != null) ? defType.ToString() : null,
						" named ",
						text2,
						" to match ",
						text,
						" (",
						fileSource,
						")"
					}));
				}
				return false;
			}
			bool flag2 = false;
			int num = 0;
			List<object> list = new List<object>();
			bool result2;
			try
			{
				List<string> list2 = path.Split('.', StringSplitOptions.None).ToList<string>();
				object obj = GenDefDatabase.GetDefSilentFail(defType, list2[0], false);
				if (obj == null)
				{
					throw new InvalidOperationException("Def named " + list2[0] + " not found.");
				}
				num++;
				string text3;
				int num2;
				DefInjectionPathPartKind defInjectionPathPartKind;
				FieldInfo field;
				int num3;
				int num4;
				for (;;)
				{
					text3 = list2[num];
					list.Add(obj);
					num2 = -1;
					if (int.TryParse(text3, out num2))
					{
						defInjectionPathPartKind = DefInjectionPathPartKind.ListIndex;
					}
					else if (this.GetFieldNamed(obj.GetType(), text3) != null)
					{
						defInjectionPathPartKind = DefInjectionPathPartKind.Field;
					}
					else if (obj.GetType().GetProperty("Count") != null)
					{
						if (text3.Contains('-'))
						{
							defInjectionPathPartKind = DefInjectionPathPartKind.ListHandleWithIndex;
							string[] array = text3.Split('-', StringSplitOptions.None);
							text3 = array[0];
							num2 = ParseHelper.FromString<int>(array[1]);
						}
						else
						{
							defInjectionPathPartKind = DefInjectionPathPartKind.ListHandle;
						}
					}
					else
					{
						defInjectionPathPartKind = DefInjectionPathPartKind.Field;
					}
					if (num == list2.Count - 1)
					{
						break;
					}
					if (defInjectionPathPartKind == DefInjectionPathPartKind.Field)
					{
						field = obj.GetType().GetField(text3, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
						if (field == null)
						{
							goto Block_61;
						}
						if (field.HasAttribute<NoTranslateAttribute>())
						{
							goto Block_62;
						}
						if (field.HasAttribute<UnsavedAttribute>())
						{
							goto Block_64;
						}
						if (field.HasAttribute<TranslationCanChangeCountAttribute>())
						{
							flag2 = true;
						}
						if (defInjectionPathPartKind == DefInjectionPathPartKind.Field)
						{
							obj = field.GetValue(obj);
						}
						else
						{
							object value2 = field.GetValue(obj);
							PropertyInfo property = value2.GetType().GetProperty("Item");
							if (property == null)
							{
								goto Block_68;
							}
							num3 = (int)value2.GetType().GetProperty("Count").GetValue(value2, null);
							if (num2 < 0 || num2 >= num3)
							{
								goto IL_BDB;
							}
							obj = property.GetValue(value2, new object[]
							{
								num2
							});
						}
					}
					else if (defInjectionPathPartKind == DefInjectionPathPartKind.ListIndex || defInjectionPathPartKind == DefInjectionPathPartKind.ListHandle || defInjectionPathPartKind == DefInjectionPathPartKind.ListHandleWithIndex)
					{
						object obj2 = obj;
						PropertyInfo property2 = obj2.GetType().GetProperty("Item");
						if (property2 == null)
						{
							goto Block_72;
						}
						bool flag3;
						if (defInjectionPathPartKind == DefInjectionPathPartKind.ListHandle || defInjectionPathPartKind == DefInjectionPathPartKind.ListHandleWithIndex)
						{
							num2 = TranslationHandleUtility.GetElementIndexByHandle(obj2, text3, num2);
							defInjectionPathPartKind = DefInjectionPathPartKind.ListIndex;
							flag3 = true;
						}
						else
						{
							flag3 = false;
						}
						num4 = (int)obj2.GetType().GetProperty("Count").GetValue(obj2, null);
						if (num2 < 0 || num2 >= num4)
						{
							goto IL_CA3;
						}
						obj = property2.GetValue(obj2, new object[]
						{
							num2
						});
						if (flag3)
						{
							string[] array2 = normalizedPath.Split('.', StringSplitOptions.None);
							array2[num] = num2.ToString();
							normalizedPath = string.Join(".", array2);
						}
						else if (!flag)
						{
							string bestHandleWithIndexForListElement = TranslationHandleUtility.GetBestHandleWithIndexForListElement(obj2, obj);
							if (!bestHandleWithIndexForListElement.NullOrEmpty())
							{
								string[] array3 = suggestedPath.Split('.', StringSplitOptions.None);
								array3[num] = bestHandleWithIndexForListElement;
								suggestedPath = string.Join(".", array3);
							}
						}
					}
					else
					{
						this.loadErrors.Add(string.Concat(new string[]
						{
							"Can't enter node ",
							text3,
							" at path ",
							text,
							", element kind is ",
							defInjectionPathPartKind.ToString(),
							". (",
							fileSource,
							")"
						}));
					}
					num++;
				}
				bool flag4 = false;
				foreach (KeyValuePair<string, DefInjectionPackage.DefInjection> keyValuePair in this.injections)
				{
					if (!(keyValuePair.Key == b) && keyValuePair.Value.injected && keyValuePair.Value.normalizedPath == normalizedPath)
					{
						string text4 = string.Concat(new string[]
						{
							"Duplicate def-injected translation key. Both ",
							keyValuePair.Value.path,
							" and ",
							text,
							" refer to the same field (",
							suggestedPath,
							")"
						});
						if (keyValuePair.Value.path != keyValuePair.Value.nonBackCompatiblePath)
						{
							text4 = string.Concat(new string[]
							{
								text4,
								" (",
								keyValuePair.Value.nonBackCompatiblePath,
								" was auto-renamed to ",
								keyValuePair.Value.path,
								")"
							});
						}
						text4 = text4 + " (" + keyValuePair.Value.fileSource + ")";
						this.loadErrors.Add(text4);
						flag4 = true;
						break;
					}
				}
				bool result = false;
				if (!flag4)
				{
					if (defInjectionPathPartKind == DefInjectionPathPartKind.Field)
					{
						FieldInfo fieldNamed = this.GetFieldNamed(obj.GetType(), text3);
						if (fieldNamed == null)
						{
							string[] array4 = new string[5];
							array4[0] = "Field ";
							array4[1] = text3;
							array4[2] = " does not exist in type ";
							int num5 = 3;
							Type type = obj.GetType();
							array4[num5] = ((type != null) ? type.ToString() : null);
							array4[4] = ".";
							throw new InvalidOperationException(string.Concat(array4));
						}
						if (fieldNamed.HasAttribute<NoTranslateAttribute>())
						{
							List<string> list3 = this.loadErrors;
							string[] array5 = new string[9];
							array5[0] = "Translated untranslatable field ";
							array5[1] = fieldNamed.Name;
							array5[2] = " of type ";
							int num6 = 3;
							Type fieldType = fieldNamed.FieldType;
							array5[num6] = ((fieldType != null) ? fieldType.ToString() : null);
							array5[4] = " at path ";
							array5[5] = text;
							array5[6] = ". Translating this field will break the game. (";
							array5[7] = fileSource;
							array5[8] = ")";
							list3.Add(string.Concat(array5));
						}
						else if (fieldNamed.HasAttribute<UnsavedAttribute>())
						{
							List<string> list4 = this.loadErrors;
							string[] array6 = new string[9];
							array6[0] = "Translated untranslatable field (UnsavedAttribute) ";
							array6[1] = fieldNamed.Name;
							array6[2] = " of type ";
							int num7 = 3;
							Type fieldType2 = fieldNamed.FieldType;
							array6[num7] = ((fieldType2 != null) ? fieldType2.ToString() : null);
							array6[4] = " at path ";
							array6[5] = text;
							array6[6] = ". Translating this field will break the game. (";
							array6[7] = fileSource;
							array6[8] = ")";
							list4.Add(string.Concat(array6));
						}
						else if (!isPlaceholder && fieldNamed.FieldType != ensureFieldType)
						{
							List<string> list5 = this.loadErrors;
							string[] array7 = new string[13];
							array7[0] = "Translated non-";
							array7[1] = ((ensureFieldType != null) ? ensureFieldType.ToString() : null);
							array7[2] = " field ";
							array7[3] = fieldNamed.Name;
							array7[4] = " of type ";
							int num8 = 5;
							Type fieldType3 = fieldNamed.FieldType;
							array7[num8] = ((fieldType3 != null) ? fieldType3.ToString() : null);
							array7[6] = " at path ";
							array7[7] = text;
							array7[8] = ". Expected ";
							array7[9] = ((ensureFieldType != null) ? ensureFieldType.ToString() : null);
							array7[10] = ". (";
							array7[11] = fileSource;
							array7[12] = ")";
							list5.Add(string.Concat(array7));
						}
						else if (!isPlaceholder && ensureFieldType != typeof(string) && !fieldNamed.HasAttribute<TranslationCanChangeCountAttribute>())
						{
							List<string> list6 = this.loadErrors;
							string[] array8 = new string[9];
							array8[0] = "Tried to translate field ";
							array8[1] = fieldNamed.Name;
							array8[2] = " of type ";
							int num9 = 3;
							Type fieldType4 = fieldNamed.FieldType;
							array8[num9] = ((fieldType4 != null) ? fieldType4.ToString() : null);
							array8[4] = " at path ";
							array8[5] = text;
							array8[6] = ", but this field doesn't have [TranslationCanChangeCount] attribute so it doesn't allow this type of translation. (";
							array8[7] = fileSource;
							array8[8] = ")";
							list6.Add(string.Concat(array8));
						}
						else if (!isPlaceholder)
						{
							if (ensureFieldType == typeof(string))
							{
								replacedString = (string)fieldNamed.GetValue(obj);
							}
							else
							{
								replacedStringsList = (fieldNamed.GetValue(obj) as IEnumerable<string>);
							}
							fieldNamed.SetValue(obj, value);
							result = true;
						}
					}
					else if (defInjectionPathPartKind == DefInjectionPathPartKind.ListIndex || defInjectionPathPartKind == DefInjectionPathPartKind.ListHandle || defInjectionPathPartKind == DefInjectionPathPartKind.ListHandleWithIndex)
					{
						object obj3 = obj;
						if (obj3 == null)
						{
							throw new InvalidOperationException("Tried to use index on null list at " + text);
						}
						Type type2 = obj3.GetType();
						PropertyInfo property3 = type2.GetProperty("Count");
						if (property3 == null)
						{
							throw new InvalidOperationException("Tried to use index on non-list (missing 'Count' property).");
						}
						if (defInjectionPathPartKind == DefInjectionPathPartKind.ListHandle || defInjectionPathPartKind == DefInjectionPathPartKind.ListHandleWithIndex)
						{
							num2 = TranslationHandleUtility.GetElementIndexByHandle(obj3, text3, num2);
							defInjectionPathPartKind = DefInjectionPathPartKind.ListIndex;
						}
						int num10 = (int)property3.GetValue(obj3, null);
						if (num2 >= num10)
						{
							throw new InvalidOperationException(string.Concat(new string[]
							{
								"Trying to translate ",
								(defType != null) ? defType.ToString() : null,
								".",
								text,
								" at index ",
								num2.ToString(),
								" but the list only has ",
								num10.ToString(),
								" entries (so max index is ",
								(num10 - 1).ToString(),
								")."
							}));
						}
						PropertyInfo property4 = type2.GetProperty("Item");
						if (property4 == null)
						{
							throw new InvalidOperationException("Tried to use index on non-list (missing 'Item' property).");
						}
						Type propertyType = property4.PropertyType;
						if (!isPlaceholder && propertyType != ensureFieldType)
						{
							List<string> list7 = this.loadErrors;
							string[] array9 = new string[11];
							array9[0] = "Translated non-";
							array9[1] = ((ensureFieldType != null) ? ensureFieldType.ToString() : null);
							array9[2] = " list item of type ";
							int num11 = 3;
							Type type3 = propertyType;
							array9[num11] = ((type3 != null) ? type3.ToString() : null);
							array9[4] = " at path ";
							array9[5] = text;
							array9[6] = ". Expected ";
							array9[7] = ((ensureFieldType != null) ? ensureFieldType.ToString() : null);
							array9[8] = ". (";
							array9[9] = fileSource;
							array9[10] = ")";
							list7.Add(string.Concat(array9));
						}
						else if (!isPlaceholder && ensureFieldType != typeof(string) && !flag2)
						{
							List<string> list8 = this.loadErrors;
							string[] array10 = new string[7];
							array10[0] = "Tried to translate field of type ";
							int num12 = 1;
							Type type4 = propertyType;
							array10[num12] = ((type4 != null) ? type4.ToString() : null);
							array10[2] = " at path ";
							array10[3] = text;
							array10[4] = ", but this field doesn't have [TranslationCanChangeCount] attribute so it doesn't allow this type of translation. (";
							array10[5] = fileSource;
							array10[6] = ")";
							list8.Add(string.Concat(array10));
						}
						else if (num2 < 0 || num2 >= (int)type2.GetProperty("Count").GetValue(obj3, null))
						{
							this.loadErrors.Add("Index out of bounds (max index is " + ((int)type2.GetProperty("Count").GetValue(obj3, null) - 1).ToString() + ")");
						}
						else if (!isPlaceholder)
						{
							replacedString = (string)property4.GetValue(obj3, new object[]
							{
								num2
							});
							property4.SetValue(obj3, value, new object[]
							{
								num2
							});
							result = true;
						}
					}
					else
					{
						this.loadErrors.Add(string.Concat(new string[]
						{
							"Translated ",
							text3,
							" at path ",
							text,
							" but it's not a field, it's ",
							defInjectionPathPartKind.ToString(),
							". (",
							fileSource,
							")"
						}));
					}
				}
				for (int i = list.Count - 1; i > 0; i--)
				{
					if (list[i].GetType().IsValueType && !list[i].GetType().IsPrimitive)
					{
						FieldInfo fieldNamed2 = this.GetFieldNamed(list[i - 1].GetType(), list2[i]);
						if (fieldNamed2 != null)
						{
							fieldNamed2.SetValue(list[i - 1], list[i]);
						}
					}
				}
				string text5;
				if (flag)
				{
					path = suggestedPath;
				}
				else if (TKeySystem.TrySuggestTKeyPath(path, out text5, null))
				{
					suggestedPath = text5;
				}
				if (path != suggestedPath)
				{
					IList<string> list9 = value as IList<string>;
					string text6;
					if (list9 != null)
					{
						text6 = list9.ToStringSafeEnumerable();
					}
					else
					{
						text6 = value.ToString();
					}
					this.loadSyntaxSuggestions.Add(string.Concat(new string[]
					{
						"Consider using ",
						suggestedPath,
						" instead of ",
						text,
						" for translation '",
						text6,
						"' (",
						fileSource,
						")"
					}));
				}
				return result;
				Block_61:
				throw new InvalidOperationException("Field or TKey " + text3 + " does not exist.");
				Block_62:
				string[] array11 = new string[7];
				array11[0] = "Translated untranslatable field ";
				array11[1] = field.Name;
				array11[2] = " of type ";
				int num13 = 3;
				Type fieldType5 = field.FieldType;
				array11[num13] = ((fieldType5 != null) ? fieldType5.ToString() : null);
				array11[4] = " at path ";
				array11[5] = text;
				array11[6] = ". Translating this field will break the game.";
				throw new InvalidOperationException(string.Concat(array11));
				Block_64:
				string[] array12 = new string[7];
				array12[0] = "Translated untranslatable field ([Unsaved] attribute) ";
				array12[1] = field.Name;
				array12[2] = " of type ";
				int num14 = 3;
				Type fieldType6 = field.FieldType;
				array12[num14] = ((fieldType6 != null) ? fieldType6.ToString() : null);
				array12[4] = " at path ";
				array12[5] = text;
				array12[6] = ". Translating this field will break the game.";
				throw new InvalidOperationException(string.Concat(array12));
				Block_68:
				throw new InvalidOperationException("Tried to use index on non-list (missing 'Item' property).");
				IL_BDB:
				throw new InvalidOperationException("Index out of bounds (max index is " + (num3 - 1).ToString() + ")");
				Block_72:
				throw new InvalidOperationException("Tried to use index on non-list (missing 'Item' property).");
				IL_CA3:
				throw new InvalidOperationException("Index out of bounds (max index is " + (num4 - 1).ToString() + ")");
			}
			catch (Exception ex)
			{
				string text7 = string.Concat(new string[]
				{
					"Couldn't inject ",
					text,
					" into ",
					(defType != null) ? defType.ToString() : null,
					" (",
					fileSource,
					"): ",
					ex.Message
				});
				if (ex.InnerException != null)
				{
					text7 = text7 + " -> " + ex.InnerException.Message;
				}
				this.loadErrors.Add(text7);
				result2 = false;
			}
			return result2;
		}

		// Token: 0x06001694 RID: 5780 RVA: 0x0006FF5C File Offset: 0x0006E15C
		private FieldInfo GetFieldNamed(Type type, string name)
		{
			FieldInfo field = type.GetField(name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			if (field == null)
			{
				FieldInfo[] fields = type.GetFields(BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				for (int i = 0; i < fields.Length; i++)
				{
					object[] customAttributes = fields[i].GetCustomAttributes(typeof(LoadAliasAttribute), false);
					if (customAttributes != null && customAttributes.Length != 0)
					{
						for (int j = 0; j < customAttributes.Length; j++)
						{
							if (((LoadAliasAttribute)customAttributes[j]).alias == name)
							{
								return fields[i];
							}
						}
					}
				}
			}
			return field;
		}

		// Token: 0x06001695 RID: 5781 RVA: 0x0006FFDC File Offset: 0x0006E1DC
		public List<string> MissingInjections(List<string> outUnnecessaryDefInjections)
		{
			List<string> missingInjections = new List<string>();
			Dictionary<string, DefInjectionPackage.DefInjection> injectionsByNormalizedPath = new Dictionary<string, DefInjectionPackage.DefInjection>();
			foreach (KeyValuePair<string, DefInjectionPackage.DefInjection> keyValuePair in this.injections)
			{
				if (!injectionsByNormalizedPath.ContainsKey(keyValuePair.Value.normalizedPath))
				{
					injectionsByNormalizedPath.Add(keyValuePair.Value.normalizedPath, keyValuePair.Value);
				}
			}
			DefInjectionUtility.ForEachPossibleDefInjection(this.defType, delegate(string suggestedPath, string normalizedPath, bool isCollection, string str, IEnumerable<string> collection, bool translationAllowed, bool fullListTranslationAllowed, FieldInfo fieldInfo, Def def)
			{
				DefInjectionPackage.DefInjection defInjection2;
				if (!isCollection)
				{
					bool flag = false;
					string text = null;
					DefInjectionPackage.DefInjection defInjection;
					if (injectionsByNormalizedPath.TryGetValue(normalizedPath, out defInjection) && !defInjection.IsFullListInjection)
					{
						if (!translationAllowed)
						{
							outUnnecessaryDefInjections.Add(defInjection.path + " '" + defInjection.injection.Replace("\n", "\\n") + "'");
						}
						else if (defInjection.isPlaceholder)
						{
							flag = true;
							text = defInjection.fileSource;
						}
					}
					else
					{
						flag = true;
					}
					if (flag && translationAllowed && DefInjectionUtility.ShouldCheckMissingInjection(str, fieldInfo, def))
					{
						missingInjections.Add(string.Concat(new string[]
						{
							suggestedPath,
							" '",
							str.Replace("\n", "\\n"),
							"'",
							text.NullOrEmpty() ? "" : (" (placeholder exists in " + text + ")")
						}));
						return;
					}
				}
				else if (injectionsByNormalizedPath.TryGetValue(normalizedPath, out defInjection2) && defInjection2.IsFullListInjection)
				{
					if (!translationAllowed || !fullListTranslationAllowed)
					{
						outUnnecessaryDefInjections.Add(defInjection2.path + " '" + defInjection2.fullListInjection.ToStringSafeEnumerable().Replace("\n", "\\n") + "'");
						return;
					}
					if (defInjection2.isPlaceholder && translationAllowed && !def.generated)
					{
						missingInjections.Add(suggestedPath + (defInjection2.fileSource.NullOrEmpty() ? "" : (" (placeholder exists in " + defInjection2.fileSource + ")")));
						return;
					}
				}
				else
				{
					int num = 0;
					foreach (string text2 in collection)
					{
						string key = normalizedPath + "." + num.ToString();
						string text3 = suggestedPath + "." + num.ToString();
						bool flag2 = false;
						string text4 = null;
						DefInjectionPackage.DefInjection defInjection3;
						if (injectionsByNormalizedPath.TryGetValue(key, out defInjection3) && !defInjection3.IsFullListInjection)
						{
							if (!translationAllowed)
							{
								outUnnecessaryDefInjections.Add(defInjection3.path + " '" + defInjection3.injection.Replace("\n", "\\n") + "'");
							}
							else if (defInjection3.isPlaceholder)
							{
								flag2 = true;
								text4 = defInjection3.fileSource;
							}
						}
						else
						{
							flag2 = true;
						}
						if (flag2 && translationAllowed && DefInjectionUtility.ShouldCheckMissingInjection(text2, fieldInfo, def))
						{
							DefInjectionPackage.DefInjection defInjection4;
							if (text4.NullOrEmpty() && injectionsByNormalizedPath.TryGetValue(normalizedPath, out defInjection4) && defInjection4.isPlaceholder)
							{
								text4 = defInjection4.fileSource;
							}
							missingInjections.Add(string.Concat(new string[]
							{
								text3,
								" '",
								text2.Replace("\n", "\\n"),
								"'",
								fullListTranslationAllowed ? " (hint: this list allows full-list translation by using <li> nodes)" : "",
								text4.NullOrEmpty() ? "" : (" (placeholder exists in " + text4 + ")")
							}));
						}
						num++;
					}
				}
			}, null);
			return missingInjections;
		}

		// Token: 0x04001492 RID: 5266
		public Type defType;

		// Token: 0x04001493 RID: 5267
		public Dictionary<string, DefInjectionPackage.DefInjection> injections = new Dictionary<string, DefInjectionPackage.DefInjection>();

		// Token: 0x04001494 RID: 5268
		public List<string> loadErrors = new List<string>();

		// Token: 0x04001495 RID: 5269
		public List<string> loadSyntaxSuggestions = new List<string>();

		// Token: 0x04001496 RID: 5270
		public bool usedOldRepSyntax;

		// Token: 0x04001497 RID: 5271
		public const BindingFlags FieldBindingFlags = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

		// Token: 0x04001498 RID: 5272
		public const string RepNodeName = "rep";

		// Token: 0x0200036F RID: 879
		public class DefInjection
		{
			// Token: 0x1700044C RID: 1100
			// (get) Token: 0x06001696 RID: 5782 RVA: 0x000700A0 File Offset: 0x0006E2A0
			public bool IsFullListInjection
			{
				get
				{
					return this.fullListInjection != null;
				}
			}

			// Token: 0x1700044D RID: 1101
			// (get) Token: 0x06001697 RID: 5783 RVA: 0x000700AB File Offset: 0x0006E2AB
			public string DefName
			{
				get
				{
					if (!this.path.NullOrEmpty())
					{
						return this.path.Split('.', StringSplitOptions.None)[0];
					}
					return "";
				}
			}

			// Token: 0x06001698 RID: 5784 RVA: 0x000700D0 File Offset: 0x0006E2D0
			public bool ModifiesDefFromModOrNullCore(ModMetaData mod, Type defType)
			{
				Def defSilentFail = GenDefDatabase.GetDefSilentFail(defType, this.DefName, true);
				if (defSilentFail == null)
				{
					return mod.IsCoreMod;
				}
				if (mod == null)
				{
					return defSilentFail.modContentPack == null;
				}
				return defSilentFail.modContentPack != null && defSilentFail.modContentPack.FolderName == mod.FolderName;
			}

			// Token: 0x04001499 RID: 5273
			public string path;

			// Token: 0x0400149A RID: 5274
			public string normalizedPath;

			// Token: 0x0400149B RID: 5275
			public string nonBackCompatiblePath;

			// Token: 0x0400149C RID: 5276
			public string suggestedPath;

			// Token: 0x0400149D RID: 5277
			public string injection;

			// Token: 0x0400149E RID: 5278
			public List<string> fullListInjection;

			// Token: 0x0400149F RID: 5279
			public List<Pair<int, string>> fullListInjectionComments;

			// Token: 0x040014A0 RID: 5280
			public string fileSource;

			// Token: 0x040014A1 RID: 5281
			public bool injected;

			// Token: 0x040014A2 RID: 5282
			public string replacedString;

			// Token: 0x040014A3 RID: 5283
			public IEnumerable<string> replacedList;

			// Token: 0x040014A4 RID: 5284
			public bool isPlaceholder;
		}
	}
}
