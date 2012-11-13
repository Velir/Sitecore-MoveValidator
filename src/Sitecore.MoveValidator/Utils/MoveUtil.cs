using System.Collections.Generic;
using System.Linq;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Masters;
using Sitecore.SharedSource.Commons.Extensions;
using Sitecore.SharedSource.MoveValidator.CustomItems.Common.MoveValidator;
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
		/// <param name = "item"></param>
		/// <param name = "destinationItem"></param>
		/// <returns>List of strings</returns>
		public static bool IsValidCopy(Item item, Item destinationItem)
		{
			if (item == null || destinationItem == null) return false;

			//is valid location that we are copying to?
			MoveValidatorSettingsItem settingsItem = GetSettingsItem();
			if (settingsItem.IsNull()) return false;

			//if no locations in the settings item have been configured to monitor 
			//copying, allow the item to pass validation
			List<Item> selectedLocations = settingsItem.AppliedLocations.GetItems().ToList();
			if (selectedLocations.Count == 0) return true;

			//check against the settings item selected locations to see
			//if the destination item matches or is a child of that selected item
			bool matchedLocation = false;
			foreach (Item locationItem in selectedLocations)
			{
				if (destinationItem.Axes.IsDescendantOf(locationItem))
				{
					matchedLocation = true;
					break;
				}
			}

			//if it does not meet any of the selected locations, the validator
			//does not apply to this destination folder
			if (!matchedLocation) return true;

			//check whether the insert options are valid and return answer, final check
			List<string> insertOptions = GetInsertOptions(destinationItem);
			bool returnVal = insertOptions.Contains(item.TemplateID.ToString());
			// If the item comes from a branch, and the item being moved is at the top of the branch, and the branch is allowed by the insert options, allow the move.
			if (!returnVal && item.Branch != null)
			{
				List<string> branchChildrenTemplateIds = item.Branch.InnerItem.GetChildren().InnerChildren.ToList().Select(x => x.TemplateID.ToString()).ToList();
				returnVal = (insertOptions.Contains(item.BranchId.ToString()) && branchChildrenTemplateIds.Contains(item.TemplateID.ToString()));
			}
			return returnVal;
		}

		/// <summary>
		/// 	Returns the Custom Item for the Move Validator Settings Item
		/// </summary>
		/// <returns>MoveValidatorSettingsItem</returns>
		public static MoveValidatorSettingsItem GetSettingsItem()
		{
			Database database = Client.ContentDatabase;
			if (database == null)
			{
				return null;
			}

			Item item = database.GetItem("/sitecore/system/Modules/Move Validator Settings");
			if (item.IsNull())
			{
				return null;
			}

			return item;
		}

		/// <summary>
		/// Prompts the user after the validation has failed
		/// </summary>
		/// <param name = "args">Client Pipeline Arguments</param>
		/// <returns></returns>
		public static void PromptUser(ClientPipelineArgs args)
		{
			//validation failed, if administrator prompt with confirmation message for override
			MoveValidatorSettingsItem settingsItem = GetSettingsItem();

			//if administrator allow for an override
			if (Context.User.IsAdministrator)
			{
				//validation failed, if administrator prompt with confirmation message for override
				if (settingsItem.IsNull() || string.IsNullOrEmpty(settingsItem.AdminAlertMessage))
					SheerResponse.Confirm(MoveValidatorSettingsItem.DEFAULT_ADMIN_ALERT);
				else
					SheerResponse.Confirm(settingsItem.AdminAlertMessage);
				args.WaitForPostBack();
			}
			else
			{
				//notify user this action is not allowed
				if (settingsItem.IsNull() || string.IsNullOrEmpty(settingsItem.UserAlertLongMessage) ||
				    string.IsNullOrEmpty(settingsItem.UserAlertShortMessage))
					SheerResponse.ShowError(MoveValidatorSettingsItem.DEFAULT_USER_SHORT_ALERT,
					                        MoveValidatorSettingsItem.DEFAULT_USER_LONG_ALERT);
				else
					SheerResponse.ShowError(settingsItem.UserAlertShortMessage, settingsItem.UserAlertLongMessage);
				args.AbortPipeline();
			}
		}

		/// <summary>
		/// Prompts the user after the validation has failed.
		/// Additional functionality to pass parameters into the args so 
		/// they can be picked up on the postback.
		/// </summary>
		/// <param name = "args">Client Pipeline Arguments</param>
		/// <param name = "selectedItem">Selected Item that is being copied or moved</param>
		/// <param name = "destinationItem">Destination Item</param>
		/// <returns></returns>
		public static void PromptUser(ClientPipelineArgs args, Item selectedItem, Item destinationItem)
		{
			//validation failed, if administrator prompt with confirmation message for override
			MoveValidatorSettingsItem settingsItem = GetSettingsItem();

			//if administrator allow for an override
			if (Context.User.IsAdministrator)
			{
				//validation failed, if administrator prompt with confirmation message for override
				if (settingsItem.IsNull() || string.IsNullOrEmpty(settingsItem.AdminAlertMessage))
					SheerResponse.Confirm(MoveValidatorSettingsItem.DEFAULT_ADMIN_ALERT);
				else
					SheerResponse.Confirm(settingsItem.AdminAlertMessage);

				//add parameters for command style approach, we loose our parameters on the postback otherwise
				//and therefore cannot pickup the item and destination item to perform the event.
				args.Parameters["selectedItem"] = selectedItem.ID.ToString();
				args.Parameters["destinationItem"] = destinationItem.ID.ToString();
				args.Parameters["database"] = selectedItem.Database.Name;
				args.Parameters["copyOrMove"] = "move";
				if (args.Result.ToLower().StartsWith("sitecore:copy:"))
					args.Parameters["copyOrMove"] = "copy";

				args.WaitForPostBack();
			}
			else
			{
				//notify user this action is not allowed
				if (settingsItem.IsNull() || string.IsNullOrEmpty(settingsItem.UserAlertLongMessage) ||
				    string.IsNullOrEmpty(settingsItem.UserAlertShortMessage))
					SheerResponse.ShowError(MoveValidatorSettingsItem.DEFAULT_USER_SHORT_ALERT,
					                        MoveValidatorSettingsItem.DEFAULT_USER_LONG_ALERT);
				else
					SheerResponse.ShowError(settingsItem.UserAlertShortMessage, settingsItem.UserAlertLongMessage);
				args.AbortPipeline();
			}
		}
	}
}