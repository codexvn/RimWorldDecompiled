using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002964 RID: 10596
	[StaticConstructorOnStartup]
	public class MapPortal : Building, IThingHolder
	{
		// Token: 0x17002373 RID: 9075
		// (get) Token: 0x0600E894 RID: 59540 RVA: 0x0043B52C File Offset: 0x0043972C
		public Map PocketMap
		{
			get
			{
				Map map = this.pocketMap;
				bool flag;
				if (map == null)
				{
					flag = false;
				}
				else
				{
					MapParent parent = map.Parent;
					bool? flag2 = (parent != null) ? new bool?(parent.HasMap) : null;
					bool flag3 = false;
					flag = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
				}
				if (flag)
				{
					this.pocketMap = null;
				}
				return this.pocketMap;
			}
		}

		// Token: 0x17002374 RID: 9076
		// (get) Token: 0x0600E895 RID: 59541 RVA: 0x0043B588 File Offset: 0x00439788
		public bool PocketMapExists
		{
			get
			{
				return this.PocketMap != null;
			}
		}

		// Token: 0x17002375 RID: 9077
		// (get) Token: 0x0600E896 RID: 59542 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool AutoDraftOnEnter
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002376 RID: 9078
		// (get) Token: 0x0600E897 RID: 59543 RVA: 0x0043B593 File Offset: 0x00439793
		protected virtual Texture2D EnterTex
		{
			get
			{
				return MapPortal.DefaultEnterTex;
			}
		}

		// Token: 0x17002377 RID: 9079
		// (get) Token: 0x0600E898 RID: 59544 RVA: 0x0043B59A File Offset: 0x0043979A
		public virtual string EnterString
		{
			get
			{
				return "EnterPortal".Translate(this.Label);
			}
		}

		// Token: 0x17002378 RID: 9080
		// (get) Token: 0x0600E899 RID: 59545 RVA: 0x0043B5B6 File Offset: 0x004397B6
		public virtual string CancelEnterString
		{
			get
			{
				return "CommandCancelEnterPortal".Translate();
			}
		}

		// Token: 0x17002379 RID: 9081
		// (get) Token: 0x0600E89A RID: 59546 RVA: 0x0043B5C7 File Offset: 0x004397C7
		public virtual string EnteringString
		{
			get
			{
				return "EnteringPortal".Translate(this.Label);
			}
		}

		// Token: 0x1700237A RID: 9082
		// (get) Token: 0x0600E89B RID: 59547 RVA: 0x0043B5E3 File Offset: 0x004397E3
		public bool LoadInProgress
		{
			get
			{
				return this.leftToLoad != null && this.leftToLoad.Any<TransferableOneWay>();
			}
		}

		// Token: 0x1700237B RID: 9083
		// (get) Token: 0x0600E89C RID: 59548 RVA: 0x0043B5FC File Offset: 0x004397FC
		public bool AnyPawnCanLoadAnythingNow
		{
			get
			{
				if (!this.LoadInProgress)
				{
					return false;
				}
				if (!base.Spawned)
				{
					return false;
				}
				IReadOnlyList<Pawn> allPawnsSpawned = base.Map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].CurJobDef == JobDefOf.HaulToPortal && ((JobDriver_HaulToPortal)allPawnsSpawned[i].jobs.curDriver).MapPortal == this)
					{
						return true;
					}
					if (allPawnsSpawned[i].CurJobDef == JobDefOf.EnterPortal && ((JobDriver_EnterPortal)allPawnsSpawned[i].jobs.curDriver).MapPortal == this)
					{
						return true;
					}
				}
				for (int j = 0; j < allPawnsSpawned.Count; j++)
				{
					Pawn_MindState mindState = allPawnsSpawned[j].mindState;
					Thing thing;
					if (mindState == null)
					{
						thing = null;
					}
					else
					{
						PawnDuty duty = mindState.duty;
						thing = ((duty != null) ? duty.focus.Thing : null);
					}
					Thing thing2 = thing;
					if (thing2 != null && thing2 == this && allPawnsSpawned[j].CanReach(thing2, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
					{
						return true;
					}
				}
				for (int k = 0; k < allPawnsSpawned.Count; k++)
				{
					if (allPawnsSpawned[k].IsColonist && EnterPortalUtility.HasJobOnPortal(allPawnsSpawned[k], this))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x0600E89D RID: 59549 RVA: 0x0043B73C File Offset: 0x0043993C
		public override void ExposeData()
		{
			base.ExposeData();
			Map map = this.pocketMap;
			bool flag;
			if (map == null)
			{
				flag = false;
			}
			else
			{
				MapParent parent = map.Parent;
				bool? flag2 = (parent != null) ? new bool?(parent.HasMap) : null;
				bool flag3 = false;
				flag = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
			}
			if (flag)
			{
				this.pocketMap = null;
			}
			Scribe_References.Look<Map>(ref this.pocketMap, "pocketMap", false);
			Scribe_References.Look<PocketMapExit>(ref this.exit, "exit", false);
			Scribe_Values.Look<bool>(ref this.beenEntered, "beenEntered", false, false);
			Scribe_Collections.Look<TransferableOneWay>(ref this.leftToLoad, "leftToLoad", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				List<TransferableOneWay> list = this.leftToLoad;
				if (list == null)
				{
					return;
				}
				list.RemoveAll((TransferableOneWay x) => x.AnyThing == null);
			}
		}

		// Token: 0x0600E89E RID: 59550 RVA: 0x0043B81A File Offset: 0x00439A1A
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.containerProxy = new PortalContainerProxy
			{
				portal = this
			};
		}

		// Token: 0x0600E89F RID: 59551 RVA: 0x0043B838 File Offset: 0x00439A38
		protected override void Tick()
		{
			base.Tick();
			if (this.IsHashIntervalTick(60) && base.Spawned && this.LoadInProgress && !this.notifiedCantLoadMore && !this.AnyPawnCanLoadAnythingNow)
			{
				TransferableOneWay transferableOneWay = this.leftToLoad[0];
				if (((transferableOneWay != null) ? transferableOneWay.AnyThing : null) != null)
				{
					this.notifiedCantLoadMore = true;
					Messages.Message("MessageCantLoadMoreIntoPortal".Translate(this.Label, Faction.OfPlayer.def.pawnsPlural, this.leftToLoad[0].AnyThing), this, MessageTypeDefOf.CautionInput, true);
				}
			}
		}

		// Token: 0x0600E8A0 RID: 59552 RVA: 0x000026EA File Offset: 0x000008EA
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
		}

		// Token: 0x0600E8A1 RID: 59553 RVA: 0x0043B8F4 File Offset: 0x00439AF4
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.containerProxy;
		}

		// Token: 0x0600E8A2 RID: 59554 RVA: 0x0043B8FC File Offset: 0x00439AFC
		public void Notify_ThingAdded(Thing t)
		{
			this.SubtractFromToLoadList(t, t.stackCount);
		}

		// Token: 0x0600E8A3 RID: 59555 RVA: 0x0043B90C File Offset: 0x00439B0C
		public void AddToTheToLoadList(TransferableOneWay t, int count)
		{
			if (!t.HasAnyThing || count <= 0)
			{
				return;
			}
			if (this.leftToLoad == null)
			{
				this.leftToLoad = new List<TransferableOneWay>();
			}
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatching<TransferableOneWay>(t.AnyThing, this.leftToLoad, TransferAsOneMode.PodsOrCaravanPacking);
			if (transferableOneWay != null)
			{
				for (int i = 0; i < t.things.Count; i++)
				{
					if (!transferableOneWay.things.Contains(t.things[i]))
					{
						transferableOneWay.things.Add(t.things[i]);
					}
				}
				if (transferableOneWay.CanAdjustBy(count).Accepted)
				{
					transferableOneWay.AdjustBy(count);
					return;
				}
			}
			else
			{
				TransferableOneWay transferableOneWay2 = new TransferableOneWay();
				this.leftToLoad.Add(transferableOneWay2);
				transferableOneWay2.things.AddRange(t.things);
				transferableOneWay2.AdjustTo(count);
			}
		}

		// Token: 0x0600E8A4 RID: 59556 RVA: 0x0043B9D8 File Offset: 0x00439BD8
		public int SubtractFromToLoadList(Thing t, int count)
		{
			if (this.leftToLoad == null)
			{
				return 0;
			}
			TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatchingDesperate(t, this.leftToLoad, TransferAsOneMode.PodsOrCaravanPacking);
			if (transferableOneWay == null)
			{
				return 0;
			}
			if (transferableOneWay.CountToTransfer <= 0)
			{
				return 0;
			}
			int num = Mathf.Min(count, transferableOneWay.CountToTransfer);
			transferableOneWay.AdjustBy(-num);
			transferableOneWay.things.Remove(t);
			if (transferableOneWay.CountToTransfer <= 0)
			{
				this.leftToLoad.Remove(transferableOneWay);
			}
			return num;
		}

		// Token: 0x0600E8A5 RID: 59557 RVA: 0x0043BA48 File Offset: 0x00439C48
		public void CancelLoad()
		{
			Lord lord = base.Map.lordManager.lords.FirstOrDefault(delegate(Lord l)
			{
				LordJob_LoadAndEnterPortal lordJob_LoadAndEnterPortal = l.LordJob as LordJob_LoadAndEnterPortal;
				return lordJob_LoadAndEnterPortal != null && lordJob_LoadAndEnterPortal.portal == this;
			});
			if (lord != null)
			{
				base.Map.lordManager.RemoveLord(lord);
			}
			this.leftToLoad.Clear();
		}

		// Token: 0x0600E8A6 RID: 59558 RVA: 0x0043BA96 File Offset: 0x00439C96
		public virtual bool IsEnterable(out string reason)
		{
			reason = "";
			return true;
		}

		// Token: 0x0600E8A7 RID: 59559 RVA: 0x0043BAA0 File Offset: 0x00439CA0
		public virtual Map GetOtherMap()
		{
			if (this.PocketMap == null)
			{
				this.GeneratePocketMap();
			}
			return this.PocketMap;
		}

		// Token: 0x0600E8A8 RID: 59560 RVA: 0x0043BAB6 File Offset: 0x00439CB6
		public virtual IntVec3 GetDestinationLocation()
		{
			PocketMapExit pocketMapExit = this.exit;
			if (pocketMapExit == null)
			{
				return IntVec3.Invalid;
			}
			return pocketMapExit.Position;
		}

		// Token: 0x0600E8A9 RID: 59561 RVA: 0x0043BAD0 File Offset: 0x00439CD0
		public virtual void OnEntered(Pawn pawn)
		{
			this.Notify_ThingAdded(pawn);
			if (!this.beenEntered)
			{
				this.beenEntered = true;
				if (!this.def.portal.enteredLetterLabel.NullOrEmpty())
				{
					Find.LetterStack.ReceiveLetter(this.def.portal.enteredLetterLabel, this.def.portal.enteredLetterText.Formatted(pawn.Named("PAWN")), this.def.portal.enteredLetterDef, this.exit, null, null, null, null, 0, true);
				}
			}
			if (Find.CurrentMap != base.Map)
			{
				if (Find.CurrentMap == this.exit.Map)
				{
					SoundDef traverseSound = this.def.portal.traverseSound;
					if (traverseSound == null)
					{
						return;
					}
					traverseSound.PlayOneShot(this.exit);
				}
				return;
			}
			SoundDef traverseSound2 = this.def.portal.traverseSound;
			if (traverseSound2 == null)
			{
				return;
			}
			traverseSound2.PlayOneShot(this);
		}

		// Token: 0x0600E8AA RID: 59562 RVA: 0x0043BBCF File Offset: 0x00439DCF
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			string disabledReason;
			yield return new Command_Action
			{
				action = delegate()
				{
					Dialog_EnterPortal window = new Dialog_EnterPortal(this);
					Find.WindowStack.Add(window);
				},
				icon = this.EnterTex,
				defaultLabel = this.EnterString + "...",
				defaultDesc = "CommandEnterPortalDesc".Translate(this.Label),
				Disabled = !this.IsEnterable(out disabledReason),
				disabledReason = disabledReason
			};
			if (this.LoadInProgress)
			{
				yield return new Command_Action
				{
					action = new Action(this.CancelLoad),
					icon = MapPortal.CancelEnterTex,
					defaultLabel = this.CancelEnterString,
					defaultDesc = "CommandCancelEnterPortalDesc".Translate()
				};
			}
			if (this.pocketMap != null)
			{
				yield return new Command_Action
				{
					defaultLabel = "CommandViewPocketMapLabel".Translate(this.def.portal.pocketMapGenerator.label),
					defaultDesc = "CommandViewPocketMapDesc".Translate(this.def.portal.pocketMapGenerator.label),
					icon = MapPortal.ViewPocketMapTex,
					action = delegate()
					{
						CameraJumper.TryJumpAndSelect(this.exit, CameraJumper.MovementMode.Pan);
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0600E8AB RID: 59563 RVA: 0x0043BBDF File Offset: 0x00439DDF
		private void GeneratePocketMap()
		{
			PocketMapUtility.currentlyGeneratingPortal = this;
			this.pocketMap = this.GeneratePocketMapInt();
			PocketMapUtility.currentlyGeneratingPortal = null;
		}

		// Token: 0x0600E8AC RID: 59564 RVA: 0x0043BBFC File Offset: 0x00439DFC
		protected virtual Map GeneratePocketMapInt()
		{
			return PocketMapUtility.GeneratePocketMap(new IntVec3(this.def.portal.pocketMapSize, 1, this.def.portal.pocketMapSize), this.def.portal.pocketMapGenerator, this.GetExtraGenSteps(), base.Map);
		}

		// Token: 0x0600E8AD RID: 59565 RVA: 0x0043BC50 File Offset: 0x00439E50
		protected virtual IEnumerable<GenStepWithParams> GetExtraGenSteps()
		{
			return Enumerable.Empty<GenStepWithParams>();
		}

		// Token: 0x04009AF1 RID: 39665
		private static readonly Texture2D ViewPocketMapTex = ContentFinder<Texture2D>.Get("UI/Commands/ViewCave", true);

		// Token: 0x04009AF2 RID: 39666
		private static readonly Texture2D CancelEnterTex = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x04009AF3 RID: 39667
		private static readonly Texture2D DefaultEnterTex = ContentFinder<Texture2D>.Get("UI/Commands/EnterCave", true);

		// Token: 0x04009AF4 RID: 39668
		protected Map pocketMap;

		// Token: 0x04009AF5 RID: 39669
		public PocketMapExit exit;

		// Token: 0x04009AF6 RID: 39670
		protected bool beenEntered;

		// Token: 0x04009AF7 RID: 39671
		public List<TransferableOneWay> leftToLoad;

		// Token: 0x04009AF8 RID: 39672
		public PortalContainerProxy containerProxy;

		// Token: 0x04009AF9 RID: 39673
		public bool notifiedCantLoadMore;
	}
}
