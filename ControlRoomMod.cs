using Mafi.Collections;
using Mafi.Core.Game;
using Mafi;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomMod;

public sealed class ControlRoomMod : IMod
{
    public string Name => "ControlRoom";

    public int Version => 1;
    public static Version ModVersion = new Version(0, 0, 1);
    public bool IsUiOnly => false;

    public Option<IConfig> ModConfig { get; }

    public void ChangeConfigs(Lyst<IConfig> configs)
    {
    }

    public void Initialize(DependencyResolver resolver, bool gameWasLoaded)
    {
        LogWrite.Info("Initializing ");
    }

    public void RegisterDependencies(DependencyResolverBuilder depBuilder, ProtosDb protosDb, bool gameWasLoaded)
    {
        LogWrite.Info("Register Dependencies ");
    }

    public void RegisterPrototypes(ProtoRegistrator registrator)
    {
        LogWrite.Info("Registrating Prototypes");
        registrator.RegisterData<ControlRoomRegistrator>();

    }

    public void EarlyInit(DependencyResolver resolver)
    {
        LogWrite.Info("EarlyInit");
    }
}
