using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CB6 RID: 11446
	public class CompGiveHediffSeverity : ThingComp
	{
		// Token: 0x170028A2 RID: 10402
		// (get) Token: 0x06010112 RID: 65810 RVA: 0x004A6306 File Offset: 0x004A4506
		private CompProperties_GiveHediffSeverity Props
		{
			get
			{
				return (CompProperties_GiveHediffSeverity)this.props;
			}
		}

		// Token: 0x06010113 RID: 65811 RVA: 0x004A6313 File Offset: 0x004A4513
		private bool AppliesTo(Pawn pawn)
		{
			return pawn.GetRoom(RegionType.Set_All) == this.parent.GetRoom(RegionType.Set_All) && (this.Props.allowMechs || pawn.RaceProps.IsFlesh);
		}

		// Token: 0x06010114 RID: 65812 RVA: 0x004A634C File Offset: 0x004A454C
		public override void CompTick()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			if (!this.parent.IsHashIntervalTick(87))
			{
				return;
			}
			CompRefuelable compRefuelable = this.parent.TryGetComp<CompRefuelable>();
			CompPowerTrader compPowerTrader = this.parent.TryGetComp<CompPowerTrader>();
			if (compRefuelable != null && !compRefuelable.HasFuel)
			{
				return;
			}
			if (compPowerTrader != null && !compPowerTrader.PowerOn)
			{
				return;
			}
			int num = GenRadial.NumCellsInRadius(this.Props.range);
			for (int i = 0; i < num; i++)
			{
				List<Thing> thingList = (this.parent.Position + GenRadial.RadialPattern[i]).GetThingList(this.parent.Map);
				for (int j = 0; j < thingList.Count; j++)
				{
					Pawn pawn = thingList[j] as Pawn;
					if (pawn != null && this.AppliesTo(pawn))
					{
						Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(this.Props.hediff, false);
						float num2 = this.Props.severityPerSecond * 1.45f;
						if (firstHediffOfDef != null)
						{
							firstHediffOfDef.Severity += num2;
						}
						else
						{
							pawn.health.AddHediff(this.Props.hediff, null, null, null).Severity = num2;
						}
						if (this.Props.drugExposure)
						{
							pawn.mindState.lastTakeRecreationalDrugTick = Find.TickManager.TicksGame;
							Pawn_NeedsTracker needs = pawn.needs;
							if (((needs != null) ? needs.drugsDesire : null) != null)
							{
								pawn.needs.drugsDesire.CurLevel += 0.14500001f;
							}
							ChemicalDef chemical = this.Props.chemical;
							if (((chemical != null) ? chemical.addictionHediff : null) != null && pawn.needs != null)
							{
								HediffDef addictionHediffDef = this.Props.chemical.addictionHediff;
								Need need = pawn.needs.AllNeeds.Find((Need x) => x.def == addictionHediffDef.chemicalNeed);
								if (need != null)
								{
									need.CurLevel += 0.14500001f;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0400A7E3 RID: 42979
		private const int UpdateInterval = 87;

		// Token: 0x0400A7E4 RID: 42980
		private const float ChemicalSatisfactionPerSecond = 0.1f;
	}
}
