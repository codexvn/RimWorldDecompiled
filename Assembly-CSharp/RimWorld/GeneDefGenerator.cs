using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public static class GeneDefGenerator
{
	[CompilerGenerated]
	private sealed class _003CImpliedGeneDefs_003Ed__0 : IEnumerable<GeneDef>, IEnumerable, IEnumerator<GeneDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GeneDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private bool hotReload;

		public bool _003C_003E3__hotReload;

		private IEnumerator<GeneTemplateDef> _003C_003E7__wrap1;

		private GeneTemplateDef _003Cg_003E5__3;

		private IEnumerator<SkillDef> _003C_003E7__wrap3;

		private IEnumerator<ChemicalDef> _003C_003E7__wrap4;

		GeneDef IEnumerator<GeneDef>.Current
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
		public _003CImpliedGeneDefs_003Ed__0(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = null;
			_003Cg_003E5__3 = null;
			_003C_003E7__wrap3 = null;
			_003C_003E7__wrap4 = null;
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
					if (!ModsConfig.BiotechActive)
					{
						return false;
					}
					_003C_003E7__wrap1 = DefDatabase<GeneTemplateDef>.AllDefs.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0187;
				case 1:
					_003C_003E1__state = -4;
					goto IL_00de;
				case 2:
					{
						_003C_003E1__state = -5;
						goto IL_0166;
					}
					IL_00de:
					if (_003C_003E7__wrap3.MoveNext())
					{
						SkillDef current = _003C_003E7__wrap3.Current;
						_003C_003E2__current = GetFromTemplate(_003Cg_003E5__3, current, current.index * 1000, hotReload);
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap3 = null;
					goto IL_0180;
					IL_0187:
					if (_003C_003E7__wrap1.MoveNext())
					{
						_003Cg_003E5__3 = _003C_003E7__wrap1.Current;
						GeneTemplateDef.GeneTemplateType geneTemplateType = _003Cg_003E5__3.geneTemplateType;
						if (geneTemplateType != 0)
						{
							if (geneTemplateType != GeneTemplateDef.GeneTemplateType.Chemical)
							{
								goto IL_0180;
							}
							_003C_003E7__wrap4 = DefDatabase<ChemicalDef>.AllDefs.GetEnumerator();
							_003C_003E1__state = -5;
							goto IL_0166;
						}
						_003C_003E7__wrap3 = DefDatabase<SkillDef>.AllDefs.GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_00de;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					return false;
					IL_0166:
					while (_003C_003E7__wrap4.MoveNext())
					{
						ChemicalDef current2 = _003C_003E7__wrap4.Current;
						if (current2.generateAddictionGenes)
						{
							_003C_003E2__current = GetFromTemplate(_003Cg_003E5__3, current2, current2.index * 1000, hotReload);
							_003C_003E1__state = 2;
							return true;
						}
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap4 = null;
					goto IL_0180;
					IL_0180:
					_003Cg_003E5__3 = null;
					goto IL_0187;
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
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
			if (_003C_003E7__wrap3 != null)
			{
				_003C_003E7__wrap3.Dispose();
			}
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -3;
			if (_003C_003E7__wrap4 != null)
			{
				_003C_003E7__wrap4.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<GeneDef> IEnumerable<GeneDef>.GetEnumerator()
		{
			_003CImpliedGeneDefs_003Ed__0 _003CImpliedGeneDefs_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CImpliedGeneDefs_003Ed__ = this;
			}
			else
			{
				_003CImpliedGeneDefs_003Ed__ = new _003CImpliedGeneDefs_003Ed__0(0);
			}
			_003CImpliedGeneDefs_003Ed__.hotReload = _003C_003E3__hotReload;
			return _003CImpliedGeneDefs_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<GeneDef>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CImpliedThoughtDefs_003Ed__1 : IEnumerable<ThoughtDef>, IEnumerable, IEnumerator<ThoughtDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThoughtDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private bool hotReload;

		public bool _003C_003E3__hotReload;

		private IEnumerator<GeneTemplateDef> _003C_003E7__wrap1;

		private IEnumerator<ChemicalDef> _003C_003E7__wrap2;

		ThoughtDef IEnumerator<ThoughtDef>.Current
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
		public _003CImpliedThoughtDefs_003Ed__1(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || num == 1)
			{
				try
				{
					if (num == -4 || num == 1)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
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
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -4;
					goto IL_00cc;
				}
				_003C_003E1__state = -1;
				if (!ModsConfig.BiotechActive)
				{
					return false;
				}
				_003C_003E7__wrap1 = DefDatabase<GeneTemplateDef>.AllDefs.GetEnumerator();
				_003C_003E1__state = -3;
				goto IL_00e6;
				IL_00cc:
				while (_003C_003E7__wrap2.MoveNext())
				{
					ChemicalDef current = _003C_003E7__wrap2.Current;
					if (current.generateAddictionGenes)
					{
						_003C_003E2__current = DependencyThoughtDef(current, hotReload);
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap2 = null;
				goto IL_00e6;
				IL_00e6:
				GeneTemplateDef current2;
				do
				{
					if (_003C_003E7__wrap1.MoveNext())
					{
						current2 = _003C_003E7__wrap1.Current;
						continue;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					return false;
				}
				while (current2.geneTemplateType != GeneTemplateDef.GeneTemplateType.Chemical || !(current2.geneClass == typeof(Gene_ChemicalDependency)));
				_003C_003E7__wrap2 = DefDatabase<ChemicalDef>.AllDefs.GetEnumerator();
				_003C_003E1__state = -4;
				goto IL_00cc;
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
			_003C_003E1__state = -3;
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
		IEnumerator<ThoughtDef> IEnumerable<ThoughtDef>.GetEnumerator()
		{
			_003CImpliedThoughtDefs_003Ed__1 _003CImpliedThoughtDefs_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CImpliedThoughtDefs_003Ed__ = this;
			}
			else
			{
				_003CImpliedThoughtDefs_003Ed__ = new _003CImpliedThoughtDefs_003Ed__1(0);
			}
			_003CImpliedThoughtDefs_003Ed__.hotReload = _003C_003E3__hotReload;
			return _003CImpliedThoughtDefs_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThoughtDef>)this).GetEnumerator();
		}
	}

	[IteratorStateMachine(typeof(_003CImpliedGeneDefs_003Ed__0))]
	public static IEnumerable<GeneDef> ImpliedGeneDefs(bool hotReload = false)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CImpliedGeneDefs_003Ed__0(-2)
		{
			_003C_003E3__hotReload = hotReload
		};
	}

	[IteratorStateMachine(typeof(_003CImpliedThoughtDefs_003Ed__1))]
	public static IEnumerable<ThoughtDef> ImpliedThoughtDefs(bool hotReload = false)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CImpliedThoughtDefs_003Ed__1(-2)
		{
			_003C_003E3__hotReload = hotReload
		};
	}

	private static GeneDef GetFromTemplate(GeneTemplateDef template, Def def, int displayOrderBase, bool hotReload = false)
	{
		string defName = template.defName + "_" + def.defName;
		GeneDef geneDef = (hotReload ? (DefDatabase<GeneDef>.GetNamed(defName, errorOnFail: false) ?? new GeneDef()) : new GeneDef());
		geneDef.defName = defName;
		geneDef.geneClass = template.geneClass;
		geneDef.label = template.label.Formatted(def.label);
		geneDef.iconPath = template.iconPath.Formatted(def.defName);
		geneDef.description = ResolveDescription();
		geneDef.labelShortAdj = template.labelShortAdj.Formatted(def.label);
		geneDef.selectionWeight = template.selectionWeight;
		geneDef.biostatCpx = template.biostatCpx;
		geneDef.biostatMet = template.biostatMet;
		geneDef.displayCategory = template.displayCategory;
		geneDef.displayOrderInCategory = displayOrderBase + template.displayOrderOffset;
		geneDef.minAgeActive = template.minAgeActive;
		geneDef.modContentPack = template.modContentPack;
		if (!template.exclusionTagPrefix.NullOrEmpty())
		{
			geneDef.exclusionTags = new List<string> { template.exclusionTagPrefix + "_" + def.defName };
		}
		if (def is SkillDef skill)
		{
			if (template.aptitudeOffset != 0)
			{
				geneDef.aptitudes = new List<Aptitude>
				{
					new Aptitude(skill, template.aptitudeOffset)
				};
			}
			if (template.passionModType != 0)
			{
				geneDef.passionMod = new PassionMod(skill, template.passionModType);
			}
		}
		else if (def is ChemicalDef chemicalDef)
		{
			geneDef.chemical = chemicalDef;
			geneDef.addictionChanceFactor = template.addictionChanceFactor;
			if (!template.chemicalBiostatOverrides.NullOrEmpty())
			{
				foreach (GeneTemplateDef.ChemicalBiostatOverride chemicalBiostatOverride in template.chemicalBiostatOverrides)
				{
					if (chemicalBiostatOverride.chemical == chemicalDef)
					{
						geneDef.biostatCpx = chemicalBiostatOverride.biostatCpx ?? geneDef.biostatCpx;
						geneDef.biostatMet = chemicalBiostatOverride.biostatMet ?? geneDef.biostatMet;
						geneDef.biostatArc = chemicalBiostatOverride.biostatArc ?? geneDef.biostatArc;
					}
				}
			}
			if (geneDef.geneClass != typeof(Gene_ChemicalDependency))
			{
				if (geneDef.addictionChanceFactor <= 0f)
				{
					geneDef.overdoseChanceFactor = chemicalDef.geneOverdoseChanceFactorImmune;
					geneDef.toleranceBuildupFactor = chemicalDef.geneToleranceBuildupFactorImmune;
				}
				else
				{
					geneDef.overdoseChanceFactor = chemicalDef.geneOverdoseChanceFactorResist;
					geneDef.toleranceBuildupFactor = chemicalDef.geneToleranceBuildupFactorResist;
				}
				if (geneDef.overdoseChanceFactor != 1f)
				{
					geneDef.description += " " + ((geneDef.overdoseChanceFactor == 0f) ? "GeneOverdoseImmune" : "GeneOverdoseFactor").Translate(chemicalDef, geneDef.overdoseChanceFactor.ToStringPercent());
				}
				if (geneDef.toleranceBuildupFactor != 1f)
				{
					geneDef.description += " " + ((geneDef.toleranceBuildupFactor == 0f) ? "GeneToleranceBuildupImmune" : "GeneToleranceBuildupFactor").Translate(chemicalDef, geneDef.toleranceBuildupFactor.ToStringPercent());
				}
			}
		}
		return geneDef;
		string ResolveDescription()
		{
			if (template.geneClass == typeof(Gene_ChemicalDependency))
			{
				return template.description.Formatted(def.label, "PeriodDays".Translate(5f).Named("DEFICIENCYDURATION"), "PeriodDays".Translate(30f).Named("COMADURATION"), "PeriodDays".Translate(60f).Named("DEATHDURATION"));
			}
			return template.description.Formatted(def.label);
		}
	}

	private static ThoughtDef DependencyThoughtDef(ChemicalDef chemicalDef, bool hotReload = false)
	{
		string defName = "GeneticChemicalDependency_" + chemicalDef.defName;
		ThoughtDef obj = (hotReload ? (DefDatabase<ThoughtDef>.GetNamed(defName, errorOnFail: false) ?? new ThoughtDef()) : new ThoughtDef());
		obj.defName = defName;
		obj.thoughtClass = typeof(Thought_Situational_GeneticChemicalDependency);
		obj.workerClass = typeof(ThoughtWorker_GeneticChemicalDependency);
		obj.developmentalStageFilter = DevelopmentalStage.Child | DevelopmentalStage.Adult;
		obj.chemicalDef = chemicalDef;
		obj.stages = new List<ThoughtStage>
		{
			new ThoughtStage
			{
				label = "GeneticChemicalDependency_Label".Translate(chemicalDef.Named("CHEMICAL")),
				description = "GeneticChemicalDependency_Desc".Translate(chemicalDef.Named("CHEMICAL")),
				baseMoodEffect = -1f
			}
		};
		return obj;
	}
}
