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

            builder.RegisterType<DisplayManager>()
                .As<IDisplayManager>()
                .SingleInstance();

            builder.RegisterType<ScreenModel>()
                .AsSelf();

            builder.RegisterType<ScreenWrapper>()
                .As<IScreenWrapper>()
                .SingleInstance();

        }
    }
}
