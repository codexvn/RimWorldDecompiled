using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000862 RID: 2146
	public static class Scribe_Collections
	{
		// Token: 0x0600362A RID: 13866 RVA: 0x00121388 File Offset: 0x0011F588
		public static void Look<T>(ref List<T> list, string label, bool saveDestroyedThings, LookMode lookMode = LookMode.Undefined, params object[] ctorArgs)
		{
			if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
			{
				if (Scribe_Collections.lookListOfListsInfo == null)
				{
					Type typeFromHandle = typeof(Scribe_Collections);
					Scribe_Collections.lookListOfListsInfo = ((typeFromHandle != null) ? typeFromHandle.GetMethod("LookListOfLists", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic) : null);
				}
				MethodInfo methodInfo = Scribe_Collections.lookListOfListsInfo;
				MethodInfo methodInfo2 = (methodInfo != null) ? methodInfo.MakeGenericMethod(new Type[]
				{
					typeof(T).GetGenericArguments()[0]
				}) : null;
				object[] array = new object[]
				{
					list,
					saveDestroyedThings,
					label,
					lookMode,
					ctorArgs
				};
				methodInfo2.Invoke(null, array);
				list = (List<T>)array[0];
				return;
			}
			Scribe_Collections.Look<T>(ref list, saveDestroyedThings, label, lookMode, ctorArgs);
		}

		// Token: 0x0600362B RID: 13867 RVA: 0x0012146D File Offset: 0x0011F66D
		public static void Look<T>(ref List<T> list, string label, LookMode lookMode = LookMode.Undefined, params object[] ctorArgs)
		{
			Scribe_Collections.Look<T>(ref list, label, false, lookMode, ctorArgs);
		}

		// Token: 0x0600362C RID: 13868 RVA: 0x0012147C File Offset: 0x0011F67C
		private static void LookListOfLists<T>(ref List<List<T>> list, bool saveDestroyedThings, string label, LookMode lookMode = LookMode.Undefined, params object[] ctorArgs)
		{
			if (lookMode == LookMode.Undefined && !Scribe_Universal.TryResolveLookMode(typeof(List<T>), out lookMode, false, false))
			{
				string str = "LookList call with a list of ";
				Type typeFromHandle = typeof(List<T>);
				Log.Error(str + ((typeFromHandle != null) ? typeFromHandle.ToString() : null) + " must have lookMode set explicitly.");
				return;
			}
			if (Scribe.EnterNode(label))
			{
				try
				{
					if (Scribe.mode == LoadSaveMode.Saving)
					{
						if (list == null)
						{
							Scribe.saver.WriteAttribute("IsNull", "True");
							return;
						}
						using (List<List<T>>.Enumerator enumerator = list.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								List<T> list2 = enumerator.Current;
								Scribe_Collections.Look<T>(ref list2, saveDestroyedThings, "li", lookMode, ctorArgs);
							}
							return;
						}
					}
					if (Scribe.mode == LoadSaveMode.LoadingVars)
					{
						XmlNode curXmlParent = Scribe.loader.curXmlParent;
						XmlAttribute xmlAttribute = curXmlParent.Attributes["IsNull"];
						if (xmlAttribute != null && xmlAttribute.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
						{
							if (lookMode == LookMode.Reference)
							{
								Scribe.loader.crossRefs.loadIDs.RegisterLoadIDListReadFromXml(null, null);
							}
							list = null;
							return;
						}
						list = new List<List<T>>(curXmlParent.ChildNodes.Count);
						int num = 0;
						using (IEnumerator enumerator2 = curXmlParent.ChildNodes.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								object obj = enumerator2.Current;
								List<T> item = new List<T>(((XmlNode)obj).ChildNodes.Count);
								Scribe_Collections.Look<T>(ref item, saveDestroyedThings, num.ToString(), lookMode, ctorArgs);
								list.Add(item);
								num++;
							}
							return;
						}
					}
					if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs && list != null)
					{
						for (int i = 0; i < list.Count; i++)
						{
							List<T> value = list[i];
							Scribe_Collections.Look<T>(ref value, saveDestroyedThings, i.ToString(), lookMode, ctorArgs);
							list[i] = value;
						}
					}
					return;
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (lookMode == LookMode.Reference)
				{
					Scribe.loader.crossRefs.loadIDs.RegisterLoadIDListReadFromXml(null, label);
				}
				list = null;
			}
		}

		// Token: 0x0600362D RID: 13869 RVA: 0x001216E4 File Offset: 0x0011F8E4
		private static void Look<T>(ref List<T> list, bool saveDestroyedThings, string label, LookMode lookMode = LookMode.Undefined, params object[] ctorArgs)
		{
			if (lookMode == LookMode.Undefined && !Scribe_Universal.TryResolveLookMode(typeof(T), out lookMode, false, false))
			{
				string str = "LookList call with a list of ";
				Type typeFromHandle = typeof(T);
				Log.Error(str + ((typeFromHandle != null) ? typeFromHandle.ToString() : null) + " must have lookMode set explicitly.");
				return;
			}
			if (Scribe.EnterNode(label))
			{
				try
				{
					if (Scribe.mode == LoadSaveMode.Saving)
					{
						if (list == null)
						{
							Scribe.saver.WriteAttribute("IsNull", "True");
							return;
						}
						using (List<T>.Enumerator enumerator = list.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								T t = enumerator.Current;
								if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
								{
									throw new InvalidOperationException("This case should be impossible; it should be calling the list-of-lists overload.");
								}
								if (lookMode == LookMode.Value)
								{
									T t2 = t;
									Scribe_Values.Look<T>(ref t2, "li", default(T), true);
								}
								else if (lookMode == LookMode.LocalTargetInfo)
								{
									LocalTargetInfo localTargetInfo = (LocalTargetInfo)((object)t);
									Scribe_TargetInfo.Look(ref localTargetInfo, saveDestroyedThings, "li");
								}
								else if (lookMode == LookMode.TargetInfo)
								{
									TargetInfo targetInfo = (TargetInfo)((object)t);
									Scribe_TargetInfo.Look(ref targetInfo, saveDestroyedThings, "li");
								}
								else if (lookMode == LookMode.GlobalTargetInfo)
								{
									GlobalTargetInfo globalTargetInfo = (GlobalTargetInfo)((object)t);
									Scribe_TargetInfo.Look(ref globalTargetInfo, saveDestroyedThings, "li");
								}
								else if (lookMode == LookMode.Def)
								{
									Def def = (Def)((object)t);
									Scribe_Defs.Look<Def>(ref def, "li");
								}
								else if (lookMode == LookMode.BodyPart)
								{
									BodyPartRecord bodyPartRecord = (BodyPartRecord)((object)t);
									Scribe_BodyParts.Look(ref bodyPartRecord, "li", null);
								}
								else if (lookMode == LookMode.Deep)
								{
									T t3 = t;
									Scribe_Deep.Look<T>(ref t3, saveDestroyedThings, "li", ctorArgs);
								}
								else if (lookMode == LookMode.Reference)
								{
									if (t != null && !(t is ILoadReferenceable))
									{
										string str2 = "Cannot save reference to ";
										string str3;
										if (t == null)
										{
											str3 = null;
										}
										else
										{
											Type type = t.GetType();
											str3 = ((type != null) ? type.ToStringSafe<Type>() : null);
										}
										throw new InvalidOperationException(str2 + str3 + " item if it is not ILoadReferenceable");
									}
									ILoadReferenceable loadReferenceable = t as ILoadReferenceable;
									Scribe_References.Look<ILoadReferenceable>(ref loadReferenceable, "li", saveDestroyedThings);
								}
							}
							return;
						}
					}
					if (Scribe.mode == LoadSaveMode.LoadingVars)
					{
						XmlNode curXmlParent = Scribe.loader.curXmlParent;
						XmlAttribute xmlAttribute = curXmlParent.Attributes["IsNull"];
						if (xmlAttribute != null && xmlAttribute.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
						{
							if (lookMode == LookMode.Reference)
							{
								Scribe.loader.crossRefs.loadIDs.RegisterLoadIDListReadFromXml(null, null);
							}
							list = null;
						}
						else
						{
							if (lookMode == LookMode.Value)
							{
								list = new List<T>(curXmlParent.ChildNodes.Count);
								using (IEnumerator enumerator2 = curXmlParent.ChildNodes.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										object obj = enumerator2.Current;
										T item = ScribeExtractor.ValueFromNode<T>((XmlNode)obj, default(T));
										list.Add(item);
									}
									return;
								}
							}
							if (lookMode == LookMode.Deep)
							{
								list = new List<T>(curXmlParent.ChildNodes.Count);
								using (IEnumerator enumerator2 = curXmlParent.ChildNodes.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										object obj2 = enumerator2.Current;
										T item2 = ScribeExtractor.SaveableFromNode<T>((XmlNode)obj2, ctorArgs);
										list.Add(item2);
									}
									return;
								}
							}
							if (lookMode == LookMode.Def)
							{
								list = new List<T>(curXmlParent.ChildNodes.Count);
								using (IEnumerator enumerator2 = curXmlParent.ChildNodes.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										object obj3 = enumerator2.Current;
										T item3 = ScribeExtractor.DefFromNodeUnsafe<T>((XmlNode)obj3);
										list.Add(item3);
									}
									return;
								}
							}
							if (lookMode == LookMode.BodyPart)
							{
								list = new List<T>(curXmlParent.ChildNodes.Count);
								int num = 0;
								using (IEnumerator enumerator2 = curXmlParent.ChildNodes.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										object obj4 = enumerator2.Current;
										T item4 = (T)((object)ScribeExtractor.BodyPartFromNode((XmlNode)obj4, num.ToString(), null));
										list.Add(item4);
										num++;
									}
									return;
								}
							}
							if (lookMode == LookMode.LocalTargetInfo)
							{
								list = new List<T>(curXmlParent.ChildNodes.Count);
								int num2 = 0;
								using (IEnumerator enumerator2 = curXmlParent.ChildNodes.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										object obj5 = enumerator2.Current;
										T item5 = (T)((object)ScribeExtractor.LocalTargetInfoFromNode((XmlNode)obj5, num2.ToString(), LocalTargetInfo.Invalid));
										list.Add(item5);
										num2++;
									}
									return;
								}
							}
							if (lookMode == LookMode.TargetInfo)
							{
								list = new List<T>(curXmlParent.ChildNodes.Count);
								int num3 = 0;
								using (IEnumerator enumerator2 = curXmlParent.ChildNodes.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										object obj6 = enumerator2.Current;
										T item6 = (T)((object)ScribeExtractor.TargetInfoFromNode((XmlNode)obj6, num3.ToString(), TargetInfo.Invalid));
										list.Add(item6);
										num3++;
									}
									return;
								}
							}
							if (lookMode == LookMode.GlobalTargetInfo)
							{
								list = new List<T>(curXmlParent.ChildNodes.Count);
								int num4 = 0;
								using (IEnumerator enumerator2 = curXmlParent.ChildNodes.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										object obj7 = enumerator2.Current;
										T item7 = (T)((object)ScribeExtractor.GlobalTargetInfoFromNode((XmlNode)obj7, num4.ToString(), GlobalTargetInfo.Invalid));
										list.Add(item7);
										num4++;
									}
									return;
								}
							}
							if (lookMode == LookMode.Reference)
							{
								List<string> list2 = new List<string>(curXmlParent.ChildNodes.Count);
								foreach (object obj8 in curXmlParent.ChildNodes)
								{
									XmlNode xmlNode = (XmlNode)obj8;
									list2.Add(xmlNode.InnerText);
								}
								Scribe.loader.crossRefs.loadIDs.RegisterLoadIDListReadFromXml(list2, "");
							}
						}
					}
					else if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
					{
						if (lookMode == LookMode.Reference)
						{
							list = Scribe.loader.crossRefs.TakeResolvedRefList<T>("");
						}
						else if (lookMode == LookMode.LocalTargetInfo)
						{
							if (list != null)
							{
								for (int i = 0; i < list.Count; i++)
								{
									list[i] = (T)((object)ScribeExtractor.ResolveLocalTargetInfo((LocalTargetInfo)((object)list[i]), i.ToString()));
								}
							}
						}
						else if (lookMode == LookMode.TargetInfo)
						{
							if (list != null)
							{
								for (int j = 0; j < list.Count; j++)
								{
									list[j] = (T)((object)ScribeExtractor.ResolveTargetInfo((TargetInfo)((object)list[j]), j.ToString()));
								}
							}
						}
						else if (lookMode == LookMode.GlobalTargetInfo && list != null)
						{
							for (int k = 0; k < list.Count; k++)
							{
								list[k] = (T)((object)ScribeExtractor.ResolveGlobalTargetInfo((GlobalTargetInfo)((object)list[k]), k.ToString()));
							}
						}
					}
					return;
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (lookMode == LookMode.Reference)
				{
					Scribe.loader.crossRefs.loadIDs.RegisterLoadIDListReadFromXml(null, label);
				}
				list = null;
			}
		}

		// Token: 0x0600362E RID: 13870 RVA: 0x00121FB0 File Offset: 0x001201B0
		public static void Look<K, V>(ref Dictionary<K, V> dict, string label, LookMode keyLookMode = LookMode.Undefined, LookMode valueLookMode = LookMode.Undefined)
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				bool flag = keyLookMode == LookMode.Reference;
				bool flag2 = valueLookMode == LookMode.Reference;
				if (flag != flag2)
				{
					Log.Error("You need to provide working lists for the keys and values in order to be able to load such dictionary. label=" + label);
				}
			}
			List<K> list = null;
			List<V> list2 = null;
			Scribe_Collections.Look<K, V>(ref dict, label, keyLookMode, valueLookMode, ref list, ref list2, true, false, false);
		}

		// Token: 0x0600362F RID: 13871 RVA: 0x00121FF8 File Offset: 0x001201F8
		private static void BuildDictionary<K, V>(Dictionary<K, V> dict, List<K> keysWorkingList, List<V> valuesWorkingList, string label, bool logNullErrors)
		{
			if (dict == null)
			{
				return;
			}
			if (keysWorkingList == null)
			{
				Log.Error("Cannot fill dictionary because there are no keys. label=" + label);
				return;
			}
			if (valuesWorkingList == null)
			{
				Log.Error("Cannot fill dictionary because there are no values. label=" + label);
				return;
			}
			if (keysWorkingList.Count != valuesWorkingList.Count)
			{
				Log.Error(string.Concat(new string[]
				{
					"Keys count does not match the values count while loading a dictionary (maybe keys and values were resolved during different passes?). Some elements will be skipped. keys=",
					keysWorkingList.Count.ToString(),
					", values=",
					valuesWorkingList.Count.ToString(),
					", label=",
					label
				}));
			}
			int num = Math.Min(keysWorkingList.Count, valuesWorkingList.Count);
			for (int i = 0; i < num; i++)
			{
				if (keysWorkingList[i] == null)
				{
					if (logNullErrors)
					{
						string[] array = new string[6];
						array[0] = "Null key while loading dictionary of ";
						int num2 = 1;
						Type typeFromHandle = typeof(K);
						array[num2] = ((typeFromHandle != null) ? typeFromHandle.ToString() : null);
						array[2] = " and ";
						int num3 = 3;
						Type typeFromHandle2 = typeof(V);
						array[num3] = ((typeFromHandle2 != null) ? typeFromHandle2.ToString() : null);
						array[4] = ". label=";
						array[5] = label;
						Log.Error(string.Concat(array));
					}
				}
				else
				{
					try
					{
						V v;
						if (dict.TryGetValue(keysWorkingList[i], out v))
						{
							if (!object.Equals(v, valuesWorkingList[i]))
							{
								throw new InvalidOperationException("Tried to add different values for the same key.");
							}
						}
						else
						{
							dict.Add(keysWorkingList[i], valuesWorkingList[i]);
						}
					}
					catch (OutOfMemoryException)
					{
						throw;
					}
					catch (Exception ex)
					{
						string str = "Exception in LookDictionary(label=";
						string str2 = "): ";
						Exception ex2 = ex;
						Log.Error(str + label + str2 + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
		}

		// Token: 0x06003630 RID: 13872 RVA: 0x001221B8 File Offset: 0x001203B8
		public static void Look<K, V>(ref Dictionary<K, V> dict, string label, LookMode keyLookMode, LookMode valueLookMode, ref List<K> keysWorkingList, ref List<V> valuesWorkingList, bool logNullErrors = true, bool saveDestroyedKeys = false, bool saveDestroyedValues = false)
		{
			if (Scribe.EnterNode(label))
			{
				try
				{
					if (Scribe.mode == LoadSaveMode.Saving && dict == null)
					{
						Scribe.saver.WriteAttribute("IsNull", "True");
						return;
					}
					if (Scribe.mode == LoadSaveMode.Saving && typeof(K).IsGenericType && typeof(K).GetGenericTypeDefinition() == typeof(List<>))
					{
						throw new InvalidOperationException("Cannot sensibly save a dictionary with a key type which is a list.");
					}
					if (Scribe.mode == LoadSaveMode.LoadingVars)
					{
						XmlAttribute xmlAttribute = Scribe.loader.curXmlParent.Attributes["IsNull"];
						if (xmlAttribute != null && xmlAttribute.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
						{
							dict = null;
						}
						else
						{
							dict = new Dictionary<K, V>();
						}
					}
					if (Scribe.mode == LoadSaveMode.Saving || Scribe.mode == LoadSaveMode.LoadingVars)
					{
						keysWorkingList = new List<K>();
						valuesWorkingList = new List<V>();
						if (Scribe.mode == LoadSaveMode.Saving && dict != null)
						{
							foreach (KeyValuePair<K, V> keyValuePair in dict)
							{
								keysWorkingList.Add(keyValuePair.Key);
								valuesWorkingList.Add(keyValuePair.Value);
							}
						}
					}
					if (Scribe.mode == LoadSaveMode.Saving || dict != null)
					{
						Scribe_Collections.Look<K>(ref keysWorkingList, "keys", saveDestroyedKeys, keyLookMode, Array.Empty<object>());
						Scribe_Collections.Look<V>(ref valuesWorkingList, "values", saveDestroyedValues, valueLookMode, Array.Empty<object>());
					}
					if (Scribe.mode == LoadSaveMode.Saving)
					{
						if (keysWorkingList != null)
						{
							keysWorkingList.Clear();
							keysWorkingList = null;
						}
						if (valuesWorkingList != null)
						{
							valuesWorkingList.Clear();
							valuesWorkingList = null;
						}
					}
					bool flag = keyLookMode == LookMode.Reference || valueLookMode == LookMode.Reference;
					if ((flag && Scribe.mode == LoadSaveMode.ResolvingCrossRefs) || (!flag && Scribe.mode == LoadSaveMode.LoadingVars))
					{
						Scribe_Collections.BuildDictionary<K, V>(dict, keysWorkingList, valuesWorkingList, label, logNullErrors);
					}
					if (Scribe.mode == LoadSaveMode.PostLoadInit)
					{
						if (keysWorkingList != null)
						{
							keysWorkingList.Clear();
							keysWorkingList = null;
						}
						if (valuesWorkingList != null)
						{
							valuesWorkingList.Clear();
							valuesWorkingList = null;
						}
					}
					return;
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				dict = null;
			}
		}

		// Token: 0x06003631 RID: 13873 RVA: 0x001223F4 File Offset: 0x001205F4
		public static void Look<T>(ref HashSet<T> valueHashSet, string label, LookMode lookMode = LookMode.Undefined)
		{
			Scribe_Collections.Look<T>(ref valueHashSet, false, label, lookMode);
		}

		// Token: 0x06003632 RID: 13874 RVA: 0x00122400 File Offset: 0x00120600
		public static void Look<T>(ref HashSet<T> valueHashSet, bool saveDestroyedThings, string label, LookMode lookMode = LookMode.Undefined)
		{
			List<T> list = null;
			if (Scribe.mode == LoadSaveMode.Saving && valueHashSet != null)
			{
				list = new List<T>();
				foreach (T item in valueHashSet)
				{
					list.Add(item);
				}
			}
			Scribe_Collections.Look<T>(ref list, saveDestroyedThings, label, lookMode, Array.Empty<object>());
			if ((lookMode == LookMode.Reference && Scribe.mode == LoadSaveMode.ResolvingCrossRefs) || (lookMode != LookMode.Reference && Scribe.mode == LoadSaveMode.LoadingVars))
			{
				if (list == null)
				{
					valueHashSet = null;
					return;
				}
				valueHashSet = new HashSet<T>();
				for (int i = 0; i < list.Count; i++)
				{
					valueHashSet.Add(list[i]);
				}
			}
		}

		// Token: 0x06003633 RID: 13875 RVA: 0x001224B8 File Offset: 0x001206B8
		public static void Look<T>(ref Stack<T> valueStack, string label, LookMode lookMode = LookMode.Undefined)
		{
			List<T> list = null;
			if (Scribe.mode == LoadSaveMode.Saving && valueStack != null)
			{
				list = new List<T>();
				foreach (T item in valueStack)
				{
					list.Add(item);
				}
			}
			Scribe_Collections.Look<T>(ref list, label, lookMode, Array.Empty<object>());
			if ((lookMode == LookMode.Reference && Scribe.mode == LoadSaveMode.ResolvingCrossRefs) || (lookMode != LookMode.Reference && Scribe.mode == LoadSaveMode.LoadingVars))
			{
				if (list == null)
				{
					valueStack = null;
					return;
				}
				valueStack = new Stack<T>();
				for (int i = 0; i < list.Count; i++)
				{
					valueStack.Push(list[i]);
				}
			}
		}

		// Token: 0x06003634 RID: 13876 RVA: 0x00122570 File Offset: 0x00120770
		public static void Look<T>(ref Queue<T> valueQueue, string label, LookMode lookMode = LookMode.Undefined)
		{
			List<T> list = null;
			if (Scribe.mode == LoadSaveMode.Saving && valueQueue != null)
			{
				list = new List<T>();
				foreach (T item in valueQueue)
				{
					list.Add(item);
				}
			}
			Scribe_Collections.Look<T>(ref list, label, lookMode, Array.Empty<object>());
			if ((lookMode == LookMode.Reference && Scribe.mode == LoadSaveMode.ResolvingCrossRefs) || (lookMode != LookMode.Reference && Scribe.mode == LoadSaveMode.LoadingVars))
			{
				if (list == null)
				{
					valueQueue = null;
					return;
				}
				valueQueue = new Queue<T>();
				for (int i = 0; i < list.Count; i++)
				{
					valueQueue.Enqueue(list[i]);
				}
			}
		}

		// Token: 0x040028E2 RID: 10466
		private static MethodInfo lookListOfListsInfo;
	}
}
