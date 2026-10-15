using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

public abstract class CompAncientVent : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__16 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompAncientVent _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__16(int _003C_003E1__state)
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
			CompAncientVent @object = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!DebugSettings.ShowDevGizmos)
				{
					return false;
				}
				_003C_003E2__current = new Command_Action
				{
					action = @object.DevToggleVent,
					defaultLabel = "DEV: Toggle vents on map",
					defaultDesc = "Toggles all vents on the map on or off"
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__16 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__16(0)
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

	private static Dictionary<int, int> sharedLastCheckMtbTickPerMap = new Dictionary<int, int>();

	private static Dictionary<int, bool> sharedLastMtbCheckDidOccurPerMap = new Dictionary<int, bool>();

	private const int PushHeatInterval = 20;

	private bool ventOn;

	private Sustainer sustainer;

	public CompProperties_AncientVentEmitter Props => (CompProperties_AncientVentEmitter)props;

	protected virtual bool AppliesEffectsToPawns => false;

	public bool VentOn => ventOn;

	public override void CompTick()
	{
		base.CompTick();
		if (parent.Destroyed)
		{
			return;
		}
		if (CheckShouldToggleAndHandleMapState())
		{
			ventOn = !ventOn;
			ToggleIndividualVent(ventOn);
		}
		if (!ventOn)
		{
			sustainer?.End();
			sustainer = null;
			return;
		}
		if (sustainer == null)
		{
			sustainer = Props.activeSound.TrySpawnSustainer(SoundInfo.InMap(parent, MaintenanceType.PerTick));
		}
		sustainer.Maintain();
		if (parent.IsHashIntervalTick(20) && Props.heatToPush != 0)
		{
			GenTemperature.PushHeat(parent, Props.heatToPush);
		}
		if (!AppliesEffectsToPawns)
		{
			return;
		}
		Map map = parent.Map;
		IntVec3 position = parent.Position;
		int num = Mathf.FloorToInt((float)parent.def.size.x / 2f);
		for (int i = 0; i < map.mapPawns.AllPawnsSpawned.Count; i++)
		{
			Pawn pawn = map.mapPawns.AllPawnsSpawned[i];
			if (!(pawn.Position.DistanceTo(position) > (float)num))
			{
				ApplyPawnEffect(pawn, 1);
			}
		}
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref ventOn, "ventOn", defaultValue: false);
	}

	private bool CheckShouldToggleAndHandleMapState()
	{
		bool value2;
		if ((sharedLastCheckMtbTickPerMap.TryGetValue(parent.Map.uniqueID, out var value) ? value : 0) == GenTicks.TicksGame)
		{
			return sharedLastMtbCheckDidOccurPerMap.TryGetValue(parent.Map.uniqueID, out value2) && value2;
		}
		int num = (ventOn ? Props.onDurationMtbDays : Props.offDurationMtbDays);
		sharedLastCheckMtbTickPerMap[parent.Map.uniqueID] = GenTicks.TicksGame;
		bool flag2 = (sharedLastMtbCheckDidOccurPerMap[parent.Map.uniqueID] = Rand.MTBEventOccurs(num, 60000f, 1f));
		if (flag2)
		{
			ToggleMapState();
		}
		return flag2;
	}

	private void ToggleMapState()
	{
		if (ventOn)
		{
			if (Props.conditionToCause != null)
			{
				parent.Map.gameConditionManager.GetActiveCondition(Props.conditionToCause)?.End();
			}
			SendToggleAlert(goingToBeOn: false);
		}
		else if (Props.conditionToCause != null)
		{
			if (!parent.Map.gameConditionManager.ConditionIsActive(Props.conditionToCause))
			{
				GameCondition gameCondition = GameConditionMaker.MakeConditionPermanent(Props.conditionToCause);
				gameCondition.suppressEndMessage = true;
				gameCondition.conditionCauser = parent;
				gameCondition.forceDisplayAsDuration = true;
				parent.Map.gameConditionManager.RegisterCondition(gameCondition);
			}
			SendToggleAlert(goingToBeOn: true);
		}
	}

	public override string CompInspectStringExtra()
	{
		if (!ventOn)
		{
			return "DormantCompInactive".Translate();
		}
		return Props.activeInspectStringKey.Translate().CapitalizeFirst();
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__16))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__16(-2)
		{
			_003C_003E4__this = this
		};
	}

	private void DevToggleVent()
	{
		ToggleMapState();
		bool on = !ventOn;
		foreach (Building item in parent.Map.listerBuildings.AllBuildingsNonColonistOfDef(parent.def))
		{
			CompAncientVent compAncientVent = item.TryGetComp<CompAncientVent>();
			if (compAncientVent != null)
			{
				compAncientVent.ventOn = on;
				compAncientVent.ToggleIndividualVent(on);
			}
		}
	}

	protected void SendToggleAlert(bool goingToBeOn)
	{
		Messages.Message((goingToBeOn ? Props.startupMessageKey : Props.shutdownMessageKey).Translate(), new LookTargets(parent.Map.listerBuildings.AllBuildingsNonColonistOfDef(parent.def)), MessageTypeDefOf.NeutralEvent, historical: false);
	}

	protected abstract void ToggleIndividualVent(bool on);

	protected virtual void ApplyPawnEffect(Pawn pawn, int delta)
	{
	}
}
