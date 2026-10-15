using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;

namespace Verse;

public class RaceProperties
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass172_0
	{
		public ThingDef parentDef;

		public Pawn pawnThing;

		internal bool _003CSpecialDisplayStats_003Eb__0(TrainableDef td)
		{
			bool visible;
			return Pawn_TrainingTracker.CanAssignToTrain(td, parentDef, out visible, pawnThing);
		}
	}

	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__171 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private ThingDef thingDef;

		public ThingDef _003C_003E3__thingDef;

		public RaceProperties _003C_003E4__this;

		private int _003Ci_003E5__2;

		private int _003Cj_003E5__3;

		private IEnumerator<string> _003C_003E7__wrap3;

		private List<ThingDef>.Enumerator _003C_003E7__wrap4;

		string IEnumerator<string>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CConfigErrors_003Ed__171(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 5:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 15:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			case -5:
			case 17:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			}
			_003C_003E7__wrap3 = null;
			_003C_003E7__wrap4 = default(List<ThingDef>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				RaceProperties raceProperties = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (thingDef.IsCorpse)
					{
						return false;
					}
					if (raceProperties.predator && !raceProperties.Eats(FoodTypeFlags.Meat))
					{
						_003C_003E2__current = "predator but doesn't eat meat";
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_00af;
				case 1:
					_003C_003E1__state = -1;
					goto IL_00af;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00e0;
				case 3:
					_003C_003E1__state = -1;
					goto IL_0143;
				case 4:
					_003C_003E1__state = -1;
					goto IL_01e0;
				case 5:
					_003C_003E1__state = -3;
					goto IL_0238;
				case 6:
					_003C_003E1__state = -1;
					goto IL_0287;
				case 7:
					_003C_003E1__state = -1;
					goto IL_02b7;
				case 8:
					_003C_003E1__state = -1;
					goto IL_0304;
				case 9:
					_003C_003E1__state = -1;
					goto IL_0376;
				case 10:
					_003C_003E1__state = -1;
					goto IL_03c4;
				case 11:
					_003C_003E1__state = -1;
					goto IL_0437;
				case 12:
					_003C_003E1__state = -1;
					goto IL_0468;
				case 13:
					_003C_003E1__state = -1;
					goto IL_04a3;
				case 14:
					_003C_003E1__state = -1;
					goto IL_04ee;
				case 15:
					_003C_003E1__state = -4;
					goto IL_053a;
				case 16:
					_003C_003E1__state = -1;
					goto IL_057d;
				case 17:
					{
						_003C_003E1__state = -5;
						goto IL_05f7;
					}
					IL_04a3:
					if (raceProperties.IsMechanoid && thingDef.butcherProducts.NullOrEmpty())
					{
						_003C_003E2__current = thingDef.label + " mech has no butcher products set";
						_003C_003E1__state = 14;
						return true;
					}
					goto IL_04ee;
					IL_0287:
					if (raceProperties.specificMeatDef != null && raceProperties.useMeatFrom != null)
					{
						_003C_003E2__current = "specificMeatDef and useMeatFrom are both non-null. specificMeatDef will be chosen.";
						_003C_003E1__state = 7;
						return true;
					}
					goto IL_02b7;
					IL_0468:
					if (raceProperties.fleshType == FleshTypeDefOf.Normal && raceProperties.gestationPeriodDays < 0f)
					{
						_003C_003E2__current = "normal flesh but gestationPeriodDays not configured.";
						_003C_003E1__state = 13;
						return true;
					}
					goto IL_04a3;
					IL_00af:
					if (raceProperties.canFishForFood && !raceProperties.Eats(FoodTypeFlags.Meat))
					{
						_003C_003E2__current = "canFishForFood but doesn't eat meat";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00e0;
					IL_02b7:
					if (raceProperties.useMeatFrom != null && raceProperties.useMeatFrom.category != ThingCategory.Pawn)
					{
						_003C_003E2__current = "tries to use meat from non-pawn " + raceProperties.useMeatFrom;
						_003C_003E1__state = 8;
						return true;
					}
					goto IL_0304;
					IL_04ee:
					_003C_003E7__wrap3 = raceProperties.deathAction.ConfigErrors().GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_053a;
					IL_00e0:
					_003Ci_003E5__2 = 0;
					goto IL_0171;
					IL_0171:
					if (_003Ci_003E5__2 < raceProperties.lifeStageAges.Count)
					{
						_003Cj_003E5__3 = 0;
						goto IL_0153;
					}
					if (thingDef.IsCaravanRideable() && !raceProperties.lifeStageAges.Any((LifeStageAge s) => s.def.caravanRideable))
					{
						_003C_003E2__current = "must contain at least one lifeStage with caravanRideable when CaravanRidingSpeedFactor is defined";
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_01e0;
					IL_05f7:
					while (_003C_003E7__wrap4.MoveNext())
					{
						ThingDef current = _003C_003E7__wrap4.Current;
						if (current.race == null || !current.race.Animal)
						{
							_003C_003E2__current = "tries to set canCrossBreedWith " + current.defName + " but that is not an animal";
							_003C_003E1__state = 17;
							return true;
						}
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap4 = default(List<ThingDef>.Enumerator);
					break;
					IL_0143:
					_003Cj_003E5__3++;
					goto IL_0153;
					IL_0304:
					if (raceProperties.useMeatFrom?.race.useMeatFrom != null)
					{
						_003C_003E2__current = "tries to use meat from " + raceProperties.useMeatFrom?.ToString() + " which uses meat from " + raceProperties.useMeatFrom.race.useMeatFrom;
						_003C_003E1__state = 9;
						return true;
					}
					goto IL_0376;
					IL_0376:
					if (raceProperties.useLeatherFrom != null && raceProperties.useLeatherFrom.category != ThingCategory.Pawn)
					{
						_003C_003E2__current = "tries to use leather from non-pawn " + raceProperties.useLeatherFrom;
						_003C_003E1__state = 10;
						return true;
					}
					goto IL_03c4;
					IL_0153:
					if (_003Cj_003E5__3 < _003Ci_003E5__2)
					{
						if (raceProperties.lifeStageAges[_003Cj_003E5__3].minAge > raceProperties.lifeStageAges[_003Ci_003E5__2].minAge)
						{
							_003C_003E2__current = "lifeStages minAges are not in ascending order";
							_003C_003E1__state = 3;
							return true;
						}
						goto IL_0143;
					}
					_003Ci_003E5__2++;
					goto IL_0171;
					IL_01e0:
					if (raceProperties.litterSizeCurve != null)
					{
						_003C_003E7__wrap3 = raceProperties.litterSizeCurve.ConfigErrors("litterSizeCurve").GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_0238;
					}
					goto IL_0252;
					IL_0437:
					if (raceProperties.Animal && raceProperties.trainability == null)
					{
						_003C_003E2__current = "animal has trainability = null";
						_003C_003E1__state = 12;
						return true;
					}
					goto IL_0468;
					IL_0238:
					if (_003C_003E7__wrap3.MoveNext())
					{
						string current2 = _003C_003E7__wrap3.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 5;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap3 = null;
					goto IL_0252;
					IL_053a:
					if (_003C_003E7__wrap3.MoveNext())
					{
						string current3 = _003C_003E7__wrap3.Current;
						_003C_003E2__current = current3;
						_003C_003E1__state = 15;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap3 = null;
					if (raceProperties.renderTree == null)
					{
						_003C_003E2__current = "renderTree is null";
						_003C_003E1__state = 16;
						return true;
					}
					goto IL_057d;
					IL_03c4:
					if (raceProperties.useLeatherFrom != null && raceProperties.useLeatherFrom.race.useLeatherFrom != null)
					{
						_003C_003E2__current = "tries to use leather from " + raceProperties.useLeatherFrom?.ToString() + " which uses leather from " + raceProperties.useLeatherFrom.race.useLeatherFrom;
						_003C_003E1__state = 11;
						return true;
					}
					goto IL_0437;
					IL_0252:
					if (raceProperties.nameOnTameChance > 0f && raceProperties.nameGenerator == null)
					{
						_003C_003E2__current = "can be named, but has no nameGenerator";
						_003C_003E1__state = 6;
						return true;
					}
					goto IL_0287;
					IL_057d:
					if (raceProperties.canCrossBreedWith == null)
					{
						break;
					}
					_003C_003E7__wrap4 = raceProperties.canCrossBreedWith.GetEnumerator();
					_003C_003E1__state = -5;
					goto IL_05f7;
				}
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap3 != null)
			{
				_003C_003E7__wrap3.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap3 != null)
			{
				_003C_003E7__wrap3.Dispose();
			}
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap4/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__171 _003CConfigErrors_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CConfigErrors_003Ed__ = this;
			}
			else
			{
				_003CConfigErrors_003Ed__ = new _003CConfigErrors_003Ed__171(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CConfigErrors_003Ed__.thingDef = _003C_003E3__thingDef;
			return _003CConfigErrors_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__172 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private ThingDef parentDef;

		public ThingDef _003C_003E3__parentDef;

		private StatRequest req;

		public StatRequest _003C_003E3__req;

		public RaceProperties _003C_003E4__this;

		private _003C_003Ec__DisplayClass172_0 _003C_003E8__1;

		private StringBuilder _003Csb_003E5__2;

		private IEnumerator<StatDrawEntry> _003C_003E7__wrap2;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CSpecialDisplayStats_003Ed__172(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 16:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 19:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E8__1 = null;
			_003Csb_003E5__2 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				RaceProperties raceProperties = _003C_003E4__this;
				TrainabilityDef trainabilityDef;
				float f2;
				ThingDef thingDef;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass172_0();
					_003C_003E8__1.parentDef = parentDef;
					_003C_003E8__1.pawnThing = req.Pawn ?? (req.Thing as Pawn);
					if (!ModsConfig.BiotechActive || !raceProperties.Humanlike || _003C_003E8__1.pawnThing?.genes == null || _003C_003E8__1.pawnThing.genes.Xenotype == XenotypeDefOf.Baseliner)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Race".Translate(), _003C_003E8__1.parentDef.LabelCap, _003C_003E8__1.parentDef.description, 4205, null, null, forceUnfinalizedMode: false, overridesHideStats: true);
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_015e;
				case 1:
					_003C_003E1__state = -1;
					goto IL_015e;
				case 2:
					_003C_003E1__state = -1;
					goto IL_02d1;
				case 3:
					_003C_003E1__state = -1;
					goto IL_02d1;
				case 4:
					_003C_003E1__state = -1;
					goto IL_035c;
				case 5:
					_003C_003E1__state = -1;
					goto IL_0408;
				case 6:
				{
					_003C_003E1__state = -1;
					float f = ((_003C_003E8__1.pawnThing == null) ? PawnUtility.GetManhunterOnTameFailChance(_003C_003E8__1.parentDef) : PawnUtility.GetManhunterOnTameFailChance(_003C_003E8__1.pawnThing));
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Animals, "TameFailedRevengeChance".Translate(), f.ToStringPercent(), PawnUtility.GetManhunterOnTameFailChanceExplanation(_003C_003E8__1.parentDef, _003C_003E8__1.pawnThing), 511);
					_003C_003E1__state = 7;
					return true;
				}
				case 7:
					_003C_003E1__state = -1;
					goto IL_0581;
				case 8:
				{
					_003C_003E1__state = -1;
					_003Csb_003E5__2.Clear();
					_003Csb_003E5__2.AppendLine("StatsReport_AvailableTraining_Desc".Translate());
					bool visible;
					IOrderedEnumerable<TrainableDef> orderedEnumerable = from td in DefDatabase<TrainableDef>.AllDefsListForReading
						where Pawn_TrainingTracker.CanAssignToTrain(td, _003C_003E8__1.parentDef, out visible, _003C_003E8__1.pawnThing)
						orderby td.listPriority descending
						select td;
					foreach (TrainableDef item in orderedEnumerable)
					{
						_003Csb_003E5__2.AppendLine();
						_003Csb_003E5__2.AppendLine(item.LabelCap.Colorize(ColorLibrary.Yellow).ResolveTags());
						_003Csb_003E5__2.AppendLine(item.description);
					}
					if (_003Csb_003E5__2.Length != 0)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Animals, "StatsReport_AvailableTraining".Translate(), orderedEnumerable.Select((TrainableDef td) => td.label).ToCommaList().CapitalizeFirst(), _003Csb_003E5__2.ToString(), 2499);
						_003C_003E1__state = 9;
						return true;
					}
					goto IL_080e;
				}
				case 9:
					_003C_003E1__state = -1;
					goto IL_080e;
				case 10:
					_003C_003E1__state = -1;
					goto IL_0886;
				case 11:
					_003C_003E1__state = -1;
					goto IL_0944;
				case 12:
					_003C_003E1__state = -1;
					if (_003C_003E8__1.pawnThing != null && _003C_003E8__1.pawnThing.gender != 0)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Sex".Translate(), _003C_003E8__1.pawnThing.gender.GetLabel(animal: true).CapitalizeFirst(), _003C_003E8__1.pawnThing.gender.GetLabel(animal: true).CapitalizeFirst(), 4204);
						_003C_003E1__state = 13;
						return true;
					}
					goto IL_0a68;
				case 13:
					_003C_003E1__state = -1;
					goto IL_0a68;
				case 14:
					_003C_003E1__state = -1;
					goto IL_0b25;
				case 15:
					_003C_003E1__state = -1;
					goto IL_0c0f;
				case 16:
					_003C_003E1__state = -3;
					goto IL_0c60;
				case 17:
					_003C_003E1__state = -1;
					goto IL_0cf4;
				case 18:
					_003C_003E1__state = -1;
					goto IL_0d77;
				case 19:
					{
						_003C_003E1__state = -4;
						goto IL_0dcb;
					}
					IL_015e:
					if (_003C_003E8__1.pawnThing != null && _003C_003E8__1.pawnThing.IsMutant && _003C_003E8__1.pawnThing.mutant.Def.overrideFoodType)
					{
						string text = _003C_003E8__1.pawnThing.mutant.Def.foodType.ToHumanString().CapitalizeFirst();
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.PawnFood, "Diet".Translate(), text, "Stat_Race_Diet_Desc".Translate(text), 1500);
						_003C_003E1__state = 2;
						return true;
					}
					if (!_003C_003E8__1.parentDef.race.IsMechanoid && !_003C_003E8__1.parentDef.race.IsAnomalyEntity && !_003C_003E8__1.parentDef.race.IsDrone)
					{
						string text2 = raceProperties.foodType.ToHumanString().CapitalizeFirst();
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.PawnFood, "Diet".Translate(), text2, "Stat_Race_Diet_Desc".Translate(text2), 1500);
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_02d1;
					IL_080e:
					_003Csb_003E5__2 = null;
					goto IL_0815;
					IL_0a68:
					if (_003C_003E8__1.parentDef.race.nuzzleMtbHours > 0f)
					{
						float num2 = ((_003C_003E8__1.pawnThing != null) ? NuzzleUtility.GetNuzzleMTBHours(_003C_003E8__1.pawnThing) : _003C_003E8__1.parentDef.race.nuzzleMtbHours);
						StatDrawEntry statDrawEntry = new StatDrawEntry(StatCategoryDefOf.Animals, "NuzzleInterval".Translate(), Mathf.RoundToInt(num2 * 2500f).ToStringTicksToPeriod(), "NuzzleIntervalExplanation".Translate(), 500);
						_003C_003E2__current = statDrawEntry;
						_003C_003E1__state = 14;
						return true;
					}
					goto IL_0b25;
					IL_0581:
					trainabilityDef = TrainableUtility.GetTrainability(_003C_003E8__1.pawnThing) ?? raceProperties.trainability;
					if ((int)raceProperties.intelligence < 2 && trainabilityDef != null && !_003C_003E8__1.parentDef.race.IsAnomalyEntity && !_003C_003E8__1.parentDef.race.IsDrone)
					{
						_003Csb_003E5__2 = new StringBuilder();
						_003Csb_003E5__2.AppendLine("Stat_Race_Trainability_Desc".Translate());
						if (ModsConfig.OdysseyActive)
						{
							Pawn pawn = _003C_003E8__1.pawnThing;
							if (pawn != null && pawn.health.hediffSet.HasHediff(HediffDefOf.SentienceCatalyst))
							{
								_003Csb_003E5__2.AppendLine();
								_003Csb_003E5__2.AppendLine("StatsReport_SentienceCatalystInstalled".Translate());
							}
						}
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Animals, "Trainability".Translate(), trainabilityDef.LabelCap, _003Csb_003E5__2.ToString(), 2500);
						_003C_003E1__state = 8;
						return true;
					}
					goto IL_0815;
					IL_035c:
					if (_003C_003E8__1.parentDef.race.leatherDef != null)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "LeatherType".Translate(), _003C_003E8__1.parentDef.race.leatherDef.LabelCap, "Stat_Race_LeatherType_Desc".Translate(), 3550, null, new Dialog_InfoCard.Hyperlink[1]
						{
							new Dialog_InfoCard.Hyperlink(_003C_003E8__1.parentDef.race.leatherDef)
						});
						_003C_003E1__state = 5;
						return true;
					}
					goto IL_0408;
					IL_0b25:
					if (_003C_003E8__1.parentDef.race.roamMtbDays.HasValue)
					{
						Pawn pawn2 = _003C_003E8__1.pawnThing;
						if (pawn2 == null || pawn2.Roamer)
						{
							_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Animals, "StatsReport_RoamInterval".Translate(), Mathf.RoundToInt((_003C_003E8__1.pawnThing?.RoamMtbDays ?? _003C_003E8__1.parentDef.race.roamMtbDays).Value * 60000f).ToStringTicksToPeriod(), "Stat_Race_RoamInterval_Desc".Translate(), 2030);
							_003C_003E1__state = 15;
							return true;
						}
					}
					goto IL_0c0f;
					IL_0408:
					if (!_003C_003E8__1.parentDef.race.Animal)
					{
						Pawn pawn3 = _003C_003E8__1.pawnThing;
						if (pawn3 == null || !pawn3.IsWildMan() || !(_003C_003E8__1.parentDef.GetStatValueAbstract(StatDefOf.Wildness) > 0f))
						{
							goto IL_0581;
						}
					}
					f2 = ((_003C_003E8__1.pawnThing == null) ? PawnUtility.GetManhunterOnDamageChance(_003C_003E8__1.parentDef) : PawnUtility.GetManhunterOnDamageChance(_003C_003E8__1.pawnThing));
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Animals, "HarmedRevengeChance".Translate(), f2.ToStringPercent(), PawnUtility.GetManhunterOnDamageChanceExplanation(_003C_003E8__1.parentDef, _003C_003E8__1.pawnThing), 510);
					_003C_003E1__state = 6;
					return true;
					IL_0c7a:
					if (!ModsConfig.BiotechActive || !raceProperties.IsMechanoid)
					{
						break;
					}
					if (raceProperties.mechWeightClass != null)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Mechanoid, "MechWeightClass".Translate(), raceProperties.mechWeightClass.LabelCap, "MechWeightClassExplanation".Translate(), 500);
						_003C_003E1__state = 17;
						return true;
					}
					goto IL_0cf4;
					IL_0c0f:
					_003C_003E7__wrap2 = AnimalProductionUtility.AnimalProductionStats(_003C_003E8__1.parentDef).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0c60;
					IL_0c60:
					if (_003C_003E7__wrap2.MoveNext())
					{
						StatDrawEntry current = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 16;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = null;
					goto IL_0c7a;
					IL_02d1:
					if (_003C_003E8__1.pawnThing != null && _003C_003E8__1.pawnThing.needs?.food != null)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.PawnFood, "FoodConsumption".Translate(), NutritionEatenPerDay(_003C_003E8__1.pawnThing), NutritionEatenPerDayExplanation(_003C_003E8__1.pawnThing), 1600);
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_035c;
					IL_0815:
					if (!_003C_003E8__1.parentDef.race.IsDrone)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.PawnHealth, "StatsReport_LifeExpectancy".Translate(), raceProperties.lifeExpectancy.ToStringByStyle(ToStringStyle.Integer), "Stat_Race_LifeExpectancy_Desc".Translate(), 4300);
						_003C_003E1__state = 10;
						return true;
					}
					goto IL_0886;
					IL_0886:
					if (_003C_003E8__1.parentDef.race.Animal || (_003C_003E8__1.pawnThing?.FenceBlocked ?? raceProperties.FenceBlocked))
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Animals, "StatsReport_BlockedByFences".Translate(), (_003C_003E8__1.pawnThing?.FenceBlocked ?? raceProperties.FenceBlocked) ? "Yes".Translate() : "No".Translate(), "Stat_Race_BlockedByFences_Desc".Translate(), 2040);
						_003C_003E1__state = 11;
						return true;
					}
					goto IL_0944;
					IL_0cf4:
					thingDef = MechanitorUtility.RechargerForMech(_003C_003E8__1.parentDef);
					if (thingDef != null)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Mechanoid, "StatsReport_RechargerNeeded".Translate(), thingDef.LabelCap, "StatsReport_RechargerNeeded_Desc".Translate(), 503, null, new Dialog_InfoCard.Hyperlink[1]
						{
							new Dialog_InfoCard.Hyperlink(thingDef)
						});
						_003C_003E1__state = 18;
						return true;
					}
					goto IL_0d77;
					IL_0d77:
					_003C_003E7__wrap2 = MechWorkUtility.SpecialDisplayStats(_003C_003E8__1.parentDef, req).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0dcb;
					IL_0944:
					if (_003C_003E8__1.parentDef.race.Animal)
					{
						StatDrawEntry statDrawEntry2 = new StatDrawEntry(StatCategoryDefOf.Animals, "PackAnimal".Translate(), raceProperties.packAnimal ? "Yes".Translate() : "No".Translate(), "PackAnimalExplanation".Translate(), 2202);
						_003C_003E2__current = statDrawEntry2;
						_003C_003E1__state = 12;
						return true;
					}
					goto IL_0c7a;
					IL_0dcb:
					if (_003C_003E7__wrap2.MoveNext())
					{
						StatDrawEntry current3 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current3;
						_003C_003E1__state = 19;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = null;
					break;
				}
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__172 _003CSpecialDisplayStats_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CSpecialDisplayStats_003Ed__ = this;
			}
			else
			{
				_003CSpecialDisplayStats_003Ed__ = new _003CSpecialDisplayStats_003Ed__172(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CSpecialDisplayStats_003Ed__.parentDef = _003C_003E3__parentDef;
			_003CSpecialDisplayStats_003Ed__.req = _003C_003E3__req;
			return _003CSpecialDisplayStats_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	public Intelligence intelligence;

	private FleshTypeDef fleshType;

	private ThingDef bloodDef;

	private ThingDef bloodSmearDef;

	public bool hasGenders = true;

	public Gender forceGender;

	public bool needsRest = true;

	public ThinkTreeDef thinkTreeMain;

	public ThinkTreeDef thinkTreeConstant;

	public DutyDef dutyBoss;

	public PawnNameCategory nameCategory;

	public FoodTypeFlags foodType;

	public BodyDef body;

	public DeathActionProperties deathAction = new DeathActionProperties();

	public List<AnimalBiomeRecord> wildBiomes;

	public SimpleCurve ageGenerationCurve;

	public bool makesFootprints;

	public int executionRange = 2;

	public float lifeExpectancy = 10f;

	public List<HediffGiverSetDef> hediffGiverSets;

	public float? roamMtbDays;

	public bool allowedOnCaravan = true;

	public bool canReleaseToWild = true;

	public bool playerCanChangeMaster = true;

	public bool showTrainables = true;

	public bool hideTrainingTab;

	public bool doesntMove;

	public PawnRenderTreeDef renderTree;

	public AnimationDef startingAnimation;

	public ThingDef linkedCorpseKind;

	public bool canOpenFactionlessDoors = true;

	public bool alwaysAwake;

	public bool alwaysViolent;

	public bool isImmuneToInfections;

	public float bleedRateFactor = 1f;

	public bool canBecomeShambler;

	public bool neverIncludeInQuests;

	public bool canBeVacuumBurnt = true;

	public bool herdAnimal;

	public bool packAnimal;

	public bool predator;

	public float maxPreyBodySize = 99999f;

	public float petness;

	public float nuzzleMtbHours = -1f;

	public float manhunterOnDamageChance;

	public float manhunterOnTameFailChance;

	public bool canBePredatorPrey = true;

	public bool herdMigrationAllowed = true;

	public AnimalType animalType;

	public List<ThingDef> willNeverEat;

	public bool giveNonToolUserBeatFireVerb;

	public bool disableIgniteVerb;

	public bool disableAreaControl;

	public bool waterSeeker;

	public int? waterCellCost;

	public bool disableMating;

	public List<ThingDef> canCrossBreedWith;

	public List<ThingDef> crossAggroWith;

	public bool canFishForFood;

	public bool canFlyInVacuum;

	public PawnKindDef manhunterPackUseLabelFrom;

	public float flightStartChanceOnJobStart;

	public float flightSpeedFactor = 2.8f;

	public bool canFlyIntoMap;

	public bool canLeaveMapFlying;

	public float leaveMapOnFleeChance;

	public int maxMechEnergy = 100;

	public List<WorkTypeDef> mechEnabledWorkTypes = new List<WorkTypeDef>();

	public int mechFixedSkillLevel = 10;

	public List<MechWorkTypePriority> mechWorkTypePriorities;

	public int? bulletStaggerDelayTicks;

	public float? bulletStaggerSpeedFactor;

	public EffecterDef bulletStaggerEffecterDef;

	public bool bulletStaggerIgnoreBodySize;

	public MechWeightClassDef mechWeightClass;

	public List<DetritusLeavingType> detritusLeavings = new List<DetritusLeavingType>();

	public float gestationPeriodDays = -1f;

	public SimpleCurve litterSizeCurve;

	public float mateMtbHours = 12f;

	[NoTranslate]
	public List<string> untrainableTags;

	[NoTranslate]
	public List<string> trainableTags;

	public TrainabilityDef trainability;

	public List<TrainableDef> specialTrainables;

	private RulePackDef nameGenerator;

	private RulePackDef nameGeneratorFemale;

	public float nameOnTameChance;

	public float baseBodySize = 1f;

	public float baseHealthScale = 1f;

	public float baseHungerRate = 1f;

	public List<LifeStageAge> lifeStageAges = new List<LifeStageAge>();

	public List<LifeStageWorkSettings> lifeStageWorkSettings = new List<LifeStageWorkSettings>();

	public bool hasMeat = true;

	[MustTranslate]
	public string meatLabel;

	public Color meatColor = Color.white;

	public float meatMarketValue = 2f;

	public ThingDef specificMeatDef;

	public ThingDef useMeatFrom;

	public ThingDef useLeatherFrom;

	public bool hasCorpse = true;

	public bool hasUnnaturalCorpse;

	public bool corpseHiddenWhileUndiscovered;

	public ThingDef leatherDef;

	public ShadowData specialShadowData;

	public List<Vector3> headPosPerRotation;

	public IntRange soundCallIntervalRange = new IntRange(2000, 4000);

	public float soundCallIntervalFriendlyFactor = 1f;

	public float soundCallIntervalAggressiveFactor = 0.25f;

	public SoundDef soundMeleeHitPawn;

	public SoundDef soundMeleeHitBuilding;

	public SoundDef soundMeleeMiss;

	public SoundDef soundMeleeDodge;

	public SoundDef soundAmbience;

	public SoundDef soundMoving;

	public SoundDef soundEating;

	public KnowledgeCategoryDef knowledgeCategory;

	public int anomalyKnowledge;

	[Unsaved(false)]
	public ThingDef meatDef;

	[Unsaved(false)]
	public ThingDef corpseDef;

	[Unsaved(false)]
	public ThingDef unnaturalCorpseDef;

	[Unsaved(false)]
	private PawnKindDef cachedAnyPawnKind;

	public bool Humanlike => (int)intelligence >= 2;

	public bool ToolUser => (int)intelligence >= 1;

	public bool Animal
	{
		get
		{
			if (!ToolUser && IsFlesh)
			{
				return !IsAnomalyEntity;
			}
			return false;
		}
	}

	public bool Insect => FleshType == FleshTypeDefOf.Insectoid;

	public bool Dryad => animalType == AnimalType.Dryad;

	public bool ShouldHaveAbilityTracker
	{
		get
		{
			if (!Humanlike)
			{
				return IsMechanoid;
			}
			return true;
		}
	}

	public bool EatsFood => foodType != FoodTypeFlags.None;

	public float FoodLevelPercentageWantEat => ResolvedDietCategory switch
	{
		DietCategory.NeverEats => 0.3f, 
		DietCategory.Omnivorous => 0.3f, 
		DietCategory.Carnivorous => 0.3f, 
		DietCategory.Ovivorous => 0.4f, 
		DietCategory.Herbivorous => 0.45f, 
		DietCategory.Dendrovorous => 0.45f, 
		_ => throw new InvalidOperationException(), 
	};

	public DietCategory ResolvedDietCategory
	{
		get
		{
			if (!EatsFood)
			{
				return DietCategory.NeverEats;
			}
			if (Eats(FoodTypeFlags.Tree))
			{
				return DietCategory.Dendrovorous;
			}
			if (Eats(FoodTypeFlags.Meat))
			{
				if (Eats(FoodTypeFlags.VegetableOrFruit) || Eats(FoodTypeFlags.Plant))
				{
					return DietCategory.Omnivorous;
				}
				return DietCategory.Carnivorous;
			}
			if (Eats(FoodTypeFlags.AnimalProduct))
			{
				return DietCategory.Ovivorous;
			}
			return DietCategory.Herbivorous;
		}
	}

	public DeathActionWorker DeathActionWorker => deathAction.Worker;

	public FleshTypeDef FleshType => fleshType ?? FleshTypeDefOf.Normal;

	public bool IsMechanoid => FleshType == FleshTypeDefOf.Mechanoid;

	public bool IsFlesh => FleshType.isOrganic;

	public bool IsAnomalyEntity
	{
		get
		{
			if (ModsConfig.AnomalyActive)
			{
				if (FleshType != FleshTypeDefOf.EntityMechanical && FleshType != FleshTypeDefOf.EntityFlesh)
				{
					return FleshType == FleshTypeDefOf.Fleshbeast;
				}
				return true;
			}
			return false;
		}
	}

	public bool IsDrone
	{
		get
		{
			if (ModsConfig.OdysseyActive)
			{
				return FleshType == FleshTypeDefOf.Drone;
			}
			return false;
		}
	}

	public ThingDef BloodDef => bloodDef;

	public ThingDef BloodSmearDef => bloodSmearDef;

	public bool CanDoHerdMigration
	{
		get
		{
			if (Animal)
			{
				return herdMigrationAllowed;
			}
			return false;
		}
	}

	public bool CanPassFences => !FenceBlocked;

	public bool FenceBlocked => Roamer;

	public bool Roamer => roamMtbDays.HasValue;

	public bool IsWorkMech => !mechEnabledWorkTypes.NullOrEmpty();

	public PawnKindDef AnyPawnKind
	{
		get
		{
			if (cachedAnyPawnKind == null)
			{
				List<PawnKindDef> allDefsListForReading = DefDatabase<PawnKindDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					if (allDefsListForReading[i].race.race == this)
					{
						cachedAnyPawnKind = allDefsListForReading[i];
						break;
					}
				}
			}
			return cachedAnyPawnKind;
		}
	}

	public RulePackDef GetNameGenerator(Gender gender)
	{
		if (gender == Gender.Female && nameGeneratorFemale != null)
		{
			return nameGeneratorFemale;
		}
		return nameGenerator;
	}

	public bool CanEverEat(Thing t)
	{
		return CanEverEat(t.def);
	}

	public bool CanEverEat(ThingDef t)
	{
		if (!EatsFood)
		{
			return false;
		}
		if (t.ingestible == null)
		{
			return false;
		}
		if (t.ingestible.preferability == FoodPreferability.Undefined)
		{
			return false;
		}
		if (willNeverEat != null && willNeverEat.Contains(t))
		{
			return false;
		}
		return Eats(t.ingestible.foodType);
	}

	public bool Eats(FoodTypeFlags food)
	{
		if (!EatsFood)
		{
			return false;
		}
		return (foodType & food) != 0;
	}

	public void ResolveReferencesSpecial()
	{
		if (specificMeatDef != null)
		{
			meatDef = specificMeatDef;
		}
		else if (useMeatFrom != null)
		{
			meatDef = useMeatFrom.race.meatDef;
		}
		if (useLeatherFrom != null)
		{
			leatherDef = useLeatherFrom.race.leatherDef;
		}
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__171))]
	public IEnumerable<string> ConfigErrors(ThingDef thingDef)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__171(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__thingDef = thingDef
		};
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__172))]
	public IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef, StatRequest req)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__172(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__parentDef = parentDef,
			_003C_003E3__req = req
		};
	}

	public static string NutritionEatenPerDay(Pawn p)
	{
		return (p.needs.food.FoodFallPerTickAssumingCategory(HungerCategory.Fed) * 60000f).ToString("0.##");
	}

	public static string NutritionEatenPerDayExplanation(Pawn p, bool showDiet = false, bool showLegend = false, bool showCalculations = true)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("NutritionEatenPerDayTip".Translate(ThingDefOf.MealSimple.GetStatValueAbstract(StatDefOf.Nutrition).ToString("0.##")));
		stringBuilder.AppendLine();
		if (showDiet)
		{
			stringBuilder.AppendLine("CanEat".Translate() + ": " + p.RaceProps.foodType.ToHumanString());
			stringBuilder.AppendLine();
		}
		if (showLegend)
		{
			stringBuilder.AppendLine("Legend".Translate() + ":");
			stringBuilder.AppendLine("NoDietCategoryLetter".Translate() + " - " + DietCategory.Omnivorous.ToStringHuman());
			DietCategory[] array = (DietCategory[])Enum.GetValues(typeof(DietCategory));
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != 0 && array[i] != DietCategory.Omnivorous)
				{
					stringBuilder.AppendLine(array[i].ToStringHumanShort() + " - " + array[i].ToStringHuman());
				}
			}
			stringBuilder.AppendLine();
		}
		if (showCalculations)
		{
			stringBuilder.AppendLine("StatsReport_BaseValue".Translate() + ": " + (p.ageTracker.CurLifeStage.hungerRateFactor * p.RaceProps.baseHungerRate * 2.6666667E-05f * 60000f).ToStringByStyle(ToStringStyle.FloatTwo));
			if (p.health.hediffSet.HungerRateFactor != 1f)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("StatsReport_RelevantHediffs".Translate() + ": " + p.health.hediffSet.HungerRateFactor.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor));
				foreach (Hediff hediff in p.health.hediffSet.hediffs)
				{
					if (hediff.CurStage != null && hediff.CurStage.hungerRateFactor != 1f)
					{
						stringBuilder.AppendLine("    " + hediff.LabelCap + ": " + hediff.CurStage.hungerRateFactor.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor));
					}
				}
				foreach (Hediff hediff2 in p.health.hediffSet.hediffs)
				{
					if (hediff2.CurStage != null && hediff2.CurStage.hungerRateFactorOffset != 0f)
					{
						stringBuilder.AppendLine("    " + hediff2.LabelCap + ": " + hediff2.CurStage.hungerRateFactorOffset.ToStringByStyle(ToStringStyle.FloatMaxOne, ToStringNumberSense.Offset));
					}
				}
			}
			if (p.story?.traits != null && p.story.traits.HungerRateFactor != 1f)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("StatsReport_RelevantTraits".Translate() + ": " + p.story.traits.HungerRateFactor.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor));
				foreach (Trait allTrait in p.story.traits.allTraits)
				{
					if (!allTrait.Suppressed && allTrait.CurrentData.hungerRateFactor != 1f)
					{
						stringBuilder.AppendLine("    " + allTrait.LabelCap + ": " + allTrait.CurrentData.hungerRateFactor.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor));
					}
				}
			}
			Building_Bed building_Bed = p.CurrentBed() ?? p.CurrentCaravanBed();
			if (building_Bed != null)
			{
				float statValue = building_Bed.GetStatValue(StatDefOf.BedHungerRateFactor);
				if (statValue != 1f)
				{
					stringBuilder.AppendLine().AppendLine("StatsReport_InBed".Translate() + ": x" + statValue.ToStringPercent());
				}
			}
			if (ModsConfig.BiotechActive)
			{
				Hediff firstHediffOfDef = p.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Lactating);
				if (firstHediffOfDef != null)
				{
					HediffComp_Lactating hediffComp_Lactating = firstHediffOfDef.TryGetComp<HediffComp_Lactating>();
					if (hediffComp_Lactating != null)
					{
						float f = hediffComp_Lactating.AddedNutritionPerDay();
						stringBuilder.AppendLine();
						stringBuilder.AppendLine(firstHediffOfDef.LabelBaseCap + ": " + f.ToStringWithSign());
					}
				}
			}
			if (p.genes != null)
			{
				int num = 0;
				foreach (Gene item in p.genes.GenesListForReading)
				{
					if (!item.Overridden)
					{
						num += item.def.biostatMet;
					}
				}
				float num2 = GeneTuning.MetabolismToFoodConsumptionFactorCurve.Evaluate(num);
				if (num2 != 1f)
				{
					stringBuilder.AppendLine().AppendLine("FactorForMetabolism".Translate() + ": x" + num2.ToStringPercent());
				}
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("StatsReport_FinalValue".Translate() + ": " + NutritionEatenPerDay(p));
		}
		return stringBuilder.ToString().TrimEndNewlines();
	}
}
