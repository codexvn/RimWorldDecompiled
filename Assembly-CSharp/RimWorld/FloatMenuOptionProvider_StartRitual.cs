using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003339 RID: 13113
	public class FloatMenuOptionProvider_StartRitual : FloatMenuOptionProvider
	{
		// Token: 0x17002FFD RID: 12285
		// (get) Token: 0x060126E7 RID: 75495 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Drafted
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002FFE RID: 12286
		// (get) Token: 0x060126E8 RID: 75496 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FFF RID: 12287
		// (get) Token: 0x060126E9 RID: 75497 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060126EA RID: 75498 RVA: 0x00565319 File Offset: 0x00563519
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return ModsConfig.IdeologyActive;
		}

		// Token: 0x060126EB RID: 75499 RVA: 0x0056E786 File Offset: 0x0056C986
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
		{
			FloatMenuOptionProvider_StartRitual.<>c__DisplayClass8_0 CS$<>8__locals1 = new FloatMenuOptionProvider_StartRitual.<>c__DisplayClass8_0();
			CS$<>8__locals1.clickedThing = clickedThing;
			CS$<>8__locals1.context = context;
			FloatMenuOptionProvider_StartRitual.showingGizmosForRitualsTmp.Clear();
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				int num;
				for (int i = 0; i < ideo.PreceptsListForReading.Count; i = num + 1)
				{
					FloatMenuOptionProvider_StartRitual.<>c__DisplayClass8_1 CS$<>8__locals2 = new FloatMenuOptionProvider_StartRitual.<>c__DisplayClass8_1();
					CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
					Precept precept = ideo.PreceptsListForReading[i];
					if (precept.def.showRitualFloatMenuOption)
					{
						CS$<>8__locals2.ritual = (precept as Precept_Ritual);
						if (CS$<>8__locals2.ritual != null)
						{
							if (CS$<>8__locals2.ritual.def.mergeRitualGizmosFromAllIdeos && FloatMenuOptionProvider_StartRitual.showingGizmosForRitualsTmp.Contains(CS$<>8__locals2.ritual.sourcePattern))
							{
								goto IL_71C;
							}
							if (!CS$<>8__locals2.ritual.activeObligations.NullOrEmpty<RitualObligation>())
							{
								bool disabledReasonSet = false;
								string disableReason = null;
								using (List<RitualObligation>.Enumerator enumerator2 = CS$<>8__locals2.ritual.activeObligations.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										FloatMenuOptionProvider_StartRitual.<>c__DisplayClass8_2 CS$<>8__locals3 = new FloatMenuOptionProvider_StartRitual.<>c__DisplayClass8_2();
										CS$<>8__locals3.CS$<>8__locals2 = CS$<>8__locals2;
										CS$<>8__locals3.obligation = enumerator2.Current;
										if (CS$<>8__locals3.CS$<>8__locals2.ritual.CanUseTarget(CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.clickedThing, CS$<>8__locals3.obligation).canUse)
										{
											if (!disabledReasonSet)
											{
												disabledReasonSet = true;
												disableReason = CS$<>8__locals3.CS$<>8__locals2.ritual.behavior.CanStartRitualNow(CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.clickedThing, CS$<>8__locals3.CS$<>8__locals2.ritual, CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.context.FirstSelectedPawn, null);
												if (CS$<>8__locals3.CS$<>8__locals2.ritual.abilityOnCooldownUntilTick > Find.TickManager.TicksGame)
												{
													disableReason = "AbilityOnCooldown".Translate((CS$<>8__locals3.CS$<>8__locals2.ritual.abilityOnCooldownUntilTick - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true, false)).Resolve();
												}
											}
											string text = CS$<>8__locals3.CS$<>8__locals2.ritual.GetBeginRitualText(CS$<>8__locals3.obligation);
											bool disabled = !disableReason.NullOrEmpty();
											if (disabled)
											{
												text = text + " (" + disableReason + ")";
											}
											Action action = delegate()
											{
												CS$<>8__locals3.CS$<>8__locals2.ritual.ShowRitualBeginWindow(CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.clickedThing, CS$<>8__locals3.obligation, CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.context.FirstSelectedPawn, null);
											};
											yield return new FloatMenuOption(text, (!disabled) ? action : null, CS$<>8__locals3.CS$<>8__locals2.ritual.Icon, (!CS$<>8__locals3.CS$<>8__locals2.ritual.def.mergeRitualGizmosFromAllIdeos) ? ideo.Color : Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false);
											FloatMenuOptionProvider_StartRitual.showingGizmosForRitualsTmp.Add(CS$<>8__locals3.CS$<>8__locals2.ritual.sourcePattern);
											if (!disabled && CS$<>8__locals3.CS$<>8__locals2.ritual.mergeGizmosForObligations)
											{
												break;
											}
											CS$<>8__locals3 = null;
										}
									}
								}
								List<RitualObligation>.Enumerator enumerator2 = default(List<RitualObligation>.Enumerator);
								disableReason = null;
							}
							else if (CS$<>8__locals2.ritual.isAnytime && CS$<>8__locals2.ritual.ShouldShowGizmo(CS$<>8__locals2.CS$<>8__locals1.clickedThing))
							{
								TaggedString taggedString = CS$<>8__locals2.ritual.GetBeginRitualText(null);
								RitualTargetUseReport ritualTargetUseReport = CS$<>8__locals2.ritual.CanUseTarget(CS$<>8__locals2.CS$<>8__locals1.clickedThing, null);
								string text2 = CS$<>8__locals2.ritual.behavior.CanStartRitualNow(CS$<>8__locals2.CS$<>8__locals1.clickedThing, CS$<>8__locals2.ritual, CS$<>8__locals2.CS$<>8__locals1.context.FirstSelectedPawn, null);
								if (CS$<>8__locals2.ritual.abilityOnCooldownUntilTick > Find.TickManager.TicksGame)
								{
									text2 = "AbilityOnCooldown".Translate((CS$<>8__locals2.ritual.abilityOnCooldownUntilTick - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true, false)).Resolve();
								}
								else if (CS$<>8__locals2.ritual.def.sourcePawnRoleDef != null && CS$<>8__locals2.ritual.def.sourceAbilityDef != null)
								{
									Precept_Role precept_Role = CS$<>8__locals2.ritual.ideo.RolesListForReading.FirstOrDefault((Precept_Role r) => r.def == CS$<>8__locals2.ritual.def.sourcePawnRoleDef);
									if (precept_Role != null)
									{
										Precept_RoleSingle precept_RoleSingle = precept_Role as Precept_RoleSingle;
										if (precept_RoleSingle != null && precept_RoleSingle.ChosenPawnSingle() != null)
										{
											Ability ability = precept_RoleSingle.AbilitiesForReading.FirstOrDefault((Ability a) => a.def == CS$<>8__locals2.ritual.def.sourceAbilityDef);
											if (ability != null)
											{
												ability.GizmoDisabled(out text2);
											}
										}
									}
								}
								if (!text2.NullOrEmpty())
								{
									taggedString += " (" + text2 + ")";
								}
								else if (!ritualTargetUseReport.failReason.NullOrEmpty())
								{
									taggedString += " (" + ritualTargetUseReport.failReason + ")";
								}
								yield return new FloatMenuOption(taggedString, (text2.NullOrEmpty() && ritualTargetUseReport.failReason.NullOrEmpty()) ? new Action(CS$<>8__locals2.<GetOptionsFor>g__Action|1) : null, CS$<>8__locals2.ritual.Icon, (!CS$<>8__locals2.ritual.def.mergeRitualGizmosFromAllIdeos) ? ideo.Color : Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false);
								FloatMenuOptionProvider_StartRitual.showingGizmosForRitualsTmp.Add(CS$<>8__locals2.ritual.sourcePattern);
							}
						}
						CS$<>8__locals2 = null;
					}
					IL_71C:
					num = i;
				}
				ideo = null;
			}
			IEnumerator<Ideo> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0400BF6E RID: 49006
		private static HashSet<RitualPatternDef> showingGizmosForRitualsTmp = new HashSet<RitualPatternDef>();
	}
}
