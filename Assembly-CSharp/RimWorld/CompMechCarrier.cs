using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld;

public class CompMechCarrier : ThingComp, IThingHolder
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__25 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompMechCarrier _003C_003E4__this;

		private Command_ActionWithCooldown _003Cact_003E5__2;

		private IEnumerator<Gizmo> _003C_003E7__wrap2;

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
		public _003CCompGetGizmosExtra_003Ed__25(int _003C_003E1__state)
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
			_003Cact_003E5__2 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				CompMechCarrier CS_0024_003C_003E8__locals29 = _003C_003E4__this;
				AcceptanceReport canSpawn;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (!(CS_0024_003C_003E8__locals29.parent is Pawn { IsColonyMech: not false } pawn) || pawn.GetOverseer() == null)
					{
						break;
					}
					_003C_003E7__wrap2 = ((ThingComp)CS_0024_003C_003E8__locals29).CompGetGizmosExtra().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00ac;
				case 1:
					_003C_003E1__state = -3;
					goto IL_00ac;
				case 2:
					_003C_003E1__state = -1;
					goto IL_010d;
				case 3:
					_003C_003E1__state = -1;
					goto IL_02a7;
				case 4:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Empty " + CS_0024_003C_003E8__locals29.Props.fixedIngredient.label,
						action = delegate
						{
							CS_0024_003C_003E8__locals29.innerContainer.ClearAndDestroyContents();
						}
					};
					_003C_003E1__state = 5;
					return true;
				case 5:
					_003C_003E1__state = -1;
					goto IL_0354;
				case 6:
					{
						_003C_003E1__state = -1;
						_003Cact_003E5__2 = null;
						break;
					}
					IL_02a7:
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Fill with " + CS_0024_003C_003E8__locals29.Props.fixedIngredient.label,
						action = delegate
						{
							while (CS_0024_003C_003E8__locals29.IngredientCount < CS_0024_003C_003E8__locals29.Props.maxIngredientCount)
							{
								int stackCount = Mathf.Min(CS_0024_003C_003E8__locals29.Props.maxIngredientCount - CS_0024_003C_003E8__locals29.IngredientCount, CS_0024_003C_003E8__locals29.Props.fixedIngredient.stackLimit);
								Thing thing = ThingMaker.MakeThing(CS_0024_003C_003E8__locals29.Props.fixedIngredient);
								thing.stackCount = stackCount;
								CS_0024_003C_003E8__locals29.innerContainer.TryAdd(thing, thing.stackCount);
							}
						}
					};
					_003C_003E1__state = 4;
					return true;
					IL_0354:
					_003C_003E2__current = _003Cact_003E5__2;
					_003C_003E1__state = 6;
					return true;
					IL_00ac:
					if (_003C_003E7__wrap2.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = null;
					if (Find.Selector.SingleSelectedThing == CS_0024_003C_003E8__locals29.parent)
					{
						if (CS_0024_003C_003E8__locals29.gizmo == null)
						{
							CS_0024_003C_003E8__locals29.gizmo = new MechCarrierGizmo(CS_0024_003C_003E8__locals29);
						}
						_003C_003E2__current = CS_0024_003C_003E8__locals29.gizmo;
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_010d;
					IL_010d:
					canSpawn = CS_0024_003C_003E8__locals29.CanSpawn;
					_003Cact_003E5__2 = new Command_ActionWithCooldown();
					_003Cact_003E5__2.cooldownPercentGetter = () => Mathf.InverseLerp(CS_0024_003C_003E8__locals29.Props.cooldownTicks, 0f, CS_0024_003C_003E8__locals29.cooldownTicksRemaining);
					_003Cact_003E5__2.action = delegate
					{
						CS_0024_003C_003E8__locals29.TrySpawnPawns();
					};
					_003Cact_003E5__2.hotKey = KeyBindingDefOf.Misc2;
					_003Cact_003E5__2.Disabled = !canSpawn.Accepted;
					_003Cact_003E5__2.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/ReleaseWarUrchins");
					_003Cact_003E5__2.defaultLabel = "MechCarrierRelease".Translate(CS_0024_003C_003E8__locals29.Props.spawnPawnKind.labelPlural);
					_003Cact_003E5__2.defaultDesc = "MechCarrierDesc".Translate(CS_0024_003C_003E8__locals29.Props.maxPawnsToSpawn, CS_0024_003C_003E8__locals29.Props.spawnPawnKind.labelPlural, CS_0024_003C_003E8__locals29.Props.spawnPawnKind.label, CS_0024_003C_003E8__locals29.Props.costPerPawn, CS_0024_003C_003E8__locals29.Props.fixedIngredient.label);
					if (!canSpawn.Reason.NullOrEmpty())
					{
						_003Cact_003E5__2.Disable(canSpawn.Reason);
					}
					if (DebugSettings.ShowDevGizmos)
					{
						if (CS_0024_003C_003E8__locals29.cooldownTicksRemaining > 0)
						{
							_003C_003E2__current = new Command_Action
							{
								defaultLabel = "DEV: Reset cooldown",
								action = delegate
								{
									CS_0024_003C_003E8__locals29.cooldownTicksRemaining = 0;
								}
							};
							_003C_003E1__state = 3;
							return true;
						}
						goto IL_02a7;
					}
					goto IL_0354;
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
			_003CCompGetGizmosExtra_003Ed__25 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__25(0)
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

	private const int LowIngredientCountThreshold = 250;

	private int cooldownTicksRemaining;

	private ThingOwner innerContainer;

	private List<Pawn> spawnedPawns = new List<Pawn>();

	private MechCarrierGizmo gizmo;

	public int maxToFill;

	private List<Thing> tmpResources = new List<Thing>();

	public CompProperties_MechCarrier Props => (CompProperties_MechCarrier)props;

	public AcceptanceReport CanSpawn
	{
		get
		{
			if (parent is Pawn pawn)
			{
				if (pawn.IsSelfShutdown())
				{
					return "SelfShutdown".Translate();
				}
				if (pawn.Faction == Faction.OfPlayer && !pawn.IsColonyMechPlayerControlled)
				{
					return false;
				}
				if (!pawn.Awake() || pawn.Downed || pawn.Dead || !pawn.Spawned)
				{
					return false;
				}
			}
			if (MaxCanSpawn <= 0)
			{
				return "MechCarrierNotEnoughResources".Translate();
			}
			if (cooldownTicksRemaining > 0)
			{
				return "CooldownTime".Translate() + " " + cooldownTicksRemaining.ToStringSecondsFromTicks();
			}
			return true;
		}
	}

	public int IngredientCount => innerContainer.TotalStackCountOfDef(Props.fixedIngredient);

	public int AmountToAutofill => Mathf.Max(0, maxToFill - IngredientCount);

	public int MaxCanSpawn => Mathf.Min(Mathf.FloorToInt(IngredientCount / Props.costPerPawn), Props.maxPawnsToSpawn);

	public bool LowIngredientCount => IngredientCount < 250;

	public float PercentageFull => (float)IngredientCount / (float)Props.maxIngredientCount;

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		if (!ModLister.CheckBiotech("Mech carrier"))
		{
			parent.Destroy();
		}
	}

	public override void PostPostMake()
	{
		innerContainer = new ThingOwner<Thing>(this, oneStackOnly: false);
		if (Props.startingIngredientCount > 0)
		{
			Thing thing = ThingMaker.MakeThing(Props.fixedIngredient);
			thing.stackCount = Props.startingIngredientCount;
			innerContainer.TryAdd(thing, Props.startingIngredientCount);
		}
		maxToFill = Props.startingIngredientCount;
	}

	public void TrySpawnPawns()
	{
		int maxCanSpawn = MaxCanSpawn;
		if (maxCanSpawn <= 0)
		{
			return;
		}
		PawnGenerationRequest request = new PawnGenerationRequest(Props.spawnPawnKind, parent.Faction, PawnGenerationContext.NonPlayer, null, forceGenerateNewPawn: true, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, forceNoIdeo: false, forceNoBackstory: false, forbidAnyTitle: false, forceDead: false, null, null, null, null, null, 0f, DevelopmentalStage.Newborn, null, null, null);
		tmpResources.Clear();
		tmpResources.AddRange(innerContainer);
		Lord lord = ((parent is Pawn p) ? p.GetLord() : null);
		for (int i = 0; i < maxCanSpawn; i++)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			GenSpawn.Spawn(pawn, parent.Position, parent.Map);
			spawnedPawns.Add(pawn);
			lord?.AddPawn(pawn);
			int num = Props.costPerPawn;
			for (int j = 0; j < tmpResources.Count; j++)
			{
				Thing thing = innerContainer.Take(tmpResources[j], Mathf.Min(tmpResources[j].stackCount, Props.costPerPawn));
				num -= thing.stackCount;
				thing.Destroy();
				if (num <= 0)
				{
					break;
				}
			}
			if (Props.spawnedMechEffecter != null)
			{
				Effecter effecter = new Effecter(Props.spawnedMechEffecter);
				effecter.Trigger(Props.attachSpawnedMechEffecter ? ((TargetInfo)pawn) : new TargetInfo(pawn.Position, pawn.Map), TargetInfo.Invalid);
				effecter.Cleanup();
			}
		}
		tmpResources.Clear();
		cooldownTicksRemaining = Props.cooldownTicks;
		if (Props.spawnEffecter != null)
		{
			Effecter effecter2 = new Effecter(Props.spawnEffecter);
			effecter2.Trigger(Props.attachSpawnedEffecter ? ((TargetInfo)parent) : new TargetInfo(parent.Position, parent.Map), TargetInfo.Invalid);
			effecter2.Cleanup();
		}
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__25))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__25(-2)
		{
			_003C_003E4__this = this
		};
	}

	public void GetChildHolders(List<IThingHolder> outChildren)
	{
		ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
	}

	public ThingOwner GetDirectlyHeldThings()
	{
		return innerContainer;
	}

	public override string CompInspectStringExtra()
	{
		string text = base.CompInspectStringExtra();
		if (!text.NullOrEmpty())
		{
			text += "\n";
		}
		return text + ("CasketContains".Translate() + ": " + innerContainer.ContentsString.CapitalizeFirst());
	}

	public override void PostDestroy(DestroyMode mode, Map previousMap)
	{
		base.PostDestroy(mode, previousMap);
		innerContainer?.ClearAndDestroyContents();
		for (int i = 0; i < spawnedPawns.Count; i++)
		{
			if (!spawnedPawns[i].Dead)
			{
				spawnedPawns[i].Kill(null);
			}
		}
	}

	public override void PostDrawExtraSelectionOverlays()
	{
		if (!Find.Selector.IsSelected(parent))
		{
			return;
		}
		for (int i = 0; i < spawnedPawns.Count; i++)
		{
			if (!spawnedPawns[i].Dead)
			{
				GenDraw.DrawLineBetween(parent.TrueCenter(), spawnedPawns[i].TrueCenter());
			}
		}
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Deep.Look(ref innerContainer, "innerContainer", this);
		Scribe_Values.Look(ref cooldownTicksRemaining, "cooldownTicksRemaining", 0);
		Scribe_Values.Look(ref maxToFill, "maxToFill", 0);
		Scribe_Collections.Look(ref spawnedPawns, "spawnedPawns", LookMode.Reference);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			spawnedPawns.RemoveAll((Pawn x) => x == null);
		}
	}

	public override void CompTick()
	{
		if (cooldownTicksRemaining > 0)
		{
			cooldownTicksRemaining--;
		}
	}
}
