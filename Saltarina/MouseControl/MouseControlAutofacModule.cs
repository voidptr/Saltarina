using Autofac;

namespace Saltarina.MouseControl
{
    public class MouseControlAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MouseControl>()
                .As<IMouseControl>()
                .SingleInstance();
        }
    }
}
