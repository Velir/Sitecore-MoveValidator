using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.ItemInterface;

namespace Sitecore.SharedSource.MoveValidator.CustomSitecore.Pipelines
{
	public interface IClientPipelineArgs
	{
		IItem GetSource();
		IItem GetTarget();
		bool IsPostBack();
		bool NeedToAbortPipeline();
		void AbortPipeline();
		void WaitForPostBack();

		bool IsCopy();

		void SetParameter(string name, string value);
		string GetParameter(string name);
	}
}
