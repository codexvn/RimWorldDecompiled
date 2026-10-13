using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B73 RID: 11123
	public class ThingSetMaker_Pawn : ThingSetMaker
	{
		// Token: 0x0600F8BF RID: 63679 RVA: 0x00485488 File Offset: 0x00483688
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(this.pawnKind, null, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
			outThings.Add(pawn);
			if (this.alive != null && !this.alive.Value)
			{
				pawn.Kill(null, null);
				if (this.corpseAgeRangeDays != null)
				{
					int num = Mathf.RoundToInt(this.corpseAgeRangeDays.Value.RandomInRange * 60000f);
					pawn.Corpse.timeOfDeath = Find.TickManager.TicksGame - num;
					CompRottable compRottable = pawn.Corpse.TryGetComp<CompRottable>();
					if (compRottable != null)
					{
						compRottable.RotProgress += (float)num;
					}
				}
			}
		}

		// Token: 0x0600F8C0 RID: 63680 RVA: 0x004855C7 File Offset: 0x004837C7
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			yield return this.pawnKind.race;
			yield break;
		}

		// Token: 0x0600F8C1 RID: 63681 RVA: 0x004855D7 File Offset: 0x004837D7
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.pawnKind == null)
			{
				yield return "pawnKind is null.";
			}
			yield break;
		}

		// Token: 0x0400A397 RID: 41879
		public PawnKindDef pawnKind;

		// Token: 0x0400A398 RID: 41880
		public bool? alive;

		// Token: 0x0400A399 RID: 41881
		public FloatRange? corpseAgeRangeDays;
	}
}
