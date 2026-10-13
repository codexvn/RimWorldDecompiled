using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020030FB RID: 12539
	public abstract class Designator_ZoneAdd : Designator_Zone
	{
		// Token: 0x17002DC5 RID: 11717
		// (get) Token: 0x060119E6 RID: 72166 RVA: 0x0050E7D0 File Offset: 0x0050C9D0
		// (set) Token: 0x060119E7 RID: 72167 RVA: 0x0050E7DC File Offset: 0x0050C9DC
		private Zone SelectedZone
		{
			get
			{
				return Find.Selector.SelectedZone;
			}
			set
			{
				Find.Selector.ClearSelection();
				if (value != null)
				{
					Find.Selector.Select(value, false, false);
				}
			}
		}

		// Token: 0x17002DC6 RID: 11718
		// (get) Token: 0x060119E8 RID: 72168
		protected abstract string NewZoneLabel { get; }

		// Token: 0x060119E9 RID: 72169
		protected abstract Zone MakeNewZone();

		// Token: 0x060119EA RID: 72170 RVA: 0x0050E7F8 File Offset: 0x0050C9F8
		public Designator_ZoneAdd()
		{
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = SoundDefOf.Designate_DragZone_Changed;
			this.soundSucceeded = SoundDefOf.Designate_ZoneAdd;
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc6;
		}

		// Token: 0x060119EB RID: 72171 RVA: 0x0050E84C File Offset: 0x0050CA4C
		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			if (Find.Selector.SelectedZone != null && Find.Selector.SelectedZone.GetType() != this.zoneTypeToPlace)
			{
				Find.Selector.Deselect(Find.Selector.SelectedZone);
			}
		}

		// Token: 0x060119EC RID: 72172 RVA: 0x0050E89C File Offset: 0x0050CA9C
		public override void DrawMouseAttachments()
		{
			if (this.useMouseIcon)
			{
				string text = "";
				if (!Input.GetKey(KeyCode.Mouse0))
				{
					Zone selectedZone = Find.Selector.SelectedZone;
					if (selectedZone != null)
					{
						text = "ExpandOrCreateZone".Translate(selectedZone.label, this.NewZoneLabel);
					}
					else
					{
						text = "CreateNewZone".Translate(this.NewZoneLabel);
					}
				}
				GenUI.DrawMouseAttachment(this.icon, text, 0f, default(Vector2), null, null, false, default(Color), null, null);
			}
		}

		// Token: 0x060119ED RID: 72173 RVA: 0x0050E95C File Offset: 0x0050CB5C
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			Zone zone = base.Map.zoneManager.ZoneAt(c);
			if (zone != null && zone.GetType() != this.zoneTypeToPlace)
			{
				return false;
			}
			return Designator_ZoneAdd.IsZoneableCell(c, base.Map);
		}

		// Token: 0x060119EE RID: 72174 RVA: 0x0050E9BC File Offset: 0x0050CBBC
		public static AcceptanceReport IsZoneableCell(IntVec3 c, Map map)
		{
			if (!c.InBounds(map))
			{
				return false;
			}
			if (c.Fogged(map))
			{
				return false;
			}
			if (c.InNoZoneEdgeArea(map))
			{
				return "TooCloseToMapEdge".Translate();
			}
			using (IEnumerator<Thing> enumerator = map.thingGrid.ThingsAt(c).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.def.CanOverlapZones)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060119EF RID: 72175 RVA: 0x0050EA60 File Offset: 0x0050CC60
		public override void DesignateMultiCell(IEnumerable<IntVec3> cells)
		{
			this.unsetCells.Clear();
			this.unsetCells.AddRange(cells);
			if (this.unsetCells.Count == 1)
			{
				Zone zone = base.Map.zoneManager.ZoneAt(this.unsetCells[0]);
				if (zone != null)
				{
					if (zone.GetType() == this.zoneTypeToPlace)
					{
						this.SelectedZone = zone;
					}
					return;
				}
			}
			if (this.SelectedZone == null)
			{
				Zone zone2 = null;
				foreach (IntVec3 c3 in cells)
				{
					Zone zone3 = base.Map.zoneManager.ZoneAt(c3);
					if (zone3 != null && zone3.GetType() == this.zoneTypeToPlace)
					{
						if (zone2 == null)
						{
							zone2 = zone3;
						}
						else if (zone3 != zone2)
						{
							zone2 = null;
							break;
						}
					}
				}
				this.SelectedZone = zone2;
			}
			this.unsetCells.RemoveAll((IntVec3 c) => base.Map.zoneManager.ZoneAt(c) != null);
			if (this.unsetCells.Count == 0)
			{
				return;
			}
			if (TutorSystem.TutorialMode && !TutorSystem.AllowAction(new EventPack(base.TutorTagDesignate, this.unsetCells)))
			{
				return;
			}
			if (this.SelectedZone == null)
			{
				this.SelectedZone = this.MakeNewZone();
				base.Map.zoneManager.RegisterZone(this.SelectedZone);
				this.SelectedZone.AddCell(this.unsetCells[0]);
				this.unsetCells.RemoveAt(0);
			}
			bool somethingSucceeded;
			for (;;)
			{
				somethingSucceeded = true;
				int count = this.unsetCells.Count;
				for (int i = this.unsetCells.Count - 1; i >= 0; i--)
				{
					bool flag = false;
					for (int j = 0; j < 4; j++)
					{
						IntVec3 c2 = this.unsetCells[i] + GenAdj.CardinalDirections[j];
						if (c2.InBounds(base.Map) && base.Map.zoneManager.ZoneAt(c2) == this.SelectedZone)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						this.SelectedZone.AddCell(this.unsetCells[i]);
						this.unsetCells.RemoveAt(i);
					}
				}
				if (this.unsetCells.Count == 0)
				{
					break;
				}
				if (this.unsetCells.Count == count)
				{
					this.SelectedZone = this.MakeNewZone();
					base.Map.zoneManager.RegisterZone(this.SelectedZone);
					this.SelectedZone.AddCell(this.unsetCells[0]);
					this.unsetCells.RemoveAt(0);
				}
			}
			this.SelectedZone.CheckContiguous();
			base.Finalize(somethingSucceeded);
			Zone_Stockpile zone_Stockpile = this.SelectedZone as Zone_Stockpile;
			if (zone_Stockpile != null)
			{
				zone_Stockpile.slotGroup.RemoveHaulDesignationOnStoredThings();
			}
			TutorSystem.Notify_Event(new EventPack(base.TutorTagDesignate, this.unsetCells));
		}

		// Token: 0x0400B58F RID: 46479
		protected Type zoneTypeToPlace;

		// Token: 0x0400B590 RID: 46480
		private List<IntVec3> unsetCells = new List<IntVec3>();
	}
}
