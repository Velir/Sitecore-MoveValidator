using Sitecore.SharedSource.MoveValidator.CustomSitecore.Domain;
using Sitecore.SharedSource.MoveValidator.Utils;
using Sitecore.Web.UI.Sheer;

namespace Sitecore.SharedSource.MoveValidator.CustomSitecore.Pipelines
{
	public interface IMoveManager
	{
		void ProcessPostBack();
		void PromptIfNotValid();
	}

	public class PipelineMoveManager : IMoveManager
	{
		private readonly IClientPipelineArgs _iClientPipelineArgs;
		private readonly IMoveValidatorSettings _iMoveValidatorSettings;
		public PipelineMoveManager(IClientPipelineArgs iClientPipelineArgs, IMoveValidatorSettings iMoveValidatorSettings)
		{
			_iClientPipelineArgs = iClientPipelineArgs;
			_iMoveValidatorSettings = iMoveValidatorSettings;
		}

		public bool PostBackProcessed;
		public bool PipelineAborted;
		public void ProcessPostBack()
		{
			if (_iClientPipelineArgs.IsPostBack())
			{
				if (_iClientPipelineArgs.NeedToAbortPipeline())
				{
					_iClientPipelineArgs.AbortPipeline();
					PipelineAborted = true;
				}
				PostBackProcessed = true;
			}
		}

		public bool UserWasPrompted;
		public void PromptIfNotValid()
		{
			bool isValid = MoveUtil.IsValidCopy(_iClientPipelineArgs, _iMoveValidatorSettings);
			if (!isValid)
			{
				MoveUtil.PromptUser(_iClientPipelineArgs, _iMoveValidatorSettings, _iClientPipelineArgs.GetSource(), _iClientPipelineArgs.GetTarget());
				UserWasPrompted = true;
			}
		}
	}
}