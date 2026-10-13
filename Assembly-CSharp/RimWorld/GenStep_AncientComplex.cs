using System;
using RimWorld.BaseGen;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x0200204D RID: 8269
	public class GenStep_AncientComplex : GenStep_LargeRuins
	{
		// Token: 0x17001BAB RID: 7083
		// (get) Token: 0x0600B59B RID: 46491 RVA: 0x003432FA File Offset: 0x003414FA
		public override int SeedPart
		{
			get
			{
				return 235635649;
			}
		}

		// Token: 0x17001BAC RID: 7084
		// (get) Token: 0x0600B59C RID: 46492 RVA: 0x00343301 File Offset: 0x00341501
		private IntVec2 Size
		{
			get
			{
				return new IntVec2(this.structureSketch.structureLayout.container.Width + 10, this.structureSketch.structureLayout.container.Height + 10);
			}
		}

		// Token: 0x17001BAD RID: 7085
		// (get) Token: 0x0600B59D RID: 46493 RVA: 0x00002C42 File Offset: 0x00000E42
		protected override LayoutDef LayoutDef
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001BAE RID: 7086
		// (get) Token: 0x0600B59E RID: 46494 RVA: 0x00343338 File Offset: 0x00341538
		protected override Faction Faction
		{
			get
			{
				return Faction.OfAncients;
			}
		}

		// Token: 0x17001BAF RID: 7087
		// (get) Token: 0x0600B59F RID: 46495 RVA: 0x0001BC84 File Offset: 0x00019E84
		protected override IntRange RuinsMinMaxRange
		{
			get
			{
				return IntRange.One;
			}
		}

		// Token: 0x17001BB0 RID: 7088
		// (get) Token: 0x0600B5A0 RID: 46496 RVA: 0x00002C42 File Offset: 0x00000E42
		protected override TerrainAffordanceDef MinAffordance
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001BB1 RID: 7089
		// (get) Token: 0x0600B5A1 RID: 46497 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool AvoidWaterRoads
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001BB2 RID: 7090
		// (get) Token: 0x0600B5A2 RID: 46498 RVA: 0x00343340 File Offset: 0x00341540
		protected override IntVec2 MinSize
		{
			get
			{
				IntVec2? intVec = this.actualMin;
				if (intVec == null)
				{
					return GenStep_AncientComplex.DefaultComplexSize;
				}
				return intVec.GetValueOrDefault();
			}
		}

		// Token: 0x17001BB3 RID: 7091
		// (get) Token: 0x0600B5A3 RID: 46499 RVA: 0x0034336C File Offset: 0x0034156C
		protected override IntVec2 MaxSize
		{
			get
			{
				IntVec2? intVec = this.actualMax;
				if (intVec == null)
				{
					return new IntVec2(100, 100);
				}
				return intVec.GetValueOrDefault();
			}
		}

		// Token: 0x0600B5A4 RID: 46500 RVA: 0x0034339C File Offset: 0x0034159C
		public override void Generate(Map map, GenStepParams parms)
		{
			this.structureSketch = parms.sitePart.parms.ancientLayoutStructureSketch;
			parms.sitePart.parms.ancientLayoutStructureSketch = null;
			LayoutStructureSketch layoutStructureSketch = this.structureSketch;
			if (((layoutStructureSketch != null) ? layoutStructureSketch.structureLayout : null) == null)
			{
				this.TryRecoverEmptySketch(parms);
			}
			map.layoutStructureSketches.Add(this.structureSketch);
			this.actualMin = (this.actualMax = new IntVec2?(this.Size));
			base.Generate(map, parms);
			this.actualMin = (this.actualMax = null);
		}

		// Token: 0x0600B5A5 RID: 46501 RVA: 0x00343438 File Offset: 0x00341638
		protected override LayoutStructureSketch GenerateAndSpawn(CellRect rect, Map map, GenStepParams parms, LayoutDef layoutDef)
		{
			CellRect container = this.structureSketch.structureLayout.container;
			CellRect rect2;
			if (!rect.TryFindRandomInnerRect(new IntVec2(container.Width, container.Height), out rect2, null))
			{
				rect2 = rect;
				Log.Error(string.Format("Attempted to generate and spawn an anicent complex, but could not find rect of valid size {0} within provided Rect {1}", this.Size, rect));
			}
			ResolveParams resolveParams = new ResolveParams
			{
				ancientLayoutStructureSketch = this.structureSketch,
				threatPoints = new float?(parms.sitePart.parms.threatPoints),
				rect = rect2,
				thingSetMakerDef = parms.sitePart.parms.ancientComplexRewardMaker
			};
			FormCaravanComp component = parms.sitePart.site.GetComponent<FormCaravanComp>();
			if (component != null)
			{
				component.foggedRoomsCheckRect = new CellRect?(resolveParams.rect);
			}
			MapGenerator.UsedRects.Add(resolveParams.rect);
			this.GenerateComplex(map, resolveParams);
			return this.structureSketch;
		}

		// Token: 0x0600B5A6 RID: 46502 RVA: 0x00343530 File Offset: 0x00341730
		private void TryRecoverEmptySketch(GenStepParams parms)
		{
			bool flag = false;
			foreach (Quest quest in Find.QuestManager.QuestsListForReading)
			{
				QuestPart_SpawnWorldObject questPart_SpawnWorldObject;
				if (quest.TryGetFirstPartOfType<QuestPart_SpawnWorldObject>(out questPart_SpawnWorldObject) && questPart_SpawnWorldObject.worldObject == parms.sitePart.site)
				{
					QuestNode_Root_AncientComplex questNode_Root_AncientComplex = quest.root.root as QuestNode_Root_AncientComplex;
					if (questNode_Root_AncientComplex != null)
					{
						this.structureSketch = questNode_Root_AncientComplex.QuestSetupComplex(quest, parms.sitePart.parms.points);
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				return;
			}
			StructureGenParams parms2 = new StructureGenParams
			{
				size = GenStep_AncientComplex.DefaultComplexSize
			};
			this.structureSketch = LayoutDefOf.AncientComplex.Worker.GenerateStructureSketch(parms2);
			Log.Warning("Failed to recover lost complex from any quest. Generating default.");
		}

		// Token: 0x0600B5A7 RID: 46503 RVA: 0x00343610 File Offset: 0x00341810
		protected virtual void GenerateComplex(Map map, ResolveParams parms)
		{
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("ancientComplex", parms, null);
			BaseGen.Generate();
		}

		// Token: 0x04007CA8 RID: 31912
		private LayoutStructureSketch structureSketch;

		// Token: 0x04007CA9 RID: 31913
		private static readonly IntVec2 DefaultComplexSize = new IntVec2(80, 80);

		// Token: 0x04007CAA RID: 31914
		private IntVec2? actualMin;

		// Token: 0x04007CAB RID: 31915
		private IntVec2? actualMax;
	}
}
