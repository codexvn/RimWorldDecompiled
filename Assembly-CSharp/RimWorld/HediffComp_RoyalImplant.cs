using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200385B RID: 14427
	public class HediffComp_RoyalImplant : HediffComp
	{
		// Token: 0x170032EF RID: 13039
		// (get) Token: 0x06013F24 RID: 81700 RVA: 0x005EB0EC File Offset: 0x005E92EC
		public HediffCompProperties_RoyalImplant Props
		{
			get
			{
				return (HediffCompProperties_RoyalImplant)this.props;
			}
		}

		// Token: 0x06013F25 RID: 81701 RVA: 0x005EB0FC File Offset: 0x005E92FC
		public static int GetImplantLevel(Hediff implant)
		{
			Hediff_Level hediff_Level = implant as Hediff_Level;
			if (hediff_Level != null)
			{
				return hediff_Level.level;
			}
			return 0;
		}

		// Token: 0x06013F26 RID: 81702 RVA: 0x005EB11B File Offset: 0x005E931B
		public bool IsViolatingRulesOf(Faction faction, int violationSourceLevel = -1)
		{
			return ThingRequiringRoyalPermissionUtility.IsViolatingRulesOf(base.Def, this.parent.pawn, faction, (violationSourceLevel == -1) ? HediffComp_RoyalImplant.GetImplantLevel(this.parent) : violationSourceLevel);
		}

		// Token: 0x06013F27 RID: 81703 RVA: 0x005EB148 File Offset: 0x005E9348
		public override void Notify_ImplantUsed(string violationSourceName, float detectionChance, int violationSourceLevel = -1)
		{
			base.Notify_ImplantUsed(violationSourceName, detectionChance, -1);
			if (this.parent.pawn.Faction != Faction.OfPlayer)
			{
				return;
			}
			if (!Rand.Chance(detectionChance))
			{
				return;
			}
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (this.IsViolatingRulesOf(faction, violationSourceLevel))
				{
					faction.Notify_RoyalThingUseViolation(this.parent.def, base.Pawn, violationSourceName, detectionChance, violationSourceLevel);
				}
			}
		}
	}
}
