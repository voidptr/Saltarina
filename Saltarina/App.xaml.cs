using Autofac;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Extensions.Logging;
using Saltarina.ViewModels;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace Saltarina
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon trayIcon;
        private Service service;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                Serilog.Debugging.SelfLog.Enable(Console.Error);
                var container = BuildContainer();

                var scope = container.BeginLifetimeScope();

                var logger = scope.Resolve<ILogger<App>>();
                try
                {
                    LogStartup(scope);

                    service = scope.Resolve<Service>();

                    trayIcon = (TaskbarIcon)FindResource("TrayIcon");
                    trayIcon.DataContext = scope.Resolve<INotifyIconViewModel>();

                }
                catch (Exception ex)
                {
                    logger.LogCritical($"An error has occurred. Shutting down. - {ex.Message}", ex);
                    Current?.Shutdown();
                }
            }
            catch (Exception except)
            {
                Console.WriteLine($"Error occurred during application start. - {except}");
                Current?.Shutdown();
            }
        }



        /// <summary>
        /// Declare all dependencies for Autofac dependency injection
        /// and build the container.
        /// </summary>
        private IContainer BuildContainer()
        {
            // set up the dependency injection
            var builder = new ContainerBuilder();

            // Trigger registration of Autofac.Module classes in local assemblies
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            builder.RegisterAssemblyModules(assemblies);

            // set up logging
            builder.Register<Serilog.ILogger>((c, p) =>
            {
                var logfile = "log.txt";

                var logconfig = new LoggerConfiguration()
                    .WriteTo.File(logfile);

                LogEventLevel level = LogEventLevel.Debug;
                logconfig = logconfig.MinimumLevel.Is(level);
                
                return logconfig.CreateLogger();

            }).SingleInstance();
            builder.Register((c, p) =>
            {
                return new LoggerFactory().AddSerilog(c.Resolve<Serilog.ILogger>());
            }).As<ILoggerFactory>().SingleInstance();
            builder.RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>))
                .SingleInstance();

            // Add the Service class
            builder.RegisterType<Service>()
                .AsSelf();

            // Add the Window class
            builder.RegisterType<MainWindow>()
                .AsSelf();

            var container = builder.Build();

            return container;
        }

        private void LogStartup(ILifetimeScope scope)
        {
            var logger = scope.Resolve<ILogger<App>>();

            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            logger.LogInformation("-----------------------------------------");
            logger.LogInformation($"{fvi.ProductName} - {fvi.ProductVersion}");
            logger.LogInformation($"{Environment.MachineName}");
            logger.LogInformation("-----------------------------------------");
        }
    }
}
