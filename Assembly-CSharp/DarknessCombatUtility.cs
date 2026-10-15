using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using Verse;

public static class DarknessCombatUtility
{
	[CompilerGenerated]
	private sealed class _003CGetStatEntriesForPawn_003Ed__8 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private List<Precept> _003Cprecepts_003E5__2;

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
		public _003CGetStatEntriesForPawn_003Ed__8(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cprecepts_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (pawn.Ideo == null)
				{
					return false;
				}
				_003Cprecepts_003E5__2 = pawn.Ideo.PreceptsListForReading;
				tmpStatDefs.Clear();
				for (int i = 0; i < _003Cprecepts_003E5__2.Count; i++)
				{
					List<StatModifier> statOffsets = _003Cprecepts_003E5__2[i].def.statOffsets;
					if (statOffsets == null)
					{
						continue;
					}
					for (int j = 0; j < statOffsets.Count; j++)
					{
						StatModifier statModifier = statOffsets[j];
						if (statModifier.stat == StatDefOf.ShootingAccuracyOutdoorsLitOffset || statModifier.stat == StatDefOf.ShootingAccuracyOutdoorsDarkOffset || statModifier.stat == StatDefOf.ShootingAccuracyIndoorsDarkOffset || statModifier.stat == StatDefOf.ShootingAccuracyIndoorsLitOffset)
						{
							tmpStatDefs.Add(statModifier);
						}
					}
				}
				if (tmpStatDefs.Count > 0)
				{
					Ideo ideo = pawn.Ideo;
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("Stat_Pawn_DarknessCombatShooting_Desc".Translate() + " " + "Stat_PawnDarkness_FollowingOffset".Translate() + ":");
					stringBuilder.AppendLine();
					for (int k = 0; k < tmpStatDefs.Count; k++)
					{
						stringBuilder.AppendLine(tmpStatDefs[k].stat.LabelCap + ": " + tmpStatDefs[k].ValueToStringAsOffset);
					}
					stringBuilder.AppendLine("\n" + "CausedBy".Translate() + ": " + "BeliefInIdeo".Translate() + " " + ideo.name);
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.PawnCombat, "Stat_Pawn_DarknessCombatShooting_Name".Translate(), tmpStatDefs.MinBy((StatModifier s) => s.value).ValueToStringAsOffset + " ~ " + tmpStatDefs.MaxBy((StatModifier s) => s.value).ValueToStringAsOffset, stringBuilder.ToString(), 4051, null, new Dialog_InfoCard.Hyperlink[1]
					{
						new Dialog_InfoCard.Hyperlink(ideo)
					});
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_02b0;
			}
			case 1:
				_003C_003E1__state = -1;
				goto IL_02b0;
			case 2:
				_003C_003E1__state = -1;
				goto IL_0526;
			case 3:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0526:
				tmpStatDefs.Clear();
				for (int l = 0; l < _003Cprecepts_003E5__2.Count; l++)
				{
					List<StatModifier> statOffsets2 = _003Cprecepts_003E5__2[l].def.statOffsets;
					if (statOffsets2 == null)
					{
						continue;
					}
					for (int m = 0; m < statOffsets2.Count; m++)
					{
						StatModifier statModifier2 = statOffsets2[m];
						if (statModifier2.stat == StatDefOf.MeleeDodgeChanceIndoorsDarkOffset || statModifier2.stat == StatDefOf.MeleeDodgeChanceIndoorsLitOffset || statModifier2.stat == StatDefOf.MeleeDodgeChanceOutdoorsLitOffset || statModifier2.stat == StatDefOf.MeleeDodgeChanceOutdoorsDarkOffset)
						{
							tmpStatDefs.Add(statModifier2);
						}
					}
				}
				if (tmpStatDefs.Count > 0)
				{
					Ideo ideo2 = pawn.Ideo;
					StringBuilder stringBuilder2 = new StringBuilder();
					stringBuilder2.AppendLine("Stat_Pawn_DarknessMeleeDodgeChance_Desc".Translate() + " " + "Stat_PawnDarkness_FollowingOffset".Translate() + ":");
					stringBuilder2.AppendLine();
					for (int n = 0; n < tmpStatDefs.Count; n++)
					{
						stringBuilder2.AppendLine(tmpStatDefs[n].stat.LabelCap + ": " + tmpStatDefs[n].ValueToStringAsOffset);
					}
					stringBuilder2.AppendLine("\n" + "CausedBy".Translate() + ": " + "BeliefInIdeo".Translate() + " " + ideo2.name);
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.PawnCombat, "Stat_Pawn_DarknessMeleeDodgeChance_Name".Translate(), tmpStatDefs.MinBy((StatModifier s) => s.value).ValueToStringAsOffset + " ~ " + tmpStatDefs.MaxBy((StatModifier s) => s.value).ValueToStringAsOffset, stringBuilder2.ToString(), 4101, null, new Dialog_InfoCard.Hyperlink[1]
					{
						new Dialog_InfoCard.Hyperlink(ideo2)
					});
					_003C_003E1__state = 3;
					return true;
				}
				break;
				IL_02b0:
				tmpStatDefs.Clear();
				for (int num = 0; num < _003Cprecepts_003E5__2.Count; num++)
				{
					List<StatModifier> statOffsets3 = _003Cprecepts_003E5__2[num].def.statOffsets;
					if (statOffsets3 == null)
					{
						continue;
					}
					for (int num2 = 0; num2 < statOffsets3.Count; num2++)
					{
						StatModifier statModifier3 = statOffsets3[num2];
						if (statModifier3.stat == StatDefOf.MeleeHitChanceIndoorsDarkOffset || statModifier3.stat == StatDefOf.MeleeHitChanceIndoorsLitOffset || statModifier3.stat == StatDefOf.MeleeHitChanceOutdoorsLitOffset || statModifier3.stat == StatDefOf.MeleeHitChanceOutdoorsDarkOffset)
						{
							tmpStatDefs.Add(statModifier3);
						}
					}
				}
				if (tmpStatDefs.Count > 0)
				{
					Ideo ideo3 = pawn.Ideo;
					StringBuilder stringBuilder3 = new StringBuilder();
					stringBuilder3.AppendLine("Stat_Pawn_DarknessMeleeHitChance_Desc".Translate() + " " + "Stat_PawnDarkness_FollowingOffset".Translate() + ":");
					stringBuilder3.AppendLine();
					for (int num3 = 0; num3 < tmpStatDefs.Count; num3++)
					{
						stringBuilder3.AppendLine(tmpStatDefs[num3].stat.LabelCap + ": " + tmpStatDefs[num3].ValueToStringAsOffset);
					}
					stringBuilder3.AppendLine("\n" + "CausedBy".Translate() + ": " + "BeliefInIdeo".Translate() + " " + ideo3.name);
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.PawnCombat, "Stat_Pawn_DarknessMeleeHitChance_Name".Translate(), tmpStatDefs.MinBy((StatModifier s) => s.value).ValueToStringAsOffset + " ~ " + tmpStatDefs.MaxBy((StatModifier s) => s.value).ValueToStringAsOffset, stringBuilder3.ToString(), 4101, null, new Dialog_InfoCard.Hyperlink[1]
					{
						new Dialog_InfoCard.Hyperlink(ideo3)
					});
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_0526;
			}
			tmpStatDefs.Clear();
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CGetStatEntriesForPawn_003Ed__8 _003CGetStatEntriesForPawn_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetStatEntriesForPawn_003Ed__ = this;
			}
			else
			{
				_003CGetStatEntriesForPawn_003Ed__ = new _003CGetStatEntriesForPawn_003Ed__8(0);
			}
			_003CGetStatEntriesForPawn_003Ed__.pawn = _003C_003E3__pawn;
			return _003CGetStatEntriesForPawn_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	private const float SkyGlowDarkThreshold = 0.35f;

	private static List<StatModifier> tmpStatDefs = new List<StatModifier>();

	public static bool IsOutdoorsAndDark(Thing thing)
	{
		if (Outdoors(thing))
		{
			return !IsOutdoorsLit(thing.Map);
		}
		return false;
	}

	public static bool IsOutdoorsAndLit(Thing thing)
	{
		if (Outdoors(thing))
		{
			return IsOutdoorsLit(thing.Map);
		}
		return false;
	}

	public static bool IsIndoorsAndDark(Thing thing)
	{
		if (!Outdoors(thing))
		{
			if (thing.Map.glowGrid.PsychGlowAt(thing.Position) != 0)
			{
				return DarklightUtility.IsDarklightAt(thing.Position, thing.Map);
			}
			return true;
		}
		return false;
	}

	public static bool IsIndoorsAndLit(Thing thing)
	{
		if (!Outdoors(thing) && thing.Map.glowGrid.PsychGlowAt(thing.Position) == PsychGlow.Lit)
		{
			return !DarklightUtility.IsDarklightAt(thing.Position, thing.Map);
		}
		return false;
	}

	private static bool Outdoors(Thing thing)
	{
		RoofDef roof = thing.Position.GetRoof(thing.Map);
		if (roof != null && (roof.isNatural || roof.isThickRoof))
		{
			return false;
		}
		return thing.Position.GetRoom(thing.Map)?.PsychologicallyOutdoors ?? false;
	}

	private static bool IsOutdoorsLit(Map map)
	{
		return map.skyManager.CurSkyGlow > 0.35f;
	}

	[IteratorStateMachine(typeof(_003CGetStatEntriesForPawn_003Ed__8))]
	public static IEnumerable<StatDrawEntry> GetStatEntriesForPawn(Pawn pawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetStatEntriesForPawn_003Ed__8(-2)
		{
			_003C_003E3__pawn = pawn
		};
	}
}
