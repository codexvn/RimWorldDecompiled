using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C55 RID: 15445
	public class MoveableStockpile : MoveableArea
	{
		// Token: 0x06014FBB RID: 85947 RVA: 0x00648098 File Offset: 0x00646298
		public MoveableStockpile()
		{
		}

		// Token: 0x06014FBC RID: 85948 RVA: 0x006480A0 File Offset: 0x006462A0
		public MoveableStockpile(Gravship gravship, Zone_Stockpile stockpile) : base(gravship, stockpile.label, stockpile.RenamableLabel, stockpile.color, stockpile.ID)
		{
			this.hidden = stockpile.Hidden;
			this.settings = new StorageSettings();
			this.settings.CopyFrom(stockpile.settings);
		}

		// Token: 0x06014FBD RID: 85949 RVA: 0x006480F4 File Offset: 0x006462F4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.hidden, "hidden", false, false);
			Scribe_Deep.Look<StorageSettings>(ref this.settings, "settings", Array.Empty<object>());
		}

		// Token: 0x06014FBE RID: 85950 RVA: 0x00648124 File Offset: 0x00646324
		public void TryCreateStockpile(ZoneManager zoneManager, IntVec3 newOrigin)
		{
			Zone_Stockpile zone_Stockpile = new Zone_Stockpile(StorageSettingsPreset.DefaultStockpile, zoneManager)
			{
				label = this.label,
				Hidden = this.hidden,
				color = this.color,
				ID = this.id
			};
			zone_Stockpile.settings = new StorageSettings(zone_Stockpile);
			zone_Stockpile.settings.CopyFrom(this.settings);
			zoneManager.RegisterZone(zone_Stockpile);
			foreach (IntVec3 b in base.RelativeCells)
			{
				zone_Stockpile.AddCell(newOrigin + b);
			}
		}

		// Token: 0x0400E9E7 RID: 59879
		private bool hidden;

		// Token: 0x0400E9E8 RID: 59880
		private StorageSettings settings;
	}
}
