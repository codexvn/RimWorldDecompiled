using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020030A8 RID: 12456
	public class Designator_Mine : Designator_Cells
	{
		// Token: 0x17002D43 RID: 11587
		// (get) Token: 0x060117BE RID: 71614 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002D44 RID: 11588
		// (get) Token: 0x060117BF RID: 71615 RVA: 0x0050579F File Offset: 0x0050399F
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Mine;
			}
		}

		// Token: 0x17002D45 RID: 11589
		// (get) Token: 0x060117C0 RID: 71616 RVA: 0x005057A6 File Offset: 0x005039A6
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.Mine;
			}
		}

		// Token: 0x060117C1 RID: 71617 RVA: 0x005057B0 File Offset: 0x005039B0
		public Designator_Mine()
		{
			this.defaultLabel = "DesignatorMine".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Mine", true);
			this.defaultDesc = "DesignatorMineDesc".Translate();
			this.useMouseIcon = true;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.soundSucceeded = SoundDefOf.Designate_Mine;
			this.removeAllOtherDesignationDefs = new DesignationDef[]
			{
				DesignationDefOf.MineVein
			};
			this.hotKey = KeyBindingDefOf.Misc10;
			this.tutorTag = "Mine";
		}

		// Token: 0x060117C2 RID: 71618 RVA: 0x00505850 File Offset: 0x00503A50
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

		// Token: 0x060117C3 RID: 71619 RVA: 0x005058E0 File Offset: 0x00503AE0
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			if (!t.def.mineable)
			{
				return false;
			}
			if (base.Map.designationManager.DesignationAt(t.Position, this.Designation) != null)
			{
				return AcceptanceReport.WasRejected;
			}
			if (base.Map.designationManager.DesignationAt(t.Position, DesignationDefOf.MineVein) != null)
			{
				return AcceptanceReport.WasRejected;
			}
			return true;
		}

		// Token: 0x060117C4 RID: 71620 RVA: 0x00505950 File Offset: 0x00503B50
		public override void DesignateSingleCell(IntVec3 loc)
		{
			base.Map.designationManager.AddDesignation(new Designation(loc, this.Designation, null));
			base.Map.designationManager.TryRemoveDesignation(loc, DesignationDefOf.SmoothWall);
			Designator_Mine.PossiblyWarnPlayerOnDesignatingMining();
			if (DebugSettings.godMode)
			{
				Mineable firstMineable = loc.GetFirstMineable(base.Map);
				if (firstMineable != null)
				{
					firstMineable.DestroyMined(null);
				}
			}
		}

		// Token: 0x060117C5 RID: 71621 RVA: 0x005059B8 File Offset: 0x00503BB8
		public override void DesignateThing(Thing t)
		{
			this.DesignateSingleCell(t.Position);
		}

		// Token: 0x060117C6 RID: 71622 RVA: 0x005059C6 File Offset: 0x00503BC6
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Mining, KnowledgeAmount.SpecificInteraction);
		}

		// Token: 0x060117C7 RID: 71623 RVA: 0x00502E05 File Offset: 0x00501005
		public override void SelectedUpdate()
		{
			GenUI.RenderMouseoverBracket();
		}

		// Token: 0x060117C8 RID: 71624 RVA: 0x005059DC File Offset: 0x00503BDC
		protected static void PossiblyWarnPlayerOnDesignatingMining()
		{
			if (!ModsConfig.IdeologyActive)
			{
				return;
			}
			Designator_Mine.tmpIdeoMemberNames.Clear();
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				if (ideo.WarnPlayerOnDesignateMine)
				{
					Designator_Mine.tmpIdeoMemberNames.Add(Find.ActiveLanguageWorker.Pluralize(ideo.memberName, -1));
				}
			}
			if (Designator_Mine.tmpIdeoMemberNames.Any<string>())
			{
				Messages.Message("MessageWarningPlayerDesignatedMining".Translate(Designator_Mine.tmpIdeoMemberNames.ToCommaList(true, false)), MessageTypeDefOf.CautionInput, false);
			}
			Designator_Mine.tmpIdeoMemberNames.Clear();
		}

		// Token: 0x0400B4C7 RID: 46279
		private static readonly List<string> tmpIdeoMemberNames = new List<string>();
	}
}
