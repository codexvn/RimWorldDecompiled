using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02003042 RID: 12354
	public class Alert_MinifiedTreeAboutToDie : Alert
	{
		// Token: 0x17002CF5 RID: 11509
		// (get) Token: 0x060115C4 RID: 71108 RVA: 0x004FAFE4 File Offset: 0x004F91E4
		private List<Pawn> ColonistsBotheredByTreesDying
		{
			get
			{
				this.colonistsBotheredByTreesDyingResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					foreach (Pawn pawn in maps[i].mapPawns.FreeColonistsSpawned)
					{
						if (pawn.Ideo != null && pawn.Ideo.WarnPlayerOnDesignateChopTree)
						{
							this.colonistsBotheredByTreesDyingResult.Add(pawn);
						}
					}
				}
				return this.colonistsBotheredByTreesDyingResult;
			}
		}

		// Token: 0x17002CF6 RID: 11510
		// (get) Token: 0x060115C5 RID: 71109 RVA: 0x004FB084 File Offset: 0x004F9284
		private List<Thing> TreesAboutToDie
		{
			get
			{
				this.treesAboutToDieResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					this.<get_TreesAboutToDie>g__CheckForTrees|6_0(maps[i].listerThings.ThingsOfDef(ThingDefOf.MinifiedTree));
					foreach (Thing thing in maps[i].listerThings.ThingsInGroup(ThingRequestGroup.ThingHolder))
					{
						ThingOwner thingOwner = thing.TryGetInnerInteractableThingOwner();
						if (thingOwner != null)
						{
							this.<get_TreesAboutToDie>g__CheckForTrees|6_0(thingOwner);
						}
					}
				}
				foreach (Caravan caravan in Find.WorldObjects.Caravans)
				{
					this.<get_TreesAboutToDie>g__CheckForTrees|6_0(CaravanInventoryUtility.AllInventoryItems(caravan));
				}
				return this.treesAboutToDieResult;
			}
		}

		// Token: 0x060115C6 RID: 71110 RVA: 0x004FB180 File Offset: 0x004F9380
		public override string GetLabel()
		{
			return "AlertMinifiedTreeAboutToDie".Translate(this.TreesAboutToDie.Count.ToStringCached());
		}

		// Token: 0x060115C7 RID: 71111 RVA: 0x004FB1A8 File Offset: 0x004F93A8
		public override TaggedString GetExplanation()
		{
			this.sb.Length = 0;
			foreach (Pawn pawn in this.ColonistsBotheredByTreesDying)
			{
				this.sb.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "AlertMinifiedTreeAboutToDieDesc".Translate(this.TreesAboutToDie.Count.ToStringCached()) + ":\n" + this.sb.ToString().TrimEndNewlines();
		}

		// Token: 0x060115C8 RID: 71112 RVA: 0x004FB264 File Offset: 0x004F9464
		public override AlertReport GetReport()
		{
			List<Thing> treesAboutToDie = this.TreesAboutToDie;
			if (treesAboutToDie.Count == 0 || this.ColonistsBotheredByTreesDying.Count == 0)
			{
				return false;
			}
			return AlertReport.CulpritsAre(treesAboutToDie);
		}

		// Token: 0x060115CA RID: 71114 RVA: 0x004FB2C4 File Offset: 0x004F94C4
		[CompilerGenerated]
		private void <get_TreesAboutToDie>g__CheckForTrees|6_0(IList<Thing> things)
		{
			for (int i = 0; i < things.Count; i++)
			{
				MinifiedTree minifiedTree = things[i] as MinifiedTree;
				if (minifiedTree != null && minifiedTree.TicksTillDeath <= 60000)
				{
					this.treesAboutToDieResult.Add(minifiedTree);
				}
			}
		}

		// Token: 0x0400B3C6 RID: 46022
		public const int TicksLeftThreshold = 60000;

		// Token: 0x0400B3C7 RID: 46023
		private List<Pawn> colonistsBotheredByTreesDyingResult = new List<Pawn>();

		// Token: 0x0400B3C8 RID: 46024
		private List<Thing> treesAboutToDieResult = new List<Thing>();

		// Token: 0x0400B3C9 RID: 46025
		private StringBuilder sb = new StringBuilder();
	}
}
