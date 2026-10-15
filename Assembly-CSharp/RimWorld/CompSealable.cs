using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class CompSealable : ThingComp
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass12_0
	{
		public LocalTargetInfo target;

		public CompSealable _003C_003E4__this;

		internal void _003CCompGetGizmosExtra_003Eb__7()
		{
			target.Pawn?.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Seal, _003C_003E4__this.Portal), JobTag.Misc);
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__12 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompSealable _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__12(int _003C_003E1__state)
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
			CompSealable CS_0024_003C_003E8__locals15 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!CS_0024_003C_003E8__locals15.Portal.PocketMapExists)
				{
					return false;
				}
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = CS_0024_003C_003E8__locals15.Props.sealCommandLabel + "...",
					defaultDesc = CS_0024_003C_003E8__locals15.Props.sealCommandDesc,
					icon = CS_0024_003C_003E8__locals15.SealTex,
					action = delegate
					{
						Find.Targeter.BeginTargeting(TargetingParameters.ForColonist(), delegate(LocalTargetInfo target)
						{
							_003C_003Ec__DisplayClass12_0 CS_0024_003C_003E8__locals10 = new _003C_003Ec__DisplayClass12_0
							{
								_003C_003E4__this = CS_0024_003C_003E8__locals15,
								target = target
							};
							string text = "";
							List<Pawn> list = new List<Pawn>();
							List<Pawn> list2 = new List<Pawn>();
							foreach (Pawn allPawn in CS_0024_003C_003E8__locals15.Portal.PocketMap.mapPawns.AllPawns)
							{
								if (allPawn.Faction == Faction.OfPlayer)
								{
									if (allPawn.RaceProps.Humanlike)
									{
										list.Add(allPawn);
									}
									else
									{
										list2.Add(allPawn);
									}
								}
							}
							if (list.Count > 0)
							{
								text = text + "\n\n" + ("Warning".Translate() + ": ").Colorize(ColorLibrary.RedReadable) + "PeopleWillBeLeftBehind".Translate().Resolve() + ":\n" + list.Select((Pawn p) => p.NameFullColored.Resolve()).ToLineList("  - ", capitalizeItems: true);
							}
							if (list2.Count > 0)
							{
								text = text + "\n\n" + ("Warning".Translate() + ": ").Colorize(ColorLibrary.RedReadable) + "AnimalsWillBeLeftBehind".Translate().Resolve() + ":\n" + list2.Select((Pawn p) => p.NameFullColored.Resolve()).ToLineList("  - ", capitalizeItems: true);
							}
							Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(CS_0024_003C_003E8__locals15.Props.confirmSealText.Formatted(text), delegate
							{
								CS_0024_003C_003E8__locals10.target.Pawn?.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Seal, CS_0024_003C_003E8__locals10._003C_003E4__this.Portal), JobTag.Misc);
							}));
						}, delegate(LocalTargetInfo target)
						{
							Pawn pawn = target.Pawn;
							if (pawn != null && pawn.IsColonistPlayerControlled)
							{
								GenDraw.DrawTargetHighlight(target);
							}
						}, (LocalTargetInfo target) => CS_0024_003C_003E8__locals15.ValidateSealer(target).Accepted, null, null, CS_0024_003C_003E8__locals15.SealTex, playSoundOnAction: true, delegate(LocalTargetInfo target)
						{
							AcceptanceReport acceptanceReport = CS_0024_003C_003E8__locals15.ValidateSealer(target);
							Pawn pawn2 = target.Pawn;
							if (pawn2 != null && pawn2.IsColonistPlayerControlled && !acceptanceReport.Accepted)
							{
								if (!acceptanceReport.Reason.NullOrEmpty())
								{
									Widgets.MouseAttachedLabel((CS_0024_003C_003E8__locals15.Props.cannotSealLabel + ": " + acceptanceReport.Reason.CapitalizeFirst()).Colorize(ColorLibrary.RedReadable), 0f, 0f, null);
								}
								else
								{
									Widgets.MouseAttachedLabel(CS_0024_003C_003E8__locals15.Props.cannotSealLabel, 0f, 0f, null);
								}
							}
						});
					}
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
			_003CCompGetGizmosExtra_003Ed__12 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__12(0)
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

	public const string SealedSignal = "Sealed";

	private bool isSealed;

	private Texture2D cachedSealTex;

	private CompProperties_Sealable Props => (CompProperties_Sealable)props;

	private MapPortal Portal => (MapPortal)parent;

	private Texture2D SealTex => cachedSealTex ?? (cachedSealTex = ContentFinder<Texture2D>.Get(Props.sealTexPath));

	public override void PostExposeData()
	{
		Scribe_Values.Look(ref isSealed, "isSealed", defaultValue: false);
	}

	public override AcceptanceReport CanEnterPortal()
	{
		if (isSealed)
		{
			return Props.cannotEnterLabel;
		}
		return true;
	}

	public void Seal()
	{
		if (!Portal.PocketMapExists)
		{
			Log.Error("Tried to seal portal but pocket map doesn't exist");
			return;
		}
		isSealed = true;
		PocketMapUtility.DestroyPocketMap(Portal.PocketMap);
		Portal.BroadcastCompSignal("Sealed");
		if (Props.destroyPortal)
		{
			Thing.allowDestroyNonDestroyable = true;
			Portal.Destroy(DestroyMode.Deconstruct);
			Thing.allowDestroyNonDestroyable = false;
		}
		else
		{
			Portal.DirtyMapMesh(Portal.Map);
		}
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__12))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__12(-2)
		{
			_003C_003E4__this = this
		};
	}

	private AcceptanceReport ValidateSealer(LocalTargetInfo target)
	{
		if (!(target.Thing is Pawn pawn))
		{
			return false;
		}
		if (!pawn.CanReach(Portal, PathEndMode.Touch, Danger.Deadly))
		{
			return "NoPath".Translate();
		}
		if (pawn.Downed)
		{
			return "DownedLower".Translate();
		}
		return true;
	}

	public override string CompInspectStringExtra()
	{
		if (!isSealed)
		{
			return null;
		}
		return Props.sealedInspectString;
	}
}
