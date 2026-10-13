using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002C90 RID: 11408
	public class CompFleshmass : ThingComp, ISizeReporter
	{
		// Token: 0x1700286B RID: 10347
		// (get) Token: 0x06010002 RID: 65538 RVA: 0x004A269B File Offset: 0x004A089B
		public CompProperties_Fleshmass Props
		{
			get
			{
				return (CompProperties_Fleshmass)this.props;
			}
		}

		// Token: 0x06010003 RID: 65539 RVA: 0x0004FC72 File Offset: 0x0004DE72
		public float CurrentSize()
		{
			return 1f;
		}

		// Token: 0x06010004 RID: 65540 RVA: 0x004A26A8 File Offset: 0x004A08A8
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_References.Look<Thing>(ref this.source, "source", false);
		}

		// Token: 0x06010005 RID: 65541 RVA: 0x004A26C4 File Offset: 0x004A08C4
		public override void CompTickRare()
		{
			if (this.sustainer == null && !this.parent.Position.Fogged(this.parent.Map))
			{
				SoundInfo info = SoundInfo.InMap(new TargetInfo(this.parent.Position, this.parent.Map, false), MaintenanceType.PerTickRare);
				this.sustainer = SustainerAggregatorUtility.AggregateOrSpawnSustainerFor(this, SoundDefOf.FleshmassAmbience, info);
			}
			Sustainer sustainer = this.sustainer;
			if (sustainer == null)
			{
				return;
			}
			sustainer.Maintain();
		}

		// Token: 0x06010006 RID: 65542 RVA: 0x004A273C File Offset: 0x004A093C
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.sustainer != null)
			{
				if (this.sustainer.externalParams.sizeAggregator == null)
				{
					this.sustainer.externalParams.sizeAggregator = new SoundSizeAggregator();
				}
				this.sustainer.externalParams.sizeAggregator.RemoveReporter(this);
			}
			this.sustainer = null;
		}

		// Token: 0x06010007 RID: 65543 RVA: 0x004A2798 File Offset: 0x004A0998
		public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
		{
			bool flag;
			if (dinfo == null)
			{
				flag = (null != null);
			}
			else
			{
				Thing instigator = dinfo.GetValueOrDefault().Instigator;
				flag = (((instigator != null) ? instigator.Faction : null) != null);
			}
			if ((!flag || dinfo.Value.Instigator.Faction == Faction.OfPlayer) && this.source != null && this.source.Spawned)
			{
				CompGrowsFleshmassTendrils compGrowsFleshmassTendrils = this.source.TryGetComp<CompGrowsFleshmassTendrils>();
				if (compGrowsFleshmassTendrils == null)
				{
					return;
				}
				compGrowsFleshmassTendrils.Notify_FleshmassDestroyedByPlayer(this.parent);
			}
		}

		// Token: 0x06010008 RID: 65544 RVA: 0x004A281E File Offset: 0x004A0A1E
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			CompGrowsFleshmassTendrils compGrowsFleshmassTendrils = this.source.TryGetComp<CompGrowsFleshmassTendrils>();
			if (compGrowsFleshmassTendrils == null)
			{
				return;
			}
			compGrowsFleshmassTendrils.Notify_FleshmassDestroyed(this.parent);
		}

		// Token: 0x06010009 RID: 65545 RVA: 0x004A283C File Offset: 0x004A0A3C
		public override string CompInspectStringExtra()
		{
			Thing thing = this.source;
			CompGrowsFleshmassTendrils compGrowsFleshmassTendrils = (thing != null) ? thing.TryGetComp<CompGrowsFleshmassTendrils>() : null;
			if (this.parent.def == ThingDefOf.Fleshmass_Active && compGrowsFleshmassTendrils != null && compGrowsFleshmassTendrils.Props.fleshbeastBirthThresholdRange != null)
			{
				return "ActiveFleshmassInspect".Translate();
			}
			return "";
		}

		// Token: 0x0400A766 RID: 42854
		public Thing source;

		// Token: 0x0400A767 RID: 42855
		private Sustainer sustainer;
	}
}
