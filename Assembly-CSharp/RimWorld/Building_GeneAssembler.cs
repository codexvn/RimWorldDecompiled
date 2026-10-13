using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002A01 RID: 10753
	[StaticConstructorOnStartup]
	public class Building_GeneAssembler : Building, IThingHolder
	{
		// Token: 0x170024CF RID: 9423
		// (get) Token: 0x0600EDC0 RID: 60864 RVA: 0x0044F856 File Offset: 0x0044DA56
		public float ProgressPercent
		{
			get
			{
				return this.workDone / this.totalWorkRequired;
			}
		}

		// Token: 0x170024D0 RID: 9424
		// (get) Token: 0x0600EDC1 RID: 60865 RVA: 0x0044F865 File Offset: 0x0044DA65
		public bool Working
		{
			get
			{
				return this.workingInt;
			}
		}

		// Token: 0x170024D1 RID: 9425
		// (get) Token: 0x0600EDC2 RID: 60866 RVA: 0x0044F870 File Offset: 0x0044DA70
		private CompPowerTrader PowerTraderComp
		{
			get
			{
				CompPowerTrader result;
				if ((result = this.cachedPowerComp) == null)
				{
					result = (this.cachedPowerComp = this.TryGetComp<CompPowerTrader>());
				}
				return result;
			}
		}

		// Token: 0x170024D2 RID: 9426
		// (get) Token: 0x0600EDC3 RID: 60867 RVA: 0x0044F896 File Offset: 0x0044DA96
		public bool PowerOn
		{
			get
			{
				return this.PowerTraderComp.PowerOn;
			}
		}

		// Token: 0x170024D3 RID: 9427
		// (get) Token: 0x0600EDC4 RID: 60868 RVA: 0x0044F8A3 File Offset: 0x0044DAA3
		public List<Thing> ConnectedFacilities
		{
			get
			{
				return this.TryGetComp<CompAffectedByFacilities>().LinkedFacilitiesListForReading;
			}
		}

		// Token: 0x170024D4 RID: 9428
		// (get) Token: 0x0600EDC5 RID: 60869 RVA: 0x0044F8B0 File Offset: 0x0044DAB0
		public int ArchitesCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.innerContainer.Count; i++)
				{
					if (this.innerContainer[i].def == ThingDefOf.ArchiteCapsule)
					{
						num += this.innerContainer[i].stackCount;
					}
				}
				return num;
			}
		}

		// Token: 0x170024D5 RID: 9429
		// (get) Token: 0x0600EDC6 RID: 60870 RVA: 0x0044F902 File Offset: 0x0044DB02
		public int ArchitesRequiredNow
		{
			get
			{
				return this.architesRequired - this.ArchitesCount;
			}
		}

		// Token: 0x170024D6 RID: 9430
		// (get) Token: 0x0600EDC7 RID: 60871 RVA: 0x0044F914 File Offset: 0x0044DB14
		private HashSet<Thing> UsedFacilities
		{
			get
			{
				this.tmpUsedFacilities.Clear();
				if (!this.genepacksToRecombine.NullOrEmpty<Genepack>())
				{
					List<Thing> connectedFacilities = this.ConnectedFacilities;
					for (int i = 0; i < this.genepacksToRecombine.Count; i++)
					{
						for (int j = 0; j < connectedFacilities.Count; j++)
						{
							if (!this.tmpUsedFacilities.Contains(connectedFacilities[j]))
							{
								CompGenepackContainer compGenepackContainer = connectedFacilities[j].TryGetComp<CompGenepackContainer>();
								if (compGenepackContainer != null && compGenepackContainer.ContainedGenepacks.Contains(this.genepacksToRecombine[i]))
								{
									this.tmpUsedFacilities.Add(connectedFacilities[j]);
									break;
								}
							}
						}
					}
				}
				return this.tmpUsedFacilities;
			}
		}

		// Token: 0x170024D7 RID: 9431
		// (get) Token: 0x0600EDC8 RID: 60872 RVA: 0x0044F9C4 File Offset: 0x0044DBC4
		public AcceptanceReport CanBeWorkedOnNow
		{
			get
			{
				if (!this.Working)
				{
					return false;
				}
				if (this.ArchitesRequiredNow > 0)
				{
					return false;
				}
				if (!this.PowerOn)
				{
					return "NoPower".Translate().CapitalizeFirst();
				}
				foreach (Thing thing in this.UsedFacilities)
				{
					CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
					if (compPowerTrader != null && !compPowerTrader.PowerOn)
					{
						return "GenebankUnpowered".Translate();
					}
				}
				if (this.MaxComplexity() < this.TotalGCX)
				{
					return "GeneProcessorUnpowered".Translate();
				}
				return true;
			}
		}

		// Token: 0x170024D8 RID: 9432
		// (get) Token: 0x0600EDC9 RID: 60873 RVA: 0x0044FA98 File Offset: 0x0044DC98
		private int TotalGCX
		{
			get
			{
				if (!this.Working)
				{
					return 0;
				}
				if (this.cachedComplexity == null)
				{
					this.cachedComplexity = new int?(0);
					if (!this.genepacksToRecombine.NullOrEmpty<Genepack>())
					{
						List<GeneDefWithType> list = new List<GeneDefWithType>();
						for (int i = 0; i < this.genepacksToRecombine.Count; i++)
						{
							if (this.genepacksToRecombine[i].GeneSet != null)
							{
								for (int j = 0; j < this.genepacksToRecombine[i].GeneSet.GenesListForReading.Count; j++)
								{
									list.Add(new GeneDefWithType(this.genepacksToRecombine[i].GeneSet.GenesListForReading[j], true));
								}
							}
						}
						List<GeneDef> list2 = list.NonOverriddenGenes();
						for (int k = 0; k < list2.Count; k++)
						{
							this.cachedComplexity += list2[k].biostatCpx;
						}
					}
				}
				return this.cachedComplexity.Value;
			}
		}

		// Token: 0x0600EDCA RID: 60874 RVA: 0x0044FBC0 File Offset: 0x0044DDC0
		public override void PostPostMake()
		{
			if (!ModLister.CheckBiotech("Gene assembler"))
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostPostMake();
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x0600EDCB RID: 60875 RVA: 0x0044FBE8 File Offset: 0x0044DDE8
		protected override void Tick()
		{
			base.Tick();
			if (this.IsHashIntervalTick(250))
			{
				bool flag = this.lastWorkedTick + 250 + 2 >= Find.TickManager.TicksGame;
				this.PowerTraderComp.PowerOutput = (flag ? (-base.PowerComp.Props.PowerConsumption) : (-base.PowerComp.Props.idlePowerDraw));
			}
			if (this.Working && this.IsHashIntervalTick(180))
			{
				this.CheckAllContainersValid();
			}
		}

		// Token: 0x0600EDCC RID: 60876 RVA: 0x0044FC74 File Offset: 0x0044DE74
		public void Start(List<Genepack> packs, int architesRequired, string xenotypeName, XenotypeIconDef iconDef)
		{
			this.Reset();
			this.genepacksToRecombine = packs;
			this.architesRequired = architesRequired;
			this.xenotypeName = xenotypeName;
			this.iconDef = iconDef;
			this.workingInt = true;
			this.totalWorkRequired = GeneTuning.ComplexityToCreationHoursCurve.Evaluate((float)this.TotalGCX) * 2500f;
		}

		// Token: 0x0600EDCD RID: 60877 RVA: 0x0044FCC8 File Offset: 0x0044DEC8
		public void DoWork(float workAmount)
		{
			this.workDone += workAmount;
			this.lastWorkAmount = workAmount;
			this.lastWorkedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600EDCE RID: 60878 RVA: 0x0044FCF0 File Offset: 0x0044DEF0
		public void Finish()
		{
			if (!this.genepacksToRecombine.NullOrEmpty<Genepack>())
			{
				SoundDefOf.GeneAssembler_Complete.PlayOneShot(SoundInfo.InMap(this, MaintenanceType.None));
				Xenogerm xenogerm = (Xenogerm)ThingMaker.MakeThing(ThingDefOf.Xenogerm, null);
				xenogerm.Initialize(this.genepacksToRecombine, this.xenotypeName, this.iconDef);
				if (GenPlace.TryPlaceThing(xenogerm, this.InteractionCell, base.Map, ThingPlaceMode.Near, null, null, null, 1))
				{
					Messages.Message("MessageXenogermCompleted".Translate(), xenogerm, MessageTypeDefOf.PositiveEvent, true);
				}
			}
			if (this.architesRequired > 0)
			{
				for (int i = this.innerContainer.Count - 1; i >= 0; i--)
				{
					if (this.innerContainer[i].def == ThingDefOf.ArchiteCapsule)
					{
						Thing thing = this.innerContainer[i].SplitOff(Mathf.Min(this.innerContainer[i].stackCount, this.architesRequired));
						this.architesRequired -= thing.stackCount;
						thing.Destroy(DestroyMode.Vanish);
						if (this.architesRequired <= 0)
						{
							break;
						}
					}
				}
			}
			this.Reset();
		}

		// Token: 0x0600EDCF RID: 60879 RVA: 0x0044FE20 File Offset: 0x0044E020
		public List<Genepack> GetGenepacks(bool includePowered, bool includeUnpowered)
		{
			this.tmpGenepacks.Clear();
			List<Thing> connectedFacilities = this.ConnectedFacilities;
			if (connectedFacilities != null)
			{
				foreach (Thing thing in connectedFacilities)
				{
					CompGenepackContainer compGenepackContainer = thing.TryGetComp<CompGenepackContainer>();
					if (compGenepackContainer != null)
					{
						CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
						bool flag = compPowerTrader == null || compPowerTrader.PowerOn;
						if ((includePowered && flag) || (includeUnpowered && !flag))
						{
							this.tmpGenepacks.AddRange(compGenepackContainer.ContainedGenepacks);
						}
					}
				}
			}
			return this.tmpGenepacks;
		}

		// Token: 0x0600EDD0 RID: 60880 RVA: 0x0044FEC4 File Offset: 0x0044E0C4
		public CompGenepackContainer GetGeneBankHoldingPack(Genepack pack)
		{
			List<Thing> connectedFacilities = this.ConnectedFacilities;
			if (connectedFacilities != null)
			{
				foreach (Thing thing in connectedFacilities)
				{
					CompGenepackContainer compGenepackContainer = thing.TryGetComp<CompGenepackContainer>();
					if (compGenepackContainer != null)
					{
						using (List<Genepack>.Enumerator enumerator2 = compGenepackContainer.ContainedGenepacks.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								if (enumerator2.Current == pack)
								{
									return compGenepackContainer;
								}
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x0600EDD1 RID: 60881 RVA: 0x0044FF64 File Offset: 0x0044E164
		public int MaxComplexity()
		{
			int num = 6;
			List<Thing> connectedFacilities = this.ConnectedFacilities;
			if (connectedFacilities != null)
			{
				foreach (Thing thing in connectedFacilities)
				{
					CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
					if (compPowerTrader == null || compPowerTrader.PowerOn)
					{
						num += (int)thing.GetStatValue(StatDefOf.GeneticComplexityIncrease, true, -1);
					}
				}
			}
			return num;
		}

		// Token: 0x0600EDD2 RID: 60882 RVA: 0x0044FFE0 File Offset: 0x0044E1E0
		private void Reset()
		{
			this.workingInt = false;
			this.genepacksToRecombine = null;
			this.xenotypeName = null;
			this.cachedComplexity = null;
			this.iconDef = XenotypeIconDefOf.Basic;
			this.workDone = 0f;
			this.lastWorkedTick = -999;
			this.architesRequired = 0;
			this.innerContainer.TryDropAll(this.def.hasInteractionCell ? this.InteractionCell : base.Position, base.Map, ThingPlaceMode.Near, null, null, true);
		}

		// Token: 0x0600EDD3 RID: 60883 RVA: 0x00450068 File Offset: 0x0044E268
		private void CheckAllContainersValid()
		{
			if (this.genepacksToRecombine.NullOrEmpty<Genepack>())
			{
				return;
			}
			List<Thing> connectedFacilities = this.ConnectedFacilities;
			for (int i = 0; i < this.genepacksToRecombine.Count; i++)
			{
				bool flag = false;
				for (int j = 0; j < connectedFacilities.Count; j++)
				{
					CompGenepackContainer compGenepackContainer = connectedFacilities[j].TryGetComp<CompGenepackContainer>();
					if (compGenepackContainer != null && compGenepackContainer.ContainedGenepacks.Contains(this.genepacksToRecombine[i]))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					Messages.Message("MessageXenogermCancelledMissingPack".Translate(this), this, MessageTypeDefOf.NegativeEvent, true);
					this.Reset();
					return;
				}
			}
		}

		// Token: 0x0600EDD4 RID: 60884 RVA: 0x00450116 File Offset: 0x0044E316
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600EDD5 RID: 60885 RVA: 0x00450124 File Offset: 0x0044E324
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x0600EDD6 RID: 60886 RVA: 0x0045012C File Offset: 0x0044E32C
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "Recombine".Translate() + "...";
			command_Action.defaultDesc = "RecombineDesc".Translate();
			command_Action.icon = Building_GeneAssembler.RecombineIcon.Texture;
			command_Action.action = delegate()
			{
				Find.WindowStack.Add(new Dialog_CreateXenogerm(this));
			};
			if (!this.def.IsResearchFinished)
			{
				command_Action.Disable("MissingRequiredResearch".Translate() + ": " + (from x in this.def.researchPrerequisites
				where !x.IsFinished
				select x.label).ToCommaList(true, false).CapitalizeFirst());
			}
			else if (!this.PowerOn)
			{
				command_Action.Disable("CannotUseNoPower".Translate());
			}
			else if (!this.GetGenepacks(true, false).Any<Genepack>())
			{
				command_Action.Disable("CannotUseReason".Translate("NoGenepacksAvailable".Translate().CapitalizeFirst()));
			}
			yield return command_Action;
			if (this.Working)
			{
				yield return new Command_Action
				{
					defaultLabel = "CancelXenogerm".Translate(),
					defaultDesc = "CancelXenogermDesc".Translate(),
					action = new Action(this.Reset),
					icon = Building_GeneAssembler.CancelIcon
				};
				if (DebugSettings.ShowDevGizmos)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Finish xenogerm",
						action = new Action(this.Finish)
					};
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600EDD7 RID: 60887 RVA: 0x0045013C File Offset: 0x0044E33C
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (this.Working)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text = text + ("CreatingXenogerm".Translate() + ": " + this.xenotypeName.CapitalizeFirst() + "\n" + "ComplexityTotal".Translate() + ": ") + this.TotalGCX.ToString();
				text += "\n" + "Progress".Translate() + ": " + this.ProgressPercent.ToStringPercent();
				int numTicks = Mathf.RoundToInt((this.totalWorkRequired - this.workDone) / ((this.lastWorkAmount > 0f) ? this.lastWorkAmount : this.GetStatValue(StatDefOf.AssemblySpeedFactor, true, -1)));
				text = text + " (" + "DurationLeft".Translate(numTicks.ToStringTicksToPeriod(true, false, true, true, false)).Resolve() + ")";
				AcceptanceReport canBeWorkedOnNow = this.CanBeWorkedOnNow;
				if (!canBeWorkedOnNow.Accepted && !canBeWorkedOnNow.Reason.NullOrEmpty())
				{
					text = text + "\n" + ("AssemblyPaused".Translate() + ": " + canBeWorkedOnNow.Reason).Colorize(ColorLibrary.RedReadable);
				}
				if (this.architesRequired > 0)
				{
					text = string.Concat(new string[]
					{
						text,
						"\n" + "ArchitesRequired".Translate() + ": ",
						this.ArchitesCount.ToString(),
						" / ",
						this.architesRequired.ToString()
					});
				}
			}
			return text;
		}

		// Token: 0x0600EDD8 RID: 60888 RVA: 0x00450330 File Offset: 0x0044E530
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Collections.Look<Genepack>(ref this.genepacksToRecombine, "genepacksToRecombine", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.workingInt, "workingInt", false, false);
			Scribe_Values.Look<float>(ref this.workDone, "workDone", 0f, false);
			Scribe_Values.Look<float>(ref this.totalWorkRequired, "totalWorkRequired", 0f, false);
			Scribe_Values.Look<int>(ref this.lastWorkedTick, "lastWorkedTick", -999, false);
			Scribe_Values.Look<int>(ref this.architesRequired, "architesRequired", 0, false);
			Scribe_Values.Look<string>(ref this.xenotypeName, "xenotypeName", null, false);
			Scribe_Defs.Look<XenotypeIconDef>(ref this.iconDef, "iconDef");
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.iconDef == null)
			{
				this.iconDef = XenotypeIconDefOf.Basic;
			}
		}

		// Token: 0x04009D50 RID: 40272
		public ThingOwner innerContainer;

		// Token: 0x04009D51 RID: 40273
		private List<Genepack> genepacksToRecombine;

		// Token: 0x04009D52 RID: 40274
		private int architesRequired;

		// Token: 0x04009D53 RID: 40275
		private bool workingInt;

		// Token: 0x04009D54 RID: 40276
		private int lastWorkedTick = -999;

		// Token: 0x04009D55 RID: 40277
		private float workDone;

		// Token: 0x04009D56 RID: 40278
		private float totalWorkRequired;

		// Token: 0x04009D57 RID: 40279
		public string xenotypeName;

		// Token: 0x04009D58 RID: 40280
		public XenotypeIconDef iconDef;

		// Token: 0x04009D59 RID: 40281
		[Unsaved(false)]
		private float lastWorkAmount = -1f;

		// Token: 0x04009D5A RID: 40282
		[Unsaved(false)]
		private CompPowerTrader cachedPowerComp;

		// Token: 0x04009D5B RID: 40283
		[Unsaved(false)]
		private List<Genepack> tmpGenepacks = new List<Genepack>();

		// Token: 0x04009D5C RID: 40284
		[Unsaved(false)]
		private HashSet<Thing> tmpUsedFacilities = new HashSet<Thing>();

		// Token: 0x04009D5D RID: 40285
		[Unsaved(false)]
		private int? cachedComplexity;

		// Token: 0x04009D5E RID: 40286
		private const int CheckContainersInterval = 180;

		// Token: 0x04009D5F RID: 40287
		private static readonly Texture2D CancelIcon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x04009D60 RID: 40288
		private static readonly CachedTexture RecombineIcon = new CachedTexture("UI/Gizmos/RecombineGenes");
	}
}
