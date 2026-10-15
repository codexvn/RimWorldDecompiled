using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml;
using RimWorld.IO;
using UnityEngine;

namespace Verse;

public static class DirectXmlLoader
{
	[CompilerGenerated]
	private sealed class _003CAllGameItemsFromAsset_003Ed__7<T> : IEnumerable<T>, IEnumerable, IEnumerator<T>, IEnumerator, IDisposable where T : new()
	{
		private int _003C_003E1__state;

		private T _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private LoadableXmlAsset asset;

		public LoadableXmlAsset _003C_003E3__asset;

		private bool _003CgotData_003E5__2;

		private IEnumerator _003C_003E7__wrap2;

		T IEnumerator<T>.Current
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
		public _003CAllGameItemsFromAsset_003Ed__7(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					if (asset.xmlDoc == null)
					{
						return false;
					}
					XmlNodeList xmlNodeList = asset.xmlDoc.DocumentElement.SelectNodes(typeof(T).Name);
					_003CgotData_003E5__2 = false;
					_003C_003E7__wrap2 = xmlNodeList.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				}
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap2.MoveNext())
				{
					XmlNode xmlNode = (XmlNode)_003C_003E7__wrap2.Current;
					XmlAttribute xmlAttribute = xmlNode.Attributes["Abstract"];
					if (xmlAttribute != null && xmlAttribute.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
					{
						continue;
					}
					DeepProfiler.Start("DirectXmlToObject.ObjectFromXml<" + typeof(T).Name + ">");
					T val;
					try
					{
						val = DirectXmlToObject.ObjectFromXml<T>(xmlNode, doPostLoad: true);
						_003CgotData_003E5__2 = true;
					}
					catch (Exception ex)
					{
						Log.Error("Exception loading data from file " + asset.name + ": " + ex);
						continue;
					}
					finally
					{
						DeepProfiler.End();
					}
					_003C_003E2__current = val;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap2 = null;
				if (!_003CgotData_003E5__2)
				{
					Log.Error("Found no usable data when trying to get " + typeof(T)?.ToString() + "s from file " + asset.name);
				}
				return false;
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
			if (_003C_003E7__wrap2 is IDisposable disposable)
			{
				disposable.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			_003CAllGameItemsFromAsset_003Ed__7<T> _003CAllGameItemsFromAsset_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CAllGameItemsFromAsset_003Ed__ = this;
			}
			else
			{
				_003CAllGameItemsFromAsset_003Ed__ = new _003CAllGameItemsFromAsset_003Ed__7<T>(0);
			}
			_003CAllGameItemsFromAsset_003Ed__.asset = _003C_003E3__asset;
			return _003CAllGameItemsFromAsset_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CLoadXmlDataInResourcesFolder_003Ed__2<T> : IEnumerable<T>, IEnumerable, IEnumerator<T>, IEnumerator, IDisposable where T : new()
	{
		private int _003C_003E1__state;

		private T _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private string folderPath;

		public string _003C_003E3__folderPath;

		private List<LoadableXmlAsset> _003Cassets_003E5__2;

		private int _003Ci_003E5__3;

		private IEnumerator<T> _003C_003E7__wrap3;

		T IEnumerator<T>.Current
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
		public _003CLoadXmlDataInResourcesFolder_003Ed__2(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003Cassets_003E5__2 = null;
			_003C_003E7__wrap3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -3;
					goto IL_015a;
				}
				_003C_003E1__state = -1;
				XmlInheritance.Clear();
				DeepProfiler.Start("Resources.LoadAll<TextAsset>");
				TextAsset[] source = Resources.LoadAll<TextAsset>(folderPath);
				DeepProfiler.End();
				DeepProfiler.Start("Load XML");
				_003Cassets_003E5__2 = (from x in source.Select((TextAsset x) => new { x.name, x.text }).ToList().AsParallel()
					select new LoadableXmlAsset(x.name, x.text)).ToList();
				DeepProfiler.End();
				DeepProfiler.Start("Resolve inheritance");
				foreach (LoadableXmlAsset item in _003Cassets_003E5__2)
				{
					XmlInheritance.TryRegisterAllFrom(item, null);
				}
				XmlInheritance.Resolve();
				DeepProfiler.End();
				DeepProfiler.Start("Read game items from XML");
				_003Ci_003E5__3 = 0;
				goto IL_0186;
				IL_015a:
				if (_003C_003E7__wrap3.MoveNext())
				{
					T current = _003C_003E7__wrap3.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap3 = null;
				_003Ci_003E5__3++;
				goto IL_0186;
				IL_0186:
				if (_003Ci_003E5__3 < _003Cassets_003E5__2.Count)
				{
					_003C_003E7__wrap3 = AllGameItemsFromAsset<T>(_003Cassets_003E5__2[_003Ci_003E5__3]).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_015a;
				}
				DeepProfiler.End();
				XmlInheritance.Clear();
				return false;
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
			if (_003C_003E7__wrap3 != null)
			{
				_003C_003E7__wrap3.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			_003CLoadXmlDataInResourcesFolder_003Ed__2<T> _003CLoadXmlDataInResourcesFolder_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CLoadXmlDataInResourcesFolder_003Ed__ = this;
			}
			else
			{
				_003CLoadXmlDataInResourcesFolder_003Ed__ = new _003CLoadXmlDataInResourcesFolder_003Ed__2<T>(0);
			}
			_003CLoadXmlDataInResourcesFolder_003Ed__.folderPath = _003C_003E3__folderPath;
			return _003CLoadXmlDataInResourcesFolder_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}
	}

	private static readonly LoadableXmlAsset[] EmptyXmlAssetsArray = Array.Empty<LoadableXmlAsset>();

	public static LoadableXmlAsset[] XmlAssetsInModFolder(ModContentPack mod, string folderPath, List<string> foldersToLoadDebug = null)
	{
		List<string> list = foldersToLoadDebug ?? mod.foldersToLoadDescendingOrder;
		Dictionary<string, FileInfo> dictionary = new Dictionary<string, FileInfo>();
		for (int i = 0; i < list.Count; i++)
		{
			string text = list[i];
			DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(text, folderPath));
			if (!directoryInfo.Exists)
			{
				continue;
			}
			FileInfo[] files = directoryInfo.GetFiles("*.xml", SearchOption.AllDirectories);
			foreach (FileInfo fileInfo in files)
			{
				if (!fileInfo.Name.StartsWith("._") && !fileInfo.Name.StartsWith("."))
				{
					string key = fileInfo.FullName.Substring(text.Length + 1);
					dictionary.TryAdd(key, fileInfo);
				}
			}
		}
		if (dictionary.Count == 0)
		{
			return EmptyXmlAssetsArray;
		}
		List<FileInfo> list2 = dictionary.Values.ToList();
		LoadableXmlAsset[] assets = new LoadableXmlAsset[list2.Count];
		ConcurrentBag<KeyValuePair<int, FileInfo>> toLoad = new ConcurrentBag<KeyValuePair<int, FileInfo>>();
		for (int k = 0; k < list2.Count; k++)
		{
			int key2 = k;
			FileInfo value = list2[k];
			toLoad.Add(new KeyValuePair<int, FileInfo>(key2, value));
		}
		Thread[] array = new Thread[2];
		for (int l = 0; l < array.Length; l++)
		{
			array[l] = new Thread((ThreadStart)delegate
			{
				KeyValuePair<int, FileInfo> result;
				while (toLoad.TryTake(out result))
				{
					int key3 = result.Key;
					FileInfo value2 = result.Value;
					assets[key3] = new LoadableXmlAsset(value2, mod);
				}
			})
			{
				Name = $"DirectXmlLoader Thread {l + 1} of {2}"
			};
			array[l].Start();
		}
		KeyValuePair<int, FileInfo> result2;
		while (toLoad.TryTake(out result2))
		{
			int key4 = result2.Key;
			FileInfo value3 = result2.Value;
			assets[key4] = new LoadableXmlAsset(value3, mod);
		}
		Thread[] array2 = array;
		for (int m = 0; m < array2.Length; m++)
		{
			array2[m].Join();
		}
		return assets;
	}

	[IteratorStateMachine(typeof(_003CLoadXmlDataInResourcesFolder_003Ed__2<>))]
	public static IEnumerable<T> LoadXmlDataInResourcesFolder<T>(string folderPath) where T : new()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CLoadXmlDataInResourcesFolder_003Ed__2<T>(-2)
		{
			_003C_003E3__folderPath = folderPath
		};
	}

