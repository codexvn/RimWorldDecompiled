using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000B8C RID: 2956
	public class Command_VerbTarget : Command
	{
		// Token: 0x17000B82 RID: 2946
		// (get) Token: 0x06004BA6 RID: 19366 RVA: 0x00181590 File Offset: 0x0017F790
		public override Color IconDrawColor
		{
			get
			{
				if (this.verb.EquipmentSource != null)
				{
					return this.verb.EquipmentSource.DrawColor;
				}
				return base.IconDrawColor;
			}
		}

		// Token: 0x06004BA7 RID: 19367 RVA: 0x001815B8 File Offset: 0x0017F7B8
		public override void DrawIcon(Rect rect, Material buttonMat, GizmoRenderParms parms)
		{
			if (this.ownerThing != null)
			{
				Thing thing = this.ownerThing;
				float alpha = 1f;
				float scale = this.iconDrawScale * 0.85f;
				bool lowLight = parms.lowLight;
				Widgets.ThingIcon(rect, thing, alpha, null, false, scale, lowLight);
				return;
			}
			base.DrawIcon(rect, buttonMat, parms);
		}

		// Token: 0x06004BA8 RID: 19368 RVA: 0x00181608 File Offset: 0x0017F808
		public override void GizmoUpdateOnMouseover()
		{
			if (!this.drawRadius)
			{
				return;
			}
			this.verb.verbProps.DrawRadiusRing(this.verb.caster.Position, this.verb);
			if (!this.groupedVerbs.NullOrEmpty<Verb>())
			{
				foreach (Verb verb in this.groupedVerbs)
				{
					verb.verbProps.DrawRadiusRing(verb.caster.Position, verb);
				}
			}
		}

		// Token: 0x06004BA9 RID: 19369 RVA: 0x001816A8 File Offset: 0x0017F8A8
		public override bool GroupsWith(Gizmo other)
		{
			Command_VerbTarget command_VerbTarget = other as Command_VerbTarget;
			if (command_VerbTarget == null)
			{
				return false;
			}
			if (this.ownerThing != null && command_VerbTarget.ownerThing != null)
			{
				return this.ownerThing.def == command_VerbTarget.ownerThing.def;
			}
			return base.GroupsWith(other);
		}

		// Token: 0x06004BAA RID: 19370 RVA: 0x001816F4 File Offset: 0x0017F8F4
		public override void MergeWith(Gizmo other)
		{
			base.MergeWith(other);
			Command_VerbTarget command_VerbTarget = other as Command_VerbTarget;
			if (command_VerbTarget == null)
			{
				Log.ErrorOnce("Tried to merge Command_VerbTarget with unexpected type", 73406263);
				return;
			}
			if (this.groupedVerbs == null)
			{
				this.groupedVerbs = new List<Verb>();
			}
			this.groupedVerbs.Add(command_VerbTarget.verb);
			if (command_VerbTarget.groupedVerbs != null)
			{
				this.groupedVerbs.AddRange(command_VerbTarget.groupedVerbs);
			}
		}

		// Token: 0x06004BAB RID: 19371 RVA: 0x00181760 File Offset: 0x0017F960
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			Targeter targeter = Find.Targeter;
			if (this.verb.CasterIsPawn && targeter.targetingSource != null && targeter.targetingSource.GetVerb.verbProps == this.verb.verbProps)
			{
				Pawn casterPawn = this.verb.CasterPawn;
				if (!targeter.IsPawnTargeting(casterPawn))
				{
					targeter.targetingSourceAdditionalPawns.Add(casterPawn);
					return;
				}
			}
			else
			{
				Find.Targeter.BeginTargeting(this.verb, null, false, null, null, this.requiresAvailableVerb);
			}
		}

		// Token: 0x040035E8 RID: 13800
		public Verb verb;

		// Token: 0x040035E9 RID: 13801
		private List<Verb> groupedVerbs;

		// Token: 0x040035EA RID: 13802
		public Thing ownerThing;

		// Token: 0x040035EB RID: 13803
		public bool drawRadius = true;

		// Token: 0x040035EC RID: 13804
		public bool requiresAvailableVerb = true;
	}
}
