using Autofac;

namespace Saltarina.ViewModels
{
    public class ViewModelsAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<NotifyIconViewModel>()
                .As<INotifyIconViewModel>()
                .SingleInstance();

            // Register classes for the WebSocket Cloud Connection
            builder.RegisterType<AboutViewModel>()
                .As<IAboutViewModel>()
                .SingleInstance();
        }
    }   
}
