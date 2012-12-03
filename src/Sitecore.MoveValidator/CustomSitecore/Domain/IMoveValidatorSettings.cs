using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.MoveableItems;

namespace Sitecore.SharedSource.MoveValidator.CustomSitecore.Domain
{
	public interface IMoveValidatorSettings
	{
		//	List<Item> selectedLocations = settingsItem.AppliedLocations.GetItems().ToList();

		List<IMoveableItem> MonitoredLocations { get; set; }

		string AdminAlertMessage { get; }
		string UserAlertLongMessage { get; }
		string UserAlertShortMessage { get; }
	}
}
