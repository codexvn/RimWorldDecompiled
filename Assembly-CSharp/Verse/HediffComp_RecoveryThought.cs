using System;

namespace Verse
{
	// Token: 0x020006F6 RID: 1782
	public class HediffComp_RecoveryThought : HediffComp
	{
		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x06002BCF RID: 11215 RVA: 0x000E4482 File Offset: 0x000E2682
		public HediffCompProperties_RecoveryThought Props
		{
			get
			{
				return (HediffCompProperties_RecoveryThought)this.props;
			}
		}

		// Token: 0x06002BD0 RID: 11216 RVA: 0x000E4490 File Offset: 0x000E2690
		public override void CompPostPostRemoved()
		{
			base.CompPostPostRemoved();
			if (!base.Pawn.Dead && base.Pawn.needs.mood != null)
			{
				base.Pawn.needs.mood.thoughts.memories.TryGainMemory(this.Props.thought, null, null);
			}
		}
	}
}
