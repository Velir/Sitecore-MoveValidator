using System.Collections.Generic;
using System.Linq;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Masters;
using Sitecore.SharedSource.Commons.Abstractions.Items;
using Sitecore.SharedSource.Commons.Extensions;
using Sitecore.SharedSource.MoveValidator.CustomItems.Common.MoveValidator;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.Domain;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.MoveableItems;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.Pipelines.CustomClientPipelineArgs;
using Sitecore.Web.UI.Sheer;

namespace Sitecore.SharedSource.MoveValidator.Utils
{
	public class MoveUtil// : IMoveUtils
	{
		/// <summary>
		/// Validates the selected item against the destination items insert options and 
		/// location (as it related to the setting item).
		/// </summary>
		/// <param name="iClientPipelineArgs"></param>
		/// <param name="iMoveValidatorSettings"></param>
		/// <returns>List of strings</returns>
		public static bool IsValidCopy(IClientPipelineArgs iClientPipelineArgs, IMoveValidatorSettings iMoveValidatorSettings)
		{
			if (iClientPipelineArgs == null) return false;
			IMoveableItem sourceItem = iClientPipelineArgs.GetSource();
			IMoveableItem targetItem = iClientPipelineArgs.GetTarget();

			//if no locations in the settings item have been configured to monitor copying, allow the item to pass validation
			List<IMoveableItem> monitoredLocations = iMoveValidatorSettings.MonitoredLocations;
			if (monitoredLocations.Count == 0) return true;

			//if it does not meet any of the selected locations, the validator does not apply to this destination folder
			bool destinationIsMonitored = IsDestinationMonitored(monitoredLocations, targetItem);
			if (!destinationIsMonitored) return true;

			//check whether the insert options are valid and return answer, final check
			bool isAllowedInInsertOptions = IsItemAllowedInInsertOptions(sourceItem, targetItem);

			// If the item comes from a branch, and the item being moved is at the top of the branch, and the branch is allowed by the insert options, allow the move.
			if (!isAllowedInInsertOptions)
			{
				isAllowedInInsertOptions = IsBranchAllowedInInsertOptions(sourceItem, targetItem);
			}

			return isAllowedInInsertOptions;
		}

		/// <summary>
		/// Determines whether [is allowed in insert options] [the specified source item].
		/// </summary>
		/// <param name="sourceItem">The source item.</param>
		/// <param name="targetItem">The target item.</param>
		/// <returns>
		///   <c>true</c> if [is allowed in insert options] [the specified source item]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsItemAllowedInInsertOptions(IMoveableItem sourceItem, IMoveableItem targetItem)
		{
			List<string> insertOptions = targetItem.InsertOptions;
			bool isAllowedInInsertOptions = insertOptions.Contains(sourceItem.TemplateId.ToString());
			return isAllowedInInsertOptions;
		}


		/// <summary>
		/// Determines whether [is branch allowed in insert options] [the specified source item].
		/// </summary>
		/// <param name="sourceItem">The source item.</param>
		/// <param name="targetItem">The target item.</param>
		/// <returns>
		///   <c>true</c> if [is branch allowed in insert options] [the specified source item]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsBranchAllowedInInsertOptions(IMoveableItem sourceItem, IMoveableItem targetItem)
		{
			// only run if the source item has a branch id
			if (sourceItem.BranchId == (ID) null) return false;

			List<string> insertOptions = targetItem.InsertOptions;
			List<string> branchChildrenTemplateIds = sourceItem.BranchTemplateIds;
			bool isBranchInInsertOptions = (insertOptions.Contains(sourceItem.BranchId.ToString()) && branchChildrenTemplateIds.Contains(sourceItem.TemplateId.ToString()));
			return isBranchInInsertOptions;
		}


		/// <summary>
		/// 	check against the settings item selected locations to see
		//		if the destination item matches or is a child of that selected item
		/// </summary>
		/// <param name="selectedLocations">The selected locations.</param>
		/// <param name="targetItem">The target item.</param>
		/// <returns>
		///   <c>true</c> if [is destination monitored] [the specified selected locations]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsDestinationMonitored(IEnumerable<IMoveableItem> selectedLocations, IMoveableItem targetItem)
		{
			bool destinationIsMonitored = false;
			foreach (IMoveableItem iLocationItem in selectedLocations)
			{
				if (targetItem.IsDescendantOf(iLocationItem))
				{
					destinationIsMonitored = true;
					break;
				}
			}
			return destinationIsMonitored;
		}


