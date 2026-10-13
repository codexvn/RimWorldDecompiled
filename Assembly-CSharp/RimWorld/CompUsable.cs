using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002EBC RID: 11964
	public class CompUsable : ThingComp, ITargetingSource
	{
		// Token: 0x17002B83 RID: 11139
		// (get) Token: 0x06010E28 RID: 69160 RVA: 0x004DCCAB File Offset: 0x004DAEAB
		public CompProperties_Usable Props
		{
			get
			{
				return (CompProperties_Usable)this.props;
			}
		}

		// Token: 0x17002B84 RID: 11140
		// (get) Token: 0x06010E29 RID: 69161 RVA: 0x004DCCB8 File Offset: 0x004DAEB8
		private Texture2D Icon
		{
			get
			{
				if (this.icon == null && this.Props.floatMenuFactionIcon != null)
				{
					Faction faction = Find.FactionManager.FirstFactionOfDef(this.Props.floatMenuFactionIcon);
					Texture2D texture2D;
					if (faction == null)
					{
						texture2D = null;
					}
					else
					{
						FactionDef def = faction.def;
						texture2D = ((def != null) ? def.FactionIcon : null);
					}
					this.icon = texture2D;
				}
				return this.icon;
			}
		}

		// Token: 0x17002B85 RID: 11141
		// (get) Token: 0x06010E2A RID: 69162 RVA: 0x004DCD1C File Offset: 0x004DAF1C
		private Color IconColor
		{
			get
			{
				if (this.iconColor == null && this.Props.floatMenuFactionIcon != null)
				{
					Faction faction = Find.FactionManager.FirstFactionOfDef(this.Props.floatMenuFactionIcon);
					this.iconColor = ((faction != null) ? new Color?(faction.Color) : null);
				}
				Color? color = this.iconColor;
				if (color == null)
				{
					return Color.white;
				}
				return color.GetValueOrDefault();
			}
		}

		// Token: 0x17002B86 RID: 11142
		// (get) Token: 0x06010E2B RID: 69163 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool CasterIsPawn
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002B87 RID: 11143
		// (get) Token: 0x06010E2C RID: 69164 RVA: 0x00002501 File Offset: 0x00000701
		public bool IsMeleeAttack
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002B88 RID: 11144
		// (get) Token: 0x06010E2D RID: 69165 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool Targetable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002B89 RID: 11145
		// (get) Token: 0x06010E2E RID: 69166 RVA: 0x00002501 File Offset: 0x00000701
		public bool MultiSelect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002B8A RID: 11146
		// (get) Token: 0x06010E2F RID: 69167 RVA: 0x00002501 File Offset: 0x00000701
		public bool HidePawnTooltips
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002B8B RID: 11147
		// (get) Token: 0x06010E30 RID: 69168 RVA: 0x0048FDB8 File Offset: 0x0048DFB8
		public Thing Caster
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x17002B8C RID: 11148
		// (get) Token: 0x06010E31 RID: 69169 RVA: 0x00002C42 File Offset: 0x00000E42
		public Pawn CasterPawn
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17002B8D RID: 11149
		// (get) Token: 0x06010E32 RID: 69170 RVA: 0x00002C42 File Offset: 0x00000E42
		public Verb GetVerb
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17002B8E RID: 11150
		// (get) Token: 0x06010E33 RID: 69171 RVA: 0x004DCD94 File Offset: 0x004DAF94
		public TargetingParameters targetParams
		{
			get
			{
				return TargetingParameters.ForPawns();
			}
		}

		// Token: 0x17002B8F RID: 11151
		// (get) Token: 0x06010E34 RID: 69172 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual ITargetingSource DestinationSelector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17002B90 RID: 11152
		// (get) Token: 0x06010E35 RID: 69173 RVA: 0x00002C42 File Offset: 0x00000E42
		public Texture2D UIIcon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06010E36 RID: 69174 RVA: 0x004DCD9B File Offset: 0x004DAF9B
		protected virtual string FloatMenuOptionLabel(Pawn pawn)
		{
			return this.Props.useLabel.Formatted(this.parent);
		}

		// Token: 0x06010E37 RID: 69175 RVA: 0x004DCDBD File Offset: 0x004DAFBD
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn myPawn)
		{
			if (this.Props.useJob == null)
			{
				yield break;
			}
			AcceptanceReport acceptanceReport = this.CanBeUsedBy(myPawn, this.Props.ignoreOtherReservations, false);
			string label = this.FloatMenuOptionLabel(myPawn);
			Action action = delegate()
			{
				using (IEnumerator<CompUseEffect> enumerator = this.parent.GetComps<CompUseEffect>().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.SelectedUseOption(myPawn))
						{
							return;
						}
					}
				}
				this.TryStartUseJob(myPawn, this.GetExtraTarget(myPawn), this.Props.ignoreOtherReservations);
			};
			MenuOptionPriority floatMenuOptionPriority = this.Props.floatMenuOptionPriority;
			FloatMenuOption floatMenuOption = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label, action, this.Icon, this.IconColor, floatMenuOptionPriority, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false), myPawn, this.parent, "ReservedBy", null);
			if (!acceptanceReport.Accepted)
			{
				floatMenuOption.Disabled = true;
				FloatMenuOption floatMenuOption2 = floatMenuOption;
				floatMenuOption2.Label = floatMenuOption2.Label + " (" + acceptanceReport.Reason + ")";
			}
			yield return floatMenuOption;
			yield break;
		}

		// Token: 0x06010E38 RID: 69176 RVA: 0x0047604F File Offset: 0x0047424F
		public virtual LocalTargetInfo GetExtraTarget(Pawn pawn)
		{
			return LocalTargetInfo.Invalid;
		}

		// Token: 0x06010E39 RID: 69177 RVA: 0x004DCDD4 File Offset: 0x004DAFD4
		public virtual void TryStartUseJob(Pawn pawn, LocalTargetInfo extraTarget, bool forced = false)
		{
			CompUsable.<>c__DisplayClass33_0 CS$<>8__locals1 = new CompUsable.<>c__DisplayClass33_0();
			CS$<>8__locals1.extraTarget = extraTarget;
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.<>4__this = this;
			if (this.Props.useJob == null)
			{
				return;
			}
			if (!this.CanBeUsedBy(CS$<>8__locals1.pawn, forced, false))
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (CompUseEffect compUseEffect in this.parent.GetComps<CompUseEffect>())
			{
				TaggedString taggedString = compUseEffect.ConfirmMessage(CS$<>8__locals1.pawn);
				if (!taggedString.NullOrEmpty())
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.AppendTagged(taggedString);
				}
			}
			string str = stringBuilder.ToString();
			if (str.NullOrEmpty())
			{
				CS$<>8__locals1.<TryStartUseJob>g__StartJob|0();
				return;
			}
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(str, new Action(CS$<>8__locals1.<TryStartUseJob>g__StartJob|0), false, null, WindowLayer.Dialog));
		}

		// Token: 0x06010E3A RID: 69178 RVA: 0x004DCECC File Offset: 0x004DB0CC
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!this.Props.showUseGizmo)
			{
				yield break;
			}
			yield return new Command_Action
			{
				icon = this.parent.def.uiIcon,
				defaultLabel = string.Format("{0} {1}...", "UseGizmo".Translate(), this.parent.def.label),
				defaultDesc = "UseGizmoTooltip".Translate(this.parent.def.label),
				action = delegate()
				{
					SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
					Find.Targeter.BeginTargeting(this, null, false, null, null, true);
				}
			};
			yield break;
		}

		// Token: 0x06010E3B RID: 69179 RVA: 0x004DCEDC File Offset: 0x004DB0DC
		public virtual void UsedBy(Pawn p)
		{
			if (!this.CanBeUsedBy(p, false, true))
			{
				return;
			}
			foreach (CompUseEffect compUseEffect in from x in this.parent.GetComps<CompUseEffect>()
			orderby x.OrderPriority descending
			select x)
			{
				try
				{
					compUseEffect.DoEffect(p);
				}
				catch (Exception ex)
				{
					string str = "Error in CompUseEffect: ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
			if (this.Props.useMessage != null)
			{
				Messages.Message(this.Props.useMessage.Formatted(p.Named("PAWN"), this.parent), this.parent, MessageTypeDefOf.NeutralEvent, true);
			}
		}

		// Token: 0x06010E3C RID: 69180 RVA: 0x004DCFE0 File Offset: 0x004DB1E0
		public virtual AcceptanceReport CanBeUsedBy(Pawn p, bool forced = false, bool ignoreReserveAndReachable = false)
		{
			if (p.IsMutant && !this.Props.allowedMutants.Contains(p.mutant.Def))
			{
				return false;
			}
			PlanetTile tile = p.MapHeld.Tile;
			if (tile.Valid && !this.Props.layerWhitelist.NullOrEmpty<PlanetLayerDef>() && !this.Props.layerWhitelist.Contains(tile.LayerDef))
			{
				return "CannotPerformPlanetLayer".Translate(tile.LayerDef.gerundLabel.Named("GERUND"), tile.LayerDef.label.Named("LAYER")).Resolve();
			}
			if (tile.Valid && !this.Props.layerBlacklist.NullOrEmpty<PlanetLayerDef>() && this.Props.layerBlacklist.Contains(tile.LayerDef))
			{
				return "CannotPerformPlanetLayer".Translate(tile.LayerDef.gerundLabel.Named("GERUND"), tile.LayerDef.label.Named("LAYER")).Resolve();
			}
			CompPowerTrader compPowerTrader;
			if (this.parent.TryGetComp(out compPowerTrader) && !compPowerTrader.PowerOn)
			{
				return "NoPower".Translate();
			}
			if (!ignoreReserveAndReachable && !p.CanReach(this.parent, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return "NoPath".Translate();
			}
			if (!ignoreReserveAndReachable && !p.CanReserve(this.parent, 1, -1, null, forced))
			{
				Pawn pawn = p.Map.reservationManager.FirstRespectedReserver(this.parent, p, null) ?? p.Map.physicalInteractionReservationManager.FirstReserverOf(this.parent);
				if (pawn != null)
				{
					return "ReservedBy".Translate(pawn.LabelShort, pawn);
				}
				return "Reserved".Translate();
			}
			else
			{
				if (this.Props.userMustHaveHediff != null && !p.health.hediffSet.HasHediff(this.Props.userMustHaveHediff, false))
				{
					return "MustHaveHediff".Translate(this.Props.userMustHaveHediff);
				}
				List<ThingComp> allComps = this.parent.AllComps;
				for (int i = 0; i < allComps.Count; i++)
				{
					CompUseEffect compUseEffect = allComps[i] as CompUseEffect;
					if (compUseEffect != null)
					{
						AcceptanceReport result = compUseEffect.CanBeUsedBy(p);
						if (!result.Accepted)
						{
							return result;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x06010E3D RID: 69181 RVA: 0x004DD294 File Offset: 0x004DB494
		public bool CanHitTarget(LocalTargetInfo target)
		{
			return this.ValidateTarget(target, false);
		}

		// Token: 0x06010E3E RID: 69182 RVA: 0x004DD2A0 File Offset: 0x004DB4A0
		public bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			Pawn pawn;
			if (!target.IsValid || !target.TryGetPawn(out pawn) || !pawn.IsPlayerControlled)
			{
				return false;
			}
			AcceptanceReport acceptanceReport = this.CanBeUsedBy(pawn, this.Props.ignoreOtherReservations, false);
			if (!acceptanceReport.Accepted)
			{
				if (showMessages && !acceptanceReport.Reason.NullOrEmpty())
				{
					this.SendCannotUseMessage(pawn, acceptanceReport.Reason);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06010E3F RID: 69183 RVA: 0x004DD30A File Offset: 0x004DB50A
		public void SendCannotUseMessage(Pawn pawn, string reason)
		{
			Messages.Message(string.Format("{0}: {1}", "CannotGenericWorkCustom".Translate(this.FloatMenuOptionLabel(pawn)), reason.CapitalizeFirst()), pawn, MessageTypeDefOf.RejectInput, false);
		}

		// Token: 0x06010E40 RID: 69184 RVA: 0x0045EA21 File Offset: 0x0045CC21
		public void DrawHighlight(LocalTargetInfo target)
		{
			if (target.IsValid)
			{
				GenDraw.DrawTargetHighlight(target);
			}
		}

		// Token: 0x06010E41 RID: 69185 RVA: 0x004DD348 File Offset: 0x004DB548
		public void OrderForceTarget(LocalTargetInfo target)
		{
			using (IEnumerator<CompUseEffect> enumerator = this.parent.GetComps<CompUseEffect>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.SelectedUseOption(target.Pawn))
					{
						return;
					}
				}
			}
			this.TryStartUseJob(target.Pawn, this.GetExtraTarget(target.Pawn), this.Props.ignoreOtherReservations);
		}

		// Token: 0x06010E42 RID: 69186 RVA: 0x004DD3C8 File Offset: 0x004DB5C8
		public void OnGUI(LocalTargetInfo target)
		{
			Widgets.MouseAttachedLabel("UseGizmoMouse".Translate(), 0f, 0f, null);
			if (this.ValidateTarget(target, false))
			{
				GenUI.DrawMouseAttachment(TexCommand.Attack);
				return;
			}
			GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
		}

		// Token: 0x0400AFBB RID: 44987
		private Texture2D icon;

		// Token: 0x0400AFBC RID: 44988
		private Color? iconColor;
	}
}
