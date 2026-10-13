using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002C45 RID: 11333
	public class CompChimera : ThingComp
	{
		// Token: 0x17002801 RID: 10241
		// (get) Token: 0x0600FE25 RID: 65061 RVA: 0x0049B538 File Offset: 0x00499738
		public CompProperties_Chimera Props
		{
			get
			{
				return (CompProperties_Chimera)this.props;
			}
		}

		// Token: 0x17002802 RID: 10242
		// (get) Token: 0x0600FE26 RID: 65062 RVA: 0x0049B545 File Offset: 0x00499745
		public Pawn Pawn
		{
			get
			{
				return (Pawn)this.parent;
			}
		}

		// Token: 0x0600FE27 RID: 65063 RVA: 0x0049B554 File Offset: 0x00499754
		public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (this.Pawn.Dead)
			{
				return;
			}
			this.totalDamageTaken += totalDamageDealt;
			if (!this.Pawn.Dead && this.totalDamageTaken > 0f && !this.Pawn.health.hediffSet.HasHediff(HediffDefOf.RageSpeed, false) && !this.Pawn.health.Downed)
			{
				this.Pawn.health.AddHediff(HediffMaker.MakeHediff(HediffDefOf.RageSpeed, this.Pawn, null), null, null, null);
				if (this.Pawn.Spawned)
				{
					EffecterDefOf.ChimeraRage.Spawn(this.Pawn.Position, this.Pawn.Map, 1f).Cleanup();
				}
			}
		}

		// Token: 0x0600FE28 RID: 65064 RVA: 0x0049B630 File Offset: 0x00499830
		public override void Notify_Downed()
		{
			Hediff firstHediffOfDef = this.Pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.RageSpeed, false);
			if (firstHediffOfDef != null)
			{
				this.Pawn.health.RemoveHediff(firstHediffOfDef);
			}
		}

		// Token: 0x0600FE29 RID: 65065 RVA: 0x0049B670 File Offset: 0x00499870
		public override void CompTickRare()
		{
			base.CompTickRare();
			if (this.Pawn.Dead)
			{
				return;
			}
			if (this.Pawn.health.summaryHealth.SummaryHealthPercent >= this.Props.rageEndHealthPercentThreshold)
			{
				Hediff firstHediffOfDef = this.Pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.RageSpeed, false);
				if (firstHediffOfDef != null)
				{
					this.Pawn.health.RemoveHediff(firstHediffOfDef);
					this.totalDamageTaken = 0f;
				}
			}
		}

		// Token: 0x0600FE2A RID: 65066 RVA: 0x0049B6EE File Offset: 0x004998EE
		public Thought_MemoryObservation GiveObservedThought(Pawn observer)
		{
			Thought_MemoryObservation_Chimera thought_MemoryObservation_Chimera = (Thought_MemoryObservation_Chimera)ThoughtMaker.MakeThought(ThoughtDefOf.Chimera);
			thought_MemoryObservation_Chimera.Target = this.Pawn;
			return thought_MemoryObservation_Chimera;
		}

		// Token: 0x0600FE2B RID: 65067 RVA: 0x0049B70B File Offset: 0x0049990B
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Switch stalk/attack mode",
					action = delegate()
					{
						Lord lord = this.Pawn.GetLord();
						if (lord == null)
						{
							lord = LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_ChimeraAssault(), this.Pawn.Map, new List<Pawn>
							{
								this.Pawn
							});
							this.Pawn.SetFaction(Faction.OfEntities, null);
						}
						LordJob_ChimeraAssault lordJob_ChimeraAssault = lord.LordJob as LordJob_ChimeraAssault;
						if (lordJob_ChimeraAssault != null)
						{
							lordJob_ChimeraAssault.SwitchMode();
						}
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0600FE2C RID: 65068 RVA: 0x0049B71B File Offset: 0x0049991B
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.totalDamageTaken, "totalDamageTaken", 0f, false);
		}

		// Token: 0x0400A698 RID: 42648
		private float totalDamageTaken;
	}
}
