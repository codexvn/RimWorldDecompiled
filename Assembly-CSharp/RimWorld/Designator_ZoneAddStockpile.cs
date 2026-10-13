using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020030FC RID: 12540
	public abstract class Designator_ZoneAddStockpile : Designator_ZoneAdd
	{
		// Token: 0x17002DC7 RID: 11719
		// (get) Token: 0x060119F1 RID: 72177 RVA: 0x0050ED66 File Offset: 0x0050CF66
		protected override string NewZoneLabel
		{
			get
			{
				return this.preset.PresetName();
			}
		}

		// Token: 0x17002DC8 RID: 11720
		// (get) Token: 0x060119F2 RID: 72178 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool ShowRightClickHideOptions
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060119F3 RID: 72179 RVA: 0x0050ED73 File Offset: 0x0050CF73
		protected override Zone MakeNewZone()
		{
			return new Zone_Stockpile(this.preset, Find.CurrentMap.zoneManager);
		}

		// Token: 0x060119F4 RID: 72180 RVA: 0x0050ED8A File Offset: 0x0050CF8A
		public Designator_ZoneAddStockpile()
		{
			this.zoneTypeToPlace = typeof(Zone_Stockpile);
			this.soundSucceeded = SoundDefOf.Designate_ZoneAdd_Stockpile;
		}

		// Token: 0x060119F5 RID: 72181 RVA: 0x0050EDB0 File Offset: 0x0050CFB0
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			AcceptanceReport result = base.CanDesignateCell(c);
			if (!result.Accepted)
			{
				return result;
			}
			if (c.GetTerrain(base.Map).passability == Traversability.Impassable)
			{
				return false;
			}
			List<Thing> list = base.Map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (!list[i].def.CanOverlapZones)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060119F6 RID: 72182 RVA: 0x0050EE2E File Offset: 0x0050D02E
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Stockpiles, KnowledgeAmount.Total);
		}

		// Token: 0x17002DC9 RID: 11721
		// (get) Token: 0x060119F7 RID: 72183 RVA: 0x0050EE41 File Offset: 0x0050D041
		public override IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
		{
			get
			{
				if (!this.ShowRightClickHideOptions)
				{
					yield break;
				}
				foreach (FloatMenuOption floatMenuOption in Command_Hide_ZoneStockpile.GetHideOptions())
				{
					yield return floatMenuOption;
				}
				IEnumerator<FloatMenuOption> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x0400B591 RID: 46481
		protected StorageSettingsPreset preset;
	}
}
