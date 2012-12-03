using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.MoveableItems;

namespace Sitecore.MoveValidator.Tests.CustomSitecore.MoveableItems
{
	public class MoveableTestItem : IMoveableItem
	{
		public string Guid { get; set; }
		public ID Id { get; set; }
		public string Name { get; set; }
		public ID TemplateId { get; set; }
		public ID BranchId { get; set; }

		public List<string> BranchTemplateIds { get; set; } 

		/// <summary>
		/// this will usually be Stubbed
		/// </summary>
		/// <param name="ancestorItem"></param>
		/// <returns></returns>
		public bool IsDescendantOf(IMoveableItem ancestorItem)
		{
			return false;
		}

		/// <summary>
		/// this will usually be stubbed
		/// </summary>
		/// <param name="iItem"></param>
		/// <returns></returns>
		public bool IsAncestorOf(IMoveableItem iItem)
		{
			return false;
		}


		public List<string> InsertOptions { get; set; }
		public string DatabaseName { get; set; }


		public void CopyTo(IMoveableItem iTargetItem)
		{
			
		}
		public void MoveTo(IMoveableItem iTargetItem)
		{
			
		}
	}
}