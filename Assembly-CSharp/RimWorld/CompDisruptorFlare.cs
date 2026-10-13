using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C62 RID: 11362
	public class CompDisruptorFlare : ThingComp
	{
		// Token: 0x17002824 RID: 10276
		// (get) Token: 0x0600FEC9 RID: 65225 RVA: 0x0049D737 File Offset: 0x0049B937
		public CompProperties_DisruptorFlare Props
		{
			get
			{
				return (CompProperties_DisruptorFlare)this.props;
			}
		}

		// Token: 0x17002825 RID: 10277
		// (get) Token: 0x0600FECA RID: 65226 RVA: 0x0049D744 File Offset: 0x0049B944
		private int WarnTick
		{
			get
			{
				return this.destroyDelayedComp.DestructionTick - 900;
			}
		}

		// Token: 0x17002826 RID: 10278
		// (get) Token: 0x0600FECB RID: 65227 RVA: 0x0049D757 File Offset: 0x0049B957
		private float WarnAlpha
		{
			get
			{
				return Mathf.InverseLerp((float)this.WarnTick, (float)this.destroyDelayedComp.DestructionTick, (float)Find.TickManager.TicksGame);
			}
		}

		// Token: 0x17002827 RID: 10279
		// (get) Token: 0x0600FECC RID: 65228 RVA: 0x0049D77C File Offset: 0x0049B97C
		private float GlowRadius
		{
			get
			{
				return this.Props.radius + 2f;
			}
		}

		// Token: 0x0600FECD RID: 65229 RVA: 0x0049D790 File Offset: 0x0049B990
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.glowComp = this.parent.GetComp<CompGlower>();
			if (this.glowComp != null)
			{
				this.glowComp.GlowRadius = this.GlowRadius;
			}
			this.destroyDelayedComp = this.parent.GetComp<CompDestroyAfterDelay>();
			if (respawningAfterLoad)
			{
				return;
			}
			foreach (Pawn pawn in this.parent.Map.mapPawns.AllPawnsSpawned.ToList<Pawn>())
			{
				if (pawn.Spawned && pawn.Position.InHorDistOf(this.parent.Position, this.Props.radius) && GenSight.LineOfSightToThing(this.parent.Position, pawn, this.parent.Map, false, null))
				{
					if (!pawn.health.hediffSet.HasHediff(HediffDefOf.DisruptorFlash, false))
					{
						this.PsychicStun(pawn);
					}
					pawn.health.AddHediff(HediffDefOf.DisruptorFlash, null, null, null);
					CompRevenant compRevenant = pawn.TryGetComp<CompRevenant>();
					if (compRevenant != null && pawn.IsPsychologicallyInvisible())
					{
						compRevenant.SetState(RevenantState.Escape);
					}
				}
			}
		}

		// Token: 0x0600FECE RID: 65230 RVA: 0x0049D8E4 File Offset: 0x0049BAE4
		private void PsychicStun(Pawn pawn)
		{
			float statValue = pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1);
			if (statValue > 0f)
			{
				Pawn_StanceTracker stances = pawn.stances;
				if (stances == null)
				{
					return;
				}
				StunHandler stunner = stances.stunner;
				if (stunner == null)
				{
					return;
				}
				stunner.StunFor(Mathf.RoundToInt(120f * statValue), null, true, true, false);
			}
		}

		// Token: 0x0600FECF RID: 65231 RVA: 0x0049D934 File Offset: 0x0049BB34
		public override void CompTick()
		{
			if (this.parent.Spawned)
			{
				if (this.attachedEffecter == null)
				{
					this.attachedEffecter = this.Props.effecterDef.SpawnAttached(this.parent, this.parent.MapHeld, 1f);
				}
				if (this.WarnTick - Find.TickManager.TicksGame > 80)
				{
					Effecter effecter = this.attachedEffecter;
					if (effecter != null)
					{
						effecter.EffectTick(this.parent, this.parent);
					}
				}
			}
			else
			{
				Effecter effecter2 = this.attachedEffecter;
				if (effecter2 != null)
				{
					effecter2.Cleanup();
				}
				this.attachedEffecter = null;
			}
			if (Find.TickManager.TicksGame >= this.WarnTick)
			{
				if (this.destroyWarningEffecter == null)
				{
					this.destroyWarningEffecter = this.Props.destroyWarningEffecterDef.SpawnAttached(this.parent, this.parent.MapHeld, 1f);
				}
				this.destroyWarningEffecter.EffectTick(this.parent, this.parent);
				float warnAlpha = this.WarnAlpha;
				ColorInt glowColor = this.glowComp.GlowColor;
				glowColor.a = (int)((byte)Mathf.Lerp((float)glowColor.a, 0f, warnAlpha));
				this.glowComp.GlowRadius = Mathf.Lerp(this.GlowRadius, 0.1f, warnAlpha);
				this.glowComp.GlowColor = glowColor;
				if (this.attachedEffecter != null)
				{
					this.attachedEffecter.children[0].colorOverride = new Color?(new Color(1f, 1f, 1f, warnAlpha));
				}
			}
		}

		// Token: 0x0400A6E4 RID: 42724
		private const int BaseStunDuration = 120;

		// Token: 0x0400A6E5 RID: 42725
		private const int DestroyWarningTicks = 900;

		// Token: 0x0400A6E6 RID: 42726
		private Effecter destroyWarningEffecter;

		// Token: 0x0400A6E7 RID: 42727
		private CompDestroyAfterDelay destroyDelayedComp;

		// Token: 0x0400A6E8 RID: 42728
		private Effecter attachedEffecter;

		// Token: 0x0400A6E9 RID: 42729
		private CompGlower glowComp;
	}
}
