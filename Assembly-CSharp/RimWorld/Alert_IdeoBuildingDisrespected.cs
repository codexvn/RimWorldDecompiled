using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FDB RID: 12251
	public class Alert_IdeoBuildingDisrespected : Alert_Precept
	{
		// Token: 0x06011430 RID: 70704 RVA: 0x004F237F File Offset: 0x004F057F
		public Alert_IdeoBuildingDisrespected()
		{
		}

		// Token: 0x06011431 RID: 70705 RVA: 0x004F23A0 File Offset: 0x004F05A0
		public Alert_IdeoBuildingDisrespected(IdeoBuildingPresenceDemand demand)
		{
			this.demand = demand;
			this.label = "IdeoBuildingDisrespected".Translate(demand.parent.LabelCap);
			this.sourcePrecept = demand.parent;
		}

		// Token: 0x17002CC5 RID: 11461
		// (get) Token: 0x06011432 RID: 70706 RVA: 0x004F2404 File Offset: 0x004F0604
		private List<GlobalTargetInfo> Targets
		{
			get
			{
				this.targets.Clear();
				this.targetNames.Clear();
				Map currentMap = Find.CurrentMap;
				if (currentMap == null)
				{
					return null;
				}
				foreach (Pawn pawn in currentMap.mapPawns.FreeColonistsSpawned)
				{
					if (pawn.Ideo == this.demand.parent.ideo)
					{
						this.targetNames.Add(pawn.LabelShort);
					}
				}
				Thing thing = this.demand.BestBuilding(currentMap, false);
				if (thing != null)
				{
					this.targets.Add(thing);
				}
				return this.targets;
			}
		}

		// Token: 0x06011433 RID: 70707 RVA: 0x004F24C8 File Offset: 0x004F06C8
		public override AlertReport GetReport()
		{
			if (!ModsConfig.IdeologyActive)
			{
				return false;
			}
			Map currentMap = Find.CurrentMap;
			if (currentMap == null || !this.demand.BuildingPresent(currentMap) || !this.demand.AppliesTo(currentMap) || this.demand.RequirementsSatisfied(currentMap))
			{
				return false;
			}
			if (!Faction.OfPlayer.ideos.Has(this.demand.parent.ideo))
			{
				return false;
			}
			return new AlertReport
			{
				active = true,
				culpritsTargets = this.Targets
			};
		}

		// Token: 0x06011434 RID: 70708 RVA: 0x004F2564 File Offset: 0x004F0764
		public override TaggedString GetExplanation()
		{
			return "IdeoBuildingDisrespectedDesc".Translate(this.demand.parent.ideo.name, this.demand.parent.Label.Colorize(ColoredText.NameColor), this.demand.RoomRequirementsInfo.ToLineList(" -  "), this.targetNames.ToLineList(" -  "));
		}

		// Token: 0x0400B313 RID: 45843
		public IdeoBuildingPresenceDemand demand;

		// Token: 0x0400B314 RID: 45844
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x0400B315 RID: 45845
		private List<string> targetNames = new List<string>();
	}
}
