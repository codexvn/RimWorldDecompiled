using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000441 RID: 1089
	public sealed class ListerBuildings : IDisposable
	{
		// Token: 0x06001C28 RID: 7208 RVA: 0x0008DD65 File Offset: 0x0008BF65
		public ListerBuildings.TrackingScope Track(Predicate<Building> predicate = null)
		{
			return new ListerBuildings.TrackingScope(this, predicate);
		}

		// Token: 0x06001C29 RID: 7209 RVA: 0x0008DD6E File Offset: 0x0008BF6E
		private void StartTracker(ListerBuildings.TrackingScope scope)
		{
			this.scopes.Add(scope);
		}

		// Token: 0x06001C2A RID: 7210 RVA: 0x0008DD7C File Offset: 0x0008BF7C
		private void ReleaseTracker(ListerBuildings.TrackingScope scope)
		{
			this.scopes.Remove(scope);
		}

		// Token: 0x06001C2B RID: 7211 RVA: 0x0008DD8C File Offset: 0x0008BF8C
		public void Add(Building b)
		{
			if (b.def.building != null && b.def.building.isNaturalRock)
			{
				return;
			}
			if (b.Faction == Faction.OfPlayer)
			{
				this.allBuildingsColonist.Add(b);
				if (b is IAttackTarget)
				{
					this.allBuildingsColonistCombatTargets.Add(b);
				}
			}
			else
			{
				this.allBuildingsNonColonist.Add(b);
			}
			CompProperties_Power compProperties = b.def.GetCompProperties<CompProperties_Power>();
			if (compProperties != null && compProperties.shortCircuitInRain)
			{
				this.allBuildingsColonistElecFire.Add(b);
			}
			if (b.TryGetComp<CompAnimalPenMarker>() != null)
			{
				this.allBuildingsAnimalPenMarkers.Add(b);
			}
			if (b.def == ThingDefOf.CaravanPackingSpot)
			{
				this.allBuildingsHitchingPosts.Add(b);
			}
			foreach (ListerBuildings.TrackingScope trackingScope in this.scopes)
			{
				if ((trackingScope.Enabled && trackingScope.predicate == null) || trackingScope.predicate(b))
				{
					trackingScope.tracked.Add(b);
				}
			}
		}

		// Token: 0x06001C2C RID: 7212 RVA: 0x0008DEB4 File Offset: 0x0008C0B4
		public void Remove(Building b)
		{
			this.allBuildingsColonist.Remove(b);
			this.allBuildingsNonColonist.Remove(b);
			if (b is IAttackTarget)
			{
				this.allBuildingsColonistCombatTargets.Remove(b);
			}
			CompProperties_Power compProperties = b.def.GetCompProperties<CompProperties_Power>();
			if (compProperties != null && compProperties.shortCircuitInRain)
			{
				this.allBuildingsColonistElecFire.Remove(b);
			}
			this.allBuildingsAnimalPenMarkers.Remove(b);
			this.allBuildingsHitchingPosts.Remove(b);
			foreach (ListerBuildings.TrackingScope trackingScope in this.scopes)
			{
				if (trackingScope.Enabled)
				{
					trackingScope.tracked.Remove(b);
				}
			}
		}

		// Token: 0x06001C2D RID: 7213 RVA: 0x0008DF84 File Offset: 0x0008C184
		public void RegisterInstallBlueprint(Blueprint_Install blueprint)
		{
			this.reinstallationMap.Add(blueprint.MiniToInstallOrBuildingToReinstall.GetInnerIfMinified(), blueprint);
		}

		// Token: 0x06001C2E RID: 7214 RVA: 0x0008DFA0 File Offset: 0x0008C1A0
		public void DeregisterInstallBlueprint(Blueprint_Install blueprint)
		{
			Thing miniToInstallOrBuildingToReinstall = blueprint.MiniToInstallOrBuildingToReinstall;
			Thing thing = (miniToInstallOrBuildingToReinstall != null) ? miniToInstallOrBuildingToReinstall.GetInnerIfMinified() : null;
			if (thing != null)
			{
				this.reinstallationMap.Remove(thing);
				return;
			}
			Thing thing2 = null;
			foreach (KeyValuePair<Thing, Blueprint_Install> keyValuePair in this.reinstallationMap)
			{
				if (keyValuePair.Value == blueprint)
				{
					thing2 = keyValuePair.Key;
					break;
				}
			}
			if (thing2 != null)
			{
				this.reinstallationMap.Remove(thing2);
			}
		}

		// Token: 0x06001C2F RID: 7215 RVA: 0x0008E038 File Offset: 0x0008C238
		public bool ColonistsHaveBuilding(ThingDef def)
		{
			for (int i = 0; i < this.allBuildingsColonist.Count; i++)
			{
				if (this.allBuildingsColonist[i].def == def)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001C30 RID: 7216 RVA: 0x0008E074 File Offset: 0x0008C274
		public bool ColonistsHaveBuilding(Func<Thing, bool> predicate)
		{
			for (int i = 0; i < this.allBuildingsColonist.Count; i++)
			{
				if (predicate(this.allBuildingsColonist[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001C31 RID: 7217 RVA: 0x0008E0AE File Offset: 0x0008C2AE
		public bool ColonistsHaveResearchBench()
		{
			return this.AllBuildingsColonistOfClass<Building_ResearchBench>().Any<Building_ResearchBench>();
		}

		// Token: 0x06001C32 RID: 7218 RVA: 0x0008E0BC File Offset: 0x0008C2BC
		public bool ColonistsHaveBuildingWithPowerOn(ThingDef def)
		{
			List<Building> list = this.AllBuildingsColonistOfDef(def);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def == def)
				{
					CompPowerTrader compPowerTrader = list[i].TryGetComp<CompPowerTrader>();
					if (compPowerTrader == null || compPowerTrader.PowerOn)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001C33 RID: 7219 RVA: 0x0008E10C File Offset: 0x0008C30C
		public List<Building> AllBuildingsColonistOfDef(ThingDef def)
		{
			ListerBuildings.allBuildingsColonistOfDefResult.Clear();
			for (int i = 0; i < this.allBuildingsColonist.Count; i++)
			{
				if (this.allBuildingsColonist[i].def == def)
				{
					ListerBuildings.allBuildingsColonistOfDefResult.Add(this.allBuildingsColonist[i]);
				}
			}
			return ListerBuildings.allBuildingsColonistOfDefResult;
		}

		// Token: 0x06001C34 RID: 7220 RVA: 0x0008E168 File Offset: 0x0008C368
		public List<Building> AllBuildingsColonistOfGroup(ThingRequestGroup group)
		{
			ListerBuildings.allBuildingsColonistOfGroupResult.Clear();
			for (int i = 0; i < this.allBuildingsColonist.Count; i++)
			{
				if (group.Includes(this.allBuildingsColonist[i].def))
				{
					ListerBuildings.allBuildingsColonistOfGroupResult.Add(this.allBuildingsColonist[i]);
				}
			}
			return ListerBuildings.allBuildingsColonistOfGroupResult;
		}

		// Token: 0x06001C35 RID: 7221 RVA: 0x0008E1C9 File Offset: 0x0008C3C9
		public IEnumerable<T> AllBuildingsColonistOfClass<T>() where T : Building
		{
			int num;
			for (int i = 0; i < this.allBuildingsColonist.Count; i = num + 1)
			{
				T t = this.allBuildingsColonist[i] as T;
				if (t != null)
				{
					yield return t;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06001C36 RID: 7222 RVA: 0x0008E1D9 File Offset: 0x0008C3D9
		public IEnumerable<T> AllColonistBuildingsOfType<T>()
		{
			int num;
			for (int i = 0; i < this.allBuildingsColonist.Count; i = num + 1)
			{
				Building building = this.allBuildingsColonist[i];
				if (building is T)
				{
					T t = building as T;
					yield return t;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06001C37 RID: 7223 RVA: 0x0008E1E9 File Offset: 0x0008C3E9
		public IEnumerable<Building> AllBuildingsNonColonistOfDef(ThingDef def)
		{
			int num;
			for (int i = 0; i < this.allBuildingsNonColonist.Count; i = num + 1)
			{
				if (this.allBuildingsNonColonist[i].def == def)
				{
					yield return this.allBuildingsNonColonist[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06001C38 RID: 7224 RVA: 0x0008E200 File Offset: 0x0008C400
		public bool TryGetReinstallBlueprint(Thing building, out Blueprint_Install bp)
		{
			return this.reinstallationMap.TryGetValue(building, out bp);
		}

		// Token: 0x06001C39 RID: 7225 RVA: 0x0008E210 File Offset: 0x0008C410
		public void Notify_FactionRemoved(Faction faction)
		{
			for (int i = 0; i < this.allBuildingsNonColonist.Count; i++)
			{
				if (this.allBuildingsNonColonist[i].Faction == faction)
				{
					this.allBuildingsNonColonist[i].SetFaction(null, null);
				}
			}
		}

		// Token: 0x06001C3A RID: 7226 RVA: 0x0008E25C File Offset: 0x0008C45C
		public void Dispose()
		{
			this.allBuildingsColonist.Clear();
			this.allBuildingsNonColonist.Clear();
			this.allBuildingsColonistCombatTargets.Clear();
			this.allBuildingsColonistElecFire.Clear();
			this.allBuildingsAnimalPenMarkers.Clear();
			this.allBuildingsHitchingPosts.Clear();
			this.reinstallationMap.Clear();
		}

		// Token: 0x0400182D RID: 6189
		public readonly List<Building> allBuildingsColonist = new List<Building>();

		// Token: 0x0400182E RID: 6190
		public readonly List<Building> allBuildingsNonColonist = new List<Building>();

		// Token: 0x0400182F RID: 6191
		public readonly HashSet<Building> allBuildingsColonistCombatTargets = new HashSet<Building>();

		// Token: 0x04001830 RID: 6192
		public readonly HashSet<Building> allBuildingsColonistElecFire = new HashSet<Building>();

		// Token: 0x04001831 RID: 6193
		public readonly HashSet<Building> allBuildingsAnimalPenMarkers = new HashSet<Building>();

		// Token: 0x04001832 RID: 6194
		public readonly HashSet<Building> allBuildingsHitchingPosts = new HashSet<Building>();

		// Token: 0x04001833 RID: 6195
		private readonly Dictionary<Thing, Blueprint_Install> reinstallationMap = new Dictionary<Thing, Blueprint_Install>();

		// Token: 0x04001834 RID: 6196
		private readonly List<ListerBuildings.TrackingScope> scopes = new List<ListerBuildings.TrackingScope>();

		// Token: 0x04001835 RID: 6197
		private static List<Building> allBuildingsColonistOfDefResult = new List<Building>();

		// Token: 0x04001836 RID: 6198
		private static List<Building> allBuildingsColonistOfGroupResult = new List<Building>();

		// Token: 0x02000442 RID: 1090
		public class TrackingScope : IDisposable, IEnumerable<Building>, IEnumerable
		{
			// Token: 0x170004F3 RID: 1267
			// (get) Token: 0x06001C3D RID: 7229 RVA: 0x0008E339 File Offset: 0x0008C539
			public bool Enabled
			{
				get
				{
					return this.enabled;
				}
			}

			// Token: 0x06001C3E RID: 7230 RVA: 0x0008E341 File Offset: 0x0008C541
			public TrackingScope(ListerBuildings parent, Predicate<Building> predicate = null)
			{
				this.tracked = SimplePool<HashSet<Building>>.Get();
				this.parent = parent;
				this.predicate = predicate;
				this.Start();
			}

			// Token: 0x06001C3F RID: 7231 RVA: 0x0008E368 File Offset: 0x0008C568
			public void Dispose()
			{
				this.tracked.Clear();
				SimplePool<HashSet<Building>>.Return(this.tracked);
				this.Stop();
			}

			// Token: 0x06001C40 RID: 7232 RVA: 0x0008E386 File Offset: 0x0008C586
			public void Start()
			{
				if (this.enabled)
				{
					return;
				}
				this.enabled = true;
				this.parent.StartTracker(this);
			}

			// Token: 0x06001C41 RID: 7233 RVA: 0x0008E3A4 File Offset: 0x0008C5A4
			public void Stop()
			{
				if (!this.enabled)
				{
					return;
				}
				this.enabled = false;
				this.parent.ReleaseTracker(this);
			}

			// Token: 0x06001C42 RID: 7234 RVA: 0x0008E3C2 File Offset: 0x0008C5C2
			public IEnumerator<Building> GetEnumerator()
			{
				return this.tracked.GetEnumerator();
			}

			// Token: 0x06001C43 RID: 7235 RVA: 0x0008E3C2 File Offset: 0x0008C5C2
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.tracked.GetEnumerator();
			}

			// Token: 0x04001837 RID: 6199
			private bool enabled;

			// Token: 0x04001838 RID: 6200
			private readonly ListerBuildings parent;

			// Token: 0x04001839 RID: 6201
			public readonly HashSet<Building> tracked;

			// Token: 0x0400183A RID: 6202
			public readonly Predicate<Building> predicate;
		}
	}
}
