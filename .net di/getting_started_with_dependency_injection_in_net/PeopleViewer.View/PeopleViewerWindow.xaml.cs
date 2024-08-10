using PeopleViewer.Presentation;
using System.Windows;

namespace PeopleViewer
{
	public partial class PeopleViewerWindow : Window
	{
		readonly PeopleViewModel _viewModel;

		public PeopleViewerWindow(PeopleViewModel viewModel)
		{
			InitializeComponent();
			this.DataContext = viewModel;
			_viewModel = viewModel;
		}

		private void FetchButton_Click(object sender, RoutedEventArgs e)
		{
			_viewModel.RefreshPeople();
			ShowRepositoryType();
		}

		private void ClearButton_Click(object sender, RoutedEventArgs e)
		{
			_viewModel.ClearPeople();
			ClearRepositoryType();
		}

		private void ShowRepositoryType()
		{
			RepositoryTypeTextBlock.Text = _viewModel.DataReaderType;
		}

		private void ClearRepositoryType()
		{
			RepositoryTypeTextBlock.Text = string.Empty;
		}
	}
}
