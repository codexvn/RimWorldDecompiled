using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Grammar;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002809 RID: 10249
	public abstract class Precept_Role : Precept
	{
		// Token: 0x17002234 RID: 8756
		// (get) Token: 0x0600E014 RID: 57364 RVA: 0x0041CA50 File Offset: 0x0041AC50
		public bool Active
		{
			get
			{
				return this.active || DebugSettings.activateAllIdeoRoles;
			}
		}

		// Token: 0x17002235 RID: 8757
		// (get) Token: 0x0600E015 RID: 57365 RVA: 0x003FC6B6 File Offset: 0x003FA8B6
		public override string UIInfoFirstLine
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x17002236 RID: 8758
		// (get) Token: 0x0600E016 RID: 57366 RVA: 0x004008D2 File Offset: 0x003FEAD2
		public override string UIInfoSecondLine
		{
			get
			{
				return base.LabelCap;
			}
		}

		// Token: 0x17002237 RID: 8759
		// (get) Token: 0x0600E017 RID: 57367 RVA: 0x0041CA61 File Offset: 0x0041AC61
		public override string TipLabel
		{
			get
			{
				return this.def.issue.LabelCap + ": " + base.LabelCap;
			}
		}

		// Token: 0x17002238 RID: 8760
		// (get) Token: 0x0600E018 RID: 57368 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool UsesGeneratedName
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002239 RID: 8761
		// (get) Token: 0x0600E019 RID: 57369 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool CanRegenerate
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700223A RID: 8762
		// (get) Token: 0x0600E01A RID: 57370 RVA: 0x00002501 File Offset: 0x00000701
		public override bool SortByImpact
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700223B RID: 8763
		// (get) Token: 0x0600E01B RID: 57371 RVA: 0x0041CA8D File Offset: 0x0041AC8D
		// (set) Token: 0x0600E01C RID: 57372 RVA: 0x0041CA95 File Offset: 0x0041AC95
		public override List<PreceptApparelRequirement> ApparelRequirements
		{
			get
			{
				return this.apparelRequirements;
			}
			set
			{
				this.tipCached = null;
				this.allApparelRequirementLabelsCached.Clear();
				this.apparelRequirements = value;
			}
		}

		// Token: 0x0600E01D RID: 57373 RVA: 0x0041CAB0 File Offset: 0x0041ACB0
		public List<string> AllApparelRequirementLabels(Gender gender, Pawn forPawn = null)
		{
			List<string> list;
			if (!this.allApparelRequirementLabelsCached.TryGetValue(gender, out list))
			{
				list = new List<string>();
				if (this.apparelRequirements != null)
				{
					for (int i = 0; i < this.apparelRequirements.Count; i++)
					{
						ApparelRequirement requirement = this.apparelRequirements[i].requirement;
						list.Add((!requirement.groupLabel.NullOrEmpty()) ? requirement.groupLabel : requirement.AllRequiredApparel(Gender.Male).First<ThingDef>().LabelCap.Resolve());
					}
				}
				this.allApparelRequirementLabelsCached[gender] = list;
			}
			if (this.apparelRequirements != null)
			{
				bool flag = false;
				for (int j = 0; j < this.apparelRequirements.Count; j++)
				{
					ApparelRequirement requirement2 = this.apparelRequirements[j].requirement;
					string t;
					if (forPawn != null && !ApparelUtility.IsRequirementActive(requirement2, ApparelRequirementSource.Role, forPawn, out t))
					{
						if (!flag)
						{
							list = new List<string>(list);
							flag = true;
						}
						string text = list[j];
						text += " [" + "ApparelRequirementDisabledLabel".Translate() + ": " + t + "]";
						list[j] = text;
					}
				}
			}
			return list;
		}

		// Token: 0x1700223C RID: 8764
		// (get) Token: 0x0600E01E RID: 57374 RVA: 0x0041CBF6 File Offset: 0x0041ADF6
		public IEnumerable<WorkTypeDef> DisabledWorkTypes
		{
			get
			{
				List<WorkTypeDef> list = DefDatabase<WorkTypeDef>.AllDefsListForReading;
				int num;
				for (int i = 0; i < list.Count; i = num + 1)
				{
					if ((this.def.roleDisabledWorkTags & list[i].workTags) != WorkTags.None)
					{
						yield return list[i];
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x0600E01F RID: 57375
		public abstract IEnumerable<Pawn> ChosenPawns();

		// Token: 0x0600E020 RID: 57376
		public abstract Pawn ChosenPawnSingle();

		// Token: 0x0600E021 RID: 57377
		public abstract bool IsAssigned(Pawn p);

		// Token: 0x0600E022 RID: 57378
		public abstract void Unassign(Pawn p, bool generateThoughts);

		// Token: 0x0600E023 RID: 57379 RVA: 0x0041CC08 File Offset: 0x0041AE08
		public string LabelForPawn(Pawn p)
		{
			if (this.def.leaderRole)
			{
				return ((p.gender == Gender.Female && !this.ideo.leaderTitleFemale.NullOrEmpty()) ? this.ideo.leaderTitleFemale : this.ideo.leaderTitleMale).CapitalizeFirst();
			}
			return this.name;
		}

		// Token: 0x0600E024 RID: 57380 RVA: 0x0041CC64 File Offset: 0x0041AE64
		public static List<PreceptApparelRequirement> AllPossibleRequirements(Ideo ideo, PreceptDef def, bool desperate = false)
		{
			Precept_Role.<>c__DisplayClass30_0 CS$<>8__locals1;
			CS$<>8__locals1.apparelRequirementPool = new List<PreceptApparelRequirement>();
			foreach (MemeDef memeDef in ideo.memes)
			{
				if (!memeDef.apparelRequirements.NullOrEmpty<PreceptApparelRequirement>())
				{
					Precept_Role.<AllPossibleRequirements>g__AddDistinct|30_0(memeDef.apparelRequirements, ref CS$<>8__locals1);
				}
			}
			Precept_Role.<AllPossibleRequirements>g__AddDistinct|30_0(from pa in ideo.PreceptsListForReading.OfType<Precept_Apparel>()
			where pa.TargetGender == Gender.None
			select pa into apparelPrecept
			select new PreceptApparelRequirement
			{
				requirement = new ApparelRequirement
				{
					bodyPartGroupsMatchAny = apparelPrecept.apparelDef.apparel.bodyPartGroups,
					requiredDefs = new List<ThingDef>
					{
						apparelPrecept.apparelDef
					}
				}
			}, ref CS$<>8__locals1);
			if (def.roleApparelRequirements != null)
			{
				Precept_Role.<AllPossibleRequirements>g__AddDistinct|30_0(def.roleApparelRequirements, ref CS$<>8__locals1);
			}
			if (!desperate)
			{
				for (int i = CS$<>8__locals1.apparelRequirementPool.Count - 1; i >= 0; i--)
				{
					foreach (Precept precept in ideo.PreceptsListForReading)
					{
						Precept_Apparel precept_Apparel = precept as Precept_Apparel;
						if (precept_Apparel != null && !CS$<>8__locals1.apparelRequirementPool[i].CanWearTogetherWith(precept_Apparel.apparelDef))
						{
							CS$<>8__locals1.apparelRequirementPool.RemoveAt(i);
							break;
						}
					}
				}
			}
			return CS$<>8__locals1.apparelRequirementPool;
		}

		// Token: 0x0600E025 RID: 57381 RVA: 0x0041CDD4 File Offset: 0x0041AFD4
		public override List<PreceptApparelRequirement> GenerateNewApparelRequirements(FactionDef generatingFor = null)
		{
			Precept_Role.<>c__DisplayClass31_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.generatingFor = generatingFor;
			CS$<>8__locals1.apparelRequirementCount = ((this.def.roleApparelRequirementCountCurve != null) ? Mathf.CeilToInt(this.def.roleApparelRequirementCountCurve.Evaluate(Rand.Value)) : 0);
			if (CS$<>8__locals1.apparelRequirementCount == 0)
			{
				return null;
			}
			CS$<>8__locals1.apparelRequirements = new List<PreceptApparelRequirement>();
			using (List<MemeDef>.Enumerator enumerator = this.ideo.memes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.preventApparelRequirements)
					{
						return CS$<>8__locals1.apparelRequirements;
					}
				}
			}
			this.<GenerateNewApparelRequirements>g__Choose|31_1(false, ref CS$<>8__locals1);
			if (CS$<>8__locals1.apparelRequirements.Count < CS$<>8__locals1.apparelRequirementCount)
			{
				this.<GenerateNewApparelRequirements>g__Choose|31_1(true, ref CS$<>8__locals1);
			}
			return CS$<>8__locals1.apparelRequirements;
		}

		// Token: 0x0600E026 RID: 57382 RVA: 0x0041CEB8 File Offset: 0x0041B0B8
		public override void Init(Ideo ideo, FactionDef generatingFor = null)
		{
			base.Init(ideo, null);
			if (!ModLister.CheckIdeology("Ideology role"))
			{
				return;
			}
			base.RegenerateName();
			if (!ideo.classicExtraMode && !ideo.classicMode)
			{
				this.apparelRequirements = this.GenerateNewApparelRequirements(generatingFor);
			}
			this.allApparelRequirementLabelsCached.Clear();
			this.restrictToSupremeGender = (Rand.Value < this.def.restrictToSupremeGenderChance);
			this.FillOrUpdateAbilities();
		}

		// Token: 0x0600E027 RID: 57383 RVA: 0x0041CF28 File Offset: 0x0041B128
		public override string GenerateNameRaw()
		{
			if (!this.def.leaderRole)
			{
				GrammarRequest request = default(GrammarRequest);
				request.Includes.Add(this.def.nameMaker);
				base.AddIdeoRulesTo(ref request);
				return GenText.CapitalizeAsTitle(GrammarResolver.Resolve("r_roleName", request, null, false, null, null, null, false));
			}
			this.ideo.foundation.GenerateLeaderTitle();
			foreach (Precept precept in this.ideo.PreceptsListForReading)
			{
				precept.ClearTipCache();
			}
			return this.ideo.leaderTitleMale;
		}

		// Token: 0x0600E028 RID: 57384 RVA: 0x0041CFE4 File Offset: 0x0041B1E4
		public override string GetTip()
		{
			if (this.tipCached.NullOrEmpty())
			{
				Precept.tmpCompsDesc.Clear();
				if (!this.def.description.NullOrEmpty())
				{
					Precept.tmpCompsDesc.Append(this.def.description);
				}
				if (!this.def.leaderRole && this.def.activationBelieverCount != -1)
				{
					Precept.tmpCompsDesc.AppendLine();
					string value = (this.def.activationBelieverCount > 1) ? Find.ActiveLanguageWorker.Pluralize(this.ideo.memberName, -1) : this.ideo.memberName;
					string value2 = (this.def.deactivationBelieverCount > 1) ? Find.ActiveLanguageWorker.Pluralize(this.ideo.memberName, -1) : this.ideo.memberName;
					Precept.tmpCompsDesc.AppendInNewLine("RoleBelieverCountDesc".Translate(value, this.def.activationBelieverCount, value2, this.def.deactivationBelieverCount).Resolve());
				}
				if (!this.def.grantedAbilities.NullOrEmpty<AbilityDef>())
				{
					List<string> list = new List<string>();
					int i = 0;
					while (i < this.def.grantedAbilities.Count)
					{
						CompProperties_AbilityStartRitual compProperties_AbilityStartRitual = this.def.grantedAbilities[i].comps.FirstOrDefault((AbilityCompProperties comp) => comp is CompProperties_AbilityStartRitual) as CompProperties_AbilityStartRitual;
						if (compProperties_AbilityStartRitual != null)
						{
							using (List<Precept>.Enumerator enumerator = this.ideo.PreceptsListForReading.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									Precept precept = enumerator.Current;
									Precept_Ritual precept_Ritual = precept as Precept_Ritual;
									if (precept_Ritual != null && precept_Ritual.def == compProperties_AbilityStartRitual.ritualDef)
									{
										list.Add(precept_Ritual.LabelCap);
										break;
									}
								}
								goto IL_20D;
							}
							goto IL_1E6;
						}
						goto IL_1E6;
						IL_20D:
						i++;
						continue;
						IL_1E6:
						list.Add(this.def.grantedAbilities[i].LabelCap.Resolve());
						goto IL_20D;
					}
					Precept.tmpCompsDesc.AppendLine();
					Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("RoleGrantedAbilitiesLabel".Translate().Resolve() + ":"));
					Precept.tmpCompsDesc.AppendInNewLine(list.ToLineList("  - "));
				}
				List<string> list2 = new List<string>();
				foreach (Precept precept2 in this.ideo.PreceptsListForReading)
				{
					Precept_Ritual precept_Ritual2 = precept2 as Precept_Ritual;
					if (precept_Ritual2 != null && precept2.def.listedForRoles)
					{
						bool flag;
						if (precept_Ritual2 == null)
						{
							flag = (null != null);
						}
						else
						{
							RitualBehaviorWorker behavior = precept_Ritual2.behavior;
							flag = (((behavior != null) ? behavior.def.stages : null) != null);
						}
						if (flag && precept_Ritual2.behavior.def.roles != null && !list2.Contains(precept_Ritual2.LabelCap) && precept_Ritual2.behavior.def.roles.Any((RitualRole r) => r.precept == this.def))
						{
							list2.Add(precept_Ritual2.LabelCap);
						}
					}
				}
				if (list2.Count > 0)
				{
					Precept.tmpCompsDesc.AppendLine();
					Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("RoleRitualsLabel".Translate().Resolve() + ":"));
					Precept.tmpCompsDesc.AppendInNewLine(list2.ToLineList("  - "));
				}
				if (!this.def.roleRequirements.NullOrEmpty<RoleRequirement>())
				{
					List<string> list3 = new List<string>();
					for (int j = 0; j < this.def.roleRequirements.Count; j++)
					{
						string text = this.def.roleRequirements[j].GetLabelCap(this).ResolveTags();
						if (!text.NullOrEmpty())
						{
							list3.Add(text);
						}
					}
					Precept.tmpCompsDesc.AppendLine();
					Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("RoleRequirementsLabel".Translate().Resolve() + ":"));
					Precept.tmpCompsDesc.AppendInNewLine(list3.ToLineList("  - "));
				}
				if (!this.def.roleEffects.NullOrEmpty<RoleEffect>())
				{
					List<string> list4 = new List<string>();
					foreach (RoleEffect roleEffect in this.def.roleEffects.OrderBy(delegate(RoleEffect r)
					{
						if (!r.IsBad)
						{
							return 0;
						}
						return 1;
					}))
					{
						list4.Add(roleEffect.Label(null, this));
					}
					if (list4.Count > 0)
					{
						Precept.tmpCompsDesc.AppendLine();
						Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("RoleEffectsLabel".Translate().Resolve() + ":"));
						Precept.tmpCompsDesc.AppendInNewLine(list4.ToLineList("  - "));
					}
				}
				List<string> list5 = this.AllApparelRequirementLabels((this.ChosenPawnSingle() != null) ? this.ChosenPawnSingle().gender : Gender.Male, this.ChosenPawnSingle());
				if (list5.Count > 0)
				{
					Precept.tmpCompsDesc.AppendLine();
					Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("RoleRequiredApparelLabel".Translate().Resolve() + ":"));
					Precept.tmpCompsDesc.AppendInNewLine(list5.ToLineList("  - "));
				}
				if (this.def.expectationsOffset != 0)
				{
					Precept.tmpCompsDesc.AppendLine();
					Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("Expectations".Translate().CapitalizeFirst().Resolve() + ":"));
					Precept.tmpCompsDesc.AppendInNewLine("  - " + "RoleExpectationOffset".Translate(this.def.expectationsOffset).Resolve());
				}
				if (this.def.roleDisabledWorkTags != WorkTags.None)
				{
					Precept.tmpCompsDesc.AppendLine();
					Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("DisabledWorkLabel".Translate().Resolve() + ":"));
					Precept.tmpCompsDesc.AppendInNewLine("  - " + (from w in this.DisabledWorkTypes
					select w.labelShort).ToCommaList(false, false).CapitalizeFirst());
				}
				if (!this.def.requiredMemes.NullOrEmpty<MemeDef>())
				{
					List<MemeDef> requiredMemes = this.def.requiredMemes;
					IEnumerable<MemeDef> source = (requiredMemes != null) ? (from m in requiredMemes
					where this.ideo.memes.Contains(m)
					select m) : null;
					if (source.Any<MemeDef>())
					{
						Precept.tmpCompsDesc.AppendLine();
						Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("UnlockedByMeme".Translate().Resolve() + ":"));
						Precept.tmpCompsDesc.AppendInNewLine((from m in source
						select m.LabelCap.Resolve()).ToLineList("  - ", false));
					}
				}
				Precept.tmpCompsDesc.AppendLine();
				Precept.tmpCompsDesc.AppendInNewLine("RoleAssignmentTip".Translate().Colorize(Color.gray).ResolveTags());
				this.tipCached = Precept.tmpCompsDesc.ToString();
			}
			return this.tipCached;
		}

		// Token: 0x0600E029 RID: 57385 RVA: 0x0041D814 File Offset: 0x0041BA14
		public override void Tick()
		{
			base.Tick();
			this.RecacheActivity();
		}

		// Token: 0x0600E02A RID: 57386
		public abstract void RecacheActivity();

		// Token: 0x0600E02B RID: 57387
		public abstract void Assign(Pawn p, bool addThoughts);

		// Token: 0x0600E02C RID: 57388 RVA: 0x0041D822 File Offset: 0x0041BA22
		public override IEnumerable<FloatMenuOption> EditFloatMenuOptions()
		{
			yield return base.EditFloatMenuOption();
			yield break;
		}

		// Token: 0x0600E02D RID: 57389
		public abstract void FillOrUpdateAbilities();

		// Token: 0x0600E02E RID: 57390 RVA: 0x0041D834 File Offset: 0x0041BA34
		protected List<Ability> FillOrUpdateAbilityList(Pawn forPawn, List<Ability> abilities)
		{
			if (this.def.grantedAbilities == null)
			{
				return null;
			}
			if (abilities != null)
			{
				abilities.RemoveAll((Ability a) => a == null || a.def == null);
				int num = 0;
				for (int i = 0; i < abilities.Count; i++)
				{
					if (this.def.grantedAbilities.Contains(abilities[i].def))
					{
						num++;
					}
				}
				if (num != this.def.grantedAbilities.Count)
				{
					abilities = null;
				}
			}
			if (abilities == null)
			{
				abilities = new List<Ability>();
				using (List<AbilityDef>.Enumerator enumerator = this.def.grantedAbilities.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						AbilityDef def = enumerator.Current;
						abilities.Add(AbilityUtility.MakeAbility(def, forPawn, this));
					}
					return abilities;
				}
			}
			foreach (Ability ability in abilities)
			{
				ability.pawn = forPawn;
				ability.verb.caster = forPawn;
				ability.sourcePrecept = this;
			}
			return abilities;
		}

		// Token: 0x0600E02F RID: 57391
		public abstract List<Ability> AbilitiesFor(Pawn p);

		// Token: 0x0600E030 RID: 57392 RVA: 0x0041D970 File Offset: 0x0041BB70
		public void Notify_PawnUnassigned(Pawn oldPawn)
		{
			if (oldPawn != null)
			{
				Pawn_AbilityTracker abilities = oldPawn.abilities;
				if (abilities != null)
				{
					abilities.Notify_TemporaryAbilitiesChanged();
				}
			}
			if (oldPawn != null)
			{
				oldPawn.Notify_DisabledWorkTypesChanged();
			}
			if (oldPawn != null)
			{
				Pawn_ApparelTracker apparel = oldPawn.apparel;
				if (apparel != null)
				{
					apparel.Notify_RoleChanged();
				}
			}
			if (oldPawn != null && oldPawn.IsPrisoner)
			{
				Messages.Message("MessageRoleUnassignedPrisoner".Translate(oldPawn, this.LabelForPawn(oldPawn)), oldPawn, MessageTypeDefOf.SilentInput, false);
				SoundDefOf.Quest_Succeded.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x0600E031 RID: 57393 RVA: 0x0041D9F8 File Offset: 0x0041BBF8
		public void Notify_PawnAssigned(Pawn newPawn)
		{
			if (newPawn != null)
			{
				Pawn_AbilityTracker abilities = newPawn.abilities;
				if (abilities != null)
				{
					abilities.Notify_TemporaryAbilitiesChanged();
				}
			}
			if (newPawn != null)
			{
				newPawn.Notify_DisabledWorkTypesChanged();
			}
			if (newPawn != null)
			{
				Pawn_ApparelTracker apparel = newPawn.apparel;
				if (apparel != null)
				{
					apparel.Notify_RoleChanged();
				}
			}
			if (newPawn != null)
			{
				Messages.Message("MessageRoleAssigned".Translate(newPawn, this.LabelForPawn(newPawn)), newPawn, MessageTypeDefOf.SilentInput, false);
				SoundDefOf.Quest_Succeded.PlayOneShotOnCamera(null);
			}
			this.FillOrUpdateAbilities();
		}

		// Token: 0x0600E032 RID: 57394 RVA: 0x0041DA7C File Offset: 0x0041BC7C
		public RoleRequirement GetFirstUnmetRequirement(Pawn p)
		{
			if (!this.def.roleCanBeChild && !RoleRequirement_NotChild.Requirement.Met(p, this))
			{
				return RoleRequirement_NotChild.Requirement;
			}
			foreach (RoleRequirement roleRequirement in this.def.roleRequirements)
			{
				if (!roleRequirement.Met(p, this))
				{
					return roleRequirement;
				}
			}
			return null;
		}

		// Token: 0x0600E033 RID: 57395 RVA: 0x0041DB00 File Offset: 0x0041BD00
		public bool RequirementsMet(Pawn p)
		{
			if (!this.def.roleCanBeChild && !RoleRequirement_NotChild.Requirement.Met(p, this))
			{
				return false;
			}
			using (List<RoleRequirement>.Enumerator enumerator = this.def.roleRequirements.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.Met(p, this))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600E034 RID: 57396 RVA: 0x00402698 File Offset: 0x00400898
		public override void DrawIcon(Rect rect)
		{
			GUI.color = this.ideo.Color;
			GUI.DrawTexture(rect, this.Icon);
			GUI.color = Color.white;
		}

		// Token: 0x0600E035 RID: 57397 RVA: 0x0041DB80 File Offset: 0x0041BD80
		protected override void PostDrawBox(Rect rect, out bool anyTooltipActive)
		{
			bool localAnyTooltipActive = false;
			if (!this.apparelRequirements.NullOrEmpty<PreceptApparelRequirement>())
			{
				List<ThingDef> list = this.apparelRequirements.SelectMany((PreceptApparelRequirement ar) => ar.requirement.AllRequiredApparel(Gender.None)).Distinct<ThingDef>().ToList<ThingDef>();
				int num = list.Count * 24;
				num += (list.Count - 1) * 2;
				GenUI.DrawElementStack<ThingDef>(new Rect(rect.xMax - (float)num, rect.yMin, (float)num, 24f), 24f, list, delegate(Rect r, ThingDef apparel)
				{
					if (Mouse.IsOver(r))
					{
						localAnyTooltipActive = true;
						TooltipHandler.TipRegion(r, "RoleRequiredApparelLabel".Translate() + ": " + apparel.LabelCap + "\n\n" + apparel.DescriptionDetailed);
					}
					GUI.DrawTexture(r, apparel.uiIcon, ScaleMode.ScaleToFit, true, 0f, apparel.uiIconColor, 0f, 0f);
				}, (ThingDef apparel) => 24f, 0f, 2f, true);
			}
			anyTooltipActive = localAnyTooltipActive;
		}

		// Token: 0x0600E036 RID: 57398 RVA: 0x0041DC60 File Offset: 0x0041BE60
		public override List<Thought_Situational> SituationThoughtsToAdd(Pawn pawn, List<Thought_Situational> activeThoughts)
		{
			this.tmpSituationalThoughts.Clear();
			if (!this.Active || !pawn.IsFreeNonSlaveColonist || pawn.IsQuestLodger())
			{
				return this.tmpSituationalThoughts;
			}
			if (!this.def.leaderRole && this.def.createsRoleEmptyThought && !this.ChosenPawns().Any<Pawn>() && !activeThoughts.Any(delegate(Thought_Situational t)
			{
				Thought_IdeoRoleEmpty thought_IdeoRoleEmpty2 = t as Thought_IdeoRoleEmpty;
				return thought_IdeoRoleEmpty2 != null && thought_IdeoRoleEmpty2.Role == this;
			}))
			{
				Thought_IdeoRoleEmpty thought_IdeoRoleEmpty = (Thought_IdeoRoleEmpty)ThoughtMaker.MakeThought(ThoughtDefOf.IdeoRoleEmpty);
				if (thought_IdeoRoleEmpty != null)
				{
					thought_IdeoRoleEmpty.pawn = pawn;
					thought_IdeoRoleEmpty.sourcePrecept = this;
					this.tmpSituationalThoughts.Add(thought_IdeoRoleEmpty);
				}
			}
			if (this.IsAssigned(pawn) && this.apparelRequirements != null && this.apparelRequirements.Count > 0 && !pawn.IsQuestLodger() && !activeThoughts.Any(delegate(Thought_Situational t)
			{
				Thought_IdeoRoleApparelRequirementNotMet thought_IdeoRoleApparelRequirementNotMet2 = t as Thought_IdeoRoleApparelRequirementNotMet;
				return thought_IdeoRoleApparelRequirementNotMet2 != null && thought_IdeoRoleApparelRequirementNotMet2.Role == this;
			}))
			{
				Thought_IdeoRoleApparelRequirementNotMet thought_IdeoRoleApparelRequirementNotMet = (Thought_IdeoRoleApparelRequirementNotMet)ThoughtMaker.MakeThought(ThoughtDefOf.IdeoRoleApparelRequirementNotMet);
				if (thought_IdeoRoleApparelRequirementNotMet != null)
				{
					thought_IdeoRoleApparelRequirementNotMet.pawn = pawn;
					thought_IdeoRoleApparelRequirementNotMet.sourcePrecept = this;
					this.tmpSituationalThoughts.Add(thought_IdeoRoleApparelRequirementNotMet);
				}
			}
			return this.tmpSituationalThoughts;
		}

		// Token: 0x0600E037 RID: 57399 RVA: 0x0041DD6D File Offset: 0x0041BF6D
		protected bool ValidatePawn(Pawn p)
		{
			return p.Faction != null && (!p.Faction.IsPlayer || p.IsFreeNonSlaveColonist) && !p.Destroyed && !p.Dead && this.RequirementsMet(p);
		}

		// Token: 0x0600E038 RID: 57400 RVA: 0x0041DDAC File Offset: 0x0041BFAC
		public override void Notify_RemovedByReforming()
		{
			foreach (Pawn pawn in this.ChosenPawns())
			{
				if (pawn != null)
				{
					pawn.Notify_DisabledWorkTypesChanged();
				}
			}
		}

		// Token: 0x0600E039 RID: 57401 RVA: 0x0041DDFC File Offset: 0x0041BFFC
		public bool CanEquip(Pawn pawn, Thing thing, out string reason)
		{
			if (this.def.roleEffects != null)
			{
				foreach (RoleEffect roleEffect in this.def.roleEffects)
				{
					if (!roleEffect.CanEquip(pawn, thing))
					{
						reason = roleEffect.Label(pawn, this);
						return false;
					}
				}
			}
			reason = null;
			return true;
		}

		// Token: 0x0600E03A RID: 57402 RVA: 0x0041DE78 File Offset: 0x0041C078
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.active, "active", false, false);
			Scribe_Collections.Look<PreceptApparelRequirement>(ref this.apparelRequirements, "apparelRequirements", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.restrictToSupremeGender, "restrictToSupremeGender", false, false);
		}

		// Token: 0x0600E03B RID: 57403 RVA: 0x0041DEC8 File Offset: 0x0041C0C8
		public override void CopyTo(Precept precept)
		{
			base.CopyTo(precept);
			Precept_Role precept_Role = (Precept_Role)precept;
			precept_Role.active = this.active;
			precept_Role.restrictToSupremeGender = this.restrictToSupremeGender;
			if (this.apparelRequirements != null)
			{
				List<PreceptApparelRequirement> list = new List<PreceptApparelRequirement>();
				for (int i = 0; i < this.apparelRequirements.Count; i++)
				{
					PreceptApparelRequirement preceptApparelRequirement = new PreceptApparelRequirement();
					this.apparelRequirements[i].CopyTo(preceptApparelRequirement);
					list.Add(preceptApparelRequirement);
				}
				precept_Role.ApparelRequirements = list;
				return;
			}
			precept_Role.ApparelRequirements = null;
		}

		// Token: 0x0600E03D RID: 57405 RVA: 0x0041DF60 File Offset: 0x0041C160
		[CompilerGenerated]
		internal static void <AllPossibleRequirements>g__AddDistinct|30_0(IEnumerable<PreceptApparelRequirement> requirements, ref Precept_Role.<>c__DisplayClass30_0 A_1)
		{
			using (IEnumerator<PreceptApparelRequirement> enumerator = requirements.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PreceptApparelRequirement requirement = enumerator.Current;
					bool flag = false;
					Func<ThingDef, bool> <>9__3;
					foreach (PreceptApparelRequirement preceptApparelRequirement in A_1.apparelRequirementPool)
					{
						IEnumerable<ThingDef> source = preceptApparelRequirement.requirement.AllRequiredApparel(Gender.None);
						Func<ThingDef, bool> predicate;
						if ((predicate = <>9__3) == null)
						{
							predicate = (<>9__3 = ((ThingDef a) => requirement.requirement.AllRequiredApparel(Gender.None).Contains(a)));
						}
						if (source.All(predicate))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						A_1.apparelRequirementPool.Add(requirement);
					}
				}
			}
		}

		// Token: 0x0600E03E RID: 57406 RVA: 0x0041E03C File Offset: 0x0041C23C
		[CompilerGenerated]
		private void <GenerateNewApparelRequirements>g__ChooseApparelRequirements|31_0(List<PreceptApparelRequirement> source, ref Precept_Role.<>c__DisplayClass31_0 A_2)
		{
			if (A_2.apparelRequirements.Count >= A_2.apparelRequirementCount)
			{
				return;
			}
			List<PreceptApparelRequirement> list = new List<PreceptApparelRequirement>(source);
			list.Shuffle<PreceptApparelRequirement>();
			for (int i = 0; i < list.Count; i++)
			{
				string text;
				if (list[i].CanAddRequirement(this, A_2.apparelRequirements, out text, A_2.generatingFor) && !list[i].RequirementOverlapsOther(A_2.apparelRequirements, out text))
				{
					A_2.apparelRequirements.Add(list[i]);
					if (A_2.apparelRequirements.Count >= A_2.apparelRequirementCount)
					{
						break;
					}
				}
			}
		}

		// Token: 0x0600E03F RID: 57407 RVA: 0x0041E0D4 File Offset: 0x0041C2D4
		[CompilerGenerated]
		private void <GenerateNewApparelRequirements>g__Choose|31_1(bool desperate, ref Precept_Role.<>c__DisplayClass31_0 A_2)
		{
			List<PreceptApparelRequirement> list = Precept_Role.AllPossibleRequirements(this.ideo, this.def, desperate);
			List<PreceptApparelRequirement> list2 = new List<PreceptApparelRequirement>(list);
			using (List<PreceptApparelRequirement>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PreceptApparelRequirement req = enumerator.Current;
					Predicate<PreceptApparelRequirement> <>9__2;
					foreach (Precept_Role precept_Role in this.ideo.RolesListForReading)
					{
						if (precept_Role.apparelRequirements != null)
						{
							List<PreceptApparelRequirement> list3 = precept_Role.apparelRequirements;
							Predicate<PreceptApparelRequirement> predicate;
							if ((predicate = <>9__2) == null)
							{
								predicate = (<>9__2 = ((PreceptApparelRequirement o) => o.requirement.SameApparelAs(req.requirement)));
							}
							if (list3.Any(predicate))
							{
								list2.Remove(req);
							}
						}
					}
				}
			}
			this.<GenerateNewApparelRequirements>g__ChooseApparelRequirements|31_0(list2, ref A_2);
			if (A_2.apparelRequirements.Count < A_2.apparelRequirementCount)
			{
				this.<GenerateNewApparelRequirements>g__ChooseApparelRequirements|31_0(list, ref A_2);
			}
		}

		// Token: 0x040096C7 RID: 38599
		protected bool active;

		// Token: 0x040096C8 RID: 38600
		public bool restrictToSupremeGender;

		// Token: 0x040096C9 RID: 38601
		protected const string BulletPointString = "  - ";

		// Token: 0x040096CA RID: 38602
		public List<PreceptApparelRequirement> apparelRequirements;

		// Token: 0x040096CB RID: 38603
		private Dictionary<Gender, List<string>> allApparelRequirementLabelsCached = new Dictionary<Gender, List<string>>();
	}
}
