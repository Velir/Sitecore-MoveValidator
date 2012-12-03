using System.Collections.Generic;
using Sitecore.Data.Items;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.Domain;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.MoveableItems;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.Pipelines.CustomClientPipelineArgs;

namespace Sitecore.SharedSource.MoveValidator.Utils
{
	public interface IMoveUtils
	{
		/// <summary>
		/// Returns the Insert Options of the passed Item.
		/// </summary>
		/// <param name = "item"></param>
		/// <returns>List of strings</returns>
		List<string> GetInsertOptions(Item item);

		/// <summary>
		/// Validates the selected item against the destination items insert options and 
		/// location (as it related to the setting item).
		/// </summary>
		/// <param name="iClientPipelineArgs"></param>
		/// <param name="iMoveValidatorSettings"></param>
		/// <returns>List of strings</returns>
		bool IsValidCopy(IClientPipelineArgs iClientPipelineArgs, IMoveValidatorSettings iMoveValidatorSettings);

		bool IsDestinationMonitored(IEnumerable<IMoveableItem> selectedLocations, IMoveableItem targetItem);

		/// <summary>
		/// Prompts the user after the validation has failed
		/// </summary>
		/// <param name = "args">Client Pipeline Arguments</param>
		/// <param name="iMoveValidatorSettings"></param>
		/// <returns></returns>
		void PromptUser(IClientPipelineArgs args, IMoveValidatorSettings iMoveValidatorSettings);

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
		void PromptUser(IClientPipelineArgs args, IMoveValidatorSettings iMoveValidatorSettings, IMoveableItem selectedItem, IMoveableItem destinationItem);
	}
}