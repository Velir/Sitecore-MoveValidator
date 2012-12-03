using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.SharedSource.Commons.Extensions;
using Sitecore.SharedSource.MoveValidator.CustomItems.Common.MoveValidator;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.MoveableItems;

namespace Sitecore.SharedSource.MoveValidator.CustomSitecore.Domain
{
	public class MoveValidatorSettings : IMoveValidatorSettings
	{
		private MoveValidatorSettingsItem _moveValidatorSettingsItem;

		public List<IMoveableItem> MonitoredLocations
		{
			get
			{
				IEnumerable<IMoveableItem> appliedLocations = GetSettingsItem().AppliedLocations.GetItems().Select(x => new MoveableSitecoreItem(x) as IMoveableItem);
				return appliedLocations.ToList();
			}
			set { }
		}
		public string AdminAlertMessage
		{
			get { return GetSettingsItem().AdminAlertMessage; }
		}

		public string UserAlertLongMessage
		{
			get { return GetSettingsItem().UserAlertLongMessage; }
		}

		public string UserAlertShortMessage
		{
			get { return GetSettingsItem().UserAlertShortMessage; }
		}


		/// <summary>
		/// 	Returns the Custom Item for the Move Validator Settings Item
		/// </summary>
		/// <returns>MoveValidatorSettingsItem</returns>
		private MoveValidatorSettingsItem GetSettingsItem()
		{
			if (_moveValidatorSettingsItem != null) return _moveValidatorSettingsItem;

			Database database = Client.ContentDatabase;
			if (database == null)
			{
				return null;
			}

			_moveValidatorSettingsItem = database.GetItem("/sitecore/system/Modules/Move Validator Settings");
			return _moveValidatorSettingsItem;
		}
	}
}
