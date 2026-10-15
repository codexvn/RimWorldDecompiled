using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse.ProfilerUI;

public abstract class ProfilerRowSection
{
	protected readonly ProfilerSession session;

	public readonly ProfilerRowListView View;

	public Action CopyViewRequested;

	protected abstract string CopyHeader { get; }

	protected virtual bool SupportsSubtreeCopy => true;

	protected virtual float FooterHeight => 0f;

	protected ProfilerRowSection(ProfilerSession session)
	{
		this.session = session;
		View = new ProfilerRowListView(session)
		{
			ToggleExpansion = ToggleExpansionAt,
			ContextMenu = OpenContextMenu
		};
	}

	public abstract void Refresh();

	public abstract void Invalidate();

	public abstract void Notify_SearchChanged();

	public abstract void ExpandAll();

	public abstract void CollapseAll();

	protected abstract void ToggleExpansionAt(int index);

	public virtual void CollapseSelected()
	{
		View.CollapseSelected();
	}

	public virtual void Clear()
	{
		View.SelectQuietly(-1);
		View.Resize(0);
		Invalidate();
	}

	protected virtual void AppendCopyContext(StringBuilder builder)
	{
	}

	protected abstract bool BuildFullSubtree(int index, List<ProfilerRowData> into);

	private void CopySelectedSubtree(bool full)
	{
		int selectedIndex = View.SelectedIndex;
		if (selectedIndex >= 0 && selectedIndex < View.Rows.Count)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(CopyHeader);
			List<ProfilerRowData> list = new List<ProfilerRowData>();
			if (full && BuildFullSubtree(selectedIndex, list))
			{
				ProfilerClipboard.AppendSubtree(stringBuilder, list, 0);
			}
			else
			{
				ProfilerClipboard.AppendSubtree(stringBuilder, View.Rows, selectedIndex);
			}
			ProfilerClipboard.Put(stringBuilder);
		}
	}

	public void AppendCopyText(StringBuilder builder)
	{
		AppendCopyContext(builder);
		builder.AppendLine(CopyHeader);
		ProfilerClipboard.AppendRows(builder, View.Rows);
	}

	public virtual void Draw(Rect rect)
	{
		GUI.BeginGroup(rect);
		try
		{
			float footerHeight = FooterHeight;
			View.DrawHeader(new Rect(0f, 0f, rect.width, 22f));
			View.DrawRows(new Rect(0f, 22f, rect.width, rect.height - 22f - 24f - footerHeight));
			if (footerHeight > 0f)
			{
				DrawFooter(new Rect(0f, rect.height - 24f - footerHeight, rect.width, footerHeight));
			}
			View.DrawBreadcrumbs(new Rect(0f, rect.height - 24f, rect.width, 24f));
		}
		finally
		{
			GUI.EndGroup();
		}
	}

	protected virtual void DrawFooter(Rect rect)
	{
	}

	private void OpenContextMenu()
	{
		List<FloatMenuOption> list = new List<FloatMenuOption>
		{
			new FloatMenuOption("Copy row", delegate
			{
				View.CopySelectedRow();
			})
		};
		if (SupportsSubtreeCopy)
		{
			list.Add(new FloatMenuOption("Copy subtree", delegate
			{
				CopySelectedSubtree(full: true);
			}));
			list.Add(new FloatMenuOption("Copy visible subtree", delegate
			{
				CopySelectedSubtree(full: false);
			}));
		}
		list.Add(new FloatMenuOption("Copy view", delegate
		{
			CopyViewRequested?.Invoke();
		}));
		Find.WindowStack.Add(new FloatMenu(list));
	}
}
