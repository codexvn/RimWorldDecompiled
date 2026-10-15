using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public static class TrainableUtility
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass10_0
	{
		public Pawn p;

		internal void _003CMasterSelectButton_GenerateMenu_003Eb__0()
		{
			p.playerSettings.Master = null;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass10_1
	{
		public Pawn col;

		public _003C_003Ec__DisplayClass10_0 CS_0024_003C_003E8__locals1;

		internal void _003CMasterSelectButton_GenerateMenu_003Eb__1()
		{
			CS_0024_003C_003E8__locals1.p.playerSettings.Master = col;
		}
	}

	[CompilerGenerated]
	private sealed class _003CMasterSelectButton_GenerateMenu_003Ed__10 : IEnumerable<Widgets.DropdownMenuElement<Pawn>>, IEnumerable, IEnumerator<Widgets.DropdownMenuElement<Pawn>>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Widgets.DropdownMenuElement<Pawn> _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn p;

		public Pawn _003C_003E3__p;

		private _003C_003Ec__DisplayClass10_0 _003C_003E8__1;

		private List<Pawn>.Enumerator _003C_003E7__wrap1;

		Widgets.DropdownMenuElement<Pawn> IEnumerator<Widgets.DropdownMenuElement<Pawn>>.Current
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
		public _003CMasterSelectButton_GenerateMenu_003Ed__10(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 2)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E8__1 = null;
			_003C_003E7__wrap1 = default(List<Pawn>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass10_0();
					_003C_003E8__1.p = p;
					_003C_003E2__current = new Widgets.DropdownMenuElement<Pawn>
					{
						option = new FloatMenuOption("(" + "NoneLower".Translate() + ")", delegate
						{
							_003C_003E8__1.p.playerSettings.Master = null;
						}),
						payload = null
					};
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = PawnsFinder.AllMaps_FreeColonistsSpawned.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 2:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					_003C_003Ec__DisplayClass10_1 CS_0024_003C_003E8__locals9 = new _003C_003Ec__DisplayClass10_1
					{
						CS_0024_003C_003E8__locals1 = _003C_003E8__1,
						col = _003C_003E7__wrap1.Current
					};
					string text = RelationsUtility.LabelWithBondInfo(CS_0024_003C_003E8__locals9.col, CS_0024_003C_003E8__locals9.CS_0024_003C_003E8__locals1.p);
					Action action = null;
					if (CanBeMaster(CS_0024_003C_003E8__locals9.col, CS_0024_003C_003E8__locals9.CS_0024_003C_003E8__locals1.p))
					{
						action = delegate
						{
							CS_0024_003C_003E8__locals9.CS_0024_003C_003E8__locals1.p.playerSettings.Master = CS_0024_003C_003E8__locals9.col;
						};
					}
					else
					{
						int level = CS_0024_003C_003E8__locals9.col.skills.GetSkill(SkillDefOf.Animals).Level;
						int num = MinimumHandlingSkill(CS_0024_003C_003E8__locals9.CS_0024_003C_003E8__locals1.p);
						if (level < num)
						{
							action = null;
							text += " (" + "SkillTooLow".Translate(SkillDefOf.Animals.LabelCap, level, num) + ")";
						}
					}
					_003C_003E2__current = new Widgets.DropdownMenuElement<Pawn>
					{
						option = new FloatMenuOption(text, action),
						payload = CS_0024_003C_003E8__locals9.col
					};
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<Pawn>.Enumerator);
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Widgets.DropdownMenuElement<Pawn>> IEnumerable<Widgets.DropdownMenuElement<Pawn>>.GetEnumerator()
		{
			_003CMasterSelectButton_GenerateMenu_003Ed__10 _003CMasterSelectButton_GenerateMenu_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CMasterSelectButton_GenerateMenu_003Ed__ = this;
			}
			else
			{
				_003CMasterSelectButton_GenerateMenu_003Ed__ = new _003CMasterSelectButton_GenerateMenu_003Ed__10(0);
			}
			_003CMasterSelectButton_GenerateMenu_003Ed__.p = _003C_003E3__p;
			return _003CMasterSelectButton_GenerateMenu_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Widgets.DropdownMenuElement<Pawn>>)this).GetEnumerator();
		}
	}

	private static List<TrainableDef> defsInListOrder = new List<TrainableDef>();

	public const int MinTrainInterval = 15000;

	private static readonly SimpleCurve DecayIntervalDaysFromWildnessCurve = new SimpleCurve
	{
		new CurvePoint(0f, 12f),
		new CurvePoint(1f, 6f)
	};

	private static List<TrainabilityDef> cachedTrainabilityDefs;

	public static List<TrainableDef> TrainableDefsInListOrder => defsInListOrder;

	public static void Reset()
	{
		defsInListOrder.Clear();
		defsInListOrder.AddRange(DefDatabase<TrainableDef>.AllDefsListForReading.OrderByDescending((TrainableDef td) => td.listPriority));
		bool flag;
		do
		{
			flag = false;
			for (int i = 0; i < defsInListOrder.Count; i++)
			{
				TrainableDef trainableDef = defsInListOrder[i];
				if (trainableDef.prerequisites != null)
				{
					for (int j = 0; j < trainableDef.prerequisites.Count; j++)
					{
						if (trainableDef.indent <= trainableDef.prerequisites[j].indent)
						{
							trainableDef.indent = trainableDef.prerequisites[j].indent + 1;
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					break;
				}
			}
		}
		while (flag);
	}

	public static string MasterString(Pawn pawn)
	{
		if (pawn.playerSettings.Master == null)
		{
			return "(" + "NoneLower".TranslateSimple() + ")";
		}
		return RelationsUtility.LabelWithBondInfo(pawn.playerSettings.Master, pawn);
	}

	public static int MinimumHandlingSkill(Pawn p)
	{
		return Mathf.RoundToInt(p.GetStatValue(StatDefOf.MinimumHandlingSkill));
	}

	public static void MasterSelectButton(Rect rect, Pawn pawn, bool paintable)
	{
		Widgets.Dropdown(rect, pawn, MasterSelectButton_GetMaster, MasterSelectButton_GenerateMenu, MasterString(pawn).Truncate(rect.width), null, MasterString(pawn), null, null, paintable);
	}

	private static Pawn MasterSelectButton_GetMaster(Pawn pet)
	{
		return pet.playerSettings.Master;
	}

	[IteratorStateMachine(typeof(_003CMasterSelectButton_GenerateMenu_003Ed__10))]
	private static IEnumerable<Widgets.DropdownMenuElement<Pawn>> MasterSelectButton_GenerateMenu(Pawn p)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMasterSelectButton_GenerateMenu_003Ed__10(-2)
		{
			_003C_003E3__p = p
		};
	}

	public static bool CanBeMaster(Pawn master, Pawn animal, bool checkSpawned = true)
	{
		if ((checkSpawned && !master.Spawned) || master.IsPrisoner)
		{
			return false;
		}
		if (ModsConfig.IdeologyActive && animal.RaceProps.animalType == AnimalType.Dryad && animal.connections != null)
		{
			foreach (Thing connectedThing in animal.connections.ConnectedThings)
			{
				CompTreeConnection compTreeConnection = connectedThing.TryGetComp<CompTreeConnection>();
				if (compTreeConnection != null && compTreeConnection.ConnectedPawn == master)
				{
					return true;
				}
			}
			return false;
		}
		if (master.relations.DirectRelationExists(PawnRelationDefOf.Bond, animal))
		{
			return true;
		}
		int level = master.skills.GetSkill(SkillDefOf.Animals).Level;
		int num = MinimumHandlingSkill(animal);
		return level >= num;
	}

	public static string GetIconTooltipText(Pawn pawn)
	{
		string text = "";
		if (pawn.playerSettings != null && pawn.playerSettings.Master != null)
		{
			text += string.Format("{0}: {1}\n", "Master".Translate(), pawn.playerSettings.Master.LabelShort);
		}
		IEnumerable<Pawn> allColonistBondsFor = GetAllColonistBondsFor(pawn);
		if (allColonistBondsFor.Any())
		{
			text += string.Format("{0}: {1}\n", "BondedTo".Translate(), allColonistBondsFor.Select((Pawn bond) => bond.LabelShort).ToCommaList(useAnd: true));
		}
		return text.TrimEndNewlines();
	}

	public static IEnumerable<Pawn> GetAllColonistBondsFor(Pawn pet)
	{
		return from bond in pet.relations.DirectRelations
			where bond.def == PawnRelationDefOf.Bond && bond.otherPawn != null && bond.otherPawn.IsColonist
			select bond.otherPawn;
	}

	public static int DegradationPeriodTicks(ThingDef def)
	{
		return Mathf.RoundToInt(DecayIntervalDaysFromWildnessCurve.Evaluate(def.GetStatValueAbstract(StatDefOf.Wildness)) * 60000f);
	}

	public static int DegradationPeriodTicks(Pawn pawn)
	{
		if (pawn == null)
		{
			return 0;
		}
		return Mathf.RoundToInt(DecayIntervalDaysFromWildnessCurve.Evaluate(pawn.GetStatValue(StatDefOf.Wildness, applyPostProcess: true, 1)) * 60000f);
	}

	public static bool TamenessCanDecay(ThingDef def)
	{
		if (def.race.FenceBlocked)
		{
			return false;
		}
		return def.GetStatValueAbstract(StatDefOf.Wildness) > 0.101f;
	}

	public static bool TamenessCanDecay(Pawn pawn)
	{
		if (pawn.FenceBlocked)
		{
			return false;
		}
		return pawn.GetStatValue(StatDefOf.Wildness, applyPostProcess: true, 1) > 0.101f;
	}

	public static bool TrainedTooRecently(Pawn animal)
	{
		return Find.TickManager.TicksGame < animal.mindState.lastAssignedInteractTime + 15000;
	}

	public static bool IsReleasedToAttack(this Pawn pawn)
	{
		if (pawn?.playerSettings?.Master?.playerSettings == null || pawn.training == null)
		{
			return false;
		}
		if (pawn.playerSettings.Master.playerSettings.animalsReleased)
		{
			return pawn.training.HasLearned(TrainableDefOf.Release);
		}
		return false;
	}

	public static TrainabilityDef GetTrainability(Pawn pawn)
	{
		if (pawn == null)
		{
			return null;
		}
		if (ModsConfig.OdysseyActive && pawn.health.hediffSet.HasHediff(HediffDefOf.SentienceCatalyst))
		{
			return GetNextHighestTrainability(pawn.RaceProps.trainability);
		}
		return pawn.RaceProps.trainability;
	}

	private static TrainabilityDef GetNextHighestTrainability(TrainabilityDef racePropsTrainability)
	{
		if (cachedTrainabilityDefs == null)
		{
			cachedTrainabilityDefs = DefDatabase<TrainabilityDef>.AllDefsListForReading.ToList();
			cachedTrainabilityDefs.SortBy((TrainabilityDef td) => td.intelligenceOrder);
		}
		for (int i = 0; i < cachedTrainabilityDefs.Count; i++)
		{
			if (cachedTrainabilityDefs[i] == racePropsTrainability)
			{
				return cachedTrainabilityDefs[Mathf.Min(cachedTrainabilityDefs.Count - 1, i + 1)];
			}
		}
		return null;
	}
}
