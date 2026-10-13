using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003378 RID: 13176
	public class ChoiceLetter_RansomDemand : ChoiceLetter
	{
		// Token: 0x17003059 RID: 12377
		// (get) Token: 0x0601285D RID: 75869 RVA: 0x0057742C File Offset: 0x0057562C
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				if (base.ArchivedOnly)
				{
					yield return base.Option_Close;
					yield break;
				}
				DiaOption diaOption = new DiaOption("RansomDemand_Accept".Translate());
				diaOption.action = delegate()
				{
					this.faction.kidnapped.RemoveKidnappedPawn(this.kidnapped);
					Find.WorldPawns.RemovePawn(this.kidnapped);
					IntVec3 intVec;
					if (this.faction.def.techLevel < TechLevel.Industrial)
					{
						if (!CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => c.Standable(this.map) && this.map.reachability.CanReachColony(c), this.map, CellFinder.EdgeRoadChance_Friendly, out intVec) && !CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => c.Standable(this.map), this.map, CellFinder.EdgeRoadChance_Friendly, out intVec))
						{
							Log.Warning("Could not find any edge cell.");
							intVec = DropCellFinder.TradeDropSpot(this.map);
						}
						GenSpawn.Spawn(this.kidnapped, intVec, this.map, WipeMode.Vanish);
					}
					else
					{
						intVec = DropCellFinder.TradeDropSpot(this.map);
						TradeUtility.SpawnDropPod(intVec, this.map, this.kidnapped);
					}
					CameraJumper.TryJump(intVec, this.map, CameraJumper.MovementMode.Pan);
					TradeUtility.LaunchSilver(this.map, this.fee);
					Find.LetterStack.RemoveLetter(this);
				};
				diaOption.resolveTree = true;
				if (!TradeUtility.ColonyHasEnoughSilver(this.map, this.fee))
				{
					diaOption.Disable("NeedSilverLaunchable".Translate(this.fee.ToString()));
				}
				yield return diaOption;
				yield return base.Option_Reject;
				yield return base.Option_Postpone;
				yield break;
			}
		}

		// Token: 0x1700305A RID: 12378
		// (get) Token: 0x0601285E RID: 75870 RVA: 0x0057743C File Offset: 0x0057563C
		public override bool CanShowInLetterStack
		{
			get
			{
				return base.CanShowInLetterStack && Find.Maps.Contains(this.map) && this.faction.kidnapped.KidnappedPawnsListForReading.Contains(this.kidnapped);
			}
		}

		// Token: 0x0601285F RID: 75871 RVA: 0x00577478 File Offset: 0x00575678
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Map>(ref this.map, "map", false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_References.Look<Pawn>(ref this.kidnapped, "kidnapped", false);
			Scribe_Values.Look<int>(ref this.fee, "fee", 0, false);
		}

		// Token: 0x0400C071 RID: 49265
		public Map map;

		// Token: 0x0400C072 RID: 49266
		public Faction faction;

		// Token: 0x0400C073 RID: 49267
		public Pawn kidnapped;

		// Token: 0x0400C074 RID: 49268
		public int fee;
	}
}
