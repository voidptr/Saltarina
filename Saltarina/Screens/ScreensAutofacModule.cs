using Autofac;
using Gma.System.MouseKeyHook;

namespace Saltarina.Screens
{
    public class ScreensAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ScreenMapper>()
                .As<IScreenMapper>()
                .SingleInstance();

            builder.RegisterType<ScreenManager>()
                .As<IScreenManager>()
                .SingleInstance();
        }
    }
}
