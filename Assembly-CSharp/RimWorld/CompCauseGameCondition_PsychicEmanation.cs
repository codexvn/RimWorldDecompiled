using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002BBD RID: 11197
	public class CompCauseGameCondition_PsychicEmanation : CompCauseGameCondition
	{
		// Token: 0x1700273E RID: 10046
		// (get) Token: 0x0600FA57 RID: 64087 RVA: 0x0048C275 File Offset: 0x0048A475
		public new CompProperties_CausesGameCondition_PsychicEmanation Props
		{
			get
			{
				return (CompProperties_CausesGameCondition_PsychicEmanation)this.props;
			}
		}

		// Token: 0x1700273F RID: 10047
		// (get) Token: 0x0600FA58 RID: 64088 RVA: 0x0048C282 File Offset: 0x0048A482
		public PsychicDroneLevel Level
		{
			get
			{
				return this.droneLevel;
			}
		}

		// Token: 0x17002740 RID: 10048
		// (get) Token: 0x0600FA59 RID: 64089 RVA: 0x0048C28A File Offset: 0x0048A48A
		private bool DroneLevelIncreases
		{
			get
			{
				return this.Props.droneLevelIncreaseInterval != int.MinValue;
			}
		}

		// Token: 0x0600FA5A RID: 64090 RVA: 0x0048C2A1 File Offset: 0x0048A4A1
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.gender = (Rand.Bool ? Gender.Male : Gender.Female);
			this.droneLevel = this.Props.droneLevel;
		}

		// Token: 0x0600FA5B RID: 64091 RVA: 0x0048C2CC File Offset: 0x0048A4CC
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad && this.DroneLevelIncreases && !this.parent.BeingTransportedOnGravship)
			{
				this.ticksToIncreaseDroneLevel = this.Props.droneLevelIncreaseInterval;
				SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(this.parent.Map);
			}
		}

		// Token: 0x0600FA5C RID: 64092 RVA: 0x0048C320 File Offset: 0x0048A520
		public override void CompTick()
		{
			base.CompTick();
			if (!this.parent.Spawned || !this.DroneLevelIncreases || !this.Active)
			{
				return;
			}
			this.ticksToIncreaseDroneLevel--;
			if (this.ticksToIncreaseDroneLevel <= 0)
			{
				this.IncreaseDroneLevel();
				this.ticksToIncreaseDroneLevel = this.Props.droneLevelIncreaseInterval;
			}
		}

		// Token: 0x0600FA5D RID: 64093 RVA: 0x0048C380 File Offset: 0x0048A580
		private void IncreaseDroneLevel()
		{
			if (this.droneLevel == PsychicDroneLevel.BadExtreme)
			{
				return;
			}
			this.droneLevel += 1;
			TaggedString text = "LetterPsychicDroneLevelIncreased".Translate(this.gender.GetLabel(false));
			Find.LetterStack.ReceiveLetter("LetterLabelPsychicDroneLevelIncreased".Translate(), text, LetterDefOf.NegativeEvent, null, 0, true);
			SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(this.parent.Map);
			base.ReSetupAllConditions();
		}

		// Token: 0x0600FA5E RID: 64094 RVA: 0x0048C3FA File Offset: 0x0048A5FA
		protected override void SetupCondition(GameCondition condition, Map map)
		{
			base.SetupCondition(condition, map);
			GameCondition_PsychicEmanation gameCondition_PsychicEmanation = (GameCondition_PsychicEmanation)condition;
			gameCondition_PsychicEmanation.gender = this.gender;
			gameCondition_PsychicEmanation.level = this.Level;
		}

		// Token: 0x0600FA5F RID: 64095 RVA: 0x0048C421 File Offset: 0x0048A621
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
			Scribe_Values.Look<int>(ref this.ticksToIncreaseDroneLevel, "ticksToIncreaseDroneLevel", 0, false);
			Scribe_Values.Look<PsychicDroneLevel>(ref this.droneLevel, "droneLevel", PsychicDroneLevel.None, false);
		}

		// Token: 0x0600FA60 RID: 64096 RVA: 0x0048C45F File Offset: 0x0048A65F
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!Prefs.DevMode || !DebugSettings.godMode)
			{
				yield break;
			}
			yield return new Command_Action
			{
				defaultLabel = this.gender.GetLabel(false),
				action = delegate()
				{
					if (this.gender == Gender.Female)
					{
						this.gender = Gender.Male;
					}
					else
					{
						this.gender = Gender.Female;
					}
					base.ReSetupAllConditions();
				},
				hotKey = KeyBindingDefOf.Misc1
			};
			yield return new Command_Action
			{
				defaultLabel = this.droneLevel.GetLabel(),
				action = delegate()
				{
					this.IncreaseDroneLevel();
					base.ReSetupAllConditions();
				},
				hotKey = KeyBindingDefOf.Misc2
			};
			yield break;
		}

		// Token: 0x0600FA61 RID: 64097 RVA: 0x0048C470 File Offset: 0x0048A670
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + ("AffectedGender".Translate() + ": " + this.gender.GetLabel(false).CapitalizeFirst() + "\n" + "PsychicDroneLevel".Translate(this.droneLevel.GetLabelCap()));
		}

		// Token: 0x0600FA62 RID: 64098 RVA: 0x0048C4F8 File Offset: 0x0048A6F8
		public override void RandomizeSettings(Site site)
		{
			this.gender = (Rand.Bool ? Gender.Male : Gender.Female);
			if (site.ActualThreatPoints < 800f)
			{
				this.droneLevel = PsychicDroneLevel.BadLow;
				return;
			}
			if (site.ActualThreatPoints < 2000f)
			{
				this.droneLevel = PsychicDroneLevel.BadMedium;
				return;
			}
			this.droneLevel = PsychicDroneLevel.BadHigh;
		}

		// Token: 0x0400A483 RID: 42115
		public Gender gender;

		// Token: 0x0400A484 RID: 42116
		private int ticksToIncreaseDroneLevel;

		// Token: 0x0400A485 RID: 42117
		private PsychicDroneLevel droneLevel = PsychicDroneLevel.BadHigh;
	}
}
