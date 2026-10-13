using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001AE6 RID: 6886
	public class SitePartWorker
	{
		// Token: 0x060099DF RID: 39391 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void SitePartWorkerTick(SitePart sitePart)
		{
		}

		// Token: 0x060099E0 RID: 39392 RVA: 0x002CC818 File Offset: 0x002CAA18
		public virtual void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			outExtraDescriptionRules.AddRange(GrammarUtility.RulesForDef("", part.def));
			outExtraDescriptionConstants.Add("sitePart", part.def.defName);
		}

		// Token: 0x060099E1 RID: 39393 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostMapGenerate(Map map)
		{
		}

		// Token: 0x060099E2 RID: 39394 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool FactionCanOwn(Faction faction)
		{
			return true;
		}

		// Token: 0x060099E3 RID: 39395 RVA: 0x002CC847 File Offset: 0x002CAA47
		public virtual string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			preferredLetterDef = this.def.arrivedLetterDef;
			lookTargets = new LookTargets(map.Parent);
			return this.def.arrivedLetter;
		}

		// Token: 0x060099E4 RID: 39396 RVA: 0x002CC86E File Offset: 0x002CAA6E
		public virtual string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			return this.def.label;
		}

		// Token: 0x060099E5 RID: 39397 RVA: 0x002CC87B File Offset: 0x002CAA7B
		public virtual SitePartParams GenerateDefaultParams(float myThreatPoints, PlanetTile tile, Faction faction)
		{
			return new SitePartParams
			{
				randomValue = Rand.Int,
				threatPoints = (this.def.wantsThreatPoints ? myThreatPoints : 0f)
			};
		}

		// Token: 0x060099E6 RID: 39398 RVA: 0x002CC8A8 File Offset: 0x002CAAA8
		public virtual bool IncreasesPopulation(SitePartParams parms)
		{
			return this.def.increasesPopulation;
		}

		// Token: 0x060099E7 RID: 39399 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Init(Site site, SitePart sitePart)
		{
		}

		// Token: 0x060099E8 RID: 39400 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool IsAvailable()
		{
			return true;
		}

		// Token: 0x060099E9 RID: 39401 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostDrawExtraSelectionOverlays(SitePart sitePart)
		{
		}

		// Token: 0x060099EA RID: 39402 RVA: 0x002CC8B8 File Offset: 0x002CAAB8
		public virtual void PostDestroy(SitePart sitePart)
		{
			if (this.def.leaveAbandonedSettlement)
			{
				if (sitePart.site.Tile.LayerDef != PlanetLayerDefOf.Surface)
				{
					return;
				}
				WorldObject worldObject = WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.AbandonedSettlement);
				worldObject.Tile = sitePart.site.Tile;
				worldObject.SetFaction(Faction.OfPlayer);
				Find.WorldObjects.Add(worldObject);
			}
		}

		// Token: 0x060099EB RID: 39403 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_SiteMapAboutToBeRemoved(SitePart sitePart)
		{
		}

		// Token: 0x040069A5 RID: 27045
		public SitePartDef def;
	}
}
