using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002271 RID: 8817
	public class CompAbilityEffect_ResurrectMech : CompAbilityEffect
	{
		// Token: 0x17001D5B RID: 7515
		// (get) Token: 0x0600C023 RID: 49187 RVA: 0x0037AA87 File Offset: 0x00378C87
		public new CompProperties_ResurrectMech Props
		{
			get
			{
				return (CompProperties_ResurrectMech)this.props;
			}
		}

		// Token: 0x17001D5C RID: 7516
		// (get) Token: 0x0600C024 RID: 49188 RVA: 0x0037AA94 File Offset: 0x00378C94
		public int ChargesRemaining
		{
			get
			{
				return this.resurrectCharges;
			}
		}

		// Token: 0x17001D5D RID: 7517
		// (get) Token: 0x0600C025 RID: 49189 RVA: 0x0037AA9C File Offset: 0x00378C9C
		public override bool CanCast
		{
			get
			{
				return this.resurrectCharges > 0;
			}
		}

		// Token: 0x0600C026 RID: 49190 RVA: 0x0037AAA8 File Offset: 0x00378CA8
		public override void Initialize(AbilityCompProperties props)
		{
			base.Initialize(props);
			this.costsByWeightClass = new Dictionary<MechWeightClassDef, int>();
			for (int i = 0; i < this.Props.costs.Count; i++)
			{
				this.costsByWeightClass[this.Props.costs[i].weightClass] = this.Props.costs[i].cost;
			}
		}

		// Token: 0x0600C027 RID: 49191 RVA: 0x0037AB1C File Offset: 0x00378D1C
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (!base.CanApplyOn(target, dest))
			{
				return false;
			}
			if (target.HasThing)
			{
				Corpse corpse = target.Thing as Corpse;
				if (corpse != null)
				{
					return this.CanResurrect(corpse);
				}
			}
			return false;
		}

		// Token: 0x0600C028 RID: 49192 RVA: 0x0037AB5C File Offset: 0x00378D5C
		private bool TryGetResurrectCost(Corpse corpse, out int cost)
		{
			if (corpse.InnerPawn.RaceProps.mechWeightClass != null && this.costsByWeightClass.ContainsKey(corpse.InnerPawn.RaceProps.mechWeightClass))
			{
				cost = this.costsByWeightClass[corpse.InnerPawn.RaceProps.mechWeightClass];
				return true;
			}
			cost = -1;
			return false;
		}

		// Token: 0x0600C029 RID: 49193 RVA: 0x0037ABBC File Offset: 0x00378DBC
		private bool CanResurrect(Corpse corpse)
		{
			int num;
			return corpse.InnerPawn.RaceProps.IsMechanoid && corpse.InnerPawn.RaceProps.mechWeightClass != null && corpse.InnerPawn.RaceProps.mechWeightClass.canResurrect && corpse.InnerPawn.Faction == this.parent.pawn.Faction && (corpse.InnerPawn.kindDef.abilities == null || !corpse.InnerPawn.kindDef.abilities.Contains(AbilityDefOf.ResurrectionMech)) && corpse.timeOfDeath >= Find.TickManager.TicksGame - this.Props.maxCorpseAgeTicks && this.TryGetResurrectCost(corpse, out num) && num <= this.resurrectCharges;
		}

		// Token: 0x0600C02A RID: 49194 RVA: 0x0037AC8C File Offset: 0x00378E8C
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Corpse corpse = (Corpse)target.Thing;
			if (!this.CanResurrect(corpse))
			{
				return;
			}
			int num;
			if (this.TryGetResurrectCost(corpse, out num))
			{
				Pawn innerPawn = corpse.InnerPawn;
				this.resurrectCharges -= num;
				ResurrectionUtility.TryResurrect(innerPawn, null);
				if (this.Props.appliedEffecterDef != null)
				{
					Effecter effecter = this.Props.appliedEffecterDef.SpawnAttached(innerPawn, innerPawn.MapHeld, 1f);
					effecter.Trigger(innerPawn, innerPawn, -1);
					effecter.Cleanup();
				}
				innerPawn.stances.stagger.StaggerFor(60, 0.17f);
			}
		}

		// Token: 0x0600C02B RID: 49195 RVA: 0x00132D70 File Offset: 0x00130F70
		public override bool GizmoDisabled(out string reason)
		{
			reason = null;
			return false;
		}

		// Token: 0x0600C02C RID: 49196 RVA: 0x0037AD3A File Offset: 0x00378F3A
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.gizmo == null)
			{
				this.gizmo = new Gizmo_MechResurrectionCharges(this);
			}
			yield return this.gizmo;
			if (DebugSettings.ShowDevGizmos)
			{
				Command_Action add = new Command_Action();
				add.defaultLabel = "DEV: Add charge";
				add.action = delegate()
				{
					this.resurrectCharges++;
				};
				yield return add;
				Command_Action command_Action = new Command_Action();
				command_Action.defaultLabel = "DEV: Remove charge";
				command_Action.action = delegate()
				{
					this.resurrectCharges--;
					this.resurrectCharges = Mathf.Max(this.resurrectCharges, 0);
				};
				yield return add;
				add = null;
			}
			yield break;
		}

		// Token: 0x0600C02D RID: 49197 RVA: 0x0037AD4A File Offset: 0x00378F4A
		public override IEnumerable<Mote> CustomWarmupMotes(LocalTargetInfo target)
		{
			foreach (LocalTargetInfo localTargetInfo in this.parent.GetAffectedTargets(target))
			{
				Thing thing = localTargetInfo.Thing;
				yield return MoteMaker.MakeAttachedOverlay(thing, ThingDefOf.Mote_MechResurrectWarmupOnTarget, Vector3.zero, 1f, -1f);
			}
			IEnumerator<LocalTargetInfo> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600C02E RID: 49198 RVA: 0x0037AD64 File Offset: 0x00378F64
		public override void PostApplied(List<LocalTargetInfo> targets, Map map)
		{
			Vector3 vector = Vector3.zero;
			foreach (LocalTargetInfo localTargetInfo in targets)
			{
				vector += localTargetInfo.Cell.ToVector3Shifted();
			}
			vector /= (float)targets.Count;
			IntVec3 intVec = vector.ToIntVec3();
			EffecterDefOf.ApocrionAoeResolve.Spawn(intVec, map, 1f).EffectTick(new TargetInfo(intVec, map, false), new TargetInfo(intVec, map, false));
		}

		// Token: 0x0600C02F RID: 49199 RVA: 0x0037AE04 File Offset: 0x00379004
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.resurrectCharges, "resurrectCharges", 30, false);
		}

		// Token: 0x040083AF RID: 33711
		[LoadAlias("charges")]
		private int resurrectCharges = 30;

		// Token: 0x040083B0 RID: 33712
		private Dictionary<MechWeightClassDef, int> costsByWeightClass;

		// Token: 0x040083B1 RID: 33713
		private Gizmo gizmo;
	}
}
