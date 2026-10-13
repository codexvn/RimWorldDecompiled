using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020032E5 RID: 13029
	public class FloatMenuOptionProvider_DraftedAttack : FloatMenuOptionProvider
	{
		// Token: 0x17002F74 RID: 12148
		// (get) Token: 0x06012548 RID: 75080 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F75 RID: 12149
		// (get) Token: 0x06012549 RID: 75081 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Undrafted
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002F76 RID: 12150
		// (get) Token: 0x0601254A RID: 75082 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Multiselect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F77 RID: 12151
		// (get) Token: 0x0601254B RID: 75083 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool MechanoidCanDo
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0601254C RID: 75084 RVA: 0x00566C10 File Offset: 0x00564E10
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
		{
			if (!FloatMenuOptionProvider_DraftedAttack.CanTarget(clickedThing))
			{
				yield break;
			}
			if (context.IsMultiselect)
			{
				yield return this.GetMultiselectAttackOption(clickedThing, context);
			}
			else
			{
				foreach (FloatMenuOption floatMenuOption in FloatMenuOptionProvider_DraftedAttack.GetSingleSelectAttackOptions(clickedThing, context))
				{
					yield return floatMenuOption;
				}
				IEnumerator<FloatMenuOption> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0601254D RID: 75085 RVA: 0x00566C30 File Offset: 0x00564E30
		private static bool CanTarget(Thing clickedThing)
		{
			if (clickedThing.def.noRightClickDraftAttack && clickedThing.HostileTo(Faction.OfPlayer))
			{
				return false;
			}
			if (clickedThing.def.IsNonDeconstructibleAttackableBuilding)
			{
				return true;
			}
			BuildingProperties building = clickedThing.def.building;
			if (building != null && building.quickTargetable)
			{
				return true;
			}
			if (!clickedThing.def.destroyable)
			{
				return false;
			}
			if (clickedThing.HostileTo(Faction.OfPlayer))
			{
				return true;
			}
			Pawn pawn = clickedThing as Pawn;
			return pawn != null && pawn.NonHumanlikeOrWildMan();
		}

		// Token: 0x0601254E RID: 75086 RVA: 0x00566CB6 File Offset: 0x00564EB6
		private static IEnumerable<FloatMenuOption> GetSingleSelectAttackOptions(Thing clickedThing, FloatMenuContext context)
		{
			string text = null;
			Pawn pawn = context.FirstSelectedPawn;
			bool hostile = clickedThing.HostileTo(Faction.OfPlayer);
			if (ModsConfig.BiotechActive && pawn.IsColonyMech && !MechanitorUtility.InMechanitorCommandRange(pawn, clickedThing))
			{
				text = "OutOfCommandRange".Translate();
				yield return new FloatMenuOption("Attack".Translate(clickedThing.Label, clickedThing) + ": " + text, null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			string text2;
			Action rangedAction = FloatMenuOptionProvider_DraftedAttack.GetRangedAttackAction(pawn, clickedThing, out text2, out text);
			if (rangedAction != null)
			{
				FloatMenuOption floatMenuOption = new FloatMenuOption(text2, delegate()
				{
					FleckMaker.Static(clickedThing.DrawPos, clickedThing.Map, FleckDefOf.FeedbackShoot, 1f);
					rangedAction();
				}, MenuOptionPriority.AttackEnemy, null, null, 0f, null, null, true, 0);
				floatMenuOption.Priority = (hostile ? MenuOptionPriority.AttackEnemy : MenuOptionPriority.VeryLow);
				FloatMenuOption floatMenuOption2 = floatMenuOption;
				bool autoTakeable;
				if (!hostile)
				{
					BuildingProperties building = clickedThing.def.building;
					autoTakeable = (building != null && building.quickTargetable);
				}
				else
				{
					autoTakeable = true;
				}
				floatMenuOption2.autoTakeable = autoTakeable;
				floatMenuOption.autoTakeablePriority = 40f;
				yield return floatMenuOption;
			}
			else if (!text.NullOrEmpty())
			{
				yield return new FloatMenuOption((text2 ?? "Attack".Translate(clickedThing.Label, clickedThing)) + ": " + text, null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			string text3;
			Action meleeAction = FloatMenuOptionProvider_DraftedAttack.GetMeleeAttackAction(pawn, clickedThing, out text3, out text);
			if (meleeAction != null)
			{
				FloatMenuOption floatMenuOption3 = new FloatMenuOption(text3, delegate()
				{
					FleckMaker.Static(clickedThing.DrawPos, clickedThing.Map, FleckDefOf.FeedbackMelee, 1f);
					meleeAction();
				}, MenuOptionPriority.AttackEnemy, null, null, 0f, null, null, true, 0);
				floatMenuOption3.Priority = (hostile ? MenuOptionPriority.AttackEnemy : MenuOptionPriority.VeryLow);
				FloatMenuOption floatMenuOption4 = floatMenuOption3;
				bool autoTakeable2;
				if (!hostile)
				{
					BuildingProperties building = clickedThing.def.building;
					autoTakeable2 = (building != null && building.quickTargetable);
				}
				else
				{
					autoTakeable2 = true;
				}
				floatMenuOption4.autoTakeable = autoTakeable2;
				floatMenuOption3.autoTakeablePriority = 30f;
				yield return floatMenuOption3;
			}
			else if (!text.NullOrEmpty())
			{
				yield return new FloatMenuOption((text3 ?? "Attack".Translate(clickedThing.Label, clickedThing)) + ": " + text, null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			yield break;
		}

		// Token: 0x0601254F RID: 75087 RVA: 0x00566CD0 File Offset: 0x00564ED0
		private FloatMenuOption GetMultiselectAttackOption(Thing clickedThing, FloatMenuContext context)
		{
			FloatMenuOptionProvider_DraftedAttack.tmpPawns.Clear();
			string text = null;
			bool flag = clickedThing.HostileTo(Faction.OfPlayer);
			foreach (Pawn pawn in context.ValidSelectedPawns)
			{
				string text2;
				if (FloatMenuOptionProvider_DraftedAttack.GetAttackAction(pawn, clickedThing, out text, out text2) != null)
				{
					FloatMenuOptionProvider_DraftedAttack.tmpPawns.Add(pawn);
				}
			}
			if (FloatMenuOptionProvider_DraftedAttack.tmpPawns.Count == 0)
			{
				return null;
			}
			FleckDef fleck = FloatMenuUtility.UseRangedAttack(FloatMenuOptionProvider_DraftedAttack.tmpPawns[0]) ? FleckDefOf.FeedbackShoot : FleckDefOf.FeedbackMelee;
			FloatMenuOption floatMenuOption = new FloatMenuOption(text ?? "Attack".Translate(clickedThing.Label, clickedThing), delegate()
			{
				FleckMaker.Static(clickedThing.DrawPos, clickedThing.Map, fleck, 1f);
				foreach (Pawn pawn2 in FloatMenuOptionProvider_DraftedAttack.tmpPawns)
				{
					string text3;
					string text4;
					Action attackAction = FloatMenuOptionProvider_DraftedAttack.GetAttackAction(pawn2, clickedThing, out text3, out text4);
					if (attackAction != null)
					{
						attackAction();
					}
				}
			}, MenuOptionPriority.AttackEnemy, null, null, 0f, null, null, true, 0);
			floatMenuOption.Priority = (flag ? MenuOptionPriority.AttackEnemy : MenuOptionPriority.VeryLow);
			bool autoTakeable;
			if (!flag)
			{
				BuildingProperties building = clickedThing.def.building;
				autoTakeable = (building != null && building.quickTargetable);
			}
			else
			{
				autoTakeable = true;
			}
			floatMenuOption.autoTakeable = autoTakeable;
			floatMenuOption.autoTakeablePriority = 40f;
			return floatMenuOption;
		}

		// Token: 0x06012550 RID: 75088 RVA: 0x00566E24 File Offset: 0x00565024
		private static Action GetAttackAction(Pawn pawn, Thing target, out string label, out string failStr)
		{
			failStr = null;
			label = "Attack".Translate(target.Label, target);
			if (ModsConfig.BiotechActive && pawn.IsColonyMech && !MechanitorUtility.InMechanitorCommandRange(pawn, target))
			{
				failStr = "OutOfCommandRange".Translate();
				return null;
			}
			Action rangedAttackAction = FloatMenuOptionProvider_DraftedAttack.GetRangedAttackAction(pawn, target, out label, out failStr);
			if (rangedAttackAction != null)
			{
				return rangedAttackAction;
			}
			return FloatMenuOptionProvider_DraftedAttack.GetMeleeAttackAction(pawn, target, out label, out failStr);
		}

		// Token: 0x06012551 RID: 75089 RVA: 0x00566EA0 File Offset: 0x005650A0
		private static Action GetRangedAttackAction(Pawn pawn, Thing target, out string label, out string failStr)
		{
			failStr = null;
			label = "FireAt".Translate(target.Label, target);
			if (!FloatMenuUtility.UseRangedAttack(pawn))
			{
				return null;
			}
			label = "FireAt".Translate(target.Label, target);
			return FloatMenuUtility.GetRangedAttackAction(pawn, target, out failStr);
		}

		// Token: 0x06012552 RID: 75090 RVA: 0x00566F0C File Offset: 0x0056510C
		private static Action GetMeleeAttackAction(Pawn pawn, Thing target, out string label, out string failStr)
		{
			failStr = null;
			label = "Attack".Translate(target.Label, target);
			Pawn pawn2 = target as Pawn;
			if (pawn2 != null && pawn2.Downed)
			{
				label = "MeleeAttackToDeath".Translate(target.Label, target);
			}
			else
			{
				label = "MeleeAttack".Translate(target.Label, target);
			}
			return FloatMenuUtility.GetMeleeAttackAction(pawn, target, out failStr, false);
		}

		// Token: 0x06012553 RID: 75091 RVA: 0x00566FA4 File Offset: 0x005651A4
		[Obsolete]
		protected override FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
		{
			return base.GetSingleOptionFor(clickedThing, context);
		}

		// Token: 0x06012554 RID: 75092 RVA: 0x00566FB0 File Offset: 0x005651B0
		[Obsolete]
		private static FloatMenuOption GetSingleSelectAttackOption(Thing clickedThing, FloatMenuContext context)
		{
			string text;
			string text2;
			Action action = FloatMenuOptionProvider_DraftedAttack.GetAttackAction(context.FirstSelectedPawn, clickedThing, out text, out text2);
			FleckDef fleck = FloatMenuUtility.UseRangedAttack(context.FirstSelectedPawn) ? FleckDefOf.FeedbackShoot : FleckDefOf.FeedbackMelee;
			if (action != null)
			{
				return new FloatMenuOption(text ?? "Attack".Translate(clickedThing.Label, clickedThing), delegate()
				{
					FleckMaker.Static(clickedThing.DrawPos, clickedThing.Map, fleck, 1f);
					action();
				}, MenuOptionPriority.AttackEnemy, null, null, 0f, null, null, true, 0);
			}
			if (!text2.NullOrEmpty())
			{
				return new FloatMenuOption((text ?? "Attack".Translate(clickedThing.Label, clickedThing)) + ": " + text2, null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			return null;
		}

		// Token: 0x0400BEA5 RID: 48805
		private static readonly List<Pawn> tmpPawns = new List<Pawn>();
	}
}
