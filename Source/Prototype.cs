using ControlRoomMod;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Prototypes;
using Mafi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Mafi.Core.Prototypes.Proto;

namespace ControlRoomMod;

public partial class PrototypeIDs
{
    public partial class LocalEntities
    {
        public static readonly ControlRoomPrototype.ID ControlRoomID = new ControlRoomPrototype.ID("ControlRoom");
    }
}

public class ControlRoomPrototype : LayoutEntityProto, IProto
{
    public ControlRoomPrototype(ID id, Str strings, EntityLayout layout, EntityCosts costs, Gfx graphics)
        : base(id, strings, layout, costs, graphics)
    {
    }

    public override Type EntityType => typeof(ControlRoom);


}
