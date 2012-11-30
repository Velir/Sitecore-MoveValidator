using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.ItemInterface;

namespace Sitecore.SharedSource.MoveValidator.CustomSitecore.Domain
{
	public interface IMoveValidatorSettings
	{
		//	List<Item> selectedLocations = settingsItem.AppliedLocations.GetItems().ToList();

		List<IItem> AppliedLocations { get; set; }

		string AdminAlertMessage { get; }
		string UserAlertLongMessage { get; }
		string UserAlertShortMessage { get; }
	}
}
