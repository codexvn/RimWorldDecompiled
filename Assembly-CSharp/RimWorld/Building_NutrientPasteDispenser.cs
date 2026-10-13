using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002A39 RID: 10809
	public class Building_NutrientPasteDispenser : Building
	{
		// Token: 0x17002554 RID: 9556
		// (get) Token: 0x0600EFE8 RID: 61416 RVA: 0x00459E77 File Offset: 0x00458077
		public bool CanDispenseNow
		{
			get
			{
				return this.powerComp.PowerOn && this.HasEnoughFeedstockInHoppers();
			}
		}

		// Token: 0x17002555 RID: 9557
		// (get) Token: 0x0600EFE9 RID: 61417 RVA: 0x00459E8E File Offset: 0x0045808E
		public List<IntVec3> AdjCellsCardinalInBounds
		{
			get
			{
				if (this.cachedAdjCellsCardinal == null)
				{
					this.cachedAdjCellsCardinal = (from c in GenAdj.CellsAdjacentCardinal(this)
					where c.InBounds(base.Map)
					select c).ToList<IntVec3>();
				}
				return this.cachedAdjCellsCardinal;
			}
		}

		// Token: 0x17002556 RID: 9558
		// (get) Token: 0x0600EFEA RID: 61418 RVA: 0x00459EC0 File Offset: 0x004580C0
		public virtual ThingDef DispensableDef
		{
			get
			{
				return ThingDefOf.MealNutrientPaste;
			}
		}

		// Token: 0x17002557 RID: 9559
		// (get) Token: 0x0600EFEB RID: 61419 RVA: 0x00459EC7 File Offset: 0x004580C7
		public override Color DrawColor
		{
			get
			{
				if (!this.IsSociallyProper(null, false, false))
				{
					return Building_Bed.SheetColorForPrisoner;
				}
				return base.DrawColor;
			}
		}

		// Token: 0x0600EFEC RID: 61420 RVA: 0x00459EE0 File Offset: 0x004580E0
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.powerComp = base.GetComp<CompPowerTrader>();
			this.cachedAdjCellsCardinal = null;
		}

		// Token: 0x0600EFED RID: 61421 RVA: 0x00459F00 File Offset: 0x00458100
		public virtual Building AdjacentReachableHopper(Pawn reacher)
		{
			for (int i = 0; i < this.AdjCellsCardinalInBounds.Count; i++)
			{
				Building edifice = this.AdjCellsCardinalInBounds[i].GetEdifice(base.Map);
				if (edifice != null && edifice.IsHopper() && reacher.CanReach(edifice, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) && !edifice.IsBurning())
				{
					return edifice;
				}
			}
			return null;
		}

		// Token: 0x0600EFEE RID: 61422 RVA: 0x00459F64 File Offset: 0x00458164
		public virtual Thing TryDispenseFood()
		{
			if (!this.CanDispenseNow)
			{
				return null;
			}
			float num = this.def.building.nutritionCostPerDispense - 0.0001f;
			List<ThingDef> list = new List<ThingDef>();
			for (;;)
			{
				Thing thing = this.FindFeedInAnyHopper();
				if (thing == null)
				{
					break;
				}
				int num2 = Mathf.Min(thing.stackCount, Mathf.CeilToInt(num / thing.GetStatValue(StatDefOf.Nutrition, true, -1)));
				num -= (float)num2 * thing.GetStatValue(StatDefOf.Nutrition, true, -1);
				list.Add(thing.def);
				thing.SplitOff(num2);
				if (num <= 0f)
				{
					goto Block_3;
				}
			}
			Log.Error("Did not find enough food in hoppers while trying to dispense.");
			return null;
			Block_3:
			this.def.building.soundDispense.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
			Thing thing2 = ThingMaker.MakeThing(ThingDefOf.MealNutrientPaste, null);
			CompIngredients compIngredients = thing2.TryGetComp<CompIngredients>();
			for (int i = 0; i < list.Count; i++)
			{
				compIngredients.RegisterIngredient(list[i]);
			}
			return thing2;
		}

		// Token: 0x0600EFEF RID: 61423 RVA: 0x0045A068 File Offset: 0x00458268
		public virtual Thing FindFeedInAnyHopper()
		{
			for (int i = 0; i < this.AdjCellsCardinalInBounds.Count; i++)
			{
				Thing thing = null;
				Thing thing2 = null;
				List<Thing> thingList = this.AdjCellsCardinalInBounds[i].GetThingList(base.Map);
				for (int j = 0; j < thingList.Count; j++)
				{
					Thing thing3 = thingList[j];
					if (Building_NutrientPasteDispenser.IsAcceptableFeedstock(thing3.def))
					{
						thing = thing3;
					}
					if (thing3.IsHopper())
					{
						thing2 = thing3;
					}
				}
				if (thing != null && thing2 != null)
				{
					return thing;
				}
			}
			return null;
		}

		// Token: 0x0600EFF0 RID: 61424 RVA: 0x0045A0EC File Offset: 0x004582EC
		public virtual bool HasEnoughFeedstockInHoppers()
		{
			float num = 0f;
			for (int i = 0; i < this.AdjCellsCardinalInBounds.Count; i++)
			{
				IntVec3 c = this.AdjCellsCardinalInBounds[i];
				Thing thing = null;
				Thing thing2 = null;
				List<Thing> thingList = c.GetThingList(base.Map);
				for (int j = 0; j < thingList.Count; j++)
				{
					Thing thing3 = thingList[j];
					if (Building_NutrientPasteDispenser.IsAcceptableFeedstock(thing3.def))
					{
						thing = thing3;
					}
					if (thing3.IsHopper())
					{
						thing2 = thing3;
					}
				}
				if (thing != null && thing2 != null)
				{
					num += (float)thing.stackCount * thing.GetStatValue(StatDefOf.Nutrition, true, -1);
				}
				if (num >= this.def.building.nutritionCostPerDispense)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600EFF1 RID: 61425 RVA: 0x0045A1AC File Offset: 0x004583AC
		public static bool IsAcceptableFeedstock(ThingDef def)
		{
			return def.IsNutritionGivingIngestible && def.ingestible.preferability != FoodPreferability.Undefined && (def.ingestible.foodType & FoodTypeFlags.Plant) != FoodTypeFlags.Plant && (def.ingestible.foodType & FoodTypeFlags.Tree) != FoodTypeFlags.Tree;
		}

		// Token: 0x0600EFF2 RID: 61426 RVA: 0x0045A1FD File Offset: 0x004583FD
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			Designator_Build designator_Build = BuildCopyCommandUtility.FindAllowedDesignator(ThingDefOf.Hopper, true);
			if (designator_Build != null)
			{
				yield return designator_Build;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600EFF3 RID: 61427 RVA: 0x0045A210 File Offset: 0x00458410
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(base.GetInspectString());
			if (!this.IsSociallyProper(null, false, false))
			{
				stringBuilder.AppendLine("InPrisonCell".Translate());
			}
			return stringBuilder.ToString().Trim();
		}

		// Token: 0x04009E76 RID: 40566
		public CompPowerTrader powerComp;

		// Token: 0x04009E77 RID: 40567
		private List<IntVec3> cachedAdjCellsCardinal;

		// Token: 0x04009E78 RID: 40568
		public static int CollectDuration = 50;
	}
}
