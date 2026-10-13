using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B17 RID: 11031
	public class StunHandler : IExposable
	{
		// Token: 0x17002693 RID: 9875
		// (get) Token: 0x0600F66C RID: 63084 RVA: 0x00478A31 File Offset: 0x00476C31
		public bool Stunned
		{
			get
			{
				return this.stunTicksLeft > 0 || this.Hypnotized;
			}
		}

		// Token: 0x17002694 RID: 9876
		// (get) Token: 0x0600F66D RID: 63085 RVA: 0x00478A44 File Offset: 0x00476C44
		public bool StunFromEMP
		{
			get
			{
				return this.stunFromEMP;
			}
		}

		// Token: 0x17002695 RID: 9877
		// (get) Token: 0x0600F66E RID: 63086 RVA: 0x00478A4C File Offset: 0x00476C4C
		public int StunTicksLeft
		{
			get
			{
				return this.stunTicksLeft;
			}
		}

		// Token: 0x17002696 RID: 9878
		// (get) Token: 0x0600F66F RID: 63087 RVA: 0x00478A54 File Offset: 0x00476C54
		public bool DisableRotation
		{
			get
			{
				return this.disableRotation || this.Hypnotized;
			}
		}

		// Token: 0x17002697 RID: 9879
		// (get) Token: 0x0600F670 RID: 63088 RVA: 0x00478A66 File Offset: 0x00476C66
		public bool Hypnotized
		{
			get
			{
				return ModsConfig.AnomalyActive && this.parentPawn != null && Find.Anomaly.IsPawnHypnotized(this.parentPawn);
			}
		}

		// Token: 0x0600F671 RID: 63089 RVA: 0x00478A89 File Offset: 0x00476C89
		public StunHandler(Thing parent)
		{
			this.parent = parent;
			this.parentPawn = (parent as Pawn);
			this.stunnableComp = parent.TryGetComp<CompStunnable>();
			this.adaptationTicksLeft = new Dictionary<DamageDef, int>();
		}

		// Token: 0x0600F672 RID: 63090 RVA: 0x00478AC4 File Offset: 0x00476CC4
		public void StunHandlerTick()
		{
			if (this.adaptationTicksLeft.Count > 0)
			{
				StunHandler.tmpAdaptationTicksLeft.Clear();
				StunHandler.tmpAdaptationTicksLeft.AddRange(this.adaptationTicksLeft);
				foreach (KeyValuePair<DamageDef, int> keyValuePair in StunHandler.tmpAdaptationTicksLeft)
				{
					DamageDef damageDef;
					int num;
					keyValuePair.Deconstruct(out damageDef, out num);
					DamageDef damageDef2 = damageDef;
					if (num > 0)
					{
						Dictionary<DamageDef, int> dictionary = this.adaptationTicksLeft;
						damageDef = damageDef2;
						num = dictionary[damageDef];
						dictionary[damageDef] = num - 1;
					}
				}
			}
			if (this.stunTicksLeft > 0)
			{
				this.stunTicksLeft--;
				if (this.showStunMote && (this.moteStun == null || this.moteStun.Destroyed) && !this.Hypnotized)
				{
					this.moteStun = MoteMaker.MakeStunOverlay(this.parent);
				}
				Pawn pawn = this.parent as Pawn;
				if (pawn != null && pawn.Downed)
				{
					this.stunTicksLeft = 0;
				}
				Mote mote = this.moteStun;
				if (mote != null)
				{
					mote.Maintain();
				}
				if (this.stunFromEMP)
				{
					if (this.empEffecter == null)
					{
						if (this.stunnableComp != null && this.stunnableComp.UseLargeEMPEffecter)
						{
							this.empEffecter = this.CreateLargeEffecter();
						}
						else
						{
							this.empEffecter = EffecterDefOf.DisabledByEMP.Spawn();
						}
					}
					this.empEffecter.EffectTick(this.parent, this.parent);
					return;
				}
			}
			else
			{
				if (this.empEffecter != null)
				{
					this.empEffecter.Cleanup();
					this.empEffecter = null;
					this.stunFromEMP = false;
				}
				this.disableRotation = false;
			}
		}

		// Token: 0x0600F673 RID: 63091 RVA: 0x00478C78 File Offset: 0x00476E78
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (!this.CanBeStunnedByDamage(dinfo.Def))
			{
				return;
			}
			int? constantStunDurationTicks = dinfo.Def.constantStunDurationTicks;
			float num = (constantStunDurationTicks != null) ? ((float)constantStunDurationTicks.GetValueOrDefault()) : (dinfo.Amount * 30f);
			if (this.CanAdaptToDamage(dinfo.Def))
			{
				int num2;
				if ((this.adaptationTicksLeft.TryGetValue(dinfo.Def, out num2) ? num2 : 0) <= 0)
				{
					this.adaptationTicksLeft.SetOrAdd(dinfo.Def, dinfo.Def.stunAdaptationTicks);
				}
				else
				{
					if (dinfo.Def.displayAdaptedTextMote)
					{
						Vector3 loc = new Vector3((float)this.parent.Position.x + 1f, (float)this.parent.Position.y, (float)this.parent.Position.z + 1f);
						string text = dinfo.Def.adaptedText ?? "Adapted".Translate();
						MoteMaker.ThrowText(loc, this.parent.Map, text, Color.white, -1f);
					}
					num = 0f;
				}
			}
			if (num > 0f)
			{
				if (dinfo.Def.stunResistStat != null && !dinfo.Def.stunResistStat.Worker.IsDisabledFor(this.parent))
				{
					num *= Mathf.Clamp01(1f - this.parent.GetStatValue(dinfo.Def.stunResistStat, true, -1));
				}
				if (dinfo.Def == DamageDefOf.EMP)
				{
					this.stunFromEMP = true;
				}
				this.StunFor(Mathf.RoundToInt(num), dinfo.Instigator, true, true, false);
			}
		}

		// Token: 0x0600F674 RID: 63092 RVA: 0x00478E30 File Offset: 0x00477030
		public void StunFor(int ticks, Thing instigator, bool addBattleLog = true, bool showMote = true, bool disableRotation = false)
		{
			this.stunTicksLeft = Mathf.Max(this.stunTicksLeft, ticks);
			this.showStunMote = showMote;
			this.disableRotation = disableRotation;
			if (addBattleLog)
			{
				Find.BattleLog.Add(new BattleLogEntry_Event(this.parent, RulePackDefOf.Event_Stun, instigator));
			}
		}

		// Token: 0x0600F675 RID: 63093 RVA: 0x00478E7D File Offset: 0x0047707D
		public void StopStun()
		{
			this.stunTicksLeft = 0;
		}

		// Token: 0x0600F676 RID: 63094 RVA: 0x00478E88 File Offset: 0x00477088
		private Effecter CreateLargeEffecter()
		{
			Effecter effecter = EffecterDefOf.DisabledByEMPLarge.Spawn();
			for (int i = 0; i < effecter.children.Count; i++)
			{
				SubEffecter_SprayerChance subEffecter_SprayerChance = effecter.children[i] as SubEffecter_SprayerChance;
				if (subEffecter_SprayerChance != null)
				{
					if (this.stunnableComp.EMPEffecterDimensions != null || this.stunnableComp.EMPEffecterOffset != null)
					{
						subEffecter_SprayerChance.spawnLocOverride = new MoteSpawnLocType?(MoteSpawnLocType.OnSource);
					}
					subEffecter_SprayerChance.dimensionsOverride = this.stunnableComp.EMPEffecterDimensions;
					subEffecter_SprayerChance.offsetOverride = this.stunnableComp.EMPEffecterOffset;
					subEffecter_SprayerChance.chanceOverride = this.stunnableComp.EMPChancePerTick;
				}
			}
			return effecter;
		}

		// Token: 0x0600F677 RID: 63095 RVA: 0x00478F3C File Offset: 0x0047713C
		private bool CanBeStunnedByDamage(DamageDef def)
		{
			if (!def.causeStun)
			{
				return false;
			}
			if (this.stunnableComp != null && !this.stunnableComp.CanBeStunnedByDamage(def))
			{
				return false;
			}
			Pawn pawn = this.parent as Pawn;
			return pawn == null || (!pawn.Downed && !pawn.Dead && (!ModsConfig.AnomalyActive || !pawn.health.hediffSet.HasHediff(HediffDefOf.AwokenCorpse, false)) && (def == DamageDefOf.Stun || (def == DamageDefOf.EMP && !pawn.RaceProps.IsFlesh) || (ModsConfig.BiotechActive && def == DamageDefOf.MechBandShockwave && pawn.RaceProps.IsMechanoid) || (ModsConfig.AnomalyActive && def == DamageDefOf.NerveStun && !pawn.RaceProps.IsMechanoid)));
		}

		// Token: 0x0600F678 RID: 63096 RVA: 0x00479010 File Offset: 0x00477210
		private bool CanAdaptToDamage(DamageDef def)
		{
			if (def.stunAdaptationTicks <= 0)
			{
				return false;
			}
			if (this.stunnableComp != null && this.stunnableComp.CanAdaptToDamage(def))
			{
				return true;
			}
			Pawn pawn = this.parent as Pawn;
			if (pawn != null)
			{
				if (def == DamageDefOf.EMP)
				{
					return (ModsConfig.AnomalyActive && (pawn.kindDef == PawnKindDefOf.Revenant || pawn.kindDef == PawnKindDefOf.Nociosphere)) || pawn.RaceProps.IsMechanoid || pawn.RaceProps.IsDrone;
				}
				if (ModsConfig.AnomalyActive && def == DamageDefOf.NerveStun)
				{
					return !pawn.RaceProps.IsMechanoid && !pawn.RaceProps.IsDrone;
				}
			}
			return false;
		}

		// Token: 0x0600F679 RID: 63097 RVA: 0x004790C4 File Offset: 0x004772C4
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.stunTicksLeft, "stunTicksLeft", 0, false);
			Scribe_Values.Look<bool>(ref this.showStunMote, "showStunMote", false, false);
			Scribe_Values.Look<bool>(ref this.stunFromEMP, "stunFromEMP", false, false);
			Scribe_Values.Look<bool>(ref this.disableRotation, "disableRotation", false, false);
			Scribe_Collections.Look<DamageDef, int>(ref this.adaptationTicksLeft, "adaptationTicksLeft", LookMode.Def, LookMode.Value);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.adaptationTicksLeft == null)
				{
					this.adaptationTicksLeft = new Dictionary<DamageDef, int>();
				}
				int num = 0;
				Scribe_Values.Look<int>(ref num, "EMPAdaptedTicksLeft", 0, false);
				if (num != 0)
				{
					this.adaptationTicksLeft.SetOrAdd(DamageDefOf.EMP, num);
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.adaptationTicksLeft.RemoveAll((KeyValuePair<DamageDef, int> x) => x.Value <= 0);
			}
		}

		// Token: 0x0400A234 RID: 41524
		public Thing parent;

		// Token: 0x0400A235 RID: 41525
		private Pawn parentPawn;

		// Token: 0x0400A236 RID: 41526
		private int stunTicksLeft;

		// Token: 0x0400A237 RID: 41527
		private Mote moteStun;

		// Token: 0x0400A238 RID: 41528
		private bool showStunMote = true;

		// Token: 0x0400A239 RID: 41529
		private Effecter empEffecter;

		// Token: 0x0400A23A RID: 41530
		private bool stunFromEMP;

		// Token: 0x0400A23B RID: 41531
		private Dictionary<DamageDef, int> adaptationTicksLeft;

		// Token: 0x0400A23C RID: 41532
		private bool disableRotation;

		// Token: 0x0400A23D RID: 41533
		private CompStunnable stunnableComp;

		// Token: 0x0400A23E RID: 41534
		public const float StunDurationTicksPerDamage = 30f;

		// Token: 0x0400A23F RID: 41535
		private static readonly List<KeyValuePair<DamageDef, int>> tmpAdaptationTicksLeft = new List<KeyValuePair<DamageDef, int>>();
	}
}
