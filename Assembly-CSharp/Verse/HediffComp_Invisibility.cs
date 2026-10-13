using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020006D5 RID: 1749
	public class HediffComp_Invisibility : HediffComp
	{
		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x06002B5C RID: 11100 RVA: 0x000E2C14 File Offset: 0x000E0E14
		private CompHoldingPlatformTarget PlatformTarget
		{
			get
			{
				CompHoldingPlatformTarget result;
				if ((result = this.platformTarget) == null)
				{
					result = (this.platformTarget = base.Pawn.TryGetComp<CompHoldingPlatformTarget>());
				}
				return result;
			}
		}

		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x06002B5D RID: 11101 RVA: 0x000E2C3F File Offset: 0x000E0E3F
		public HediffCompProperties_Invisibility Props
		{
			get
			{
				return (HediffCompProperties_Invisibility)this.props;
			}
		}

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x06002B5E RID: 11102 RVA: 0x000E2C4C File Offset: 0x000E0E4C
		private float FadeIn
		{
			get
			{
				return Mathf.Clamp01((float)(Find.TickManager.TicksGame - base.Pawn.mindState.lastBecameVisibleTick) / (float)this.Props.fadeDurationTicks);
			}
		}

		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x06002B5F RID: 11103 RVA: 0x000E2C7C File Offset: 0x000E0E7C
		private float FadeOut
		{
			get
			{
				return 1f - Mathf.Clamp01((float)(Find.TickManager.TicksGame - base.Pawn.mindState.lastBecameInvisibleTick) / (float)this.Props.fadeDurationTicks);
			}
		}

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x06002B60 RID: 11104 RVA: 0x000E2CB2 File Offset: 0x000E0EB2
		private bool ShouldBeVisible
		{
			get
			{
				return base.Pawn.mindState.lastBecameVisibleTick >= base.Pawn.mindState.lastBecameInvisibleTick;
			}
		}

		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x06002B61 RID: 11105 RVA: 0x000E2CD9 File Offset: 0x000E0ED9
		private float FadePct
		{
			get
			{
				if (!this.ShouldBeVisible)
				{
					return this.FadeOut;
				}
				return this.FadeIn;
			}
		}

		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x06002B62 RID: 11106 RVA: 0x000E2CF0 File Offset: 0x000E0EF0
		public bool PsychologicallyVisible
		{
			get
			{
				return this.ForcedVisible || this.FadePct > 0f;
			}
		}

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x06002B63 RID: 11107 RVA: 0x000E2D0C File Offset: 0x000E0F0C
		private bool ForcedVisible
		{
			get
			{
				if (base.Pawn.Downed)
				{
					return true;
				}
				if (Find.TickManager.TicksGame < this.lastDisrupted + this.Props.recoverFromDisruptedTicks)
				{
					return true;
				}
				if (base.Pawn.Faction == null || !base.Pawn.Faction.IsPlayer)
				{
					Pawn_StanceTracker stances = base.Pawn.stances;
					bool? flag;
					if (stances == null)
					{
						flag = null;
					}
					else
					{
						StunHandler stunner = stances.stunner;
						flag = ((stunner != null) ? new bool?(stunner.Stunned) : null);
					}
					bool? flag2 = flag;
					if (flag2.GetValueOrDefault())
					{
						return true;
					}
				}
				if (base.Pawn.IsBurning())
				{
					return true;
				}
				if (this.Props.affectedByDisruptor && base.Pawn.health.hediffSet.HasHediff(HediffDefOf.DisruptorFlash, false))
				{
					return true;
				}
				if (base.Pawn.health.hediffSet.HasHediff(HediffDefOf.CoveredInFirefoam, false))
				{
					return true;
				}
				if (base.Pawn.ParentHolder is Pawn_CarryTracker)
				{
					return true;
				}
				CompHoldingPlatformTarget compHoldingPlatformTarget = this.PlatformTarget;
				return compHoldingPlatformTarget != null && compHoldingPlatformTarget.CurrentlyHeldOnPlatform;
			}
		}

		// Token: 0x06002B64 RID: 11108 RVA: 0x000E2E30 File Offset: 0x000E1030
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<bool>(ref this.wasForcedVisibleLastTick, "wasForcedVisibleLastTick", false, false);
			Scribe_Values.Look<int>(ref this.lastDisrupted, "lastTookDamageTick", -99999, false);
			Scribe_Values.Look<bool>(ref this.everVisible, "everVisible", false, false);
		}

		// Token: 0x06002B65 RID: 11109 RVA: 0x000E2E7D File Offset: 0x000E107D
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			this.BecomeInvisible(true);
			this.UpdateTarget();
		}

		// Token: 0x06002B66 RID: 11110 RVA: 0x000E2E93 File Offset: 0x000E1093
		public override void CompPostPostRemoved()
		{
			base.CompPostPostRemoved();
			this.UpdateTarget();
		}

		// Token: 0x06002B67 RID: 11111 RVA: 0x000E2EA4 File Offset: 0x000E10A4
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (!this.ShouldBeVisible)
			{
				if (!this.wasForcedVisibleLastTick && this.ForcedVisible && this.FadePct == 0f)
				{
					base.Pawn.mindState.lastForcedVisibleTick = Find.TickManager.TicksGame;
					EffecterDefOf.ForcedVisible.Spawn(this.parent.pawn, this.parent.pawn.MapHeld, 1f).Cleanup();
					base.Pawn.Notify_ForcedVisible();
				}
				if (this.wasForcedVisibleLastTick && !this.ForcedVisible)
				{
					base.Pawn.mindState.lastBecameInvisibleTick = Find.TickManager.TicksGame;
					base.Pawn.Notify_BecameInvisible();
				}
			}
			this.wasForcedVisibleLastTick = this.ForcedVisible;
			if (!this.everVisible && this.PsychologicallyVisible)
			{
				this.everVisible = true;
				List<EntityCodexEntryDef> entries;
				if (ModsConfig.AnomalyActive && !this.Props.visibleToPlayer && AnomalyUtility.ShouldNotifyCodex(base.Pawn, EntityDiscoveryType.BecameVisible, out entries))
				{
					Find.EntityCodex.SetDiscovered(entries, base.Pawn.def, base.Pawn);
					return;
				}
				Find.HiddenItemsManager.SetDiscovered(base.Pawn.def);
			}
		}

		// Token: 0x06002B68 RID: 11112 RVA: 0x000E2FDC File Offset: 0x000E11DC
		private void UpdateTarget()
		{
			if (!ModLister.CheckRoyaltyOrAnomaly("Invisibility hediff"))
			{
				return;
			}
			Pawn pawn = this.parent.pawn;
			if (pawn.Spawned)
			{
				pawn.Map.attackTargetsCache.UpdateTarget(pawn);
			}
			if (pawn.RaceProps.Humanlike)
			{
				PortraitsCache.SetDirty(pawn);
				GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(pawn);
			}
		}

		// Token: 0x06002B69 RID: 11113 RVA: 0x000E3038 File Offset: 0x000E1238
		public void BecomeVisible(bool instant = false)
		{
			if (this.ShouldBeVisible)
			{
				return;
			}
			if (this.wasForcedVisibleLastTick)
			{
				instant = true;
			}
			if (instant)
			{
				EffecterDefOf.ForcedVisible.Spawn(this.parent.pawn, this.parent.pawn.Map, 1f).Cleanup();
				base.Pawn.mindState.lastBecameVisibleTick = Find.TickManager.TicksGame - this.Props.fadeDurationTicks;
			}
			else
			{
				base.Pawn.mindState.lastBecameVisibleTick = Find.TickManager.TicksGame;
			}
			if (!this.ForcedVisible && !this.wasForcedVisibleLastTick)
			{
				base.Pawn.Notify_BecameVisible();
			}
		}

		// Token: 0x06002B6A RID: 11114 RVA: 0x000E30E8 File Offset: 0x000E12E8
		public void BecomeInvisible(bool instant = false)
		{
			if (!this.ShouldBeVisible)
			{
				return;
			}
			if (instant)
			{
				base.Pawn.mindState.lastBecameInvisibleTick = Find.TickManager.TicksGame - this.Props.fadeDurationTicks;
			}
			else
			{
				base.Pawn.mindState.lastBecameInvisibleTick = Find.TickManager.TicksGame;
			}
			if (!this.ForcedVisible)
			{
				base.Pawn.Notify_BecameInvisible();
			}
			base.Pawn.Drawer.renderer.SetAllGraphicsDirty();
		}

		// Token: 0x06002B6B RID: 11115 RVA: 0x000E316B File Offset: 0x000E136B
		public float GetAlpha()
		{
			if (this.Props.visibleToPlayer)
			{
				return 1f;
			}
			if (this.ForcedVisible)
			{
				return 1f;
			}
			return HediffComp_Invisibility.RevealCurve.Evaluate(this.FadePct);
		}

		// Token: 0x06002B6C RID: 11116 RVA: 0x000E319E File Offset: 0x000E139E
		public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			this.DisruptInvisibility();
		}

		// Token: 0x06002B6D RID: 11117 RVA: 0x000E31A6 File Offset: 0x000E13A6
		public void DisruptInvisibility()
		{
			this.lastDisrupted = Find.TickManager.TicksGame;
		}

		// Token: 0x040022F9 RID: 8953
		private int lastDisrupted = -99999;

		// Token: 0x040022FA RID: 8954
		private bool wasForcedVisibleLastTick;

		// Token: 0x040022FB RID: 8955
		private bool everVisible;

		// Token: 0x040022FC RID: 8956
		private static readonly SimpleCurve RevealCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(0.1f, 0.5f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			}
		};

		// Token: 0x040022FD RID: 8957
		[Unsaved(false)]
		private CompHoldingPlatformTarget platformTarget;
	}
}
