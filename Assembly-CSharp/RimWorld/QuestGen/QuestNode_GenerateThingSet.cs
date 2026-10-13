using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A31 RID: 14897
	public class QuestNode_GenerateThingSet : QuestNode
	{
		// Token: 0x06014648 RID: 83528 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06014649 RID: 83529 RVA: 0x00617E7C File Offset: 0x0061607C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.totalMarketValueRange = this.totalMarketValueRange.GetValue(slate);
			Thing value = this.factionOf.GetValue(slate);
			parms.makingFaction = ((value == null) ? null : value.Faction);
			parms.qualityGenerator = this.qualityGenerator.GetValue(slate);
			List<Thing> list = this.thingSetMaker.GetValue(slate).root.Generate(parms);
			QuestGen.slate.Set<List<Thing>>(this.storeAs.GetValue(slate), list, false);
			for (int i = 0; i < list.Count; i++)
			{
				Pawn pawn = list[i] as Pawn;
				if (pawn != null)
				{
					QuestGen.AddToGeneratedPawns(pawn);
					if (!pawn.IsWorldPawn())
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					}
				}
			}
		}

		// Token: 0x0400E161 RID: 57697
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E162 RID: 57698
		public SlateRef<ThingSetMakerDef> thingSetMaker;

		// Token: 0x0400E163 RID: 57699
		public SlateRef<FloatRange?> totalMarketValueRange;

		// Token: 0x0400E164 RID: 57700
		public SlateRef<Thing> factionOf;

		// Token: 0x0400E165 RID: 57701
		public SlateRef<QualityGenerator?> qualityGenerator;
	}
}
