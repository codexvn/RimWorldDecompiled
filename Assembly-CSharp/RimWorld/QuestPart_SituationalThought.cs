using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CAD RID: 7341
	public class QuestPart_SituationalThought : QuestPartActivable
	{
		// Token: 0x17001925 RID: 6437
		// (get) Token: 0x0600A385 RID: 41861 RVA: 0x002EE974 File Offset: 0x002ECB74
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.pawn != null)
				{
					yield return this.pawn;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600A386 RID: 41862 RVA: 0x002EE984 File Offset: 0x002ECB84
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThoughtDef>(ref this.def, "def");
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_Values.Look<int>(ref this.stage, "stage", 0, false);
			Scribe_Values.Look<int>(ref this.delayTicks, "delayTicks", 0, false);
		}

		// Token: 0x0600A387 RID: 41863 RVA: 0x002EE9DC File Offset: 0x002ECBDC
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.def = ThoughtDefOf.DecreeUnmet;
			this.pawn = PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>();
		}

		// Token: 0x0600A388 RID: 41864 RVA: 0x002EE9FF File Offset: 0x002ECBFF
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.pawn == replace)
			{
				this.pawn = with;
			}
		}

		// Token: 0x040070F7 RID: 28919
		public ThoughtDef def;

		// Token: 0x040070F8 RID: 28920
		public Pawn pawn;

		// Token: 0x040070F9 RID: 28921
		public int stage;

		// Token: 0x040070FA RID: 28922
		public int delayTicks;
	}
}
