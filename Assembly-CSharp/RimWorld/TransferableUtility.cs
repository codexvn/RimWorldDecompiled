using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200326D RID: 12909
	public static class TransferableUtility
	{
		// Token: 0x06012280 RID: 74368 RVA: 0x00551DC8 File Offset: 0x0054FFC8
		public static void Transfer(List<Thing> things, int count, Action<Thing, IThingHolder> transferred)
		{
			if (count <= 0)
			{
				return;
			}
			TransferableUtility.tmpThings.Clear();
			TransferableUtility.tmpThings.AddRange(things);
			int num = count;
			for (int i = 0; i < TransferableUtility.tmpThings.Count; i++)
			{
				Thing thing = TransferableUtility.tmpThings[i];
				int num2 = Mathf.Min(num, thing.stackCount);
				if (num2 > 0)
				{
					IThingHolder parentHolder = thing.ParentHolder;
					Thing thing2 = thing.SplitOff(num2);
					num -= num2;
					if (thing2 == thing)
					{
						things.Remove(thing);
					}
					transferred(thing2, parentHolder);
					if (num <= 0)
					{
						break;
					}
				}
			}
			TransferableUtility.tmpThings.Clear();
			if (num > 0)
			{
				Log.Error("Can't transfer things because there is nothing left.");
			}
		}

		// Token: 0x06012281 RID: 74369 RVA: 0x00551E6C File Offset: 0x0055006C
		public static void TransferNoSplit(List<Thing> things, int count, Action<Thing, int> transfer, bool removeIfTakingEntireThing = true, bool errorIfNotEnoughThings = true)
		{
			if (count <= 0)
			{
				return;
			}
			TransferableUtility.tmpThings.Clear();
			TransferableUtility.tmpThings.AddRange(things);
			int num = count;
			for (int i = 0; i < TransferableUtility.tmpThings.Count; i++)
			{
				Thing thing = TransferableUtility.tmpThings[i];
				int num2 = Mathf.Min(num, thing.stackCount);
				if (num2 > 0)
				{
					num -= num2;
					if (removeIfTakingEntireThing && num2 >= thing.stackCount)
					{
						things.Remove(thing);
					}
					transfer(thing, num2);
					if (num <= 0)
					{
						break;
					}
				}
			}
			TransferableUtility.tmpThings.Clear();
			if (num > 0 && errorIfNotEnoughThings)
			{
				Log.Error("Can't transfer things because there is nothing left.");
			}
		}

		// Token: 0x06012282 RID: 74370 RVA: 0x00551F08 File Offset: 0x00550108
		public static bool TransferAsOne(Thing a, Thing b, TransferAsOneMode mode)
		{
			if (a == b)
			{
				return true;
			}
			if (a.def != b.def)
			{
				return false;
			}
			a = a.GetInnerIfMinified();
			b = b.GetInnerIfMinified();
			if (a.def.tradeNeverStack || b.def.tradeNeverStack)
			{
				return false;
			}
			if (!TransferableUtility.CanStackTogether(a, b))
			{
				return false;
			}
			if (!TransferableUtility.CanStack(a) || !TransferableUtility.CanStack(b))
			{
				return false;
			}
			if (a.def != b.def || a.Stuff != b.Stuff)
			{
				return false;
			}
			if (a is Corpse && b is Corpse)
			{
				Pawn innerPawn = ((Corpse)a).InnerPawn;
				Pawn innerPawn2 = ((Corpse)b).InnerPawn;
				return innerPawn.def == innerPawn2.def && innerPawn.kindDef == innerPawn2.kindDef && !innerPawn.RaceProps.Humanlike && !innerPawn2.RaceProps.Humanlike && (innerPawn.Name == null || innerPawn.Name.Numerical) && (innerPawn2.Name == null || innerPawn2.Name.Numerical);
			}
			if (a.def.category == ThingCategory.Pawn)
			{
				if (b.def != a.def)
				{
					return false;
				}
				Pawn pawn = (Pawn)a;
				Pawn pawn2 = (Pawn)b;
				if (pawn.kindDef != pawn2.kindDef)
				{
					return false;
				}
				if (pawn.gender != pawn2.gender)
				{
					return false;
				}
				if (pawn.ageTracker.CurLifeStageIndex != pawn2.ageTracker.CurLifeStageIndex)
				{
					return false;
				}
				if (Mathf.Abs(pawn.ageTracker.AgeBiologicalYearsFloat - pawn2.ageTracker.AgeBiologicalYearsFloat) > 1f)
				{
					return false;
				}
				if (pawn.RaceProps.Animal)
				{
					if (pawn.health.hediffSet.AnyHediffMakesSickThought != pawn2.health.hediffSet.AnyHediffMakesSickThought)
					{
						return false;
					}
					if (pawn.health.hediffSet.AnyHediffMakesSickThought)
					{
						List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
						for (int i = 0; i < hediffs.Count; i++)
						{
							if (hediffs[i].def.makesSickThought && !pawn2.health.hediffSet.HasHediff(hediffs[i].def, false))
							{
								return false;
							}
						}
						List<Hediff> hediffs2 = pawn2.health.hediffSet.hediffs;
						for (int j = 0; j < hediffs2.Count; j++)
						{
							if (hediffs2[j].def.makesSickThought && !pawn.health.hediffSet.HasHediff(hediffs2[j].def, false))
							{
								return false;
							}
						}
					}
				}
				return true;
			}
			else
			{
				Apparel apparel = a as Apparel;
				Apparel apparel2 = b as Apparel;
				if (apparel != null && apparel2 != null && apparel.WornByCorpse != apparel2.WornByCorpse)
				{
					return false;
				}
				CompIngredients compIngredients = a.TryGetComp<CompIngredients>();
				CompIngredients compIngredients2 = b.TryGetComp<CompIngredients>();
				bool flag;
				if (compIngredients == null)
				{
					flag = false;
				}
				else
				{
					CompProperties_Ingredients props = compIngredients.Props;
					bool? flag2 = (props != null) ? new bool?(props.splitTransferableFoodKind) : null;
					bool flag3 = true;
					flag = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
				}
				if (flag && compIngredients2 != null)
				{
					CompProperties_Ingredients props2 = compIngredients2.Props;
					bool? flag2 = (props2 != null) ? new bool?(props2.splitTransferableFoodKind) : null;
					bool flag3 = true;
					if ((flag2.GetValueOrDefault() == flag3 & flag2 != null) && FoodUtility.GetFoodKind(a) != FoodUtility.GetFoodKind(b))
					{
						return false;
					}
				}
				if (mode != TransferAsOneMode.InactiveTradeable && a.def.useHitPoints && a.def.healthAffectsPrice && Mathf.Abs(a.HitPoints - b.HitPoints) >= 10)
				{
					return false;
				}
				QualityCategory qualityCategory;
				QualityCategory qualityCategory2;
				if (a.TryGetQuality(out qualityCategory) && b.TryGetQuality(out qualityCategory2) && qualityCategory != qualityCategory2)
				{
					return false;
				}
				if (a.def.category == ThingCategory.Item)
				{
					return a.CanStackWith(b);
				}
				if (a.def.category == ThingCategory.Building || a.def.category == ThingCategory.Plant)
				{
					return true;
				}
				string str = "Unknown TransferAsOne pair: ";
				Thing thing = a;
				string str2 = (thing != null) ? thing.ToString() : null;
				string str3 = ", ";
				Thing thing2 = b;
				Log.Error(str + str2 + str3 + ((thing2 != null) ? thing2.ToString() : null));
				return false;
			}
		}

		// Token: 0x06012283 RID: 74371 RVA: 0x00552354 File Offset: 0x00550554
		public static bool CanStack(Thing thing)
		{
			if (thing.def.category == ThingCategory.Pawn)
			{
				if (thing.def.race.Humanlike)
				{
					return false;
				}
				Pawn pawn = (Pawn)thing;
				if (pawn.health.summaryHealth.SummaryHealthPercent < 0.9999f)
				{
					return false;
				}
				if (pawn.Name != null && !pawn.Name.Numerical)
				{
					return false;
				}
				if (pawn.GetOverseer() != null)
				{
					return false;
				}
				Pawn_RelationsTracker relations = pawn.relations;
				if (((relations != null) ? relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond, null) : null) != null)
				{
					return false;
				}
				if (pawn.health.hediffSet.HasHediff(HediffDefOf.Pregnant, true))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06012284 RID: 74372 RVA: 0x00552400 File Offset: 0x00550600
		public static bool CanStackTogether(Thing thing, Thing otherThing)
		{
			Genepack genepack = thing as Genepack;
			if (genepack != null)
			{
				Genepack genepack2 = otherThing as Genepack;
				if (genepack2 != null)
				{
					return genepack.GeneSet.GenesListForReading.SetsEqual(genepack2.GeneSet.GenesListForReading) && genepack.GeneSet.Label == genepack2.GeneSet.Label;
				}
			}
			Xenogerm xenogerm = thing as Xenogerm;
			if (xenogerm != null)
			{
				Xenogerm xenogerm2 = otherThing as Xenogerm;
				if (xenogerm2 != null)
				{
					return xenogerm.GeneSet.GenesListForReading.SetsEqual(xenogerm2.GeneSet.GenesListForReading) && xenogerm.GeneSet.Label == xenogerm2.GeneSet.Label;
				}
			}
			return true;
		}

		// Token: 0x06012285 RID: 74373 RVA: 0x005524AC File Offset: 0x005506AC
		public static T TransferableMatching<T>(Thing thing, List<T> transferables, TransferAsOneMode mode) where T : Transferable
		{
			if (thing == null || transferables == null)
			{
				return default(T);
			}
			for (int i = 0; i < transferables.Count; i++)
			{
				T t = transferables[i];
				if (t.HasAnyThing && TransferableUtility.TransferAsOne(thing, t.AnyThing, mode))
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06012286 RID: 74374 RVA: 0x00552510 File Offset: 0x00550710
		public static Tradeable TradeableMatching(Thing thing, List<Tradeable> tradeables)
		{
			if (thing == null || tradeables == null)
			{
				return null;
			}
			for (int i = 0; i < tradeables.Count; i++)
			{
				Tradeable tradeable = tradeables[i];
				if (tradeable.HasAnyThing)
				{
					TransferAsOneMode mode = tradeable.TraderWillTrade ? TransferAsOneMode.Normal : TransferAsOneMode.InactiveTradeable;
					if (TransferableUtility.TransferAsOne(thing, tradeable.AnyThing, mode))
					{
						return tradeable;
					}
				}
			}
			return null;
		}

		// Token: 0x06012287 RID: 74375 RVA: 0x00552568 File Offset: 0x00550768
		public static TransferableOneWay TransferableMatchingDesperate(Thing thing, List<TransferableOneWay> transferables, TransferAsOneMode mode)
		{
			if (thing == null || transferables == null)
			{
				return null;
			}
			for (int i = 0; i < transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = transferables[i];
				if (transferableOneWay.HasAnyThing && transferableOneWay.things.Contains(thing))
				{
					return transferableOneWay;
				}
			}
			for (int j = 0; j < transferables.Count; j++)
			{
				TransferableOneWay transferableOneWay2 = transferables[j];
				if (transferableOneWay2.HasAnyThing && TransferableUtility.TransferAsOne(thing, transferableOneWay2.AnyThing, mode))
				{
					return transferableOneWay2;
				}
			}
			if (!(thing is Pawn))
			{
				for (int k = 0; k < transferables.Count; k++)
				{
					TransferableOneWay transferableOneWay3 = transferables[k];
					if (transferableOneWay3.HasAnyThing && transferableOneWay3.AnyThing.GetInnerIfMinified().def == thing.GetInnerIfMinified().def)
					{
						return transferableOneWay3;
					}
				}
			}
			return null;
		}

		// Token: 0x06012288 RID: 74376 RVA: 0x00552634 File Offset: 0x00550834
		public static List<Pawn> GetPawnsFromTransferables(List<TransferableOneWay> transferables)
		{
			List<Pawn> list = new List<Pawn>();
			for (int i = 0; i < transferables.Count; i++)
			{
				if (transferables[i].CountToTransfer > 0 && transferables[i].AnyThing is Pawn)
				{
					for (int j = 0; j < transferables[i].CountToTransfer; j++)
					{
						Pawn item = (Pawn)transferables[i].things[j];
						list.Add(item);
					}
				}
			}
			return list;
		}

		// Token: 0x06012289 RID: 74377 RVA: 0x005526B4 File Offset: 0x005508B4
		public static void SimulateTradeableTransfer(List<Thing> all, List<Tradeable> tradeables, List<ThingCount> outThingsAfterTransfer)
		{
			outThingsAfterTransfer.Clear();
			for (int i = 0; i < all.Count; i++)
			{
				outThingsAfterTransfer.Add(new ThingCount(all[i], all[i].stackCount, false));
			}
			Action<Thing, int> <>9__0;
			Action<Thing, int> <>9__1;
			for (int j = 0; j < tradeables.Count; j++)
			{
				int countToTransferToSource = tradeables[j].CountToTransferToSource;
				int countToTransferToDestination = tradeables[j].CountToTransferToDestination;
				if (countToTransferToSource > 0)
				{
					List<Thing> thingsTrader = tradeables[j].thingsTrader;
					int count = countToTransferToSource;
					Action<Thing, int> transfer;
					if ((transfer = <>9__0) == null)
					{
						transfer = (<>9__0 = delegate(Thing originalThing, int toTake)
						{
							outThingsAfterTransfer.Add(new ThingCount(originalThing, toTake, false));
						});
					}
					TransferableUtility.TransferNoSplit(thingsTrader, count, transfer, false, false);
				}
				else if (countToTransferToDestination > 0)
				{
					List<Thing> thingsColony = tradeables[j].thingsColony;
					int count2 = countToTransferToDestination;
					Action<Thing, int> transfer2;
					if ((transfer2 = <>9__1) == null)
					{
						transfer2 = (<>9__1 = delegate(Thing originalThing, int toTake)
						{
							for (int k = 0; k < outThingsAfterTransfer.Count; k++)
							{
								ThingCount thingCount = outThingsAfterTransfer[k];
								if (thingCount.Thing == originalThing)
								{
									outThingsAfterTransfer[k] = thingCount.WithCount(thingCount.Count - toTake);
									return;
								}
							}
						});
					}
					TransferableUtility.TransferNoSplit(thingsColony, count2, transfer2, false, false);
				}
			}
		}

		// Token: 0x0400BCBD RID: 48317
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
