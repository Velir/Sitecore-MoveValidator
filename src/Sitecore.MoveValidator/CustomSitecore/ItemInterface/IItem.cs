using System.Collections.Generic;
using System.Linq;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Masters;
using Sitecore.Diagnostics;

namespace Sitecore.SharedSource.MoveValidator.CustomSitecore.ItemInterface
{
	public interface IItem
	{
		string Guid { get; set; }
		ID Id { get; set; }
		string Name { get; set; }
		ID BranchId { get; set; }
		ID TemplateId { get; set; }
		List<string> BranchTemplateIds { get; set; }
		bool IsDescendantOf(IItem ancestorItem);
		bool IsAncestorOf(IItem descendantItem);
		List<string> InsertOptions { get; set; }
		string DatabaseName { get; set; }

		void CopyTo(IItem iTargetItem);
		void MoveTo(IItem iTargetItem);
	}


	public class SitecoreItem : IItem
	{
		public string Guid { get; set; }
		public ID Id { get; set; }
		public string Name { get; set; }
		public ID TemplateId { get; set; }
		public ID BranchId { get; set; }


		private Item _item;
		private Item _targetItem;
		private Item GetTargetItem(IItem iTargetItem)
		{
		if (_targetItem != null) return _targetItem;
				
			Database database = Factory.GetDatabase("master");
			_targetItem = database.GetItem(iTargetItem.Id);
			return _targetItem;
		}



		public SitecoreItem(Item item)
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

		public bool IsDescendantOf(IItem iItem)
		{
			Database database = Factory.GetDatabase("master");
			Item ancestorItem = database.GetItem(iItem.Id);

			return _item.Axes.IsDescendantOf(ancestorItem);
		}

		public bool IsAncestorOf(IItem iItem)
		{
			Database database = Factory.GetDatabase("master");
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

		public void CopyTo(IItem iTargetItem)
		{
			Item targetItem = GetTargetItem(iTargetItem);
			Log.Audit(this, "Paste from: {0} to {1}", new[] { AuditFormatter.FormatItem(targetItem), AuditFormatter.FormatItem(_item) });
			_item.CopyTo(targetItem, ItemUtil.GetCopyOfName(targetItem, Name));
		}

		public void MoveTo(IItem iTargetItem)
		{
			Item targetItem = GetTargetItem(iTargetItem);
			Log.Audit(this, "Cut from: {0} to {1}", new[] { AuditFormatter.FormatItem(targetItem), AuditFormatter.FormatItem(_item) });
			_item.MoveTo(targetItem);
		}

		

	}


	public class TestItem : IItem
	{
		public string Guid { get; set; }
		public ID Id { get; set; }
		public string Name { get; set; }
		public ID TemplateId { get; set; }
		public ID BranchId { get; set; }

		public List<string> BranchTemplateIds { get; set; } 

		// todo configure this
		public bool IsDescendantOf(IItem ancestorItem)
		{
			return false;
		}

		public bool IsAncestorOf(IItem iItem)
		{
			return false;
		}


		public List<string> InsertOptions { get; set; }
		public string DatabaseName { get; set; }


		public void CopyTo(IItem iTargetItem)
		{
			
		}
		public void MoveTo(IItem iTargetItem)
		{
			
		}
	}
}
