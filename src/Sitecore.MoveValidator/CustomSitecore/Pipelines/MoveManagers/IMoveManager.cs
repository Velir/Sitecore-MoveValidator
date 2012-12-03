namespace Sitecore.SharedSource.MoveValidator.CustomSitecore.Pipelines.MoveManagers
{
	public interface IMoveManager
	{
		void ProcessPostBack();
		void PromptIfNotValid();
	}
}