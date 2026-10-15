using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Utility;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

public class CompVoidNode : CompInteractable
{
	[CompilerGenerated]
	private sealed class _003CCompFloatMenuOptions_003Ed__18 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompVoidNode _003C_003E4__this;

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
		public _003CCompFloatMenuOptions_003Ed__18(int _003C_003E1__state)
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
				CompVoidNode compVoidNode = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (compVoidNode.activated)
					{
						return false;
					}
					_003C_003E7__wrap1 = ((CompInteractable)compVoidNode).CompFloatMenuOptions(selPawn).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					FloatMenuOption current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CCompFloatMenuOptions_003Ed__18 _003CCompFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCompFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CCompFloatMenuOptions_003Ed__ = new _003CCompFloatMenuOptions_003Ed__18(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CCompFloatMenuOptions_003Ed__.selPawn = _003C_003E3__selPawn;
			return _003CCompFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__19 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompVoidNode _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__19(int _003C_003E1__state)
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
				CompVoidNode compVoidNode = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (compVoidNode.activated)
					{
						return false;
					}
					_003C_003E7__wrap1 = ((CompInteractable)compVoidNode).CompGetGizmosExtra().GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					Gizmo current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__19 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__19(0)
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

	private const int SlowFade = 300;

	private const int QuickFade = 24;

	private const int MusicIntroLengthTicks = 260;

	private bool activated;

	private Pawn interactedPawn;

	private bool embracing;

	private int closeTick = -99999;

	private Sustainer ambientSustainer;

	private float AgeSecs()
	{
		return (float)(Find.TickManager.TicksGame - parent.TickSpawned) / 60f;
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		if (!ModLister.CheckAnomaly("Void node"))
		{
			parent.Destroy();
			return;
		}
		base.PostSpawnSetup(respawningAfterLoad);
		LongEventHandler.ExecuteWhenFinished(delegate
		{
			parent.Graphic.MatSingle.SetFloat(ShaderPropertyIDs.VoidNodeDestructionStartAge, float.MaxValue);
			ambientSustainer = SoundDefOf.VoidNode_Ambient.TrySpawnSustainer(SoundInfo.InMap(parent));
		});
	}

	public override void CompTick()
	{
		base.CompTick();
		if (closeTick <= 0)
		{
			return;
		}
		ambientSustainer.End();
		if (Find.TickManager.TicksGame + 260 == closeTick)
		{
			Find.MusicManagerPlay.ForcePlaySong(SongDefOf.AnomalyCreditsSong, ignorePrefsVolume: false);
		}
		if (Find.TickManager.TicksGame == closeTick)
		{
			Find.TickManager.Pause();
			Thing.allowDestroyNonDestroyable = true;
			parent.Destroy();
			Thing.allowDestroyNonDestroyable = false;
			CameraJumper.TryJump(Find.Anomaly.monolith, CameraJumper.MovementMode.Cut);
			if (embracing)
			{
				VoidAwakeningUtility.EmbraceTheVoid(interactedPawn);
			}
			else
			{
				VoidAwakeningUtility.DisruptTheLink(interactedPawn);
			}
		}
	}

	private void OpenDialog(Pawn pawn)
	{
		Dialog_NodeTree dialog_NodeTree = new Dialog_NodeTree(new DiaNode("VoidNodeLetter".Translate(pawn.Named("PAWN")))
		{
			options = 
			{
				new DiaOption("VoidNodeDisrupt".Translate())
				{
					action = DisruptNode,
					resolveTree = true
				},
				new DiaOption("VoidNodeEmbrace".Translate())
				{
					action = delegate
					{
						CloseNode(embraceVoid: true, 300, 0);
					},
					resolveTree = true
				},
				new DiaOption("VoidNodePostpone".Translate())
				{
					resolveTree = true
				}
			}
		});
		dialog_NodeTree.forcePause = true;
		Find.WindowStack.Add(dialog_NodeTree);
	}

	private void DisruptNode()
	{
		SoundDefOf.VoidNode_Explode.PlayOneShotOnCamera();
		EffecterDefOf.VoidNodeDisrupted.SpawnMaintained(parent, parent.Map);
		EffecterDefOf.VoidNodeDisruptedFlashes.SpawnMaintained(parent, parent.Map);
		parent.Graphic.MatSingle.SetFloat(ShaderPropertyIDs.VoidNodeDestructionStartAge, AgeSecs());
		CloseNode(embraceVoid: false, 24, 600);
	}

	private void CloseNode(bool embraceVoid, int fadeTimeTicks, int fadeDelayTicks)
	{
		closeTick = Find.TickManager.TicksGame + fadeTimeTicks + fadeDelayTicks;
		embracing = embraceVoid;
		activated = true;
		QuestUtility.SendQuestTargetSignals(parent.questTags, "NodeClosed");
		Find.TickManager.slower.SignalForceNormalSpeed();
		Find.MusicManagerPlay.ForceSilenceFor(999f);
		CameraJumper.TryJump(parent, CameraJumper.MovementMode.Cut);
		ScreenFader.StartFade(Color.white, (float)(fadeTimeTicks - 5) / 60f, (float)fadeDelayTicks / 60f);
	}

	protected override void OnInteracted(Pawn caster)
	{
		interactedPawn = caster;
		OpenDialog(caster);
	}

	public override AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
	{
		if (activated)
		{
			return false;
		}
		return base.CanInteract(activateBy, checkOptionalItems);
	}

	public void Reset()
	{
		activated = false;
	}

	public override string CompInspectStringExtra()
	{
		if (activated)
		{
			return "";
		}
		return base.CompInspectStringExtra();
	}

	[IteratorStateMachine(typeof(_003CCompFloatMenuOptions_003Ed__18))]
	public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompFloatMenuOptions_003Ed__18(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__selPawn = selPawn
		};
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__19))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__19(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref activated, "activated", defaultValue: false);
		Scribe_References.Look(ref interactedPawn, "interactedPawn");
		Scribe_Values.Look(ref embracing, "embracing", defaultValue: false);
		Scribe_Values.Look(ref closeTick, "closeTick", 0);
	}
}
