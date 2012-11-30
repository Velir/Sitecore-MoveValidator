using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.ItemInterface;
using Sitecore.Text;
using Sitecore.Web.UI.Sheer;

namespace Sitecore.SharedSource.MoveValidator.CustomSitecore.Pipelines
{
	public class CopyClientPipelineItemArgs: AClientPipelineItemArgs
	{
		public CopyClientPipelineItemArgs(ClientPipelineArgs args) : base(args)
		{
		}

		public override IItem GetSource()
		{
			Assert.ArgumentNotNull(_args, "args");
			List<Item> list = new List<Item>();
			Database database = GetDatabase();
			ListString str = new ListString(_args.Parameters["items"], '|');
			foreach (string str2 in str)
			{
				Item item = database.Items[str2];
				if (item != null)
				{
					list.Add(item);
				}
			}
			if (list.Count > 0)
			{
				return new SitecoreItem(list[0]);
			}
			return null;
		}

		public override IItem GetTarget()
		{
			Assert.ArgumentNotNull(_args, "args");
			Item item = GetDatabase().Items[_args.Parameters["destination"]];
			Assert.IsNotNull(item, typeof(Item), "ID: {0}", new object[] { _args.Parameters["destination"] });
			return new SitecoreItem(item);
		}
	}
}