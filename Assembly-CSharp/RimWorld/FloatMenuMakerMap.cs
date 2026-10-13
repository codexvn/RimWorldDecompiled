using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020032B5 RID: 12981
	public static class FloatMenuMakerMap
	{
		// Token: 0x06012455 RID: 74837 RVA: 0x005623E4 File Offset: 0x005605E4
		public static void Init()
		{
			FloatMenuMakerMap.providers = new List<FloatMenuOptionProvider>();
			foreach (Type type in typeof(FloatMenuOptionProvider).AllSubclassesNonAbstract())
			{
				FloatMenuMakerMap.providers.Add((FloatMenuOptionProvider)Activator.CreateInstance(type));
			}
		}

		// Token: 0x06012456 RID: 74838 RVA: 0x00562458 File Offset: 0x00560658
		public static List<FloatMenuOption> GetOptions(List<Pawn> selectedPawns, Vector3 clickPos, out FloatMenuContext context)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			context = null;
			if (!clickPos.InBounds(Find.CurrentMap))
			{
				return list;
			}
			context = new FloatMenuContext(selectedPawns, clickPos, Find.CurrentMap);
			if (!context.allSelectedPawns.Any<Pawn>())
			{
				return list;
			}
			if (!context.ClickedCell.IsValid || !context.ClickedCell.InBounds(Find.CurrentMap))
			{
				return list;
			}
			if (!context.IsMultiselect)
			{
				AcceptanceReport acceptanceReport = FloatMenuMakerMap.ShouldGenerateFloatMenuForPawn(context.FirstSelectedPawn);
				if (!acceptanceReport.Accepted)
				{
					if (!acceptanceReport.Reason.NullOrEmpty())
					{
						Messages.Message(acceptanceReport.Reason, context.FirstSelectedPawn, MessageTypeDefOf.RejectInput, false);
					}
					return list;
				}
			}
			else
			{
				context.allSelectedPawns.RemoveAll((Pawn selectedPawn) => !FloatMenuMakerMap.ShouldGenerateFloatMenuForPawn(selectedPawn));
				if (!context.allSelectedPawns.Any<Pawn>())
				{
					return list;
				}
			}
			if (!context.IsMultiselect)
			{
				FloatMenuMakerMap.makingFor = context.FirstSelectedPawn;
			}
			FloatMenuMakerMap.GetProviderOptions(context, list);
			FloatMenuMakerMap.makingFor = null;
			return list;
		}

		// Token: 0x06012457 RID: 74839 RVA: 0x00562570 File Offset: 0x00560770
		private static void GetProviderOptions(FloatMenuContext context, List<FloatMenuOption> options)
		{
			foreach (FloatMenuOptionProvider floatMenuOptionProvider in FloatMenuMakerMap.providers)
			{
				try
				{
					FloatMenuMakerMap.currentProvider = floatMenuOptionProvider;
					if (context.ValidSelectedPawns.Any<Pawn>())
					{
						if (floatMenuOptionProvider.Applies(context))
						{
							options.AddRange(floatMenuOptionProvider.GetOptions(context));
							foreach (Thing thing in context.ClickedThings)
							{
								if (floatMenuOptionProvider.TargetThingValid(thing, context))
								{
									Thing thing2 = thing;
									CompSelectProxy compSelectProxy;
									if (thing2.TryGetComp(out compSelectProxy) && compSelectProxy.thingToSelect != null)
									{
										thing2 = compSelectProxy.thingToSelect;
									}
									foreach (FloatMenuOption floatMenuOption in floatMenuOptionProvider.GetOptionsFor(thing2, context))
									{
										FloatMenuOption floatMenuOption2 = floatMenuOption;
										if (floatMenuOption2.iconThing == null)
										{
											floatMenuOption2.iconThing = thing2;
										}
										floatMenuOption.targetsDespawned = !thing2.Spawned;
										options.Add(floatMenuOption);
									}
								}
							}
							foreach (Pawn pawn in context.ClickedPawns)
							{
								if (floatMenuOptionProvider.TargetPawnValid(pawn, context))
								{
									foreach (FloatMenuOption floatMenuOption3 in floatMenuOptionProvider.GetOptionsFor(pawn, context))
									{
										FloatMenuOption floatMenuOption2 = floatMenuOption3;
										if (floatMenuOption2.iconThing == null)
										{
											floatMenuOption2.iconThing = pawn;
										}
										floatMenuOption3.targetsDespawned = !pawn.Spawned;
										options.Add(floatMenuOption3);
									}
								}
							}
						}
					}
				}
				catch (Exception arg)
				{
					Log.Error(string.Format("Error in FloatMenuWorker {0}: {1}", floatMenuOptionProvider.GetType().Name, arg));
				}
			}
			FloatMenuMakerMap.currentProvider = null;
		}

		// Token: 0x06012458 RID: 74840 RVA: 0x00562800 File Offset: 0x00560A00
		public static AcceptanceReport ShouldGenerateFloatMenuForPawn(Pawn pawn)
		{
			if (pawn.Map != Find.CurrentMap)
			{
				return false;
			}
			if (pawn.Downed)
			{
				return "IsIncapped".Translate(pawn.LabelCap, pawn);
			}
			if (ModsConfig.BiotechActive && pawn.Deathresting)
			{
				return "IsDeathresting".Translate(pawn.Named("PAWN"));
			}
			Lord lord = pawn.GetLord();
			if (lord != null)
			{
				AcceptanceReport result = lord.AllowsFloatMenu(pawn);
				if (!result.Accepted)
				{
					return result;
				}
			}
			return true;
		}

		// Token: 0x06012459 RID: 74841 RVA: 0x00562898 File Offset: 0x00560A98
		public static FloatMenuOption GetAutoTakeOption(List<FloatMenuOption> options)
		{
			bool flag = true;
			FloatMenuOption floatMenuOption = null;
			foreach (FloatMenuOption floatMenuOption2 in options)
			{
				if (floatMenuOption2.Disabled || !floatMenuOption2.autoTakeable)
				{
					flag = false;
					break;
				}
				if (floatMenuOption == null || floatMenuOption2.autoTakeablePriority > floatMenuOption.autoTakeablePriority)
				{
					floatMenuOption = floatMenuOption2;
				}
			}
			if (!flag || floatMenuOption == null)
			{
				return null;
			}
			return floatMenuOption;
		}

		// Token: 0x0400BE2F RID: 48687
		private static List<FloatMenuOptionProvider> providers;

		// Token: 0x0400BE30 RID: 48688
		public static FloatMenuOptionProvider currentProvider;

		// Token: 0x0400BE31 RID: 48689
		public static Pawn makingFor;
	}
}
