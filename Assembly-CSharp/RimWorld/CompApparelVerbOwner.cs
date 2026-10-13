using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BE1 RID: 11233
	public class CompApparelVerbOwner : ThingComp, IVerbOwner
	{
		// Token: 0x17002770 RID: 10096
		// (get) Token: 0x0600FB67 RID: 64359 RVA: 0x0049039F File Offset: 0x0048E59F
		public CompProperties_ApparelVerbOwner Props
		{
			get
			{
				return this.props as CompProperties_ApparelVerbOwner;
			}
		}

		// Token: 0x17002771 RID: 10097
		// (get) Token: 0x0600FB68 RID: 64360 RVA: 0x004903AC File Offset: 0x0048E5AC
		public Pawn Wearer
		{
			get
			{
				Pawn_ApparelTracker pawn_ApparelTracker = base.ParentHolder as Pawn_ApparelTracker;
				if (pawn_ApparelTracker == null)
				{
					return null;
				}
				return pawn_ApparelTracker.pawn;
			}
		}

		// Token: 0x17002772 RID: 10098
		// (get) Token: 0x0600FB69 RID: 64361 RVA: 0x0013A1A6 File Offset: 0x001383A6
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.parent.def.Verbs;
			}
		}

		// Token: 0x17002773 RID: 10099
		// (get) Token: 0x0600FB6A RID: 64362 RVA: 0x0013A1B8 File Offset: 0x001383B8
		public List<Tool> Tools
		{
			get
			{
				return this.parent.def.tools;
			}
		}

		// Token: 0x17002774 RID: 10100
		// (get) Token: 0x0600FB6B RID: 64363 RVA: 0x0036EC81 File Offset: 0x0036CE81
		public ImplementOwnerTypeDef ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.NativeVerb;
			}
		}

		// Token: 0x17002775 RID: 10101
		// (get) Token: 0x0600FB6C RID: 64364 RVA: 0x004903C4 File Offset: 0x0048E5C4
		public Thing ConstantCaster
		{
			get
			{
				return this.Wearer;
			}
		}

		// Token: 0x0600FB6D RID: 64365 RVA: 0x004903CC File Offset: 0x0048E5CC
		public string UniqueVerbOwnerID()
		{
			return "CompVerbOwner_" + this.parent.ThingID;
		}

		// Token: 0x17002776 RID: 10102
		// (get) Token: 0x0600FB6E RID: 64366 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string GizmoExtraLabel
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600FB6F RID: 64367 RVA: 0x004903E3 File Offset: 0x0048E5E3
		public bool VerbsStillUsableBy(Pawn p)
		{
			return this.Wearer == p;
		}

		// Token: 0x17002777 RID: 10103
		// (get) Token: 0x0600FB70 RID: 64368 RVA: 0x004903EE File Offset: 0x0048E5EE
		public VerbTracker VerbTracker
		{
			get
			{
				if (this.verbTracker == null)
				{
					this.verbTracker = new VerbTracker(this);
				}
				return this.verbTracker;
			}
		}

		// Token: 0x17002778 RID: 10104
		// (get) Token: 0x0600FB71 RID: 64369 RVA: 0x0049040A File Offset: 0x0048E60A
		public List<Verb> AllVerbs
		{
			get
			{
				return this.VerbTracker.AllVerbs;
			}
		}

		// Token: 0x0600FB72 RID: 64370 RVA: 0x00490417 File Offset: 0x0048E617
		public override void PostExposeData()
		{
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.SetupVerbs();
			}
		}

		// Token: 0x0600FB73 RID: 64371 RVA: 0x00490441 File Offset: 0x0048E641
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.SetupVerbs();
		}

		// Token: 0x0600FB74 RID: 64372 RVA: 0x00490450 File Offset: 0x0048E650
		private void SetupVerbs()
		{
			for (int i = 0; i < this.AllVerbs.Count; i++)
			{
				this.AllVerbs[i].caster = this.Wearer;
			}
		}

		// Token: 0x0600FB75 RID: 64373 RVA: 0x0049048A File Offset: 0x0048E68A
		public override IEnumerable<Gizmo> CompGetWornGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetWornGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			bool drafted = this.Wearer.Drafted;
			if ((drafted && !this.Props.displayGizmoWhileDrafted) || (!drafted && !this.Props.displayGizmoWhileUndrafted))
			{
				yield break;
			}
			ThingWithComps gear = this.parent;
			foreach (Verb verb in this.VerbTracker.AllVerbs)
			{
				if (verb.verbProps.hasStandardCommand)
				{
					yield return this.CreateVerbTargetCommand(gear, verb);
				}
			}
			List<Verb>.Enumerator enumerator2 = default(List<Verb>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600FB76 RID: 64374 RVA: 0x0049049C File Offset: 0x0048E69C
		private Command_VerbTarget CreateVerbTargetCommand(Thing gear, Verb verb)
		{
			Command_VerbOwner command_VerbOwner = new Command_VerbOwner(this);
			command_VerbOwner.defaultDesc = gear.def.description;
			command_VerbOwner.hotKey = this.Props.hotKey;
			command_VerbOwner.defaultLabel = verb.verbProps.label;
			command_VerbOwner.verb = verb;
			if (verb.verbProps.defaultProjectile != null && verb.verbProps.commandIcon == null)
			{
				command_VerbOwner.icon = verb.verbProps.defaultProjectile.uiIcon;
				command_VerbOwner.iconAngle = verb.verbProps.defaultProjectile.uiIconAngle;
				command_VerbOwner.iconOffset = verb.verbProps.defaultProjectile.uiIconOffset;
				command_VerbOwner.overrideColor = new Color?(verb.verbProps.defaultProjectile.graphicData.color);
			}
			else
			{
				command_VerbOwner.icon = ((verb.UIIcon != BaseContent.BadTex) ? verb.UIIcon : gear.def.uiIcon);
				command_VerbOwner.iconAngle = gear.def.uiIconAngle;
				command_VerbOwner.iconOffset = gear.def.uiIconOffset;
				command_VerbOwner.defaultIconColor = gear.DrawColor;
			}
			string reason;
			if (!this.Wearer.IsColonistPlayerControlled)
			{
				command_VerbOwner.Disable("CannotOrderNonControlled".Translate());
			}
			else if (verb.verbProps.violent && this.Wearer.WorkTagIsDisabled(WorkTags.Violent))
			{
				command_VerbOwner.Disable("IsIncapableOfViolenceLower".Translate(this.Wearer.LabelShort, this.Wearer).CapitalizeFirst() + ".");
			}
			else if (!this.CanBeUsed(out reason))
			{
				command_VerbOwner.Disable(reason);
			}
			return command_VerbOwner;
		}

		// Token: 0x0600FB77 RID: 64375 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void UsedOnce()
		{
		}

		// Token: 0x0600FB78 RID: 64376 RVA: 0x00490658 File Offset: 0x0048E858
		public virtual bool CanBeUsed(out string reason)
		{
			reason = "";
			if (this.parent.MapHeld == null)
			{
				return false;
			}
			if (this.parent.MapHeld.IsPocketMap)
			{
				if (this.VerbProperties.Any((VerbProperties vp) => !vp.useableInPocketMaps))
				{
					reason = "CannotUseReason_PocketMap".Translate(this.parent.MapHeld.generatorDef.label);
					return false;
				}
			}
			if (this.parent.MapHeld.Biome.inVacuum)
			{
				if (this.VerbProperties.Any((VerbProperties vp) => !vp.useableInVacuum))
				{
					reason = "CannotFunctionInVacuum".Translate();
					return false;
				}
			}
			return true;
		}

		// Token: 0x0400A501 RID: 42241
		private VerbTracker verbTracker;
	}
}
