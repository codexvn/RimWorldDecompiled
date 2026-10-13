using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D4A RID: 11594
	public class CompMeditationFocus : CompStatOffsetBase
	{
		// Token: 0x1700296B RID: 10603
		// (get) Token: 0x0601049A RID: 66714 RVA: 0x004B4DE7 File Offset: 0x004B2FE7
		public new CompProperties_MeditationFocus Props
		{
			get
			{
				return (CompProperties_MeditationFocus)this.props;
			}
		}

		// Token: 0x1700296C RID: 10604
		// (get) Token: 0x0601049B RID: 66715 RVA: 0x004B4DF4 File Offset: 0x004B2FF4
		public override Pawn LastUser
		{
			get
			{
				if (this.lastUsedTick < 0 || Find.TickManager.TicksGame - this.lastUsedTick > 5)
				{
					return null;
				}
				return base.LastUser;
			}
		}

		// Token: 0x0601049C RID: 66716 RVA: 0x004B4E1C File Offset: 0x004B301C
		public override float GetStatOffset(Pawn pawn = null)
		{
			if (!ModLister.CheckRoyalty("Meditation focus"))
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 0; i < this.Props.offsets.Count; i++)
			{
				if (this.Props.offsets[i].CanApply(this.parent, pawn))
				{
					num += this.Props.offsets[i].GetOffset(this.parent, pawn);
				}
			}
			return num;
		}

		// Token: 0x0601049D RID: 66717 RVA: 0x004B4E9C File Offset: 0x004B309C
		public override IEnumerable<string> GetExplanation()
		{
			int num;
			for (int i = 0; i < this.Props.offsets.Count; i = num + 1)
			{
				string explanation = this.Props.offsets[i].GetExplanation(this.parent);
				if (!explanation.NullOrEmpty())
				{
					yield return explanation;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0601049E RID: 66718 RVA: 0x004B4EAC File Offset: 0x004B30AC
		public bool CanPawnUse(Pawn pawn)
		{
			for (int i = 0; i < this.Props.focusTypes.Count; i++)
			{
				if (this.Props.focusTypes[i].CanPawnUse(pawn))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0601049F RID: 66719 RVA: 0x004B4EF0 File Offset: 0x004B30F0
		public bool WillBeAffectedBy(ThingDef def, Faction faction, IntVec3 pos, Rot4 rotation)
		{
			CellRect cellRect = GenAdj.OccupiedRect(pos, rotation, def.size);
			foreach (FocusStrengthOffset focusStrengthOffset in this.Props.offsets)
			{
				FocusStrengthOffset_ArtificialBuildings focusStrengthOffset_ArtificialBuildings = focusStrengthOffset as FocusStrengthOffset_ArtificialBuildings;
				if (focusStrengthOffset_ArtificialBuildings != null && MeditationUtility.CountsAsArtificialBuilding(def, faction) && cellRect.ClosestCellTo(this.parent.Position).DistanceTo(this.parent.Position) <= focusStrengthOffset_ArtificialBuildings.radius)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060104A0 RID: 66720 RVA: 0x004B4F94 File Offset: 0x004B3194
		public override void Used(Pawn pawn)
		{
			base.Used(pawn);
			this.lastUsedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060104A1 RID: 66721 RVA: 0x004B4FB0 File Offset: 0x004B31B0
		public override string CompInspectStringExtra()
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (this.LastUser != null)
			{
				stringBuilder.Append("UserMeditationFocusStrength".Translate(this.LastUser.Named("LASTUSER")) + ": " + this.Props.statDef.ValueToString(this.parent.GetStatValueForPawn(this.Props.statDef, this.LastUser, true), ToStringNumberSense.Absolute, true));
			}
			for (int i = 0; i < this.Props.offsets.Count; i++)
			{
				string text = this.Props.offsets[i].InspectStringExtra(this.parent, null);
				if (!text.NullOrEmpty())
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(text);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060104A2 RID: 66722 RVA: 0x004B5097 File Offset: 0x004B3297
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (ModsConfig.RoyaltyActive)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Meditation, "MeditationFocuses".Translate(), (from f in this.Props.focusTypes
				select f.label).ToCommaList(false, false).CapitalizeFirst(), "MeditationFocusesDesc".Translate(), 4011, null, null, false, false);
			}
			yield break;
		}

		// Token: 0x060104A3 RID: 66723 RVA: 0x004B50A8 File Offset: 0x004B32A8
		public override void PostDrawExtraSelectionOverlays()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			base.PostDrawExtraSelectionOverlays();
			if (ModsConfig.RoyaltyActive)
			{
				for (int i = 0; i < this.Props.offsets.Count; i++)
				{
					this.Props.offsets[i].PostDrawExtraSelectionOverlays(this.parent, this.LastUser);
				}
			}
		}

		// Token: 0x060104A4 RID: 66724 RVA: 0x004B510D File Offset: 0x004B330D
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.lastUsedTick, "lastUsedTick", -1, false);
		}

		// Token: 0x0400AA20 RID: 43552
		private int lastUsedTick = -1;
	}
}
