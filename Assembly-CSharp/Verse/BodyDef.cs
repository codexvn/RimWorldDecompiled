using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse;

public class BodyDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__14 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public BodyDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

		private List<BodyPartRecord>.Enumerator _003C_003E7__wrap2;

		string IEnumerator<string>.Current
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
		public _003CConfigErrors_003Ed__14(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 3:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = default(List<BodyPartRecord>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				BodyDef bodyDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)bodyDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0076;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0076;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00bd;
				case 3:
					{
						_003C_003E1__state = -4;
						break;
					}
					IL_0076:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (bodyDef.cachedPartsVulnerableToFrostbite.NullOrEmpty())
					{
						_003C_003E2__current = "no parts vulnerable to frostbite";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00bd;
					IL_00bd:
					_003C_003E7__wrap2 = bodyDef.AllParts.GetEnumerator();
					_003C_003E1__state = -4;
					break;
				}
				while (_003C_003E7__wrap2.MoveNext())
				{
					BodyPartRecord current2 = _003C_003E7__wrap2.Current;
					if (current2.def.conceptual && current2.coverageAbs != 0f)
					{
						_003C_003E2__current = $"part {current2} is tagged conceptual, but has nonzero coverage";
						_003C_003E1__state = 3;
						return true;
					}
					if (!Prefs.DevMode || current2.def.conceptual)
					{
						continue;
					}
					float num2 = 0f;
					foreach (BodyPartRecord part in current2.parts)
					{
						num2 += part.coverage;
					}
					if (num2 >= 1f)
					{
						Log.Warning("BodyDef " + bodyDef.defName + " has BodyPartRecord of " + current2.def.defName + " whose children have more or equal coverage than 100% (" + (num2 * 100f).ToString("0.00") + "%)");
					}
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap2 = default(List<BodyPartRecord>.Enumerator);
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

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__14 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__14(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public BodyPartRecord corePart;

	[Unsaved(false)]
	private List<BodyPartRecord> cachedAllParts = new List<BodyPartRecord>();

	[Unsaved(false)]
	private List<BodyPartRecord> cachedPartsVulnerableToFrostbite;

	[Unsaved(false)]
	public Dictionary<BodyPartTagDef, List<BodyPartRecord>> cachedPartsByTag = new Dictionary<BodyPartTagDef, List<BodyPartRecord>>();

	[Unsaved(false)]
	public Dictionary<BodyPartDef, List<BodyPartRecord>> cachedPartsByDef = new Dictionary<BodyPartDef, List<BodyPartRecord>>();

	public List<BodyPartRecord> AllParts => cachedAllParts;

	public List<BodyPartRecord> AllPartsVulnerableToFrostbite => cachedPartsVulnerableToFrostbite;

	public List<BodyPartRecord> GetPartsWithTag(BodyPartTagDef tag)
	{
		if (!cachedPartsByTag.ContainsKey(tag))
		{
			cachedPartsByTag[tag] = new List<BodyPartRecord>();
			for (int i = 0; i < AllParts.Count; i++)
			{
				BodyPartRecord bodyPartRecord = AllParts[i];
				if (bodyPartRecord.def.tags.Contains(tag))
				{
					cachedPartsByTag[tag].Add(bodyPartRecord);
				}
			}
		}
		return cachedPartsByTag[tag];
	}

	public List<BodyPartRecord> GetPartsWithDef(BodyPartDef def)
	{
		if (!cachedPartsByDef.ContainsKey(def))
		{
			cachedPartsByDef[def] = new List<BodyPartRecord>();
			for (int i = 0; i < AllParts.Count; i++)
			{
				BodyPartRecord bodyPartRecord = AllParts[i];
				if (bodyPartRecord.def == def)
				{
					cachedPartsByDef[def].Add(bodyPartRecord);
				}
			}
		}
		return cachedPartsByDef[def];
	}

	public bool HasPartWithTag(BodyPartTagDef tag)
	{
		for (int i = 0; i < AllParts.Count; i++)
		{
			if (AllParts[i].def.tags.Contains(tag))
			{
				return true;
			}
		}
		return false;
	}

	public BodyPartRecord GetPartAtIndex(int index)
	{
		if (index < 0 || index >= cachedAllParts.Count)
		{
			return null;
		}
		return cachedAllParts[index];
	}

	public int GetIndexOfPart(BodyPartRecord rec)
	{
		for (int i = 0; i < cachedAllParts.Count; i++)
		{
			if (cachedAllParts[i] == rec)
			{
				return i;
			}
		}
		return -1;
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__14))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__14(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ResolveReferences()
	{
		base.ResolveReferences();
		if (corePart != null)
		{
			CacheDataRecursive(corePart);
		}
		cachedPartsVulnerableToFrostbite = new List<BodyPartRecord>();
		List<BodyPartRecord> allParts = AllParts;
		for (int i = 0; i < allParts.Count; i++)
		{
			if (allParts[i].def.frostbiteVulnerability > 0f)
			{
				cachedPartsVulnerableToFrostbite.Add(allParts[i]);
			}
		}
	}

	private void CacheDataRecursive(BodyPartRecord node)
	{
		if (node.def == null)
		{
			Log.Error("BodyPartRecord with null def. body=" + this);
			return;
		}
		node.body = this;
		for (int i = 0; i < node.parts.Count; i++)
		{
			node.parts[i].parent = node;
		}
		if (node.parent != null)
		{
			node.coverageAbsWithChildren = node.parent.coverageAbsWithChildren * node.coverage;
		}
		else
		{
			node.coverageAbsWithChildren = 1f;
		}
		float num = 1f;
		for (int j = 0; j < node.parts.Count; j++)
		{
			num -= node.parts[j].coverage;
		}
		if (Mathf.Abs(num) < 1E-05f)
		{
			num = 0f;
		}
		if (num <= 0f)
		{
			num = 0f;
		}
		node.coverageAbs = node.coverageAbsWithChildren * num;
		if (node.height == BodyPartHeight.Undefined)
		{
			node.height = BodyPartHeight.Middle;
		}
		if (node.depth == BodyPartDepth.Undefined)
		{
			node.depth = BodyPartDepth.Outside;
		}
		for (int k = 0; k < node.parts.Count; k++)
		{
			if (node.parts[k].height == BodyPartHeight.Undefined)
			{
				node.parts[k].height = node.height;
			}
			if (node.parts[k].depth == BodyPartDepth.Undefined)
			{
				node.parts[k].depth = node.depth;
			}
		}
		cachedAllParts.Add(node);
		for (int l = 0; l < node.parts.Count; l++)
		{
			CacheDataRecursive(node.parts[l]);
		}
	}

	public override void ClearCachedData()
	{
		base.ClearCachedData();
		cachedPartsByTag.Clear();
		cachedPartsByDef.Clear();
	}
}
