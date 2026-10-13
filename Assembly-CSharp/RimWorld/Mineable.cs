using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200296C RID: 10604
	public class Mineable : Building
	{
		// Token: 0x0600E8D6 RID: 59606 RVA: 0x0043C8EF File Offset: 0x0043AAEF
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.yieldPct, "yieldPct", 0f, false);
		}

		// Token: 0x0600E8D7 RID: 59607 RVA: 0x0043C910 File Offset: 0x0043AB10
		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (absorbed)
			{
				return;
			}
			if (this.def.building.mineableThing != null && this.def.building.mineableYieldWasteable && dinfo.Def == DamageDefOf.Mining && dinfo.Instigator != null && dinfo.Instigator is Pawn)
			{
				this.Notify_TookMiningDamage(GenMath.RoundRandom(dinfo.Amount), (Pawn)dinfo.Instigator);
			}
			absorbed = false;
		}

		// Token: 0x0600E8D8 RID: 59608 RVA: 0x0043C990 File Offset: 0x0043AB90
		public override void Kill(DamageInfo? dinfo, Hediff exactCulprit = null)
		{
			if (dinfo != null && dinfo.Value.Instigator != null && dinfo.Value.Def != DamageDefOf.Mining)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.DestroyedMineable, dinfo.Value.Instigator.Named(HistoryEventArgsNames.Doer)), true);
			}
			base.Kill(dinfo, exactCulprit);
		}

		// Token: 0x0600E8D9 RID: 59609 RVA: 0x0043CA04 File Offset: 0x0043AC04
		public void DestroyMined(Pawn pawn)
		{
			Map map = base.Map;
			base.Destroy(DestroyMode.KillFinalize);
			this.TrySpawnYield(map, true, pawn);
			if (pawn != null)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.Mined, pawn.Named(HistoryEventArgsNames.Doer)), true);
			}
		}

		// Token: 0x0600E8DA RID: 59610 RVA: 0x0043CA4C File Offset: 0x0043AC4C
		public override void Destroy(DestroyMode mode)
		{
			Map map = base.Map;
			base.Destroy(mode);
			if (mode == DestroyMode.KillFinalize)
			{
				this.TrySpawnYield(map, false, null);
			}
		}

		// Token: 0x0600E8DB RID: 59611 RVA: 0x0043CA74 File Offset: 0x0043AC74
		[Obsolete("Will be removeds.")]
		private void TrySpawnYield(Map map, float yieldChance, bool moteOnWaste, Pawn pawn)
		{
			this.TrySpawnYield(map, moteOnWaste, pawn);
		}

		// Token: 0x0600E8DC RID: 59612 RVA: 0x0043CA80 File Offset: 0x0043AC80
		private void TrySpawnYield(Map map, bool moteOnWaste, Pawn pawn)
		{
			Mineable.<>c__DisplayClass7_0 CS$<>8__locals1 = new Mineable.<>c__DisplayClass7_0();
			CS$<>8__locals1.pawn = pawn;
			if (this.def.building.mineableThing == null)
			{
				return;
			}
			if (Rand.Value > this.def.building.mineableDropChance)
			{
				return;
			}
			int num = Mathf.Max(1, this.def.building.EffectiveMineableYield);
			if (this.def.building.mineableYieldWasteable)
			{
				num = Mathf.Max(1, GenMath.RoundRandom((float)num * this.yieldPct));
			}
			Thing thing = ThingMaker.MakeThing(this.def.building.mineableThing, null);
			thing.stackCount = num;
			GenPlace.TryPlaceThing(thing, base.Position, map, ThingPlaceMode.Near, new Action<Thing, int>(CS$<>8__locals1.<TrySpawnYield>g__ForbidIfNecessary|0), null, null, 1);
		}

		// Token: 0x0600E8DD RID: 59613 RVA: 0x0043CB48 File Offset: 0x0043AD48
		public void Notify_TookMiningDamage(int amount, Pawn miner)
		{
			float num = (float)Mathf.Min(amount, this.HitPoints) / (float)base.MaxHitPoints;
			this.yieldPct += num * miner.GetStatValue(StatDefOf.MiningYield, true, -1);
		}

		// Token: 0x04009B0E RID: 39694
		private float yieldPct;
	}
}
