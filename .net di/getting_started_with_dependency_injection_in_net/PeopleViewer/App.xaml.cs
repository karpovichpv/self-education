using PeopleViewer.Common;
using PeopleViewer.Presentation;
using PersonDataReader.Decorators;
using PersonDataReader.Service;
using System.Windows;

namespace PeopleViewer
{
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			ComposeOptions();
			Application.Current.MainWindow.Title = "With Dependency Indjection";
			Application.Current.MainWindow.Show();
		}

		private static void ComposeOptions()
		{
			IPersonReader reader = new ServiceReader();
			IPersonReader cachingReader = new CachingReader(reader);
			PeopleViewModel viewModel = new PeopleViewModel(cachingReader);
			Application.Current.MainWindow = new PeopleViewerWindow(viewModel);
		}
	}
}
