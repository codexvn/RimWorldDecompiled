using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

public class Building_BioferriteHarvester : Building
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__31 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_BioferriteHarvester _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

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
		public _003CGetGizmos_003Ed__31(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Building_BioferriteHarvester CS_0024_003C_003E8__locals9 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Building)CS_0024_003C_003E8__locals9).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007c;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007c;
				case 2:
					_003C_003E1__state = -1;
					if (CS_0024_003C_003E8__locals9.containedBioferrite >= 1f)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "BioferriteHarvesterEjectContents".Translate(),
							defaultDesc = "BioferriteHarvesterEjectContentsDesc".Translate(Find.ActiveLanguageWorker.Pluralize(ThingDefOf.Bioferrite.label)),
							action = delegate
							{
								CS_0024_003C_003E8__locals9.EjectContents();
							},
							Disabled = (CS_0024_003C_003E8__locals9.containedBioferrite == 0f),
							activateSound = SoundDefOf.Tick_Tiny,
							icon = ContentFinder<Texture2D>.Get("UI/Commands/EjectBioferrite")
						};
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_01de;
				case 3:
					_003C_003E1__state = -1;
					goto IL_01de;
				case 4:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_007c:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E2__current = new Command_Toggle
					{
						defaultLabel = "BioferriteHarvesterToggleUnloading".Translate(),
						defaultDesc = "BioferriteHarvesterToggleUnloadingDesc".Translate(),
						isActive = () => CS_0024_003C_003E8__locals9.unloadingEnabled,
						toggleAction = delegate
						{
							CS_0024_003C_003E8__locals9.unloadingEnabled = !CS_0024_003C_003E8__locals9.unloadingEnabled;
						},
						activateSound = SoundDefOf.Tick_Tiny,
						icon = ContentFinder<Texture2D>.Get("UI/Commands/BioferriteUnloading")
					};
					_003C_003E1__state = 2;
					return true;
					IL_01de:
					if (DebugSettings.ShowDevGizmos)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Add +1 bioferrite",
							action = delegate
							{
								CS_0024_003C_003E8__locals9.containedBioferrite = Mathf.Min(CS_0024_003C_003E8__locals9.containedBioferrite + 1f, 60f);
							}
						};
						_003C_003E1__state = 4;
						return true;
					}
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
			_003CGetGizmos_003Ed__31 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__31(0)
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

	public float containedBioferrite;

	public bool unloadingEnabled = true;

	private bool initalized;

	private const int HaulingThreshold = 30;

	private const float MaxCapacity = 60f;

	private CompFacility facilityComp;

	private CompPowerTrader powerComp;

	private CompCableConnection cableConnection;

	private Sustainer workingSustainer;

	public CompFacility FacilityComp => facilityComp ?? (facilityComp = GetComp<CompFacility>());

	public CompPowerTrader Power => powerComp ?? (powerComp = GetComp<CompPowerTrader>());

	public List<Thing> Platforms => FacilityComp.LinkedBuildings;

	public bool ReadyForHauling => Mathf.FloorToInt(containedBioferrite) >= 30;

	private float BioferritePerDay
	{
		get
		{
			if (!Power.PowerOn)
			{
				return 0f;
			}
			float num = 0f;
			foreach (Thing platform in Platforms)
			{
				if (platform is Building_HoldingPlatform { Occupied: not false } building_HoldingPlatform)
				{
					num += CompProducesBioferrite.BioferritePerDay(building_HoldingPlatform.HeldPawn);
				}
			}
			return num;
		}
	}

	public CompCableConnection CableConnection => cableConnection ?? (cableConnection = GetComp<CompCableConnection>());

	public override void SpawnSetup(Map map, bool respawningAfterLoad)
	{
		base.SpawnSetup(map, respawningAfterLoad);
		if (!respawningAfterLoad)
		{
			Initialize();
		}
	}

	public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
	{
		base.DeSpawn(mode);
		FacilityComp.OnLinkAdded -= OnLinkAdded;
		FacilityComp.OnLinkRemoved -= OnLinkRemoved;
		initalized = false;
		workingSustainer?.End();
		workingSustainer = null;
	}

	private void Initialize()
	{
		if (initalized)
		{
			return;
		}
		initalized = true;
		FacilityComp.OnLinkAdded += OnLinkAdded;
		FacilityComp.OnLinkRemoved += OnLinkRemoved;
		foreach (Thing platform in Platforms)
		{
			if (platform is Building_HoldingPlatform building_HoldingPlatform)
			{
				building_HoldingPlatform.innerContainer.OnContentsChanged += RebuildCables;
			}
		}
		RebuildCables();
	}

	protected override void DrawAt(Vector3 drawLoc, bool flip = false)
	{
		base.DrawAt(drawLoc, flip);
		if (!initalized)
		{
			Initialize();
		}
	}

	private void OnLinkRemoved(CompFacility facility, Thing thing)
	{
		if (thing is Building_HoldingPlatform building_HoldingPlatform)
		{
			building_HoldingPlatform.innerContainer.OnContentsChanged -= RebuildCables;
			RebuildCables();
		}
	}

	private void OnLinkAdded(CompFacility facility, Thing thing)
	{
		if (thing is Building_HoldingPlatform building_HoldingPlatform)
		{
			building_HoldingPlatform.innerContainer.OnContentsChanged += RebuildCables;
			RebuildCables();
		}
	}

	protected override void Tick()
	{
		base.Tick();
		if (this.IsHashIntervalTick(250))
		{
			containedBioferrite = Mathf.Min(containedBioferrite + BioferritePerDay / 60000f * 250f, 60f);
		}
		if (IsWorking())
		{
			if (workingSustainer == null)
			{
				workingSustainer = SoundDefOf.BioferriteHarvester_Ambient.TrySpawnSustainer(SoundInfo.InMap(this));
			}
			workingSustainer.Maintain();
		}
		else
		{
			workingSustainer?.End();
			workingSustainer = null;
		}
	}

	public override bool IsWorking()
	{
		return BioferritePerDay != 0f;
	}

	public override string GetInspectString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(base.GetInspectString());
		if (stringBuilder.Length != 0)
		{
			stringBuilder.AppendLine();
		}
		stringBuilder.Append("BioferriteHarvesterContained".Translate());
		stringBuilder.Append($": {containedBioferrite:F2} / {60f.ToString()} (+{BioferritePerDay:F2} ");
		stringBuilder.Append($"{"BioferriteHarvesterPerDay".Translate()})");
		return stringBuilder.ToString();
	}

	public Thing TakeOutBioferrite()
	{
		int num = Mathf.FloorToInt(containedBioferrite);
		if (num == 0)
		{
			return null;
		}
		containedBioferrite -= num;
		Thing thing = ThingMaker.MakeThing(ThingDefOf.Bioferrite);
		thing.stackCount = num;
		return thing;
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__31))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__31(-2)
		{
			_003C_003E4__this = this
		};
	}

	private void EjectContents()
	{
		Thing thing = TakeOutBioferrite();
		if (thing != null)
		{
			GenPlace.TryPlaceThing(thing, base.Position, base.Map, ThingPlaceMode.Near, null, null, null);
		}
	}

	public override void Notify_DefsHotReloaded()
	{
		base.Notify_DefsHotReloaded();
		RebuildCables();
	}

	private void RebuildCables()
	{
		CableConnection.RebuildCables(Platforms, (Thing thing) => thing is Building_HoldingPlatform building_HoldingPlatform && building_HoldingPlatform.Occupied);
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref containedBioferrite, "containedBioferrite", 0f);
		Scribe_Values.Look(ref unloadingEnabled, "unloadingEnabled", defaultValue: false);
	}
}
