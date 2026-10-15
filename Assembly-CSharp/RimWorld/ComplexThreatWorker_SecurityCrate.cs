using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class ComplexThreatWorker_SecurityCrate : ComplexThreatWorker
{
	[CompilerGenerated]
	private sealed class _003Cget_SubThreats_003Ed__1 : IEnumerable<ComplexThreatDef>, IEnumerable, IEnumerator<ComplexThreatDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ComplexThreatDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		ComplexThreatDef IEnumerator<ComplexThreatDef>.Current
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
		public _003Cget_SubThreats_003Ed__1(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = ComplexThreatDefOf.SleepingInsects;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = ComplexThreatDefOf.SleepingMechanoids;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = ComplexThreatDefOf.Infestation;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = ComplexThreatDefOf.CryptosleepPods;
				_003C_003E1__state = 4;
				return true;
			case 4:
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
		IEnumerator<ComplexThreatDef> IEnumerable<ComplexThreatDef>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003Cget_SubThreats_003Ed__1(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ComplexThreatDef>)this).GetEnumerator();
		}
	}

	public IEnumerable<ComplexThreatDef> SubThreats
	{
		[IteratorStateMachine(typeof(_003Cget_SubThreats_003Ed__1))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_SubThreats_003Ed__1(-2);
		}
	}

	protected override bool CanResolveInt(ComplexResolveParams parms)
	{
		if (base.CanResolveInt(parms) && ComplexUtility.TryFindRandomSpawnCell(ThingDefOf.AncientSecurityCrate, parms.room, parms.map, out var _, 1, null))
		{
			return SubThreats.Any((ComplexThreatDef st) => st.Worker.CanResolve(parms));
		}
		return false;
	}

	protected override void ResolveInt(ComplexResolveParams parms, ref float threatPointsUsed, List<Thing> outSpawnedThings)
	{
		ComplexUtility.TryFindRandomSpawnCell(ThingDefOf.AncientSecurityCrate, parms.room, parms.map, out var spawnPosition, 1, null);
		Building_Crate building_Crate = (Building_Crate)GenSpawn.Spawn(ThingDefOf.AncientSecurityCrate, spawnPosition, parms.map);
		List<Thing> list = ThingSetMakerDefOf.MapGen_AncientComplex_SecurityCrate.root.Generate(default(ThingSetMakerParams));
		for (int num = list.Count - 1; num >= 0; num--)
		{
			if (!building_Crate.TryAcceptThing(list[num], allowSpecialEffects: false))
			{
				list[num].Destroy();
			}
		}
		outSpawnedThings.Add(building_Crate);
		parms.spawnedThings.Add(building_Crate);
		if (building_Crate.openedSignal.NullOrEmpty())
		{
			building_Crate.openedSignal = "CrateOpened" + Find.UniqueIDsManager.GetNextSignalTagID();
		}
		parms.triggerSignal = building_Crate.openedSignal;
		if (SubThreats.Where((ComplexThreatDef st) => st.Worker.CanResolve(parms)).TryRandomElement(out var result))
		{
			float threatPointsUsed2 = 0f;
			result.Worker.Resolve(parms, ref threatPointsUsed2, outSpawnedThings);
			threatPointsUsed += threatPointsUsed2;
		}
		else
		{
			Log.Warning("Failed to find a viable subthreat when placing a security crate threat.");
		}
	}
}
