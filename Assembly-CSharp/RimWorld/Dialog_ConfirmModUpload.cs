using System;
using UnityEngine;
using Verse;

namespace RimWorld;

public class Dialog_ConfirmModUpload : Dialog_MessageBox
{
	private ModMetaData mod;

	public Dialog_ConfirmModUpload(ModMetaData mod, Action acceptAction)
		: base("ConfirmSteamWorkshopUpload".Translate(), "Confirm".Translate(), acceptAction, "GoBack".Translate(), null, null, buttonADestructive: true, acceptAction)
	{
		this.mod = mod;
	}

	public override void DoWindowContents(Rect inRect)
	{
		base.DoWindowContents(inRect);
		Vector2 vector = new Vector2(inRect.x + 10f, inRect.height - 35f - 24f - 10f);
		Widgets.Checkbox(vector, ref mod.translationMod);
		Widgets.Label(new Rect(vector.x + 24f + 10f, vector.y + (24f - Text.LineHeight) / 2f, inRect.width / 2f, 24f), "TagAsTranslation".Translate());
		vector.y -= 34f;
		if (!mod.uploadBuildAssetPack)
		{
			mod.uploadSkipSourceTextures = false;
		}
		AssetPackOption(vector, inRect, ref mod.uploadSkipSourceTextures, "UploadSkipSourceTextures", !mod.uploadBuildAssetPack);
		vector.y -= 34f;
		AssetPackOption(vector, inRect, ref mod.uploadBuildAssetPack, "UploadBuildAssetPack", disabled: false);
	}

	private static void AssetPackOption(Vector2 checkBoxPos, Rect inRect, ref bool value, string key, bool disabled)
	{
		Widgets.Checkbox(checkBoxPos, ref value, 24f, disabled);
		Rect rect = new Rect(checkBoxPos.x + 24f + 10f, checkBoxPos.y + (24f - Text.LineHeight) / 2f, inRect.width / 2f, 24f);
		if (disabled)
		{
			GUI.color = Color.gray;
		}
		Widgets.Label(rect, key.Translate());
		GUI.color = Color.white;
		TooltipHandler.TipRegion(new Rect(checkBoxPos.x, checkBoxPos.y, 34f + rect.width, 24f), (key + "Tip").Translate());
	}
}
