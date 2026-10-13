using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CB2 RID: 7346
	public class QuestPart_SpawnWorldObject : QuestPart
	{
		// Token: 0x1700192D RID: 6445
		// (get) Token: 0x0600A3A9 RID: 41897 RVA: 0x002EF34B File Offset: 0x002ED54B
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.worldObject != null)
				{
					yield return this.worldObject;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x1700192E RID: 6446
		// (get) Token: 0x0600A3AA RID: 41898 RVA: 0x002EF35C File Offset: 0x002ED55C
		public override bool IncreasesPopulation
		{
			get
			{
				Site site = this.worldObject as Site;
				return site != null && site.IncreasesPopulation;
			}
		}

		// Token: 0x0600A3AB RID: 41899 RVA: 0x002EF384 File Offset: 0x002ED584
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && !this.spawned)
			{
				PlanetTile planetTile = this.worldObject.Tile;
				if (!planetTile.Valid)
				{
					if (!TileFinder.TryFindNewSiteTile(out planetTile, 7, 27, false, null, 0.5f, true, TileFinderMode.Near, false, false, null, null))
					{
						planetTile = PlanetTile.Invalid;
					}
				}
				else if (Find.WorldObjects.AnyWorldObjectAt(planetTile))
				{
					if (!TileFinder.TryFindPassableTileWithTraversalDistance(planetTile, 1, 50, out planetTile, (PlanetTile x) => !Find.WorldObjects.AnyWorldObjectAt(x), false, TileFinderMode.Near, false, false))
					{
						planetTile = PlanetTile.Invalid;
					}
				}
				if (planetTile.Valid)
				{
					this.worldObject.Tile = planetTile;
					Find.WorldObjects.Add(this.worldObject);
					this.spawned = true;
				}
			}
		}

		// Token: 0x0600A3AC RID: 41900 RVA: 0x002EF460 File Offset: 0x002ED660
		public override void PostQuestAdded()
		{
			base.PostQuestAdded();
			Site site = this.worldObject as Site;
			if (site != null)
			{
				for (int i = 0; i < site.parts.Count; i++)
				{
					if (site.parts[i].things != null)
					{
						for (int j = 0; j < site.parts[i].things.Count; j++)
						{
							if (site.parts[i].things[j].def == ThingDefOf.PsychicAmplifier)
							{
								Find.History.Notify_PsylinkAvailable();
								return;
							}
						}
					}
				}
			}
		}

		// Token: 0x0600A3AD RID: 41901 RVA: 0x002EF4FA File Offset: 0x002ED6FA
		public override void Cleanup()
		{
			if (!this.spawned)
			{
				this.worldObject.Destroy();
			}
		}

		// Token: 0x0600A3AE RID: 41902 RVA: 0x002EF510 File Offset: 0x002ED710
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving && this.spawned && this.worldObject != null && (this.worldObject.Destroyed || !this.worldObject.Spawned))
			{
				this.worldObject = null;
			}
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<bool>(ref this.spawned, "spawned", false, false);
			Scribe_Collections.Look<ThingDef>(ref this.defsToExcludeFromHyperlinks, "defsToExcludeFromHyperlinks", LookMode.Def, Array.Empty<object>());
			if (this.spawned)
			{
				Scribe_References.Look<WorldObject>(ref this.worldObject, "worldObject", false);
				return;
			}
			Scribe_Deep.Look<WorldObject>(ref this.worldObject, "worldObject", Array.Empty<object>());
		}

		// Token: 0x0600A3AF RID: 41903 RVA: 0x002EF5C8 File Offset: 0x002ED7C8
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			PlanetTile tile;
			if (TileFinder.TryFindNewSiteTile(out tile, 7, 27, false, null, 0.5f, true, TileFinderMode.Near, false, false, null, null))
			{
				this.worldObject = SiteMaker.MakeSite(null, tile, null, true, null, null);
			}
		}

		// Token: 0x04007112 RID: 28946
		public string inSignal;

		// Token: 0x04007113 RID: 28947
		public WorldObject worldObject;

		// Token: 0x04007114 RID: 28948
		public List<ThingDef> defsToExcludeFromHyperlinks;

		// Token: 0x04007115 RID: 28949
		private bool spawned;
	}
}
