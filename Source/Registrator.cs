using Mafi.Base;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlRoomMod;

public class ControlRoomRegistrator : IModData
{
    public void RegisterData(ProtoRegistrator registrator)
    {
        Proto.Str ps = Proto.CreateStr(PrototypeIDs.LocalEntities.ControlRoomID, "ControlRoom", "ControlRoom description");
        EntityLayout el = registrator.LayoutParser.ParseLayoutOrThrow("[8]");

        EntityCostsTpl ecTpl = new EntityCostsTpl.Builder().CP(1).Workers(10);
        EntityCosts ec = ecTpl.MapToEntityCosts(registrator);

        LayoutEntityProto.Gfx lg =
            new LayoutEntityProto.Gfx("Assets/ExampleMod/ExampleModel/CraneTextured/CraneTexturedParticle.prefab",

            customIconPath: "Assets/Unity/Generated/Icons/LayoutEntity/Flare.png",
            categories: new ImmutableArray<ToolbarCategoryProto>?(registrator.GetCategoriesProtos(Ids.ToolbarCategories.Landmarks)))
            ;

        ControlRoomPrototype bp =
            new ControlRoomPrototype(
                PrototypeIDs.LocalEntities.ControlRoomID, ps, el, ec, lg);
        registrator.PrototypesDb.Add(bp);
    }
}
