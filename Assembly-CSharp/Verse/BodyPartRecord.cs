using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Verse;

public class BodyPartRecord
{
	[CompilerGenerated]
	private sealed class _003CGetChildParts_003Ed__31 : IEnumerable<BodyPartRecord>, IEnumerable, IEnumerator<BodyPartRecord>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private BodyPartRecord _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public BodyPartRecord _003C_003E4__this;

		private BodyPartTagDef tag;

		public BodyPartTagDef _003C_003E3__tag;

		private int _003Ci_003E5__2;

		private IEnumerator<BodyPartRecord> _003C_003E7__wrap2;

		BodyPartRecord IEnumerator<BodyPartRecord>.Current
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
		public _003CGetChildParts_003Ed__31(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 2)
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
				int num = _003C_003E1__state;
				BodyPartRecord bodyPartRecord = _003C_003E4__this;
				int num2;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (bodyPartRecord.def.tags.Contains(tag))
					{
						_003C_003E2__current = bodyPartRecord;
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_0062;
				case 1:
					_003C_003E1__state = -1;
					goto IL_0062;
				case 2:
					{
						_003C_003E1__state = -3;
						goto IL_00c5;
					}
					IL_0062:
					_003Ci_003E5__2 = 0;
					goto IL_00f1;
					IL_00f1:
					if (_003Ci_003E5__2 < bodyPartRecord.parts.Count)
					{
						_003C_003E7__wrap2 = bodyPartRecord.parts[_003Ci_003E5__2].GetChildParts(tag).GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_00c5;
					}
					return false;
					IL_00c5:
					if (_003C_003E7__wrap2.MoveNext())
					{
						BodyPartRecord current = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = null;
					num2 = _003Ci_003E5__2 + 1;
					_003Ci_003E5__2 = num2;
					goto IL_00f1;
				}
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
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<BodyPartRecord> IEnumerable<BodyPartRecord>.GetEnumerator()
		{
			_003CGetChildParts_003Ed__31 _003CGetChildParts_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetChildParts_003Ed__ = this;
			}
			else
			{
				_003CGetChildParts_003Ed__ = new _003CGetChildParts_003Ed__31(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetChildParts_003Ed__.tag = _003C_003E3__tag;
			return _003CGetChildParts_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<BodyPartRecord>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetConnectedParts_003Ed__35 : IEnumerable<BodyPartRecord>, IEnumerable, IEnumerator<BodyPartRecord>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private BodyPartRecord _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public BodyPartRecord _003C_003E4__this;

		private BodyPartTagDef tag;

		public BodyPartTagDef _003C_003E3__tag;

		private IEnumerator<BodyPartRecord> _003C_003E7__wrap1;

		BodyPartRecord IEnumerator<BodyPartRecord>.Current
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
		public _003CGetConnectedParts_003Ed__35(int _003C_003E1__state)
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
				int num = _003C_003E1__state;
				BodyPartRecord bodyPartRecord = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					BodyPartRecord bodyPartRecord2 = bodyPartRecord;
					while (bodyPartRecord2.parent != null && bodyPartRecord2.parent.def.tags.Contains(tag))
					{
						bodyPartRecord2 = bodyPartRecord2.parent;
					}
					_003C_003E7__wrap1 = bodyPartRecord2.GetChildParts(tag).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				}
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					BodyPartRecord current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
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
		IEnumerator<BodyPartRecord> IEnumerable<BodyPartRecord>.GetEnumerator()
		{
			_003CGetConnectedParts_003Ed__35 _003CGetConnectedParts_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetConnectedParts_003Ed__ = this;
			}
			else
			{
				_003CGetConnectedParts_003Ed__ = new _003CGetConnectedParts_003Ed__35(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetConnectedParts_003Ed__.tag = _003C_003E3__tag;
			return _003CGetConnectedParts_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<BodyPartRecord>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetDirectChildParts_003Ed__33 : IEnumerable<BodyPartRecord>, IEnumerable, IEnumerator<BodyPartRecord>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private BodyPartRecord _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public BodyPartRecord _003C_003E4__this;

		private int _003Ci_003E5__2;

		BodyPartRecord IEnumerator<BodyPartRecord>.Current
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
		public _003CGetDirectChildParts_003Ed__33(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			BodyPartRecord bodyPartRecord = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003Ci_003E5__2 = 0;
				break;
			case 1:
			{
				_003C_003E1__state = -1;
				int num2 = _003Ci_003E5__2 + 1;
				_003Ci_003E5__2 = num2;
				break;
			}
			}
			if (_003Ci_003E5__2 < bodyPartRecord.parts.Count)
			{
				_003C_003E2__current = bodyPartRecord.parts[_003Ci_003E5__2];
				_003C_003E1__state = 1;
				return true;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<BodyPartRecord> IEnumerable<BodyPartRecord>.GetEnumerator()
		{
			_003CGetDirectChildParts_003Ed__33 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetDirectChildParts_003Ed__33(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<BodyPartRecord>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetPartAndAllChildParts_003Ed__32 : IEnumerable<BodyPartRecord>, IEnumerable, IEnumerator<BodyPartRecord>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private BodyPartRecord _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public BodyPartRecord _003C_003E4__this;

		private int _003Ci_003E5__2;

		private IEnumerator<BodyPartRecord> _003C_003E7__wrap2;

		BodyPartRecord IEnumerator<BodyPartRecord>.Current
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
		public _003CGetPartAndAllChildParts_003Ed__32(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 2)
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
				int num = _003C_003E1__state;
				BodyPartRecord bodyPartRecord = _003C_003E4__this;
				int num2;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E2__current = bodyPartRecord;
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					_003Ci_003E5__2 = 0;
					goto IL_00d0;
				case 2:
					{
						_003C_003E1__state = -3;
						goto IL_00a4;
					}
					IL_00d0:
					if (_003Ci_003E5__2 < bodyPartRecord.parts.Count)
					{
						_003C_003E7__wrap2 = bodyPartRecord.parts[_003Ci_003E5__2].GetPartAndAllChildParts().GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_00a4;
					}
					return false;
					IL_00a4:
					if (_003C_003E7__wrap2.MoveNext())
					{
						BodyPartRecord current = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = null;
					num2 = _003Ci_003E5__2 + 1;
					_003Ci_003E5__2 = num2;
					goto IL_00d0;
				}
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
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<BodyPartRecord> IEnumerable<BodyPartRecord>.GetEnumerator()
		{
			_003CGetPartAndAllChildParts_003Ed__32 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetPartAndAllChildParts_003Ed__32(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<BodyPartRecord>)this).GetEnumerator();
		}
	}

	public BodyDef body;

	[TranslationHandle]
	public BodyPartDef def;

	[MustTranslate]
	public string customLabel;

	[Unsaved(false)]
	[TranslationHandle(Priority = 100)]
	public string untranslatedCustomLabel;

	public List<BodyPartRecord> parts = new List<BodyPartRecord>();

	public BodyPartHeight height;

	public BodyPartDepth depth;

	public float coverage = 1f;

	public List<BodyPartGroupDef> groups = new List<BodyPartGroupDef>();

	[NoTranslate]
	public string woundAnchorTag;

	public bool flipGraphic;

	public List<Rot4> visibleHediffRots;

	[Unsaved(false)]
	public BodyPartRecord parent;

	[Unsaved(false)]
	public float coverageAbsWithChildren;

	[Unsaved(false)]
	public float coverageAbs;

	[Unsaved(false)]
	private string cachedCustomLabelCap;

	public bool IsCorePart => parent == null;

	public string Label
	{
		get
		{
			if (!customLabel.NullOrEmpty())
			{
				return customLabel;
			}
			return def.label;
		}
	}

	public string LabelCap
	{
		get
		{
			if (customLabel.NullOrEmpty())
			{
				return def.LabelCap;
			}
			if (cachedCustomLabelCap == null)
			{
				cachedCustomLabelCap = customLabel.CapitalizeFirst();
			}
			return cachedCustomLabelCap;
		}
	}

	public string LabelShort => def.LabelShort;

	public string LabelShortCap => def.LabelShortCap;

	public int Index => body.GetIndexOfPart(this);

	public override string ToString()
	{
		return "BodyPartRecord(" + ((def != null) ? def.defName : "NULL_DEF") + " parts.Count=" + parts.Count + ")";
	}

	public void PostLoad()
	{
		untranslatedCustomLabel = customLabel;
	}

	public bool IsInGroup(BodyPartGroupDef group)
	{
		for (int i = 0; i < groups.Count; i++)
		{
			if (groups[i] == group)
			{
				return true;
			}
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CGetChildParts_003Ed__31))]
	public IEnumerable<BodyPartRecord> GetChildParts(BodyPartTagDef tag)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetChildParts_003Ed__31(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__tag = tag
		};
	}

	[IteratorStateMachine(typeof(_003CGetPartAndAllChildParts_003Ed__32))]
	public IEnumerable<BodyPartRecord> GetPartAndAllChildParts()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetPartAndAllChildParts_003Ed__32(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CGetDirectChildParts_003Ed__33))]
	public IEnumerable<BodyPartRecord> GetDirectChildParts()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetDirectChildParts_003Ed__33(-2)
		{
			_003C_003E4__this = this
		};
	}

	public bool HasChildParts(BodyPartTagDef tag)
	{
		return GetChildParts(tag).Any();
	}

	[IteratorStateMachine(typeof(_003CGetConnectedParts_003Ed__35))]
	public IEnumerable<BodyPartRecord> GetConnectedParts(BodyPartTagDef tag)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetConnectedParts_003Ed__35(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__tag = tag
		};
	}
}
