using System;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002C6C RID: 11372
	public class CompDryadHealingPod : CompDryadHolder
	{
		// Token: 0x0600FEFC RID: 65276 RVA: 0x0049E8AF File Offset: 0x0049CAAF
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				this.innerContainer = new ThingOwner<Thing>(this, false, LookMode.Deep, true);
				this.tickExpire = Find.TickManager.TicksGame + 600;
			}
		}

		// Token: 0x0600FEFD RID: 65277 RVA: 0x0049E8D9 File Offset: 0x0049CAD9
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode != DestroyMode.WillReplace)
			{
				this.innerContainer.TryDropAll(this.parent.Position, map, ThingPlaceMode.Near, delegate(Thing t, int c)
				{
					Pawn pawn = t as Pawn;
					if (pawn != null && pawn.mindState != null)
					{
						pawn.mindState.returnToHealingPod = false;
					}
					t.Rotation = Rot4.South;
					SoundDefOf.Pawn_Dryad_Spawn.PlayOneShot(this.parent);
				}, null, false);
			}
		}

		// Token: 0x0600FEFE RID: 65278 RVA: 0x0049E908 File Offset: 0x0049CB08
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.Destroyed)
			{
				return;
			}
			if (this.innerContainer.Count > 0 && base.TreeComp.ShouldReturnToTree((Pawn)this.innerContainer[0]))
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

		// Token: 0x0600FEFF RID: 65279 RVA: 0x0049E990 File Offset: 0x0049CB90
		public override void TryAcceptPawn(Pawn p)
		{
			base.TryAcceptPawn(p);
			p.Rotation = Rot4.South;
			this.tickComplete = Find.TickManager.TicksGame + (int)(60000f * base.Props.daysToComplete);
			this.tickExpire = -1;
		}

		// Token: 0x0600FF00 RID: 65280 RVA: 0x0049E9D0 File Offset: 0x0049CBD0
		protected override void Complete()
		{
			this.tickComplete = Find.TickManager.TicksGame;
			EffecterDefOf.DryadEmergeFromCocoon.Spawn(this.parent.Position, this.parent.Map, 1f).Cleanup();
			foreach (Thing thing in ((IEnumerable<Thing>)this.innerContainer))
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null)
				{
					pawn.mindState.returnToHealingPod = false;
					int num = 1000;
					Hediff hediff;
					BodyPartRecord bodyPartRecord;
					while (num > 0 && HealthUtility.TryGetWorstHealthCondition(pawn, out hediff, out bodyPartRecord, Array.Empty<HediffDef>()))
					{
						if (hediff != null)
						{
							pawn.health.RemoveHediff(hediff);
						}
						else if (bodyPartRecord != null)
						{
							pawn.health.RestorePart(bodyPartRecord, null, true);
						}
						num--;
					}
				}
			}
			this.parent.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x0400A6FF RID: 42751
		private int tickExpire = -1;
	}
}
