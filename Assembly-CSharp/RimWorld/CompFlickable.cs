using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

public class CompFlickable : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__20 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompFlickable _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__20(int _003C_003E1__state)
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
				CompFlickable CS_0024_003C_003E8__locals9 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((ThingComp)CS_0024_003C_003E8__locals9).CompGetGizmosExtra().GetEnumerator();
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
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (CS_0024_003C_003E8__locals9.parent.Faction == Faction.OfPlayer)
					{
						_003C_003E2__current = new Command_Toggle
						{
							hotKey = KeyBindingDefOf.Command_TogglePower,
							icon = CS_0024_003C_003E8__locals9.CommandTex,
							defaultLabel = CS_0024_003C_003E8__locals9.Props.commandLabelKey.Translate(),
							defaultDesc = CS_0024_003C_003E8__locals9.Props.commandDescKey.Translate(),
							isActive = () => CS_0024_003C_003E8__locals9.wantSwitchOn,
							toggleAction = delegate
							{
								CS_0024_003C_003E8__locals9.wantSwitchOn = !CS_0024_003C_003E8__locals9.wantSwitchOn;
								FlickUtility.UpdateFlickDesignation(CS_0024_003C_003E8__locals9.parent);
							}
						};
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__20 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__20(0)
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

	private bool switchOnInt = true;

	private bool wantSwitchOn = true;

	private Graphic offGraphic;

	private Texture2D cachedCommandTex;

	private const string OffGraphicSuffix = "_Off";

	public const string FlickedOnSignal = "FlickedOn";

	public const string FlickedOffSignal = "FlickedOff";

	private CompProperties_Flickable Props => (CompProperties_Flickable)props;

	private Texture2D CommandTex
	{
		get
		{
			if (cachedCommandTex == null)
			{
				cachedCommandTex = ContentFinder<Texture2D>.Get(Props.commandTexture);
			}
			return cachedCommandTex;
		}
	}

	public bool SwitchIsOn
	{
		get
		{
			return switchOnInt;
		}
		set
		{
			if (switchOnInt != value)
			{
				switchOnInt = value;
				if (switchOnInt)
				{
					parent.BroadcastCompSignal("FlickedOn");
				}
				else
				{
					parent.BroadcastCompSignal("FlickedOff");
				}
				if (parent.Spawned)
				{
					parent.Map.mapDrawer.MapMeshDirty(parent.Position, (ulong)MapMeshFlagDefOf.Buildings | (ulong)MapMeshFlagDefOf.Things);
				}
			}
		}
	}

	public Graphic CurrentGraphic
	{
		get
		{
			if (SwitchIsOn)
			{
				return parent.DefaultGraphic;
			}
			if (offGraphic == null)
			{
				offGraphic = GraphicDatabase.Get(parent.def.graphicData.graphicClass, parent.def.graphicData.texPath + "_Off", parent.def.graphicData.shaderType.Shader, parent.def.graphicData.drawSize, parent.DrawColor, parent.DrawColorTwo);
			}
			return offGraphic;
		}
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref switchOnInt, "switchOn", defaultValue: true);
		Scribe_Values.Look(ref wantSwitchOn, "wantSwitchOn", defaultValue: true);
	}

	public bool WantsFlick()
	{
		return wantSwitchOn != switchOnInt;
	}

	public void DoFlick()
	{
		SwitchIsOn = !SwitchIsOn;
		SoundDefOf.FlickSwitch.PlayOneShot(new TargetInfo(parent.Position, parent.Map));
	}

	public void ResetToOn()
	{
		switchOnInt = true;
		wantSwitchOn = true;
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__20))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__20(-2)
		{
			_003C_003E4__this = this
		};
	}
}
