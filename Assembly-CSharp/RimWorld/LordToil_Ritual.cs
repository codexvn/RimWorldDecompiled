using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001741 RID: 5953
	public class LordToil_Ritual : LordToil_Gathering
	{
		// Token: 0x17001589 RID: 5513
		// (get) Token: 0x06008BE2 RID: 35810 RVA: 0x0029A3DB File Offset: 0x002985DB
		public List<LocalTargetInfo> ReservedThings
		{
			get
			{
				return this.reservedThings;
			}
		}

		// Token: 0x06008BE3 RID: 35811 RVA: 0x0029A3E4 File Offset: 0x002985E4
		public LordToil_Ritual(IntVec3 spot, LordJob_Ritual ritual, RitualStage stage, Pawn organizer) : base(spot, null)
		{
			this.stage = stage;
			this.organizer = organizer;
			this.ritual = ritual;
		}

		// Token: 0x06008BE4 RID: 35812 RVA: 0x0029A430 File Offset: 0x00298630
		public override void Init()
		{
			base.Init();
			Action action = this.startAction;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06008BE5 RID: 35813 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override ThinkTreeDutyHook VoluntaryJoinDutyHookFor(Pawn p)
		{
			return ThinkTreeDutyHook.HighPriority;
		}

		// Token: 0x06008BE6 RID: 35814 RVA: 0x0029A448 File Offset: 0x00298648
		public override void LordToilTick()
		{
			base.LordToilTick();
			Action action = this.tickAction;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06008BE7 RID: 35815 RVA: 0x0029A460 File Offset: 0x00298660
		public override void UpdateAllDuties()
		{
			this.reservedThings.Clear();
			this.cachedDuties.Clear();
			LocalTargetInfo localTargetInfo = (LocalTargetInfo)this.ritual.selectedTarget;
			IntVec3 intVec = IntVec3.Invalid;
			this.tmpPawns = this.lord.ownedPawns.ToList<Pawn>();
			for (int i = 0; i < this.tmpPawns.Count; i++)
			{
				Pawn pawn = this.tmpPawns[i];
				DutyDef dutyDef = this.stage.defaultDuty;
				IntVec3 intVec2 = this.spot;
				LocalTargetInfo secondFocus = LocalTargetInfo.Invalid;
				Thing usedThing = null;
				Rot4 overrideFacing = Rot4.Invalid;
				if (this.ritual.assignments.PawnParticipating(pawn))
				{
					dutyDef = (this.stage.GetDuty(pawn, null, this.ritual) ?? dutyDef);
					PawnStagePosition pawnStagePosition = this.ritual.PawnPositionForStage(pawn, this.stage);
					intVec2 = pawnStagePosition.cell;
					usedThing = pawnStagePosition.thing;
					overrideFacing = pawnStagePosition.orientation;
					TargetInfo targetInfo = this.ritual.SecondFocusForStage(this.stage, pawn);
					secondFocus = ((targetInfo.Thing != null) ? new LocalTargetInfo(targetInfo.Thing) : targetInfo.Cell);
				}
				this.cachedDuties.Add(new CachedPawnRitualDuty
				{
					duty = dutyDef,
					spot = intVec2,
					usedThing = usedThing,
					overrideFacing = overrideFacing,
					secondFocus = secondFocus
				});
				if (dutyDef.ritualSpectateTarget)
				{
					intVec = intVec2;
				}
			}
			this.tmpPawns = this.lord.ownedPawns.ToList<Pawn>();
			this.SortByDuty();
			for (int j = 0; j < this.tmpPawns.Count; j++)
			{
				Pawn pawn2 = this.tmpPawns[j];
				CachedPawnRitualDuty cachedPawnRitualDuty = this.cachedDuties[j];
				IntVec3 spot = cachedPawnRitualDuty.spot;
				LocalTargetInfo secondFocus2 = cachedPawnRitualDuty.secondFocus;
				Rot4 overrideFacing2 = cachedPawnRitualDuty.overrideFacing;
				Thing usedThing2 = cachedPawnRitualDuty.usedThing;
				PawnDuty pawnDuty = new PawnDuty(cachedPawnRitualDuty.duty, spot, secondFocus2, localTargetInfo, -1f);
				pawnDuty.spectateRect = CellRect.CenteredOn(this.spot, 0);
				Thing thing = localTargetInfo.Thing;
				RitualFocusProperties ritualFocusProperties = (thing != null) ? thing.def.ritualFocus : null;
				if (ritualFocusProperties != null)
				{
					pawnDuty.spectateRectAllowedSides = ritualFocusProperties.allowedSpectateSides;
					pawnDuty.spectateDistance = ritualFocusProperties.spectateDistance;
				}
				else
				{
					pawnDuty.spectateDistance = new IntRange(2, 2);
				}
				if (this.stage.allowedSpectateSidesOverride != SpectateRectSide.None)
				{
					pawnDuty.spectateRectAllowedSides = this.stage.allowedSpectateSidesOverride;
				}
				if (this.stage.spectateDistanceOverride != IntRange.Zero)
				{
					pawnDuty.spectateDistance = this.stage.spectateDistanceOverride;
				}
				if (thing != null)
				{
					pawnDuty.spectateRectAllowedSides = pawnDuty.spectateRectAllowedSides.Rotated(thing.Rotation);
					pawnDuty.spectateRect = thing.OccupiedRect();
				}
				if (intVec.IsValid && intVec != spot)
				{
					pawnDuty.spectateRect = CellRect.CenteredOn(intVec, 0);
				}
				if (cachedPawnRitualDuty.overrideFacing.IsValid)
				{
					pawnDuty.spectateRect = CellRect.CenteredOn(spot + overrideFacing2.FacingCell, 0);
					pawnDuty.overrideFacing = cachedPawnRitualDuty.overrideFacing;
				}
				if (pawnDuty.spectateRectAllowedSides == SpectateRectSide.Horizontal)
				{
					pawnDuty.spectateRectPreferredSide = ((intVec.x < spot.x) ? SpectateRectSide.Right : SpectateRectSide.Left);
				}
				else if (pawnDuty.spectateRectAllowedSides == SpectateRectSide.Vertical)
				{
					pawnDuty.spectateRectPreferredSide = ((intVec.y < spot.y) ? SpectateRectSide.Down : SpectateRectSide.Up);
				}
				else if (pawnDuty.spectateRectAllowedSides != SpectateRectSide.All && pawnDuty.spectateRectAllowedSides != SpectateRectSide.None)
				{
					pawnDuty.spectateRectPreferredSide = pawnDuty.spectateRectAllowedSides;
				}
				else
				{
					pawnDuty.spectateRectPreferredSide = SpectateRectSide.Down;
				}
				if (spot.IsValid && !this.reservedThings.Contains(spot))
				{
					this.reservedThings.Add(spot);
				}
				if (secondFocus2.IsValid && !this.reservedThings.Contains(secondFocus2))
				{
					this.reservedThings.Add(secondFocus2);
				}
				if (localTargetInfo.IsValid && !this.reservedThings.Contains(localTargetInfo))
				{
					this.reservedThings.Add(localTargetInfo);
				}
				if (!this.reservedThings.Contains(pawn2))
				{
					this.reservedThings.Add(pawn2);
				}
				foreach (Pawn pawn3 in this.ritual.pawnsDeathIgnored)
				{
					if (!this.reservedThings.Contains(pawn3.Corpse))
					{
						this.reservedThings.Add(pawn3.Corpse);
					}
				}
				if (usedThing2 != null && !this.ritual.usedThings.Contains(usedThing2))
				{
					this.ritual.usedThings.Add(usedThing2);
				}
				pawn2.mindState.duty = pawnDuty;
				pawn2.mindState.priorityWork.ClearPrioritizedWorkAndJobQueue();
				Pawn_JobTracker jobs = pawn2.jobs;
				if (jobs != null)
				{
					jobs.CheckForJobOverride(0f, true);
				}
			}
		}

		// Token: 0x06008BE8 RID: 35816 RVA: 0x0029A994 File Offset: 0x00298B94
		private void SortByDuty()
		{
			HashSet<Pawn> hashSet = new HashSet<Pawn>(this.ritual.assignments.SpectatorsForReading);
			int num = 0;
			for (int i = 0; i < this.tmpPawns.Count; i++)
			{
				if (!hashSet.Contains(this.tmpPawns[i]))
				{
					if (i != num)
					{
						List<Pawn> list = this.tmpPawns;
						int index = i;
						List<Pawn> list2 = this.tmpPawns;
						int index2 = num;
						Pawn value = this.tmpPawns[num];
						Pawn value2 = this.tmpPawns[i];
						list[index] = value;
						list2[index2] = value2;
						List<CachedPawnRitualDuty> list3 = this.cachedDuties;
						index2 = i;
						List<CachedPawnRitualDuty> list4 = this.cachedDuties;
						index = num;
						CachedPawnRitualDuty value3 = this.cachedDuties[num];
						CachedPawnRitualDuty value4 = this.cachedDuties[i];
						list3[index2] = value3;
						list4[index] = value4;
					}
					num++;
				}
			}
		}

		// Token: 0x04005AEA RID: 23274
		public Pawn organizer;

		// Token: 0x04005AEB RID: 23275
		public LordJob_Ritual ritual;

		// Token: 0x04005AEC RID: 23276
		public RitualStage stage;

		// Token: 0x04005AED RID: 23277
		public Action startAction;

		// Token: 0x04005AEE RID: 23278
		public Action tickAction;

		// Token: 0x04005AEF RID: 23279
		protected List<LocalTargetInfo> reservedThings = new List<LocalTargetInfo>();

		// Token: 0x04005AF0 RID: 23280
		private readonly List<CachedPawnRitualDuty> cachedDuties = new List<CachedPawnRitualDuty>();

		// Token: 0x04005AF1 RID: 23281
		private List<Pawn> tmpPawns = new List<Pawn>();
	}
}
