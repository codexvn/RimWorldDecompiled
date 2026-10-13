using System;
using System.Collections.Generic;
using System.Linq;
using LudeonTK;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B3D RID: 2877
	public static class DebugTools_Health
	{
		// Token: 0x06004A73 RID: 19059 RVA: 0x0017AF78 File Offset: 0x00179178
		public static List<DebugMenuOption> Options_RestorePart(Pawn p)
		{
			if (p == null)
			{
				throw new ArgumentNullException("p");
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (BodyPartRecord localPart2 in p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null))
			{
				BodyPartRecord localPart = localPart2;
				list.Add(new DebugMenuOption(localPart.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					p.health.RestorePart(localPart, null, true);
				}));
			}
			return list;
		}

		// Token: 0x06004A74 RID: 19060 RVA: 0x0017B034 File Offset: 0x00179234
		public static List<DebugActionNode> Options_ApplyDamage()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (DamageDef localDef2 in DefDatabase<DamageDef>.AllDefs)
			{
				DamageDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.Action, null, null)
				{
					actionType = DebugActionType.ToolMapForPawns,
					pawnAction = delegate(Pawn p)
					{
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_Damage_BodyParts(p, localDef, 5f), null));
					}
				});
				list.Add(new DebugActionNode(localDef.defName + " (Destroy part)", DebugActionType.Action, null, null)
				{
					actionType = DebugActionType.ToolMapForPawns,
					pawnAction = delegate(Pawn p)
					{
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_Damage_BodyParts(p, localDef, 99999f), null));
					}
				});
			}
			return list;
		}

		// Token: 0x06004A75 RID: 19061 RVA: 0x0017B104 File Offset: 0x00179304
		private static List<DebugMenuOption> Options_Damage_BodyParts(Pawn p, DamageDef def, float amount = 5f)
		{
			if (p == null)
			{
				throw new ArgumentNullException("p");
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("(no body part)", DebugMenuOptionMode.Action, delegate()
			{
				p.TakeDamage(new DamageInfo(def, 5f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			}));
			foreach (BodyPartRecord localPart2 in p.RaceProps.body.AllParts)
			{
				BodyPartRecord localPart = localPart2;
				list.Add(new DebugMenuOption(localPart.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					p.TakeDamage(new DamageInfo(def, amount, 0f, -1f, null, localPart, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
				}));
			}
			return list;
		}

		// Token: 0x06004A76 RID: 19062 RVA: 0x0017B1F0 File Offset: 0x001793F0
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, actionType = DebugActionType.Action)]
		public static List<DebugActionNode> AddHediff()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (HediffDef localDef2 in DefDatabase<HediffDef>.AllDefs.OrderBy(delegate(HediffDef d)
			{
				if (!d.label.NullOrEmpty())
				{
					return d.label;
				}
				return d.hediffClass.ToStringSafe<Type>();
			}))
			{
				HediffDef localDef = localDef2;
				string text = localDef.LabelCap;
				if (!localDef.debugLabelExtra.NullOrEmpty())
				{
					text = text + " (" + localDef.debugLabelExtra + ")";
				}
				list.Add(new DebugActionNode(text, DebugActionType.ToolMapForPawns, null, delegate(Pawn p)
				{
					Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_Hediff_BodyParts(p, localDef), null));
				}));
			}
			return list;
		}

		// Token: 0x06004A77 RID: 19063 RVA: 0x0017B2CC File Offset: 0x001794CC
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.PlayingOnMap, actionType = DebugActionType.ToolMapForPawns)]
		public static void TendBleedingHediffs(Pawn pawn)
		{
			foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
			{
				if (hediff.Bleeding)
				{
					hediff.Tended(1f, 1f, 0);
				}
			}
		}

		// Token: 0x06004A78 RID: 19064 RVA: 0x0017B33C File Offset: 0x0017953C
		public static List<DebugMenuOption> Options_AddHediff(Pawn pawn)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (HediffDef localDef2 in DefDatabase<HediffDef>.AllDefs.OrderBy(delegate(HediffDef d)
			{
				if (!d.label.NullOrEmpty())
				{
					return d.label;
				}
				return d.hediffClass.ToStringSafe<Type>();
			}))
			{
				HediffDef localDef = localDef2;
				DebugMenuOptionMode mode = (pawn != null) ? DebugMenuOptionMode.Action : DebugMenuOptionMode.Tool;
				string text = localDef.LabelCap;
				if (!localDef.debugLabelExtra.NullOrEmpty())
				{
					text = text + " (" + localDef.debugLabelExtra + ")";
				}
				list.Add(new DebugMenuOption(text, mode, delegate()
				{
					Pawn pawn2;
					if (mode == DebugMenuOptionMode.Tool)
					{
						pawn2 = (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
						where t is Pawn
						select t).Cast<Pawn>().FirstOrDefault<Pawn>();
					}
					else
					{
						pawn2 = pawn;
					}
					if (pawn2 != null)
					{
						Find.WindowStack.Add(new Dialog_DebugOptionListLister(DebugTools_Health.Options_Hediff_BodyParts(pawn2, localDef), null));
					}
				}));
			}
			return list;
		}

		// Token: 0x06004A79 RID: 19065 RVA: 0x0017B458 File Offset: 0x00179658
		private static List<DebugMenuOption> Options_Hediff_BodyParts(Pawn p, HediffDef def)
		{
			if (p == null)
			{
				throw new ArgumentNullException("p");
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			list.Add(new DebugMenuOption("(no body part)", DebugMenuOptionMode.Action, delegate()
			{
				p.health.AddHediff(def, null, null, null).PostDebugAdd();
			}));
			foreach (BodyPartRecord localPart2 in from pa in p.RaceProps.body.AllParts
			orderby pa.Label
			select pa)
			{
				BodyPartRecord localPart = localPart2;
				list.Add(new DebugMenuOption(localPart.LabelCap, DebugMenuOptionMode.Action, delegate()
				{
					p.health.AddHediff(def, localPart, null, null).PostDebugAdd();
				}));
			}
			return list;
		}

		// Token: 0x06004A7A RID: 19066 RVA: 0x0017B558 File Offset: 0x00179758
		public static List<DebugMenuOption> Options_RemoveHediff(Pawn pawn)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			foreach (Hediff localH2 in pawn.health.hediffSet.hediffs)
			{
				Hediff localH = localH2;
				List<DebugMenuOption> list2 = list;
				string labelCap = localH.LabelCap;
				string str;
				if (localH.Part == null)
				{
					str = "";
				}
				else
				{
					string str2 = " (";
					BodyPartDef def = localH.Part.def;
					str = str2 + ((def != null) ? def.ToString() : null) + ")";
				}
				list2.Add(new DebugMenuOption(labelCap + str, DebugMenuOptionMode.Action, delegate()
				{
					pawn.health.RemoveHediff(localH);
				}));
			}
			return list;
		}
	}
}
