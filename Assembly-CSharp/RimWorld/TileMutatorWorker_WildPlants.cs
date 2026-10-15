using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public class TileMutatorWorker_WildPlants : TileMutatorWorker
{
	[CompilerGenerated]
	private sealed class _003CAdditionalWildPlants_003Ed__4 : IEnumerable<BiomePlantRecord>, IEnumerable, IEnumerator<BiomePlantRecord>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private BiomePlantRecord _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public TileMutatorWorker_WildPlants _003C_003E4__this;

		private PlanetTile tile;

		public PlanetTile _003C_003E3__tile;

		BiomePlantRecord IEnumerator<BiomePlantRecord>.Current
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
		public _003CAdditionalWildPlants_003Ed__4(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			TileMutatorWorker_WildPlants tileMutatorWorker_WildPlants = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!ModsConfig.OdysseyActive)
				{
					return false;
				}
				_003C_003E2__current = new BiomePlantRecord
				{
					plant = tileMutatorWorker_WildPlants.GetPlantKind(tile),
					commonality = 0.1f
				};
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				return false;
			}
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
		IEnumerator<BiomePlantRecord> IEnumerable<BiomePlantRecord>.GetEnumerator()
		{
			_003CAdditionalWildPlants_003Ed__4 _003CAdditionalWildPlants_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CAdditionalWildPlants_003Ed__ = this;
			}
			else
			{
				_003CAdditionalWildPlants_003Ed__ = new _003CAdditionalWildPlants_003Ed__4(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CAdditionalWildPlants_003Ed__.tile = _003C_003E3__tile;
			return _003CAdditionalWildPlants_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<BiomePlantRecord>)this).GetEnumerator();
		}
	}

	private const float PlantCommonality = 0.1f;

	public TileMutatorWorker_WildPlants(TileMutatorDef def)
		: base(def)
	{
	}

	public override string GetLabel(PlanetTile tile)
	{
		return "WildPlants".Translate(GetPlantKind(tile).label);
	}

	public override string GetDescription(PlanetTile tile)
	{
		return def.description.Formatted(GetPlantKind(tile).label);
	}

	[IteratorStateMachine(typeof(_003CAdditionalWildPlants_003Ed__4))]
	public override IEnumerable<BiomePlantRecord> AdditionalWildPlants(PlanetTile tile)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAdditionalWildPlants_003Ed__4(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__tile = tile
		};
	}

	private ThingDef GetPlantKind(PlanetTile tile)
	{
		Rand.PushState();
		Rand.Seed = tile.GetHashCode();
		ThingDef result = def.plantKinds.RandomElement();
		Rand.PopState();
		return result;
	}
}
