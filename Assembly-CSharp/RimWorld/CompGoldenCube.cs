using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld;

public class CompGoldenCube : CompInteractable, IRoofCollapseAlert
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass12_0
	{
		public CompGoldenCube _003C_003E4__this;

		public Pawn selPawn;

		internal void _003CCompFloatMenuOptions_003Eb__0()
		{
			_003C_003E4__this.OrderDeactivation(selPawn);
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompFloatMenuOptions_003Ed__12 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompGoldenCube _003C_003E4__this;

		private Pawn selPawn;

		public Pawn _003C_003E3__selPawn;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
		public _003CCompFloatMenuOptions_003Ed__12(int _003C_003E1__state)
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
			CompGoldenCube compGoldenCube = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass12_0 CS_0024_003C_003E8__locals3 = new _003C_003Ec__DisplayClass12_0
				{
					_003C_003E4__this = _003C_003E4__this,
					selPawn = selPawn
				};
				if (!compGoldenCube.Deactivatable)
				{
					return false;
				}
				AcceptanceReport acceptanceReport = compGoldenCube.CanInteract(CS_0024_003C_003E8__locals3.selPawn);
				FloatMenuOption floatMenuOption = new FloatMenuOption(compGoldenCube.Props.jobString.CapitalizeFirst(), delegate
				{
					CS_0024_003C_003E8__locals3._003C_003E4__this.OrderDeactivation(CS_0024_003C_003E8__locals3.selPawn);
				});
				if (!acceptanceReport.Accepted)
				{
					floatMenuOption.Disabled = true;
					floatMenuOption.Label = floatMenuOption.Label + " (" + acceptanceReport.Reason + ")";
				}
				_003C_003E2__current = floatMenuOption;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				return false;
			}
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CCompFloatMenuOptions_003Ed__12 _003CCompFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCompFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CCompFloatMenuOptions_003Ed__ = new _003CCompFloatMenuOptions_003Ed__12(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CCompFloatMenuOptions_003Ed__.selPawn = _003C_003E3__selPawn;
			return _003CCompFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__13 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompGoldenCube _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

		Gizmo IEnumerator<Gizmo>.Current
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
		public _003CCompGetGizmosExtra_003Ed__13(int _003C_003E1__state)
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
				CompGoldenCube compGoldenCube = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (!compGoldenCube.Deactivatable)
					{
						return false;
					}
					_003C_003E7__wrap1 = ((CompInteractable)compGoldenCube).CompGetGizmosExtra().GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					Gizmo current = _003C_003E7__wrap1.Current;
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__13 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__13(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	private CompStudyUnlocks studyUnlocksInt;

	private const float EnthrallMTBDays = 12f;

	private static readonly IntRange GoldRange = new IntRange(125, 175);

	public new CompProperties_GoldenCube Props => (CompProperties_GoldenCube)props;

	public CompStudyUnlocks StudyUnlocks => studyUnlocksInt ?? (studyUnlocksInt = parent.GetComp<CompStudyUnlocks>());

	public bool Deactivatable => StudyUnlocks.Completed;

	public override void OrderForceTarget(LocalTargetInfo target)
	{
		if (ValidateTarget(target, showMessages: false))
		{
			OrderDeactivation(target.Pawn);
		}
	}

	public override void CompTickInterval(int delta)
	{
		base.CompTickInterval(delta);
		if (parent.IsHashIntervalTick(2500, delta) && (!AnyPawnHasHediff() || Rand.MTBEventOccurs(12f, 60000f, 2500f)) && QuestUtility.TryGetIdealColonist(out var pawn, parent.MapHeld, ValidatePawn))
		{
			GiveHediff(pawn);
		}
	}

	private void GiveHediff(Pawn pawn)
	{
		pawn.health.AddHediff(HediffDefOf.CubeInterest, null, null);
		Messages.Message("MessageGoldenCubeInterest".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.NeutralEvent);
	}

	[IteratorStateMachine(typeof(_003CCompFloatMenuOptions_003Ed__12))]
	public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompFloatMenuOptions_003Ed__12(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__selPawn = selPawn
		};
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__13))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__13(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string CompInspectStringExtra()
	{
		return "";
	}

	protected override void OnInteracted(Pawn caster)
	{
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		foreach (Pawn item in Find.WorldPawns.AllPawnsAlive)
		{
			if (item.health.hediffSet.HasHediff(HediffDefOf.CubeInterest) && item.IsCaravanMember() && item.Faction == Faction.OfPlayer)
			{
				PawnBanishUtility.Banish(item, giveThoughts: false);
				list2.Add(item.LabelShort);
			}
			CurePawn(item);
		}
		foreach (Map map in Find.Maps)
		{
			for (int num = map.mapPawns.AllPawns.Count - 1; num >= 0; num--)
			{
				Pawn pawn = map.mapPawns.AllPawns[num];
				if (pawn.health.hediffSet.HasHediff(HediffDefOf.CubeInterest))
				{
					PawnUtility.ForceEjectFromContainer(pawn);
					pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk, null, forced: true, forceWake: false, causedByMood: false, null, transitionSilently: true);
					list.Add(pawn.LabelShort);
				}
				CurePawn(pawn);
			}
		}
		if (list.Any() || list2.Any())
		{
			TaggedString text = "CubeDeactivationBerserkDesc".Translate(caster.Named("PAWN"));
			if (list.Any())
			{
				text += "\n\n" + "CubeDeactivationBerserkPawns".Translate() + ":\n" + list.ToLineList("- ");
			}
			if (list2.Any())
			{
				text += "\n\n" + "CubeDeactivationLostPawns".Translate() + ":\n" + list2.ToLineList("- ");
			}
			Find.LetterStack.ReceiveLetter("CubeDeactivationBerserk".Translate(), text, list.Any() ? LetterDefOf.ThreatBig : LetterDefOf.NegativeEvent);
		}
		parent.Kill(null);
	}

	private bool AnyPawnHasHediff()
	{
		if (parent.SpawnedOrAnyParentSpawned)
		{
			foreach (Pawn allPawn in parent.MapHeld.mapPawns.AllPawns)
			{
				if (allPawn.IsColonist && allPawn.health.hediffSet.HasHediff(HediffDefOf.CubeInterest))
				{
					return true;
				}
			}
		}
		Hediff hediff;
		foreach (Map map in Find.Maps)
		{
			if (map == parent.MapHeld)
			{
				continue;
			}
			foreach (Pawn allPawn2 in map.mapPawns.AllPawns)
			{
				if (allPawn2.IsColonist && allPawn2.health.hediffSet.TryGetHediff(HediffDefOf.CubeInterest, out hediff))
				{
					return true;
				}
			}
		}
		foreach (Pawn item in Find.WorldPawns.AllPawnsAlive)
		{
			if (item.IsColonist && item.health.hediffSet.TryGetHediff(HediffDefOf.CubeInterest, out hediff))
			{
				return true;
			}
		}
		return false;
	}

	public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
	{
		Thing thing = ThingMaker.MakeThing(ThingDefOf.Gold);
		thing.stackCount = GoldRange.RandomInRange;
		GenPlace.TryPlaceThing(thing, parent.PositionHeld, prevMap, ThingPlaceMode.Near, null, null, null);
	}

	private void CurePawn(Pawn pawn)
	{
		if (pawn.health.hediffSet.TryGetHediff(HediffDefOf.CubeInterest, out var hediff))
		{
			pawn.health.RemoveHediff(hediff);
		}
		if (pawn.health.hediffSet.TryGetHediff(HediffDefOf.CubeWithdrawal, out var hediff2))
		{
			pawn.health.RemoveHediff(hediff2);
		}
		if (pawn.health.hediffSet.TryGetHediff(HediffDefOf.CubeComa, out var hediff3))
		{
			pawn.health.RemoveHediff(hediff3);
		}
	}

	public override AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
	{
		AcceptanceReport result = base.CanInteract(activateBy, checkOptionalItems);
		if (!result.Accepted)
		{
			return result;
		}
		if (activateBy != null)
		{
			if (activateBy.health.hediffSet.HasHediff(HediffDefOf.CubeInterest))
			{
				return "CannotDisableCube".Translate(activateBy.Named("PAWN"));
			}
			if (checkOptionalItems && !activateBy.HasReserved(ThingDefOf.Shard) && !activateBy.CanReserveAndReachableOfDef(ThingDefOf.Shard))
			{
				return "NoItemReservedOrReachable".Translate(ThingDefOf.Shard.label);
			}
		}
		else if (checkOptionalItems && !ReservationUtility.ExistsUnreservedAmountOfDef(parent.MapHeld, ThingDefOf.Shard, Faction.OfPlayer, 1))
		{
			return "NoItemReserved".Translate(ThingDefOf.Shard.label);
		}
		return true;
	}

	private void OrderDeactivation(Pawn pawn)
	{
		TaggedString text = "CubeDeactivationConfirmation".Translate();
		List<Pawn> list = Find.Maps.SelectMany((Map m) => m.mapPawns.AllPawns.Where((Pawn p) => p.Faction == Faction.OfPlayer && p.health.hediffSet.HasHediff(HediffDefOf.CubeInterest))).ToList();
		List<Pawn> list2 = Find.WorldObjects.Caravans.SelectMany((Caravan c) => c.pawns.InnerListForReading.Where((Pawn p) => p.Faction == Faction.OfPlayer && p.health.hediffSet.HasHediff(HediffDefOf.CubeInterest))).ToList();
		if (list.Any())
		{
			text += "\n\n" + "CubeDeactivationConfirmationPawnsBerserk".Translate() + ":\n" + list.Select((Pawn p) => p.LabelShort).ToLineList("- ");
		}
		if (list2.Any())
		{
			text += "\n\n" + "CubeDeactivationConfirmationPawnsLost".Translate() + ":\n" + list2.Select((Pawn p) => p.LabelShort).ToLineList("- ");
		}
		text += "\n\n" + "CubeDeactivationConfirmationEnd".Translate();
		Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(text, delegate
		{
			if (pawn.TryFindReserveAndReachableOfDef(ThingDefOf.Shard, out var thing))
			{
				Job job = JobMaker.MakeJob(JobDefOf.InteractThing, parent, thing);
				job.count = 1;
				job.playerForced = true;
				pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
			}
		}));
	}

	private bool ValidatePawn(Pawn pawn)
	{
		if (pawn.health.hediffSet.HasHediff(HediffDefOf.CubeInterest) || pawn.health.hediffSet.HasHediff(HediffDefOf.CubeComa))
		{
			return false;
		}
		if (pawn.MapHeld != parent.MapHeld)
		{
			return false;
		}
		if (parent.IsInCaravan())
		{
			return pawn.GetCaravan()?.AllThings?.Contains(parent) == true;
		}
		return true;
	}

	public RoofCollapseResponse Notify_OnBeforeRoofCollapse()
	{
		if (RCellFinder.TryFindRandomCellNearWith(parent.Position, (IntVec3 c) => IsValidCell(c, parent.MapHeld), parent.MapHeld, out var result, 10))
		{
			SkipUtility.SkipTo(parent, result, parent.MapHeld);
		}
		return RoofCollapseResponse.RemoveThing;
	}

	private static bool IsValidCell(IntVec3 cell, Map map)
	{
		if (cell.InBounds(map))
		{
			return cell.Walkable(map);
		}
		return false;
	}
}
