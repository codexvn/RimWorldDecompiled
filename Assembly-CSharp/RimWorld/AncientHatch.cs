using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002AF3 RID: 10995
	[StaticConstructorOnStartup]
	public class AncientHatch : MapPortal
	{
		// Token: 0x1700265A RID: 9818
		// (get) Token: 0x0600F54A RID: 62794 RVA: 0x004746E0 File Offset: 0x004728E0
		private CompHackable Hackable
		{
			get
			{
				CompHackable result;
				if ((result = this.hackableInt) == null)
				{
					result = (this.hackableInt = base.GetComp<CompHackable>());
				}
				return result;
			}
		}

		// Token: 0x0600F54B RID: 62795 RVA: 0x00474706 File Offset: 0x00472906
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<TileMutatorWorker_Stockpile.StockpileType>(ref this.stockpileType, "stockpileType", TileMutatorWorker_Stockpile.StockpileType.Medicine, false);
			Scribe_Defs.Look<LayoutDef>(ref this.layout, "layout");
			Scribe_Values.Look<bool>(ref this.isSealed, "isSealed", false, false);
		}

		// Token: 0x0600F54C RID: 62796 RVA: 0x00474742 File Offset: 0x00472942
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.openGraphicData = new GraphicData();
			this.openGraphicData.CopyFrom(this.def.graphicData);
			this.openGraphicData.texPath = "Things/Building/AncientHatch/AncientHatch_Open";
		}

		// Token: 0x0600F54D RID: 62797 RVA: 0x00474780 File Offset: 0x00472980
		public override void Print(SectionLayer layer)
		{
			string text;
			if (this.IsEnterable(out text))
			{
				this.openGraphicData.Graphic.Print(layer, this, 0f);
				return;
			}
			this.Graphic.Print(layer, this, 0f);
		}

		// Token: 0x0600F54E RID: 62798 RVA: 0x004747C1 File Offset: 0x004729C1
		protected override IEnumerable<GenStepWithParams> GetExtraGenSteps()
		{
			if (this.layout != null)
			{
				yield return new GenStepWithParams(GenStepDefOf.AncientStockpile, new GenStepParams
				{
					layout = this.layout
				});
			}
			else
			{
				yield return new GenStepWithParams(GenStepDefOf.AncientStockpile, default(GenStepParams));
			}
			yield break;
		}

		// Token: 0x0600F54F RID: 62799 RVA: 0x004747D4 File Offset: 0x004729D4
		public override bool IsEnterable(out string reason)
		{
			if (!this.Hackable.IsHacked)
			{
				reason = "Locked".Translate();
				return false;
			}
			if (this.isSealed)
			{
				reason = "Sealed".Translate();
				return false;
			}
			return base.IsEnterable(out reason);
		}

		// Token: 0x0600F550 RID: 62800 RVA: 0x00474823 File Offset: 0x00472A23
		public void Seal()
		{
			if (!base.PocketMapExists)
			{
				Log.Error("Tried to seal ancient hatch but pocket map doesn't exist");
				return;
			}
			PocketMapUtility.DestroyPocketMap(this.pocketMap);
			base.DirtyMapMesh(base.Map);
			this.isSealed = true;
		}

		// Token: 0x0600F551 RID: 62801 RVA: 0x00474858 File Offset: 0x00472A58
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder(base.GetInspectString());
			if (this.isSealed)
			{
				stringBuilder.AppendLineIfNotEmpty();
				stringBuilder.Append("HatchSealed".Translate());
			}
			else if (this.Hackable.IsHacked)
			{
				stringBuilder.AppendLineIfNotEmpty();
				stringBuilder.Append("HatchUnlocked".Translate());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600F552 RID: 62802 RVA: 0x004748C8 File Offset: 0x00472AC8
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (base.PocketMapExists)
			{
				yield return new Command_Action
				{
					defaultLabel = "SealHatch".Translate(),
					defaultDesc = "SealHatchDesc".Translate(),
					icon = AncientHatch.SealHatchIcon.Texture,
					action = delegate()
					{
						Find.Targeter.BeginTargeting(TargetingParameters.ForColonist(), delegate(LocalTargetInfo target)
						{
							string text = "";
							List<Pawn> list = new List<Pawn>();
							List<Pawn> list2 = new List<Pawn>();
							foreach (Pawn pawn in base.PocketMap.mapPawns.AllPawns)
							{
								if (pawn.Faction == Faction.OfPlayer)
								{
									if (pawn.RaceProps.Humanlike)
									{
										list.Add(pawn);
									}
									else
									{
										list2.Add(pawn);
									}
								}
							}
							if (list.Count > 0)
							{
								string[] array = new string[6];
								array[0] = text;
								array[1] = "\n\n";
								array[2] = ("GravEngineWarning".Translate() + ": ").Colorize(ColorLibrary.RedReadable);
								array[3] = "PeopleWillBeLeftBehind".Translate().Resolve();
								array[4] = ":\n";
								array[5] = (from p in list
								select p.NameFullColored.Resolve()).ToLineList("  - ", true);
								text = string.Concat(array);
							}
							if (list2.Count > 0)
							{
								string[] array2 = new string[6];
								array2[0] = text;
								array2[1] = "\n\n";
								array2[2] = ("GravEngineWarning".Translate() + ": ").Colorize(ColorLibrary.RedReadable);
								array2[3] = "AnimalsWillBeLeftBehind".Translate().Resolve();
								array2[4] = ":\n";
								array2[5] = (from p in list2
								select p.NameFullColored.Resolve()).ToLineList("  - ", true);
								text = string.Concat(array2);
							}
							Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmSealHatch".Translate(text), delegate()
							{
								Pawn pawn2 = target.Pawn;
								if (pawn2 == null)
								{
									return;
								}
								pawn2.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Seal, this), new JobTag?(JobTag.Misc), false);
							}, false, null, WindowLayer.Dialog));
						}, delegate(LocalTargetInfo target)
						{
							Pawn pawn = target.Pawn;
							if (pawn != null && pawn.IsColonistPlayerControlled)
							{
								GenDraw.DrawTargetHighlight(target);
							}
						}, (LocalTargetInfo target) => this.ValidateSealer(target).Accepted, null, null, AncientHatch.SealHatchIcon.Texture, true, delegate(LocalTargetInfo target)
						{
							AcceptanceReport acceptanceReport = this.ValidateSealer(target);
							Pawn pawn = target.Pawn;
							if (pawn != null && pawn.IsColonistPlayerControlled && !acceptanceReport.Accepted)
							{
								if (!acceptanceReport.Reason.NullOrEmpty())
								{
									Widgets.MouseAttachedLabel(("CannotChooseSealer".Translate() + ": " + acceptanceReport.Reason.CapitalizeFirst()).Colorize(ColorLibrary.RedReadable), 0f, 0f, null);
									return;
								}
								Widgets.MouseAttachedLabel("CannotChooseSealer".Translate(), 0f, 0f, null);
							}
						}, null);
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0600F553 RID: 62803 RVA: 0x004748D8 File Offset: 0x00472AD8
		private AcceptanceReport ValidateSealer(LocalTargetInfo target)
		{
			Pawn pawn = target.Thing as Pawn;
			if (pawn == null)
			{
				return false;
			}
			if (!pawn.CanReach(this, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return "NoPath".Translate();
			}
			if (pawn.Downed)
			{
				return "DownedLower".Translate();
			}
			return true;
		}

		// Token: 0x0400A193 RID: 41363
		public TileMutatorWorker_Stockpile.StockpileType stockpileType;

		// Token: 0x0400A194 RID: 41364
		public LayoutDef layout;

		// Token: 0x0400A195 RID: 41365
		private bool isSealed;

		// Token: 0x0400A196 RID: 41366
		private CompHackable hackableInt;

		// Token: 0x0400A197 RID: 41367
		private GraphicData openGraphicData;

		// Token: 0x0400A198 RID: 41368
		public static CachedTexture SealHatchIcon = new CachedTexture("UI/Commands/SealHatch");

		// Token: 0x0400A199 RID: 41369
		private const string OpenTexturePath = "Things/Building/AncientHatch/AncientHatch_Open";
	}
}
