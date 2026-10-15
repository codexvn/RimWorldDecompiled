using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class ApparelProperties : IRenderNodePropertiesParent
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__56 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ApparelProperties _003C_003E4__this;

		private ThingDef parentDef;

		public ThingDef _003C_003E3__parentDef;

		private List<PawnRenderNodeProperties>.Enumerator _003C_003E7__wrap1;

		private IEnumerator<string> _003C_003E7__wrap2;

		string IEnumerator<string>.Current
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
		public _003CConfigErrors_003Ed__56(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || num == 2)
			{
				try
				{
					if (num == -4 || num == 2)
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
			_003C_003E7__wrap1 = default(List<PawnRenderNodeProperties>.Enumerator);
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				ApparelProperties apparelProperties = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (apparelProperties.layers.NullOrEmpty())
					{
						_003C_003E2__current = parentDef.defName + " apparel has no layers.";
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_006b;
				case 1:
					_003C_003E1__state = -1;
					goto IL_006b;
				case 2:
					{
						_003C_003E1__state = -4;
						goto IL_00e0;
					}
					IL_00e0:
					if (_003C_003E7__wrap2.MoveNext())
					{
						string current = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = null;
					goto IL_00fa;
					IL_006b:
					if (!apparelProperties.HasDefinedGraphicProperties)
					{
						break;
					}
					_003C_003E7__wrap1 = apparelProperties.renderNodeProperties.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00fa;
					IL_00fa:
					if (_003C_003E7__wrap1.MoveNext())
					{
						PawnRenderNodeProperties current2 = _003C_003E7__wrap1.Current;
						_003C_003E7__wrap2 = current2.ConfigErrors().GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_00e0;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<PawnRenderNodeProperties>.Enumerator);
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__56 _003CConfigErrors_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CConfigErrors_003Ed__ = this;
			}
			else
			{
				_003CConfigErrors_003Ed__ = new _003CConfigErrors_003Ed__56(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CConfigErrors_003Ed__.parentDef = _003C_003E3__parentDef;
			return _003CConfigErrors_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public List<BodyPartGroupDef> bodyPartGroups = new List<BodyPartGroupDef>();

	public List<ApparelLayerDef> layers = new List<ApparelLayerDef>();

	[NoTranslate]
	public string wornGraphicPath = "";

	[NoTranslate]
	public List<string> wornGraphicPaths;

	public WornGraphicData wornGraphicData;

	public bool useWornGraphicMask;

	private List<PawnRenderNodeProperties> renderNodeProperties;

	public List<RenderSkipFlagDef> renderSkipFlags;

	public DrawData drawData;

	public PawnRenderNodeTagDef parentTagDef;

	public List<int> forceEyesVisibleForRotations = new List<int>();

	public bool shellRenderedBehindHead;

	[NoTranslate]
	public List<string> tags = new List<string>();

	[NoTranslate]
	public List<string> defaultOutfitTags;

	public bool canBeGeneratedToSatisfyWarmth = true;

	public bool canBeGeneratedToSatisfyVacuumResistance = true;

	public bool canBeGeneratedToSatisfyToxicEnvironmentResistance = true;

	public bool canBeDesiredForIdeo = true;

	public List<string> ideoDesireAllowedFactionCategoryTags;

	public List<string> ideoDesireDisallowedFactionCategoryTags;

	public List<AbilityDef> abilities = new List<AbilityDef>();

	public float wearPerDay = 0.4f;

	public bool careIfWornByCorpse = true;

	public bool careIfDamaged = true;

	public bool ignoredByNonViolent;

	public bool ai_pickUpOpportunistically;

	public bool blocksAddedPartWoundGraphics;

	public bool blocksVision;

	public bool immuneToToxGasExposure;

	public bool slaveApparel;

	public bool mechanitorApparel;

	public bool legsNakedUnlessCoveredBySomethingElse;

	public bool useDeflectMetalEffect;

	public bool countsAsClothingForNudity = true;

	public bool anyTechLevelCanUseForWarmth;

	public Gender gender;

	public float scoreOffset;

	public DevelopmentalStage developmentalStageFilter = DevelopmentalStage.Adult;

	public SoundDef soundWear;

	public SoundDef soundRemove;

	[Unsaved(false)]
	private float cachedHumanBodyCoverage = -1f;

	[Unsaved(false)]
	private BodyPartGroupDef[][] interferingBodyPartGroups;

	private static BodyPartGroupDef[] apparelRelevantGroups;

	public bool HasDefinedGraphicProperties => !renderNodeProperties.NullOrEmpty();

	public List<PawnRenderNodeProperties> RenderNodeProperties => renderNodeProperties;

	public ApparelLayerDef LastLayer
	{
		get
		{
			if (layers.Count > 0)
			{
				return layers[layers.Count - 1];
			}
			Log.ErrorOnce("Failed to get last layer on apparel item (see your config errors)", 31234937);
			return ApparelLayerDefOf.Belt;
		}
	}

	public float HumanBodyCoverage
	{
		get
		{
			if (cachedHumanBodyCoverage < 0f)
			{
				cachedHumanBodyCoverage = 0f;
				List<BodyPartRecord> allParts = BodyDefOf.Human.AllParts;
				for (int i = 0; i < allParts.Count; i++)
				{
					if (CoversBodyPart(allParts[i]))
					{
						cachedHumanBodyCoverage += allParts[i].coverageAbs;
					}
				}
			}
			return cachedHumanBodyCoverage;
		}
	}

	public bool CorrectGenderForWearing(Gender wearerGender)
	{
		if (gender == Gender.None || wearerGender == Gender.None)
		{
			return true;
		}
		return gender == wearerGender;
	}

	public bool CorrectAgeForWearing(Pawn pawn)
	{
		if (ModsConfig.BiotechActive && !developmentalStageFilter.Has(pawn.DevelopmentalStage))
		{
			return false;
		}
		return true;
	}

	public bool PawnCanWear(Gender gender = Gender.None, DevelopmentalStage developmentalStage = DevelopmentalStage.Adult)
	{
		if (!CorrectGenderForWearing(gender))
		{
			return false;
		}
		if (!developmentalStageFilter.Has(developmentalStage))
		{
			return false;
		}
		return true;
	}

	public bool PawnCanWear(Pawn pawn, bool ignoreGender = false)
	{
		return PawnCanWear((!ignoreGender) ? pawn.gender : Gender.None, pawn.DevelopmentalStage);
	}

	public static void ResetStaticData()
	{
		apparelRelevantGroups = DefDatabase<ThingDef>.AllDefs.Where((ThingDef td) => td.IsApparel).SelectMany((ThingDef td) => td.apparel.bodyPartGroups).Distinct()
			.ToArray();
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__56))]
	public IEnumerable<string> ConfigErrors(ThingDef parentDef)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__56(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__parentDef = parentDef
		};
	}

	public void PostLoadSpecial(ThingDef thingDef)
	{
	}

	public bool CoversBodyPart(BodyPartRecord partRec)
	{
		for (int i = 0; i < partRec.groups.Count; i++)
		{
			if (bodyPartGroups.Contains(partRec.groups[i]))
			{
				return true;
			}
		}
		return false;
	}

	public bool CoversBodyPartGroup(BodyPartGroupDef group)
	{
		return bodyPartGroups.NotNullAndContains(group);
	}

	public string GetCoveredOuterPartsString(BodyDef body)
	{
		return (from part in body.AllParts.Where((BodyPartRecord x) => x.depth == BodyPartDepth.Outside && x.groups.Any((BodyPartGroupDef y) => bodyPartGroups.Contains(y))).Distinct()
			select part.Label).ToCommaList().CapitalizeFirst();
	}

	public string GetLayersString()
	{
		return layers.Select((ApparelLayerDef layer) => layer.label).ToCommaList().CapitalizeFirst();
	}

	public BodyPartGroupDef[] GetInterferingBodyPartGroups(BodyDef body)
	{
		if (interferingBodyPartGroups == null || interferingBodyPartGroups.Length != DefDatabase<BodyDef>.DefCount)
		{
			interferingBodyPartGroups = new BodyPartGroupDef[DefDatabase<BodyDef>.DefCount][];
		}
		if (interferingBodyPartGroups[body.index] == null)
		{
			BodyPartGroupDef[] array = (from bpgd in body.AllParts.Where((BodyPartRecord part) => part.groups.Any((BodyPartGroupDef @group) => bodyPartGroups.Contains(@group))).ToArray().SelectMany((BodyPartRecord bpr) => bpr.groups)
					.Distinct()
				where apparelRelevantGroups.Contains(bpgd)
				select bpgd).ToArray();
			interferingBodyPartGroups[body.index] = array;
		}
		return interferingBodyPartGroups[body.index];
	}

	public void ResolveReferencesSpecial()
	{
		if (renderNodeProperties != null)
		{
			for (int i = 0; i < renderNodeProperties.Count; i++)
			{
				renderNodeProperties[i].ResolveReferencesRecursive();
			}
		}
	}
}
