using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.SharedSource.Commons.Extensions;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.ItemInterface;
using Sitecore.Text;
using Sitecore.Web.UI.Sheer;

namespace Sitecore.SharedSource.MoveValidator.CustomSitecore.Pipelines
{
	public class MoveClientPipelineItemArgs : AClientPipelineItemArgs
	{
		public MoveClientPipelineItemArgs(ClientPipelineArgs args)
			: base(args)
		{
		}

		#region IClientPipelineArgs Members

		public override IItem GetSource()
		{
			Assert.ArgumentNotNull(_args, "args");
			var list = new List<Item>();
			Database database = GetDatabase();
			var str = new ListString(_args.Parameters["items"], '|');
			foreach (string str2 in str)
			{
				Item item = database.Items[str2];
				if (item.IsNotNull())
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
			Item item = GetDatabase().Items[_args.Parameters["target"]];
			Assert.IsNotNull(item, typeof(Item), "ID: {0}", new object[] { _args.Parameters["target"] });
			return new SitecoreItem(item);
		}


		#endregion
	}
}