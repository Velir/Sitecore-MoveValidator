using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using Sitecore.Data;
using Sitecore.MoveValidator.Tests.CustomSitecore.MoveableItems;
using Sitecore.SharedSource.MoveValidator.CustomSitecore.MoveableItems;
using Sitecore.SharedSource.MoveValidator.Utils;

namespace Sitecore.MoveValidator.Tests.CustomSitecore.Utils
{
	[TestFixture]
	public class MoveUtilsTests
	{

		[Test]
		public void TheDestinationIsMonitoredTest()
		{
			// assemble
			// let's say we are configured to monitor three locations
			IMoveableItem monitoredItemA = new MoveableTestItem();
			IMoveableItem monitoredItemB = new MoveableTestItem();
			IMoveableItem monitoredItemC = new MoveableTestItem();

			// create a stub to return true when location C is checked
			IMoveableItem destinationItem = MockRepository.GenerateStub<IMoveableItem>();
			destinationItem.Stub(x => x.IsDescendantOf(monitoredItemA)).Return(false);
			destinationItem.Stub(x => x.IsDescendantOf(monitoredItemB)).Return(false);
			destinationItem.Stub(x => x.IsDescendantOf(monitoredItemC)).Return(true);

			// add the items to a list
			IList<IMoveableItem> monitoredLocations = new List<IMoveableItem>();
			monitoredLocations.Add(monitoredItemA);
			monitoredLocations.Add(monitoredItemB);
			monitoredLocations.Add(monitoredItemC);

			// action
			bool destinationIsMonitored = MoveUtils.IsDestinationMonitored(monitoredLocations, destinationItem);

			// assert
			Assert.IsTrue(destinationIsMonitored);
		}



		[Test]
		public void TheDestinationIsNotMonitoredTest()
		{
			// assemble
			// let's say we are configured to monitor three locations
			IMoveableItem monitoredItemA = new MoveableTestItem();
			IMoveableItem monitoredItemB = new MoveableTestItem();
			IMoveableItem monitoredItemC = new MoveableTestItem();

			// create a stub to return true when location C is checked
			IMoveableItem destinationItem = MockRepository.GenerateStub<IMoveableItem>();
			destinationItem.Stub(x => x.IsDescendantOf(monitoredItemA)).Return(false);
			destinationItem.Stub(x => x.IsDescendantOf(monitoredItemB)).Return(false);
			destinationItem.Stub(x => x.IsDescendantOf(monitoredItemC)).Return(false);

			// add the items to a list
			IList<IMoveableItem> monitoredLocations = new List<IMoveableItem>();
			monitoredLocations.Add(monitoredItemA);
			monitoredLocations.Add(monitoredItemB);
			monitoredLocations.Add(monitoredItemC);

			// action
			bool destinationIsMonitored = MoveUtils.IsDestinationMonitored(monitoredLocations, destinationItem);

			// assert
			Assert.IsFalse(destinationIsMonitored);
		}


		[Test]
		public void ThereAreNoMonitoredDestinationsTest()
		{
			// assemble
			IMoveableItem destinationItem = MockRepository.GenerateStub<IMoveableItem>();

			// we have an empty list of monitored locations
			IList<IMoveableItem> monitoredLocations = new List<IMoveableItem>();

			// action
			bool destinationIsMonitored = MoveUtils.IsDestinationMonitored(monitoredLocations, destinationItem);

			// assert
			Assert.IsFalse(destinationIsMonitored);
		}

		[Test]
		public void IsInInsertOptions()
		{
			// assemble
			string guidA = ID.NewID.ToString();
			string guidB = ID.NewID.ToString();
			string guidC = ID.NewID.ToString();

			IMoveableItem targetItem = new MoveableTestItem();
			targetItem.InsertOptions = new List<string> { guidA, guidB, guidC };

			IMoveableItem sourceItem = new MoveableTestItem();
			sourceItem.TemplateId = new ID(guidC);

			// action
			bool itemIsAllowedInInsertOptions = MoveUtils.IsItemAllowedInInsertOptions(sourceItem, targetItem);

			// assert
			Assert.IsTrue(itemIsAllowedInInsertOptions);
		}

		[Test]
		public void IsNotInInsertOptions()
		{
			// assemble
			string guidA = ID.NewID.ToString();
			string guidB = ID.NewID.ToString();
			string guidC = ID.NewID.ToString();

			IMoveableItem targetItem = new MoveableTestItem();
			targetItem.InsertOptions = new List<string> { guidA, guidB };

			IMoveableItem sourceItem = new MoveableTestItem();
			sourceItem.TemplateId = new ID(guidC);

			// action
			bool itemIsAllowedInInsertOptions = MoveUtils.IsItemAllowedInInsertOptions(sourceItem, targetItem);

			// assert
			Assert.IsFalse(itemIsAllowedInInsertOptions);
		}


		[Test]
		public void BranchIsInInsertOptionsTest()
		{
			// assemble
			string guidA = ID.NewID.ToString();
			string guidB = ID.NewID.ToString();
			string guidC = ID.NewID.ToString();

			IMoveableItem targetItem = new MoveableTestItem();
			targetItem.InsertOptions = new List<string> { guidA, guidB, guidC };

			IMoveableItem sourceItem = new MoveableTestItem();
			sourceItem.TemplateId = new ID(guidC);
			sourceItem.BranchId = new ID(guidC);
			sourceItem.BranchTemplateIds = new List<string> { guidC };

			// action
			bool branchIsAllowedInInsertOptions = MoveUtils.IsBranchAllowedInInsertOptions(sourceItem, targetItem);

			// assert
			Assert.IsTrue(branchIsAllowedInInsertOptions);
		}

		[Test]
		public void BranchIsNotInInsertOptionsTest()
		{
			// assemble
			string guidA = ID.NewID.ToString();
			string guidB = ID.NewID.ToString();
			string guidC = ID.NewID.ToString();

			IMoveableItem targetItem = new MoveableTestItem();
			targetItem.InsertOptions = new List<string> { guidA, guidB };

			IMoveableItem sourceItem = new MoveableTestItem();
			sourceItem.TemplateId = new ID(guidC);
			sourceItem.BranchId = new ID(guidC);
			sourceItem.BranchTemplateIds = new List<string> { guidC };

			// action
			bool branchIsAllowedInInsertOptions = MoveUtils.IsBranchAllowedInInsertOptions(sourceItem, targetItem);

			// assert
			Assert.IsFalse(branchIsAllowedInInsertOptions);
		}


		[Test]
		public void ItemIsNotFromBranchTest()
		{
			// assemble
			string guidA = ID.NewID.ToString();
			string guidB = ID.NewID.ToString();
			string guidC = ID.NewID.ToString();

			IMoveableItem targetItem = new MoveableTestItem();
			targetItem.InsertOptions = new List<string> { guidA, guidB };

			IMoveableItem sourceItem = new MoveableTestItem();
			sourceItem.TemplateId = new ID(guidC);
			//sourceItem.BranchId = new ID(guidC);
			sourceItem.BranchTemplateIds = new List<string> { guidC };

			// action
			bool branchIsAllowedInInsertOptions = MoveUtils.IsBranchAllowedInInsertOptions(sourceItem, targetItem);

			// assert
			Assert.IsFalse(branchIsAllowedInInsertOptions);
		}
	}
}
