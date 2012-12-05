﻿using Sitecore.SharedSource.MoveValidator.CustomSitecore.MoveableItems;

namespace Sitecore.SharedSource.MoveValidator.CustomSitecore.CustomClientPipelineArgs
{
	public interface IClientPipelineArgs
	{
		IMoveableItem GetSource();
		IMoveableItem GetTarget();
		bool IsPostBack();
		bool NeedToAbortPipeline();
		void AbortPipeline();
		void WaitForPostBack();
		string Result();

		bool IsCopy();

		void SetParameter(string name, string value);
		string GetParameter(string name);
	}
}