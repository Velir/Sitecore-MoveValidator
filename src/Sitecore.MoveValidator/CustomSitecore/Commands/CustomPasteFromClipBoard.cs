using System;
using System.Collections.Specialized;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.SharedSource.MoveValidator.Utils;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;

namespace Sitecore.SharedSource.MoveValidator.CustomSitecore.Commands
{
	[Serializable]
	public class CustomPasteFromClipBoard : PasteFromClipboard
	{
		/// <summary>
		/// 	Copied Sitecore.Shell.Framework.Commands.PasteFromClipboard
		/// 	This method is fired in the pipeline as a validation check before the process executes.
		/// </summary>
		/// <param name = "context"></param>
		/// <returns></returns>
		public override void Execute(CommandContext context)
		{
			Assert.ArgumentNotNull(context, "context");
			bool alert = true;
			if (IsSupported(alert) && (context.Items.Length == 1))
			{
				Item item = context.Items[0];
				var parameters = new NameValueCollection();
				parameters["id"] = item.ID.ToString();
				parameters["database"] = item.Database.Name;
				parameters["fetched"] = "0";
				Context.ClientPage.Start(this, "Run", parameters);
			}
		}

		/// <summary>
		/// 	Copied Sitecore.Shell.Framework.Commands.PasteFromClipboard
		/// </summary>
		/// <param name = "context"></param>
		/// <returns>CommandState</returns>
		public override CommandState QueryState(CommandContext context)
		{
			Assert.ArgumentNotNull(context, "context");
			bool alert = false;
			if (!IsSupported(alert))
			{
				return CommandState.Hidden;
			}
			if (context.Items.Length != 1)
			{
				return CommandState.Disabled;
			}
			if (!context.Items[0].Access.CanCreate())
			{
				return CommandState.Disabled;
			}
			return base.QueryState(context);
		}

		/// <summary>
		/// 	This method is fired by Execute.
		/// 	Runs through a range of checks to validate the event.
		/// </summary>
		/// <param name = "args"></param>
		/// <returns></returns>
		protected void Run(ClientPipelineArgs args)
		{
			Assert.ArgumentNotNull(args, "args");

			//check for postback
			//this occurs when an Administrator fires the event and is prompted with a confirmation message
			if (args.IsPostBack && args.Result == "yes")
			{
				PerformEvent(args);
				args.AbortPipeline();
				return;
			}

			//this occurs when an Administrator clicks the cancel button on the override prompt
			if (args.IsPostBack && args.Result == "no")
				return;

			if (args.Parameters["fetched"] == "0")
			{
				args.Parameters["fetched"] = "1";
				Context.ClientPage.ClientResponse.Eval("window.clipboardData.getData(\"Text\")").Attributes["response"]
					= "1";
				args.WaitForPostBack();
			}
			else
			{
				string name = args.Parameters["database"];
				Database database = Factory.GetDatabase(name);
				Assert.IsNotNull(database, name);
				string str2 = StringUtil.GetString(new[] {args.Result});
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
							Item item = database.GetItem(id);
							if (item == null)
							{
								SheerResponse.Alert(
									"The item that you want to paste could not be found.\n\nIt may have been deleted by another user.",
									new string[0]);
								args.AbortPipeline();
							}
							else
							{
								Item item2 = database.GetItem(args.Parameters["id"]);
								if (item2 == null)
								{
									SheerResponse.Alert(
										"The destination item could not be found.\n\nIt may have been deleted by another user.",
										new string[0]);
									args.AbortPipeline();
								}
								else if ((item.ID != item2.ID) && item.Axes.IsAncestorOf(item2))
								{
									SheerResponse.Alert("You cannot paste an item to a subitem of itself.",
									                    new string[0]);
									args.AbortPipeline();
								}
									//check to see if it meets custom validation
								else if (!MoveUtil.IsValidCopy(item, item2))
								{
									//validation failed, prompt user
									MoveUtil.PromptUser(args, item, item2);
								}
								else if (str2.StartsWith("sitecore:copy:"))
								{
									Log.Audit(this, "Paste from: {0} to {1}",
									          new[] {AuditFormatter.FormatItem(item2), AuditFormatter.FormatItem(item)});
									item.CopyTo(item2, ItemUtil.GetCopyOfName(item2, item.Name));
								}
								else if (item.ID != item2.ID)
								{
									Log.Audit(this, "Cut from: {0} to {1}",
									          new[] {AuditFormatter.FormatItem(item2), AuditFormatter.FormatItem(item)});
									item.MoveTo(item2);
								}
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// 	Performs the move/copy event after the validation is complete.
		/// </summary>
		/// <param name = "args">Client Pipeline Arguments</param>
		/// <returns></returns>
		private void PerformEvent(ClientPipelineArgs args)
		{
			//get parameters
			string selectedItemParameter = args.Parameters["selectedItem"];
			string destinationItemParameter = args.Parameters["destinationItem"];
			string copyOrMoveParameter = args.Parameters["copyOrMove"];
			string databaseParameter = args.Parameters["database"];

			//verify the parameters are valid
			if (string.IsNullOrEmpty(databaseParameter) || string.IsNullOrEmpty(selectedItemParameter) ||
			    string.IsNullOrEmpty(destinationItemParameter) || string.IsNullOrEmpty(copyOrMoveParameter))
				return;

			//get database
			Database database = Factory.GetDatabase(databaseParameter);
			if (database == null) return;

			//get items
			Item selectedItem = database.GetItem(selectedItemParameter);
			Item destinationItem = database.GetItem(destinationItemParameter);

			//verify we have valid items
			if (selectedItem == null || destinationItem == null) return;

			if (copyOrMoveParameter.Equals("copy"))
			{
				//copy
				Log.Audit(this, "Paste from: {0} to {1}",
				          new[] {AuditFormatter.FormatItem(destinationItem), AuditFormatter.FormatItem(selectedItem)});
				selectedItem.CopyTo(destinationItem, ItemUtil.GetCopyOfName(destinationItem, selectedItem.Name));
			}
			else if (selectedItem.ID != destinationItem.ID)
			{
				//move
				Log.Audit(this, "Cut from: {0} to {1}",
				          new[] {AuditFormatter.FormatItem(destinationItem), AuditFormatter.FormatItem(selectedItem)});
				selectedItem.MoveTo(destinationItem);
			}
		}
	}
}