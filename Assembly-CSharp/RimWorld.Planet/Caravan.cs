using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet;

[StaticConstructorOnStartup]
public class Caravan : WorldObject, IThingHolder, IIncidentTarget, ILoadReferenceable, ITrader
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass123_0
	{
		public IEnumerable<Thing> allDissolvingThings;

		internal void _003CGetGizmos_003Eb__18()
		{
			allDissolvingThings.RandomElement().TryGetComp<CompDissolution>().TriggerDissolutionEvent();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__123 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Caravan _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

		private List<Pawn>.Enumerator _003C_003E7__wrap2;

		private Pawn _003Cp_003E5__4;

		private List<FactionPermit>.Enumerator _003C_003E7__wrap4;

		private IEnumerator<WorldObject> _003C_003E7__wrap5;

		private List<Ability>.Enumerator _003C_003E7__wrap6;

		private IEnumerator<Command> _003C_003E7__wrap7;

		private List<Gene>.Enumerator _003C_003E7__wrap8;

		Gizmo IEnumerator<Gizmo>.Current
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
		public _003CGetGizmos_003Ed__123(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			switch (num)
			{
			case -3:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -6:
			case -5:
			case -4:
			case 5:
				try
				{
					if ((uint)(num - -6) > 1u && num != 5)
					{
						break;
					}
					try
					{
						if (num == -6 || num == 5)
						{
							try
							{
							}
							finally
							{
								_003C_003Em__Finally4();
							}
						}
					}
					finally
					{
						_003C_003Em__Finally3();
					}
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			case -7:
			case 9:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally5();
				}
				break;
			case -9:
			case -8:
			case 10:
				try
				{
					if (num == -9 || num == 10)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally7();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally6();
				}
				break;
			case -14:
			case -13:
			case -12:
			case -11:
			case -10:
			case 11:
			case 12:
				try
				{
					switch (num)
					{
					case -12:
					case -11:
					case 11:
						try
						{
							if (num == -12 || num == 11)
							{
								try
								{
								}
								finally
								{
									_003C_003Em__Finally10();
								}
							}
						}
						finally
						{
							_003C_003Em__Finally9();
						}
						break;
					case -14:
					case -13:
					case 12:
						try
						{
							if (num == -14 || num == 12)
							{
								try
								{
								}
								finally
								{
									_003C_003Em__Finally12();
								}
							}
						}
						finally
						{
							_003C_003Em__Finally11();
						}
						break;
					}
				}
				finally
				{
					_003C_003Em__Finally8();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = default(List<Pawn>.Enumerator);
			_003Cp_003E5__4 = null;
			_003C_003E7__wrap4 = default(List<FactionPermit>.Enumerator);
			_003C_003E7__wrap5 = null;
			_003C_003E7__wrap6 = default(List<Ability>.Enumerator);
			_003C_003E7__wrap7 = null;
			_003C_003E7__wrap8 = default(List<Gene>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Caravan CS_0024_003C_003E8__locals47 = _003C_003E4__this;
				_003C_003Ec__DisplayClass123_0 CS_0024_003C_003E8__locals11;
				Ability current2;
				Gene current5;
				IEnumerable<Gizmo> caravanGizmos;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (Find.WorldSelector.SingleSelectedObject == CS_0024_003C_003E8__locals47)
					{
						_003C_003E2__current = new Gizmo_CaravanInfo(CS_0024_003C_003E8__locals47);
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_00b8;
				case 1:
					_003C_003E1__state = -1;
					goto IL_00b8;
				case 2:
					_003C_003E1__state = -3;
					goto IL_00fc;
				case 3:
					_003C_003E1__state = -1;
					_003C_003E2__current = SettleInEmptyTileUtility.SetupCamp(CS_0024_003C_003E8__locals47);
					_003C_003E1__state = 4;
					return true;
				case 4:
					_003C_003E1__state = -1;
					_003C_003E7__wrap2 = CS_0024_003C_003E8__locals47.pawns.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0290;
				case 5:
					_003C_003E1__state = -6;
					goto IL_024d;
				case 6:
					_003C_003E1__state = -1;
					goto IL_035a;
				case 7:
					_003C_003E1__state = -1;
					goto IL_041f;
				case 8:
					_003C_003E1__state = -1;
					goto IL_0447;
				case 9:
					_003C_003E1__state = -7;
					goto IL_0493;
				case 10:
					_003C_003E1__state = -9;
					goto IL_0528;
				case 11:
					_003C_003E1__state = -12;
					goto IL_0653;
				case 12:
					_003C_003E1__state = -14;
					goto IL_0727;
				case 13:
					_003C_003E1__state = -1;
					_003C_003E2__current = CS_0024_003C_003E8__locals47.GetRefuelGizmo();
					_003C_003E1__state = 14;
					return true;
				case 14:
					_003C_003E1__state = -1;
					if (DebugSettings.ShowDevGizmos)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: End cooldown",
							action = delegate
							{
								CS_0024_003C_003E8__locals47.Shuttle.LaunchableComp.lastLaunchTick = Find.TickManager.TicksGame - CS_0024_003C_003E8__locals47.Shuttle.LaunchableComp.Props.cooldownTicks;
							}
						};
						_003C_003E1__state = 15;
						return true;
					}
					goto IL_0840;
				case 15:
					_003C_003E1__state = -1;
					goto IL_0840;
				case 16:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Make random pawn hungry",
						action = delegate
						{
							if (CS_0024_003C_003E8__locals47.PawnsListForReading.Where((Pawn x) => x.needs.food != null).TryRandomElement(out var result5))
							{
								result5.needs.food.CurLevelPercentage = 0f;
							}
						}
					};
					_003C_003E1__state = 17;
					return true;
				case 17:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Kill random pawn",
						action = delegate
						{
							if (CS_0024_003C_003E8__locals47.PawnsListForReading.TryRandomElement(out var result4))
							{
								result4.Kill(null);
								Messages.Message("DEV: Killed " + result4.LabelShort, CS_0024_003C_003E8__locals47, MessageTypeDefOf.TaskCompletion, historical: false);
							}
						}
					};
					_003C_003E1__state = 18;
					return true;
				case 18:
					_003C_003E1__state = -1;
					if (ModsConfig.IdeologyActive)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Kill all non-slave pawns",
							action = delegate
							{
								for (int num2 = CS_0024_003C_003E8__locals47.PawnsListForReading.Count - 1; num2 >= 0; num2--)
								{
									Pawn pawn = CS_0024_003C_003E8__locals47.PawnsListForReading[num2];
									if (!pawn.IsSlave)
									{
										pawn.Kill(null);
										Messages.Message("DEV: Killed " + pawn.LabelShort, CS_0024_003C_003E8__locals47, MessageTypeDefOf.TaskCompletion, historical: false);
									}
								}
							}
						};
						_003C_003E1__state = 19;
						return true;
					}
					goto IL_0961;
				case 19:
					_003C_003E1__state = -1;
					goto IL_0961;
				case 20:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Down random pawn",
						action = delegate
						{
							if (CS_0024_003C_003E8__locals47.PawnsListForReading.Where((Pawn x) => !x.Downed).TryRandomElement(out var result2))
							{
								HealthUtility.DamageUntilDowned(result2);
								Messages.Message("DEV: Downed " + result2.LabelShort, CS_0024_003C_003E8__locals47, MessageTypeDefOf.TaskCompletion, historical: false);
							}
						}
					};
					_003C_003E1__state = 21;
					return true;
				case 21:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Plague on random pawn",
						action = delegate
						{
							if (CS_0024_003C_003E8__locals47.PawnsListForReading.Where((Pawn x) => !x.Downed && x.RaceProps.IsFlesh).TryRandomElement(out var result))
							{
								Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.Plague, result);
								hediff.Severity = HediffDefOf.Plague.stages[1].minSeverity - 0.001f;
								result.health.AddHediff(hediff, null, null);
								Messages.Message("DEV: Gave advanced plague to " + result.LabelShort, CS_0024_003C_003E8__locals47, MessageTypeDefOf.TaskCompletion, historical: false);
							}
						}
					};
					_003C_003E1__state = 22;
					return true;
				case 22:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Teleport to destination",
						action = delegate
						{
							CS_0024_003C_003E8__locals47.Tile = CS_0024_003C_003E8__locals47.pather.Destination;
							CS_0024_003C_003E8__locals47.pather.StopDead();
						}
					};
					_003C_003E1__state = 23;
					return true;
				case 23:
					_003C_003E1__state = -1;
					if (ModsConfig.RoyaltyActive)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: +20% psyfocus",
							action = delegate
							{
								for (int i = 0; i < CS_0024_003C_003E8__locals47.PawnsListForReading.Count; i++)
								{
									CS_0024_003C_003E8__locals47.PawnsListForReading[i].psychicEntropy?.OffsetPsyfocusDirectly(0.2f);
								}
							}
						};
						_003C_003E1__state = 24;
						return true;
					}
					goto IL_0ab9;
				case 24:
					_003C_003E1__state = -1;
					goto IL_0ab9;
				case 25:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_035a:
					if (CS_0024_003C_003E8__locals47.pather.Moving)
					{
						_003C_003E2__current = new Command_Toggle
						{
							hotKey = KeyBindingDefOf.Misc1,
							isActive = () => CS_0024_003C_003E8__locals47.pather.Paused,
							toggleAction = delegate
							{
								if (CS_0024_003C_003E8__locals47.pather.Moving)
								{
									CS_0024_003C_003E8__locals47.pather.Paused = !CS_0024_003C_003E8__locals47.pather.Paused;
								}
							},
							defaultDesc = "CommandToggleCaravanPauseDesc".Translate(2f.ToString("0.#"), 0.3f.ToStringPercent()),
							icon = TexCommand.PauseCaravan,
							defaultLabel = "CommandPauseCaravan".Translate()
						};
						_003C_003E1__state = 7;
						return true;
					}
					goto IL_041f;
					IL_0ab9:
					if (!ModsConfig.BiotechActive)
					{
						break;
					}
					CS_0024_003C_003E8__locals11 = new _003C_003Ec__DisplayClass123_0
					{
						allDissolvingThings = CaravanInventoryUtility.GetAllDissolvingThings(CS_0024_003C_003E8__locals47)
					};
					if (CS_0024_003C_003E8__locals11.allDissolvingThings.Any())
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Trigger random dissolution event",
							action = delegate
							{
								CS_0024_003C_003E8__locals11.allDissolvingThings.RandomElement().TryGetComp<CompDissolution>().TriggerDissolutionEvent();
							}
						};
						_003C_003E1__state = 25;
						return true;
					}
					break;
					IL_0653:
					if (_003C_003E7__wrap7.MoveNext())
					{
						Command current = _003C_003E7__wrap7.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 11;
						return true;
					}
					_003C_003Em__Finally10();
					_003C_003E7__wrap7 = null;
					goto IL_066d;
					IL_041f:
					if (CaravanMergeUtility.ShouldShowMergeCommand)
					{
						_003C_003E2__current = CaravanMergeUtility.MergeCommand(CS_0024_003C_003E8__locals47);
						_003C_003E1__state = 8;
						return true;
					}
					goto IL_0447;
					IL_066d:
					while (_003C_003E7__wrap6.MoveNext())
					{
						current2 = _003C_003E7__wrap6.Current;
						if (!current2.def.showGizmoOnWorldView)
						{
							continue;
						}
						goto IL_060b;
					}
					_003C_003Em__Finally9();
					_003C_003E7__wrap6 = default(List<Ability>.Enumerator);
					goto IL_068f;
					IL_0961:
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Harm random pawn",
						action = delegate
						{
							if (CS_0024_003C_003E8__locals47.PawnsListForReading.TryRandomElement(out var result3))
							{
								DamageInfo dinfo = new DamageInfo(DamageDefOf.Scratch, 10f, 999f);
								result3.TakeDamage(dinfo);
							}
						}
					};
					_003C_003E1__state = 20;
					return true;
					IL_0447:
					_003C_003E7__wrap1 = CS_0024_003C_003E8__locals47.forage.GetGizmos().GetEnumerator();
					_003C_003E1__state = -7;
					goto IL_0493;
					IL_0290:
					if (_003C_003E7__wrap2.MoveNext())
					{
						_003Cp_003E5__4 = _003C_003E7__wrap2.Current;
						if (_003Cp_003E5__4.royalty != null)
						{
							_003C_003E7__wrap4 = _003Cp_003E5__4.royalty.AllFactionPermits.GetEnumerator();
							_003C_003E1__state = -5;
							goto IL_0267;
						}
						goto IL_0289;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = default(List<Pawn>.Enumerator);
					if (CS_0024_003C_003E8__locals47.PawnsListForReading.Count((Pawn x) => x.IsColonist) >= 2)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "CommandSplitCaravan".Translate(),
							defaultDesc = "CommandSplitCaravanDesc".Translate(),
							icon = SplitCommand,
							hotKey = KeyBindingDefOf.Misc5,
							action = delegate
							{
								Find.WindowStack.Add(new Dialog_SplitCaravan(CS_0024_003C_003E8__locals47));
							}
						};
						_003C_003E1__state = 6;
						return true;
					}
					goto IL_035a;
					IL_0493:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current3 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current3;
						_003C_003E1__state = 9;
						return true;
					}
					_003C_003Em__Finally5();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap5 = Find.WorldObjects.ObjectsAt(CS_0024_003C_003E8__locals47.Tile).GetEnumerator();
					_003C_003E1__state = -8;
					goto IL_0542;
					IL_0840:
					if (!DebugSettings.ShowDevGizmos)
					{
						break;
					}
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Mental break",
						action = delegate
						{
							if (CS_0024_003C_003E8__locals47.PawnsListForReading.Where((Pawn x) => x.RaceProps.Humanlike && !x.InMentalState).TryRandomElement(out var result6))
							{
								result6.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Wander_Sad);
							}
						}
					};
					_003C_003E1__state = 16;
					return true;
					IL_0542:
					if (_003C_003E7__wrap5.MoveNext())
					{
						WorldObject current4 = _003C_003E7__wrap5.Current;
						_003C_003E7__wrap1 = current4.GetCaravanGizmos(CS_0024_003C_003E8__locals47).GetEnumerator();
						_003C_003E1__state = -9;
						goto IL_0528;
					}
					_003C_003Em__Finally6();
					_003C_003E7__wrap5 = null;
					if (Find.WorldSelector.SingleSelectedObject == CS_0024_003C_003E8__locals47)
					{
						_003C_003E7__wrap2 = CS_0024_003C_003E8__locals47.pawns.GetEnumerator();
						_003C_003E1__state = -10;
						goto IL_076a;
					}
					goto IL_078c;
					IL_0741:
					while (_003C_003E7__wrap8.MoveNext())
					{
						current5 = _003C_003E7__wrap8.Current;
						if (!current5.def.showGizmoOnWorldView)
						{
							continue;
						}
						goto IL_06df;
					}
					_003C_003Em__Finally11();
					_003C_003E7__wrap8 = default(List<Gene>.Enumerator);
					goto IL_0763;
					IL_068f:
					if (_003Cp_003E5__4.genes != null)
					{
						_003C_003E7__wrap8 = _003Cp_003E5__4.genes.GenesListForReading.GetEnumerator();
						_003C_003E1__state = -13;
						goto IL_0741;
					}
					goto IL_0763;
					IL_076a:
					if (_003C_003E7__wrap2.MoveNext())
					{
						_003Cp_003E5__4 = _003C_003E7__wrap2.Current;
						if (_003Cp_003E5__4.abilities != null && !_003Cp_003E5__4.Downed && !_003Cp_003E5__4.IsPrisoner)
						{
							_003C_003E7__wrap6 = _003Cp_003E5__4.abilities.abilities.GetEnumerator();
							_003C_003E1__state = -11;
							goto IL_066d;
						}
						goto IL_068f;
					}
					_003C_003Em__Finally8();
					_003C_003E7__wrap2 = default(List<Pawn>.Enumerator);
					goto IL_078c;
					IL_0267:
					while (_003C_003E7__wrap4.MoveNext())
					{
						FactionPermit current6 = _003C_003E7__wrap4.Current;
						caravanGizmos = current6.Permit.Worker.GetCaravanGizmos(_003Cp_003E5__4, current6.Faction);
						if (caravanGizmos == null)
						{
							continue;
						}
						goto IL_020b;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap4 = default(List<FactionPermit>.Enumerator);
					goto IL_0289;
					IL_06df:
					_003C_003E7__wrap1 = current5.GetGizmos().GetEnumerator();
					_003C_003E1__state = -14;
					goto IL_0727;
					IL_020b:
					_003C_003E7__wrap1 = caravanGizmos.GetEnumerator();
					_003C_003E1__state = -6;
					goto IL_024d;
					IL_024d:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current7 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current7;
						_003C_003E1__state = 5;
						return true;
					}
					_003C_003Em__Finally4();
					_003C_003E7__wrap1 = null;
					goto IL_0267;
					IL_0727:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current8 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current8;
						_003C_003E1__state = 12;
						return true;
					}
					_003C_003Em__Finally12();
					_003C_003E7__wrap1 = null;
					goto IL_0741;
					IL_0528:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current9 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current9;
						_003C_003E1__state = 10;
						return true;
					}
					_003C_003Em__Finally7();
					_003C_003E7__wrap1 = null;
					goto IL_0542;
					IL_0289:
					_003Cp_003E5__4 = null;
					goto IL_0290;
					IL_00b8:
					_003C_003E7__wrap1 = ((WorldObject)CS_0024_003C_003E8__locals47).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00fc;
					IL_00fc:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current10 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current10;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (CS_0024_003C_003E8__locals47.IsPlayerControlled)
					{
						if (Find.WorldSelector.SingleSelectedObject == CS_0024_003C_003E8__locals47)
						{
							_003C_003E2__current = SettleInEmptyTileUtility.SettleCommand(CS_0024_003C_003E8__locals47);
							_003C_003E1__state = 3;
							return true;
						}
						goto IL_035a;
					}
					goto IL_0840;
					IL_0763:
					_003Cp_003E5__4 = null;
					goto IL_076a;
					IL_078c:
					if (ModsConfig.OdysseyActive && CS_0024_003C_003E8__locals47.Shuttle != null && Find.WorldSelector.NumSelectedObjects == 1)
					{
						_003C_003E2__current = CS_0024_003C_003E8__locals47.GetLaunchGizmo();
						_003C_003E1__state = 13;
						return true;
					}
					goto IL_0840;
					IL_060b:
					_003C_003E7__wrap7 = current2.GetGizmos().GetEnumerator();
					_003C_003E1__state = -12;
					goto IL_0653;
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
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -4;
			((IDisposable)_003C_003E7__wrap4/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally4()
		{
			_003C_003E1__state = -5;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally5()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally6()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap5 != null)
			{
				_003C_003E7__wrap5.Dispose();
			}
		}

		private void _003C_003Em__Finally7()
		{
			_003C_003E1__state = -8;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally8()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally9()
		{
			_003C_003E1__state = -10;
			((IDisposable)_003C_003E7__wrap6/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally10()
		{
			_003C_003E1__state = -11;
			if (_003C_003E7__wrap7 != null)
			{
				_003C_003E7__wrap7.Dispose();
			}
		}

		private void _003C_003Em__Finally11()
		{
			_003C_003E1__state = -10;
			((IDisposable)_003C_003E7__wrap8/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally12()
		{
			_003C_003E1__state = -13;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__123 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__123(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetTransportersFloatMenuOptions_003Ed__126 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Caravan _003C_003E4__this;

		private IEnumerable<IThingHolder> pods;

		public IEnumerable<IThingHolder> _003C_003E3__pods;

		private Action<PlanetTile, TransportersArrivalAction> launchAction;

		public Action<PlanetTile, TransportersArrivalAction> _003C_003E3__launchAction;

		private IEnumerator<FloatMenuOption> _003C_003E7__wrap1;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
		public _003CGetTransportersFloatMenuOptions_003Ed__126(int _003C_003E1__state)
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
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Caravan caravan = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((WorldObject)caravan).GetTransportersFloatMenuOptions(pods, launchAction).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007e;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007e;
				case 2:
					{
						_003C_003E1__state = -4;
						break;
					}
					IL_007e:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap1 = TransportersArrivalAction_GiveToCaravan.GetFloatMenuOptions(launchAction, pods, caravan).GetEnumerator();
					_003C_003E1__state = -4;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					FloatMenuOption current2 = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current2;
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap1 = null;
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
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetTransportersFloatMenuOptions_003Ed__126 _003CGetTransportersFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetTransportersFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetTransportersFloatMenuOptions_003Ed__ = new _003CGetTransportersFloatMenuOptions_003Ed__126(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetTransportersFloatMenuOptions_003Ed__.pods = _003C_003E3__pods;
			_003CGetTransportersFloatMenuOptions_003Ed__.launchAction = _003C_003E3__launchAction;
			return _003CGetTransportersFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	private int uniqueId = -1;

	private string nameInt;

	public ThingOwner<Pawn> pawns;

	public bool autoJoinable;

	public Caravan_PathFollower pather;

	public Caravan_GotoMoteRenderer gotoMote;

	public Caravan_Tweener tweener;

	public Caravan_TraderTracker trader;

	public Caravan_ForageTracker forage;

	public Caravan_NeedsTracker needs;

	public Caravan_CarryTracker carryTracker;

	public Caravan_BedsTracker beds;

	public Caravan_BabyTracker babies;

	public StoryState storyState;

	private Material cachedMat;

	private bool cachedImmobilized;

	private int cachedImmobilizedForTicks = -99999;

	private (float days, float tillRot) cachedDaysWorthOfFood;

	private int cachedDaysWorthOfFoodForTicks = -99999;

	public bool notifiedOutOfFood;

	private const int ImmobilizedCacheDuration = 60;

	private const int DaysWorthOfFoodCacheDuration = 3000;

	private static readonly Texture2D SplitCommand = ContentFinder<Texture2D>.Get("UI/Commands/SplitCaravan");

	private static readonly Color PlayerCaravanColor = new Color(1f, 0.863f, 0.33f);

	public bool hasShuttleDirty = true;

	private Building_PassengerShuttle cachedShuttle;

	public List<Pawn> PawnsListForReading => pawns.InnerListForReading;

	public override float DrawAltitude => 0.035f;

	public override Material Material
	{
		get
		{
			if (cachedMat == null)
			{
				cachedMat = MaterialPool.MatFrom(color: (base.Faction == null) ? Color.white : ((!base.Faction.IsPlayer) ? base.Faction.Color : PlayerCaravanColor), texPath: def.texture, shader: ShaderDatabase.WorldOverlayTransparentLit, renderQueue: 3600);
			}
			return cachedMat;
		}
	}

	public string Name
	{
		get
		{
			return nameInt;
		}
		set
		{
			nameInt = value;
		}
	}

	public override Vector3 DrawPos => tweener.TweenedPos;

	public bool IsPlayerControlled => base.Faction == Faction.OfPlayer;

	public bool ImmobilizedByMass
	{
		get
		{
			if (Find.TickManager.TicksGame - cachedImmobilizedForTicks < 60)
			{
				return cachedImmobilized;
			}
			cachedImmobilized = MassUsage > MassCapacity;
			cachedImmobilizedForTicks = Find.TickManager.TicksGame;
			return cachedImmobilized;
		}
	}

	public (float days, float tillRot) DaysWorthOfFood
	{
		get
		{
			if (Find.TickManager.TicksGame - cachedDaysWorthOfFoodForTicks < 3000)
			{
				return cachedDaysWorthOfFood;
			}
			cachedDaysWorthOfFood = (days: DaysWorthOfFoodCalculator.ApproxDaysWorthOfFood(this), tillRot: DaysUntilRotCalculator.ApproxDaysUntilRot(this));
			cachedDaysWorthOfFoodForTicks = Find.TickManager.TicksGame;
			return cachedDaysWorthOfFood;
		}
	}

	public bool CantMove
	{
		get
		{
			if (!NightResting && !pather.Paused && !AllOwnersHaveMentalBreak && !AllOwnersDowned && !ImmobilizedByMass)
			{
				return Shuttle != null;
			}
			return true;
		}
	}

	public float MassCapacity => CollectionsMassCalculator.Capacity(PawnsListForReading);

	public string MassCapacityExplanation
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			CollectionsMassCalculator.Capacity(PawnsListForReading, stringBuilder);
			return stringBuilder.ToString();
		}
	}

	public float MassUsage => CollectionsMassCalculator.MassUsage(PawnsListForReading, IgnorePawnsInventoryMode.DontIgnore);

	public bool AllOwnersDowned
	{
		get
		{
			for (int i = 0; i < pawns.Count; i++)
			{
				if (IsOwner(pawns[i]) && !pawns[i].Downed)
				{
					return false;
				}
			}
			return true;
		}
	}

	public bool AllOwnersHaveMentalBreak
	{
		get
		{
			for (int i = 0; i < pawns.Count; i++)
			{
				if (IsOwner(pawns[i]) && !pawns[i].InMentalState)
				{
					return false;
				}
			}
			return true;
		}
	}

	public bool NightResting
	{
		get
		{
			if (!base.Spawned)
			{
				return false;
			}
			if (!needs.AnyPawnsNeedRest)
			{
				return false;
			}
			if (pather.Moving && pather.nextTile == pather.Destination && Caravan_PathFollower.IsValidFinalPushDestination(pather.Destination) && Mathf.CeilToInt(pather.nextTileCostLeft / 1f) <= 10000)
			{
				return false;
			}
			return CaravanNightRestUtility.RestingNowAt(base.Tile);
		}
	}

	public int LeftRestTicks
	{
		get
		{
			if (!NightResting)
			{
				return 0;
			}
			return CaravanNightRestUtility.LeftRestTicksAt(base.Tile);
		}
	}

	public int LeftNonRestTicks
	{
		get
		{
			if (NightResting)
			{
				return 0;
			}
			return CaravanNightRestUtility.LeftNonRestTicksAt(base.Tile);
		}
	}

	public override string Label
	{
		get
		{
			if (nameInt != null)
			{
				return nameInt;
			}
			return base.Label;
		}
	}

	public override bool HasName => !nameInt.NullOrEmpty();

	public int TicksPerMove => CaravanTicksPerMoveUtility.GetTicksPerMove(this);

	public override bool AppendFactionToInspectString => false;

	public float Visibility => CaravanVisibilityCalculator.Visibility(this);

	public string VisibilityExplanation
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			CaravanVisibilityCalculator.Visibility(this, stringBuilder);
			return stringBuilder.ToString();
		}
	}

	public string TicksPerMoveExplanation
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			CaravanTicksPerMoveUtility.GetTicksPerMove(this, stringBuilder);
			return stringBuilder.ToString();
		}
	}

	public IEnumerable<Thing> AllThings => CaravanInventoryUtility.AllInventoryItems(this).Concat(pawns);

	public Building_PassengerShuttle Shuttle
	{
		get
		{
			if (hasShuttleDirty)
			{
				hasShuttleDirty = false;
				cachedShuttle = CaravanInventoryUtility.FindShuttle(this);
			}
			return cachedShuttle;
		}
	}

	public int ConstantRandSeed => uniqueId ^ 0x2B6813E1;

	public StoryState StoryState => storyState;

	public GameConditionManager GameConditionManager
	{
		get
		{
			Log.ErrorOnce("Attempted to retrieve condition manager directly from caravan", 13291050);
			return null;
		}
	}

	public float PlayerWealthForStoryteller
	{
		get
		{
			if (!IsPlayerControlled)
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 0; i < pawns.Count; i++)
			{
				num += WealthWatcher.GetEquipmentApparelAndInventoryWealth(pawns[i]);
				if (pawns[i].Faction == Faction.OfPlayer)
				{
					float num2 = pawns[i].MarketValue;
					if (pawns[i].IsSlave)
					{
						num2 *= 0.75f;
					}
					num += num2;
				}
			}
			return num * 0.7f;
		}
	}

	public IEnumerable<Pawn> PlayerPawnsForStoryteller
	{
		get
		{
			if (!IsPlayerControlled)
			{
				return Enumerable.Empty<Pawn>();
			}
			return PawnsListForReading.Where((Pawn x) => x.Faction == Faction.OfPlayer);
		}
	}

	public FloatRange IncidentPointsRandomFactorRange => StorytellerUtility.CaravanPointsRandomFactorRange;

	public TraderKindDef TraderKind => trader.TraderKind;

	public IEnumerable<Thing> Goods => trader.Goods;

	public int RandomPriceFactorSeed => trader.RandomPriceFactorSeed;

	public string TraderName => trader.TraderName;

	public bool CanTradeNow => trader.CanTradeNow;

	public float TradePriceImprovementOffsetForPlayer => 0f;

	public TradeCurrency TradeCurrency => TraderKind.tradeCurrency;

	public void SetUniqueId(int newId)
	{
		if (uniqueId != -1 || newId < 0)
		{
			Log.Error("Tried to set caravan with uniqueId " + uniqueId + " to have uniqueId " + newId);
		}
		uniqueId = newId;
	}

	public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
	{
		return trader.ColonyThingsWillingToBuy(playerNegotiator);
	}

	public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
	{
		trader.GiveSoldThingToTrader(toGive, countToGive, playerNegotiator);
	}

	public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
	{
		trader.GiveSoldThingToPlayer(toGive, countToGive, playerNegotiator);
	}

	public Caravan()
	{
		pawns = new ThingOwner<Pawn>(this, oneStackOnly: false, LookMode.Reference);
		pather = new Caravan_PathFollower(this);
		gotoMote = new Caravan_GotoMoteRenderer();
		tweener = new Caravan_Tweener(this);
		trader = new Caravan_TraderTracker(this);
		forage = new Caravan_ForageTracker(this);
		needs = new Caravan_NeedsTracker(this);
		carryTracker = new Caravan_CarryTracker(this);
		beds = new Caravan_BedsTracker(this);
		if (ModsConfig.BiotechActive)
		{
			babies = new Caravan_BabyTracker(this);
		}
		storyState = new StoryState(this);
	}

	public override void ExposeData()
	{
		base.ExposeData();
		if (Scribe.mode == LoadSaveMode.Saving)
		{
			pawns.RemoveAll((Pawn x) => x.Destroyed);
		}
		Scribe_Values.Look(ref uniqueId, "uniqueId", 0);
		Scribe_Values.Look(ref nameInt, "name");
		Scribe_Deep.Look(ref pawns, "pawns", this);
		Scribe_Values.Look(ref autoJoinable, "autoJoinable", defaultValue: false);
		Scribe_Deep.Look(ref pather, "pather", this);
		Scribe_Deep.Look(ref trader, "trader", this);
		Scribe_Deep.Look(ref forage, "forage", this);
		Scribe_Deep.Look(ref needs, "needs", this);
		Scribe_Deep.Look(ref carryTracker, "carryTracker", this);
		Scribe_Deep.Look(ref beds, "beds", this);
		Scribe_Deep.Look(ref storyState, "storyState", this);
		BackCompatibility.PostExposeData(this);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			babies?.Recache();
		}
	}

	public override void PostAdd()
	{
		base.PostAdd();
		carryTracker.Notify_CaravanSpawned();
		beds.Notify_CaravanSpawned();
		babies?.Recache();
		Find.ColonistBar.MarkColonistsDirty();
		hasShuttleDirty = true;
	}

	public override void PostRemove()
	{
		base.PostRemove();
		pather.StopDead();
		Find.ColonistBar.MarkColonistsDirty();
	}

	protected override void TickInterval(int delta)
	{
		base.TickInterval(delta);
		CheckAnyNonWorldPawns();
		pather.PatherTickInterval(delta);
		tweener.TweenerTickInterval(delta);
		forage.ForageTrackerTickInterval(delta);
		carryTracker.CarryTrackerTickInterval(delta);
		beds.BedsTrackerTickInterval(delta);
		needs.NeedsTrackerTickInterval(delta);
		babies?.TickInterval(delta);
		CaravanDrugPolicyUtility.CheckTakeScheduledDrugs(this, delta);
		CaravanTendUtility.CheckTend(this, delta);
		if (ModsConfig.BiotechActive)
		{
			CaravanPollutionUtility.CheckDamageFromPollution(this, delta);
		}
	}

	public override void SpawnSetup()
	{
		base.SpawnSetup();
		tweener.ResetTweenedPosToRoot();
	}

	public override void DrawExtraSelectionOverlays()
	{
		base.DrawExtraSelectionOverlays();
		if (IsPlayerControlled && pather.curPath != null)
		{
			pather.curPath.DrawPath(this);
		}
		gotoMote.RenderMote();
	}

	public void AddPawn(Pawn p, bool addCarriedPawnToWorldPawnsIfAny)
	{
		if (p == null)
		{
			Log.Warning("Tried to add a null pawn to " + this);
			return;
		}
		if (p.Dead)
		{
			Log.Warning("Tried to add " + p?.ToString() + " to " + this?.ToString() + ", but this pawn is dead.");
			return;
		}
		Pawn pawn = p.carryTracker.CarriedThing as Pawn;
		if (pawn != null)
		{
			p.carryTracker.innerContainer.Remove(pawn);
		}
		p.DeSpawnOrDeselect();
		if (pawns.TryAdd(p))
		{
			if (ShouldAutoCapture(p))
			{
				p.guest.CapturedBy(base.Faction);
			}
			if (pawn != null)
			{
				if (ShouldAutoCapture(pawn))
				{
					pawn.guest.CapturedBy(base.Faction, p);
				}
				AddPawn(pawn, addCarriedPawnToWorldPawnsIfAny);
				if (addCarriedPawnToWorldPawnsIfAny)
				{
					Find.WorldPawns.PassToWorld(pawn);
				}
			}
		}
		else
		{
			Log.Error("Couldn't add pawn " + p?.ToString() + " to caravan.");
		}
	}

	public void AddPawnOrItem(Thing thing, bool addCarriedPawnToWorldPawnsIfAny)
	{
		if (thing == null)
		{
			Log.Warning("Tried to add a null thing to " + this);
		}
		else if (thing is Pawn p)
		{
			AddPawn(p, addCarriedPawnToWorldPawnsIfAny);
		}
		else
		{
			CaravanInventoryUtility.GiveThing(this, thing);
		}
	}

	public bool ContainsPawn(Pawn p)
	{
		return pawns.Contains(p);
	}

	public void RemovePawn(Pawn p)
	{
		pawns.Remove(p);
	}

	public void RemoveAllPawns()
	{
		pawns.Clear();
	}

	public bool IsOwner(Pawn p)
	{
		if (pawns.Contains(p))
		{
			return CaravanUtility.IsOwner(p, base.Faction);
		}
		return false;
	}

	public override string GetInspectString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(base.GetInspectString());
		if (stringBuilder.Length != 0)
		{
			stringBuilder.AppendLine();
		}
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		for (int i = 0; i < pawns.Count; i++)
		{
			if (pawns[i].IsColonist)
			{
				num++;
			}
			else if (pawns[i].RaceProps.Animal)
			{
				num2++;
			}
			else if (pawns[i].IsPrisoner)
			{
				num3++;
			}
			else if (pawns[i].RaceProps.IsMechanoid)
			{
				num6++;
			}
			if (pawns[i].Downed && !pawns[i].ageTracker.CurLifeStage.alwaysDowned)
			{
				num4++;
			}
			if (pawns[i].InMentalState)
			{
				num5++;
			}
		}
		stringBuilder.Append("CaravanColonistsCount".Translate(num, (num == 1) ? Faction.OfPlayer.def.pawnSingular : Faction.OfPlayer.def.pawnsPlural));
		if (num2 == 1)
		{
			stringBuilder.Append(", " + "CaravanAnimal".Translate());
		}
		else if (num2 > 1)
		{
			stringBuilder.Append(", " + "CaravanAnimalsCount".Translate(num2));
		}
		if (num3 == 1)
		{
			stringBuilder.Append(", " + "CaravanPrisoner".Translate());
		}
		else if (num3 > 1)
		{
			stringBuilder.Append(", " + "CaravanPrisonersCount".Translate(num3));
		}
		if (ModsConfig.BiotechActive)
		{
			if (num6 == 1)
			{
				stringBuilder.Append(", " + "CaravanMech".Translate());
			}
			else if (num6 > 1)
			{
				stringBuilder.Append(", " + "CaravanMechsCount".Translate(num6));
			}
		}
		stringBuilder.AppendLine();
		if (num5 > 0)
		{
			stringBuilder.Append("CaravanPawnsInMentalState".Translate(num5));
		}
		if (num4 > 0)
		{
			if (num5 > 0)
			{
				stringBuilder.Append(", ");
			}
			stringBuilder.Append("CaravanPawnsDowned".Translate(num4));
		}
		if (num5 > 0 || num4 > 0)
		{
			stringBuilder.AppendLine();
		}
		if (pather.Moving)
		{
			if (pather.ArrivalAction != null)
			{
				stringBuilder.Append(pather.ArrivalAction.ReportString);
			}
			else
			{
				stringBuilder.Append("CaravanTraveling".Translate());
			}
		}
		else
		{
			Settlement settlement = CaravanVisitUtility.SettlementVisitedNow(this);
			if (settlement != null)
			{
				stringBuilder.Append("CaravanVisiting".Translate(settlement.Label));
			}
			else
			{
				stringBuilder.Append("CaravanWaiting".Translate());
			}
		}
		if (pather.Moving)
		{
			float num7 = (float)CaravanArrivalTimeEstimator.EstimatedTicksToArrive(this, allowCaching: true) / 60000f;
			stringBuilder.AppendLine();
			stringBuilder.Append("CaravanEstimatedTimeToDestination".Translate(num7.ToString("0.#")));
		}
		if (AllOwnersDowned)
		{
			stringBuilder.AppendLine();
			stringBuilder.Append("AllCaravanMembersDowned".Translate());
		}
		else if (AllOwnersHaveMentalBreak)
		{
			stringBuilder.AppendLine();
			stringBuilder.Append("AllCaravanMembersMentalBreak".Translate());
		}
		else if (ImmobilizedByMass)
		{
			stringBuilder.AppendLine();
			stringBuilder.Append("CaravanImmobilizedByMass".Translate());
		}
		if (needs.AnyPawnOutOfFood(out var malnutritionHediff))
		{
			stringBuilder.AppendLine();
			stringBuilder.Append("CaravanOutOfFood".Translate());
			if (!malnutritionHediff.NullOrEmpty())
			{
				stringBuilder.Append(" ");
				stringBuilder.Append(malnutritionHediff);
				stringBuilder.Append(".");
			}
		}
		if (!pather.MovingNow)
		{
			int usedBedCount = beds.GetUsedBedCount();
			stringBuilder.AppendLine();
			stringBuilder.Append(CaravanBedUtility.AppendUsingBedsLabel("CaravanResting".Translate(), usedBedCount));
		}
		else
		{
			string inspectStringLine = carryTracker.GetInspectStringLine();
			if (!inspectStringLine.NullOrEmpty())
			{
				stringBuilder.AppendLine();
				stringBuilder.Append(inspectStringLine);
			}
			string inBedForMedicalReasonsInspectStringLine = beds.GetInBedForMedicalReasonsInspectStringLine();
			if (!inBedForMedicalReasonsInspectStringLine.NullOrEmpty())
			{
				stringBuilder.AppendLine();
				stringBuilder.Append(inBedForMedicalReasonsInspectStringLine);
			}
		}
		if (ModsConfig.OdysseyActive && Shuttle != null)
		{
			stringBuilder.AppendLine();
			stringBuilder.Append($"{"CaravanShuttleFuel".Translate()}: {Shuttle.FuelLevel:F0} / {Shuttle.MaxFuelLevel:F0}");
		}
		return stringBuilder.ToString();
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__123))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__123(-2)
		{
			_003C_003E4__this = this
		};
	}

	private Gizmo GetLaunchGizmo()
	{
		Command_Action command_Action = new Command_Action();
		command_Action.defaultLabel = "CommandLaunchGroup".Translate();
		command_Action.defaultDesc = "CommandLaunchGroupDesc".Translate();
		command_Action.icon = CompLaunchable.LaunchCommandTex;
		command_Action.action = delegate
		{
			Shuttle.LaunchableComp.StartChoosingDestination(delegate(PlanetTile tile, TransportersArrivalAction action)
			{
				CaravanShuttleUtility.LaunchShuttle(this, tile, action);
			}, null);
		};
		AcceptanceReport acceptanceReport = CaravanShuttleUtility.CanLaunchCaravanShuttle(this);
		if (!acceptanceReport.Accepted)
		{
			command_Action.Disable(acceptanceReport.Reason);
		}
		return command_Action;
	}

	private Gizmo GetRefuelGizmo()
	{
		float fuelInCaravan = CaravanShuttleUtility.FuelInCaravan(this);
		string text = null;
		if (fuelInCaravan <= 0f)
		{
			text = "NoFuelInCaravan".Translate();
		}
		if (Mathf.Approximately(Shuttle.FuelLevel, Shuttle.MaxFuelLevel))
		{
			text = "ShuttleFullyFueled".Translate();
		}
		return new Command_Action
		{
			defaultLabel = "CommandRefuelShuttleFromCargo".Translate(),
			defaultDesc = "CommandRefuelShuttleFromCargoDesc".Translate(),
			icon = Building_PassengerShuttle.RefuelFromCargoIcon.Texture,
			action = delegate
			{
				int to = Mathf.FloorToInt(Mathf.Min(fuelInCaravan, Shuttle.MaxFuelLevel - Shuttle.FuelLevel));
				Dialog_Slider window = new Dialog_Slider((int val) => "RefuelShuttleCount".Translate(val), 1, to, delegate(int count)
				{
					CaravanShuttleUtility.ConsumeFuelFromCaravanInventory(this, count);
					Shuttle.RefuelableComp.Refuel(count);
				});
				Find.WindowStack.Add(window);
			},
			Disabled = !text.NullOrEmpty(),
			disabledReason = text
		};
	}

	[IteratorStateMachine(typeof(_003CGetTransportersFloatMenuOptions_003Ed__126))]
	public override IEnumerable<FloatMenuOption> GetTransportersFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetTransportersFloatMenuOptions_003Ed__126(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__pods = pods,
			_003C_003E3__launchAction = launchAction
		};
	}

	public void RecacheInventory()
	{
		cachedImmobilizedForTicks = -99999;
		cachedDaysWorthOfFoodForTicks = -99999;
		hasShuttleDirty = true;
	}

	public virtual void Notify_MemberDied(Pawn member)
	{
		if (!base.Spawned)
		{
			Log.Error("Caravan member died in an unspawned caravan. Unspawned caravans shouldn't be kept for more than a single frame.");
		}
		if (!PawnsListForReading.Any((Pawn x) => x != member && IsOwner(x)))
		{
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(this);
			for (int i = 0; i < list.Count; i++)
			{
				list[i].Notify_AbandonedAtTile(base.Tile);
			}
			RemovePawn(member);
			if (base.Faction == Faction.OfPlayer)
			{
				if (ModsConfig.IdeologyActive && PawnsListForReading.Any((Pawn x) => x != member && x.IsSlave))
				{
					Find.LetterStack.ReceiveLetter("LetterLabelAllCaravanColonistsDied".Translate(), "LetterOnlySlaveCaravanColonistsLeft".Translate(Name).CapitalizeFirst(), LetterDefOf.NegativeEvent, new GlobalTargetInfo(base.Tile));
				}
				else
				{
					Find.LetterStack.ReceiveLetter("LetterLabelAllCaravanColonistsDied".Translate(), "LetterAllCaravanColonistsDied".Translate(Name).CapitalizeFirst(), LetterDefOf.NegativeEvent, new GlobalTargetInfo(base.Tile));
				}
			}
			pawns.Clear();
			Destroy();
		}
		else
		{
			member.Strip();
			RemovePawn(member);
		}
	}

	public virtual void Notify_Merged(List<Caravan> group)
	{
		notifiedOutOfFood = false;
	}

	public virtual void Notify_StartedTrading()
	{
		notifiedOutOfFood = false;
	}

	private void CheckAnyNonWorldPawns()
	{
		for (int num = pawns.Count - 1; num >= 0; num--)
		{
			if (!pawns[num].IsWorldPawn())
			{
				Log.Error($"Caravan member {pawns[num]} is not a world pawn. Removing...");
				pawns.Remove(pawns[num]);
			}
		}
	}

	private bool ShouldAutoCapture(Pawn p)
	{
		return CaravanUtility.ShouldAutoCapture(p, base.Faction);
	}

	public void Notify_PawnRemoved(Pawn p)
	{
		Find.ColonistBar.MarkColonistsDirty();
		RecacheInventory();
		carryTracker.Notify_PawnRemoved();
		beds.Notify_PawnRemoved();
		babies?.Recache();
		p.mindState?.ClearBreastfeedCaravan();
	}

	public void Notify_PawnAdded(Pawn p)
	{
		Find.ColonistBar.MarkColonistsDirty();
		RecacheInventory();
		babies?.Recache();
	}

	public void Notify_DestinationOrPauseStatusChanged()
	{
		cachedDaysWorthOfFoodForTicks = -99999;
	}

	public void Notify_Teleported()
	{
		tweener.ResetTweenedPosToRoot();
		pather.Notify_Teleported_Int();
	}

	public ThingOwner GetDirectlyHeldThings()
	{
		return pawns;
	}

	public void GetChildHolders(List<IThingHolder> outChildren)
	{
		ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
	}
}
