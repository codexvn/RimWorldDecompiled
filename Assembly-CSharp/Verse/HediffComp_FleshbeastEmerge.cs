using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020006B5 RID: 1717
	public class HediffComp_FleshbeastEmerge : HediffComp
	{
		// Token: 0x17000786 RID: 1926
		// (get) Token: 0x06002AF4 RID: 10996 RVA: 0x000E1220 File Offset: 0x000DF420
		public HediffCompProperties_FleshbeastEmerge Props
		{
			get
			{
				return (HediffCompProperties_FleshbeastEmerge)this.props;
			}
		}

		// Token: 0x06002AF5 RID: 10997 RVA: 0x000E122D File Offset: 0x000DF42D
		public override void Notify_SurgicallyRemoved(Pawn surgeon)
		{
			this.TentacleAttack(surgeon);
		}

		// Token: 0x06002AF6 RID: 10998 RVA: 0x000E122D File Offset: 0x000DF42D
		public override void Notify_SurgicallyReplaced(Pawn surgeon)
		{
			this.TentacleAttack(surgeon);
		}

		// Token: 0x06002AF7 RID: 10999 RVA: 0x000E1238 File Offset: 0x000DF438
		private void TentacleAttack(Pawn surgeon)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return;
			}
			Pawn pawn = this.parent.pawn;
			PawnKindDef fingerspike = PawnKindDefOf.Fingerspike;
			Faction ofEntities = Faction.OfEntities;
			PawnGenerationContext context = PawnGenerationContext.NonPlayer;
			float? fixedBiologicalAge = new float?(0f);
			float? fixedChronologicalAge = new float?(0f);
			Pawn pawn2 = PawnGenerator.GeneratePawn(new PawnGenerationRequest(fingerspike, ofEntities, context, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, fixedBiologicalAge, fixedChronologicalAge, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
			GenSpawn.Spawn(pawn2, CellFinder.StandableCellNear(pawn.Position, pawn.Map, 2f, null), pawn.Map, WipeMode.Vanish);
			pawn2.stances.stunner.StunFor(HediffComp_FleshbeastEmerge.StunDuration.RandomInRange, surgeon, true, true, false);
			CompInspectStringEmergence compInspectStringEmergence = pawn2.TryGetComp<CompInspectStringEmergence>();
			if (compInspectStringEmergence != null)
			{
				compInspectStringEmergence.sourcePawn = pawn;
			}
			TaggedString label = this.Props.letterLabel.Formatted(pawn.Named("PAWN"));
			TaggedString text = this.Props.letterText.Formatted(pawn.Named("PAWN"));
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.ThreatBig, pawn2, null, null, null, null, 0, true);
		}

		// Token: 0x040022AC RID: 8876
		private static readonly IntRange StunDuration = new IntRange(120, 240);
	}
}
