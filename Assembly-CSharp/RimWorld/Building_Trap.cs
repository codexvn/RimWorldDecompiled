using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002978 RID: 10616
	public abstract class Building_Trap : Building, IPathFindCostProvider
	{
		// Token: 0x1700238B RID: 9099
		// (get) Token: 0x0600E930 RID: 59696 RVA: 0x0043DA9B File Offset: 0x0043BC9B
		private bool CanSetAutoRearm
		{
			get
			{
				return base.Faction == Faction.OfPlayer && this.def.blueprintDef != null && this.def.IsResearchFinished;
			}
		}

		// Token: 0x1700238C RID: 9100
		// (get) Token: 0x0600E931 RID: 59697 RVA: 0x0043E052 File Offset: 0x0043C252
		public virtual bool ShouldShowTrapDamageStat { get; } = 1;

		// Token: 0x1700238D RID: 9101
		// (get) Token: 0x0600E932 RID: 59698 RVA: 0x0043E05C File Offset: 0x0043C25C
		protected bool IsStunned
		{
			get
			{
				if (!this.triedGettingStunner)
				{
					CompStunnable comp = base.GetComp<CompStunnable>();
					this.stunner = ((comp != null) ? comp.StunHandler : null);
					this.triedGettingStunner = true;
				}
				StunHandler stunHandler = this.stunner;
				return stunHandler != null && stunHandler.Stunned;
			}
		}

		// Token: 0x0600E933 RID: 59699 RVA: 0x0043E0A4 File Offset: 0x0043C2A4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.autoRearm, "autoRearm", false, false);
			Scribe_Collections.Look<Pawn>(ref this.touchingPawns, "testees", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.touchingPawns.RemoveAll((Pawn x) => x == null) != 0)
				{
					Log.Error("Removed null pawns from touchingPawns.");
				}
			}
		}

		// Token: 0x0600E934 RID: 59700 RVA: 0x0043E11D File Offset: 0x0043C31D
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad && !base.BeingTransportedOnGravship)
			{
				this.autoRearm = (this.CanSetAutoRearm && map.areaManager.Home[base.Position]);
			}
		}

		// Token: 0x0600E935 RID: 59701 RVA: 0x0043E15C File Offset: 0x0043C35C
		protected override void Tick()
		{
			if (base.Spawned && !this.IsStunned)
			{
				List<Thing> thingList = base.Position.GetThingList(base.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Pawn pawn = thingList[i] as Pawn;
					if (pawn != null && !pawn.Flying && !this.touchingPawns.Contains(pawn))
					{
						this.touchingPawns.Add(pawn);
						this.CheckSpring(pawn);
					}
				}
				for (int j = this.touchingPawns.Count - 1; j >= 0; j--)
				{
					Pawn pawn2 = this.touchingPawns[j];
					if (pawn2 == null || !pawn2.Spawned || pawn2.Flying || pawn2.Position != base.Position)
					{
						this.touchingPawns.Remove(pawn2);
					}
				}
			}
			base.Tick();
		}

		// Token: 0x0600E936 RID: 59702 RVA: 0x0043E240 File Offset: 0x0043C440
		private void CheckSpring(Pawn p)
		{
			if (Rand.Chance(this.SpringChance(p)))
			{
				Map map = base.Map;
				this.Spring(p);
				if (p.Faction == Faction.OfPlayer || p.HostFaction == Faction.OfPlayer)
				{
					Find.LetterStack.ReceiveLetter("LetterFriendlyTrapSprungLabel".Translate(p.LabelShort, p).CapitalizeFirst(), "LetterFriendlyTrapSprung".Translate(p.LabelShort, p).CapitalizeFirst(), LetterDefOf.NegativeEvent, new TargetInfo(base.Position, map, false), null, null, null, null, 0, true);
				}
			}
		}

		// Token: 0x0600E937 RID: 59703 RVA: 0x0043E2F4 File Offset: 0x0043C4F4
		protected virtual float SpringChance(Pawn p)
		{
			float num = 1f;
			if (p.kindDef.immuneToTraps)
			{
				return 0f;
			}
			if (this.KnowsOfTrap(p))
			{
				if (p.Faction == null)
				{
					if (p.IsAnimal)
					{
						num = 0.2f;
						num *= this.def.building.trapPeacefulWildAnimalsSpringChanceFactor;
					}
					else
					{
						num = 0.3f;
					}
				}
				else if (p.Faction == base.Faction)
				{
					num = 0.005f;
				}
				else
				{
					num = 0f;
				}
			}
			num *= this.GetStatValue(StatDefOf.TrapSpringChance, true, -1) * p.GetStatValue(StatDefOf.PawnTrapSpringChance, true, -1);
			return Mathf.Clamp01(num);
		}

		// Token: 0x0600E938 RID: 59704 RVA: 0x0043E398 File Offset: 0x0043C598
		public bool KnowsOfTrap(Pawn p)
		{
			if (p.Faction != null && !p.Faction.HostileTo(base.Faction))
			{
				return true;
			}
			if (p.Faction == null && p.IsAnimal && !p.InAggroMentalState)
			{
				return true;
			}
			if (p.guest != null && p.guest.Released)
			{
				return true;
			}
			if (!p.IsPrisoner && base.Faction != null && p.HostFaction == base.Faction)
			{
				return true;
			}
			if (p.RaceProps.Humanlike && p.IsFormingCaravan())
			{
				return true;
			}
			if (p.IsPrisoner && p.guest.ShouldWaitInsteadOfEscaping && base.Faction == p.HostFaction)
			{
				return true;
			}
			if (p.Faction == null && p.RaceProps.Humanlike)
			{
				return true;
			}
			Lord lord = p.lord;
			ILordAvoidTraps lordAvoidTraps = ((lord != null) ? lord.LordJob : null) as ILordAvoidTraps;
			if (lordAvoidTraps != null)
			{
				Faction faction = p.Faction;
				if (Rand.ValueAsync(HashCode.Combine<int, int>((faction != null) ? faction.loadID : p.lord.loadID, this.thingIDNumber)) < lordAvoidTraps.AvoidTrapRatio)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600E939 RID: 59705 RVA: 0x0043E4B8 File Offset: 0x0043C6B8
		public CellRect GetOccupiedRect()
		{
			return this.OccupiedRect();
		}

		// Token: 0x0600E93A RID: 59706 RVA: 0x0043E4C0 File Offset: 0x0043C6C0
		public ushort PathFindCostFor(Pawn p)
		{
			if (!this.KnowsOfTrap(p))
			{
				return 0;
			}
			return 800;
		}

		// Token: 0x0600E93B RID: 59707 RVA: 0x0043E4D2 File Offset: 0x0043C6D2
		public override ushort PathWalkCostFor(Pawn p)
		{
			if (!this.KnowsOfTrap(p))
			{
				return 0;
			}
			return 40;
		}

		// Token: 0x0600E93C RID: 59708 RVA: 0x0043E4E1 File Offset: 0x0043C6E1
		public override bool IsDangerousFor(Pawn p)
		{
			return this.KnowsOfTrap(p);
		}

		// Token: 0x0600E93D RID: 59709 RVA: 0x0043E4EC File Offset: 0x0043C6EC
		public void Spring(Pawn p)
		{
			bool spawned = base.Spawned;
			Map map = base.Map;
			this.SpringSub(p);
			if (this.def.building.trapDestroyOnSpring)
			{
				if (!base.Destroyed)
				{
					this.Destroy(DestroyMode.Vanish);
				}
				if (spawned)
				{
					this.CheckAutoRebuild(map);
				}
			}
		}

		// Token: 0x0600E93E RID: 59710 RVA: 0x0043E53C File Offset: 0x0043C73C
		public override void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
		{
			bool spawned = base.Spawned;
			Map map = base.Map;
			base.Kill(dinfo, exactCulprit);
			if (spawned)
			{
				this.CheckAutoRebuild(map);
			}
		}

		// Token: 0x0600E93F RID: 59711
		protected abstract void SpringSub(Pawn p);

		// Token: 0x0600E940 RID: 59712 RVA: 0x0043E568 File Offset: 0x0043C768
		private void CheckAutoRebuild(Map map)
		{
			if (this.autoRearm && this.CanSetAutoRearm && map != null && GenConstruct.CanPlaceBlueprintAt(this.def, base.Position, base.Rotation, map, false, null, null, base.Stuff, false, false, false).Accepted)
			{
				GenConstruct.PlaceBlueprintForBuild(this.def, base.Position, map, base.Rotation, Faction.OfPlayer, base.Stuff, null, null, true);
			}
		}

		// Token: 0x0600E941 RID: 59713 RVA: 0x0043E5DD File Offset: 0x0043C7DD
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.CanSetAutoRearm)
			{
				yield return new Command_Toggle
				{
					defaultLabel = "CommandAutoRearm".Translate(),
					defaultDesc = "CommandAutoRearmDesc".Translate(),
					hotKey = KeyBindingDefOf.Misc3,
					icon = TexCommand.RearmTrap,
					isActive = (() => this.autoRearm),
					toggleAction = delegate()
					{
						this.autoRearm = !this.autoRearm;
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x04009B4F RID: 39759
		private bool autoRearm;

		// Token: 0x04009B50 RID: 39760
		private List<Pawn> touchingPawns = new List<Pawn>();

		// Token: 0x04009B51 RID: 39761
		private StunHandler stunner;

		// Token: 0x04009B52 RID: 39762
		private bool triedGettingStunner;

		// Token: 0x04009B53 RID: 39763
		private const float KnowerSpringChanceFactorSameFaction = 0.005f;

		// Token: 0x04009B54 RID: 39764
		private const float KnowerSpringChanceFactorWildAnimal = 0.2f;

		// Token: 0x04009B55 RID: 39765
		private const float KnowerSpringChanceFactorFactionlessHuman = 0.3f;

		// Token: 0x04009B56 RID: 39766
		private const float KnowerSpringChanceFactorOther = 0f;

		// Token: 0x04009B57 RID: 39767
		private const ushort KnowerPathFindCost = 800;

		// Token: 0x04009B58 RID: 39768
		private const ushort KnowerPathWalkCost = 40;
	}
}
