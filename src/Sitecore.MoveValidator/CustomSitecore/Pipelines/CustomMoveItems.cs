using System.Collections.Generic;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.SharedSource.Commons.Extensions;
using Sitecore.SharedSource.MoveValidator.Utils;
using Sitecore.Shell.Framework.Pipelines;
using Sitecore.Text;
using Sitecore.Web.UI.Sheer;

namespace Sitecore.SharedSource.MoveValidator.CustomSitecore.Pipelines
{
	public class CustomMoveItems : MoveItems
	{
		/// <summary>
		/// 	This method is fired in the pipeline as a validation check before the process executes.
		/// </summary>
		/// <param name = "args"></param>
		/// <returns></returns>
		public void ConstrainMove(ClientPipelineArgs args)
		{
			//check for postback
			//this occurs when an Administrator fires the event and is prompted with a confirmation message
			if (args.IsPostBack)
			{
				if (args.Result != "yes")
					args.AbortPipeline();

				return;
			}

			//get selected item and target item
			Assert.ArgumentNotNull(args, "args");
			List<Item> items = GetItems(args);
			if (items.Count == 0) return;

			Assert.IsNotNull(items, typeof (List<Item>));
			Item targetItem = GetTarget(args);
			Assert.IsNotNull(targetItem, typeof (Item));

			//validate the event
			if (!MoveUtil.IsValidCopy(items[0], targetItem))
			{
				//validation failed, prompt user
				MoveUtil.PromptUser(args);
			}
		}

		/// <summary>
		/// 	Copied Sitecore.Shell.Framework.Pipelines.MoveItems
		/// 	Method is listed as private in Sitecore
		/// </summary>
		/// <param name = "args"></param>
		/// <returns>List of Items</returns>
		private static List<Item> GetItems(ClientPipelineArgs args)
		{
			Assert.ArgumentNotNull(args, "args");
			var list = new List<Item>();
			Database database = GetDatabase(args);
			var str = new ListString(args.Parameters["items"], '|');
			foreach (string str2 in str)
			{
				Item item = database.Items[str2];
				if (item.IsNotNull())
				{
					list.Add(item);
				}
			}
			return list;
		}

		/// <summary>
		/// 	Copied Sitecore.Shell.Framework.Pipelines.MoveItems
		/// 	Method is listed as private in Sitecore
		/// </summary>
		/// <param name = "args"></param>
		/// <returns>Item</returns>
		private static Database GetDatabase(ClientPipelineArgs args)
		{
			Database database = Factory.GetDatabase(args.Parameters["database"]);
			Assert.IsNotNull(database, typeof (Database), "Database: {0}", new object[] {args.Parameters["database"]});
			return database;
		}

		/// <summary>
		/// 	Copied Sitecore.Shell.Framework.Pipelines.MoveItems
		/// 	Method is listed as private in Sitecore
		/// </summary>
		/// <param name = "args"></param>
		/// <returns>Item</returns>
		private static Item GetTarget(ClientPipelineArgs args)
		{
			Assert.ArgumentNotNull(args, "args");
			Item item = GetDatabase(args).Items[args.Parameters["target"]];
			Assert.IsNotNull(item, typeof (Item), "ID: {0}", new object[] {args.Parameters["target"]});
			return item;
		}
	}
}