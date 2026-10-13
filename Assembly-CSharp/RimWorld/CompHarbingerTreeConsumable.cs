using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CD7 RID: 11479
	public class CompHarbingerTreeConsumable : ThingComp
	{
		// Token: 0x170028CE RID: 10446
		// (get) Token: 0x060101F8 RID: 66040 RVA: 0x004AA538 File Offset: 0x004A8738
		public bool BeingConsumed
		{
			get
			{
				return !this.tmpTreesInRange.Empty<HarbingerTree>();
			}
		}

		// Token: 0x170028CF RID: 10447
		// (get) Token: 0x060101F9 RID: 66041 RVA: 0x004AA548 File Offset: 0x004A8748
		private Corpse Corpse
		{
			get
			{
				return this.parent as Corpse;
			}
		}

		// Token: 0x170028D0 RID: 10448
		// (get) Token: 0x060101FA RID: 66042 RVA: 0x004AA558 File Offset: 0x004A8758
		public bool CanBeConsumed
		{
			get
			{
				if (!ModsConfig.AnomalyActive)
				{
					return false;
				}
				if (!this.parent.Spawned)
				{
					return false;
				}
				SlotGroup slotGroup = this.parent.GetSlotGroup();
				return slotGroup == null || slotGroup.parent.Isnt<Building>();
			}
		}

		// Token: 0x060101FB RID: 66043 RVA: 0x004AA599 File Offset: 0x004A8799
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksPassed, "ticksPassed", 0, false);
		}

		// Token: 0x060101FC RID: 66044 RVA: 0x004AA5AD File Offset: 0x004A87AD
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			LongEventHandler.ExecuteWhenFinished(new Action(this.ConsumedRootsLazy));
		}

		// Token: 0x060101FD RID: 66045 RVA: 0x004AA5C0 File Offset: 0x004A87C0
		private void ConsumedRootsLazy()
		{
			if (!this.CanBeConsumed)
			{
				return;
			}
			this.tmpTreesInRange.Clear();
			foreach (Thing thing in this.parent.Map.listerThings.ThingsOfDef(ThingDefOf.Plant_TreeHarbinger))
			{
				HarbingerTree harbingerTree = thing as HarbingerTree;
				if (harbingerTree.Position.InHorDistOf(this.parent.Position, harbingerTree.ConsumeRadius))
				{
					this.tmpTreesInRange.Add(harbingerTree);
				}
			}
		}

		// Token: 0x060101FE RID: 66046 RVA: 0x004AA668 File Offset: 0x004A8868
		public override void CompTick()
		{
			if (this.parent.IsHashIntervalTick(250))
			{
				this.CompTickRare();
			}
		}

		// Token: 0x060101FF RID: 66047 RVA: 0x004AA684 File Offset: 0x004A8884
		public override void CompTickRare()
		{
			if (!this.CanBeConsumed)
			{
				return;
			}
			this.ConsumedRootsLazy();
			if (this.BeingConsumed)
			{
				this.ticksPassed += 250;
				if (this.AvailableNutrition(false) > 0f)
				{
					EffecterDefOf.HarbingerTreeWillConsume.Spawn(this.parent.Position, this.parent.Map, 1f);
				}
				if (this.ticksPassed >= 5000)
				{
					Map map = this.parent.Map;
					if (this.TryConsume())
					{
						EffecterDefOf.HarbingerTreeConsume.Spawn(this.parent.Position, map, 1f);
					}
					this.ticksPassed -= 5000;
				}
			}
		}

		// Token: 0x06010200 RID: 66048 RVA: 0x004AA740 File Offset: 0x004A8940
		public float AvailableNutrition(bool applyDigestion)
		{
			if (this.Corpse != null && !this.Corpse.InnerPawn.RaceProps.IsFlesh)
			{
				return 0f;
			}
			float result;
			if (this.Corpse != null)
			{
				result = CompHarbingerTreeConsumable.GetNutritionFromCorpse(this.Corpse, applyDigestion);
			}
			else if (this.parent.def.category == ThingCategory.Item)
			{
				result = CompHarbingerTreeConsumable.GetNutritionFromItemStack(this.parent, applyDigestion);
			}
			else
			{
				result = 1f;
			}
			return result;
		}

		// Token: 0x06010201 RID: 66049 RVA: 0x004AA7B8 File Offset: 0x004A89B8
		private bool TryConsume()
		{
			float num = this.AvailableNutrition(true);
			if (num == 0f)
			{
				return false;
			}
			foreach (HarbingerTree harbingerTree in this.tmpTreesInRange)
			{
				harbingerTree.AddNutrition(num / (float)this.tmpTreesInRange.Count);
			}
			return true;
		}

		// Token: 0x06010202 RID: 66050 RVA: 0x004AA82C File Offset: 0x004A8A2C
		private static float GetNutritionFromCorpse(Corpse corpse, bool applyDigestion)
		{
			BodyPartRecord bodyPartRecord;
			(from x in corpse.InnerPawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
			where x.depth == BodyPartDepth.Outside && !x.def.conceptual && x != corpse.InnerPawn.RaceProps.body.corePart
			select x).TryRandomElement(out bodyPartRecord);
			float bodyPartNutrition;
			if (bodyPartRecord == null)
			{
				bodyPartNutrition = FoodUtility.GetBodyPartNutrition(corpse, corpse.InnerPawn.RaceProps.body.corePart);
				if (applyDigestion)
				{
					corpse.Destroy(DestroyMode.Vanish);
				}
			}
			else
			{
				bodyPartNutrition = FoodUtility.GetBodyPartNutrition(corpse, bodyPartRecord);
				if (applyDigestion)
				{
					Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, corpse.InnerPawn, bodyPartRecord);
					hediff_MissingPart.IsFresh = true;
					hediff_MissingPart.lastInjury = HediffDefOf.Digested;
					corpse.InnerPawn.health.AddHediff(hediff_MissingPart, null, null, null);
				}
			}
			return bodyPartNutrition;
		}

		// Token: 0x06010203 RID: 66051 RVA: 0x004AA918 File Offset: 0x004A8B18
		private static float GetNutritionFromItemStack(ThingWithComps item, bool applyDigestion)
		{
			int num = Mathf.Min(item.stackCount, CompHarbingerTreeConsumable.ItemConsumeCountRange.RandomInRange);
			float result;
			if (applyDigestion)
			{
				Thing thing = item.SplitOff(num);
				result = thing.GetStatValue(StatDefOf.Nutrition, true, -1) * (float)num;
				thing.Destroy(DestroyMode.Vanish);
			}
			else
			{
				result = item.GetStatValue(StatDefOf.Nutrition, true, -1) * ((float)num / (float)item.stackCount);
			}
			return result;
		}

		// Token: 0x06010204 RID: 66052 RVA: 0x004AA981 File Offset: 0x004A8B81
		public override string CompInspectStringExtra()
		{
			if (this.BeingConsumed)
			{
				return "ConsumedByHarbingerTree".Translate();
			}
			return "";
		}

		// Token: 0x0400A880 RID: 43136
		private const int ConsumeInterval = 5000;

		// Token: 0x0400A881 RID: 43137
		private static readonly IntRange ItemConsumeCountRange = new IntRange(4, 12);

		// Token: 0x0400A882 RID: 43138
		private int ticksPassed;

		// Token: 0x0400A883 RID: 43139
		private readonly List<HarbingerTree> tmpTreesInRange = new List<HarbingerTree>();
	}
}
