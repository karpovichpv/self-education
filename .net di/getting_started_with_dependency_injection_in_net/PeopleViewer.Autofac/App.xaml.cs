﻿using Autofac;
using PeopleViewer.Common;
using PeopleViewer.Presentation;
using PersonDataReader.Decorators;
using PersonDataReader.Service;
using System.Windows;

namespace PeopleViewer.Autofac
{
	public partial class App : Application
	{
		IContainer Container;
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			ConfigureContainer();
			ComposeObjects();
			Application.Current.MainWindow.Title = "With Dependency Injection - Autofac";
			Application.Current.MainWindow.Show();
		}

		private void ConfigureContainer()
		{
			var builder = new ContainerBuilder();
			builder.RegisterType<ServiceReader>()
				.Named<IPersonReader>("reader")
				.SingleInstance();

			builder.RegisterDecorator<IPersonReader>((c, inner) => new CachingReader(inner), fromKey: "reader");


			builder.RegisterType<PeopleViewerWindow>().InstancePerDependency();
			builder.RegisterType<PeopleViewModel>().InstancePerDependency();

			Container = builder.Build();
		}

		private void ComposeObjects()
		{
			Application.Current.MainWindow = Container.Resolve<PeopleViewerWindow>();

		}
	}
}