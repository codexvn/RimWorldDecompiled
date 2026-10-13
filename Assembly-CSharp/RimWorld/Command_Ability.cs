using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020021E9 RID: 8681
	[StaticConstructorOnStartup]
	public class Command_Ability : Command
	{
		// Token: 0x17001CFC RID: 7420
		// (get) Token: 0x0600BE26 RID: 48678 RVA: 0x00372505 File Offset: 0x00370705
		public Ability Ability
		{
			get
			{
				return this.ability;
			}
		}

		// Token: 0x17001CFD RID: 7421
		// (get) Token: 0x0600BE27 RID: 48679 RVA: 0x0037250D File Offset: 0x0037070D
		public override Texture2D BGTexture
		{
			get
			{
				return Command_Ability.BGTex;
			}
		}

		// Token: 0x17001CFE RID: 7422
		// (get) Token: 0x0600BE28 RID: 48680 RVA: 0x00372514 File Offset: 0x00370714
		public override Texture2D BGTextureShrunk
		{
			get
			{
				return Command_Ability.BGTexShrunk;
			}
		}

		// Token: 0x17001CFF RID: 7423
		// (get) Token: 0x0600BE29 RID: 48681 RVA: 0x0037251B File Offset: 0x0037071B
		// (set) Token: 0x0600BE2A RID: 48682 RVA: 0x00372523 File Offset: 0x00370723
		public Pawn Pawn { get; private set; }

		// Token: 0x17001D00 RID: 7424
		// (get) Token: 0x0600BE2B RID: 48683 RVA: 0x0037252C File Offset: 0x0037072C
		public virtual string Tooltip
		{
			get
			{
				return this.ability.Tooltip;
			}
		}

		// Token: 0x17001D01 RID: 7425
		// (get) Token: 0x0600BE2C RID: 48684 RVA: 0x00372539 File Offset: 0x00370739
		public override string TopRightLabel
		{
			get
			{
				return this.ability.GizmoExtraLabel;
			}
		}

		// Token: 0x17001D02 RID: 7426
		// (get) Token: 0x0600BE2D RID: 48685 RVA: 0x00372546 File Offset: 0x00370746
		// (set) Token: 0x0600BE2E RID: 48686 RVA: 0x0018182B File Offset: 0x0017FA2B
		public override bool Disabled
		{
			get
			{
				this.DisabledCheck();
				return this.disabled;
			}
			set
			{
				this.disabled = value;
			}
		}

		// Token: 0x0600BE2F RID: 48687 RVA: 0x00372554 File Offset: 0x00370754
		public Command_Ability(Ability ability, Pawn pawn)
		{
			this.ability = ability;
			float num = 5f;
			AbilityCategoryDef category = ability.def.category;
			int? num2 = (category != null) ? new int?(category.displayOrder) : null;
			this.Order = num + ((num2 != null) ? ((float)num2.GetValueOrDefault()) : 0f) / 100f + (float)ability.def.displayOrder / 1000f + (float)ability.def.level / 10000f;
			this.defaultLabel = ability.def.LabelCap;
			this.hotKey = ability.def.hotKey;
			this.icon = ability.def.uiIcon;
			this.shrinkable = true;
			this.Pawn = pawn;
			this.originalLabel = this.defaultLabel;
			string text = this.LabelCap.Colorize(ColoredText.TipSectionTitleColor);
			if (pawn.Name != null)
			{
				string text2 = " (" + pawn.Name.ToStringShort + ")";
				this.pawnLabel = this.defaultLabel + text2;
				this.pawnTooltip = this.Tooltip.Insert(text.Length, text2);
				return;
			}
			this.pawnLabel = this.defaultLabel;
			this.pawnTooltip = this.Tooltip;
		}

		// Token: 0x0600BE30 RID: 48688 RVA: 0x003726AC File Offset: 0x003708AC
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			this.defaultLabel = (parms.multipleSelected ? this.pawnLabel : this.originalLabel);
			if (this.devGizmo)
			{
				this.defaultLabel = "DEV: " + this.defaultLabel;
			}
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			GizmoResult result = base.GizmoOnGUI(topLeft, maxWidth, parms);
			bool flag = (this.ability.Casting || KeyBindingDefOf.QueueOrder.IsDownEvent) && !this.ability.CanQueueCast;
			if (flag)
			{
				Widgets.FillableBar(rect, 0f, Command_Ability.cooldownBarTex, null, false);
			}
			else if (this.ability.CooldownTicksRemaining > 0)
			{
				float value = Mathf.InverseLerp((float)this.ability.CooldownTicksTotal, 0f, (float)this.ability.CooldownTicksRemaining);
				Widgets.FillableBar(rect, Mathf.Clamp01(value), Command_Ability.cooldownBarTex, null, false);
				if (this.ability.CooldownTicksRemaining > 0)
				{
					Text.Font = GameFont.Tiny;
					string text = this.ability.CooldownTicksRemaining.ToStringTicksToPeriod(true, false, true, true, false);
					Vector2 vector = Text.CalcSize(text);
					vector.x += 2f;
					Rect rect2 = rect;
					rect2.x = rect.x + rect.width / 2f - vector.x / 2f;
					rect2.width = vector.x;
					rect2.height = vector.y;
					Rect position = rect2.ExpandedBy(8f, 0f);
					Text.Anchor = TextAnchor.UpperCenter;
					GUI.DrawTexture(position, TexUI.GrayTextBG);
					Widgets.Label(rect2, text);
					Text.Anchor = TextAnchor.UpperLeft;
				}
			}
			if (result.State == GizmoState.Interacted && this.ability.CanCast && !flag)
			{
				return result;
			}
			return new GizmoResult(result.State);
		}

		// Token: 0x0600BE31 RID: 48689 RVA: 0x00372898 File Offset: 0x00370A98
		protected override GizmoResult GizmoOnGUIInt(Rect butRect, GizmoRenderParms parms)
		{
			if (Mouse.IsOver(butRect))
			{
				if (parms.multipleSelected)
				{
					if (this.Pawn.Map != null)
					{
						GenUI.DrawArrowPointingAtWorldspace(this.Pawn.DrawPos, Find.Camera);
					}
					this.defaultDesc = this.pawnTooltip;
				}
				else
				{
					this.defaultDesc = this.Tooltip;
				}
			}
			this.DisabledCheck();
			return base.GizmoOnGUIInt(butRect, parms);
		}

		// Token: 0x0600BE32 RID: 48690 RVA: 0x00372900 File Offset: 0x00370B00
		public override bool GroupsWith(Gizmo other)
		{
			if (this.ability.def.groupAbility)
			{
				Command_Ability command_Ability = other as Command_Ability;
				if (command_Ability != null)
				{
					return command_Ability.ability.def == this.ability.def;
				}
			}
			return false;
		}

		// Token: 0x0600BE33 RID: 48691 RVA: 0x00372944 File Offset: 0x00370B44
		public override bool ShowPawnDetailsWith(Gizmo other)
		{
			Command_Ability command_Ability = other as Command_Ability;
			return command_Ability != null && command_Ability.ability.def == this.ability.def;
		}

		// Token: 0x0600BE34 RID: 48692 RVA: 0x00372978 File Offset: 0x00370B78
		public virtual void GroupAbilityCommands(List<Gizmo> group)
		{
			if (this.groupedCasts == null)
			{
				this.groupedCasts = new List<Command_Ability>();
			}
			this.groupedCasts.Clear();
			for (int i = 0; i < group.Count; i++)
			{
				if (group[i].GroupsWith(this))
				{
					this.groupedCasts.Add((Command_Ability)group[i]);
				}
			}
		}

		// Token: 0x0600BE35 RID: 48693 RVA: 0x003729DC File Offset: 0x00370BDC
		protected virtual void DisabledCheck()
		{
			string str;
			this.disabled = this.ability.GizmoDisabled(out str);
			if (this.disabled)
			{
				this.DisableWithReason(str.CapitalizeFirst());
			}
		}

		// Token: 0x0600BE36 RID: 48694 RVA: 0x00372A10 File Offset: 0x00370C10
		private ITargetingSource GetBetterTargetingSource(LocalTargetInfo t)
		{
			this.groupedCasts.SortBy((Command_Ability c) => c.ability.pawn.Position.DistanceToSquared(t.Cell));
			for (int i = 0; i < this.groupedCasts.Count; i++)
			{
				if (this.groupedCasts[i].ability.CanQueueCast && this.groupedCasts[i].ability.verb.ValidateTarget(t, false))
				{
					return this.groupedCasts[i].ability.verb;
				}
			}
			return null;
		}

		// Token: 0x0600BE37 RID: 48695 RVA: 0x00372AAC File Offset: 0x00370CAC
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			if (!this.ability.def.targetRequired)
			{
				this.ability.QueueCastingJob(this.ability.pawn, LocalTargetInfo.Invalid);
				return;
			}
			Find.DesignatorManager.Deselect();
			if (this.ability.def.targetWorldCell)
			{
				CameraJumper.TryJump(CameraJumper.GetWorldTarget(this.ability.pawn), CameraJumper.MovementMode.Pan);
				Find.WorldTargeter.BeginTargeting(delegate(GlobalTargetInfo t)
				{
					if (this.ability.ValidateGlobalTarget(t))
					{
						this.ability.QueueCastingJob(t);
						return true;
					}
					return false;
				}, true, this.ability.def.uiIcon, !this.ability.pawn.IsCaravanMember(), null, new Func<GlobalTargetInfo, TaggedString>(this.ability.WorldMapExtraLabel), new Func<GlobalTargetInfo, bool>(this.ability.ValidateGlobalTarget), null, false);
				return;
			}
			if (this.groupedCasts.NullOrEmpty<Command_Ability>())
			{
				Find.Targeter.BeginTargeting(this.ability.verb, null, false, null, null, true);
				return;
			}
			Find.Targeter.BeginTargeting(this.ability.verb, null, false, new Func<LocalTargetInfo, ITargetingSource>(this.GetBetterTargetingSource), null, true);
		}

		// Token: 0x0600BE38 RID: 48696 RVA: 0x00372BF0 File Offset: 0x00370DF0
		public override void GizmoUpdateOnMouseover()
		{
			Verb_CastAbility verb_CastAbility = this.ability.verb as Verb_CastAbility;
			if (verb_CastAbility != null)
			{
				verb_CastAbility.verbProps.DrawRadiusRing(verb_CastAbility.caster.Position, verb_CastAbility);
			}
			this.ability.OnGizmoUpdate();
		}

		// Token: 0x0600BE39 RID: 48697 RVA: 0x00372C33 File Offset: 0x00370E33
		protected void DisableWithReason(string reason)
		{
			this.disabledReason = reason;
			this.disabled = true;
		}

		// Token: 0x040082AB RID: 33451
		protected Ability ability;

		// Token: 0x040082AC RID: 33452
		private List<Command_Ability> groupedCasts;

		// Token: 0x040082AD RID: 33453
		private string pawnLabel;

		// Token: 0x040082AE RID: 33454
		private string originalLabel;

		// Token: 0x040082AF RID: 33455
		private string pawnTooltip;

		// Token: 0x040082B0 RID: 33456
		public bool devGizmo;

		// Token: 0x040082B2 RID: 33458
		public new static readonly Texture2D BGTex = ContentFinder<Texture2D>.Get("UI/Widgets/AbilityButBG", true);

		// Token: 0x040082B3 RID: 33459
		public new static readonly Texture2D BGTexShrunk = ContentFinder<Texture2D>.Get("UI/Widgets/AbilityButBGShrunk", true);

		// Token: 0x040082B4 RID: 33460
		private static readonly Texture2D cooldownBarTex = SolidColorMaterials.NewSolidColorTexture(new Color32(9, 203, 4, 64));
	}
}
