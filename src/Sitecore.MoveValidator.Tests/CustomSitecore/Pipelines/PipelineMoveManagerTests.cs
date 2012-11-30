using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.Domain;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.ItemInterface;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.Pipelines;
using Sitecore.Web.UI.Sheer;

namespace Sitecore.MoveValidator.Tests.CustomSitecore.Pipelines
{
	[TestFixture]
	public class PipelineMoveManagerTests
	{

		[Test]
		public void PipelineAbortedTest()
		{
			ClientPipelineArgs clientPipelineArgs = new ClientPipelineArgs {IsPostBack = true, Result = "no"};
			IClientPipelineArgs iClientPipelineArgs = new CopyClientPipelineItemArgs(clientPipelineArgs);
			IMoveValidatorSettings iMoveValidatorSettings = new MoveValidatorSettingsTester();

			PipelineMoveManager mm = new PipelineMoveManager(iClientPipelineArgs, iMoveValidatorSettings);
			mm.ProcessPostBack();

			Assert.IsTrue(mm.PostBackProcessed);
			Assert.IsTrue(mm.PipelineAborted);
		}

		[Test]
		public void PipelineNotAbortedTest()
		{
			ClientPipelineArgs clientPipelineArgs = new ClientPipelineArgs { IsPostBack = true, Result = "yes" };
			IClientPipelineArgs iClientPipelineArgs = new CopyClientPipelineItemArgs(clientPipelineArgs);
			IMoveValidatorSettings iMoveValidatorSettings = new MoveValidatorSettingsTester();

			PipelineMoveManager mm = new PipelineMoveManager(iClientPipelineArgs, iMoveValidatorSettings);
			mm.ProcessPostBack();

			Assert.IsTrue(mm.PostBackProcessed);
			Assert.IsFalse(mm.PipelineAborted);
		}


		[Test]
		public void PostBackProcessedTest()
		{
			ClientPipelineArgs clientPipelineArgs = new ClientPipelineArgs { IsPostBack = true };
			IClientPipelineArgs iClientPipelineArgs = new CopyClientPipelineItemArgs(clientPipelineArgs);
			IMoveValidatorSettings iMoveValidatorSettings = new MoveValidatorSettingsTester();

			PipelineMoveManager mm = new PipelineMoveManager(iClientPipelineArgs, iMoveValidatorSettings);
			mm.ProcessPostBack();

			Assert.IsTrue(mm.PostBackProcessed);
		}

		[Test]
		public void PostBackNotProcessedTest()
		{
			ClientPipelineArgs clientPipelineArgs = new ClientPipelineArgs { IsPostBack = false };
			IClientPipelineArgs iClientPipelineArgs = new CopyClientPipelineItemArgs(clientPipelineArgs);
			IMoveValidatorSettings iMoveValidatorSettings = new MoveValidatorSettingsTester();

			PipelineMoveManager mm = new PipelineMoveManager(iClientPipelineArgs, iMoveValidatorSettings);
			mm.ProcessPostBack();

			Assert.IsFalse(mm.PostBackProcessed);
		}


		[Test]
		public void UserWasPrompted()
		{
			ClientPipelineArgs clientPipelineArgs = new ClientPipelineArgs { IsPostBack = false };
			clientPipelineArgs.Parameters["database"] = "master";
			IClientPipelineArgs iClientPipelineArgs = new ClientPipelineTestArgs(clientPipelineArgs);
			IMoveValidatorSettings iMoveValidatorSettings = new MoveValidatorSettingsTester();

			PipelineMoveManager mm = new PipelineMoveManager(iClientPipelineArgs, iMoveValidatorSettings);
			mm.PromptIfNotValid();

			Assert.IsFalse(mm.UserWasPrompted);
		}
	}

	public class ClientPipelineTestArgs : AClientPipelineItemArgs
	{
		public ClientPipelineTestArgs(ClientPipelineArgs args)
			: base(args)
		{
		}

		public override IItem GetSource()
		{
			IItem sourceItem = new TestItem();
			sourceItem.Id = ID.NewID;
			sourceItem.Name = "source";
			return sourceItem;
		}

		public override IItem GetTarget()
		{
			IItem targetItem = new TestItem();
			targetItem.Id = ID.NewID;
			targetItem.Name = "source";
			return targetItem;
		}
	}
}
