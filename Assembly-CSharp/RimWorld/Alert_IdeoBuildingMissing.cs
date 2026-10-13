using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FD9 RID: 12249
	public class Alert_IdeoBuildingMissing : Alert_Precept
	{
		// Token: 0x06011425 RID: 70693 RVA: 0x004F1ECF File Offset: 0x004F00CF
		public Alert_IdeoBuildingMissing()
		{
		}

		// Token: 0x06011426 RID: 70694 RVA: 0x004F1EF0 File Offset: 0x004F00F0
		public Alert_IdeoBuildingMissing(IdeoBuildingPresenceDemand demand)
		{
			this.demand = demand;
			this.label = "IdeoBuildingMissing".Translate(demand.parent.LabelCap);
			this.sourcePrecept = demand.parent;
		}

		// Token: 0x17002CC4 RID: 11460
		// (get) Token: 0x06011427 RID: 70695 RVA: 0x004F1F54 File Offset: 0x004F0154
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
					if (!pawn.IsSlave && pawn.Ideo == this.demand.parent.ideo)
					{
						this.targets.Add(pawn);
						this.targetNames.Add(pawn.LabelShort.Colorize(ColoredText.NameColor));
					}
				}
				return this.targets;
			}
		}

		// Token: 0x06011428 RID: 70696 RVA: 0x004F2018 File Offset: 0x004F0218
		public override AlertReport GetReport()
		{
			if (!ModsConfig.IdeologyActive)
			{
				return false;
			}
			Map currentMap = Find.CurrentMap;
			if (currentMap == null || this.demand.BuildingPresent(currentMap))
			{
				return false;
			}
			if (!this.demand.AppliesTo(currentMap))
			{
				return false;
			}
			if (!Faction.OfPlayer.ideos.Has(this.demand.parent.ideo))
			{
				return false;
			}
			if (!Faction.OfPlayer.ideos.IsPrimary(this.demand.parent.ideo) && this.demand.parent.ideo.ColonistBelieverCountCached < 3)
			{
				return false;
			}
			List<GlobalTargetInfo> list = this.Targets;
			return new AlertReport
			{
				active = list.Any<GlobalTargetInfo>(),
				culpritsTargets = list
			};
		}

		// Token: 0x06011429 RID: 70697 RVA: 0x004F20F4 File Offset: 0x004F02F4
		public override TaggedString GetExplanation()
		{
			string value = "";
			if (!this.demand.roomRequirements.NullOrEmpty<RoomRequirement>())
			{
				value = "\n\n" + "IdeoBuildingRoomRequirementsDesc".Translate(this.demand.parent.Label.Colorize(ColoredText.NameColor)).Resolve() + ":\n" + this.demand.RoomRequirementsInfo.ToLineList(" -  ");
			}
			string arg = string.Empty;
			if (this.demand.parent.ThingDef.designationCategory != null)
			{
				arg = "IdeoBuildingArchitectTabDesc".Translate(this.demand.parent.ThingDef.designationCategory.label.Named("TABNAME"));
			}
			else
			{
				IEnumerable<string> enumerable = (from u in (from r in DefDatabase<RecipeDef>.AllDefsListForReading
				where r.recipeUsers != null && r.products.Count == 1 && r.products.Any((ThingDefCountClass p) => p.thingDef == this.demand.parent.ThingDef) && !r.IsSurgery
				select r).SelectMany((RecipeDef r) => r.recipeUsers)
				select u.label).Distinct<string>();
				if (enumerable.Any<string>())
				{
					arg = "IdeoBuildingCanBeCraftedAt".Translate(enumerable.ToLineList(" -  ", true));
				}
			}
			return "IdeoBuildingMissingDesc".Translate(this.demand.parent.Label.Colorize(ColoredText.NameColor), this.demand.parent.ideo.Named("IDEO"), ("IdeoBuildingExpectations".Translate() + " " + this.demand.minExpectation.label).Colorize(ColoredText.ExpectationsColor), this.targetNames.ToLineList(" -  "), value, 3.Named("MINBELIEVERS"), this.demand.parent.ThingDef.Named("BASEBUILDING"), arg.Named("BUILDINGINFO"));
		}

		// Token: 0x0400B30D RID: 45837
		public IdeoBuildingPresenceDemand demand;

		// Token: 0x0400B30E RID: 45838
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x0400B30F RID: 45839
		private List<string> targetNames = new List<string>();
	}
}
