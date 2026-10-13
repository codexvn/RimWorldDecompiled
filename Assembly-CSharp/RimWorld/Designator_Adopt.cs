using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200307B RID: 12411
	public class Designator_Adopt : Designator
	{
		// Token: 0x17002D15 RID: 11541
		// (get) Token: 0x060116C3 RID: 71363 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool DoTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002D16 RID: 11542
		// (get) Token: 0x060116C4 RID: 71364 RVA: 0x00501A2B File Offset: 0x004FFC2B
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return DrawStyleCategoryDefOf.FilledRectangle;
			}
		}

		// Token: 0x060116C5 RID: 71365 RVA: 0x00501A34 File Offset: 0x004FFC34
		public Designator_Adopt()
		{
			this.defaultLabel = "DesignatorAdopt".Translate();
			this.defaultDesc = "DesignatorAdoptDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/Adopt", true);
			this.soundSucceeded = SoundDefOf.Designate_Adopt;
			this.showReverseDesignatorDisabledReason = true;
		}

		// Token: 0x060116C6 RID: 71366 RVA: 0x00501A9C File Offset: 0x004FFC9C
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (c.Fogged(base.Map))
			{
				return false;
			}
			if (!c.GetThingList(base.Map).Any((Thing t) => this.CanDesignateThing(t).Accepted))
			{
				return false;
			}
			return true;
		}

		// Token: 0x060116C7 RID: 71367 RVA: 0x00501B10 File Offset: 0x004FFD10
		public override void DesignateSingleCell(IntVec3 c)
		{
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (this.CanDesignateThing(thingList[i]).Accepted)
				{
					this.DesignateThing(thingList[i]);
				}
			}
		}

		// Token: 0x060116C8 RID: 71368 RVA: 0x00501B60 File Offset: 0x004FFD60
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn == null || pawn.IsShambler)
			{
				return false;
			}
			Designator_Adopt._tmpBuilder.Clear();
			if (t.Faction != Faction.OfPlayer && t.AdoptableBy(Faction.OfPlayer, Designator_Adopt._tmpBuilder))
			{
				return true;
			}
			return Designator_Adopt._tmpBuilder.ToString();
		}

		// Token: 0x060116C9 RID: 71369 RVA: 0x00501BC8 File Offset: 0x004FFDC8
		public override void DesignateThing(Thing t)
		{
			t.SetFaction(Faction.OfPlayer, null);
			Pawn pawn = (Pawn)t;
			Building_Bed building_Bed = pawn.CurrentBed();
			if (RestUtility.CanUseBedNow(building_Bed, pawn, false, false, null))
			{
				building_Bed.TryGetComp<CompAssignableToPawn_Bed>().TryAssignPawn(pawn);
			}
			if (this.lastDesignateFrame < RealTime.frameCount)
			{
				TaggedString taggedString = "LetterTextAdopted".Translate(pawn.Named("BABY"));
				if (pawn.babyNamingDeadline >= Find.TickManager.TicksGame)
				{
					taggedString += "\n\n" + "LetterPartNameBabyAdopt".Translate(pawn, 60000.ToStringTicksToPeriod(true, false, true, true, false));
				}
				ChoiceLetter_BabyBirth choiceLetter_BabyBirth = (ChoiceLetter_BabyBirth)LetterMaker.MakeLetter("LetterLabelAdopted".Translate(pawn.Named("BABY")), taggedString, LetterDefOf.BabyBirth, pawn, null, null, null);
				choiceLetter_BabyBirth.Start();
				Find.LetterStack.ReceiveLetter(choiceLetter_BabyBirth, null, 0, true);
			}
			this.lastDesignateFrame = RealTime.frameCount;
			foreach (IntVec3 cell in t.OccupiedRect())
			{
				FleckMaker.ThrowMetaPuffs(new TargetInfo(cell, base.Map, false));
			}
		}

		// Token: 0x0400B485 RID: 46213
		private int lastDesignateFrame = -1;

		// Token: 0x0400B486 RID: 46214
		private static StringBuilder _tmpBuilder = new StringBuilder();
	}
}
