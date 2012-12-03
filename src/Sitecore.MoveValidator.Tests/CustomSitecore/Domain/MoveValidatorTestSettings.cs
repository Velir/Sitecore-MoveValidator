using System.Collections.Generic;
using Sitecore.SharedSource.MoveValidator.CustomItems.Common.MoveValidator;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.Domain;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.MoveableItems;

namespace Sitecore.MoveValidator.Tests.CustomSitecore.Domain
{
	public class MoveValidatorTestSettings : IMoveValidatorSettings
	{


		public List<IMoveableItem> MonitoredLocations {
			get { return new List<IMoveableItem>();}
			set { }
		}

		
		public string AdminAlertMessage
		{
			get { return MoveValidatorSettingsItem.DEFAULT_ADMIN_ALERT; }
		}

		public string UserAlertLongMessage
		{
			get { return MoveValidatorSettingsItem.DEFAULT_USER_LONG_ALERT; }
		}

		public string UserAlertShortMessage
		{
			get { return MoveValidatorSettingsItem.DEFAULT_USER_SHORT_ALERT; }
		}
	}
}