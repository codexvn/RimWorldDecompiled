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
public class CompBandNode : ThingComp
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass21_0
	{
		public Pawn pawn;

		internal bool _003CCompGetGizmosExtra_003Eb__4(CompBandNode n)
		{
			if (n.tunedTo != pawn)
			{
				return n.tuningTo != pawn;
			}
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass21_1
	{
		public IEnumerable<CompBandNode> bandNodes;

		public Pawn localPawn;

		internal void _003CCompGetGizmosExtra_003Eb__7()
		{
			foreach (CompBandNode item in bandNodes)
			{
				item.TuneTo(localPawn);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__21 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompBandNode _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__21(int _003C_003E1__state)
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
			CompBandNode CS_0024_003C_003E8__locals27 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				Command_Action command_Action = new Command_Action
				{
					defaultLabel = ((CS_0024_003C_003E8__locals27.tunedTo == null) ? ("BandNodeTuneTo".Translate() + "...") : ("BandNodeRetuneTo".Translate() + "...")),
					defaultDesc = ((CS_0024_003C_003E8__locals27.tunedTo == null) ? "BandNodeTuningDesc".Translate("PeriodSeconds".Translate(CS_0024_003C_003E8__locals27.Props.tuneSeconds)) : "BandNodeRetuningDesc".Translate(CS_0024_003C_003E8__locals27.Props.retuneDays + " " + "Days".Translate()))
				};
				command_Action.onHover = (Action)Delegate.Combine(command_Action.onHover, (Action)delegate
				{
					Pawn pawn = ((CS_0024_003C_003E8__locals27.tuningTo != null) ? CS_0024_003C_003E8__locals27.tuningTo : CS_0024_003C_003E8__locals27.tunedTo);
					if (pawn != null)
					{
						GenDraw.DrawLineBetween(CS_0024_003C_003E8__locals27.parent.DrawPos, pawn.DrawPos);
					}
				});
				bool flag = false;
				foreach (Pawn item in CS_0024_003C_003E8__locals27.parent.Map.mapPawns.AllPawnsSpawned)
				{
					if (MechanitorUtility.IsMechanitor(item) && CS_0024_003C_003E8__locals27.tunedTo != item && CS_0024_003C_003E8__locals27.tuningTo != item)
					{
						flag = true;
						break;
					}
				}
				command_Action.Disabled = !flag;
				command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/BandNodeTuning");
				command_Action.action = (Action)Delegate.Combine(command_Action.action, (Action)delegate
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					using (IEnumerator<Pawn> enumerator2 = CS_0024_003C_003E8__locals27.parent.Map.mapPawns.AllPawnsSpawned.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							_003C_003Ec__DisplayClass21_0 CS_0024_003C_003E8__locals22 = new _003C_003Ec__DisplayClass21_0
							{
								pawn = enumerator2.Current
							};
							if (MechanitorUtility.IsMechanitor(CS_0024_003C_003E8__locals22.pawn))
							{
								_003C_003Ec__DisplayClass21_1 CS_0024_003C_003E8__locals29 = new _003C_003Ec__DisplayClass21_1
								{
									bandNodes = from t in Find.Selector.SelectedObjects
										where t is Thing thing && thing.TryGetComp<CompBandNode>() != null
										select ((Thing)t).TryGetComp<CompBandNode>() into n
										where n.tunedTo != CS_0024_003C_003E8__locals22.pawn && n.tuningTo != CS_0024_003C_003E8__locals22.pawn
										select n
								};
								if (CS_0024_003C_003E8__locals29.bandNodes.Any())
								{
									CS_0024_003C_003E8__locals29.localPawn = CS_0024_003C_003E8__locals22.pawn;
									string text = CS_0024_003C_003E8__locals22.pawn.Name.ToStringFull;
									if (CS_0024_003C_003E8__locals29.bandNodes.All((CompBandNode b) => b.tunedTo == null) || CS_0024_003C_003E8__locals29.bandNodes.All((CompBandNode b) => b.tunedTo != null))
									{
										text = ((CS_0024_003C_003E8__locals27.tunedTo != null) ? (text + " (" + CS_0024_003C_003E8__locals27.RetuneTimeTicks.ToStringTicksToPeriod() + ")") : ((string)(text + (" (" + CS_0024_003C_003E8__locals27.Props.tuneSeconds + " " + "SecondsLower".Translate() + ")"))));
									}
									list.Add(new FloatMenuOption(text, delegate
									{
										foreach (CompBandNode item2 in CS_0024_003C_003E8__locals29.bandNodes)
										{
											item2.TuneTo(CS_0024_003C_003E8__locals29.localPawn);
										}
									}));
								}
							}
						}
					}
					Find.WindowStack.Add(new FloatMenu(list));
				});
				_003C_003E2__current = command_Action;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				if (DebugSettings.ShowDevGizmos)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: complete tuning",
						action = delegate
						{
							CS_0024_003C_003E8__locals27.tuningTimeLeft = 0;
						}
					};
					_003C_003E1__state = 2;
					return true;
				}
				break;
			case 2:
				_003C_003E1__state = -1;
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__21 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__21(0)
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

	private static readonly Material UntunedMaterial = SolidColorMaterials.SimpleSolidColorMaterial(Color.red);

	private static readonly Material TuningMaterial = SolidColorMaterials.SimpleSolidColorMaterial(Color.yellow);

	private static readonly Material TunedMaterial = SolidColorMaterials.SimpleSolidColorMaterial(Color.green);

	private static readonly Vector2 TuningBarSize = new Vector3(0.255f, 0.035f);

	private const float TuningBarYOffset = -0.4f;

	public Pawn tunedTo;

	public int tuningTimeLeft;

	public Pawn tuningTo;

	private Effecter effecter;

	private CompPowerTrader PowerTrader => parent.TryGetComp<CompPowerTrader>();

	public CompProperties_BandNode Props => (CompProperties_BandNode)props;

	private int RetuneTimeTicks => (int)(60000f * Props.retuneDays);

	private int TuningTimeTicks => (int)(Props.tuneSeconds * 60f);

	private BandNodeState State
	{
		get
		{
			if (tunedTo != null && tuningTo != null)
			{
				return BandNodeState.Retuning;
			}
			if (tuningTo != null)
			{
				return BandNodeState.Tuning;
			}
			if (tunedTo != null)
			{
				return BandNodeState.Tuned;
			}
			return BandNodeState.Untuned;
		}
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		if (!ModLister.CheckBiotech("Band node"))
		{
			parent.Destroy();
		}
		else
		{
			base.PostSpawnSetup(respawningAfterLoad);
		}
	}

	public override void PostExposeData()
	{
		Scribe_References.Look(ref tunedTo, "tunedTo");
		Scribe_References.Look(ref tuningTo, "tuningTo");
		Scribe_Values.Look(ref tuningTimeLeft, "tuningTimeLeft", 0);
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__21))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__21(-2)
		{
			_003C_003E4__this = this
		};
	}

	public void TuneTo(Pawn pawn)
	{
		tuningTimeLeft = ((tunedTo == null) ? TuningTimeTicks : RetuneTimeTicks);
		tuningTo = pawn;
	}

	public override void PostDraw()
	{
		base.PostDraw();
		Material material = State switch
		{
			BandNodeState.Tuned => TunedMaterial, 
			BandNodeState.Untuned => UntunedMaterial, 
			_ => TuningMaterial, 
		};
		Vector3 s = new Vector3(TuningBarSize.x, 1f, TuningBarSize.y);
		Vector3 pos = parent.DrawPos + new Vector3(0f, 0f, -0.4f);
		pos.y = parent.def.altitudeLayer.AltitudeFor() + 0.03658537f;
		Matrix4x4 matrix = default(Matrix4x4);
		matrix.SetTRS(pos, parent.Rotation.AsQuat, s);
		Graphics.DrawMesh(MeshPool.plane10, matrix, material, 0);
	}

	public override void CompTick()
	{
		PowerTrader.PowerOutput = ((tunedTo == null && tuningTo == null) ? ((float)(-Props.powerConsumptionIdle)) : (0f - PowerTrader.Props.PowerConsumption));
		if (tunedTo != null && tunedTo.Dead)
		{
			tunedTo = null;
		}
		if (tuningTo != null && tuningTo.Dead)
		{
			tuningTo = null;
		}
		if (PowerTrader != null && !PowerTrader.PowerOn)
		{
			effecter?.Cleanup();
			effecter = null;
			return;
		}
		if (tuningTo != null)
		{
			tuningTimeLeft--;
			if (tuningTimeLeft <= 0)
			{
				tunedTo = tuningTo;
				tuningTo = null;
				if (Props.tuningCompleteSound != null)
				{
					Props.tuningCompleteSound.PlayOneShot(parent);
				}
			}
		}
		if (tuningTo == null && tunedTo != null && !tunedTo.health.hediffSet.HasHediff(Props.hediff))
		{
			tunedTo.health.AddHediff(Props.hediff, tunedTo.health.hediffSet.GetBrain(), null);
		}
		if (State == BandNodeState.Untuned)
		{
			if (effecter == null || effecter.def != Props.untunedEffect)
			{
				effecter?.Cleanup();
				effecter = Props.untunedEffect.Spawn();
			}
		}
		else if (State == BandNodeState.Tuned)
		{
			if (effecter == null || effecter.def != Props.tunedEffect)
			{
				effecter?.Cleanup();
				effecter = Props.tunedEffect.Spawn();
			}
		}
		else if (State == BandNodeState.Tuning)
		{
			if (effecter == null || effecter.def != Props.tuningEffect)
			{
				effecter?.Cleanup();
				effecter = Props.tuningEffect.Spawn();
			}
		}
		else if (State == BandNodeState.Retuning)
		{
			if (effecter == null || effecter.def != Props.retuningEffect)
			{
				effecter?.Cleanup();
				effecter = Props.retuningEffect.Spawn();
			}
		}
		else
		{
			effecter?.Cleanup();
			effecter = null;
		}
		if (effecter != null)
		{
			effecter.EffectTick(parent, parent);
		}
	}

	public override string CompInspectStringExtra()
	{
		string text = null;
		if (!PowerTrader.PowerOn)
		{
			text = "\n" + "Unpowered".Translate().CapitalizeFirst().Resolve();
		}
		if (tuningTo != null)
		{
			return "BandNodeTuningTo".Translate() + ": " + tuningTo.Name.ToStringFull + " - " + tuningTimeLeft.ToStringTicksToPeriod() + text;
		}
		return "BandNodeTunedTo".Translate() + ": " + ((tunedTo == null) ? "Nobody".Translate().Resolve() : tunedTo.Name.ToStringFull) + text;
	}
}
