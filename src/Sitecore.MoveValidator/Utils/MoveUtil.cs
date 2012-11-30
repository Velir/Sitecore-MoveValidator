using System.Collections.Generic;
using System.Linq;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Masters;
using Sitecore.SharedSource.Commons.Extensions;
using Sitecore.SharedSource.MoveValidator.CustomItems.Common.MoveValidator;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.Domain;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.ItemInterface;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.Pipelines;
using Sitecore.Web.UI.Sheer;

namespace Sitecore.SharedSource.MoveValidator.Utils
{
	public class MoveUtil
	{
		/// <summary>
		/// Returns the Insert Options of the passed Item.
		/// </summary>
		/// <param name = "item"></param>
		/// <returns>List of strings</returns>
		public static List<string> GetInsertOptions(Item item)
		{
			List<string> retVal = Masters.GetMasters(item).Select(x => x.ID.ToString()).ToList();
			return retVal;
		}

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
			IItem sourceItem = iClientPipelineArgs.GetSource();
			IItem targetItem = iClientPipelineArgs.GetTarget();

			//if no locations in the settings item have been configured to monitor 
			//copying, allow the item to pass validation
			List<IItem> selectedLocations = iMoveValidatorSettings.AppliedLocations;
			if (selectedLocations.Count == 0) return true;

			//check against the settings item selected locations to see
			//if the destination item matches or is a child of that selected item
			bool matchedLocation = false;
			foreach (Item locationItem in selectedLocations)
			{
				IItem iLocationItem = new SitecoreItem(locationItem);
				if (targetItem.IsDescendantOf(iLocationItem))
				{
					matchedLocation = true;
					break;
				}
			}

			//if it does not meet any of the selected locations, the validator
			//does not apply to this destination folder
			if (!matchedLocation) return true;

			//check whether the insert options are valid and return answer, final check
			List<string> insertOptions = targetItem.InsertOptions;
			bool returnVal = insertOptions.Contains(sourceItem.TemplateId.ToString());
			// If the item comes from a branch, and the item being moved is at the top of the branch, and the branch is allowed by the insert options, allow the move.
			if (!returnVal && sourceItem.BranchId != (ID)null)
			{
				List<string> branchChildrenTemplateIds = sourceItem.BranchTemplateIds;// sourceItem.Branch.InnerItem.GetChildren().InnerChildren.ToList().Select(x => x.TemplateID.ToString()).ToList();
				returnVal = (insertOptions.Contains(sourceItem.BranchId.ToString()) && branchChildrenTemplateIds.Contains(sourceItem.TemplateId.ToString()));
			}
			return returnVal;
		}




		///// <summary>
		///// 	Returns the Custom Item for the Move Validator Settings Item
		///// </summary>
		///// <returns>MoveValidatorSettingsItem</returns>
		//public static MoveValidatorSettingsItem GetSettingsItem()
		//{
		//  Database database = Client.ContentDatabase;
		//  if (database == null)
		//  {
		//    return null;
		//  }

		//  Item item = database.GetItem("/sitecore/system/Modules/Move Validator Settings");
		//  if (item.IsNull())
		//  {
		//    return null;
		//  }

		//  return item;
		//}

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
		public static void PromptUser(IClientPipelineArgs args, IMoveValidatorSettings iMoveValidatorSettings, IItem selectedItem, IItem destinationItem)
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