	public static T ItemFromXmlFile<T>(string filePath, bool resolveCrossRefs = true) where T : new()
	{
		if (!new FileInfo(filePath).Exists)
		{
			return new T();
		}
		return ItemFromXmlString<T>(File.ReadAllText(filePath), filePath, resolveCrossRefs);
	}

	public static T ItemFromXmlFile<T>(VirtualDirectory directory, string filePath, bool resolveCrossRefs = true) where T : new()
	{
		if (!directory.FileExists(filePath))
		{
			return new T();
		}
		return ItemFromXmlString<T>(directory.ReadAllText(filePath), directory.FullPath + "/" + filePath, resolveCrossRefs);
	}

	public static T ItemFromXmlString<T>(string xmlContent, string filePath, bool resolveCrossRefs = true) where T : new()
	{
		if (resolveCrossRefs && DirectXmlCrossRefLoader.LoadingInProgress)
		{
			Log.Error("Cannot call ItemFromXmlString with resolveCrossRefs=true while loading is already in progress (forgot to resolve or clear cross refs from previous loading?).");
		}
		try
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xmlContent);
			T result = DirectXmlToObject.ObjectFromXml<T>(xmlDocument.DocumentElement, doPostLoad: false);
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
			return result;
		}
		catch (Exception ex)
		{
			Log.Error("Exception loading file at " + filePath + ". Loading defaults instead. Exception was: " + ex.ToString());
			return new T();
		}
	}

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
		Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(node.Name);
		if (typeInAnyAssembly == null || !GenTypes.IsDef(typeInAnyAssembly))
		{
			Log.ErrorOnce($"Type {text} is not a Def type or could not be found, in file {((loadingAsset != null) ? loadingAsset.name : "(unknown)")}. Context: {node.OuterXml}", text.GetHashCode());
			return null;
		}
		Func<XmlNode, bool, object> objectFromXmlMethod = DirectXmlToObject.GetObjectFromXmlMethod(typeInAnyAssembly);
		Def def = null;
		try
		{
			def = (Def)objectFromXmlMethod(node, arg2: true);
			def.ResolveDefNameHash();
		}
		catch (Exception ex)
		{
			Log.Error("Exception loading def from file " + ((loadingAsset != null) ? loadingAsset.name : "(unknown)") + ": " + ex);
		}
		return def;
	}

	[IteratorStateMachine(typeof(_003CAllGameItemsFromAsset_003Ed__7<>))]
	public static IEnumerable<T> AllGameItemsFromAsset<T>(LoadableXmlAsset asset) where T : new()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllGameItemsFromAsset_003Ed__7<T>(-2)
		{
			_003C_003E3__asset = asset
		};
	}
}
