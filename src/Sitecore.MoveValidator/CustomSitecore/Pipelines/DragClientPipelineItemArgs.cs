using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.ItemInterface;
using Sitecore.Web.UI.Sheer;

namespace Sitecore.SharedSource.MoveValidator.CustomSitecore.Pipelines
{
	public class DragClientPipelineItemArgs : AClientPipelineItemArgs
	{
		public DragClientPipelineItemArgs(ClientPipelineArgs args)
			: base(args)
		{
		}

		public override IItem GetSource()
		{
			Assert.ArgumentNotNull(_args, "args");
			Assert.ArgumentNotNull(GetDatabase(), "database");
			Item item = GetDatabase().Items[_args.Parameters["id"]];
			Assert.IsNotNull(item, typeof(Item), "ID:{0}", new object[] { _args.Parameters["id"] });
			return new SitecoreItem(item);
		}

		public override IItem GetTarget()
		{
			Assert.ArgumentNotNull(_args, "args");
			Item parent = GetDatabase().Items[_args.Parameters["target"]];
			Assert.IsNotNull(parent, typeof(Item), "ID:{0}", new object[] { _args.Parameters["target"] });
			if (_args.Parameters["appendAsChild"] != "1")
			{
				parent = parent.Parent;
				Assert.IsNotNull(parent, typeof(Item), "ID:{0}.Parent", new object[] { _args.Parameters["target"] });
			}
			return new SitecoreItem(parent);
		}
	}
}