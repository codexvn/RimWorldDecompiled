using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class Recipe_RemoveBodyPart : Recipe_Surgery
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass2_0
	{
		public BodyPartRecord part;

		internal bool _003CGetPartsToApplyOn_003Eb__0(Hediff d)
		{
			if ((!(d is Hediff_Injury) || d.IsPermanent()) && d.def.isBad && d.Visible)
			{
				return d.Part == part;
			}
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetPartsToApplyOn_003Ed__2 : IEnumerable<BodyPartRecord>, IEnumerable, IEnumerator<BodyPartRecord>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private BodyPartRecord _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

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
		public _003CGetPartsToApplyOn_003Ed__2(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || (uint)(num - 1) <= 3u)
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
				{
					_003C_003E1__state = -1;
					IEnumerable<BodyPartRecord> notMissingParts = pawn.health.hediffSet.GetNotMissingParts();
					_003C_003E7__wrap1 = notMissingParts.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				}
				case 1:
					_003C_003E1__state = -3;
					break;
				case 2:
					_003C_003E1__state = -3;
					break;
				case 3:
					_003C_003E1__state = -3;
					break;
				case 4:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					_003C_003Ec__DisplayClass2_0 CS_0024_003C_003E8__locals10 = new _003C_003Ec__DisplayClass2_0
					{
						part = _003C_003E7__wrap1.Current
					};
					if (pawn.health.hediffSet.HasDirectlyAddedPartFor(CS_0024_003C_003E8__locals10.part))
					{
						_003C_003E2__current = CS_0024_003C_003E8__locals10.part;
						_003C_003E1__state = 1;
						return true;
					}
					if (MedicalRecipesUtility.IsCleanAndDroppable(pawn, CS_0024_003C_003E8__locals10.part))
					{
						_003C_003E2__current = CS_0024_003C_003E8__locals10.part;
						_003C_003E1__state = 2;
						return true;
					}
					if (CS_0024_003C_003E8__locals10.part != pawn.RaceProps.body.corePart && CS_0024_003C_003E8__locals10.part.def.canSuggestAmputation && pawn.health.hediffSet.hediffs.Any((Hediff d) => (!(d is Hediff_Injury) || d.IsPermanent()) && d.def.isBad && d.Visible && d.Part == CS_0024_003C_003E8__locals10.part))
					{
						_003C_003E2__current = CS_0024_003C_003E8__locals10.part;
						_003C_003E1__state = 3;
						return true;
					}
					if (CS_0024_003C_003E8__locals10.part.def.forceAlwaysRemovable)
					{
						_003C_003E2__current = CS_0024_003C_003E8__locals10.part;
						_003C_003E1__state = 4;
						return true;
					}
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
			_003CGetPartsToApplyOn_003Ed__2 _003CGetPartsToApplyOn_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetPartsToApplyOn_003Ed__ = this;
			}
			else
			{
				_003CGetPartsToApplyOn_003Ed__ = new _003CGetPartsToApplyOn_003Ed__2(0);
			}
			_003CGetPartsToApplyOn_003Ed__.pawn = _003C_003E3__pawn;
			return _003CGetPartsToApplyOn_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<BodyPartRecord>)this).GetEnumerator();
		}
	}

	protected virtual bool SpawnPartsWhenRemoved => true;

	[IteratorStateMachine(typeof(_003CGetPartsToApplyOn_003Ed__2))]
	public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetPartsToApplyOn_003Ed__2(-2)
		{
			_003C_003E3__pawn = pawn
		};
	}

	public override bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
	{
		if ((pawn.Faction == billDoerFaction || pawn.Faction == null) && !pawn.IsQuestLodger())
		{
			return false;
		}
		if (HealthUtility.PartRemovalIntent(pawn, part) == BodyPartRemovalIntent.Harvest)
		{
			return true;
		}
		return false;
	}

	public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
	{
		bool flag = MedicalRecipesUtility.IsClean(pawn, part);
		bool flag2 = IsViolationOnPawn(pawn, part, Faction.OfPlayer);
		if (billDoer != null)
		{
			if (CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
			{
				return;
			}
			TaleRecorder.RecordTale(TaleDefOf.DidSurgery, billDoer, pawn);
			pawn.health.hediffSet.GetDirectlyAddedPartFor(part)?.Notify_SurgicallyRemoved(billDoer);
			if (SpawnPartsWhenRemoved)
			{
				MedicalRecipesUtility.SpawnNaturalPartIfClean(pawn, part, billDoer.Position, billDoer.Map);
				MedicalRecipesUtility.SpawnThingsFromHediffs(pawn, part, billDoer.Position, billDoer.Map);
			}
		}
		DamagePart(pawn, part);
		pawn.Drawer.renderer.SetAllGraphicsDirty();
		if (flag)
		{
			ApplyThoughts(pawn, billDoer);
		}
		if (flag2)
		{
			ReportViolation(pawn, billDoer, pawn.HomeFaction, -70);
		}
	}

	public virtual void DamagePart(Pawn pawn, BodyPartRecord part)
	{
		pawn.TakeDamage(new DamageInfo(DamageDefOf.SurgicalCut, 99999f, 999f, -1f, null, part));
	}

	public virtual void ApplyThoughts(Pawn pawn, Pawn billDoer)
	{
		if (pawn.Dead)
		{
			ThoughtUtility.GiveThoughtsForPawnExecuted(pawn, billDoer, PawnExecutionKind.OrganHarvesting);
		}
		else
		{
			ThoughtUtility.GiveThoughtsForPawnOrganHarvested(pawn, billDoer);
		}
	}

	public override string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
	{
		if (!part.def.removeRecipeLabelOverride.NullOrEmpty())
		{
			return part.def.removeRecipeLabelOverride;
		}
		if (pawn.RaceProps.IsMechanoid || pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(part))
		{
			if (pawn.health.hediffSet.TryGetDirectlyAddedPartFor(part, out var hediff))
			{
				return "RemovePart".Translate(hediff.Label);
			}
			return RecipeDefOf.RemoveBodyPart.label;
		}
		switch (HealthUtility.PartRemovalIntent(pawn, part))
		{
		case BodyPartRemovalIntent.Amputate:
			if (part.depth == BodyPartDepth.Inside || part.def.socketed)
			{
				return "RemoveOrgan".Translate();
			}
			return "Amputate".Translate();
		case BodyPartRemovalIntent.Harvest:
			return "HarvestOrgan".Translate();
		default:
			throw new InvalidOperationException();
		}
	}
}
