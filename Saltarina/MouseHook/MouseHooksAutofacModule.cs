using Autofac;
using Gma.System.MouseKeyHook;

namespace Saltarina.MouseHook
{
    class MouseHooksAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(ctx =>
            {
                return Hook.GlobalEvents();
            })
                .As<IKeyboardMouseEvents>()
                .SingleInstance();

            builder.RegisterType<MouseHook>()
                .As<IMouseHook>()
                .SingleInstance();
        }
    }
}
