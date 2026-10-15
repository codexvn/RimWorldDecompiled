using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;
using RimWorld.IO;

namespace Verse;

public static class DirectXmlLoaderSimple
{
	public struct XmlKeyValuePair
	{
		public string key;

		public string value;

		public int lineNumber;
	}

	[CompilerGenerated]
	private sealed class _003CValuesFromXmlFile_003Ed__3 : IEnumerable<XmlKeyValuePair>, IEnumerable, IEnumerator<XmlKeyValuePair>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private XmlKeyValuePair _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private XDocument doc;

		public XDocument _003C_003E3__doc;

		private IEnumerator<XElement> _003C_003E7__wrap1;

		XmlKeyValuePair IEnumerator<XmlKeyValuePair>.Current
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
		public _003CValuesFromXmlFile_003Ed__3(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = null;
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
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = doc.Root.Elements().GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					XElement current = _003C_003E7__wrap1.Current;
					string key = current.Name.ToString();
					string value = current.Value;
					value = value.Replace("\\n", "\n");
					XmlKeyValuePair xmlKeyValuePair = default(XmlKeyValuePair);
					xmlKeyValuePair.key = key;
					xmlKeyValuePair.value = value;
					xmlKeyValuePair.lineNumber = ((IXmlLineInfo)current).LineNumber;
					_003C_003E2__current = xmlKeyValuePair;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
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
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<XmlKeyValuePair> IEnumerable<XmlKeyValuePair>.GetEnumerator()
		{
			_003CValuesFromXmlFile_003Ed__3 _003CValuesFromXmlFile_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CValuesFromXmlFile_003Ed__ = this;
			}
			else
			{
				_003CValuesFromXmlFile_003Ed__ = new _003CValuesFromXmlFile_003Ed__3(0);
			}
			_003CValuesFromXmlFile_003Ed__.doc = _003C_003E3__doc;
			return _003CValuesFromXmlFile_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<XmlKeyValuePair>)this).GetEnumerator();
		}
	}

	public static IEnumerable<XmlKeyValuePair> ValuesFromXmlFile(string fileContents)
	{
		return ValuesFromXmlFile(VirtualFileInfoExt.LoadAsXDocument(fileContents));
	}

	public static IEnumerable<XmlKeyValuePair> ValuesFromXmlFile(VirtualFile file)
	{
		return ValuesFromXmlFile(file.LoadAsXDocument());
	}

	[IteratorStateMachine(typeof(_003CValuesFromXmlFile_003Ed__3))]
	public static IEnumerable<XmlKeyValuePair> ValuesFromXmlFile(XDocument doc)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CValuesFromXmlFile_003Ed__3(-2)
		{
			_003C_003E3__doc = doc
		};
	}
}
