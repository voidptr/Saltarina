using Autofac;

namespace Saltarina.ViewModels
{
    public class ViewModelsAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register classes for the WebSocket Cloud Connection
            builder.RegisterType<AboutViewModel>()
                .As<IAboutViewModel>()
                .SingleInstance();
        }
    }   
}
