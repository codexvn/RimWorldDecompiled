using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class Pawn_AbilityTracker : IExposable
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__13 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Pawn_AbilityTracker _003C_003E4__this;

		private bool _003CvisiblePrimary_003E5__2;

		private List<Ability>.Enumerator _003C_003E7__wrap2;

		private Ability _003Ca_003E5__4;

		private bool _003CvisibleSecondary_003E5__5;

		private IEnumerator<Command> _003C_003E7__wrap5;

		private IEnumerator<Gizmo> _003C_003E7__wrap6;

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
		public _003CGetGizmos_003Ed__13(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -5) <= 2u || (uint)(num - 1) <= 1u)
			{
				try
				{
					switch (num)
					{
					case -4:
					case 1:
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
						break;
					case -5:
					case 2:
						try
						{
						}
						finally
						{
							_003C_003Em__Finally3();
						}
						break;
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap2 = default(List<Ability>.Enumerator);
			_003Ca_003E5__4 = null;
			_003C_003E7__wrap5 = null;
			_003C_003E7__wrap6 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Pawn_AbilityTracker pawn_AbilityTracker = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003CvisiblePrimary_003E5__2 = pawn_AbilityTracker.pawn.IsColonistPlayerControlled || pawn_AbilityTracker.pawn.IsColonyMechPlayerControlled || pawn_AbilityTracker.pawn.IsColonySubhumanPlayerControlled || pawn_AbilityTracker.pawn.IsColonyAnimal;
					_003C_003E7__wrap2 = pawn_AbilityTracker.AllAbilitiesForReading.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0206;
				case 1:
					_003C_003E1__state = -4;
					goto IL_0183;
				case 2:
					{
						_003C_003E1__state = -5;
						goto IL_01e5;
					}
					IL_0183:
					if (_003C_003E7__wrap5.MoveNext())
					{
						Command current = _003C_003E7__wrap5.Current;
						if (current is Command_Ability command_Ability)
						{
							command_Ability.devGizmo = !_003CvisiblePrimary_003E5__2 && !_003CvisibleSecondary_003E5__5 && DebugSettings.ShowDevGizmos;
						}
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap5 = null;
					goto IL_019d;
					IL_019d:
					_003C_003E7__wrap6 = _003Ca_003E5__4.GetGizmosExtra().GetEnumerator();
					_003C_003E1__state = -5;
					goto IL_01e5;
					IL_01e5:
					if (_003C_003E7__wrap6.MoveNext())
					{
						Gizmo current2 = _003C_003E7__wrap6.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap6 = null;
					_003Ca_003E5__4 = null;
					goto IL_0206;
					IL_0206:
					if (_003C_003E7__wrap2.MoveNext())
					{
						_003Ca_003E5__4 = _003C_003E7__wrap2.Current;
						if (_003CvisiblePrimary_003E5__2 || DebugSettings.ShowDevGizmos)
						{
							_003CvisibleSecondary_003E5__5 = (pawn_AbilityTracker.pawn.Drafted || _003Ca_003E5__4.def.displayGizmoWhileUndrafted) && _003Ca_003E5__4.GizmosVisible();
							if (_003CvisibleSecondary_003E5__5 || (!pawn_AbilityTracker.pawn.IsColonistPlayerControlled && !pawn_AbilityTracker.pawn.IsColonyMechPlayerControlled && DebugSettings.ShowDevGizmos))
							{
								_003C_003E7__wrap5 = _003Ca_003E5__4.GetGizmos().GetEnumerator();
								_003C_003E1__state = -4;
								goto IL_0183;
							}
						}
						goto IL_019d;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = default(List<Ability>.Enumerator);
					return false;
				}
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
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
			if (_003C_003E7__wrap5 != null)
			{
				_003C_003E7__wrap5.Dispose();
			}
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -3;
			if (_003C_003E7__wrap6 != null)
			{
				_003C_003E7__wrap6.Dispose();
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
			_003CGetGizmos_003Ed__13 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__13(0)
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

	public Pawn pawn;

	public List<Ability> abilities = new List<Ability>();

	private bool allAbilitiesCachedDirty = true;

	private List<Ability> allAbilitiesCached = new List<Ability>();

	private readonly List<Ability> tmpAbilities = new List<Ability>();

	public List<Ability> AllAbilitiesForReading
	{
		get
		{
			if (allAbilitiesCachedDirty)
			{
				allAbilitiesCached.Clear();
				allAbilitiesCached.AddRange(abilities);
				if (pawn.health != null)
				{
					foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
					{
						if (!hediff.def.abilities.NullOrEmpty())
						{
							allAbilitiesCached.AddRange(hediff.AllAbilitiesForReading);
						}
					}
				}
				if (pawn.equipment != null)
				{
					CompEquippableAbility compEquippableAbility = pawn.equipment.Primary?.TryGetComp<CompEquippableAbility>();
					if (compEquippableAbility != null && compEquippableAbility.AbilityForReading != null)
					{
						allAbilitiesCached.Add(compEquippableAbility.AbilityForReading);
					}
				}
				if (pawn.apparel != null)
				{
					foreach (Apparel item in pawn.apparel.WornApparel)
					{
						allAbilitiesCached.AddRange(item.AllAbilitiesForReading);
					}
				}
				if (ModsConfig.AnomalyActive && pawn.IsMutant)
				{
					allAbilitiesCached.AddRange(pawn.mutant.AllAbilitiesForReading);
				}
				if (pawn.royalty != null)
				{
					allAbilitiesCached.AddRange(pawn.royalty.AllAbilitiesForReading);
				}
				if (ModsConfig.IdeologyActive)
				{
					Precept_Role precept_Role = pawn.Ideo?.GetRole(pawn);
					if (precept_Role != null && precept_Role.Active && !precept_Role.AbilitiesFor(pawn).NullOrEmpty())
					{
						foreach (Ability item2 in precept_Role.AbilitiesFor(pawn))
						{
							bool flag = false;
							if (!item2.def.requiredMemes.NullOrEmpty())
							{
								foreach (MemeDef requiredMeme in item2.def.requiredMemes)
								{
									if (!pawn.Ideo.memes.Contains(requiredMeme))
									{
										flag = true;
										break;
									}
								}
							}
							if (!flag)
							{
								allAbilitiesCached.Add(item2);
							}
						}
					}
				}
				if (ModsConfig.AnomalyActive && pawn.IsMutant && pawn.mutant.Def.abilityWhitelist.Any())
				{
					allAbilitiesCached = allAbilitiesCached.Where((Ability a) => pawn.mutant.Def.abilityWhitelist.Contains(a.def)).ToList();
				}
				allAbilitiesCached.SortBy((Ability a) => a.def.category?.displayOrder ?? 0, (Ability a) => a.def.displayOrder, (Ability a) => a.def.level);
				allAbilitiesCachedDirty = false;
			}
			return allAbilitiesCached;
		}
	}

	public Pawn_AbilityTracker(Pawn pawn)
	{
		this.pawn = pawn;
	}

	public void AbilitiesTick()
	{
		for (int i = 0; i < AllAbilitiesForReading.Count; i++)
		{
			AllAbilitiesForReading[i].AbilityTick();
		}
	}

	public void GainAbility(AbilityDef def)
	{
		if (!abilities.Any((Ability a) => a.def == def))
		{
			abilities.Add(AbilityUtility.MakeAbility(def, pawn));
		}
		Notify_TemporaryAbilitiesChanged();
	}

	public void RemoveAbility(AbilityDef def)
	{
		Ability ability = abilities.FirstOrDefault((Ability x) => x.def == def);
		if (ability != null)
		{
			abilities.Remove(ability);
		}
		Notify_TemporaryAbilitiesChanged();
	}

	public Ability GetAbility(AbilityDef def, bool includeTemporary = false)
	{
		List<Ability> list = (includeTemporary ? AllAbilitiesForReading : abilities);
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].def == def)
			{
				return list[i];
			}
		}
		return null;
	}

	public List<Ability> AICastableAbilities(LocalTargetInfo target, bool offensive)
	{
		tmpAbilities.Clear();
		foreach (Ability item in AllAbilitiesForReading)
		{
			if (item.def.ai_IsOffensive == offensive && (item.AICanTargetNow(target) || item.AICanTargetNow(pawn)))
			{
				tmpAbilities.Add(item);
			}
		}
		return tmpAbilities;
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__13))]
	public IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__13(-2)
		{
			_003C_003E4__this = this
		};
	}

	public void Notify_TemporaryAbilitiesChanged()
	{
		allAbilitiesCachedDirty = true;
	}

	public void ExposeData()
	{
		Scribe_Collections.Look(ref abilities, "abilities", LookMode.Deep, pawn);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			abilities.RemoveAll((Ability a) => a.def == null || a.def == AbilityDefOf.Speech);
		}
	}
}
