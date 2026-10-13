using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200372C RID: 14124
	public class StatPart_WeaponTraitsMarketValueOffset : StatPart
	{
		// Token: 0x06013931 RID: 80177 RVA: 0x005CDAD8 File Offset: 0x005CBCD8
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				CompBladelinkWeapon compBladelinkWeapon = req.Thing.TryGetComp<CompBladelinkWeapon>();
				if (compBladelinkWeapon != null)
				{
					List<WeaponTraitDef> traitsListForReading = compBladelinkWeapon.TraitsListForReading;
					if (!traitsListForReading.NullOrEmpty<WeaponTraitDef>())
					{
						for (int i = 0; i < traitsListForReading.Count; i++)
						{
							val += traitsListForReading[i].marketValueOffset;
						}
					}
				}
			}
		}

		// Token: 0x06013932 RID: 80178 RVA: 0x005CDB30 File Offset: 0x005CBD30
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				CompBladelinkWeapon compBladelinkWeapon = req.Thing.TryGetComp<CompBladelinkWeapon>();
				if (compBladelinkWeapon != null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					List<WeaponTraitDef> traitsListForReading = compBladelinkWeapon.TraitsListForReading;
					if (!traitsListForReading.NullOrEmpty<WeaponTraitDef>())
					{
						for (int i = 0; i < traitsListForReading.Count; i++)
						{
							if (traitsListForReading[i].marketValueOffset != 0f)
							{
								stringBuilder.AppendLine(traitsListForReading[i].LabelCap + ": " + traitsListForReading[i].marketValueOffset.ToStringByStyle(ToStringStyle.Money, ToStringNumberSense.Offset));
							}
						}
					}
					return stringBuilder.ToString();
				}
			}
			return null;
		}
	}
}
