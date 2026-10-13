using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x0200073A RID: 1850
	public class Hediff_CubeInterest : Hediff
	{
		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x06002D15 RID: 11541 RVA: 0x000E8C70 File Offset: 0x000E6E70
		public Hediff_CubeWithdrawal WithdrawalHediff
		{
			get
			{
				Hediff_CubeWithdrawal result;
				if ((result = this.cachedHediff) == null)
				{
					result = (this.cachedHediff = this.pawn.health.hediffSet.GetFirstHediff<Hediff_CubeWithdrawal>());
				}
				return result;
			}
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x06002D16 RID: 11542 RVA: 0x000E8CA5 File Offset: 0x000E6EA5
		public bool InWithdrawal
		{
			get
			{
				return this.pawn.health.hediffSet.HasHediff(HediffDefOf.CubeWithdrawal, false);
			}
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x06002D17 RID: 11543 RVA: 0x000E8CC2 File Offset: 0x000E6EC2
		public int Sculptures
		{
			get
			{
				return this.sculptures.Count;
			}
		}

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x06002D18 RID: 11544 RVA: 0x000E8CD0 File Offset: 0x000E6ED0
		public bool InCaravanWithCube
		{
			get
			{
				Caravan caravan = this.pawn.GetCaravan();
				return caravan != null && CaravanInventoryUtility.HasThings(caravan, ThingDefOf.GoldenCube, 1, null);
			}
		}

		// Token: 0x06002D19 RID: 11545 RVA: 0x000E8CFB File Offset: 0x000E6EFB
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (!ModLister.CheckAnomaly("Cube interest"))
			{
				this.pawn.health.RemoveHediff(this);
				return;
			}
			base.PostAdd(dinfo);
		}

		// Token: 0x06002D1A RID: 11546 RVA: 0x000E8D24 File Offset: 0x000E6F24
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (this.ageTicks <= 1000)
			{
				return;
			}
			if (Rand.MTBEventOccurs(1.5f, 60000f, 1f) && !this.InWithdrawal && !this.InCaravanWithCube)
			{
				this.StartWithdrawal();
			}
			if (Rand.MTBEventOccurs(Hediff_CubeInterest.MentalBreakSeverityDaysMTB.Evaluate(this.Severity), 60000f, 1f) && !this.pawn.InMentalState && this.pawn.CurJobDef != JobDefOf.GoldenCubePlay && this.pawn.SpawnedOrAnyParentSpawned)
			{
				this.DoMentalBreak();
			}
			if (GenTicks.TicksGame % 60 == 0)
			{
				this.UpdateSculptures();
			}
		}

		// Token: 0x06002D1B RID: 11547 RVA: 0x000E8DD8 File Offset: 0x000E6FD8
		private void UpdateSculptures()
		{
			bool flag = false;
			for (int i = this.sculptures.Count - 1; i >= 0; i--)
			{
				if (this.sculptures[i].DestroyedOrNull() || (!this.sculptures[i].SpawnedOrAnyParentSpawned && !this.sculptures[i].IsInCaravan()))
				{
					flag = true;
					this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DestroyedCubeSculpture, null, null);
					this.sculptures.RemoveAt(i);
					float num = Hediff_CubeInterest.CubeRageSeverityIncrease.Evaluate(this.Severity);
					if (num > 0f)
					{
						HealthUtility.AdjustSeverity(this.pawn, HediffDefOf.CubeRage, num);
					}
				}
			}
			if (flag)
			{
				Messages.Message("MessageGoldenCubeSculptureDestroyed".Translate(this.pawn.Named("PAWN")), this.pawn, MessageTypeDefOf.NeutralEvent, true);
			}
		}

		// Token: 0x06002D1C RID: 11548 RVA: 0x000E8ED4 File Offset: 0x000E70D4
		public override void PostRemoved()
		{
			base.PostRemoved();
			Hediff hediff;
			if (this.pawn.health.hediffSet.TryGetHediff(HediffDefOf.CubeRage, out hediff))
			{
				this.pawn.health.RemoveHediff(hediff);
			}
		}

		// Token: 0x06002D1D RID: 11549 RVA: 0x000E8F18 File Offset: 0x000E7118
		protected override void OnStageIndexChanged(int stageIndex)
		{
			if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				Messages.Message("MessageGoldenCubeSeverityIncreased".Translate(this.pawn.Named("PAWN")), this.pawn, MessageTypeDefOf.NeutralEvent, true);
			}
		}

		// Token: 0x06002D1E RID: 11550 RVA: 0x000E8F67 File Offset: 0x000E7167
		public void Notify_BuiltSculpture(Thing sculpture)
		{
			this.sculptures.Add(sculpture);
		}

		// Token: 0x06002D1F RID: 11551 RVA: 0x000E8F78 File Offset: 0x000E7178
		public void Notify_PlayedWith()
		{
			if (this.InWithdrawal)
			{
				this.pawn.health.RemoveHediff(this.WithdrawalHediff);
				this.cachedHediff = null;
			}
			this.Severity += Hediff_CubeInterest.SeverityPerPlay.RandomInRange;
			Pawn_NeedsTracker needs = this.pawn.needs;
			if (needs == null)
			{
				return;
			}
			Need_Mood mood = needs.mood;
			if (mood == null)
			{
				return;
			}
			ThoughtHandler thoughts = mood.thoughts;
			if (thoughts == null)
			{
				return;
			}
			MemoryThoughtHandler memories = thoughts.memories;
			if (memories == null)
			{
				return;
			}
			memories.TryGainMemory(ThoughtDefOf.PlayedWithCube, null, null);
		}

		// Token: 0x06002D20 RID: 11552 RVA: 0x000E8FFE File Offset: 0x000E71FE
		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (DebugSettings.ShowDevGizmos)
			{
				if (!this.InWithdrawal)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Cube withdrawal",
						action = new Action(this.StartWithdrawal)
					};
				}
				yield return new Command_Action
				{
					defaultLabel = "DEV: Cube obsession",
					action = new Action(this.DoMentalBreak)
				};
			}
			yield break;
		}

		// Token: 0x06002D21 RID: 11553 RVA: 0x000E9010 File Offset: 0x000E7210
		private void DoMentalBreak()
		{
			if (this.pawn.InMentalState)
			{
				return;
			}
			this.pawn.mindState.mentalBreaker.TryDoMentalBreak("MentalBreakReason_CubeSculpting".Translate(this.pawn.Named("PAWN")), MentalBreakDefOf.CubeSculpting);
		}

		// Token: 0x06002D22 RID: 11554 RVA: 0x000E9068 File Offset: 0x000E7268
		private void StartWithdrawal()
		{
			if (this.InWithdrawal)
			{
				return;
			}
			this.cachedHediff = (Hediff_CubeWithdrawal)this.pawn.health.AddHediff(HediffDefOf.CubeWithdrawal, null, null, null);
		}

		// Token: 0x06002D23 RID: 11555 RVA: 0x000E90A9 File Offset: 0x000E72A9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Thing>(ref this.sculptures, "sculptures", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x040023D6 RID: 9174
		private List<Thing> sculptures = new List<Thing>();

		// Token: 0x040023D7 RID: 9175
		private Hediff_CubeWithdrawal cachedHediff;

		// Token: 0x040023D8 RID: 9176
		private const float MTBPlayWantedDays = 1.5f;

		// Token: 0x040023D9 RID: 9177
		private const int InitialDelayTicks = 1000;

		// Token: 0x040023DA RID: 9178
		private const int SculptureUpdateRateTicks = 60;

		// Token: 0x040023DB RID: 9179
		private static readonly FloatRange SeverityPerPlay = new FloatRange(0.045f, 0.105f);

		// Token: 0x040023DC RID: 9180
		private static readonly SimpleCurve MentalBreakSeverityDaysMTB = new SimpleCurve
		{
			{
				new CurvePoint(0.01f, 5f),
				true
			},
			{
				new CurvePoint(0.5f, 3f),
				true
			},
			{
				new CurvePoint(0.8f, 2f),
				true
			}
		};

		// Token: 0x040023DD RID: 9181
		private static readonly SimpleCurve CubeRageSeverityIncrease = new SimpleCurve
		{
			{
				new CurvePoint(0.1f, 0.1f),
				true
			},
			{
				new CurvePoint(0.5f, 0.2f),
				true
			},
			{
				new CurvePoint(1f, 0.4f),
				true
			}
		};
	}
}
