using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C4C RID: 11340
	public class CompDamageOnInterval : ThingComp
	{
		// Token: 0x17002809 RID: 10249
		// (get) Token: 0x0600FE46 RID: 65094 RVA: 0x0049BB4B File Offset: 0x00499D4B
		public CompProperties_DamageOnInterval Props
		{
			get
			{
				return (CompProperties_DamageOnInterval)this.props;
			}
		}

		// Token: 0x0600FE47 RID: 65095 RVA: 0x0049BB58 File Offset: 0x00499D58
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.ticksToDamage = this.Props.ticksBetweenDamage;
		}

		// Token: 0x0600FE48 RID: 65096 RVA: 0x0049BB72 File Offset: 0x00499D72
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksToDamage, "ticksToPlantHarm", 0, false);
		}

		// Token: 0x0600FE49 RID: 65097 RVA: 0x0049BB8C File Offset: 0x00499D8C
		public override void CompTick()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			if (this.Props.startHitPointsPercent > 0f && (float)this.parent.HitPoints > this.Props.startHitPointsPercent * (float)this.parent.MaxHitPoints)
			{
				return;
			}
			this.ticksToDamage--;
			if (this.ticksToDamage <= 0)
			{
				this.ticksToDamage = this.Props.ticksBetweenDamage;
				this.Damage();
			}
			EffecterDef effecterDef = null;
			foreach (DamageEffectStage damageEffectStage in this.Props.effectStages)
			{
				if (damageEffectStage.minHitPointsPercent * (float)this.parent.MaxHitPoints > (float)this.parent.HitPoints)
				{
					effecterDef = damageEffectStage.effecterDef;
				}
			}
			if (effecterDef != null)
			{
				EffecterDef effecterDef2 = effecterDef;
				Effecter effecter = this.effecter;
				if (effecterDef2 != ((effecter != null) ? effecter.def : null))
				{
					Effecter effecter2 = this.effecter;
					if (effecter2 != null)
					{
						effecter2.Cleanup();
					}
					this.effecter = effecterDef.SpawnAttached(this.parent, this.parent.MapHeld, 1f);
				}
			}
			Effecter effecter3 = this.effecter;
			if (effecter3 == null)
			{
				return;
			}
			effecter3.EffectTick(this.parent, this.parent);
		}

		// Token: 0x0600FE4A RID: 65098 RVA: 0x0049BCF0 File Offset: 0x00499EF0
		public void Damage()
		{
			this.parent.TakeDamage(new DamageInfo(this.Props.damageDef ?? DamageDefOf.Deterioration, this.Props.damage, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
		}

		// Token: 0x0600FE4B RID: 65099 RVA: 0x0049BD41 File Offset: 0x00499F41
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Damage",
					action = new Action(this.Damage)
				};
			}
			yield break;
		}

		// Token: 0x0400A6AA RID: 42666
		private int ticksToDamage;

		// Token: 0x0400A6AB RID: 42667
		private Effecter effecter;
	}
}
