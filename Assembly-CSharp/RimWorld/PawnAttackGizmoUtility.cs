using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002323 RID: 8995
	public static class PawnAttackGizmoUtility
	{
		// Token: 0x0600C345 RID: 49989 RVA: 0x00389EF0 File Offset: 0x003880F0
		public static IEnumerable<Gizmo> GetAttackGizmos(Pawn pawn)
		{
			if (PawnAttackGizmoUtility.ShouldUseMeleeAttackGizmo(pawn))
			{
				yield return PawnAttackGizmoUtility.GetMeleeAttackGizmo(pawn);
			}
			if (PawnAttackGizmoUtility.ShouldUseSquadAttackGizmo())
			{
				yield return PawnAttackGizmoUtility.GetSquadAttackGizmo(pawn);
			}
			yield break;
		}

		// Token: 0x0600C346 RID: 49990 RVA: 0x00389F00 File Offset: 0x00388100
		public static bool CanShowEquipmentGizmos()
		{
			return !PawnAttackGizmoUtility.AtLeastTwoSelectedPlayerPawnsHaveDifferentWeapons();
		}

		// Token: 0x0600C347 RID: 49991 RVA: 0x00389F0A File Offset: 0x0038810A
		private static bool ShouldUseSquadAttackGizmo()
		{
			return PawnAttackGizmoUtility.AtLeastOneSelectedPlayerPawnHasRangedWeapon() && PawnAttackGizmoUtility.AtLeastTwoSelectedPlayerPawnsHaveDifferentWeapons();
		}

		// Token: 0x0600C348 RID: 49992 RVA: 0x00389F1A File Offset: 0x0038811A
		private static bool CanOrderPlayerPawn(Pawn pawn)
		{
			return pawn.IsColonistPlayerControlled || pawn.IsColonyMechPlayerControlled || pawn.IsColonySubhumanPlayerControlled;
		}

		// Token: 0x0600C349 RID: 49993 RVA: 0x00389F34 File Offset: 0x00388134
		private static Gizmo GetSquadAttackGizmo(Pawn pawn)
		{
			Command_Target command_Target = new Command_Target();
			command_Target.defaultLabel = "CommandSquadAttack".Translate();
			command_Target.defaultDesc = "CommandSquadAttackDesc".Translate();
			command_Target.hotKey = KeyBindingDefOf.Misc1;
			command_Target.icon = TexCommand.SquadAttack;
			command_Target.targetingParams = TargetingParameters.ForAttackAny();
			command_Target.targetingParams.canTargetLocations = PawnAttackGizmoUtility.AllSelectedPlayerPawnsCanTargetLocations();
			string str;
			if (FloatMenuUtility.GetAttackAction(pawn, LocalTargetInfo.Invalid, out str) == null)
			{
				command_Target.Disable(str.CapitalizeFirst() + ".");
			}
			command_Target.action = delegate(LocalTargetInfo target)
			{
				foreach (Pawn pawn2 in Find.Selector.SelectedObjects.Where(delegate(object x)
				{
					Pawn pawn3 = x as Pawn;
					return pawn3 != null && PawnAttackGizmoUtility.CanOrderPlayerPawn(pawn3) && pawn3.Drafted;
				}).Cast<Pawn>())
				{
					string text;
					Action attackAction = FloatMenuUtility.GetAttackAction(pawn2, target, out text);
					if (attackAction != null)
					{
						attackAction();
					}
					else if (!text.NullOrEmpty())
					{
						Messages.Message(text, target.Thing, MessageTypeDefOf.RejectInput, false);
					}
				}
			};
			command_Target.onUpdate = delegate(LocalTargetInfo target)
			{
				foreach (Pawn pawn2 in Find.Selector.SelectedObjects.Where(delegate(object x)
				{
					Pawn pawn3 = x as Pawn;
					return pawn3 != null && PawnAttackGizmoUtility.CanOrderPlayerPawn(pawn3) && pawn3.Drafted;
				}).Cast<Pawn>())
				{
					ThingWithComps thingWithComps = pawn2.equipment.AllEquipmentListForReading.FirstOrDefault((ThingWithComps w) => w.def.IsRangedWeapon);
					CompEquippable compEquippable;
					if (thingWithComps != null && thingWithComps.TryGetComp(out compEquippable))
					{
						Verb verb = compEquippable.AllVerbs.FirstOrDefault((Verb v) => v.verbProps.hasStandardCommand);
						if (verb != null)
						{
							verb.verbProps.DrawRadiusRing(pawn2.Position, verb);
						}
					}
				}
			};
			return command_Target;
		}

		// Token: 0x0600C34A RID: 49994 RVA: 0x0038A012 File Offset: 0x00388212
		private static bool ShouldUseMeleeAttackGizmo(Pawn pawn)
		{
			return pawn.Drafted && (PawnAttackGizmoUtility.AtLeastOneSelectedPlayerPawnHasRangedWeapon() || PawnAttackGizmoUtility.AtLeastOneSelectedPlayerPawnHasNoWeapon() || PawnAttackGizmoUtility.AtLeastTwoSelectedPlayerPawnsHaveDifferentWeapons());
		}

		// Token: 0x0600C34B RID: 49995 RVA: 0x0038A034 File Offset: 0x00388234
		public static Gizmo GetMeleeAttackGizmo(Pawn pawn)
		{
			Command_Target command_Target = new Command_Target();
			command_Target.defaultLabel = "CommandMeleeAttack".Translate();
			command_Target.defaultDesc = "CommandMeleeAttackDesc".Translate();
			command_Target.targetingParams = TargetingParameters.ForAttackAny();
			command_Target.hotKey = KeyBindingDefOf.Misc2;
			command_Target.icon = TexCommand.AttackMelee;
			string str;
			if (FloatMenuUtility.GetMeleeAttackAction(pawn, LocalTargetInfo.Invalid, out str, false) == null)
			{
				command_Target.Disable(str.CapitalizeFirst() + ".");
			}
			command_Target.action = delegate(LocalTargetInfo target)
			{
				foreach (Pawn pawn2 in Find.Selector.SelectedObjects.Where(delegate(object x)
				{
					Pawn pawn3 = x as Pawn;
					return pawn3 != null && PawnAttackGizmoUtility.CanOrderPlayerPawn(pawn3) && pawn3.Drafted;
				}).Cast<Pawn>())
				{
					string text;
					Action meleeAttackAction = FloatMenuUtility.GetMeleeAttackAction(pawn2, target, out text, false);
					if (meleeAttackAction != null)
					{
						meleeAttackAction();
					}
					else if (!text.NullOrEmpty())
					{
						Messages.Message(text, target.Thing, MessageTypeDefOf.RejectInput, false);
					}
				}
			};
			return command_Target;
		}

		// Token: 0x0600C34C RID: 49996 RVA: 0x0038A0E0 File Offset: 0x003882E0
		private static bool AtLeastOneSelectedPlayerPawnHasRangedWeapon()
		{
			List<object> selectedObjectsListForReading = Find.Selector.SelectedObjectsListForReading;
			for (int i = 0; i < selectedObjectsListForReading.Count; i++)
			{
				Pawn pawn = selectedObjectsListForReading[i] as Pawn;
				if (pawn != null && PawnAttackGizmoUtility.CanOrderPlayerPawn(pawn) && pawn.equipment != null && pawn.equipment.Primary != null && pawn.equipment.Primary.def.IsRangedWeapon)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600C34D RID: 49997 RVA: 0x0038A150 File Offset: 0x00388350
		private static bool AtLeastOneSelectedPlayerPawnHasNoWeapon()
		{
			List<object> selectedObjectsListForReading = Find.Selector.SelectedObjectsListForReading;
			for (int i = 0; i < selectedObjectsListForReading.Count; i++)
			{
				Pawn pawn = selectedObjectsListForReading[i] as Pawn;
				if (pawn != null && PawnAttackGizmoUtility.CanOrderPlayerPawn(pawn) && (pawn.equipment == null || pawn.equipment.Primary == null))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600C34E RID: 49998 RVA: 0x0038A1AC File Offset: 0x003883AC
		private static bool AtLeastTwoSelectedPlayerPawnsHaveDifferentWeapons()
		{
			if (Find.Selector.NumSelected <= 1)
			{
				return false;
			}
			ThingDef thingDef = null;
			bool flag = false;
			List<object> selectedObjectsListForReading = Find.Selector.SelectedObjectsListForReading;
			for (int i = 0; i < selectedObjectsListForReading.Count; i++)
			{
				Pawn pawn = selectedObjectsListForReading[i] as Pawn;
				if (pawn != null && PawnAttackGizmoUtility.CanOrderPlayerPawn(pawn))
				{
					ThingDef thingDef2;
					if (pawn.equipment == null || pawn.equipment.Primary == null)
					{
						thingDef2 = null;
					}
					else
					{
						thingDef2 = pawn.equipment.Primary.def;
					}
					if (!flag)
					{
						thingDef = thingDef2;
						flag = true;
					}
					else if (thingDef2 != thingDef)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600C34F RID: 49999 RVA: 0x0038A244 File Offset: 0x00388444
		private static bool AllSelectedPlayerPawnsCanTargetLocations()
		{
			foreach (object obj in Find.Selector.SelectedObjects)
			{
				Pawn pawn = obj as Pawn;
				if (pawn != null && PawnAttackGizmoUtility.CanOrderPlayerPawn(pawn) && pawn.Drafted)
				{
					if (pawn.equipment.Primary == null || pawn.equipment.PrimaryEq.PrimaryVerb.verbProps.IsMeleeAttack)
					{
						return false;
					}
					if (!pawn.equipment.PrimaryEq.PrimaryVerb.verbProps.targetParams.canTargetLocations)
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
