using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020030A9 RID: 12457
	public class Designator_MineVein : Designator_Mine
	{
		// Token: 0x17002D46 RID: 11590
		// (get) Token: 0x060117CA RID: 71626 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002D47 RID: 11591
		// (get) Token: 0x060117CB RID: 71627 RVA: 0x00505AB0 File Offset: 0x00503CB0
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.MineVein;
			}
		}

		// Token: 0x17002D48 RID: 11592
		// (get) Token: 0x060117CC RID: 71628 RVA: 0x00501A2B File Offset: 0x004FFC2B
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.FilledRectangle;
			}
		}

		// Token: 0x060117CD RID: 71629 RVA: 0x00505AB8 File Offset: 0x00503CB8
		public Designator_MineVein()
		{
			this.defaultLabel = "DesignatorMineVein".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/MineVein", true);
			this.defaultDesc = "DesignatorMineVeinDesc".Translate();
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc11;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.soundSucceeded = SoundDefOf.Designate_Mine;
		}

		// Token: 0x060117CE RID: 71630 RVA: 0x00505B3C File Offset: 0x00503D3C
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (base.Map.designationManager.DesignationAt(c, this.Designation) != null)
			{
				return AcceptanceReport.WasRejected;
			}
			if (c.Fogged(base.Map))
			{
				return true;
			}
			Mineable firstMineable = c.GetFirstMineable(base.Map);
			if (firstMineable == null)
			{
				return "MessageMustDesignateMineable".Translate();
			}
			AcceptanceReport result = this.CanDesignateThing(firstMineable);
			if (!result.Accepted)
			{
				return result;
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x060117CF RID: 71631 RVA: 0x00505BCC File Offset: 0x00503DCC
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (!t.def.mineable || !t.def.building.veinMineable)
			{
				return false;
			}
			if (base.Map.designationManager.DesignationAt(t.Position, this.Designation) != null)
			{
				return AcceptanceReport.WasRejected;
			}
			return true;
		}

		// Token: 0x060117D0 RID: 71632 RVA: 0x00505C29 File Offset: 0x00503E29
		public override void DesignateSingleCell(IntVec3 loc)
		{
			Designator_MineVein.FloodFillDesignations(loc, base.Map, loc.GetEdifice(base.Map).def);
			Designator_Mine.PossiblyWarnPlayerOnDesignatingMining();
		}

		// Token: 0x060117D1 RID: 71633 RVA: 0x00505C50 File Offset: 0x00503E50
		public static void FloodFillDesignations(IntVec3 loc, Map map, ThingDef def)
		{
			Designator_MineVein.<>c__DisplayClass10_0 CS$<>8__locals1 = new Designator_MineVein.<>c__DisplayClass10_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.def = def;
			CS$<>8__locals1.map.floodFiller.FloodFill(loc, new Predicate<IntVec3>(CS$<>8__locals1.<FloodFillDesignations>g__Validator|0), delegate(IntVec3 c)
			{
				CS$<>8__locals1.map.designationManager.AddDesignation(new Designation(c, DesignationDefOf.MineVein, null));
				CS$<>8__locals1.map.designationManager.TryRemoveDesignation(c, DesignationDefOf.Mine);
			}, int.MaxValue, false, null);
		}

		// Token: 0x060117D2 RID: 71634 RVA: 0x00505CA4 File Offset: 0x00503EA4
		public static void RemoveContiguousDesignations(IntVec3 loc, Map map, ThingDef def)
		{
			map.floodFiller.FloodFill(loc, (IntVec3 c) => map.designationManager.DesignationAt(c, DesignationDefOf.MineVein) != null && c.GetEdifice(map).def == def, delegate(IntVec3 c)
			{
				map.designationManager.RemoveDesignation(map.designationManager.DesignationAt(c, DesignationDefOf.MineVein));
			}, int.MaxValue, false, null);
		}
	}
}
