using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Population;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Mafi.Base.Assets.Base.Tutorials;

namespace ControlRoomMod;

[GenerateSerializer(false, null, 0)]
public class ControlRoom : LayoutEntity, IEntityWithWorkers, IEntityWithSimUpdate //, IAnimatedEntity, IEntityWithParticles
{
    private ControlRoomPrototype _proto;

    public enum State
    {
        None,
        Working,
        Paused,
        NotEnoughWorkers,
    }

    public ControlRoom(EntityId id,
            ControlRoomPrototype proto,
            TileTransform transform,
            EntityContext context,
            IAnimationStateFactory animationStateFactory)
    : base(id, proto, transform, context)

    {
        _proto = proto;
    }

    public override bool CanBePaused => true;

    int IEntityWithWorkers.WorkersNeeded => Prototype.Costs.Workers;
    [DoNotSave(0, null)]
    bool IEntityWithWorkers.HasWorkersCached { get; set; }

    void IEntityWithSimUpdate.SimUpdate()
    {
    }

}
