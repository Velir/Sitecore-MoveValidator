using NUnit.Framework;
using Sitecore.MoveValidator.Tests.CustomSitecore.CustomClientPipelineArgs;
using Sitecore.MoveValidator.Tests.CustomSitecore.Domain;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.CustomClientPipelineArgs;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.Domain;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.Pipelines.MoveManagers;
using Sitecore.Web.UI.Sheer;


namespace Sitecore.MoveValidator.Tests.CustomSitecore.Pipelines
{
	[TestFixture]
	public class PipelineMoveManagerTests
	{

		[Test]
		public void PipelineAbortedTest()
		{
			// assemble
			ClientPipelineArgs clientPipelineArgs = new ClientPipelineArgs {IsPostBack = true, Result = "no"};
			IClientPipelineArgs iClientPipelineArgs = new CopyClientPipelineItemArgs(clientPipelineArgs);
			IMoveValidatorSettings iMoveValidatorSettings = new MoveValidatorTestSettings();
			PipelineMoveManager mm = new PipelineMoveManager(iClientPipelineArgs, iMoveValidatorSettings);

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
			ClientPipelineArgs clientPipelineArgs = new ClientPipelineArgs { IsPostBack = true, Result = "yes" };
			IClientPipelineArgs iClientPipelineArgs = new CopyClientPipelineItemArgs(clientPipelineArgs);
			IMoveValidatorSettings iMoveValidatorSettings = new MoveValidatorTestSettings();
			PipelineMoveManager mm = new PipelineMoveManager(iClientPipelineArgs, iMoveValidatorSettings);

			// action
			mm.ProcessPostBack();

			// assert
			Assert.IsTrue(mm.PostBackProcessed);
			Assert.IsFalse(mm.PipelineAborted);
		}


		[Test]
		public void PostBackProcessedTest()
		{
			// assemble
			ClientPipelineArgs clientPipelineArgs = new ClientPipelineArgs { IsPostBack = true };
			IClientPipelineArgs iClientPipelineArgs = new CopyClientPipelineItemArgs(clientPipelineArgs);
			IMoveValidatorSettings iMoveValidatorSettings = new MoveValidatorTestSettings();
			PipelineMoveManager mm = new PipelineMoveManager(iClientPipelineArgs, iMoveValidatorSettings);

			// action
			mm.ProcessPostBack();

			// assert
			Assert.IsTrue(mm.PostBackProcessed);
		}

		[Test]
		public void PostBackNotProcessedTest()
		{
			// assemble
			ClientPipelineArgs clientPipelineArgs = new ClientPipelineArgs { IsPostBack = false };
			IClientPipelineArgs iClientPipelineArgs = new CopyClientPipelineItemArgs(clientPipelineArgs);
			IMoveValidatorSettings iMoveValidatorSettings = new MoveValidatorTestSettings();
			PipelineMoveManager mm = new PipelineMoveManager(iClientPipelineArgs, iMoveValidatorSettings);

			// action
			mm.ProcessPostBack();

			// assert
			Assert.IsFalse(mm.PostBackProcessed);
		}


		[Test]
		public void UserWasPrompted()
		{
			// assemble
			ClientPipelineArgs clientPipelineArgs = new ClientPipelineArgs { IsPostBack = false };
			clientPipelineArgs.Parameters["database"] = "master";
			IClientPipelineArgs iClientPipelineArgs = new ClientPipelineTestArgs(clientPipelineArgs);
			IMoveValidatorSettings iMoveValidatorSettings = new MoveValidatorTestSettings();
			PipelineMoveManager mm = new PipelineMoveManager(iClientPipelineArgs, iMoveValidatorSettings);

			// action
			mm.PromptIfNotValid();

			// assert
			Assert.IsFalse(mm.UserWasPrompted);
		}
	}
}
