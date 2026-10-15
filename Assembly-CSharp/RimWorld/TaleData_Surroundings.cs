using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;
using Verse.Grammar;

namespace RimWorld;

public class TaleData_Surroundings : TaleData
{
	[CompilerGenerated]
	private sealed class _003CGetRules_003Ed__12 : IEnumerable<Rule>, IEnumerable, IEnumerator<Rule>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Rule _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public TaleData_Surroundings _003C_003E4__this;

		private RoomStatScoreStage _003Cimpressiveness_003E5__2;

		private RoomStatScoreStage _003Cbeauty_003E5__3;

		private RoomStatScoreStage _003Ccleanliness_003E5__4;

		Rule IEnumerator<Rule>.Current
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
		public _003CGetRules_003Ed__12(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cimpressiveness_003E5__2 = null;
			_003Cbeauty_003E5__3 = null;
			_003Ccleanliness_003E5__4 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			TaleData_Surroundings taleData_Surroundings = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (taleData_Surroundings.biome == null)
				{
					_003C_003E2__current = new Rule_String("BIOME", Find.WorldGrid[taleData_Surroundings.tile].PrimaryBiome.label);
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E2__current = new Rule_String("BIOME", taleData_Surroundings.biome.label);
				_003C_003E1__state = 2;
				return true;
			case 1:
				_003C_003E1__state = -1;
				goto IL_00bc;
			case 2:
				_003C_003E1__state = -1;
				goto IL_00bc;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String("ROOM_roleDefinite", Find.ActiveLanguageWorker.WithDefiniteArticle(taleData_Surroundings.roomRole.label));
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String("ROOM_roleIndefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(taleData_Surroundings.roomRole.label));
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003Cimpressiveness_003E5__2 = RoomStatDefOf.Impressiveness.GetScoreStage(taleData_Surroundings.roomImpressiveness);
				_003Cbeauty_003E5__3 = RoomStatDefOf.Beauty.GetScoreStage(taleData_Surroundings.roomBeauty);
				_003Ccleanliness_003E5__4 = RoomStatDefOf.Cleanliness.GetScoreStage(taleData_Surroundings.roomCleanliness);
				_003C_003E2__current = new Rule_String("ROOM_impressiveness", _003Cimpressiveness_003E5__2.label);
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String("ROOM_impressivenessIndefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(_003Cimpressiveness_003E5__2.label));
				_003C_003E1__state = 7;
				return true;
			case 7:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String("ROOM_beauty", _003Cbeauty_003E5__3.label);
				_003C_003E1__state = 8;
				return true;
			case 8:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String("ROOM_beautyIndefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(_003Cbeauty_003E5__3.label));
				_003C_003E1__state = 9;
				return true;
			case 9:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String("ROOM_cleanliness", _003Ccleanliness_003E5__4.label);
				_003C_003E1__state = 10;
				return true;
			case 10:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String("ROOM_cleanlinessIndefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(_003Ccleanliness_003E5__4.label));
				_003C_003E1__state = 11;
				return true;
			case 11:
				{
					_003C_003E1__state = -1;
					_003Cimpressiveness_003E5__2 = null;
					_003Cbeauty_003E5__3 = null;
					_003Ccleanliness_003E5__4 = null;
					break;
				}
				IL_00bc:
				if (taleData_Surroundings.roomRole != null && taleData_Surroundings.roomRole != RoomRoleDefOf.None)
				{
					_003C_003E2__current = new Rule_String("ROOM_role", taleData_Surroundings.roomRole.label);
					_003C_003E1__state = 3;
					return true;
				}
				break;
			}
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
		IEnumerator<Rule> IEnumerable<Rule>.GetEnumerator()
		{
			_003CGetRules_003Ed__12 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetRules_003Ed__12(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Rule>)this).GetEnumerator();
		}
	}

	public PlanetTile tile;

	public BiomeDef biome;

	public float temperature;

	public float snowDepth;

	public WeatherDef weather;

	public RoomRoleDef roomRole;

	public float roomImpressiveness;

	public float roomBeauty;

	public float roomCleanliness;

	public bool Outdoors => weather != null;

	public override void ExposeData()
	{
		Scribe_Values.Look(ref tile, "tile");
		Scribe_Values.Look(ref temperature, "temperature", 0f);
		Scribe_Values.Look(ref snowDepth, "snowDepth", 0f);
		Scribe_Defs.Look(ref weather, "weather");
		Scribe_Defs.Look(ref roomRole, "roomRole");
		Scribe_Defs.Look(ref biome, "biome");
		Scribe_Values.Look(ref roomImpressiveness, "roomImpressiveness", 0f);
		Scribe_Values.Look(ref roomBeauty, "roomBeauty", 0f);
		Scribe_Values.Look(ref roomCleanliness, "roomCleanliness", 0f);
	}

	[IteratorStateMachine(typeof(_003CGetRules_003Ed__12))]
	public override IEnumerable<Rule> GetRules(Dictionary<string, string> constants = null)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetRules_003Ed__12(-2)
		{
			_003C_003E4__this = this
		};
	}

	public static TaleData_Surroundings GenerateFrom(IntVec3 c, Map map)
	{
		TaleData_Surroundings taleData_Surroundings = new TaleData_Surroundings
		{
			tile = map.Tile,
			biome = map.Biome
		};
		Room roomOrAdjacent = c.GetRoomOrAdjacent(map, RegionType.Set_All);
		if (roomOrAdjacent != null)
		{
			if (roomOrAdjacent.PsychologicallyOutdoors)
			{
				taleData_Surroundings.weather = map.weatherManager.CurWeatherPerceived;
			}
			taleData_Surroundings.roomRole = roomOrAdjacent.Role;
			taleData_Surroundings.roomImpressiveness = roomOrAdjacent.GetStat(RoomStatDefOf.Impressiveness);
			taleData_Surroundings.roomBeauty = roomOrAdjacent.GetStat(RoomStatDefOf.Beauty);
			taleData_Surroundings.roomCleanliness = roomOrAdjacent.GetStat(RoomStatDefOf.Cleanliness);
		}
		if (!GenTemperature.TryGetTemperatureForCell(c, map, out taleData_Surroundings.temperature))
		{
			taleData_Surroundings.temperature = 21f;
		}
		taleData_Surroundings.snowDepth = map.snowGrid.GetDepth(c);
		return taleData_Surroundings;
	}

	public static TaleData_Surroundings GenerateRandom(Map map)
	{
		return GenerateFrom(CellFinder.RandomCell(map), map);
	}
}
