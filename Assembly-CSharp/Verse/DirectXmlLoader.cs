using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;
using RimWorld.IO;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000822 RID: 2082
	public static class DirectXmlLoader
	{
		// Token: 0x060034E6 RID: 13542 RVA: 0x00116110 File Offset: 0x00114310
		public static LoadableXmlAsset[] XmlAssetsInModFolder(ModContentPack mod, string folderPath, List<string> foldersToLoadDebug = null)
		{
			List<string> list = foldersToLoadDebug ?? mod.foldersToLoadDescendingOrder;
			Dictionary<string, FileInfo> dictionary = new Dictionary<string, FileInfo>();
			for (int i = 0; i < list.Count; i++)
			{
				string text = list[i];
				DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(text, folderPath));
				if (directoryInfo.Exists)
				{
					foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.xml", SearchOption.AllDirectories))
					{
						string key = fileInfo.FullName.Substring(text.Length + 1);
						dictionary.TryAdd(key, fileInfo);
					}
				}
			}
			if (dictionary.Count == 0)
			{
				return DirectXmlLoader.EmptyXmlAssetsArray;
			}
			List<FileInfo> list2 = dictionary.Values.ToList<FileInfo>();
			LoadableXmlAsset[] assets = new LoadableXmlAsset[list2.Count];
			ConcurrentBag<KeyValuePair<int, FileInfo>> toLoad = new ConcurrentBag<KeyValuePair<int, FileInfo>>();
			for (int k = 0; k < list2.Count; k++)
			{
				int key2 = k;
				FileInfo value = list2[k];
				toLoad.Add(new KeyValuePair<int, FileInfo>(key2, value));
			}
			Thread[] array = new Thread[2];
			ThreadStart <>9__0;
			for (int l = 0; l < array.Length; l++)
			{
				Thread[] array2 = array;
				int num = l;
				ThreadStart start;
				if ((start = <>9__0) == null)
				{
					start = (<>9__0 = delegate()
					{
						KeyValuePair<int, FileInfo> keyValuePair2;
						while (toLoad.TryTake(out keyValuePair2))
						{
							int key4 = keyValuePair2.Key;
							FileInfo value3 = keyValuePair2.Value;
							assets[key4] = new LoadableXmlAsset(value3, mod);
						}
					});
				}
				array2[num] = new Thread(start)
				{
					Name = string.Format("DirectXmlLoader Thread {0} of {1}", l + 1, 2)
				};
				array[l].Start();
			}
			KeyValuePair<int, FileInfo> keyValuePair;
			while (toLoad.TryTake(out keyValuePair))
			{
				int key3 = keyValuePair.Key;
				FileInfo value2 = keyValuePair.Value;
				assets[key3] = new LoadableXmlAsset(value2, mod);
			}
			Thread[] array3 = array;
			for (int m = 0; m < array3.Length; m++)
			{
				array3[m].Join();
			}
			return assets;
		}

		// Token: 0x060034E7 RID: 13543 RVA: 0x001162FF File Offset: 0x001144FF
		public static IEnumerable<T> LoadXmlDataInResourcesFolder<T>(string folderPath) where T : new()
		{
			XmlInheritance.Clear();
			DeepProfiler.Start("Resources.LoadAll<TextAsset>");
			TextAsset[] source = Resources.LoadAll<TextAsset>(folderPath);
			DeepProfiler.End();
			DeepProfiler.Start("Load XML");
			List<LoadableXmlAsset> assets = (from x in (from x in source
			select new
			{
				x.name,
				x.text
			}).ToList().AsParallel()
			select new LoadableXmlAsset(x.name, x.text)).ToList<LoadableXmlAsset>();
			DeepProfiler.End();
			DeepProfiler.Start("Resolve inheritance");
			foreach (LoadableXmlAsset xmlAsset in assets)
			{
				XmlInheritance.TryRegisterAllFrom(xmlAsset, null);
			}
			XmlInheritance.Resolve();
			DeepProfiler.End();
			DeepProfiler.Start("Read game items from XML");
			int num;
			for (int i = 0; i < assets.Count; i = num + 1)
			{
				foreach (T t in DirectXmlLoader.AllGameItemsFromAsset<T>(assets[i]))
				{
					yield return t;
				}
				IEnumerator<T> enumerator2 = null;
				num = i;
			}
			DeepProfiler.End();
			XmlInheritance.Clear();
			yield break;
			yield break;
		}

		// Token: 0x060034E8 RID: 13544 RVA: 0x0011630F File Offset: 0x0011450F
		public static T ItemFromXmlFile<T>(string filePath, bool resolveCrossRefs = true) where T : new()
		{
			if (!new FileInfo(filePath).Exists)
			{
				return Activator.CreateInstance<T>();
			}
			return DirectXmlLoader.ItemFromXmlString<T>(File.ReadAllText(filePath), filePath, resolveCrossRefs);
		}

		// Token: 0x060034E9 RID: 13545 RVA: 0x00116331 File Offset: 0x00114531
		public static T ItemFromXmlFile<T>(VirtualDirectory directory, string filePath, bool resolveCrossRefs = true) where T : new()
		{
			if (!directory.FileExists(filePath))
			{
				return Activator.CreateInstance<T>();
			}
			return DirectXmlLoader.ItemFromXmlString<T>(directory.ReadAllText(filePath), directory.FullPath + "/" + filePath, resolveCrossRefs);
		}

		// Token: 0x060034EA RID: 13546 RVA: 0x00116360 File Offset: 0x00114560
		public static T ItemFromXmlString<T>(string xmlContent, string filePath, bool resolveCrossRefs = true) where T : new()
		{
			if (resolveCrossRefs && DirectXmlCrossRefLoader.LoadingInProgress)
			{
				Log.Error("Cannot call ItemFromXmlString with resolveCrossRefs=true while loading is already in progress (forgot to resolve or clear cross refs from previous loading?).");
			}
			T result;
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(xmlContent);
				T t = DirectXmlToObject.ObjectFromXml<T>(xmlDocument.DocumentElement, false);
				if (resolveCrossRefs)
				{
					try
					{
						DirectXmlCrossRefLoader.ResolveAllWantedCrossReferences(FailMode.LogErrors);
					}
					finally
					{
						DirectXmlCrossRefLoader.Clear();
					}
				}
				result = t;
			}
			catch (Exception ex)
			{
				Log.Error("Exception loading file at " + filePath + ". Loading defaults instead. Exception was: " + ex.ToString());
				result = Activator.CreateInstance<T>();
			}
			return result;
		}

		// Token: 0x060034EB RID: 13547 RVA: 0x001163F0 File Offset: 0x001145F0
		public static Def DefFromNode(XmlNode node, LoadableXmlAsset loadingAsset)
		{
			if (node.NodeType != XmlNodeType.Element)
			{
				return null;
			}
			XmlAttribute xmlAttribute = node.Attributes["Abstract"];
			if (xmlAttribute != null && xmlAttribute.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
			{
				return null;
			}
			XmlNode resolvedNodeFor = XmlInheritance.GetResolvedNodeFor(node);
			string text = node.Name;
			XmlAttribute xmlAttribute2 = resolvedNodeFor.Attributes["Class"];
			if (xmlAttribute2 != null)
			{
				text = xmlAttribute2.Value;
			}
			Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(node.Name, null);
			if (typeInAnyAssembly == null || !GenTypes.IsDef(typeInAnyAssembly))
			{
				Log.ErrorOnce(string.Concat(new string[]
				{
					"Type ",
					text,
					" is not a Def type or could not be found, in file ",
					(loadingAsset != null) ? loadingAsset.name : "(unknown)",
					". Context: ",
					node.OuterXml
				}), text.GetHashCode());
				return null;
			}
			Func<XmlNode, bool, object> objectFromXmlMethod = DirectXmlToObject.GetObjectFromXmlMethod(typeInAnyAssembly);
			Def def = null;
			try
			{
				def = (Def)objectFromXmlMethod(node, true);
				def.ResolveDefNameHash();
			}
			catch (Exception ex)
			{
				string str = "Exception loading def from file ";
				string str2 = (loadingAsset != null) ? loadingAsset.name : "(unknown)";
				string str3 = ": ";
				Exception ex2 = ex;
				Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
			}
			return def;
		}

		// Token: 0x060034EC RID: 13548 RVA: 0x00116534 File Offset: 0x00114734
		public static IEnumerable<T> AllGameItemsFromAsset<T>(LoadableXmlAsset asset) where T : new()
		{
			if (asset.xmlDoc == null)
			{
				yield break;
			}
			XmlNodeList xmlNodeList = asset.xmlDoc.DocumentElement.SelectNodes(typeof(T).Name);
			bool gotData = false;
			foreach (object obj in xmlNodeList)
			{
				XmlNode xmlNode = (XmlNode)obj;
				XmlAttribute xmlAttribute = xmlNode.Attributes["Abstract"];
				if (xmlAttribute == null || !xmlAttribute.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
				{
					DeepProfiler.Start("DirectXmlToObject.ObjectFromXml<" + typeof(T).Name + ">");
					T t;
					try
					{
						t = DirectXmlToObject.ObjectFromXml<T>(xmlNode, true);
						gotData = true;
					}
					catch (Exception ex)
					{
						string str = "Exception loading data from file ";
						string name = asset.name;
						string str2 = ": ";
						Exception ex2 = ex;
						Log.Error(str + name + str2 + ((ex2 != null) ? ex2.ToString() : null));
						continue;
					}
					finally
					{
						DeepProfiler.End();
					}
					yield return t;
				}
			}
			IEnumerator enumerator = null;
			if (!gotData)
			{
				string str3 = "Found no usable data when trying to get ";
				Type typeFromHandle = typeof(T);
				Log.Error(str3 + ((typeFromHandle != null) ? typeFromHandle.ToString() : null) + "s from file " + asset.name);
			}
			yield break;
			yield break;
		}

		// Token: 0x040027DB RID: 10203
		private static readonly LoadableXmlAsset[] EmptyXmlAssetsArray = Array.Empty<LoadableXmlAsset>();
	}
}
