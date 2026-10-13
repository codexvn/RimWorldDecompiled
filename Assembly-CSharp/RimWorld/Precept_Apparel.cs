using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002661 RID: 9825
	public class Precept_Apparel : Precept
	{
		// Token: 0x170020FC RID: 8444
		// (get) Token: 0x0600D775 RID: 55157 RVA: 0x00002501 File Offset: 0x00000701
		public override bool SortByImpact
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170020FD RID: 8445
		// (get) Token: 0x0600D776 RID: 55158 RVA: 0x003FE6C7 File Offset: 0x003FC8C7
		public override bool CanRegenerate
		{
			get
			{
				return this.def.Worker.ThingDefs.Count<PreceptThingChance>() > 1;
			}
		}

		// Token: 0x170020FE RID: 8446
		// (get) Token: 0x0600D777 RID: 55159 RVA: 0x003FE6E1 File Offset: 0x003FC8E1
		public override string TipLabel
		{
			get
			{
				return this.UIInfoFirstLine + ": " + this.def.LabelCap;
			}
		}

		// Token: 0x170020FF RID: 8447
		// (get) Token: 0x0600D778 RID: 55160 RVA: 0x003FE708 File Offset: 0x003FC908
		public override string UIInfoFirstLine
		{
			get
			{
				if (this.apparelDef != null)
				{
					return this.apparelDef.LabelCap;
				}
				return base.UIInfoFirstLine;
			}
		}

		// Token: 0x17002100 RID: 8448
		// (get) Token: 0x0600D779 RID: 55161 RVA: 0x003FE72C File Offset: 0x003FC92C
		public override string UIInfoSecondLine
		{
			get
			{
				if (this.TargetGender == Gender.None)
				{
					return "Everyone".Translate() + ": " + base.UIInfoSecondLine;
				}
				return this.TargetGender.GetLabel(false).CapitalizeFirst() + ": " + base.UIInfoSecondLine;
			}
		}

		// Token: 0x17002101 RID: 8449
		// (get) Token: 0x0600D77A RID: 55162 RVA: 0x003FE788 File Offset: 0x003FC988
		public Gender TargetGender
		{
			get
			{
				Gender? gender = this.overrideGender;
				if (gender == null)
				{
					return this.targetGender;
				}
				return gender.GetValueOrDefault();
			}
		}

		// Token: 0x0600D77B RID: 55163 RVA: 0x003FE7B4 File Offset: 0x003FC9B4
		public override void Init(Ideo ideo, FactionDef generatingFor = null)
		{
			base.Init(ideo, generatingFor);
			for (int i = 0; i < this.def.comps.Count; i++)
			{
				PreceptComp_Apparel preceptComp_Apparel = this.def.comps[i] as PreceptComp_Apparel;
				if (preceptComp_Apparel != null)
				{
					this.targetGender = preceptComp_Apparel.AffectedGender(ideo);
					break;
				}
			}
			this.InitDescription();
		}

		// Token: 0x0600D77C RID: 55164 RVA: 0x003FE814 File Offset: 0x003FCA14
		public override void Regenerate(Ideo ideo, FactionDef generatingFor = null)
		{
			IEnumerable<PreceptThingChance> source;
			if (!this.def.canUseAlreadyUsedThingDef)
			{
				Precept_Apparel.usedThingDefsTmp.Clear();
				foreach (Precept precept in ideo.PreceptsListForReading)
				{
					Precept_Apparel precept_Apparel = precept as Precept_Apparel;
					if (precept_Apparel != null)
					{
						Precept_Apparel.usedThingDefsTmp.Add(precept_Apparel.apparelDef);
					}
				}
				source = from bd in this.def.Worker.ThingDefsForIdeo(ideo, generatingFor)
				where !Precept_Apparel.usedThingDefsTmp.Contains(bd.def)
				select bd;
			}
			else
			{
				source = this.def.Worker.ThingDefsForIdeo(ideo, generatingFor);
			}
			if (source.Any<PreceptThingChance>())
			{
				PreceptThingChance preceptThingChance;
				if (source.TryRandomElementByWeight((PreceptThingChance d) => d.chance, out preceptThingChance))
				{
					this.apparelDef = preceptThingChance.def;
					goto IL_144;
				}
			}
			this.apparelDef = this.def.Worker.ThingDefsForIdeo(ideo, generatingFor).RandomElementByWeight((PreceptThingChance d) => d.chance).def;
			Log.Warning("Failed to generate a unique apparel for " + ideo.name);
			IL_144:
			if (ideo.SupremeGender == Gender.None && Rand.Value < 0.5f)
			{
				this.overrideGender = new Gender?((Rand.Value < 0.5f) ? Gender.Male : Gender.Female);
			}
			else
			{
				this.overrideGender = null;
			}
			base.Regenerate(ideo, null);
		}

		// Token: 0x0600D77D RID: 55165 RVA: 0x003FE9BC File Offset: 0x003FCBBC
		public override void DrawIcon(Rect rect)
		{
			Widgets.ThingIcon(rect, this.apparelDef, null, this.ideo.GetStyleFor(this.apparelDef), 1f, new Color?(this.ideo.ApparelColor), null, 1f);
		}

		// Token: 0x0600D77E RID: 55166 RVA: 0x003FEA0A File Offset: 0x003FCC0A
		public override IEnumerable<FloatMenuOption> EditFloatMenuOptions()
		{
			string text = "SetTargetGender".Translate() + ": ";
			FloatMenuOption floatMenuOption = new FloatMenuOption(text + Gender.Male.GetLabel(false).CapitalizeFirst(), delegate()
			{
				this.overrideGender = new Gender?(Gender.Male);
				this.InitDescription();
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			FloatMenuOption setFemale = new FloatMenuOption(text + Gender.Female.GetLabel(false).CapitalizeFirst(), delegate()
			{
				this.overrideGender = new Gender?(Gender.Female);
				this.InitDescription();
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			FloatMenuOption setNone = new FloatMenuOption(text + "Everyone".Translate().CapitalizeFirst(), delegate()
			{
				this.overrideGender = new Gender?(Gender.None);
				this.InitDescription();
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			switch (this.TargetGender)
			{
			case Gender.None:
				yield return floatMenuOption;
				yield return setFemale;
				break;
			case Gender.Male:
				yield return setFemale;
				yield return setNone;
				break;
			case Gender.Female:
				yield return floatMenuOption;
				yield return setNone;
				break;
			}
			if (this.def.apparelPreceptSwapDef != null)
			{
				yield return new FloatMenuOption("SetApparelType".Translate() + ": " + this.def.apparelPreceptSwapDef.LabelCap, delegate()
				{
					this.def = this.def.apparelPreceptSwapDef;
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			yield break;
		}

		// Token: 0x0600D77F RID: 55167 RVA: 0x003FEA1C File Offset: 0x003FCC1C
		private void InitDescription()
		{
			string arg = (this.TargetGender != Gender.None) ? this.TargetGender.GetLabel(false) : "All".Translate();
			this.descOverride = this.def.description.Formatted(arg.Named("GENDER"), this.apparelDef.Named("APPAREL")).CapitalizeFirst();
		}

		// Token: 0x0600D780 RID: 55168 RVA: 0x003FEA90 File Offset: 0x003FCC90
		public override bool CompatibleWith(Precept other)
		{
			if (other.def.prefersNudity && (this.TargetGender == Gender.None || this.TargetGender == other.def.genderPrefersNudity))
			{
				return false;
			}
			Precept_Apparel precept_Apparel = other as Precept_Apparel;
			return (precept_Apparel == null || this.def != other.def || (this.TargetGender != precept_Apparel.TargetGender && this.TargetGender != Gender.None && precept_Apparel.TargetGender != Gender.None) || ApparelUtility.CanWearTogether(this.apparelDef, precept_Apparel.apparelDef, BodyDefOf.Human)) && base.CompatibleWith(other);
		}

		// Token: 0x0600D781 RID: 55169 RVA: 0x003FEB20 File Offset: 0x003FCD20
		public override bool GetPlayerWarning(out string shortText, out string description)
		{
			foreach (Precept_Role precept_Role in this.ideo.RolesListForReading)
			{
				if ((this.TargetGender == Gender.None || !precept_Role.restrictToSupremeGender || this.ideo.SupremeGender == Gender.None || this.ideo.SupremeGender == this.TargetGender) && precept_Role.ApparelRequirements != null)
				{
					foreach (PreceptApparelRequirement preceptApparelRequirement in precept_Role.ApparelRequirements)
					{
						IEnumerable<ThingDef> source = preceptApparelRequirement.requirement.AllRequiredApparel(this.ideo.SupremeGender);
						if (source.Any((ThingDef ap) => ap != this.apparelDef && !ApparelUtility.CanWearTogether(this.apparelDef, ap, BodyDefOf.Human)))
						{
							shortText = "MessageIdeoWarnRoleApparelOverlapsDesiredApparel".Translate(this.apparelDef.label.Named("DESIREDAPPAREL"), precept_Role.def.label.Named("ROLE"));
							description = "DescriptionIdeoWarnRoleApparelOverlapsDesiredApparel".Translate(this.apparelDef.label.Named("DESIREDAPPAREL"), precept_Role.def.label.Named("ROLE"), string.Join(", ", from ap in source
							select ap.label).CapitalizeFirst().Named("ROLEAPPAREL")).CapitalizeFirst();
							return true;
						}
					}
				}
			}
			shortText = null;
			description = null;
			return false;
		}

		// Token: 0x0600D782 RID: 55170 RVA: 0x003FED08 File Offset: 0x003FCF08
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.apparelDef, "apparelDef");
			Scribe_Values.Look<Gender>(ref this.targetGender, "targetGender", Gender.None, false);
			Scribe_Values.Look<Gender?>(ref this.overrideGender, "overrideGender", null, false);
		}

		// Token: 0x0600D783 RID: 55171 RVA: 0x003FED57 File Offset: 0x003FCF57
		public override void CopyTo(Precept other)
		{
			base.CopyTo(other);
			Precept_Apparel precept_Apparel = (Precept_Apparel)other;
			precept_Apparel.apparelDef = this.apparelDef;
			precept_Apparel.targetGender = this.targetGender;
			precept_Apparel.overrideGender = this.overrideGender;
		}

		// Token: 0x0400923B RID: 37435
		public ThingDef apparelDef;

		// Token: 0x0400923C RID: 37436
		private Gender targetGender;

		// Token: 0x0400923D RID: 37437
		private Gender? overrideGender;

		// Token: 0x0400923E RID: 37438
		private static List<ThingDef> usedThingDefsTmp = new List<ThingDef>();
	}
}
