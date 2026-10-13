using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003107 RID: 12551
	public class Designator_ZoneDelete : Designator_Zone
	{
		// Token: 0x17002DDA RID: 11738
		// (get) Token: 0x06011A2C RID: 72236 RVA: 0x0050F943 File Offset: 0x0050DB43
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.RemoveZones;
			}
		}

		// Token: 0x06011A2D RID: 72237 RVA: 0x0050F94C File Offset: 0x0050DB4C
		public Designator_ZoneDelete()
		{
			this.defaultLabel = "DesignatorZoneDelete".Translate();
			this.defaultDesc = "DesignatorZoneDeleteDesc".Translate();
			this.soundDragSustain = SoundDefOf.Designate_DragAreaDelete;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneDelete;
			this.useMouseIcon = true;
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneDelete", true);
			this.hotKey = KeyBindingDefOf.Misc3;
		}

		// Token: 0x06011A2E RID: 72238 RVA: 0x0050F9D4 File Offset: 0x0050DBD4
		public override AcceptanceReport CanDesignateCell(IntVec3 sq)
		{
			if (!sq.InBounds(base.Map))
			{
				return false;
			}
			if (sq.Fogged(base.Map))
			{
				return false;
			}
			if (base.Map.zoneManager.ZoneAt(sq) == null)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06011A2F RID: 72239 RVA: 0x0050FA2C File Offset: 0x0050DC2C
		public override void DesignateSingleCell(IntVec3 c)
		{
			Zone zone = base.Map.zoneManager.ZoneAt(c);
			zone.RemoveCell(c);
			if (!this.justDesignated.Contains(zone))
			{
				this.justDesignated.Add(zone);
			}
		}

		// Token: 0x06011A30 RID: 72240 RVA: 0x0050FA6C File Offset: 0x0050DC6C
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			for (int i = 0; i < this.justDesignated.Count; i++)
			{
				this.justDesignated[i].CheckContiguous();
			}
			this.justDesignated.Clear();
		}

		// Token: 0x0400B5A4 RID: 46500
		private readonly List<Zone> justDesignated = new List<Zone>();
	}
}