		/// <summary>
		/// Prompts the user after the validation has failed
		/// </summary>
		/// <param name = "args">Client Pipeline Arguments</param>
		/// <param name="iMoveValidatorSettings"></param>
		/// <returns></returns>
		public static void PromptUser(IClientPipelineArgs args, IMoveValidatorSettings iMoveValidatorSettings)
		{
			//if administrator allow for an override
			if (Context.User.IsAdministrator)
			{
				//validation failed, if administrator prompt with confirmation message for override
				if (string.IsNullOrEmpty(iMoveValidatorSettings.AdminAlertMessage))
					SheerResponse.Confirm(MoveValidatorSettingsItem.DEFAULT_ADMIN_ALERT);
				else
					SheerResponse.Confirm(iMoveValidatorSettings.AdminAlertMessage);
				args.WaitForPostBack();
			}
			else
			{
				//notify user this action is not allowed
				if (string.IsNullOrEmpty(iMoveValidatorSettings.UserAlertLongMessage) || string.IsNullOrEmpty(iMoveValidatorSettings.UserAlertShortMessage))
					SheerResponse.ShowError(MoveValidatorSettingsItem.DEFAULT_USER_SHORT_ALERT,
					                        MoveValidatorSettingsItem.DEFAULT_USER_LONG_ALERT);
				else
					SheerResponse.ShowError(iMoveValidatorSettings.UserAlertShortMessage, iMoveValidatorSettings.UserAlertLongMessage);
				args.AbortPipeline();
			}
		}

		/// <summary>
		/// Prompts the user after the validation has failed.
		/// Additional functionality to pass parameters into the args so 
		/// they can be picked up on the postback.
		/// </summary>
		/// <param name = "args">Client Pipeline Arguments</param>
		/// <param name="iMoveValidatorSettings"></param>
		/// <param name = "selectedItem">Selected Item that is being copied or moved</param>
		/// <param name = "destinationItem">Destination Item</param>
		/// <returns></returns>
		public static void PromptUser(IClientPipelineArgs args, IMoveValidatorSettings iMoveValidatorSettings, IMoveableItem selectedItem, IMoveableItem destinationItem)
		{
			//if administrator allow for an override
			if (Context.User.IsAdministrator)
			{
				//validation failed, if administrator prompt with confirmation message for override
				if (string.IsNullOrEmpty(iMoveValidatorSettings.AdminAlertMessage))
					SheerResponse.Confirm(MoveValidatorSettingsItem.DEFAULT_ADMIN_ALERT);
				else
					SheerResponse.Confirm(iMoveValidatorSettings.AdminAlertMessage);

				//add parameters for command style approach, we loose our parameters on the postback otherwise
				//and therefore cannot pickup the item and destination item to perform the event.
				args.SetParameter("selectedItem", selectedItem.Id.ToString());
				args.SetParameter("destinationItem",  destinationItem.Id.ToString());
				args.SetParameter("database", selectedItem.DatabaseName);
				args.SetParameter("copyOrMove", "move");
				if (args.IsCopy())
					args.SetParameter("copyOrMove", "copy");

				args.WaitForPostBack();
			}
			else
			{
				//notify user this action is not allowed
				if (string.IsNullOrEmpty(iMoveValidatorSettings.UserAlertLongMessage) || string.IsNullOrEmpty(iMoveValidatorSettings.UserAlertShortMessage))
					SheerResponse.ShowError(MoveValidatorSettingsItem.DEFAULT_USER_SHORT_ALERT,  MoveValidatorSettingsItem.DEFAULT_USER_LONG_ALERT);
				else
					SheerResponse.ShowError(iMoveValidatorSettings.UserAlertShortMessage, iMoveValidatorSettings.UserAlertLongMessage);
				args.AbortPipeline();
			}
		}
	}

}