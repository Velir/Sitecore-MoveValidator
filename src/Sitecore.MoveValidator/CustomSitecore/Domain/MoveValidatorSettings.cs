using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.SharedSource.Commons.Extensions;
using Sitecore.SharedSource.MoveValidator.CustomItems.Common.MoveValidator;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.ItemInterface;

namespace Sitecore.SharedSource.MoveValidator.CustomSitecore.Domain
{
	public class MoveValidatorSettings : IMoveValidatorSettings
	{
		private MoveValidatorSettingsItem _moveValidatorSettingsItem;

		public List<IItem> AppliedLocations
		{
			get
			{
				IEnumerable<IItem> appliedLocations = GetSettingsItem().AppliedLocations.GetItems().Select(x => new SitecoreItem(x) as IItem);
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



	public class MoveValidatorSettingsTester : IMoveValidatorSettings
	{


		public List<IItem> AppliedLocations {
			get { return new List<IItem>();}
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
