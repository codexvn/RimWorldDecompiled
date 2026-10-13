using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200081C RID: 2076
	public static class DirectXmlCrossRefLoader
	{
		// Token: 0x060034CA RID: 13514 RVA: 0x00115338 File Offset: 0x00113538
		public static bool MistypedMayRequire(string mayRequireMod)
		{
			if (!Application.isEditor)
			{
				return false;
			}
			if (mayRequireMod.NullOrEmpty())
			{
				return false;
			}
			if (mayRequireMod.Contains(','))
			{
				string[] array = mayRequireMod.Split(',', StringSplitOptions.None);
				for (int i = 0; i < array.Length; i++)
				{
					if (!DirectXmlCrossRefLoader.<MistypedMayRequire>g__ExpansionFound|0_0(array[i]))
					{
						return true;
					}
				}
			}
			else if (!DirectXmlCrossRefLoader.<MistypedMayRequire>g__ExpansionFound|0_0(mayRequireMod))
			{
				return true;
			}
			return false;
		}

		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x060034CB RID: 13515 RVA: 0x00115393 File Offset: 0x00113593
		public static bool LoadingInProgress
		{
			get
			{
				return DirectXmlCrossRefLoader.wantedRefs.Count > 0;
			}
		}

		// Token: 0x060034CC RID: 13516 RVA: 0x001153A4 File Offset: 0x001135A4
		public static void RegisterObjectWantsCrossRef(object wanter, FieldInfo fi, string targetDefName, string mayRequireMod = null, string mayRequireAnyMod = null, Type assumeFieldType = null)
		{
			DeepProfiler.Start("RegisterObjectWantsCrossRef (object, FieldInfo, string)");
			try
			{
				if (wanter.GetType().IsValueType)
				{
					Log.Error(string.Format("Cannot use value types for object cross reference. {0} is a value type but wants a cross reference to {1} {2} via field {3}.", new object[]
					{
						wanter.GetType(),
						fi.FieldType,
						targetDefName,
						fi.Name
					}));
				}
				DirectXmlCrossRefLoader.WantedRefForObject item = new DirectXmlCrossRefLoader.WantedRefForObject(wanter, fi, targetDefName, mayRequireMod, mayRequireAnyMod, assumeFieldType);
				DirectXmlCrossRefLoader.wantedRefs.Add(item);
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x060034CD RID: 13517 RVA: 0x00115430 File Offset: 0x00113630
		public static void RegisterObjectWantsCrossRef(object wanter, string fieldName, string targetDefName, string mayRequireMod = null, string mayRequireAnyMod = null, Type overrideFieldType = null)
		{
			DeepProfiler.Start("RegisterObjectWantsCrossRef (object,string,string)");
			try
			{
				FieldInfo field = wanter.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (wanter.GetType().IsValueType)
				{
					Log.Error(string.Format("Cannot use value types for object cross reference. {0} is a value type but wants a cross reference to {1} {2} via field {3}.", new object[]
					{
						wanter.GetType(),
						(field != null) ? field.FieldType : null,
						targetDefName,
						((field != null) ? field.Name : null) ?? fieldName
					}));
				}
				DirectXmlCrossRefLoader.WantedRefForObject item = new DirectXmlCrossRefLoader.WantedRefForObject(wanter, field, targetDefName, mayRequireMod, mayRequireAnyMod, overrideFieldType);
				DirectXmlCrossRefLoader.wantedRefs.Add(item);
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x060034CE RID: 13518 RVA: 0x001154DC File Offset: 0x001136DC
		public static void RegisterObjectWantsCrossRef(object wanter, string fieldName, XmlNode parentNode, string mayRequireMod = null, string mayRequireAnyMod = null, Type overrideFieldType = null)
		{
			DeepProfiler.Start("RegisterObjectWantsCrossRef (object,string,XmlNode)");
			try
			{
				if (wanter.GetType().IsValueType)
				{
					Log.Error(string.Format("Cannot use value types for object cross reference. {0} is a value type but wants a cross reference to {1} via field {2}.", wanter.GetType(), parentNode.Name, fieldName));
				}
				string text = mayRequireMod;
				if (mayRequireMod == null)
				{
					XmlAttributeCollection attributes = parentNode.Attributes;
					if (attributes == null)
					{
						text = null;
					}
					else
					{
						XmlAttribute xmlAttribute = attributes["MayRequire"];
						text = ((xmlAttribute != null) ? xmlAttribute.Value.ToLower() : null);
					}
				}
				string mayRequireMod2 = text;
				string text2 = mayRequireAnyMod;
				if (mayRequireAnyMod == null)
				{
					XmlAttributeCollection attributes2 = parentNode.Attributes;
					if (attributes2 == null)
					{
						text2 = null;
					}
					else
					{
						XmlAttribute xmlAttribute2 = attributes2["MayRequireAnyOf"];
						text2 = ((xmlAttribute2 != null) ? xmlAttribute2.Value.ToLower() : null);
					}
				}
				string mayRequireAnyMod2 = text2;
				DirectXmlCrossRefLoader.WantedRefForObject item = new DirectXmlCrossRefLoader.WantedRefForObject(wanter, wanter.GetType().GetField(fieldName), parentNode.Name, mayRequireMod2, mayRequireAnyMod2, overrideFieldType);
				DirectXmlCrossRefLoader.wantedRefs.Add(item);
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x060034CF RID: 13519 RVA: 0x001155BC File Offset: 0x001137BC
		public static void RegisterListWantsCrossRef<T>(List<T> wanterList, string targetDefName, object debugWanterInfo = null, string mayRequireMod = null, string mayRequireAnyMod = null)
		{
			DeepProfiler.Start("RegisterListWantsCrossRef");
			try
			{
				DirectXmlCrossRefLoader.WantedRef wantedRef;
				DirectXmlCrossRefLoader.WantedRefForList<T> wantedRefForList;
				if (!DirectXmlCrossRefLoader.wantedListDictRefs.TryGetValue(wanterList, out wantedRef))
				{
					wantedRefForList = new DirectXmlCrossRefLoader.WantedRefForList<T>(wanterList, debugWanterInfo);
					DirectXmlCrossRefLoader.wantedListDictRefs.Add(wanterList, wantedRefForList);
					DirectXmlCrossRefLoader.wantedRefs.Add(wantedRefForList);
				}
				else
				{
					wantedRefForList = (DirectXmlCrossRefLoader.WantedRefForList<T>)wantedRef;
				}
				wantedRefForList.AddWantedListEntry(targetDefName, mayRequireMod, mayRequireAnyMod);
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x060034D0 RID: 13520 RVA: 0x00115630 File Offset: 0x00113830
		public static void RegisterDictionaryWantsCrossRef<K, V>(Dictionary<K, V> wanterDict, XmlNode entryNode, object debugWanterInfo = null)
		{
			DeepProfiler.Start("RegisterDictionaryWantsCrossRef");
			try
			{
				DirectXmlCrossRefLoader.WantedRef wantedRef;
				DirectXmlCrossRefLoader.WantedRefForDictionary<K, V> wantedRefForDictionary;
				if (!DirectXmlCrossRefLoader.wantedListDictRefs.TryGetValue(wanterDict, out wantedRef))
				{
					wantedRefForDictionary = new DirectXmlCrossRefLoader.WantedRefForDictionary<K, V>(wanterDict, debugWanterInfo);
					DirectXmlCrossRefLoader.wantedRefs.Add(wantedRefForDictionary);
					DirectXmlCrossRefLoader.wantedListDictRefs.Add(wanterDict, wantedRefForDictionary);
				}
				else
				{
					wantedRefForDictionary = (DirectXmlCrossRefLoader.WantedRefForDictionary<K, V>)wantedRef;
				}
				wantedRefForDictionary.AddWantedDictEntry(entryNode);
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x060034D1 RID: 13521 RVA: 0x001156A0 File Offset: 0x001138A0
		public static T TryResolveDef<T>(string defName, FailMode failReportMode, object debugWanterInfo = null)
		{
			DeepProfiler.Start("TryResolveDef");
			T result;
			try
			{
				T t = (T)((object)GenDefDatabase.GetDefSilentFail(typeof(T), defName, true));
				if (t != null)
				{
					result = t;
				}
				else
				{
					if (failReportMode == FailMode.LogErrors)
					{
						string str = "Could not resolve cross-reference to ";
						Type typeFromHandle = typeof(T);
						string text = str + ((typeFromHandle != null) ? typeFromHandle.ToString() : null) + " named " + defName.ToStringSafe<string>();
						if (debugWanterInfo != null)
						{
							text = text + " (wanter=" + debugWanterInfo.ToStringSafe<object>() + ")";
						}
						Log.Error(text);
					}
					result = default(T);
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			return result;
		}

		// Token: 0x060034D2 RID: 13522 RVA: 0x00115750 File Offset: 0x00113950
		public static void Clear()
		{
			DeepProfiler.Start("Clear");
			try
			{
				DirectXmlCrossRefLoader.wantedRefs.Clear();
				DirectXmlCrossRefLoader.wantedListDictRefs.Clear();
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x060034D3 RID: 13523 RVA: 0x00115794 File Offset: 0x00113994
		public static void ResolveAllWantedCrossReferences(FailMode failReportMode)
		{
			DeepProfiler.Start("ResolveAllWantedCrossReferences");
			try
			{
				HashSet<DirectXmlCrossRefLoader.WantedRef> resolvedRefs = new HashSet<DirectXmlCrossRefLoader.WantedRef>();
				object resolvedRefsLock = new object();
				DeepProfiler.enabled = false;
				GenThreading.ParallelForEach<DirectXmlCrossRefLoader.WantedRef>(DirectXmlCrossRefLoader.wantedRefs, delegate(DirectXmlCrossRefLoader.WantedRef wantedRef)
				{
					if (wantedRef.TryResolve(failReportMode))
					{
						object resolvedRefsLock = resolvedRefsLock;
						lock (resolvedRefsLock)
						{
							resolvedRefs.Add(wantedRef);
						}
					}
				}, -1);
				foreach (DirectXmlCrossRefLoader.WantedRef wantedRef2 in resolvedRefs)
				{
					wantedRef2.Apply();
				}
				DirectXmlCrossRefLoader.wantedRefs.RemoveAll((DirectXmlCrossRefLoader.WantedRef x) => resolvedRefs.Contains(x));
				DeepProfiler.enabled = true;
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x060034D5 RID: 13525 RVA: 0x0011587C File Offset: 0x00113A7C
		[CompilerGenerated]
		internal static bool <MistypedMayRequire>g__ExpansionFound|0_0(string modID)
		{
			for (int i = 0; i < ModContentPack.ProductPackageIDs.Length; i++)
			{
				if (modID.EqualsIgnoreCase(ModContentPack.ProductPackageIDs[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040027C8 RID: 10184
		private static List<DirectXmlCrossRefLoader.WantedRef> wantedRefs = new List<DirectXmlCrossRefLoader.WantedRef>();

		// Token: 0x040027C9 RID: 10185
		private static Dictionary<object, DirectXmlCrossRefLoader.WantedRef> wantedListDictRefs = new Dictionary<object, DirectXmlCrossRefLoader.WantedRef>();

		// Token: 0x0200081D RID: 2077
		private abstract class WantedRef
		{
			// Token: 0x060034D6 RID: 13526
			public abstract bool TryResolve(FailMode failReportMode);

			// Token: 0x060034D7 RID: 13527 RVA: 0x000026EA File Offset: 0x000008EA
			public virtual void Apply()
			{
			}

			// Token: 0x040027CA RID: 10186
			public object wanter;
		}

		// Token: 0x0200081E RID: 2078
		private class WantedRefForObject : DirectXmlCrossRefLoader.WantedRef
		{
			// Token: 0x170009DA RID: 2522
			// (get) Token: 0x060034D9 RID: 13529 RVA: 0x001158AD File Offset: 0x00113AAD
			private bool BadCrossRefAllowed
			{
				get
				{
					return (!this.mayRequireMod.NullOrEmpty() && !ModsConfig.AreAllActive(this.mayRequireMod)) || (!this.mayRequireAnyMod.NullOrEmpty<string>() && !ModsConfig.IsAnyActiveOrEmpty(this.mayRequireAnyMod, false));
				}
			}

			// Token: 0x060034DA RID: 13530 RVA: 0x001158EC File Offset: 0x00113AEC
			public WantedRefForObject(object wanter, FieldInfo fi, string targetDefName, string mayRequireMod = null, string mayRequireAnyMod = null, Type overrideFieldType = null)
			{
				this.wanter = wanter;
				this.fi = fi;
				this.defName = targetDefName;
				this.mayRequireMod = mayRequireMod;
				this.overrideFieldType = overrideFieldType;
				this.mayRequireAnyMod = ((mayRequireAnyMod != null) ? mayRequireAnyMod.ToLower().Split(',', StringSplitOptions.None) : null);
			}

			// Token: 0x060034DB RID: 13531 RVA: 0x00115940 File Offset: 0x00113B40
			public override bool TryResolve(FailMode failReportMode)
			{
				if (this.fi == null)
				{
					Log.Error("Trying to resolve null field for def named " + this.defName.ToStringSafe<string>());
					return false;
				}
				Type type = this.overrideFieldType ?? this.fi.FieldType;
				this.resolvedDef = GenDefDatabase.GetDefSilentFail(type, this.defName, true);
				if (DirectXmlCrossRefLoader.MistypedMayRequire(this.mayRequireMod))
				{
					Log.Error("Faulty MayRequire at def " + this.defName.ToStringSafe<string>() + ": " + this.mayRequireMod);
				}
				if (!this.mayRequireAnyMod.NullOrEmpty<string>())
				{
					foreach (string str in this.mayRequireAnyMod)
					{
						if (DirectXmlCrossRefLoader.MistypedMayRequire(str))
						{
							Log.Error("Faulty MayRequire at def " + this.defName.ToStringSafe<string>() + ": " + str);
						}
					}
				}
				if (this.resolvedDef == null)
				{
					if (failReportMode == FailMode.LogErrors && !this.BadCrossRefAllowed)
					{
						string[] array2 = new string[8];
						array2[0] = "Could not resolve cross-reference: No ";
						int num = 1;
						Type type2 = type;
						array2[num] = ((type2 != null) ? type2.ToString() : null);
						array2[2] = " named ";
						array2[3] = this.defName.ToStringSafe<string>();
						array2[4] = " found to give to ";
						int num2 = 5;
						Type type3 = this.wanter.GetType();
						array2[num2] = ((type3 != null) ? type3.ToString() : null);
						array2[6] = " ";
						array2[7] = this.wanter.ToStringSafe<object>();
						Log.Error(string.Concat(array2));
					}
					return false;
				}
				SoundDef soundDef = this.resolvedDef as SoundDef;
				if (soundDef != null && soundDef.isUndefined)
				{
					string[] array3 = new string[9];
					array3[0] = "Could not resolve cross-reference: No ";
					int num3 = 1;
					Type type4 = type;
					array3[num3] = ((type4 != null) ? type4.ToString() : null);
					array3[2] = " named ";
					array3[3] = this.defName.ToStringSafe<string>();
					array3[4] = " found to give to ";
					int num4 = 5;
					Type type5 = this.wanter.GetType();
					array3[num4] = ((type5 != null) ? type5.ToString() : null);
					array3[6] = " ";
					array3[7] = this.wanter.ToStringSafe<object>();
					array3[8] = " (using undefined sound instead)";
					Log.Warning(string.Concat(array3));
				}
				this.fi.SetValue(this.wanter, this.resolvedDef);
				return true;
			}

			// Token: 0x040027CB RID: 10187
			private readonly FieldInfo fi;

			// Token: 0x040027CC RID: 10188
			private readonly string defName;

			// Token: 0x040027CD RID: 10189
			private readonly string mayRequireMod;

			// Token: 0x040027CE RID: 10190
			private readonly string[] mayRequireAnyMod;

			// Token: 0x040027CF RID: 10191
			private readonly Type overrideFieldType;

			// Token: 0x040027D0 RID: 10192
			private Def resolvedDef;
		}

		// Token: 0x0200081F RID: 2079
		private class WantedRefForList<T> : DirectXmlCrossRefLoader.WantedRef
		{
			// Token: 0x060034DC RID: 13532 RVA: 0x00115B63 File Offset: 0x00113D63
			public WantedRefForList(object wanter, object debugWanterInfo)
			{
				this.wanter = wanter;
				this.debugWanterInfo = debugWanterInfo;
			}

			// Token: 0x060034DD RID: 13533 RVA: 0x00115B84 File Offset: 0x00113D84
			public void AddWantedListEntry(string newTargetDefName, string mayRequireMod = null, string mayRequireAnyMod = null)
			{
				if (!mayRequireMod.NullOrEmpty() && this.mayRequireMods == null)
				{
					this.mayRequireMods = new List<string>();
					for (int i = 0; i < this.defNames.Count; i++)
					{
						this.mayRequireMods.Add(null);
					}
				}
				if (!mayRequireAnyMod.NullOrEmpty() && this.mayRequireModsAny == null)
				{
					this.mayRequireModsAny = new Dictionary<string, List<string>>();
					for (int j = 0; j < this.defNames.Count; j++)
					{
						this.mayRequireModsAny.Add(this.defNames[j], new List<string>());
					}
				}
				this.defNames.Add(newTargetDefName);
				if (this.mayRequireMods != null)
				{
					this.mayRequireMods.Add(mayRequireMod);
				}
				if (this.mayRequireModsAny != null)
				{
					foreach (string text in mayRequireAnyMod.ToLower().Split(',', StringSplitOptions.None))
					{
						List<string> list;
						if (this.mayRequireModsAny.TryGetValue(newTargetDefName, out list))
						{
							list.Add(text.Trim());
						}
						else
						{
							this.mayRequireModsAny.Add(newTargetDefName, new List<string>
							{
								text.Trim()
							});
						}
					}
				}
			}

			// Token: 0x060034DE RID: 13534 RVA: 0x00115CA4 File Offset: 0x00113EA4
			public override bool TryResolve(FailMode failReportMode)
			{
				bool flag = false;
				for (int i = 0; i < this.defNames.Count; i++)
				{
					bool flag2 = this.mayRequireMods != null && i < this.mayRequireMods.Count && !this.mayRequireMods[i].NullOrEmpty() && !ModsConfig.AreAllActive(this.mayRequireMods[i]);
					List<string> mods;
					if (this.mayRequireModsAny != null && this.mayRequireModsAny.TryGetValue(this.defNames[i], out mods) && !ModsConfig.IsAnyActiveOrEmpty(mods, false))
					{
						flag2 = true;
					}
					if (this.mayRequireMods != null && i < this.mayRequireMods.Count && DirectXmlCrossRefLoader.MistypedMayRequire(this.mayRequireMods[i]))
					{
						Log.Error("Faulty MayRequire: " + this.mayRequireMods[i]);
					}
					T t = DirectXmlCrossRefLoader.TryResolveDef<T>(this.defNames[i], flag2 ? FailMode.Silent : failReportMode, this.debugWanterInfo);
					if (t != null)
					{
						((List<T>)this.wanter).Add(t);
						this.defNames.RemoveAt(i);
						if (this.mayRequireMods != null && i < this.mayRequireMods.Count)
						{
							this.mayRequireMods.RemoveAt(i);
						}
						i--;
					}
					else
					{
						flag = true;
					}
				}
				return !flag;
			}

			// Token: 0x040027D1 RID: 10193
			private List<string> defNames = new List<string>();

			// Token: 0x040027D2 RID: 10194
			private List<string> mayRequireMods;

			// Token: 0x040027D3 RID: 10195
			private Dictionary<string, List<string>> mayRequireModsAny;

			// Token: 0x040027D4 RID: 10196
			private object debugWanterInfo;
		}

		// Token: 0x02000820 RID: 2080
		private class WantedRefForDictionary<K, V> : DirectXmlCrossRefLoader.WantedRef
		{
			// Token: 0x060034DF RID: 13535 RVA: 0x00115DF5 File Offset: 0x00113FF5
			public WantedRefForDictionary(object wanter, object debugWanterInfo)
			{
				this.wanter = wanter;
				this.debugWanterInfo = debugWanterInfo;
			}

			// Token: 0x060034E0 RID: 13536 RVA: 0x00115E21 File Offset: 0x00114021
			public void AddWantedDictEntry(XmlNode entryNode)
			{
				this.wantedDictRefs.Add(entryNode);
			}

			// Token: 0x060034E1 RID: 13537 RVA: 0x00115E30 File Offset: 0x00114030
			public override bool TryResolve(FailMode failReportMode)
			{
				failReportMode = FailMode.LogErrors;
				bool flag = GenTypes.IsDef(typeof(K));
				bool flag2 = GenTypes.IsDef(typeof(V));
				foreach (XmlNode xmlNode in this.wantedDictRefs)
				{
					XmlNode xmlNode2 = xmlNode["key"];
					XmlNode xmlNode3 = xmlNode["value"];
					string text = (xmlNode2 != null) ? xmlNode2.InnerText : null;
					string text2 = (xmlNode3 != null) ? xmlNode3.InnerText : null;
					object first;
					object second;
					if (text == null || text2 == null)
					{
						if (failReportMode == FailMode.LogErrors)
						{
							string text3 = "Missing 'key' and/or 'value'.";
							if (this.debugWanterInfo != null)
							{
								text3 = text3 + " (wanter=" + this.debugWanterInfo.ToStringSafe<object>() + ")";
							}
							Log.Error(text3);
						}
						first = default(K);
						second = default(V);
					}
					else
					{
						if (flag)
						{
							first = DirectXmlCrossRefLoader.TryResolveDef<K>(text, failReportMode, this.debugWanterInfo);
						}
						else
						{
							first = xmlNode2;
						}
						if (flag2)
						{
							second = DirectXmlCrossRefLoader.TryResolveDef<V>(text2, failReportMode, this.debugWanterInfo);
						}
						else
						{
							second = xmlNode3;
						}
					}
					this.makingData.Add(new Pair<object, object>(first, second));
				}
				return true;
			}

			// Token: 0x060034E2 RID: 13538 RVA: 0x00115F9C File Offset: 0x0011419C
			public override void Apply()
			{
				Dictionary<K, V> dictionary = (Dictionary<K, V>)this.wanter;
				dictionary.Clear();
				foreach (Pair<object, object> pair in this.makingData)
				{
					try
					{
						object obj = pair.First;
						object obj2 = pair.Second;
						if (obj is XmlNode)
						{
							obj = DirectXmlToObject.ObjectFromXml<K>(obj as XmlNode, true);
						}
						if (obj2 is XmlNode)
						{
							obj2 = DirectXmlToObject.ObjectFromXml<V>(obj2 as XmlNode, true);
						}
						dictionary.Add((K)((object)obj), (V)((object)obj2));
					}
					catch
					{
						string str = "Failed to load key/value pair: ";
						object first = pair.First;
						string str2 = (first != null) ? first.ToString() : null;
						string str3 = ", ";
						object second = pair.Second;
						Log.Error(str + str2 + str3 + ((second != null) ? second.ToString() : null));
					}
				}
			}

			// Token: 0x040027D5 RID: 10197
			private List<XmlNode> wantedDictRefs = new List<XmlNode>();

			// Token: 0x040027D6 RID: 10198
			private object debugWanterInfo;

			// Token: 0x040027D7 RID: 10199
			private List<Pair<object, object>> makingData = new List<Pair<object, object>>();
		}
	}
}
