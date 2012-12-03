using Sitecore.Data;
using Sitecore.MoveValidator.Tests.CustomSitecore.MoveableItems;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.CustomClientPipelineArgs;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.MoveableItems;

namespace Sitecore.MoveValidator.Tests.CustomSitecore.CustomClientPipelineArgs
{
	public class ClientPipelineTestArgs : AClientPipelineItemArgs
	{
		public ClientPipelineTestArgs(Web.UI.Sheer.ClientPipelineArgs args)
			: base(args)
		{
		}

		public override IMoveableItem GetSource()
		{
			IMoveableItem sourceItem = new MoveableTestItem();
			sourceItem.Id = ID.NewID;
			sourceItem.Name = "source";
			return sourceItem;
		}

		public override IMoveableItem GetTarget()
		{
			IMoveableItem targetItem = new MoveableTestItem();
			targetItem.Id = ID.NewID;
			targetItem.Name = "source";
			return targetItem;
		}
	}
}