using Sitecore.SharedSource.MoveValidator.CustomSitecore.Pipelines;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Sitecore.Web.UI.Sheer;

namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for CustomCopyItemsTest and is intended
    ///to contain all CustomCopyItemsTest Unit Tests
    ///</summary>
	[TestClass()]
	public class CustomCopyItemsTest
	{



		/// <summary>
		///A test for ConstrainMove
		///</summary>
		[TestMethod()]
		public void ConstrainMoveTest()
		{
			CustomCopyItems target = new CustomCopyItems(); // TODO: Initialize to an appropriate value
			ClientPipelineArgs args = null; // TODO: Initialize to an appropriate value
			target.ConstrainMove(args);
			Assert.Inconclusive("A method that does not return a value cannot be verified.");
		}
	}
}
