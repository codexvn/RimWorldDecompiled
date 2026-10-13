using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Verse
{
	// Token: 0x020005A3 RID: 1443
	public static class TKeySystem
	{
		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x0600256E RID: 9582 RVA: 0x000028E7 File Offset: 0x00000AE7
		private static bool ShouldUseHardcodedMapping
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600256F RID: 9583 RVA: 0x000C12D5 File Offset: 0x000BF4D5
		public static void Clear()
		{
			TKeySystem.keys.Clear();
			TKeySystem.tKeyToNormalizedTranslationKey.Clear();
			TKeySystem.translationKeyToTKey.Clear();
			TKeySystem.loadErrors.Clear();
			TKeySystem.treatAsList.Clear();
		}

		// Token: 0x06002570 RID: 9584 RVA: 0x000C130C File Offset: 0x000BF50C
		public static void Parse(XmlDocument document)
		{
			foreach (object obj in document.ChildNodes[0].ChildNodes)
			{
				TKeySystem.ParseDefNode((XmlNode)obj);
			}
		}

		// Token: 0x06002571 RID: 9585 RVA: 0x000C1370 File Offset: 0x000BF570
		public static void MarkTreatAsList(XmlNode node)
		{
			TKeySystem.treatAsList.Add(node);
		}

		// Token: 0x06002572 RID: 9586 RVA: 0x000C137E File Offset: 0x000BF57E
		public static void BuildMappings()
		{
			if (TKeySystem.ShouldUseHardcodedMapping)
			{
				TKeySystem.tKeyToNormalizedTranslationKey.AddRange(TKeySystemHardcodedMapping.TKeyToNormalizedTranslationKey);
				TKeySystem.translationKeyToTKey.AddRange(TKeySystemHardcodedMapping.TranslationKeyToTKey);
				return;
			}
			TKeySystem.BuildMappings_Calculate();
		}

		// Token: 0x06002573 RID: 9587 RVA: 0x000C13AC File Offset: 0x000BF5AC
		private static void BuildMappings_Calculate()
		{
			Dictionary<string, string> tmpTranslationKeyToTKey = new Dictionary<string, string>();
			foreach (TKeySystem.TKeyRef tkeyRef in TKeySystem.keys)
			{
				string normalizedTranslationKey = TKeySystem.GetNormalizedTranslationKey(tkeyRef);
				string text;
				if (TKeySystem.tKeyToNormalizedTranslationKey.TryGetValue(tkeyRef.tKeyPath, out text))
				{
					TKeySystem.loadErrors.Add(string.Concat(new string[]
					{
						"Duplicate TKey: ",
						tkeyRef.tKeyPath,
						" -> NEW=",
						normalizedTranslationKey,
						" | OLD",
						text,
						" - Ignoring old"
					}));
				}
				else
				{
					TKeySystem.tKeyToNormalizedTranslationKey.Add(tkeyRef.tKeyPath, normalizedTranslationKey);
					tmpTranslationKeyToTKey.Add(normalizedTranslationKey, tkeyRef.tKeyPath);
				}
			}
			DefInjectionUtility.PossibleDefInjectionTraverser <>9__2;
			foreach (string typeName in (from k in TKeySystem.keys
			select k.defTypeName).Distinct<string>())
			{
				Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(typeName, null);
				DefInjectionUtility.PossibleDefInjectionTraverser action;
				if ((action = <>9__2) == null)
				{
					action = (<>9__2 = delegate(string suggestedPath, string normalizedPath, bool isCollection, string currentValue, IEnumerable<string> currentValueCollection, bool translationAllowed, bool fullListTranslationAllowed, FieldInfo fieldInfo, Def def)
					{
						string text2;
						string text3;
						if (translationAllowed && !TKeySystem.TryGetNormalizedPath(suggestedPath, out text2) && TKeySystem.TrySuggestTKeyPath(normalizedPath, out text3, tmpTranslationKeyToTKey))
						{
							if (tmpTranslationKeyToTKey.ContainsKey(suggestedPath) && tmpTranslationKeyToTKey[suggestedPath] != text3)
							{
								Log.Error(string.Concat(new string[]
								{
									"Trying to add duplicate TKey with different path. '",
									tmpTranslationKeyToTKey[suggestedPath],
									"' will be overwritten by '",
									text3,
									"'."
								}));
							}
							tmpTranslationKeyToTKey.SetOrAdd(suggestedPath, text3);
						}
					});
				}
				DefInjectionUtility.ForEachPossibleDefInjection(typeInAnyAssembly, action, null);
			}
			foreach (KeyValuePair<string, string> keyValuePair in tmpTranslationKeyToTKey)
			{
				TKeySystem.translationKeyToTKey.Add(keyValuePair.Key, keyValuePair.Value);
			}
			TKeySystem.<BuildMappings_Calculate>g__Compare|14_0(TKeySystem.tKeyToNormalizedTranslationKey, TKeySystemHardcodedMapping.TKeyToNormalizedTranslationKey, "tKeyToNormalizedTranslationKey(calculated)", "TKeySystemHardcodedMapping.TKeyToNormalizedTranslationKey");
			TKeySystem.<BuildMappings_Calculate>g__Compare|14_0(TKeySystem.translationKeyToTKey, TKeySystemHardcodedMapping.TranslationKeyToTKey, "translationKeyToTKey(calculated)", "TKeySystemHardcodedMapping.TranslationKeyToTKey");
		}

		// Token: 0x06002574 RID: 9588 RVA: 0x000C15AC File Offset: 0x000BF7AC
		public static bool TryGetNormalizedPath(string tKeyPath, out string normalizedPath)
		{
			return TKeySystem.TryFindShortestReplacementPath(tKeyPath, delegate(string path, out string result)
			{
				return TKeySystem.tKeyToNormalizedTranslationKey.TryGetValue(path, out result);
			}, out normalizedPath);
		}

		// Token: 0x06002575 RID: 9589 RVA: 0x000C15D4 File Offset: 0x000BF7D4
		private static bool TryFindShortestReplacementPath(string path, TKeySystem.PathMatcher matcher, out string result)
		{
			if (matcher(path, out result))
			{
				return true;
			}
			int num = 100;
			int num2 = path.Length - 1;
			for (;;)
			{
				if (num2 > 0 && path[num2] != '.')
				{
					num2--;
				}
				else
				{
					if (path[num2] == '.')
					{
						string path2 = path.Substring(0, num2);
						if (matcher(path2, out result))
						{
							break;
						}
					}
					num2--;
					num--;
					if (num2 <= 0 || num <= 0)
					{
						goto IL_6D;
					}
				}
			}
			result += path.Substring(num2);
			return true;
			IL_6D:
			result = null;
			return false;
		}

		// Token: 0x06002576 RID: 9590 RVA: 0x000C1654 File Offset: 0x000BF854
		public static bool TrySuggestTKeyPath(string translationPath, out string tKeyPath, Dictionary<string, string> lookup = null)
		{
			if (lookup == null)
			{
				lookup = TKeySystem.translationKeyToTKey;
			}
			return TKeySystem.TryFindShortestReplacementPath(translationPath, delegate(string path, out string result)
			{
				return lookup.TryGetValue(path, out result);
			}, out tKeyPath);
		}

		// Token: 0x06002577 RID: 9591 RVA: 0x000C1694 File Offset: 0x000BF894
		private static string GetNormalizedTranslationKey(TKeySystem.TKeyRef tKeyRef)
		{
			string text = "";
			XmlNode currentNode;
			for (currentNode = tKeyRef.node; currentNode != tKeyRef.defRootNode; currentNode = currentNode.ParentNode)
			{
				if (currentNode.Name == "li" || TKeySystem.treatAsList.Contains(currentNode.ParentNode))
				{
					text = "." + (from XmlNode n in currentNode.ParentNode.ChildNodes
					where TKeySystem.<GetNormalizedTranslationKey>g__ShouldConsiderNode|19_0(n)
					select n).FirstIndexOf((XmlNode n) => n == currentNode).ToString() + text;
				}
				else
				{
					text = "." + currentNode.Name + text;
				}
			}
			return tKeyRef.defName + text;
		}

		// Token: 0x06002578 RID: 9592 RVA: 0x000C1794 File Offset: 0x000BF994
		private static void ParseDefNode(XmlNode node)
		{
			if (TKeySystem.ShouldUseHardcodedMapping)
			{
				return;
			}
			string text = null;
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Name == "defName")
				{
					text = xmlNode.InnerText;
					break;
				}
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				return;
			}
			TKeySystem.<>c__DisplayClass20_0 CS$<>8__locals1;
			CS$<>8__locals1.tKeyRefTemplate = default(TKeySystem.TKeyRef);
			CS$<>8__locals1.tKeyRefTemplate.defName = text;
			CS$<>8__locals1.tKeyRefTemplate.defTypeName = node.Name;
			CS$<>8__locals1.tKeyRefTemplate.defRootNode = node;
			TKeySystem.<ParseDefNode>g__CrawlNodesRecursive|20_1(node, ref CS$<>8__locals1);
		}

		// Token: 0x0600257A RID: 9594 RVA: 0x000C188C File Offset: 0x000BFA8C
		[CompilerGenerated]
		internal static void <BuildMappings_Calculate>g__Compare|14_0(Dictionary<string, string> a, Dictionary<string, string> b, string aName, string bName)
		{
			foreach (KeyValuePair<string, string> keyValuePair in a)
			{
				string b2;
				if (!b.TryGetValue(keyValuePair.Key, out b2))
				{
					Log.Warning(bName + " is missing " + keyValuePair.Key);
				}
				else if (keyValuePair.Value != b2)
				{
					Log.Warning(string.Concat(new string[]
					{
						bName,
						"'s value doesn't match for ",
						keyValuePair.Key,
						". Should be ",
						keyValuePair.Value
					}));
				}
			}
			foreach (KeyValuePair<string, string> keyValuePair2 in b)
			{
				if (!a.ContainsKey(keyValuePair2.Key))
				{
					Log.Warning(string.Concat(new string[]
					{
						bName,
						" has ",
						keyValuePair2.Key,
						" but it's not present in ",
						aName,
						". We should most likely remove it from the list."
					}));
				}
			}
		}

		// Token: 0x0600257B RID: 9595 RVA: 0x000C19C8 File Offset: 0x000BFBC8
		[CompilerGenerated]
		internal static bool <GetNormalizedTranslationKey>g__ShouldConsiderNode|19_0(XmlNode node)
		{
			XmlAttribute xmlAttribute = node.Attributes["MayRequire"];
			string text = (xmlAttribute != null) ? xmlAttribute.Value : null;
			if (text != null && !ModsConfig.AreAllActive(text))
			{
				return false;
			}
			XmlAttributeCollection attributes = node.Attributes;
			string[] array;
			if (attributes == null)
			{
				array = null;
			}
			else
			{
				XmlAttribute xmlAttribute2 = attributes["MayRequire"];
				array = ((xmlAttribute2 != null) ? xmlAttribute2.Value.ToLower().Split(',', StringSplitOptions.None) : null);
			}
			string[] array2 = array;
			return array2.NullOrEmpty<string>() || ModsConfig.IsAnyActiveOrEmpty(array2, false);
		}

		// Token: 0x0600257C RID: 9596 RVA: 0x000C1A44 File Offset: 0x000BFC44
		[CompilerGenerated]
		internal static void <ParseDefNode>g__ProcessNode|20_0(XmlNode n, ref TKeySystem.<>c__DisplayClass20_0 A_1)
		{
			XmlAttribute xmlAttribute;
			if (n.Attributes != null && (xmlAttribute = n.Attributes["TKey"]) != null)
			{
				TKeySystem.TKeyRef tKeyRefTemplate = A_1.tKeyRefTemplate;
				tKeyRefTemplate.tKey = xmlAttribute.Value;
				tKeyRefTemplate.node = n;
				tKeyRefTemplate.tKeyPath = tKeyRefTemplate.defName + "." + tKeyRefTemplate.tKey;
				TKeySystem.keys.Add(tKeyRefTemplate);
			}
		}

		// Token: 0x0600257D RID: 9597 RVA: 0x000C1AB4 File Offset: 0x000BFCB4
		[CompilerGenerated]
		internal static void <ParseDefNode>g__CrawlNodesRecursive|20_1(XmlNode n, ref TKeySystem.<>c__DisplayClass20_0 A_1)
		{
			TKeySystem.<ParseDefNode>g__ProcessNode|20_0(n, ref A_1);
			foreach (object obj in n.ChildNodes)
			{
				TKeySystem.<ParseDefNode>g__CrawlNodesRecursive|20_1((XmlNode)obj, ref A_1);
			}
		}

		// Token: 0x04001EB7 RID: 7863
		private static List<TKeySystem.TKeyRef> keys = new List<TKeySystem.TKeyRef>();

		// Token: 0x04001EB8 RID: 7864
		public static List<string> loadErrors = new List<string>();

		// Token: 0x04001EB9 RID: 7865
		private static HashSet<XmlNode> treatAsList = new HashSet<XmlNode>();

		// Token: 0x04001EBA RID: 7866
		private static Dictionary<string, string> tKeyToNormalizedTranslationKey = new Dictionary<string, string>();

		// Token: 0x04001EBB RID: 7867
		private static Dictionary<string, string> translationKeyToTKey = new Dictionary<string, string>();

		// Token: 0x04001EBC RID: 7868
		public const string AttributeName = "TKey";

		// Token: 0x020005A4 RID: 1444
		private struct TKeyRef
		{
			// Token: 0x04001EBD RID: 7869
			public string defName;

			// Token: 0x04001EBE RID: 7870
			public string defTypeName;

			// Token: 0x04001EBF RID: 7871
			public XmlNode defRootNode;

			// Token: 0x04001EC0 RID: 7872
			public XmlNode node;

			// Token: 0x04001EC1 RID: 7873
			public string tKey;

			// Token: 0x04001EC2 RID: 7874
			public string tKeyPath;
		}

		// Token: 0x020005A5 RID: 1445
		private struct PossibleDefInjection
		{
			// Token: 0x04001EC3 RID: 7875
			public string normalizedPath;

			// Token: 0x04001EC4 RID: 7876
			public string path;
		}

		// Token: 0x020005A6 RID: 1446
		// (Invoke) Token: 0x0600257F RID: 9599
		private delegate bool PathMatcher(string path, out string match);
	}
}
