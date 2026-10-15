using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse;

public class Gene : IExposable, ILoadReferenceable
{
	[CompilerGenerated]
	private sealed class _003Cget_DisabledWorkTypes_003Ed__14 : IEnumerable<WorkTypeDef>, IEnumerable, IEnumerator<WorkTypeDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private WorkTypeDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Gene _003C_003E4__this;

		private List<WorkTypeDef> _003Clist_003E5__2;

		private int _003Ci_003E5__3;

		WorkTypeDef IEnumerator<WorkTypeDef>.Current
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
		public _003Cget_DisabledWorkTypes_003Ed__14(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Clist_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Gene gene = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_0087;
			}
			_003C_003E1__state = -1;
			if (!gene.Active)
			{
				return false;
			}
			_003Clist_003E5__2 = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			_003Ci_003E5__3 = 0;
			goto IL_0097;
			IL_0097:
			if (_003Ci_003E5__3 < _003Clist_003E5__2.Count)
			{
				if ((gene.def.disabledWorkTags & _003Clist_003E5__2[_003Ci_003E5__3].workTags) != 0)
				{
					_003C_003E2__current = _003Clist_003E5__2[_003Ci_003E5__3];
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0087;
			}
			return false;
			IL_0087:
			_003Ci_003E5__3++;
			goto IL_0097;
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
		IEnumerator<WorkTypeDef> IEnumerable<WorkTypeDef>.GetEnumerator()
		{
			_003Cget_DisabledWorkTypes_003Ed__14 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_DisabledWorkTypes_003Ed__14(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<WorkTypeDef>)this).GetEnumerator();
		}
	}

	public GeneDef def;

	public Pawn pawn;

	public int loadID;

	public Gene overriddenByGene;

	public Passion? passionPreAdd;

	public virtual string Label => def.label;

	public virtual string LabelCap => Label.CapitalizeFirst();

	public bool Overridden => overriddenByGene != null;

	public virtual bool Active
	{
		get
		{
			if (Overridden)
			{
				return false;
			}
			if (pawn?.ageTracker != null && (float)pawn.ageTracker.AgeBiologicalYears < def.minAgeActive)
			{
				return false;
			}
			if (pawn?.mutant != null && pawn.mutant.Def.disablesGenes.Contains(def))
			{
				return false;
			}
			return true;
		}
	}

	public IEnumerable<WorkTypeDef> DisabledWorkTypes
	{
		[IteratorStateMachine(typeof(_003Cget_DisabledWorkTypes_003Ed__14))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_DisabledWorkTypes_003Ed__14(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public virtual void PostMake()
	{
	}

	public virtual void PostAdd()
	{
		if (def.HasDefinedGraphicProperties)
		{
			pawn.Drawer.renderer.SetAllGraphicsDirty();
		}
	}

	public virtual void PostRemove()
	{
		if (def.HasDefinedGraphicProperties)
		{
			pawn.Drawer.renderer.SetAllGraphicsDirty();
		}
	}

	public virtual void Tick()
	{
	}

	public virtual void TickInterval(int delta)
	{
		if (ModsConfig.BiotechActive && def.mentalBreakMtbDays > 0f && def.mentalBreakDef != null && pawn.Spawned && pawn.IsHashIntervalTick(60, delta) && !pawn.InMentalState && !pawn.Downed && Rand.MTBEventOccurs(def.mentalBreakMtbDays, 60000f, 60f) && def.mentalBreakDef.Worker.BreakCanOccur(pawn))
		{
			def.mentalBreakDef.Worker.TryStart(pawn, "MentalStateReason_Gene".Translate() + ": " + LabelCap, causedByMood: false);
		}
	}

	public void OverrideBy(Gene overriddenBy)
	{
		if (ModsConfig.BiotechActive)
		{
			overriddenByGene = overriddenBy;
		}
	}

	public Passion NewPassionForOnRemoval(SkillRecord skillRecord)
	{
		if (def.passionMod == null)
		{
			return skillRecord.passion;
		}
		switch (def.passionMod.modType)
		{
		case PassionMod.PassionModType.AddOneLevel:
			switch (skillRecord.passion)
			{
			case Passion.Major:
				return Passion.Minor;
			case Passion.Minor:
				return Passion.None;
			}
			break;
		case PassionMod.PassionModType.DropAll:
			if (passionPreAdd.HasValue)
			{
				return passionPreAdd.Value;
			}
			return skillRecord.passion;
		}
		return skillRecord.passion;
	}

	public virtual void Notify_IngestedThing(Thing thing, int numTaken)
	{
	}

	public virtual void Notify_NewColony()
	{
	}

	public virtual void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
	{
		if (def.deathHistoryEvent != null)
		{
			Find.HistoryEventsManager.RecordEvent(new HistoryEvent(def.deathHistoryEvent));
		}
	}

	public virtual void Reset()
	{
	}

	public virtual IEnumerable<Gizmo> GetGizmos()
	{
		return null;
	}

	public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats()
	{
		return null;
	}

	public string GetUniqueLoadID()
	{
		return "Gene_" + loadID;
	}

	public virtual void ExposeData()
	{
		Scribe_Defs.Look(ref def, "def");
		Scribe_References.Look(ref pawn, "pawn", saveDestroyedThings: true);
		Scribe_References.Look(ref overriddenByGene, "overriddenByGene");
		Scribe_Values.Look(ref loadID, "loadID", 0);
		Scribe_Values.Look(ref passionPreAdd, "passionPreAdd", null);
	}
}
