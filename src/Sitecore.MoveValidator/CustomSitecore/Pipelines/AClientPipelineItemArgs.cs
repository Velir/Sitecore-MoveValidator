using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.ItemInterface;
using Sitecore.Web.UI.Sheer;

namespace Sitecore.SharedSource.MoveValidator.CustomSitecore.Pipelines
{
	public abstract class AClientPipelineItemArgs : IClientPipelineArgs
	{
		protected ClientPipelineArgs _args;

		public AClientPipelineItemArgs(ClientPipelineArgs args)
		{
			_args = args;
		}

		public abstract IItem GetSource();
		public abstract IItem GetTarget();

		protected virtual Database GetDatabase()
		{
			Database database = Factory.GetDatabase(_args.Parameters["database"]);
			Assert.IsNotNull(database, typeof(Database), "Database: {0}", new object[] { _args.Parameters["database"] });
			return database;
		}


		public bool NeedToAbortPipeline()
		{
			return (_args.Result != "yes");
		}


		public bool IsPostBack()
		{
			return _args.IsPostBack;
		}

		public void AbortPipeline()
		{
			_args.AbortPipeline();
		}
		public void WaitForPostBack()
		{
			_args.WaitForPostBack();
		}


		public bool IsCopy()
		{
			string str2 = StringUtil.GetString(new[] { _args.Result });
			if (str2.StartsWith("sitecore:copy:"))
			{
				return true;
			}
			return false;
		}

		public void SetParameter(string name, string value)
		{
			_args.Parameters[name] = value;
		}

		public string GetParameter(string name)
		{
			return _args.Parameters[name] as string;
		}
	}
}