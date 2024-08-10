using Ninject;
using PeopleViewer.Common;
using PersonDataReader.Decorators;
using PersonDataReader.Service;
using System.Windows;

namespace PeopleViewer.Ninject
{
	public partial class App : Application
	{
		private IKernel Container = new StandardKernel();

		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			ConfigureContainer();
			ComposeObjects();
			Application.Current.MainWindow.Title = "With Dependency Injection - Ninject";
			Application.Current.MainWindow.Show();
		}

		private void ConfigureContainer()
		{
			Container.Bind<IPersonReader>().To<CachingReader>()
				.InSingletonScope()
				.WithConstructorArgument<IPersonReader>(Container.Get<ServiceReader>());
		}

		private void ComposeObjects()
		{
			Application.Current.MainWindow = Container.Get<PeopleViewerWindow>();
		}
	}
}
