using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020027AE RID: 10158
	public class RitualRoleTag : RitualRole
	{
		// Token: 0x0600DEAB RID: 57003 RVA: 0x00417B74 File Offset: 0x00415D74
		public override bool AppliesToRole(Precept_Role role, out string reason, Precept_Ritual ritual = null, Pawn p = null, bool skipReason = false)
		{
			reason = null;
			if (ritual != null && p != null && p.Ideo != ritual.ideo)
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustHaveIdeoToDoRole".Translate(Find.ActiveLanguageWorker.WithIndefiniteArticle(ritual.ideo.memberName, false, false), Find.ActiveLanguageWorker.WithIndefiniteArticle(base.Label, false, false));
				}
				return false;
			}
			if ((role == null || role.def.roleTags == null || !role.def.roleTags.Contains(this.tag)) && (!this.substitutable || p == null || ritual == null || p.Ideo != ritual.ideo))
			{
				if (p != null)
				{
					IEnumerable<PreceptDef> source = from d in DefDatabase<PreceptDef>.AllDefsListForReading
					where typeof(Precept_Role).IsAssignableFrom(d.preceptClass) && d.roleTags != null && d.roleTags.Contains(this.tag)
					select d;
					if (!skipReason)
					{
						reason = "MessageRitualRoleRequired".Translate(p) + ": " + source.Select(delegate(PreceptDef r)
						{
							Precept_Ritual ritual2 = ritual;
							string text;
							if (ritual2 == null)
							{
								text = null;
							}
							else
							{
								Precept precept = ritual2.ideo.GetPrecept(r);
								text = ((precept != null) ? precept.LabelCap : null);
							}
							return text ?? r.LabelCap;
						}).ToCommaList(false, false);
					}
				}
				return false;
			}
			if (p != null && !p.Faction.IsPlayerSafe())
			{
				if (!skipReason)
				{
					reason = "MessageRitualRoleMustBeColonist".Translate(base.Label);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600DEAC RID: 57004 RVA: 0x00417CF2 File Offset: 0x00415EF2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.tag, "tag", null, false);
		}

		// Token: 0x04009610 RID: 38416
		public string tag;
	}
}
