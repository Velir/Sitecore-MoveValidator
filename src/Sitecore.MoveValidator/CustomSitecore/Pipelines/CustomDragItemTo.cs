﻿using Sitecore.SharedSource.MoveValidator.CustomSitecore.Domain;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.Pipelines.CustomClientPipelineArgs;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.Pipelines.MoveManagers;
using Sitecore.Shell.Framework.Pipelines;
using Sitecore.Web.UI.Sheer;

namespace Sitecore.SharedSource.MoveValidator.CustomSitecore.Pipelines
{
	public class CustomDragItemTo : DragItemTo
	{
		public void ConstrainDragTo(ClientPipelineArgs args)
		{
			IClientPipelineArgs iClientPipelineArgs = new CopyClientPipelineItemArgs(args);
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