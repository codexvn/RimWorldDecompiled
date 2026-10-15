using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;

namespace RimWorld;

public class CompLongRangeMineralScanner : CompScanner
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass7_0
	{
		public ThingDef localD;

		internal void _003CCompGetGizmosExtra_003Eb__1()
		{
			foreach (object selectedObject in Find.Selector.SelectedObjects)
			{
				if (selectedObject is Thing thing)
				{
					CompLongRangeMineralScanner compLongRangeMineralScanner = thing.TryGetComp<CompLongRangeMineralScanner>();
					if (compLongRangeMineralScanner != null)
					{
						compLongRangeMineralScanner.targetMineable = localD;
					}
				}
			}
		}

		internal bool _003CCompGetGizmosExtra_003Eb__2(Rect rect)
		{
			return Widgets.InfoCardButton(rect.x + 5f, rect.y + (rect.height - 24f) / 2f, localD.building.mineableThing);
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__7 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompLongRangeMineralScanner _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__7(int _003C_003E1__state)
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
				CompLongRangeMineralScanner compLongRangeMineralScanner = _003C_003E4__this;
				ThingDef mineableThing;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((CompScanner)compLongRangeMineralScanner).CompGetGizmosExtra().GetEnumerator();
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
					if (compLongRangeMineralScanner.parent.Faction != Faction.OfPlayer)
					{
						break;
					}
					mineableThing = compLongRangeMineralScanner.targetMineable.building.mineableThing;
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "CommandSelectMineralToScanFor".Translate() + ": " + mineableThing.LabelCap,
						defaultDesc = "CommandSelectMineralToScanForDesc".Translate(),
						icon = mineableThing.uiIcon,
						iconAngle = mineableThing.uiIconAngle,
						iconOffset = mineableThing.uiIconOffset,
						action = delegate
						{
							List<ThingDef> mineables = ((GenStep_PreciousLump)GenStepDefOf.PreciousLump.genStep).mineables;
							List<FloatMenuOption> list = new List<FloatMenuOption>();
							foreach (ThingDef item2 in mineables)
							{
								_003C_003Ec__DisplayClass7_0 CS_0024_003C_003E8__locals3 = new _003C_003Ec__DisplayClass7_0
								{
									localD = item2
								};
								FloatMenuOption item = new FloatMenuOption(CS_0024_003C_003E8__locals3.localD.building.mineableThing.LabelCap, delegate
								{
									foreach (object selectedObject in Find.Selector.SelectedObjects)
									{
										if (selectedObject is Thing thing)
										{
											CompLongRangeMineralScanner compLongRangeMineralScanner2 = thing.TryGetComp<CompLongRangeMineralScanner>();
											if (compLongRangeMineralScanner2 != null)
											{
												compLongRangeMineralScanner2.targetMineable = CS_0024_003C_003E8__locals3.localD;
											}
										}
									}
								}, MenuOptionPriority.Default, null, null, 29f, (Rect rect) => Widgets.InfoCardButton(rect.x + 5f, rect.y + (rect.height - 24f) / 2f, CS_0024_003C_003E8__locals3.localD.building.mineableThing));
								list.Add(item);
							}
							Find.WindowStack.Add(new FloatMenu(list));
						}
					};
					_003C_003E1__state = 2;
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__7 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__7(0)
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

	private ThingDef targetMineable;

	public new CompProperties_LongRangeMineralScanner Props => props as CompProperties_LongRangeMineralScanner;

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Defs.Look(ref targetMineable, "targetMineable");
		if (Scribe.mode == LoadSaveMode.PostLoadInit && targetMineable == null)
		{
			SetDefaultTargetMineral();
		}
	}

	public override void Initialize(CompProperties props)
	{
		base.Initialize(props);
		SetDefaultTargetMineral();
	}

	private void SetDefaultTargetMineral()
	{
		targetMineable = ThingDefOf.MineableGold;
	}

	protected override void DoFind(Pawn worker)
	{
		Slate slate = new Slate();
		slate.Set("map", parent.Map);
		slate.Set("targetMineable", targetMineable);
		slate.Set("targetMineableThing", targetMineable.building.mineableThing);
		slate.Set("worker", worker);
		if (QuestScriptDefOf.LongRangeMineralScannerLump.CanRun(slate, parent.Map))
		{
			Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(QuestScriptDefOf.LongRangeMineralScannerLump, slate);
			Find.LetterStack.ReceiveLetter(quest.name, quest.description, LetterDefOf.PositiveEvent, null, null, quest);
		}
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__7))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__7(-2)
		{
			_003C_003E4__this = this
		};
	}
}
