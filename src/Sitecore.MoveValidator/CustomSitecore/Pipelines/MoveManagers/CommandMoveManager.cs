using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.Domain;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.Pipelines.CustomClientPipelineArgs;
using Sitecore.SharedSource.MoveValidator.Utils;

namespace Sitecore.SharedSource.MoveValidator.CustomSitecore.Pipelines.MoveManagers
{
	public class CommandMoveManager : IMoveManager
	{
		private readonly IClientPipelineArgs _iClientPipelineArgs;
		private readonly IMoveValidatorSettings _iMoveValidatorSettings;
		public CommandMoveManager(IClientPipelineArgs iClientPipelineArgs, IMoveValidatorSettings iMoveValidatorSettings)
		{
			_iMoveValidatorSettings = iMoveValidatorSettings;
			_iClientPipelineArgs = iClientPipelineArgs;
		}

		public bool PostBackProcessed;
		public bool PipelineAborted;
		public void ProcessPostBack()
		{
			if (_iClientPipelineArgs.IsPostBack())
			{
				if (_iClientPipelineArgs.NeedToAbortPipeline())
				{
					PerformEvent(_iClientPipelineArgs);
					_iClientPipelineArgs.AbortPipeline();
					PipelineAborted = true;
				}
				PostBackProcessed = true;
			}
		}

		public bool UserWasPrompted;
		public void PromptIfNotValid()
		{
			bool isValid = MoveUtils.IsValidCopy(_iClientPipelineArgs, _iMoveValidatorSettings);
			if (!isValid)
			{
				MoveUtils.PromptUser(_iClientPipelineArgs, _iMoveValidatorSettings);
				UserWasPrompted = true;
			}
		}



		/// <summary>
		/// 	Performs the move/copy event after the validation is complete.
		/// </summary>
		/// <param name = "args">Client Pipeline Arguments</param>
		/// <returns></returns>
		private void PerformEvent(IClientPipelineArgs args)
		{
			//get parameters
			string selectedItemParameter = args.GetParameter("selectedItem");
			string destinationItemParameter = args.GetParameter("destinationItem");
			string copyOrMoveParameter = args.GetParameter("copyOrMove");
			string databaseParameter = args.GetParameter("database");

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
				Log.Audit(this, "Paste from: {0} to {1}", new[] { AuditFormatter.FormatItem(destinationItem), AuditFormatter.FormatItem(selectedItem) });
				selectedItem.CopyTo(destinationItem, ItemUtil.GetCopyOfName(destinationItem, selectedItem.Name));
			}
			else if (selectedItem.ID != destinationItem.ID)
			{
				//move
				Log.Audit(this, "Cut from: {0} to {1}", new[] { AuditFormatter.FormatItem(destinationItem), AuditFormatter.FormatItem(selectedItem) });
				selectedItem.MoveTo(destinationItem);
			}
		}

	}
}