using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02001051 RID: 4177
	public class AttackTargetsCache
	{
		// Token: 0x17000FDE RID: 4062
		// (get) Token: 0x060065D2 RID: 26066 RVA: 0x00204024 File Offset: 0x00202224
		public HashSet<IAttackTarget> TargetsHostileToColony
		{
			get
			{
				return this.TargetsHostileToFaction(Faction.OfPlayer);
			}
		}

		// Token: 0x060065D3 RID: 26067 RVA: 0x00204034 File Offset: 0x00202234
		public AttackTargetsCache(Map map)
		{
			this.map = map;
			map.events.FactionAdded += this.Notify_FactionAdded;
			map.events.FactionRemoved += this.Notify_FactionRemoved;
		}

		// Token: 0x060065D4 RID: 26068 RVA: 0x002040A8 File Offset: 0x002022A8
		public static void AttackTargetsCacheStaticUpdate()
		{
			AttackTargetsCache.targets.Clear();
		}

		// Token: 0x060065D5 RID: 26069 RVA: 0x002040B4 File Offset: 0x002022B4
		public void UpdateTarget(IAttackTarget t)
		{
			if (!this.allTargets.Contains(t))
			{
				return;
			}
			this.DeregisterTarget(t);
			Thing thing = t.Thing;
			if (thing.Spawned && thing.Map == this.map)
			{
				this.RegisterTarget(t);
			}
		}

		// Token: 0x060065D6 RID: 26070 RVA: 0x002040FC File Offset: 0x002022FC
		public List<IAttackTarget> GetPotentialTargetsFor(IAttackTargetSearcher th)
		{
			Thing thing = th.Thing;
			AttackTargetsCache.targets.Clear();
			Faction faction = thing.Faction;
			if (faction != null)
			{
				foreach (IAttackTarget attackTarget in this.TargetsHostileToFaction(faction))
				{
					if (thing.HostileTo(attackTarget.Thing))
					{
						AttackTargetsCache.targets.Add(attackTarget);
					}
				}
			}
			foreach (Pawn pawn in this.pawnsInAggroMentalState)
			{
				if (thing.HostileTo(pawn))
				{
					AttackTargetsCache.targets.Add(pawn);
				}
			}
			foreach (Pawn pawn2 in this.factionlessHumanlikes)
			{
				if (thing.HostileTo(pawn2))
				{
					AttackTargetsCache.targets.Add(pawn2);
				}
			}
			Pawn pawn3 = th as Pawn;
			if (pawn3 != null && PrisonBreakUtility.IsPrisonBreaking(pawn3))
			{
				Faction hostFaction = pawn3.guest.HostFaction;
				List<Pawn> list = this.map.mapPawns.SpawnedPawnsInFaction(hostFaction);
				for (int i = 0; i < list.Count; i++)
				{
					if (thing.HostileTo(list[i]))
					{
						AttackTargetsCache.targets.Add(list[i]);
					}
				}
			}
			if (pawn3 != null && ModsConfig.IdeologyActive && SlaveRebellionUtility.IsRebelling(pawn3))
			{
				Faction faction2 = pawn3.Faction;
				List<Pawn> list2 = this.map.mapPawns.SpawnedPawnsInFaction(faction2);
				for (int j = 0; j < list2.Count; j++)
				{
					if (thing.HostileTo(list2[j]))
					{
						AttackTargetsCache.targets.Add(list2[j]);
					}
				}
			}
			return AttackTargetsCache.targets;
		}

		// Token: 0x060065D7 RID: 26071 RVA: 0x00204300 File Offset: 0x00202500
		public HashSet<IAttackTarget> TargetsHostileToFaction(Faction f)
		{
			if (f == null)
			{
				Log.Warning("Called TargetsHostileToFaction with null faction.");
				return AttackTargetsCache.emptySet;
			}
			HashSet<IAttackTarget> result;
			if (this.targetsHostileToFaction.TryGetValue(f, out result))
			{
				return result;
			}
			return AttackTargetsCache.emptySet;
		}

		// Token: 0x060065D8 RID: 26072 RVA: 0x00204338 File Offset: 0x00202538
		public void Notify_ThingSpawned(Thing th)
		{
			IAttackTarget attackTarget = th as IAttackTarget;
			if (attackTarget != null)
			{
				this.RegisterTarget(attackTarget);
			}
		}

		// Token: 0x060065D9 RID: 26073 RVA: 0x00204358 File Offset: 0x00202558
		public void Notify_ThingDespawned(Thing th)
		{
			IAttackTarget attackTarget = th as IAttackTarget;
			if (attackTarget != null)
			{
				this.DeregisterTarget(attackTarget);
			}
		}

		// Token: 0x060065DA RID: 26074 RVA: 0x00204378 File Offset: 0x00202578
		public void Notify_FactionHostilityChanged(Faction f1, Faction f2)
		{
			AttackTargetsCache.tmpTargets.Clear();
			foreach (IAttackTarget attackTarget in this.allTargets)
			{
				Thing thing = attackTarget.Thing;
				Pawn pawn = thing as Pawn;
				if (thing.Faction == f1 || thing.Faction == f2 || (pawn != null && pawn.HostFaction == f1) || (pawn != null && pawn.HostFaction == f2))
				{
					AttackTargetsCache.tmpTargets.Add(attackTarget);
				}
			}
			for (int i = 0; i < AttackTargetsCache.tmpTargets.Count; i++)
			{
				this.UpdateTarget(AttackTargetsCache.tmpTargets[i]);
			}
			AttackTargetsCache.tmpTargets.Clear();
		}

		// Token: 0x060065DB RID: 26075 RVA: 0x00204448 File Offset: 0x00202648
		private void RegisterTarget(IAttackTarget target)
		{
			if (this.allTargets.Contains(target))
			{
				string str = "Tried to register the same target twice ";
				string str2 = target.ToStringSafe<IAttackTarget>();
				string str3 = " in ";
				Type type = base.GetType();
				Log.Warning(str + str2 + str3 + ((type != null) ? type.ToString() : null));
				return;
			}
			Thing thing = target.Thing;
			if (!thing.Spawned)
			{
				string str4 = "Tried to register unspawned thing ";
				string str5 = thing.ToStringSafe<Thing>();
				string str6 = " in ";
				Type type2 = base.GetType();
				Log.Warning(str4 + str5 + str6 + ((type2 != null) ? type2.ToString() : null));
				return;
			}
			if (thing.Map != this.map)
			{
				Log.Warning("Tried to register attack target " + thing.ToStringSafe<Thing>() + " but its Map is not this one.");
				return;
			}
			this.allTargets.Add(target);
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				if (thing.HostileTo(allFactionsListForReading[i]))
				{
					if (!this.targetsHostileToFaction.ContainsKey(allFactionsListForReading[i]))
					{
						this.targetsHostileToFaction.Add(allFactionsListForReading[i], new HashSet<IAttackTarget>());
					}
					this.targetsHostileToFaction[allFactionsListForReading[i]].Add(target);
				}
			}
			Pawn pawn = target as Pawn;
			if (pawn != null)
			{
				if (pawn.InAggroMentalState)
				{
					this.pawnsInAggroMentalState.Add(pawn);
				}
				if (pawn.Faction == null && pawn.RaceProps.Humanlike)
				{
					this.factionlessHumanlikes.Add(pawn);
				}
			}
		}

		// Token: 0x060065DC RID: 26076 RVA: 0x002045B4 File Offset: 0x002027B4
		private void DeregisterTarget(IAttackTarget target)
		{
			if (!this.allTargets.Contains(target))
			{
				string str = "Tried to deregister ";
				string str2 = (target != null) ? target.ToString() : null;
				string str3 = " but it's not in ";
				Type type = base.GetType();
				Log.Warning(str + str2 + str3 + ((type != null) ? type.ToString() : null));
				return;
			}
			this.allTargets.Remove(target);
			foreach (KeyValuePair<Faction, HashSet<IAttackTarget>> keyValuePair in this.targetsHostileToFaction)
			{
				keyValuePair.Value.Remove(target);
			}
			Pawn pawn = target as Pawn;
			if (pawn != null)
			{
				this.pawnsInAggroMentalState.Remove(pawn);
				this.factionlessHumanlikes.Remove(pawn);
			}
		}

		// Token: 0x060065DD RID: 26077 RVA: 0x00204684 File Offset: 0x00202884
		private void Notify_FactionAdded(Faction faction)
		{
			if (!this.targetsHostileToFaction.ContainsKey(faction))
			{
				this.targetsHostileToFaction.Add(faction, new HashSet<IAttackTarget>());
			}
		}

		// Token: 0x060065DE RID: 26078 RVA: 0x002046A5 File Offset: 0x002028A5
		private void Notify_FactionRemoved(Faction faction)
		{
			if (this.targetsHostileToFaction.ContainsKey(faction))
			{
				this.targetsHostileToFaction.Remove(faction);
			}
		}

		// Token: 0x060065DF RID: 26079 RVA: 0x002046C4 File Offset: 0x002028C4
		private void Debug_AssertHostile(Faction f, HashSet<IAttackTarget> targets)
		{
			AttackTargetsCache.tmpToUpdate.Clear();
			foreach (IAttackTarget attackTarget in targets)
			{
				if (!attackTarget.Thing.HostileTo(f))
				{
					AttackTargetsCache.tmpToUpdate.Add(attackTarget);
					Log.Error(string.Concat(new string[]
					{
						"Target ",
						attackTarget.ToStringSafe<IAttackTarget>(),
						" is not hostile to ",
						f.ToStringSafe<Faction>(),
						" (in ",
						base.GetType().Name,
						") but it's in the list (forgot to update the target somewhere?). Trying to update the target..."
					}));
				}
			}
			for (int i = 0; i < AttackTargetsCache.tmpToUpdate.Count; i++)
			{
				this.UpdateTarget(AttackTargetsCache.tmpToUpdate[i]);
			}
			AttackTargetsCache.tmpToUpdate.Clear();
		}

		// Token: 0x060065E0 RID: 26080 RVA: 0x002047B0 File Offset: 0x002029B0
		public bool Debug_CheckIfInAllTargets(IAttackTarget t)
		{
			return t != null && this.allTargets.Contains(t);
		}

		// Token: 0x060065E1 RID: 26081 RVA: 0x002047C3 File Offset: 0x002029C3
		public bool Debug_CheckIfHostileToFaction(Faction f, IAttackTarget t)
		{
			return f != null && t != null && this.targetsHostileToFaction[f].Contains(t);
		}

		// Token: 0x040048E4 RID: 18660
		private Map map;

		// Token: 0x040048E5 RID: 18661
		private HashSet<IAttackTarget> allTargets = new HashSet<IAttackTarget>();

		// Token: 0x040048E6 RID: 18662
		private Dictionary<Faction, HashSet<IAttackTarget>> targetsHostileToFaction = new Dictionary<Faction, HashSet<IAttackTarget>>();

		// Token: 0x040048E7 RID: 18663
		private HashSet<Pawn> pawnsInAggroMentalState = new HashSet<Pawn>();

		// Token: 0x040048E8 RID: 18664
		private HashSet<Pawn> factionlessHumanlikes = new HashSet<Pawn>();

		// Token: 0x040048E9 RID: 18665
		private static List<IAttackTarget> targets = new List<IAttackTarget>();

		// Token: 0x040048EA RID: 18666
		private static readonly HashSet<IAttackTarget> emptySet = new HashSet<IAttackTarget>();

		// Token: 0x040048EB RID: 18667
		private static readonly List<IAttackTarget> tmpTargets = new List<IAttackTarget>();

		// Token: 0x040048EC RID: 18668
		private static readonly List<IAttackTarget> tmpToUpdate = new List<IAttackTarget>();
	}
}
