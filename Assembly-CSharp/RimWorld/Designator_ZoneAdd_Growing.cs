using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003101 RID: 12545
	public class Designator_ZoneAdd_Growing : Designator_ZoneAdd
	{
		// Token: 0x17002DCD RID: 11725
		// (get) Token: 0x06011A07 RID: 72199 RVA: 0x0050F0C4 File Offset: 0x0050D2C4
		protected override string NewZoneLabel
		{
			get
			{
				return "GrowingZone".Translate();
			}
		}

		// Token: 0x17002DCE RID: 11726
		// (get) Token: 0x06011A08 RID: 72200 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool ShowRightClickHideOptions
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06011A09 RID: 72201 RVA: 0x0050F0D8 File Offset: 0x0050D2D8
		public Designator_ZoneAdd_Growing()
		{
			this.zoneTypeToPlace = typeof(Zone_Growing);
			this.defaultLabel = "GrowingZone".Translate();
			this.defaultDesc = "DesignatorGrowingZoneDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Growing", true);
			this.tutorTag = "ZoneAdd_Growing";
			this.hotKey = KeyBindingDefOf.Misc2;
			this.soundSucceeded = SoundDefOf.Designate_ZoneAdd_Growing;
		}

		// Token: 0x06011A0A RID: 72202 RVA: 0x0050F158 File Offset: 0x0050D358
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!base.CanDesignateCell(c).Accepted)
			{
				return false;
			}
			float num = ModsConfig.BiotechActive ? 0.5f : ThingDefOf.Plant_Potato.plant.fertilityMin;
			if (ModsConfig.IdeologyActive && BuildCopyCommandUtility.FindAllowedDesignator(TerrainDefOf.FungalGravel, true) != null)
			{
				num = Mathf.Min(num, ThingDefOf.Plant_Nutrifungus.plant.fertilityMin);
			}
			if (c.GetFertility(base.Map) < num)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06011A0B RID: 72203 RVA: 0x0050F1E1 File Offset: 0x0050D3E1
		protected override Zone MakeNewZone()
		{
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.GrowingFood, KnowledgeAmount.Total);
			return new Zone_Growing(Find.CurrentMap.zoneManager);
		}

		// Token: 0x17002DCF RID: 11727
		// (get) Token: 0x06011A0C RID: 72204 RVA: 0x0050F1FD File Offset: 0x0050D3FD
		public override IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
		{
			get
			{
				if (!this.ShowRightClickHideOptions)
				{
					yield break;
				}
				foreach (FloatMenuOption floatMenuOption in Command_Hide_ZoneGrow.GetHideOptions())
				{
					yield return floatMenuOption;
				}
				IEnumerator<FloatMenuOption> enumerator = null;
				yield break;
				yield break;
			}
		}
	}
}
