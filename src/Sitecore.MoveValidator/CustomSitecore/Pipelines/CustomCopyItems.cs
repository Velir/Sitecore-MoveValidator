﻿using Sitecore.SharedSource.MoveValidator.CustomSitecore.Domain;
using Sitecore.Shell.Framework.Pipelines;
using Sitecore.Web.UI.Sheer;

namespace Sitecore.SharedSource.MoveValidator.CustomSitecore.Pipelines
{
	public class CustomCopyItems : CopyItems, IPipeline
	{
		public void ConstrainMove(ClientPipelineArgs args)
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