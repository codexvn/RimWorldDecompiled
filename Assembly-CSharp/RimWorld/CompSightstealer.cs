using System;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002E1C RID: 11804
	public class CompSightstealer : ThingComp
	{
		// Token: 0x17002A98 RID: 10904
		// (get) Token: 0x060109FF RID: 68095 RVA: 0x0049B545 File Offset: 0x00499745
		private Pawn Sightstealer
		{
			get
			{
				return (Pawn)this.parent;
			}
		}

		// Token: 0x17002A99 RID: 10905
		// (get) Token: 0x06010A00 RID: 68096 RVA: 0x004CB5CC File Offset: 0x004C97CC
		private HediffComp_Invisibility Invisibility
		{
			get
			{
				HediffComp_Invisibility result;
				if ((result = this.invisibility) == null)
				{
					Hediff firstHediffOfDef = this.Sightstealer.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.HoraxianInvisibility, false);
					result = (this.invisibility = ((firstHediffOfDef != null) ? firstHediffOfDef.TryGetComp<HediffComp_Invisibility>() : null));
				}
				return result;
			}
		}

		// Token: 0x06010A01 RID: 68097 RVA: 0x004CB613 File Offset: 0x004C9813
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastDetectedTick, "lastDetectedTick", 0, false);
		}

		// Token: 0x06010A02 RID: 68098 RVA: 0x004CB628 File Offset: 0x004C9828
		public override void CompTick()
		{
			if (this.Sightstealer.IsShambler)
			{
				return;
			}
			if (this.Invisibility == null)
			{
				this.Sightstealer.health.AddHediff(HediffDefOf.HoraxianInvisibility, null, null, null);
			}
			if (!this.Sightstealer.Spawned)
			{
				return;
			}
			if (this.Sightstealer.IsHashIntervalTick(7))
			{
				if (Find.TickManager.TicksGame > this.lastDetectedTick + 1200)
				{
					this.CheckDetected();
				}
				if (Find.TickManager.TicksGame > this.lastDetectedTick + 1200)
				{
					this.Invisibility.BecomeInvisible(false);
				}
			}
			Lord lord = this.Sightstealer.GetLord();
			if (lord != null && Rand.MTBEventOccurs(600f, 1f, 1f))
			{
				Job curJob = this.Sightstealer.CurJob;
				if (((curJob != null) ? curJob.def : null) == JobDefOf.Wait || lord.LordJob is LordJob_EntitySwarm)
				{
					Pawn_CallTracker caller = this.Sightstealer.caller;
					if (caller == null)
					{
						return;
					}
					caller.DoCall(false);
				}
			}
		}

		// Token: 0x06010A03 RID: 68099 RVA: 0x004CB730 File Offset: 0x004C9930
		private void CheckDetected()
		{
			foreach (Thing thing in this.Sightstealer.Map.listerThings.ThingsInGroup(ThingRequestGroup.Pawn))
			{
				Pawn pawn = (Pawn)thing;
				if (this.PawnCanDetect(pawn))
				{
					if (!this.Invisibility.PsychologicallyVisible)
					{
						this.Invisibility.BecomeVisible(false);
					}
					this.lastDetectedTick = Find.TickManager.TicksGame;
				}
			}
		}

		// Token: 0x06010A04 RID: 68100 RVA: 0x004CB7C4 File Offset: 0x004C99C4
		private bool PawnCanDetect(Pawn pawn)
		{
			return pawn.Faction != Faction.OfEntities && !pawn.Downed && pawn.Awake() && !pawn.IsAnimal && this.Sightstealer.Position.InHorDistOf(pawn.Position, CompSightstealer.GetPawnSightRadius(pawn, this.Sightstealer)) && GenSight.LineOfSightToThing(pawn.Position, this.Sightstealer, this.parent.Map, false, null);
		}

		// Token: 0x06010A05 RID: 68101 RVA: 0x004CB844 File Offset: 0x004C9A44
		private static float GetPawnSightRadius(Pawn pawn, Pawn sightstealer)
		{
			float num = 14f;
			if (pawn.genes == null || pawn.genes.AffectedByDarkness)
			{
				float t = sightstealer.Map.glowGrid.GroundGlowAt(sightstealer.Position, false, false);
				num *= Mathf.Lerp(0.33f, 1f, t);
			}
			return num * pawn.health.capacities.GetLevel(PawnCapacityDefOf.Sight);
		}

		// Token: 0x06010A06 RID: 68102 RVA: 0x004CB8B1 File Offset: 0x004C9AB1
		public override void Notify_UsedVerb(Pawn pawn, Verb verb)
		{
			base.Notify_UsedVerb(pawn, verb);
			if (this.Sightstealer.IsShambler)
			{
				return;
			}
			this.Invisibility.BecomeVisible(false);
			this.lastDetectedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06010A07 RID: 68103 RVA: 0x004CB8E8 File Offset: 0x004C9AE8
		public override void Notify_BecameVisible()
		{
			SoundDefOf.Pawn_Sightstealer_Howl.PlayOneShotOnCamera(null);
			foreach (Thing thing in this.Sightstealer.MapHeld.listerThings.ThingsInGroup(ThingRequestGroup.Pawn))
			{
				Pawn pawn = (Pawn)thing;
				if (pawn.kindDef == PawnKindDefOf.Sightstealer && pawn != this.Sightstealer && pawn.Position.InHorDistOf(this.Sightstealer.Position, 30f) && !pawn.IsPsychologicallyInvisible() && GenSight.LineOfSight(pawn.Position, this.Sightstealer.Position, pawn.Map))
				{
					return;
				}
			}
			if (RealTime.LastRealTime > CompSightstealer.lastNotified + 60f)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelSightstealerRevealed".Translate(), "LetterSightstealerRevealed".Translate(), LetterDefOf.ThreatBig, this.Sightstealer, null, null, null, null, 6, true);
			}
			else
			{
				Messages.Message("MessageSightstealerRevealed".Translate(), this.Sightstealer, MessageTypeDefOf.ThreatBig, true);
			}
			CompSightstealer.lastNotified = RealTime.LastRealTime;
			this.lastDetectedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0400AD3D RID: 44349
		private const float BaseVisibleRadius = 14f;

		// Token: 0x0400AD3E RID: 44350
		private const int UndetectedTimeout = 1200;

		// Token: 0x0400AD3F RID: 44351
		private const int CheckDetectedIntervalTicks = 7;

		// Token: 0x0400AD40 RID: 44352
		private const float FirstDetectedRadius = 30f;

		// Token: 0x0400AD41 RID: 44353
		private const int RevealedLetterDelayTicks = 6;

		// Token: 0x0400AD42 RID: 44354
		private const int AmbushCallMTBTicks = 600;

		// Token: 0x0400AD43 RID: 44355
		[Unsaved(false)]
		private HediffComp_Invisibility invisibility;

		// Token: 0x0400AD44 RID: 44356
		private int lastDetectedTick = -99999;

		// Token: 0x0400AD45 RID: 44357
		private static float lastNotified = -99999f;

		// Token: 0x0400AD46 RID: 44358
		private const float NotifyCooldownSeconds = 60f;
	}
}
