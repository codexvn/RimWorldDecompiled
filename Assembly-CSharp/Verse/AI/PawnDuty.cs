using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000FC4 RID: 4036
	public class PawnDuty : IExposable
	{
		// Token: 0x17000FA1 RID: 4001
		// (get) Token: 0x06006355 RID: 25429 RVA: 0x001F6068 File Offset: 0x001F4268
		public RandomSocialMode SocialModeMax
		{
			get
			{
				RandomSocialMode? randomSocialMode = this.socialModeMaxOverride;
				if (randomSocialMode == null)
				{
					return this.def.socialModeMax;
				}
				return randomSocialMode.GetValueOrDefault();
			}
		}

		// Token: 0x06006356 RID: 25430 RVA: 0x001F6098 File Offset: 0x001F4298
		public PawnDuty()
		{
		}

		// Token: 0x06006357 RID: 25431 RVA: 0x001F6100 File Offset: 0x001F4300
		public PawnDuty(DutyDef def)
		{
			this.def = def;
		}

		// Token: 0x06006358 RID: 25432 RVA: 0x001F616D File Offset: 0x001F436D
		public PawnDuty(DutyDef def, LocalTargetInfo focus, float radius = -1f) : this(def)
		{
			this.focus = focus;
			this.radius = radius;
		}

		// Token: 0x06006359 RID: 25433 RVA: 0x001F6184 File Offset: 0x001F4384
		public PawnDuty(DutyDef def, LocalTargetInfo focus, LocalTargetInfo focusSecond, float radius = -1f) : this(def, focus, radius)
		{
			this.focusSecond = focusSecond;
		}

		// Token: 0x0600635A RID: 25434 RVA: 0x001F6197 File Offset: 0x001F4397
		public PawnDuty(DutyDef def, LocalTargetInfo focus, LocalTargetInfo focusSecond, LocalTargetInfo focusThird, float radius = -1f) : this(def, focus, radius)
		{
			this.focusSecond = focusSecond;
			this.focusThird = focusThird;
		}

		// Token: 0x0600635B RID: 25435 RVA: 0x001F61B4 File Offset: 0x001F43B4
		public void ExposeData()
		{
			Scribe_Defs.Look<DutyDef>(ref this.def, "def");
			Scribe_TargetInfo.Look(ref this.focus, true, "focus", LocalTargetInfo.Invalid, false);
			Scribe_TargetInfo.Look(ref this.focusSecond, true, "focusSecond", LocalTargetInfo.Invalid, false);
			Scribe_TargetInfo.Look(ref this.focusThird, true, "focusThird", LocalTargetInfo.Invalid, false);
			Scribe_Values.Look<float>(ref this.radius, "radius", -1f, false);
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotion, "locomotion", LocomotionUrgency.None, false);
			Scribe_Values.Look<Danger>(ref this.maxDanger, "maxDanger", Danger.Unspecified, false);
			Scribe_Values.Look<CellRect>(ref this.spectateRect, "spectateRect", default(CellRect), false);
			Scribe_Values.Look<SpectateRectSide>(ref this.spectateRectAllowedSides, "spectateRectAllowedSides", SpectateRectSide.All, false);
			Scribe_Values.Look<bool>(ref this.canDig, "canDig", false, false);
			Scribe_Values.Look<int>(ref this.transportersGroup, "transportersGroup", -1, false);
			Scribe_Values.Look<bool>(ref this.attackDownedIfStarving, "attackDownedIfStarving", false, false);
			Scribe_Values.Look<float?>(ref this.wanderRadius, "wanderRadius", null, false);
			Scribe_Values.Look<IntRange>(ref this.spectateDistance, "spectateDistance", default(IntRange), false);
			Scribe_Values.Look<int?>(ref this.ropeeLimit, "ropeeLimit", null, false);
			Scribe_Values.Look<bool>(ref this.pickupOpportunisticWeapon, "pickupOpportunisticWeapon", false, false);
			Scribe_Values.Look<Rot4>(ref this.overrideFacing, "overrideFacing", Rot4.Invalid, false);
			Scribe_References.Look<ILoadReferenceable>(ref this.source, "source", false);
			Scribe_Values.Look<string>(ref this.tag, "tag", null, false);
			Scribe_Values.Look<RandomSocialMode?>(ref this.socialModeMaxOverride, "socialModeMaxOverride", null, false);
		}

		// Token: 0x0600635C RID: 25436 RVA: 0x001F6368 File Offset: 0x001F4568
		public override string ToString()
		{
			string text = this.focus.IsValid ? this.focus.ToString() : "";
			string text2 = this.focusSecond.IsValid ? (", second=" + this.focusSecond.ToString()) : "";
			string text3 = this.focusThird.IsValid ? (", third=" + this.focusThird.ToString()) : "";
			string text4 = (this.radius > 0f) ? (", rad=" + this.radius.ToString("F2")) : "";
			string[] array = new string[8];
			array[0] = "(";
			int num = 1;
			DutyDef dutyDef = this.def;
			array[num] = ((dutyDef != null) ? dutyDef.ToString() : null);
			array[2] = " ";
			array[3] = text;
			array[4] = text2;
			array[5] = text3;
			array[6] = text4;
			array[7] = ")";
			return string.Concat(array);
		}

		// Token: 0x0600635D RID: 25437 RVA: 0x001F6474 File Offset: 0x001F4674
		internal void DrawDebug(Pawn pawn)
		{
			if (this.focus.IsValid)
			{
				GenDraw.DrawLineBetween(pawn.DrawPos, this.focus.Cell.ToVector3Shifted());
				if (this.radius > 0f)
				{
					GenDraw.DrawRadiusRing(this.focus.Cell, this.radius);
				}
			}
		}

		// Token: 0x04004701 RID: 18177
		public DutyDef def;

		// Token: 0x04004702 RID: 18178
		public LocalTargetInfo focus = LocalTargetInfo.Invalid;

		// Token: 0x04004703 RID: 18179
		public LocalTargetInfo focusSecond = LocalTargetInfo.Invalid;

		// Token: 0x04004704 RID: 18180
		public LocalTargetInfo focusThird = LocalTargetInfo.Invalid;

		// Token: 0x04004705 RID: 18181
		public float radius = -1f;

		// Token: 0x04004706 RID: 18182
		public LocomotionUrgency locomotion;

		// Token: 0x04004707 RID: 18183
		public Danger maxDanger;

		// Token: 0x04004708 RID: 18184
		public CellRect spectateRect;

		// Token: 0x04004709 RID: 18185
		public SpectateRectSide spectateRectAllowedSides = SpectateRectSide.All;

		// Token: 0x0400470A RID: 18186
		public SpectateRectSide spectateRectPreferredSide;

		// Token: 0x0400470B RID: 18187
		public IntRange spectateDistance = new IntRange(2, 5);

		// Token: 0x0400470C RID: 18188
		public bool canDig;

		// Token: 0x0400470D RID: 18189
		public int transportersGroup = -1;

		// Token: 0x0400470E RID: 18190
		public bool attackDownedIfStarving;

		// Token: 0x0400470F RID: 18191
		public float? wanderRadius;

		// Token: 0x04004710 RID: 18192
		public int? ropeeLimit;

		// Token: 0x04004711 RID: 18193
		public bool pickupOpportunisticWeapon;

		// Token: 0x04004712 RID: 18194
		public Rot4 overrideFacing = Rot4.Invalid;

		// Token: 0x04004713 RID: 18195
		public ILoadReferenceable source;

		// Token: 0x04004714 RID: 18196
		public string tag;

		// Token: 0x04004715 RID: 18197
		public RandomSocialMode? socialModeMaxOverride;
	}
}
