using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.MoveableItems;

namespace Sitecore.SharedSource.MoveValidator.CustomSitecore.Pipelines.CustomClientPipelineArgs
{
	public class DragClientPipelineItemArgs : AClientPipelineItemArgs
	{
		public DragClientPipelineItemArgs(Web.UI.Sheer.ClientPipelineArgs args)
			: base(args)
		{
		}

		public override IMoveableItem GetSource()
		{
			Assert.ArgumentNotNull(_args, "args");
			Assert.ArgumentNotNull(GetDatabase(), "database");
			Item item = GetDatabase().Items[_args.Parameters["id"]];
			Assert.IsNotNull(item, typeof(Item), "ID:{0}", new object[] { _args.Parameters["id"] });
			return new MoveableSitecoreItem(item);
		}

		public override IMoveableItem GetTarget()
		{
			Assert.ArgumentNotNull(_args, "args");
			Item parent = GetDatabase().Items[_args.Parameters["target"]];
			Assert.IsNotNull(parent, typeof(Item), "ID:{0}", new object[] { _args.Parameters["target"] });
			if (_args.Parameters["appendAsChild"] != "1")
			{
				parent = parent.Parent;
				Assert.IsNotNull(parent, typeof(Item), "ID:{0}.Parent", new object[] { _args.Parameters["target"] });
			}
			return new MoveableSitecoreItem(parent);
		}
	}
}