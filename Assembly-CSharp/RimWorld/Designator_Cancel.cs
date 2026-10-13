using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200308D RID: 12429
	public class Designator_Cancel : Designator
	{
		// Token: 0x17002D26 RID: 11558
		// (get) Token: 0x0601170B RID: 71435 RVA: 0x005029EF File Offset: 0x00500BEF
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.Cancel;
			}
		}

		// Token: 0x0601170C RID: 71436 RVA: 0x005029F8 File Offset: 0x00500BF8
		public Designator_Cancel()
		{
			this.defaultLabel = "DesignatorCancel".Translate();
			this.defaultDesc = "DesignatorCancelDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);
			this.useMouseIcon = true;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.soundSucceeded = SoundDefOf.Designate_Cancel;
			this.hotKey = KeyBindingDefOf.Designator_Cancel;
			this.tutorTag = "Cancel";
		}

		// Token: 0x0601170D RID: 71437 RVA: 0x00502A84 File Offset: 0x00500C84
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (this.CancelableDesignationsAt(c).Any<Designation>())
			{
				return true;
			}
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (this.CanDesignateThing(thingList[i]).Accepted)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0601170E RID: 71438 RVA: 0x00502B00 File Offset: 0x00500D00
		public override void DesignateSingleCell(IntVec3 c)
		{
			foreach (Designation designation in this.CancelableDesignationsAt(c).ToList<Designation>())
			{
				if (designation.def == DesignationDefOf.MineVein)
				{
					Designator_MineVein.RemoveContiguousDesignations(c, base.Map, c.GetEdifice(base.Map).def);
				}
				if (designation.def.designateCancelable)
				{
					base.Map.designationManager.RemoveDesignation(designation);
				}
			}
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = thingList.Count - 1; i >= 0; i--)
			{
				if (this.CanDesignateThing(thingList[i]).Accepted)
				{
					this.DesignateThing(thingList[i]);
				}
			}
		}

		// Token: 0x0601170F RID: 71439 RVA: 0x00502BE0 File Offset: 0x00500DE0
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (base.Map.designationManager.DesignationOn(t) != null)
			{
				using (List<Designation>.Enumerator enumerator = base.Map.designationManager.AllDesignationsOn(t).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.def.designateCancelable)
						{
							return true;
						}
					}
				}
			}
			if (t.def.mineable && base.Map.designationManager.DesignationAt(t.Position, DesignationDefOf.Mine) != null)
			{
				return true;
			}
			if (t.def.mineable && base.Map.designationManager.DesignationAt(t.Position, DesignationDefOf.MineVein) != null)
			{
				return true;
			}
			if (t.def.IsSmoothable && base.Map.designationManager.DesignationAt(t.Position, DesignationDefOf.SmoothWall) != null)
			{
				return true;
			}
			return t.Faction == Faction.OfPlayer && (t is Frame || t is Blueprint);
		}

		// Token: 0x06011710 RID: 71440 RVA: 0x00502D20 File Offset: 0x00500F20
		public override void DesignateThing(Thing t)
		{
			if (t is Frame || t is Blueprint)
			{
				t.Destroy(DestroyMode.Cancel);
				return;
			}
			base.Map.designationManager.RemoveAllDesignationsOn(t, true);
			if (t.def.mineable)
			{
				Designation designation = base.Map.designationManager.DesignationAt(t.Position, DesignationDefOf.Mine);
				if (designation != null)
				{
					base.Map.designationManager.RemoveDesignation(designation);
				}
				if (base.Map.designationManager.DesignationAt(t.Position, DesignationDefOf.MineVein) != null)
				{
					Designator_MineVein.RemoveContiguousDesignations(t.Position, base.Map, t.def);
				}
			}
			if (t.def.IsSmoothable)
			{
				Designation designation2 = base.Map.designationManager.DesignationAt(t.Position, DesignationDefOf.SmoothWall);
				if (designation2 != null)
				{
					base.Map.designationManager.RemoveDesignation(designation2);
				}
			}
		}

		// Token: 0x06011711 RID: 71441 RVA: 0x00502E05 File Offset: 0x00501005
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}

		// Token: 0x06011712 RID: 71442 RVA: 0x00502E0C File Offset: 0x0050100C
		private IEnumerable<Designation> CancelableDesignationsAt(IntVec3 c)
		{
			return base.Map.designationManager.AllDesignationsAt(c);
		}

		// Token: 0x06011713 RID: 71443 RVA: 0x00502E20 File Offset: 0x00501020
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			Designator_Cancel.seenThings.Clear();
			foreach (IntVec3 c in dragCells)
			{
				if (base.Map.designationManager.HasMapDesignationAt(c))
				{
					Graphics.DrawMesh(MeshPool.plane10, c.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays.AltitudeFor()), Quaternion.identity, DesignatorUtility.DragHighlightCellMat, 0);
					if (base.Map.designationManager.DesignationAt(c, DesignationDefOf.Mine) != null)
					{
						continue;
					}
				}
				List<Thing> thingList = c.GetThingList(base.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing thing = thingList[i];
					if (!Designator_Cancel.seenThings.Contains(thing) && this.CanDesignateThing(thing).Accepted)
					{
						Vector3 drawPos = thing.DrawPos;
						drawPos.y = AltitudeLayer.MetaOverlays.AltitudeFor();
						Graphics.DrawMesh(MeshPool.plane10, drawPos, Quaternion.identity, DesignatorUtility.DragHighlightThingMat, 0);
						Designator_Cancel.seenThings.Add(thing);
					}
				}
			}
			Designator_Cancel.seenThings.Clear();
		}

		// Token: 0x0400B491 RID: 46225
		private static HashSet<Thing> seenThings = new HashSet<Thing>();
	}
}
