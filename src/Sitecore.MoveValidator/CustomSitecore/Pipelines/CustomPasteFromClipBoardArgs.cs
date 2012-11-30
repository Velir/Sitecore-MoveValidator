using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.ItemInterface;
using Sitecore.Web.UI.Sheer;

namespace Sitecore.SharedSource.MoveValidator.CustomSitecore.Pipelines
{
	public class CustomPasteFromClipBoardArgs : AClientPipelineItemArgs
	{
		public CustomPasteFromClipBoardArgs(ClientPipelineArgs args)
			: base(args)
		{
		}

		public override IItem GetSource()
		{
			Item item = null;
			string str2 = StringUtil.GetString(new[] {_args.Result});
			if (!string.IsNullOrEmpty(str2))
			{
				if (!str2.StartsWith("sitecore:copy:") && !str2.StartsWith("sitecore:cut:"))
				{
					SheerResponse.Alert("The data on the clipboard is not valid.\n\nTry copying the data again.",
					                    new string[0]);
				}
				else
				{
					string id = StringUtil.Right(str2, 0x26);
					if (!ID.IsID(id))
					{
						SheerResponse.Alert(
							"The data on the clipboard is not valid.\n\nTry copying the data again.", new string[0]);
					}
					else
					{
						item = GetDatabase().GetItem(id);
						if (item == null)
						{
							SheerResponse.Alert(
								"The item that you want to paste could not be found.\n\nIt may have been deleted by another user.",
								new string[0]);
							_args.AbortPipeline();
						}
					}
				}
			}
			return new SitecoreItem(item);
		}

		public override IItem GetTarget()
		{
			Item item2 = GetDatabase().GetItem(_args.Parameters["id"]);
			if (item2 == null)
			{
				SheerResponse.Alert(
					"The destination item could not be found.\n\nIt may have been deleted by another user.",
					new string[0]);
				_args.AbortPipeline();
			}
			return new SitecoreItem(item2);
		}
	}
}