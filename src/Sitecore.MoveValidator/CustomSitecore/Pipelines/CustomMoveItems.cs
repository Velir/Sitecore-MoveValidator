using Sitecore.SharedSource.MoveValidator.CustomSitecore.CustomClientPipelineArgs;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.Domain;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.Pipelines.MoveManagers;
using Sitecore.Shell.Framework.Pipelines;
using Sitecore.Web.UI.Sheer;

namespace Sitecore.SharedSource.MoveValidator.CustomSitecore.Pipelines
{
	public class CustomMoveItems : MoveItems
	{
		public void ConstrainMove(ClientPipelineArgs args)
		{
			IClientPipelineArgs iClientPipelineArgs = new MoveClientPipelineItemArgs(args);
			IMoveValidatorSettings iMoveValidatorSettings = new MoveValidatorSettings();
			PipelineMoveManager mm = new PipelineMoveManager(iClientPipelineArgs, iMoveValidatorSettings);

			mm.ProcessPostBack();
			if (mm.PostBackProcessed)
			{
				return;
			}

			mm.PromptIfNotValid();
		}
	}
}