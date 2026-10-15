using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse;

public class WorkTypeDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__18 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public WorkTypeDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

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
		public _003CConfigErrors_003Ed__18(int _003C_003E1__state)
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
				WorkTypeDef workTypeDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)workTypeDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_006f;
				case 1:
					_003C_003E1__state = -3;
					goto IL_006f;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_006f:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (workTypeDef.naturalPriority < 0 || workTypeDef.naturalPriority > 10000)
					{
						_003C_003E2__current = "naturalPriority is " + workTypeDef.naturalPriority + ", but it must be between 0 and 10000";
						_003C_003E1__state = 2;
						return true;
					}
					break;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__18 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__18(0)
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

	public WorkTags workTags;

	[MustTranslate]
	public string labelShort;

	[MustTranslate]
	public string pawnLabel;

	[MustTranslate]
	public string gerundLabel;

	[MustTranslate]
	public string verb;

	public bool visible = true;

	public bool visibleOnlyWithChildrenInColony;

	public int naturalPriority;

	public bool alwaysStartActive;

	public bool requireCapableColonist;

	public List<SkillDef> relevantSkills = new List<SkillDef>();

	public bool disabledForSlaves;

	[Unsaved(false)]
	public List<WorkGiverDef> workGiversByPriority = new List<WorkGiverDef>();

	[Unsaved(false)]
	private bool cachedVisibleCurrently;

	[Unsaved(false)]
	private int cachedFrameVisibleCurrently = -1;

	public bool VisibleCurrently
	{
		get
		{
			if (cachedFrameVisibleCurrently == -1 || cachedFrameVisibleCurrently < Time.frameCount - 30)
			{
				cachedVisibleCurrently = VisibleNow();
				cachedFrameVisibleCurrently = Time.frameCount;
			}
			return cachedVisibleCurrently;
		}
	}

	public bool VisibleNow(Pawn ignorePawn = null, Pawn alsoCheckPawn = null)
	{
		if (!visible)
		{
			return false;
		}
		if (visibleOnlyWithChildrenInColony)
		{
			bool flag = false;
			foreach (Pawn item in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction)
			{
				if (item.RaceProps.Humanlike && item != ignorePawn && item.DevelopmentalStage.Juvenile())
				{
					flag = true;
					break;
				}
			}
			if (alsoCheckPawn != null && alsoCheckPawn.DevelopmentalStage.Juvenile())
			{
				flag = true;
			}
			if (!flag)
			{
				return false;
			}
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__18))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__18(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ResolveReferences()
	{
		base.ResolveReferences();
		foreach (WorkGiverDef item in from d in DefDatabase<WorkGiverDef>.AllDefs
			where d.workType == this
			orderby d.priorityInType descending
			select d)
		{
			workGiversByPriority.Add(item);
		}
	}

	public override int GetHashCode()
	{
		return Gen.HashCombine(defName.GetHashCode(), gerundLabel);
	}
}
