using System;
using System.Collections.Specialized;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.CustomClientPipelineArgs;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.Domain;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.MoveableItems;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.Pipelines.MoveManagers;
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
			IClientPipelineArgs iClientPipelineArgs = new CustomPasteFromClipBoardArgs(args);
			IMoveValidatorSettings iMoveValidatorSettings = new MoveValidatorSettings();
			CommandMoveManager commandMoveManager = new CommandMoveManager(iClientPipelineArgs, iMoveValidatorSettings);
			commandMoveManager.ProcessPostBack();
			if (commandMoveManager.PostBackProcessed)
			{
				return;
			}

			if (iClientPipelineArgs.GetParameter("fetched") == "0")
			{
				iClientPipelineArgs.SetParameter("fetched", "1");
				Context.ClientPage.ClientResponse.Eval("window.clipboardData.getData(\"Text\")").Attributes["response"] = "1";
				args.WaitForPostBack();
				return;
			}

			IMoveableItem sourceItem = iClientPipelineArgs.GetSource();
			IMoveableItem targetItem = iClientPipelineArgs.GetTarget();

			if ((sourceItem.Id != targetItem.Id) && sourceItem.IsAncestorOf(targetItem))
			{
				SheerResponse.Alert("You cannot paste an item to a subitem of itself.", new string[0]);
				args.AbortPipeline();
				return;
			}

			//check to see if it meets custom validation
			commandMoveManager.PromptIfNotValid();
			if (commandMoveManager.UserWasPrompted)
			{
				return;
			}

			if (iClientPipelineArgs.IsCopy())
			{
				sourceItem.CopyTo(targetItem);
			}
			else if (sourceItem.Id != targetItem.Id)
			{
				sourceItem.MoveTo(targetItem);
			}
		}
	}
}