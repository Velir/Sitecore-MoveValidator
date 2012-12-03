using System.Collections.Generic;
using NUnit.Framework;
using Sitecore.MoveValidator.Tests.CustomSitecore.CustomClientPipelineArgs;
using Sitecore.MoveValidator.Tests.CustomSitecore.Domain;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.CustomClientPipelineArgs;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.Domain;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.MoveableItems;
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
			// assemble
			ClientPipelineArgs clientPipelineArgs = new ClientPipelineArgs { IsPostBack = true, Result = "yes" };
			IClientPipelineArgs iClientPipelineArgs = new CustomPasteFromClipBoardArgs(clientPipelineArgs);
			IMoveValidatorSettings iMoveValidatorSettings = new MoveValidatorTestSettings();
			CommandMoveManager mm = new CommandMoveManager(iClientPipelineArgs, iMoveValidatorSettings);

			// action
			mm.ProcessPostBack();

			// assert
			Assert.IsTrue(mm.PostBackProcessed);
			Assert.IsTrue(mm.PipelineAborted);
		}


		[Test]
		public void PipelineNotAbortedTest()
		{
			// assemble
			ClientPipelineArgs clientPipelineArgs = new ClientPipelineArgs { IsPostBack = true, Result = "no" };
			IClientPipelineArgs iClientPipelineArgs = new CustomPasteFromClipBoardArgs(clientPipelineArgs);
			IMoveValidatorSettings iMoveValidatorSettings = new MoveValidatorTestSettings();
			CommandMoveManager mm = new CommandMoveManager(iClientPipelineArgs, iMoveValidatorSettings);

			// action
			mm.ProcessPostBack();

			// assert
			Assert.IsTrue(mm.PostBackProcessed);
			Assert.IsFalse(mm.PipelineAborted);
		}


		[Test]
		public void PostBackProcessSkippedTest()
		{
			// assemble
			ClientPipelineArgs clientPipelineArgs = new ClientPipelineArgs { IsPostBack = true };
			IClientPipelineArgs iClientPipelineArgs = new CustomPasteFromClipBoardArgs(clientPipelineArgs);
			IMoveValidatorSettings iMoveValidatorSettings = new MoveValidatorTestSettings();
			CommandMoveManager mm = new CommandMoveManager(iClientPipelineArgs, iMoveValidatorSettings);

			// action
			mm.ProcessPostBack();

			// assert
			Assert.IsFalse(mm.PostBackProcessed);
		}

		[Test]
		public void PostBackNotProcessedTest()
		{
			// assemble
			ClientPipelineArgs clientPipelineArgs = new ClientPipelineArgs { IsPostBack = false };
			IClientPipelineArgs iClientPipelineArgs = new CustomPasteFromClipBoardArgs(clientPipelineArgs);
			IMoveValidatorSettings iMoveValidatorSettings = new MoveValidatorTestSettings();
			CommandMoveManager mm = new CommandMoveManager(iClientPipelineArgs, iMoveValidatorSettings);

			// action
			mm.ProcessPostBack();

			// assert
			Assert.IsFalse(mm.PostBackProcessed);
		}


		[Test]
		public void UserWasPrompted()
		{
			// assemble
			ClientPipelineArgs clientPipelineArgs = new ClientPipelineArgs();
			IClientPipelineArgs iClientPipelineArgs = new ClientPipelineTestArgs(clientPipelineArgs);
			IMoveValidatorSettings iMoveValidatorSettings = new MoveValidatorTestSettings();
			// add target to monitored locations
			iMoveValidatorSettings.MonitoredLocations = new List<IMoveableItem> {iClientPipelineArgs.GetTarget()};
			CommandMoveManager mm = new CommandMoveManager(iClientPipelineArgs, iMoveValidatorSettings);

			// action
			mm.PromptIfNotValid();

			// assert
			Assert.IsFalse(mm.UserWasPrompted);
		}

	}
}
