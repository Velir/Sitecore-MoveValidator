using System.Collections.Generic;
using Sitecore.Data;

namespace Sitecore.SharedSource.MoveValidator.CustomSitecore.MoveableItems
{
	public interface IMoveableItem
	{
		string Guid { get; set; }
		ID Id { get; set; }
		string Name { get; set; }
		ID BranchId { get; set; }
		ID TemplateId { get; set; }
		List<string> BranchTemplateIds { get; set; }
		bool IsDescendantOf(IMoveableItem ancestorItem);
		bool IsAncestorOf(IMoveableItem descendantItem);
		List<string> InsertOptions { get; set; }
		string DatabaseName { get; set; }

		void CopyTo(IMoveableItem iTargetItem);
		void MoveTo(IMoveableItem iTargetItem);
	}
}
