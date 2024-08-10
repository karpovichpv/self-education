using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace PeopleViewer.Presentation.Tests
{
	[TestClass]
	public class PeopleViewModelTests
	{
		[TestMethod]
		public void People_OnRefreshPeople_IsPopulated()
		{
			Common.IPersonReader dataReader = new FakeReader();
			// Arrange
			var viewModel = new PeopleViewModel(dataReader);


			// Act
			viewModel.RefreshPeople();

			// Assert
			Assert.IsNotNull(viewModel.People);
			Assert.AreEqual(2, viewModel.People.Count());
		}

		[TestMethod]
		public void People_OnClearPeople_IsEmpty()
		{
			Common.IPersonReader dataReader = new FakeReader();
			// Arrange
			var viewModel = new PeopleViewModel(dataReader);
			viewModel.RefreshPeople();
			Assert.AreNotEqual(0, viewModel.People.Count(),
				"Invalid arrange");

			// Act
			viewModel.ClearPeople();

			// Assert
			Assert.AreEqual(0, viewModel.People.Count());
		}
	}
}
