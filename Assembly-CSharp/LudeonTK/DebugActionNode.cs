using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace LudeonTK;

public class DebugActionNode
{
	[CompilerGenerated]
	private sealed class _003CPawnsAt_003Ed__41 : IEnumerable<Pawn>, IEnumerable, IEnumerator<Pawn>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Pawn _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Map map;

		public Map _003C_003E3__map;

		private IntVec3 cell;

		public IntVec3 _003C_003E3__cell;

		private IEnumerator<Thing> _003C_003E7__wrap1;

		private IEnumerator<Pawn> _003C_003E7__wrap2;

		Pawn IEnumerator<Pawn>.Current
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
		public _003CPawnsAt_003Ed__41(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || num == 1)
			{
				try
				{
					if (num == -4 || num == 1)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = null;
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
					_003C_003E1__state = -4;
					goto IL_009f;
				}
				_003C_003E1__state = -1;
				_003C_003E7__wrap1 = map.thingGrid.ThingsAt(cell).GetEnumerator();
				_003C_003E1__state = -3;
				goto IL_00b9;
				IL_009f:
				if (_003C_003E7__wrap2.MoveNext())
				{
					Pawn current = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap2 = null;
				goto IL_00b9;
				IL_00b9:
				IThingHolder thingHolder;
				do
				{
					if (_003C_003E7__wrap1.MoveNext())
					{
						thingHolder = _003C_003E7__wrap1.Current as IThingHolder;
						continue;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					return false;
				}
				while (thingHolder == null);
				_003C_003E7__wrap2 = PawnsInside(thingHolder).GetEnumerator();
				_003C_003E1__state = -4;
				goto IL_009f;
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
			_003C_003E1__state = -3;
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
		IEnumerator<Pawn> IEnumerable<Pawn>.GetEnumerator()
		{
			_003CPawnsAt_003Ed__41 _003CPawnsAt_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CPawnsAt_003Ed__ = this;
			}
			else
			{
				_003CPawnsAt_003Ed__ = new _003CPawnsAt_003Ed__41(0);
			}
			_003CPawnsAt_003Ed__.cell = _003C_003E3__cell;
			_003CPawnsAt_003Ed__.map = _003C_003E3__map;
			return _003CPawnsAt_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Pawn>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CPawnsInside_003Ed__40 : IEnumerable<Pawn>, IEnumerable, IEnumerator<Pawn>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Pawn _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IThingHolder holder;

		public IThingHolder _003C_003E3__holder;

		private IEnumerator<Thing> _003C_003E7__wrap1;

		private IEnumerator<Pawn> _003C_003E7__wrap2;

		Pawn IEnumerator<Pawn>.Current
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
		public _003CPawnsInside_003Ed__40(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || num == 2)
			{
				try
				{
					if (num == -4 || num == 2)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				IThingHolder thingHolder;
				ThingOwner directlyHeldThings;
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (holder is Pawn pawn)
					{
						_003C_003E2__current = pawn;
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_0052;
				case 1:
					_003C_003E1__state = -1;
					goto IL_0052;
				case 2:
					{
						_003C_003E1__state = -4;
						goto IL_00d8;
					}
					IL_00f2:
					do
					{
						if (_003C_003E7__wrap1.MoveNext())
						{
							thingHolder = _003C_003E7__wrap1.Current as IThingHolder;
							continue;
						}
						_003C_003Em__Finally1();
						_003C_003E7__wrap1 = null;
						return false;
					}
					while (thingHolder == null);
					_003C_003E7__wrap2 = PawnsInside(thingHolder).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_00d8;
					IL_0052:
					directlyHeldThings = holder.GetDirectlyHeldThings();
					if (directlyHeldThings == null)
					{
						return false;
					}
					_003C_003E7__wrap1 = ((IEnumerable<Thing>)directlyHeldThings).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00f2;
					IL_00d8:
					if (_003C_003E7__wrap2.MoveNext())
					{
						Pawn current = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = null;
					goto IL_00f2;
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
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
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
		IEnumerator<Pawn> IEnumerable<Pawn>.GetEnumerator()
		{
			_003CPawnsInside_003Ed__40 _003CPawnsInside_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CPawnsInside_003Ed__ = this;
			}
			else
			{
				_003CPawnsInside_003Ed__ = new _003CPawnsInside_003Ed__40(0);
			}
			_003CPawnsInside_003Ed__.holder = _003C_003E3__holder;
			return _003CPawnsInside_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Pawn>)this).GetEnumerator();
		}
	}

	public string label;

	public DebugActionType actionType;

	public Action action;

	public Action<Pawn> pawnAction;

	public Func<List<DebugActionNode>> childGetter;

	public DebugActionAttribute sourceAttribute;

	public Func<bool> visibilityGetter;

	public Func<string> labelGetter;

	public string category;

	public int displayPriority;

	public FieldInfo settingsField;

	private bool childrenSetup;

	public DebugActionNode parent;

	public List<DebugActionNode> children = new List<DebugActionNode>();

	private string cachedPath;

	private string cachedLabelNow;

	private bool sorted;

	private bool? cachedCheckOn;

	private int lastLabelCacheFrame = -1;

	private const int LabelRecacheFrameCount = 30;

	public bool IsRoot => parent == null;

	public bool On
	{
		get
		{
			if (!cachedCheckOn.HasValue)
			{
				if (settingsField == null)
				{
					cachedCheckOn = false;
				}
				else
				{
					cachedCheckOn = (bool)settingsField.GetValue(null);
				}
			}
			return cachedCheckOn.Value;
		}
	}

	public string LabelNow
	{
		get
		{
			if (Time.frameCount >= lastLabelCacheFrame + 30)
			{
				DirtyLabelCache();
			}
			if (cachedLabelNow == null)
			{
				if (labelGetter != null)
				{
					try
					{
						cachedLabelNow = labelGetter();
					}
					catch (Exception ex)
					{
						Log.Error("Exception getting label for DebugActionNode: " + ex);
						cachedLabelNow = null;
						return label;
					}
				}
				else
				{
					cachedLabelNow = label;
				}
			}
			return cachedLabelNow;
		}
	}

	public bool ActiveNow
	{
		get
		{
			switch (actionType)
			{
			case DebugActionType.ToolWorld:
				return WorldRendererUtility.WorldSelected;
			case DebugActionType.ToolMap:
			case DebugActionType.ToolMapForPawns:
				return WorldRendererUtility.DrawingMap;
			default:
				return true;
			}
		}
	}

	public bool VisibleNow
	{
		get
		{
			if (sourceAttribute != null && !sourceAttribute.IsAllowedInCurrentGameState)
			{
				return false;
			}
			if (visibilityGetter != null && !visibilityGetter())
			{
				return false;
			}
			return true;
		}
	}

	public string Path
	{
		get
		{
			if (cachedPath == null)
			{
				if (parent != null && !parent.IsRoot)
				{
					cachedPath = parent.Path + "\\" + label;
				}
				else
				{
					cachedPath = label;
				}
			}
			return cachedPath;
		}
	}

	public DebugActionNode()
	{
	}

	public DebugActionNode(string label, DebugActionType actionType = DebugActionType.Action, Action action = null, Action<Pawn> pawnAction = null)
	{
		this.label = label;
		this.actionType = actionType;
		this.action = action;
		this.pawnAction = pawnAction;
	}

	public void AddChild(DebugActionNode child)
	{
		child.SetParent(this);
		children.Add(child);
		sorted = false;
	}

	public void SetParent(DebugActionNode newParent)
	{
		parent = newParent;
		DirtyPath();
	}

	private void DirtyPath()
	{
		cachedPath = null;
		foreach (DebugActionNode child in children)
		{
			child.DirtyPath();
		}
	}

	public void DirtyLabelCache()
	{
		cachedLabelNow = null;
		cachedCheckOn = null;
		lastLabelCacheFrame = Time.frameCount;
	}

	public void TrySort()
	{
		if (sorted)
		{
			return;
		}
		if (!children.NullOrEmpty())
		{
			children = (from r in children
				orderby DebugActionCategories.GetOrderFor(r.category), r.category, -r.displayPriority
				select r).ToList();
		}
		sorted = true;
	}

	public void TrySetupChildren()
	{
		if (!childrenSetup && childGetter != null)
		{
			foreach (DebugActionNode item in childGetter())
			{
				AddChild(item);
			}
		}
		childrenSetup = true;
	}

	[IteratorStateMachine(typeof(_003CPawnsInside_003Ed__40))]
	private static IEnumerable<Pawn> PawnsInside(IThingHolder holder)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPawnsInside_003Ed__40(-2)
		{
			_003C_003E3__holder = holder
		};
	}

	[IteratorStateMachine(typeof(_003CPawnsAt_003Ed__41))]
	private static IEnumerable<Pawn> PawnsAt(IntVec3 cell, Map map)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPawnsAt_003Ed__41(-2)
		{
			_003C_003E3__cell = cell,
			_003C_003E3__map = map
		};
	}

	public void Enter(Dialog_Debug dialog)
	{
		TrySetupChildren();
		if (children.Any())
		{
			TrySort();
			if (dialog == null)
			{
				dialog = new Dialog_Debug();
				Find.WindowStack.Add(dialog);
			}
			dialog.SetCurrentNode(this);
			return;
		}
		dialog?.Close();
		switch (actionType)
		{
		case DebugActionType.Action:
			action();
			break;
		case DebugActionType.ToolMap:
		case DebugActionType.ToolWorld:
			DebugTools.curTool = new DebugTool(LabelNow, action);
			break;
		case DebugActionType.ToolMapForPawns:
		{
			DebugActionNode instance = this;
			DebugTools.curTool = new DebugTool(LabelNow, delegate
			{
				if (UI.MouseCell().InBounds(Find.CurrentMap))
				{
					foreach (Pawn item in PawnsAt(UI.MouseCell(), Find.CurrentMap).ToList())
					{
						instance.pawnAction(item);
					}
				}
			});
			break;
		}
		}
		DirtyLabelCache();
	}
}
