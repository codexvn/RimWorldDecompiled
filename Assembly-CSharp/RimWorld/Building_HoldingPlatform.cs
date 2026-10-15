using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

[StaticConstructorOnStartup]
public class Building_HoldingPlatform : Building, IThingHolderWithDrawnPawn, IThingHolder, IRoofCollapseAlert, ISearchableContents
{
	public struct Chain
	{
		public Vector3 from;

		public Vector3 to;

		public Graphic graphic;

		public Graphic baseFastenerGraphic;

		public Graphic targetFastenerGraphic;

		public float rotation;
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass71_0
	{
		public int delay;

		public Building_HoldingPlatform _003C_003E4__this;

		internal void _003CGetGizmos_003Eb__1()
		{
			_003C_003E4__this.debugEscapeTick = Find.TickManager.TicksGame + delay;
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetFloatMenuOptions_003Ed__67 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_HoldingPlatform _003C_003E4__this;

		private Pawn selPawn;

		public Pawn _003C_003E3__selPawn;

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
		public _003CGetFloatMenuOptions_003Ed__67(int _003C_003E1__state)
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
				Building_HoldingPlatform building_HoldingPlatform = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((ThingWithComps)building_HoldingPlatform).GetFloatMenuOptions(selPawn).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0078;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0078;
				case 2:
					{
						_003C_003E1__state = -4;
						goto IL_00e8;
					}
					IL_0078:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (!building_HoldingPlatform.Occupied)
					{
						break;
					}
					_003C_003E7__wrap1 = building_HoldingPlatform.HeldPawn.GetFloatMenuOptions(selPawn).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_00e8;
					IL_00e8:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
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
			_003CGetFloatMenuOptions_003Ed__67 _003CGetFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetFloatMenuOptions_003Ed__ = new _003CGetFloatMenuOptions_003Ed__67(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetFloatMenuOptions_003Ed__.selPawn = _003C_003E3__selPawn;
			return _003CGetFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__71 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_HoldingPlatform _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

		private IEnumerator<Thing> _003C_003E7__wrap2;

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
		public _003CGetGizmos_003Ed__71(int _003C_003E1__state)
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
			case -5:
			case 3:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			case -6:
			case 4:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally4();
				}
				break;
			case -7:
			case 5:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally5();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Building_HoldingPlatform CS_0024_003C_003E8__locals11 = _003C_003E4__this;
				CompActivity comp;
				CompHoldingPlatformTarget comp2;
				CompStudiable comp3;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Building)CS_0024_003C_003E8__locals11).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0084;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0084;
				case 2:
					_003C_003E1__state = -4;
					goto IL_00fb;
				case 3:
					_003C_003E1__state = -5;
					goto IL_0173;
				case 4:
					_003C_003E1__state = -6;
					goto IL_01eb;
				case 5:
					_003C_003E1__state = -7;
					goto IL_0256;
				case 6:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0084:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (CS_0024_003C_003E8__locals11.HeldPawn != null && CS_0024_003C_003E8__locals11.HeldPawn.TryGetComp<CompActivity>(out comp))
					{
						_003C_003E7__wrap1 = comp.CompGetGizmosExtra().GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_00fb;
					}
					goto IL_0115;
					IL_01eb:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 4;
						return true;
					}
					_003C_003Em__Finally4();
					_003C_003E7__wrap1 = null;
					goto IL_0205;
					IL_0173:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current3 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current3;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap1 = null;
					goto IL_018d;
					IL_00fb:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current4 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current4;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
					goto IL_0115;
					IL_018d:
					if (CS_0024_003C_003E8__locals11.HeldPawn != null && CS_0024_003C_003E8__locals11.HeldPawn.TryGetComp<CompHoldingPlatformTarget>(out comp2))
					{
						_003C_003E7__wrap1 = comp2.CompGetGizmosExtra().GetEnumerator();
						_003C_003E1__state = -6;
						goto IL_01eb;
					}
					goto IL_0205;
					IL_0205:
					_003C_003E7__wrap2 = ((IEnumerable<Thing>)CS_0024_003C_003E8__locals11.innerContainer).GetEnumerator();
					_003C_003E1__state = -7;
					goto IL_0256;
					IL_0115:
					if (CS_0024_003C_003E8__locals11.HeldPawn != null && CS_0024_003C_003E8__locals11.HeldPawn.TryGetComp<CompStudiable>(out comp3))
					{
						_003C_003E7__wrap1 = comp3.CompGetGizmosExtra().GetEnumerator();
						_003C_003E1__state = -5;
						goto IL_0173;
					}
					goto IL_018d;
					IL_0256:
					while (_003C_003E7__wrap2.MoveNext())
					{
						Thing current5 = _003C_003E7__wrap2.Current;
						Gizmo gizmo = Building.SelectContainedItemGizmo(CS_0024_003C_003E8__locals11, current5);
						if (gizmo != null)
						{
							_003C_003E2__current = gizmo;
							_003C_003E1__state = 5;
							return true;
						}
					}
					_003C_003Em__Finally5();
					_003C_003E7__wrap2 = null;
					if (!DebugSettings.ShowDevGizmos || CS_0024_003C_003E8__locals11.HeldPawn == null)
					{
						break;
					}
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Timed escape",
						action = delegate
						{
							List<FloatMenuOption> list = new List<FloatMenuOption>();
							for (int i = 1; i < 21; i++)
							{
								_003C_003Ec__DisplayClass71_0 CS_0024_003C_003E8__locals15 = new _003C_003Ec__DisplayClass71_0
								{
									_003C_003E4__this = CS_0024_003C_003E8__locals11,
									delay = i * 60
								};
								list.Add(new FloatMenuOption(CS_0024_003C_003E8__locals15.delay.TicksToSeconds() + "s", delegate
								{
									CS_0024_003C_003E8__locals15._003C_003E4__this.debugEscapeTick = Find.TickManager.TicksGame + CS_0024_003C_003E8__locals15.delay;
								}));
							}
							Find.WindowStack.Add(new FloatMenu(list));
						}
					};
					_003C_003E1__state = 6;
					return true;
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
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally4()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally5()
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__71 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__71(0)
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
	private sealed class _003CGetInspectTabs_003Ed__69 : IEnumerable<InspectTabBase>, IEnumerable, IEnumerator<InspectTabBase>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private InspectTabBase _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_HoldingPlatform _003C_003E4__this;

		private IEnumerator<InspectTabBase> _003C_003E7__wrap1;

		InspectTabBase IEnumerator<InspectTabBase>.Current
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
		public _003CGetInspectTabs_003Ed__69(int _003C_003E1__state)
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
				Building_HoldingPlatform building_HoldingPlatform = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Thing)building_HoldingPlatform).GetInspectTabs().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0072:
					if (_003C_003E7__wrap1.MoveNext())
					{
						InspectTabBase current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (building_HoldingPlatform.HeldPawn != null && building_HoldingPlatform.HeldPawn.def.inspectorTabs.Contains(typeof(ITab_StudyNotes)))
					{
						_003C_003E2__current = building_HoldingPlatform.HeldPawn.GetInspectTabs().FirstOrDefault((InspectTabBase tab) => tab is ITab_StudyNotes);
						_003C_003E1__state = 2;
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
		IEnumerator<InspectTabBase> IEnumerable<InspectTabBase>.GetEnumerator()
		{
			_003CGetInspectTabs_003Ed__69 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetInspectTabs_003Ed__69(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<InspectTabBase>)this).GetEnumerator();
		}
	}

	public ThingOwner innerContainer;

	private int lastDamaged;

	private Graphic chainsUntetheredGraphic;

	private List<Chain> chains;

	private CompAffectedByFacilities facilitiesComp;

	private CompAttachPoints attachPointsComp;

	private AttachPointTracker targetPoints;

	private List<Chain> defaultPointMapping;

	[Unsaved(false)]
	private int debugEscapeTick = -1;

	private int heldPawnStartTick = -1;

	private const float ChainsUntetheredYOffset = 0.05f;

	private const float ChainsTetheredYOffset = 0.13658537f;

	private const float LurchMTBTicks = 100f;

	private const float DamageMTBDays = 2f;

	private static readonly FloatRange Damage = new FloatRange(1f, 3f);

	private const float LungeAnimationChance = 0.25f;

	private Dictionary<AttachPointType, Vector3> platformPoints;

	public float HeldPawnDrawPos_Y => DrawPos.y + 0.03658537f;

	public float HeldPawnBodyAngle => base.Rotation.AsAngle;

	public PawnPosture HeldPawnPosture => PawnPosture.LayingOnGroundFaceUp;

	public Rot4 HeldPawnRotation => base.Rotation;

	public Vector3 PawnDrawOffset => new Vector3(0f, 0f, 0.15f);

	public Pawn HeldPawn => innerContainer.FirstOrDefault((Thing x) => x is Pawn) as Pawn;

	public bool Occupied => HeldPawn != null;

	public float AnimationAlpha => Mathf.Clamp01((float)(Find.TickManager.TicksGame - heldPawnStartTick) / 20f);

	private CompAffectedByFacilities FacilitiesComp => facilitiesComp ?? (facilitiesComp = GetComp<CompAffectedByFacilities>());

	private CompAttachPoints AttachPointsComp => attachPointsComp ?? (attachPointsComp = GetComp<CompAttachPoints>());

	public ThingOwner SearchableContents => innerContainer;

	private AttachPointTracker TargetPawnAttachPoints
	{
		get
		{
			if (targetPoints != null && targetPoints.ThingId != HeldPawn.ThingID)
			{
				targetPoints = null;
			}
			bool num = targetPoints == null;
			targetPoints = targetPoints ?? HeldPawn.TryGetComp<CompAttachPoints>()?.points;
			if (num)
			{
				foreach (HediffComp_AttachPoints hediffComp in HeldPawn.health.hediffSet.GetHediffComps<HediffComp_AttachPoints>())
				{
					if (hediffComp.Points != null)
					{
						if (targetPoints == null)
						{
							targetPoints = hediffComp.Points;
						}
						else
						{
							targetPoints.Add(hediffComp.Points);
						}
					}
				}
			}
			return targetPoints;
		}
	}

	public bool HasAttachedElectroharvester
	{
		get
		{
			foreach (Thing item in FacilitiesComp.LinkedFacilitiesListForReading)
			{
				CompPowerPlantElectroharvester compPowerPlantElectroharvester = item.TryGetComp<CompPowerPlantElectroharvester>();
				if (compPowerPlantElectroharvester != null && compPowerPlantElectroharvester.PowerOn)
				{
					return true;
				}
			}
			return false;
		}
	}

	public bool HasAttachedBioferriteHarvester
	{
		get
		{
			foreach (Thing item in FacilitiesComp.LinkedFacilitiesListForReading)
			{
				if (item is Building_BioferriteHarvester building_BioferriteHarvester && building_BioferriteHarvester.Power.PowerOn)
				{
					return true;
				}
			}
			return false;
		}
	}

	private Graphic ChainsUntetheredGraphic
	{
		get
		{
			if (chainsUntetheredGraphic == null)
			{
				chainsUntetheredGraphic = GraphicDatabase.Get<Graphic_Single>(PlatformProps.untetheredGraphicTexPath, ShaderDatabase.Cutout, def.graphicData.drawSize, Color.white);
			}
			return chainsUntetheredGraphic;
		}
	}

	private CompProperties_EntityHolderPlatform PlatformProps => GetComp<CompEntityHolderPlatform>().Props;

	public PawnDrawParms HeldPawnDrawParms
	{
		get
		{
			PawnDrawParms result = default(PawnDrawParms);
			result.pawn = HeldPawn;
			result.facing = HeldPawn.Rotation;
			result.rotDrawMode = RotDrawMode.Fresh;
			result.posture = HeldPawn.GetPosture();
			result.flags = PawnRenderFlags.Headgear | PawnRenderFlags.Clothes;
			result.tint = Color.white;
			return result;
		}
	}

	public List<Chain> DefaultPointMapping
	{
		get
		{
			if (defaultPointMapping == null)
			{
				defaultPointMapping = new List<Chain>();
				Vector3 worldPos = AttachPointsComp.points.GetWorldPos(AttachPointType.PlatformRestraint0);
				Vector3 worldPos2 = AttachPointsComp.points.GetWorldPos(AttachPointType.PlatformRestraint1);
				Vector3 worldPos3 = AttachPointsComp.points.GetWorldPos(AttachPointType.PlatformRestraint2);
				Vector3 worldPos4 = AttachPointsComp.points.GetWorldPos(AttachPointType.PlatformRestraint3);
				Vector2 vector = new Vector2(Vector3.Distance(worldPos, worldPos3), 1f);
				defaultPointMapping.Add(new Chain
				{
					from = worldPos,
					to = worldPos3,
					graphic = (GraphicDatabase.Get<Graphic_Tiling>(PlatformProps.tilingChainTexPath, ShaderTypeDefOf.Cutout.Shader, vector, Color.white) as Graphic_Tiling).WithTiling(vector),
					baseFastenerGraphic = GraphicDatabase.Get<Graphic_Single>(PlatformProps.baseChainFastenerTexPath, ShaderTypeDefOf.Cutout.Shader, Vector2.one, Color.white),
					targetFastenerGraphic = GraphicDatabase.Get<Graphic_Single>(PlatformProps.targetChainFastenerTexPath, ShaderTypeDefOf.Cutout.Shader, Vector2.one, Color.white),
					rotation = (worldPos3.WithY(0f) - worldPos.WithY(0f)).normalized.ToAngleFlat()
				});
				vector = new Vector2(Vector3.Distance(worldPos2, worldPos4), 1f);
				defaultPointMapping.Add(new Chain
				{
					from = worldPos2,
					to = worldPos4,
					graphic = (GraphicDatabase.Get<Graphic_Tiling>(PlatformProps.tilingChainTexPath, ShaderTypeDefOf.Cutout.Shader, vector, Color.white) as Graphic_Tiling).WithTiling(vector),
					baseFastenerGraphic = GraphicDatabase.Get<Graphic_Single>(PlatformProps.baseChainFastenerTexPath, ShaderTypeDefOf.Cutout.Shader, Vector2.one, Color.white),
					targetFastenerGraphic = GraphicDatabase.Get<Graphic_Single>(PlatformProps.targetChainFastenerTexPath, ShaderTypeDefOf.Cutout.Shader, Vector2.one, Color.white),
					rotation = (worldPos4.WithY(0f) - worldPos2.WithY(0f)).normalized.ToAngleFlat()
				});
			}
			return defaultPointMapping;
		}
	}

	public Building_HoldingPlatform()
	{
		innerContainer = new ThingOwner<Thing>(this);
	}

	public override void SpawnSetup(Map map, bool respawningAfterLoad)
	{
		if (!ModLister.CheckAnomaly("Holding platform"))
		{
			Destroy();
			return;
		}
		base.SpawnSetup(map, respawningAfterLoad);
		Find.StudyManager.UpdateStudiableCache(this, base.Map);
		Find.Anomaly.hasBuiltHoldingPlatform = true;
	}

	public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
	{
		if (!base.BeingTransportedOnGravship)
		{
			EjectContents();
		}
		platformPoints = null;
		base.DeSpawn(mode);
	}

	public void GetChildHolders(List<IThingHolder> outChildren)
	{
		ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
	}

	public ThingOwner GetDirectlyHeldThings()
	{
		return innerContainer;
	}

	private bool TryGetFirstColonistDirection(out Vector2 direction)
	{
		foreach (Thing item in GenRadial.RadialDistinctThingsAround(base.Position, base.Map, 4f, useCenter: false))
		{
			if (item is Pawn { IsColonist: not false } pawn)
			{
				direction = pawn.Position.ToVector2() - base.Position.ToVector2();
				return true;
			}
		}
		direction = Vector2.zero;
		return false;
	}

	protected override void Tick()
	{
		base.Tick();
		if (Occupied && chains == null && AttachPointsComp != null)
		{
			chains = ((TargetPawnAttachPoints != null) ? BuildTargetPointMapping() : DefaultPointMapping);
		}
		if (!Occupied && chains != null)
		{
			chains = null;
		}
		if (Occupied && HasAttachedElectroharvester && Rand.MTBEventOccurs(2f, 60000f, 1f))
		{
			HeldPawn.TakeDamage(new DamageInfo(DamageDefOf.ElectricalBurn, Damage.RandomInRange));
		}
		if (Occupied && Rand.MTBEventOccurs(100f, 1f, 1f))
		{
			UpdateAnimation();
		}
		if (debugEscapeTick > 0 && Find.TickManager.TicksGame == debugEscapeTick && HeldPawn != null)
		{
			HeldPawn.TryGetComp<CompHoldingPlatformTarget>()?.Escape(initiator: false);
		}
		if (heldPawnStartTick == -1 && HeldPawn != null)
		{
			heldPawnStartTick = Find.TickManager.TicksGame;
		}
		else if (HeldPawn == null)
		{
			heldPawnStartTick = -1;
		}
	}

	private void UpdateAnimation()
	{
		if (HeldPawn.TryGetComp<CompHoldingPlatformTarget>(out var comp) && (!comp.Props.hasAnimation || HeldPawn.health.Downed))
		{
			HeldPawn.Drawer.renderer.SetAnimation(null);
			return;
		}
		SoundDef soundDef = PlatformProps.entityLungeSoundLow;
		AnimationDef animationDef = AnimationDefOf.HoldingPlatformWiggleLight;
		if (TryGetFirstColonistDirection(out var direction))
		{
			if (TargetPawnAttachPoints != null && Rand.Chance(0.25f))
			{
				Vector2 vector = direction.normalized.Cardinalize();
				if (vector == Vector2.up)
				{
					animationDef = AnimationDefOf.HoldingPlatformLungeUp;
				}
				if (vector == Vector2.right)
				{
					animationDef = AnimationDefOf.HoldingPlatformLungeRight;
				}
				if (vector == Vector2.left)
				{
					animationDef = AnimationDefOf.HoldingPlatformLungeLeft;
				}
				if (vector == Vector2.down)
				{
					animationDef = AnimationDefOf.HoldingPlatformLungeDown;
				}
				soundDef = PlatformProps.entityLungeSoundHi;
			}
			else
			{
				animationDef = AnimationDefOf.HoldingPlatformWiggleIntense;
			}
		}
		if (HeldPawn.Drawer.renderer.CurAnimation != animationDef)
		{
			soundDef?.PlayOneShot(this);
			HeldPawn.Drawer.renderer.SetAnimation(animationDef);
		}
	}

	public List<Chain> BuildTargetPointMapping()
	{
		if (chains == null)
		{
			chains = new List<Chain>();
		}
		else
		{
			chains.Clear();
		}
		HeldPawn.Drawer.renderer.renderTree.GetRootTPRS(HeldPawnDrawParms, out var offset, out var _, out var rotation, out var _);
		Vector3 vector = DrawPos + PawnDrawOffset;
		Dictionary<AttachPointType, Vector3> dictionary = new Dictionary<AttachPointType, Vector3>();
		int num = 5;
		int num2 = 8;
		foreach (AttachPointType item in TargetPawnAttachPoints.PointTypes(num, num2))
		{
			Vector3 value = vector + rotation * (offset + TargetPawnAttachPoints.GetRotatedOffset(item, base.Rotation));
			dictionary.Add(item, value);
		}
		for (int i = num; i <= num2; i++)
		{
			Vector3 vector2 = GetPlatformPoints()[(AttachPointType)i];
			Vector3 vector3 = dictionary[(AttachPointType)i];
			Vector3 vector4 = Vector3.Lerp(vector2, vector3, AnimationAlpha);
			float x = Vector3.Distance(vector4, vector2);
			Vector2 vector5 = new Vector2(x, 1f);
			chains.Add(new Chain
			{
				from = vector2,
				to = vector4,
				graphic = (GraphicDatabase.Get<Graphic_Tiling>(PlatformProps.tilingChainTexPath, ShaderTypeDefOf.CutoutTiling.Shader, vector5, Color.white) as Graphic_Tiling).WithTiling(vector5),
				baseFastenerGraphic = GraphicDatabase.Get<Graphic_Single>(PlatformProps.baseChainFastenerTexPath, ShaderTypeDefOf.CutoutTiling.Shader, Vector2.one, Color.white),
				targetFastenerGraphic = GraphicDatabase.Get<Graphic_Single>(PlatformProps.targetChainFastenerTexPath, ShaderTypeDefOf.CutoutTiling.Shader, Vector2.one, Color.white),
				rotation = (vector3.WithY(0f) - vector2.WithY(0f)).normalized.ToAngleFlat()
			});
		}
		return chains;
	}

	private Dictionary<AttachPointType, Vector3> GetPlatformPoints()
	{
		if (platformPoints == null)
		{
			platformPoints = new Dictionary<AttachPointType, Vector3>();
			int min = 5;
			int max = 8;
			foreach (AttachPointType item in AttachPointsComp.points.PointTypes(min, max))
			{
				platformPoints.Add(item, AttachPointsComp.points.GetWorldPos(item));
			}
		}
		return platformPoints;
	}

	public override void DynamicDrawPhaseAt(DrawPhase phase, Vector3 drawLoc, bool flip = false)
	{
		base.DynamicDrawPhaseAt(phase, drawLoc, flip);
		Pawn heldPawn = HeldPawn;
		if (heldPawn != null)
		{
			Rot4 value = Rot4.South;
			if (heldPawn.IsShambler && heldPawn.RaceProps.Animal)
			{
				value = Rot4.East;
			}
			heldPawn.Drawer.renderer.DynamicDrawPhaseAt(phase, DrawPos + PawnDrawOffset, value, neverAimWeapon: true);
		}
	}

	protected override void DrawAt(Vector3 drawLoc, bool flip = false)
	{
		base.DrawAt(drawLoc, flip);
		if (HeldPawn != null)
		{
			DrawChains();
		}
		else
		{
			ChainsUntetheredGraphic.Draw(drawLoc + Vector3.up * 0.05f, base.Rotation, this);
		}
	}

	private void DrawChains()
	{
		if (chains == null)
		{
			return;
		}
		chains = ((TargetPawnAttachPoints != null) ? BuildTargetPointMapping() : DefaultPointMapping);
		Vector3 vector = Vector3.up * 0.13658537f;
		foreach (Chain chain in chains)
		{
			Vector3 v = (chain.from + chain.to) / 2f;
			chain.graphic.Draw(v.WithY(DrawPos.y) + vector, base.Rotation, this, chain.rotation + 180f);
			chain.targetFastenerGraphic.Draw(chain.to + 2f * vector, base.Rotation, this, chain.rotation + 90f);
			chain.baseFastenerGraphic.Draw(chain.from + 2f * vector, base.Rotation, this, chain.rotation + 90f);
		}
	}

	public void EjectContents()
	{
		defaultPointMapping = null;
		chains = null;
		HeldPawn?.Drawer.renderer.SetAnimation(null);
		HeldPawn?.GetComp<CompHoldingPlatformTarget>()?.Notify_ReleasedFromPlatform();
		innerContainer.TryDropAll(base.Position, base.Map, ThingPlaceMode.Near);
	}

	[IteratorStateMachine(typeof(_003CGetFloatMenuOptions_003Ed__67))]
	public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetFloatMenuOptions_003Ed__67(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__selPawn = selPawn
		};
	}

	public override string GetInspectString()
	{
		string text = base.GetInspectString();
		if (!text.NullOrEmpty())
		{
			text += "\n";
		}
		Pawn heldPawn = HeldPawn;
		if (heldPawn != null)
		{
			TaggedString ts = "HoldingThing".Translate() + ": " + heldPawn.NameShortColored.CapitalizeFirst();
			bool flag = this.SafelyContains(heldPawn);
			if (!flag)
			{
				ts += " (" + "HoldingPlatformRequiresStrength".Translate(StatDefOf.MinimumContainmentStrength.Worker.ValueToString(heldPawn.GetStatValue(StatDefOf.MinimumContainmentStrength), finalized: false)) + ")";
			}
			text += ts.Colorize(flag ? Color.white : ColorLibrary.RedReadable);
		}
		else
		{
			text += "HoldingThing".Translate() + ": " + "Nothing".Translate().CapitalizeFirst();
		}
		if (heldPawn != null && heldPawn.def.IsStudiable)
		{
			string inspectStringExtraFor = CompStudiable.GetInspectStringExtraFor(heldPawn);
			if (!inspectStringExtraFor.NullOrEmpty())
			{
				text = text + "\n" + inspectStringExtraFor;
			}
		}
		if (heldPawn != null && heldPawn.TryGetComp<CompProducesBioferrite>(out var comp))
		{
			string text2 = comp.CompInspectStringExtra();
			if (!text2.NullOrEmpty())
			{
				text = text + "\n" + text2;
			}
		}
		return text;
	}

	[IteratorStateMachine(typeof(_003CGetInspectTabs_003Ed__69))]
	public override IEnumerable<InspectTabBase> GetInspectTabs()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetInspectTabs_003Ed__69(-2)
		{
			_003C_003E4__this = this
		};
	}

	public void Notify_PawnDied(Pawn pawn, DamageInfo? dinfo)
	{
		if (pawn == HeldPawn)
		{
			innerContainer.TryDropAll(base.Position, base.Map, ThingPlaceMode.Near);
			if (!dinfo.HasValue || !dinfo.Value.Def.execution)
			{
				Messages.Message("EntityDiedOnHoldingPlatform".Translate(pawn), pawn, MessageTypeDefOf.NegativeEvent);
			}
		}
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__71))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__71(-2)
		{
			_003C_003E4__this = this
		};
	}

	public RoofCollapseResponse Notify_OnBeforeRoofCollapse()
	{
		if (!Occupied)
		{
			return RoofCollapseResponse.None;
		}
		if (HeldPawn is IRoofCollapseAlert roofCollapseAlert)
		{
			roofCollapseAlert.Notify_OnBeforeRoofCollapse();
		}
		foreach (IRoofCollapseAlert comp in HeldPawn.GetComps<IRoofCollapseAlert>())
		{
			comp.Notify_OnBeforeRoofCollapse();
		}
		return RoofCollapseResponse.None;
	}

	public override void Notify_DefsHotReloaded()
	{
		base.Notify_DefsHotReloaded();
		chains = null;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref lastDamaged, "lastDamaged", 0);
		Scribe_Deep.Look(ref innerContainer, "innerContainer", this);
		Scribe_Values.Look(ref heldPawnStartTick, "heldPawnStartTick", 0);
	}
}
