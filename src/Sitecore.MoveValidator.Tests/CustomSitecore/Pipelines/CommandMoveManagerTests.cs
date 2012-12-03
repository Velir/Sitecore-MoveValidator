using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Sitecore.MoveValidator.Tests.CustomSitecore.Domain;
using Sitecore.MoveValidator.Tests.CustomSitecore.Pipelines.CustomClientPipelineArgs;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.Domain;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.Pipelines.CustomClientPipelineArgs;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.Pipelines.MoveManagers;
using Sitecore.Web.UI.Sheer;

namespace Sitecore.MoveValidator.Tests.CustomSitecore.Pipelines
{
	[TestFixture]
	public class CommandMoveManagerTests
	{

		[Test]
		public void PipelineAbortedTest()
		{
			ClientPipelineArgs clientPipelineArgs = new ClientPipelineArgs { IsPostBack = true, Result = "no" };
			IClientPipelineArgs iClientPipelineArgs = new CustomPasteFromClipBoardArgs(clientPipelineArgs);
			IMoveValidatorSettings iMoveValidatorSettings = new MoveValidatorTestSettings();

			CommandMoveManager mm = new CommandMoveManager(iClientPipelineArgs, iMoveValidatorSettings);
			mm.ProcessPostBack();

			Assert.IsTrue(mm.PostBackProcessed);
			Assert.IsTrue(mm.PipelineAborted);
		}


		[Test]
		public void PipelineNotAbortedTest()
		{
			ClientPipelineArgs clientPipelineArgs = new ClientPipelineArgs { IsPostBack = true, Result = "yes" };
			IClientPipelineArgs iClientPipelineArgs = new CustomPasteFromClipBoardArgs(clientPipelineArgs);
			IMoveValidatorSettings iMoveValidatorSettings = new MoveValidatorTestSettings();

			CommandMoveManager mm = new CommandMoveManager(iClientPipelineArgs, iMoveValidatorSettings);
			mm.ProcessPostBack();

			Assert.IsTrue(mm.PostBackProcessed);
			Assert.IsFalse(mm.PipelineAborted);
		}


		[Test]
		public void PostBackProcessedTest()
		{
			ClientPipelineArgs clientPipelineArgs = new ClientPipelineArgs { IsPostBack = true };
			IClientPipelineArgs iClientPipelineArgs = new CustomPasteFromClipBoardArgs(clientPipelineArgs);
			IMoveValidatorSettings iMoveValidatorSettings = new MoveValidatorTestSettings();

			CommandMoveManager mm = new CommandMoveManager(iClientPipelineArgs, iMoveValidatorSettings);
			mm.ProcessPostBack();

			Assert.IsTrue(mm.PostBackProcessed);
		}

		[Test]
		public void PostBackNotProcessedTest()
		{
			ClientPipelineArgs clientPipelineArgs = new ClientPipelineArgs { IsPostBack = false };
			IClientPipelineArgs iClientPipelineArgs = new CustomPasteFromClipBoardArgs(clientPipelineArgs);
			IMoveValidatorSettings iMoveValidatorSettings = new MoveValidatorTestSettings();

			CommandMoveManager mm = new CommandMoveManager(iClientPipelineArgs, iMoveValidatorSettings);
			mm.ProcessPostBack();

			Assert.IsFalse(mm.PostBackProcessed);
		}


		[Test]
		public void UserWasPrompted()
		{
			ClientPipelineArgs clientPipelineArgs = new ClientPipelineArgs { IsPostBack = false };
			clientPipelineArgs.Parameters["database"] = "master";
			IClientPipelineArgs iClientPipelineArgs = new ClientPipelineTestArgs(clientPipelineArgs);
			IMoveValidatorSettings iMoveValidatorSettings = new MoveValidatorTestSettings();

			CommandMoveManager mm = new CommandMoveManager(iClientPipelineArgs, iMoveValidatorSettings);
			mm.PromptIfNotValid();

			Assert.IsFalse(mm.UserWasPrompted);
		}

	}
}
