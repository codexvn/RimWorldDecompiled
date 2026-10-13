using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F1F RID: 12063
	public abstract class CompTargetable : CompUseEffect, ITargetingSource
	{
		// Token: 0x17002BF4 RID: 11252
		// (get) Token: 0x06011074 RID: 69748 RVA: 0x004E5D76 File Offset: 0x004E3F76
		public CompProperties_Targetable Props
		{
			get
			{
				return (CompProperties_Targetable)this.props;
			}
		}

		// Token: 0x17002BF5 RID: 11253
		// (get) Token: 0x06011075 RID: 69749
		protected abstract bool PlayerChoosesTarget { get; }

		// Token: 0x06011076 RID: 69750 RVA: 0x004E5D83 File Offset: 0x004E3F83
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_References.Look<Thing>(ref this.selectedTarget, "selectedTarget", false);
		}

		// Token: 0x06011077 RID: 69751 RVA: 0x004E5D9C File Offset: 0x004E3F9C
		public override bool SelectedUseOption(Pawn p)
		{
			if (this.PlayerChoosesTarget)
			{
				this.caster = p;
				Find.Targeter.BeginTargeting(this, null, true, null, null, true);
				return true;
			}
			this.selectedTarget = null;
			return false;
		}

		// Token: 0x06011078 RID: 69752 RVA: 0x004E5DC8 File Offset: 0x004E3FC8
		public override void DoEffect(Pawn usedBy)
		{
			if (this.PlayerChoosesTarget && this.selectedTarget == null)
			{
				return;
			}
			if (this.selectedTarget != null && !this.GetTargetingParameters().CanTarget(this.selectedTarget, null))
			{
				return;
			}
			base.DoEffect(usedBy);
			foreach (Thing thing in this.GetTargets(this.selectedTarget))
			{
				foreach (CompTargetEffect compTargetEffect in this.parent.GetComps<CompTargetEffect>())
				{
					compTargetEffect.DoEffectOn(usedBy, thing);
				}
				if (this.Props.moteOnTarget != null)
				{
					MoteMaker.MakeAttachedOverlay(thing, this.Props.moteOnTarget, Vector3.zero, 1f, -1f);
				}
				if (this.Props.fleckOnTarget != null)
				{
					FleckMaker.AttachedOverlay(thing, this.Props.fleckOnTarget, Vector3.zero, 1f, -1f);
				}
				if (this.Props.moteConnecting != null)
				{
					MoteMaker.MakeConnectingLine(usedBy.DrawPos, thing.DrawPos, this.Props.moteConnecting, usedBy.Map, 1f);
				}
				if (this.Props.fleckConnecting != null)
				{
					FleckMaker.ConnectingLine(usedBy.DrawPos, thing.DrawPos, this.Props.fleckConnecting, usedBy.Map, 1f);
				}
			}
			this.selectedTarget = null;
		}

		// Token: 0x06011079 RID: 69753
		protected abstract TargetingParameters GetTargetingParameters();

		// Token: 0x0601107A RID: 69754
		public abstract IEnumerable<Thing> GetTargets(Thing targetChosenByPlayer = null);

		// Token: 0x17002BF6 RID: 11254
		// (get) Token: 0x0601107B RID: 69755 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool CasterIsPawn
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002BF7 RID: 11255
		// (get) Token: 0x0601107C RID: 69756 RVA: 0x00002501 File Offset: 0x00000701
		public bool IsMeleeAttack
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002BF8 RID: 11256
		// (get) Token: 0x0601107D RID: 69757 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool Targetable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002BF9 RID: 11257
		// (get) Token: 0x0601107E RID: 69758 RVA: 0x00002501 File Offset: 0x00000701
		public bool MultiSelect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002BFA RID: 11258
		// (get) Token: 0x0601107F RID: 69759 RVA: 0x00002501 File Offset: 0x00000701
		public bool HidePawnTooltips
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002BFB RID: 11259
		// (get) Token: 0x06011080 RID: 69760 RVA: 0x0048FDB8 File Offset: 0x0048DFB8
		public Thing Caster
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x17002BFC RID: 11260
		// (get) Token: 0x06011081 RID: 69761 RVA: 0x004E5F78 File Offset: 0x004E4178
		public Pawn CasterPawn
		{
			get
			{
				return this.caster;
			}
		}

		// Token: 0x17002BFD RID: 11261
		// (get) Token: 0x06011082 RID: 69762 RVA: 0x00002C42 File Offset: 0x00000E42
		public Verb GetVerb
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17002BFE RID: 11262
		// (get) Token: 0x06011083 RID: 69763 RVA: 0x004E5F80 File Offset: 0x004E4180
		public TargetingParameters targetParams
		{
			get
			{
				return this.GetTargetingParameters();
			}
		}

		// Token: 0x17002BFF RID: 11263
		// (get) Token: 0x06011084 RID: 69764 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual ITargetingSource DestinationSelector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17002C00 RID: 11264
		// (get) Token: 0x06011085 RID: 69765 RVA: 0x00002C42 File Offset: 0x00000E42
		public Texture2D UIIcon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06011086 RID: 69766 RVA: 0x004E5F88 File Offset: 0x004E4188
		public bool CanHitTarget(LocalTargetInfo target)
		{
			return this.ValidateTarget(target, false);
		}

		// Token: 0x06011087 RID: 69767 RVA: 0x0045EA21 File Offset: 0x0045CC21
		public void DrawHighlight(LocalTargetInfo target)
		{
			if (target.IsValid)
			{
				GenDraw.DrawTargetHighlight(target);
			}
		}

		// Token: 0x06011088 RID: 69768 RVA: 0x004E5F94 File Offset: 0x004E4194
		public void OrderForceTarget(LocalTargetInfo target)
		{
			this.selectedTarget = target.Thing;
			CompUsable compUsable;
			if (this.parent.TryGetComp(out compUsable))
			{
				compUsable.TryStartUseJob(this.caster, target, compUsable.Props.ignoreOtherReservations);
			}
			this.caster = null;
		}

		// Token: 0x06011089 RID: 69769 RVA: 0x004E5FDC File Offset: 0x004E41DC
		public void OnGUI(LocalTargetInfo target)
		{
			Widgets.MouseAttachedLabel("TargetGizmoMouse".Translate(), 0f, 0f, null);
			if (this.ValidateTarget(target, false))
			{
				GenUI.DrawMouseAttachment(TexCommand.Attack);
				return;
			}
			GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
		}

		// Token: 0x0601108A RID: 69770 RVA: 0x004E602C File Offset: 0x004E422C
		public virtual bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (!target.HasThing)
			{
				return false;
			}
			Thing thing = target.Thing;
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				if (this.Props.psychicSensitiveTargetsOnly && pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1) <= 0f)
				{
					return false;
				}
				if (this.Props.ignoreQuestLodgerPawns && pawn.IsQuestLodger())
				{
					return false;
				}
				if (this.Props.ignorePlayerFactionPawns && pawn.Faction == Faction.OfPlayer)
				{
					return false;
				}
				if (this.Props.cannotHaveHediff != null && pawn.health.hediffSet.HasHediff(this.Props.cannotHaveHediff, false))
				{
					if (showMessages)
					{
						Messages.Message("CannotUseDueToHediff".Translate(this.parent.Named("TARGETABLE"), pawn.Named("TARGET"), this.Props.cannotHaveHediff.label), MessageTypeDefOf.RejectInput, true);
					}
					return false;
				}
			}
			if (this.Props.fleshCorpsesOnly)
			{
				Corpse corpse = thing as Corpse;
				if (corpse != null && !corpse.InnerPawn.RaceProps.IsFlesh)
				{
					return false;
				}
			}
			if (this.Props.nonDessicatedCorpsesOnly)
			{
				Corpse corpse2 = thing as Corpse;
				if (corpse2 != null && corpse2.GetRotStage() == RotStage.Dessicated)
				{
					return false;
				}
			}
			if (ModsConfig.AnomalyActive && this.Props.mutantFilter != null)
			{
				Corpse corpse3 = thing as Corpse;
				if (corpse3 == null || !corpse3.InnerPawn.IsMutant || corpse3.InnerPawn.mutant.Def != this.Props.mutantFilter)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0400B106 RID: 45318
		private Thing selectedTarget;

		// Token: 0x0400B107 RID: 45319
		private Pawn caster;
	}
}
