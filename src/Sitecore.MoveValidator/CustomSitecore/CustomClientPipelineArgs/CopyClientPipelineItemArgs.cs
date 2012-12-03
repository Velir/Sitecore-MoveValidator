using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.MoveableItems;
using Sitecore.Text;

namespace Sitecore.SharedSource.MoveValidator.CustomSitecore.CustomClientPipelineArgs
{
	public class CopyClientPipelineItemArgs: AClientPipelineItemArgs
	{
		public CopyClientPipelineItemArgs(Web.UI.Sheer.ClientPipelineArgs args) : base(args)
		{
		}

		public override IMoveableItem GetSource()
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
				return new MoveableSitecoreItem(list[0]);
			}
			return null;
		}

		public override IMoveableItem GetTarget()
		{
			Assert.ArgumentNotNull(_args, "args");
			Item item = GetDatabase().Items[_args.Parameters["destination"]];
			Assert.IsNotNull(item, typeof(Item), "ID: {0}", new object[] { _args.Parameters["destination"] });
			return new MoveableSitecoreItem(item);
		}
	}
}