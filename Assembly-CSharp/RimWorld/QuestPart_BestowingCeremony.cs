using System;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001BEF RID: 7151
	public class QuestPart_BestowingCeremony : QuestPart_MakeLord
	{
		// Token: 0x06009F43 RID: 40771 RVA: 0x002DFC0E File Offset: 0x002DDE0E
		public override bool QuestPartReserves(Pawn p)
		{
			return p == this.bestower || p == this.target;
		}

		// Token: 0x06009F44 RID: 40772 RVA: 0x002DFC24 File Offset: 0x002DDE24
		public static bool TryGetCeremonySpot(Pawn pawn, Faction bestowingFaction, out LocalTargetInfo spot, out IntVec3 absoluteSpot)
		{
			QuestPart_BestowingCeremony.<>c__DisplayClass6_0 CS$<>8__locals1 = new QuestPart_BestowingCeremony.<>c__DisplayClass6_0();
			CS$<>8__locals1.pawn = pawn;
			if (CS$<>8__locals1.pawn != null)
			{
				RoyalTitleDef titleAwardedWhenUpdating = CS$<>8__locals1.pawn.royalty.GetTitleAwardedWhenUpdating(bestowingFaction, CS$<>8__locals1.pawn.royalty.GetFavor(bestowingFaction));
				if (titleAwardedWhenUpdating != null && titleAwardedWhenUpdating.throneRoomRequirements != null && CS$<>8__locals1.pawn.ownership.AssignedThrone != null)
				{
					CS$<>8__locals1.throne = CS$<>8__locals1.pawn.ownership.AssignedThrone;
					CS$<>8__locals1.throneRoom = CS$<>8__locals1.throne.GetRoom(RegionType.Set_All);
					spot = CS$<>8__locals1.throne;
					IntVec3 facingCell = spot.Thing.Rotation.FacingCell;
					absoluteSpot = spot.Thing.InteractionCell + facingCell * 3;
					bool flag = false;
					for (int i = 0; i < 3; i++)
					{
						if (CS$<>8__locals1.<TryGetCeremonySpot>g__ValidateSpot|0(absoluteSpot))
						{
							flag = true;
							break;
						}
						absoluteSpot -= facingCell;
					}
					if (flag)
					{
						return true;
					}
					absoluteSpot = spot.Thing.InteractionCell - facingCell * 3;
					for (int j = 0; j < 3; j++)
					{
						if (CS$<>8__locals1.<TryGetCeremonySpot>g__ValidateSpot|0(absoluteSpot))
						{
							flag = true;
							break;
						}
						absoluteSpot += facingCell;
					}
					if (flag)
					{
						return true;
					}
					if (CS$<>8__locals1.throneRoom != null && (from c in CS$<>8__locals1.throneRoom.Cells
					where base.<TryGetCeremonySpot>g__ValidateSpot|0(c)
					select c).TryRandomElementByWeight((IntVec3 c) => c.DistanceTo(CS$<>8__locals1.throne.Position), out absoluteSpot))
					{
						return true;
					}
				}
				IntVec3 intVec;
				if (CS$<>8__locals1.pawn.Map != null && CS$<>8__locals1.pawn.Map.IsPlayerHome && (RCellFinder.TryFindGatheringSpot(CS$<>8__locals1.pawn, GatheringDefOf.Party, true, out intVec) || RCellFinder.TryFindRandomSpotJustOutsideColony(CS$<>8__locals1.pawn.Position, CS$<>8__locals1.pawn.Map, out intVec)))
				{
					spot = (absoluteSpot = intVec);
					return true;
				}
			}
			spot = LocalTargetInfo.Invalid;
			absoluteSpot = IntVec3.Invalid;
			return false;
		}

		// Token: 0x06009F46 RID: 40774 RVA: 0x002DFE50 File Offset: 0x002DE050
		protected override Lord MakeLord()
		{
			LocalTargetInfo targetSpot;
			IntVec3 spotCell;
			if (!QuestPart_BestowingCeremony.TryGetCeremonySpot(this.target, this.bestower.Faction, out targetSpot, out spotCell))
			{
				Log.Error("Cannot find ceremony spot for bestowing ceremony!");
				return null;
			}
			Lord lord = LordMaker.MakeNewLord(this.faction, new LordJob_BestowingCeremony(this.bestower, this.target, targetSpot, spotCell, this.shuttle, this.questTag + ".QuestEnded"), base.Map, null);
			QuestUtility.AddQuestTag(ref lord.questTags, this.questTag);
			return lord;
		}

		// Token: 0x06009F47 RID: 40775 RVA: 0x002DFED1 File Offset: 0x002DE0D1
		public override void Cleanup()
		{
			Find.SignalManager.SendSignal(new Signal(this.questTag + ".QuestEnded", this.quest.Named("SUBJECT")));
		}

		// Token: 0x06009F48 RID: 40776 RVA: 0x002DFF04 File Offset: 0x002DE104
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.bestower, "bestower", false);
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_Values.Look<string>(ref this.questTag, "questTag", null, false);
		}

		// Token: 0x04006E20 RID: 28192
		public const int PreferredDistanceFromThrone = 3;

		// Token: 0x04006E21 RID: 28193
		public Pawn bestower;

		// Token: 0x04006E22 RID: 28194
		public Pawn target;

		// Token: 0x04006E23 RID: 28195
		public Thing shuttle;

		// Token: 0x04006E24 RID: 28196
		public string questTag;
	}
}
