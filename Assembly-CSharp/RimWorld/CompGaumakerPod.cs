using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C6E RID: 11374
	public class CompGaumakerPod : CompDryadHolder
	{
		// Token: 0x17002831 RID: 10289
		// (get) Token: 0x0600FF0C RID: 65292 RVA: 0x0049EE38 File Offset: 0x0049D038
		public bool Full
		{
			get
			{
				return this.innerContainer.Count >= 3;
			}
		}

		// Token: 0x0600FF0D RID: 65293 RVA: 0x0049EE4B File Offset: 0x0049D04B
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode != DestroyMode.WillReplace && Find.TickManager.TicksGame < this.tickComplete)
			{
				this.innerContainer.TryDropAll(this.parent.Position, map, ThingPlaceMode.Near, null, null, true);
			}
		}

		// Token: 0x0600FF0E RID: 65294 RVA: 0x0049EE7F File Offset: 0x0049D07F
		public override void TryAcceptPawn(Pawn p)
		{
			base.TryAcceptPawn(p);
			if (this.Full)
			{
				this.tickComplete = Find.TickManager.TicksGame + (int)(60000f * base.Props.daysToComplete);
			}
		}

		// Token: 0x0600FF0F RID: 65295 RVA: 0x0049EEB4 File Offset: 0x0049D0B4
		protected override void Complete()
		{
			this.tickComplete = Find.TickManager.TicksGame;
			if (base.TreeComp != null)
			{
				for (int i = this.innerContainer.Count - 1; i >= 0; i--)
				{
					Pawn pawn = this.innerContainer[i] as Pawn;
					if (pawn != null)
					{
						base.TreeComp.RemoveDryad(pawn);
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
					}
				}
				base.TreeComp.gaumakerPod = null;
				((Plant)GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Plant_PodGauranlen, null), this.parent.Position, this.parent.Map, WipeMode.Vanish)).Growth = 1f;
			}
			this.parent.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x0600FF10 RID: 65296 RVA: 0x0049EF70 File Offset: 0x0049D170
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (this.innerContainer.Count < 3)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text = string.Concat(new string[]
				{
					text,
					GenLabel.BestKindLabel(PawnKindDefOf.Dryad_Gaumaker, Gender.Male, true, -1).CapitalizeFirst(),
					": ",
					this.innerContainer.Count.ToString(),
					"/",
					3.ToString()
				});
			}
			return text;
		}
	}
}
