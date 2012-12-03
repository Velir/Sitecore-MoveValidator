using System.Collections.Generic;
using System.Linq;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Masters;
using Sitecore.Diagnostics;

namespace Sitecore.SharedSource.MoveValidator.CustomSitecore.MoveableItems
{
	public class MoveableSitecoreItem : IMoveableItem
	{
		public string Guid { get; set; }
		public ID Id { get; set; }
		public string Name { get; set; }
		public ID TemplateId { get; set; }
		public ID BranchId { get; set; }


		private Item _item;
		private Item _targetItem;
		private Item GetTargetItem(IMoveableItem iTargetItem)
		{
			if (_targetItem != null) return _targetItem;
				
			Database database = Factory.GetDatabase("master");
			_targetItem = database.GetItem(iTargetItem.Id);
			return _targetItem;
		}



		public MoveableSitecoreItem(Item item)
		{
			Guid = item.ID.ToString();
			Id = item.ID;
			Name = item.Name;
			TemplateId = item.TemplateID;
			BranchId = item.BranchId;

			_item = item;
		}

		private List<string> _branchIds;
		public List<string> BranchTemplateIds
		{
			get { return _branchIds ?? (_branchIds = GetBranchIds()); }
			set { _branchIds = value; }
		}

		private List<string>  GetBranchIds()
		{
			List<string> branchChildrenTemplateIds = _item.Branch.InnerItem.GetChildren().InnerChildren.ToList().Select(x => x.TemplateID.ToString()).ToList();
			return branchChildrenTemplateIds;
		}

		public bool IsDescendantOf(IMoveableItem iItem)
		{
			Database database = _item.Database;
			Item ancestorItem = database.GetItem(iItem.Id);
			return _item.Axes.IsDescendantOf(ancestorItem);
		}

		public bool IsAncestorOf(IMoveableItem iItem)
		{
			Database database = _item.Database;
			Item descendantItem = database.GetItem(iItem.Id);
			return _item.Axes.IsAncestorOf(descendantItem);
		}

		public List<string> InsertOptions
		{
			get
			{
				List<string> retVal = Masters.GetMasters(_item).Select(x => x.ID.ToString()).ToList();
				return retVal;
			}
			set { }
		}

		public string DatabaseName
		{
			get { return _item.Database.Name; }
			set { }
		}

		public void CopyTo(IMoveableItem iTargetItem)
		{
			Item targetItem = GetTargetItem(iTargetItem);
			Log.Audit(this, "Paste from: {0} to {1}", new[] { AuditFormatter.FormatItem(targetItem), AuditFormatter.FormatItem(_item) });
			_item.CopyTo(targetItem, ItemUtil.GetCopyOfName(targetItem, Name));
		}

		public void MoveTo(IMoveableItem iTargetItem)
		{
			Item targetItem = GetTargetItem(iTargetItem);
			Log.Audit(this, "Cut from: {0} to {1}", new[] { AuditFormatter.FormatItem(targetItem), AuditFormatter.FormatItem(_item) });
			_item.MoveTo(targetItem);
		}

		

	}
}