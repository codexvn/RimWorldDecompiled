using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020006D3 RID: 1747
	public class HediffComp_Infecter : HediffComp
	{
		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x06002B52 RID: 11090 RVA: 0x000E2802 File Offset: 0x000E0A02
		public HediffCompProperties_Infecter Props
		{
			get
			{
				return (HediffCompProperties_Infecter)this.props;
			}
		}

		// Token: 0x06002B53 RID: 11091 RVA: 0x000E2810 File Offset: 0x000E0A10
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			if (this.parent.IsPermanent())
			{
				this.ticksUntilInfect = -2;
				return;
			}
			if (this.parent.Part != null)
			{
				if (this.parent.Part.def.IsSolid(this.parent.Part, base.Pawn.health.hediffSet.hediffs))
				{
					this.ticksUntilInfect = -2;
					return;
				}
				if (base.Pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(this.parent.Part))
				{
					this.ticksUntilInfect = -2;
					return;
				}
			}
			float num = this.Props.infectionChance;
			if (base.Pawn.RaceProps.Animal)
			{
				num *= 0.1f;
			}
			if (Rand.Value <= num)
			{
				this.ticksUntilInfect = HealthTuning.InfectionDelayRange.RandomInRange;
				return;
			}
			this.ticksUntilInfect = -2;
		}

		// Token: 0x06002B54 RID: 11092 RVA: 0x000E28F5 File Offset: 0x000E0AF5
		public override void CompExposeData()
		{
			Scribe_Values.Look<float>(ref this.infectionChanceFactorFromTendRoom, "infectionChanceFactor", 0f, false);
			Scribe_Values.Look<int>(ref this.ticksUntilInfect, "ticksUntilInfect", -2, false);
			Scribe_Values.Look<bool>(ref this.fromScaria, "fromScaria", false, false);
		}

		// Token: 0x06002B55 RID: 11093 RVA: 0x000E2932 File Offset: 0x000E0B32
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			if (this.ticksUntilInfect > 0)
			{
				this.ticksUntilInfect -= delta;
				if (this.ticksUntilInfect <= 0)
				{
					this.CheckMakeInfection();
				}
			}
		}

		// Token: 0x06002B56 RID: 11094 RVA: 0x000E295C File Offset: 0x000E0B5C
		public override void CompTended(float quality, float maxQuality, int batchPosition = 0)
		{
			base.CompTended(quality, maxQuality, batchPosition);
			if (base.Pawn.Spawned)
			{
				Room room = base.Pawn.GetRoom(RegionType.Set_All);
				if (room != null)
				{
					this.infectionChanceFactorFromTendRoom = room.GetStat(RoomStatDefOf.InfectionChanceFactor);
				}
			}
		}

		// Token: 0x06002B57 RID: 11095 RVA: 0x000E29A4 File Offset: 0x000E0BA4
		private void CheckMakeInfection()
		{
			if (base.Pawn.health.immunity.DiseaseContractChanceFactor(HediffDefOf.WoundInfection, this.parent.Part) <= 0.001f)
			{
				this.ticksUntilInfect = -3;
				return;
			}
			if (base.Pawn.health.hediffSet.HasRegeneration)
			{
				this.ticksUntilInfect = -3;
				return;
			}
			float num = 1f;
			HediffComp_TendDuration hediffComp_TendDuration = this.parent.TryGetComp<HediffComp_TendDuration>();
			if (hediffComp_TendDuration != null && hediffComp_TendDuration.IsTended)
			{
				num *= this.infectionChanceFactorFromTendRoom;
				num *= HediffComp_Infecter.InfectionChanceFactorFromTendQualityCurve.Evaluate(hediffComp_TendDuration.tendQuality);
			}
			num *= HediffComp_Infecter.InfectionChanceFactorFromSeverityCurve.Evaluate(this.parent.Severity);
			if (base.Pawn.Faction == Faction.OfPlayer)
			{
				num *= Find.Storyteller.difficulty.playerPawnInfectionChanceFactor;
			}
			if (Rand.Value < num)
			{
				this.ticksUntilInfect = -4;
				base.Pawn.health.AddHediff(this.fromScaria ? HediffDefOf.ScariaInfection : HediffDefOf.WoundInfection, this.parent.Part, null, null);
				return;
			}
			this.ticksUntilInfect = -3;
		}

		// Token: 0x06002B58 RID: 11096 RVA: 0x000E2AD0 File Offset: 0x000E0CD0
		public override string CompDebugString()
		{
			if (this.ticksUntilInfect > 0)
			{
				return "infection may appear in: " + this.ticksUntilInfect.ToString() + " ticks\ninfectChnceFactorFromTendRoom: " + this.infectionChanceFactorFromTendRoom.ToStringPercent();
			}
			if (this.ticksUntilInfect == -4)
			{
				return "already created infection";
			}
			if (this.ticksUntilInfect == -3)
			{
				return "failed to make infection";
			}
			if (this.ticksUntilInfect == -2)
			{
				return "will not make infection";
			}
			if (this.ticksUntilInfect == -1)
			{
				return "uninitialized data!";
			}
			return "unexpected ticksUntilInfect = " + this.ticksUntilInfect.ToString();
		}

		// Token: 0x040022EC RID: 8940
		private int ticksUntilInfect = -1;

		// Token: 0x040022ED RID: 8941
		private float infectionChanceFactorFromTendRoom = 1f;

		// Token: 0x040022EE RID: 8942
		public bool fromScaria;

		// Token: 0x040022EF RID: 8943
		private const int UninitializedValue = -1;

		// Token: 0x040022F0 RID: 8944
		private const int WillNotInfectValue = -2;

		// Token: 0x040022F1 RID: 8945
		private const int FailedToMakeInfectionValue = -3;

		// Token: 0x040022F2 RID: 8946
		private const int AlreadyMadeInfectionValue = -4;

		// Token: 0x040022F3 RID: 8947
		private static readonly SimpleCurve InfectionChanceFactorFromTendQualityCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.7f),
				true
			},
			{
				new CurvePoint(1f, 0.4f),
				true
			}
		};

		// Token: 0x040022F4 RID: 8948
		private static readonly SimpleCurve InfectionChanceFactorFromSeverityCurve = new SimpleCurve
		{
			{
				new CurvePoint(1f, 0.1f),
				true
			},
			{
				new CurvePoint(12f, 1f),
				true
			}
		};
	}
}
