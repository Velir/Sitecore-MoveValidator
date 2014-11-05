using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.SharedSource.Commons.Extensions;
using Sitecore.SharedSource.MoveValidator.Utils;
using Sitecore.Shell.Framework.Pipelines;
using Sitecore.Web.UI.Sheer;

namespace Sitecore.SharedSource.MoveValidator.CustomSitecore.Pipelines
{
	public class CustomDragItemTo : DragItemTo
	{
		/// <summary>
		/// This method is fired in the pipeline as a validation check before the process executes.
		/// </summary>
		/// <param name = "args"></param>
		/// <returns></returns>
		public void ConstrainDragTo(ClientPipelineArgs args)
		{
			//check for postback
			//this occurs when an Administrator fires the event and is prompted with a confirmation message
			if (args.IsPostBack)
			{
				if (!string.IsNullOrEmpty(args.Result) && args.Result != "yes")
				{
					args.AbortPipeline();
				}

				return;
			}

			//get database as listed in args
			Database database = GetDatabase(args);
			if (database == null) return;

			//get selected item and target item
			Item targetItem = GetTarget(args);
			Item copiedItem = GetSource(args, database);
			if (targetItem.IsNull() || copiedItem.IsNull()) return;

			//validate the event
			if (!MoveUtil.IsValidCopy(copiedItem, targetItem))
			{
				//validation failed, prompt user
				MoveUtil.PromptUser(args);
			}
		}

		/// <summary>
		/// 	Copied Sitecore.Shell.Framework.Pipelines.DragItemTo
		/// 	Method is listed as private in Sitecore
		/// </summary>
		/// <param name = "args"></param>
		/// <param name = "database"></param>
		/// <returns>List of Items</returns>
		private static Item GetSource(ClientPipelineArgs args, Database database)
		{
			Assert.ArgumentNotNull(args, "args");
			Assert.ArgumentNotNull(database, "database");
			Item item = database.Items[args.Parameters["id"]];
			Assert.IsNotNull(item, typeof (Item), "ID:{0}", new object[] {args.Parameters["id"]});
			return item;
		}

		/// <summary>
		/// 	Copied Sitecore.Shell.Framework.Pipelines.DragItemTo
		/// 	Method is listed as private in Sitecore
		/// </summary>
		/// <param name = "args"></param>
		/// <returns>Item</returns>
		private static Item GetTarget(ClientPipelineArgs args)
		{
			Assert.ArgumentNotNull(args, "args");
			Item parent = GetDatabase(args).Items[args.Parameters["target"]];
			Assert.IsNotNull(parent, typeof (Item), "ID:{0}", new object[] {args.Parameters["target"]});
			if (args.Parameters["appendAsChild"] != "1")
			{
				parent = parent.Parent;
				Assert.IsNotNull(parent, typeof (Item), "ID:{0}.Parent", new object[] {args.Parameters["target"]});
			}
			return parent;
		}

		/// <summary>
		/// 	Copied Sitecore.Shell.Framework.Pipelines.DragItemTo
		/// 	Method is listed as private in Sitecore
		/// </summary>
		/// <param name = "args"></param>
		/// <returns>Item</returns>
		private static Database GetDatabase(ClientPipelineArgs args)
		{
			Assert.ArgumentNotNull(args, "args");
			Database database = Factory.GetDatabase(args.Parameters["database"]);
			Error.Assert(database != null, "Database \"" + args.Parameters["database"] + "\" not found.");
			return database;
		}
	}
}