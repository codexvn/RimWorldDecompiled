using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020021ED RID: 8685
	public class Command_Psycast : Command_Ability
	{
		// Token: 0x0600BE42 RID: 48706 RVA: 0x00372F0F File Offset: 0x0037110F
		public Command_Psycast(Psycast ability, Pawn pawn) : base(ability, pawn)
		{
		}

		// Token: 0x0600BE43 RID: 48707 RVA: 0x00372F1C File Offset: 0x0037111C
		protected override void DisabledCheck()
		{
			AbilityDef def = this.ability.def;
			Pawn pawn = this.ability.pawn;
			this.disabled = false;
			if (def.EntropyGain > 1E-45f)
			{
				if (pawn.GetPsylinkLevel() < def.level)
				{
					base.DisableWithReason("CommandPsycastHigherLevelPsylinkRequired".Translate(def.level));
				}
				else if (pawn.psychicEntropy.WouldOverflowEntropy(def.EntropyGain + PsycastUtility.TotalEntropyFromQueuedPsycasts(pawn)))
				{
					base.DisableWithReason("CommandPsycastWouldExceedEntropy".Translate(def.label));
				}
			}
			base.DisabledCheck();
		}

		// Token: 0x17001D05 RID: 7429
		// (get) Token: 0x0600BE44 RID: 48708 RVA: 0x00372FC8 File Offset: 0x003711C8
		public override string TopRightLabel
		{
			get
			{
				AbilityDef def = this.ability.def;
				string text = "";
				if (def.EntropyGain > 1E-45f)
				{
					text += "NeuralHeatLetter".Translate() + ": " + def.EntropyGain.ToString() + "\n";
				}
				if (def.PsyfocusCost > 1E-45f)
				{
					string t;
					if (def.AnyCompOverridesPsyfocusCost)
					{
						if (def.PsyfocusCostRange.Span > 1E-45f)
						{
							t = (def.PsyfocusCostRange.min * 100f).ToString() + "-" + def.PsyfocusCostPercentMax;
						}
						else
						{
							t = def.PsyfocusCostPercentMax;
						}
					}
					else
					{
						t = def.PsyfocusCostPercent;
					}
					text += "PsyfocusLetter".Translate() + ": " + t;
				}
				return text.TrimEndNewlines();
			}
		}
	}
}
