using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002C6D RID: 11373
	public class CompDryadCocoon : CompDryadHolder
	{
		// Token: 0x0600FF03 RID: 65283 RVA: 0x0049EB13 File Offset: 0x0049CD13
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				this.innerContainer = new ThingOwner<Thing>(this, false, LookMode.Deep, true);
				this.tickExpire = Find.TickManager.TicksGame + 600;
			}
		}

		// Token: 0x0600FF04 RID: 65284 RVA: 0x0049EB40 File Offset: 0x0049CD40
		public override void TryAcceptPawn(Pawn p)
		{
			base.TryAcceptPawn(p);
			p.Rotation = Rot4.South;
			this.tickComplete = Find.TickManager.TicksGame + (int)(60000f * base.Props.daysToComplete);
			this.tickExpire = -1;
			this.dryadKind = base.TreeComp.DryadKind;
		}

		// Token: 0x0600FF05 RID: 65285 RVA: 0x0049EB9C File Offset: 0x0049CD9C
		protected override void Complete()
		{
			this.tickComplete = Find.TickManager.TicksGame;
			CompTreeConnection treeComp = base.TreeComp;
			if (treeComp != null && this.innerContainer.Count > 0)
			{
				Pawn pawn = (Pawn)this.innerContainer[0];
				long ageBiologicalTicks = pawn.ageTracker.AgeBiologicalTicks;
				treeComp.RemoveDryad(pawn);
				Pawn pawn2 = treeComp.GenerateNewDryad(this.dryadKind);
				pawn2.ageTracker.AgeBiologicalTicks = ageBiologicalTicks;
				if (!pawn.Name.Numerical)
				{
					pawn2.Name = pawn.Name;
				}
				pawn.Destroy(DestroyMode.Vanish);
				this.innerContainer.TryAddOrTransfer(pawn2, 1, true);
				EffecterDefOf.DryadEmergeFromCocoon.Spawn(this.parent.Position, this.parent.Map, 1f).Cleanup();
			}
			this.parent.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x0600FF06 RID: 65286 RVA: 0x0049EC7A File Offset: 0x0049CE7A
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode != DestroyMode.WillReplace)
			{
				this.innerContainer.TryDropAll(this.parent.Position, map, ThingPlaceMode.Near, delegate(Thing t, int c)
				{
					t.Rotation = Rot4.South;
					SoundDefOf.Pawn_Dryad_Spawn.PlayOneShot(this.parent);
				}, null, false);
			}
		}

		// Token: 0x0600FF07 RID: 65287 RVA: 0x0049ECA8 File Offset: 0x0049CEA8
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.Destroyed)
			{
				return;
			}
			if (this.dryadKind != null && this.dryadKind != base.TreeComp.DryadKind)
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			if (this.innerContainer.Count > 0 && this.tree != null && base.TreeComp.ShouldReturnToTree((Pawn)this.innerContainer[0]))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			if (this.tickExpire >= 0 && Find.TickManager.TicksGame >= this.tickExpire)
			{
				this.tickExpire = -1;
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600FF08 RID: 65288 RVA: 0x0049ED60 File Offset: 0x0049CF60
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!this.innerContainer.NullOrEmpty<Thing>() && this.dryadKind != null)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += "ChangingDryadIntoType".Translate(this.innerContainer[0].Named("DRYAD"), this.dryadKind.Named("TYPE")).Resolve();
			}
			return text;
		}

		// Token: 0x0600FF09 RID: 65289 RVA: 0x0049EDDD File Offset: 0x0049CFDD
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.tickExpire, "tickExpire", -1, false);
			Scribe_Defs.Look<PawnKindDef>(ref this.dryadKind, "dryadKind");
		}

		// Token: 0x0400A700 RID: 42752
		private int tickExpire = -1;

		// Token: 0x0400A701 RID: 42753
		private PawnKindDef dryadKind;
	}
}
