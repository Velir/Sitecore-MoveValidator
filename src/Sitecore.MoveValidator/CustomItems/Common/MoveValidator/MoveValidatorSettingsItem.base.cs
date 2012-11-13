using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Sitecore.SharedSource.MoveValidator.CustomItems.Common.MoveValidator
{
	public partial class MoveValidatorSettingsItem : CustomItem
	{
		public static readonly string TemplateId = "{A36700BE-5519-44A8-9399-C1508AB9C5EC}";

		#region Boilerplate CustomItem Code

		public MoveValidatorSettingsItem(Item innerItem)
			: base(innerItem)
		{
		}

		public static implicit operator MoveValidatorSettingsItem(Item innerItem)
		{
			return innerItem != null ? new MoveValidatorSettingsItem(innerItem) : null;
		}

		public static implicit operator Item(MoveValidatorSettingsItem customItem)
		{
			return customItem != null ? customItem.InnerItem : null;
		}

		#endregion //Boilerplate CustomItem Code

		#region Field Instance Methods

		public MultilistField AppliedLocations
		{
			get { return InnerItem.Fields["Applied Locations"]; }
			set { InnerItem["Applied Locations"] = value.Value; }
		}

		public string AdminAlertMessage
		{
			get { return InnerItem["Admin Alert Message"]; }
		}

		public string UserAlertShortMessage
		{
			get { return InnerItem["User Alert Short Message"]; }
		}

		public string UserAlertLongMessage
		{
			get { return InnerItem["User Alert Long Message"]; }
		}

		#endregion //Field Instance Methods
	}
}