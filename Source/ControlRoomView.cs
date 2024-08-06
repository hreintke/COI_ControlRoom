using ControlRoomMod;
using Mafi;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Syncers;
using Mafi.Unity;
using Mafi.Unity.InputControl.Inspectors;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using static Mafi.Unity.Assets.Unity.Generated.Icons;
using Mafi.Core.Buildings.Storages;
using Mafi.Core.Products;
using Mafi.Unity.Entities;
using static Mafi.Base.Assets.Base.Buildings;
using System.Runtime.InteropServices.WindowsRuntime;

namespace ControllerMod;

public class ControlRoomView : StaticEntityInspectorBase<ControlRoom>
{
//    private class VehiclesStatsView : IUiElement, IDynamicSizeElement
//    {
//    }

    private readonly ControlRoomInspector _inspector;
    private readonly InspectorController _inspectorController;
    private readonly EntitiesManager _entitiesManager;
    private bool supressCursor = false;
    public EntityId lastClickedEntity = EntityId.Invalid;

    public ControlRoomView(ControlRoomInspector inspector, InspectorController ic, EntitiesManager em) : base(inspector)
    {
        _inspector = inspector;
        _inspectorController = ic;
        _entitiesManager = em;
    }

    protected override ControlRoom Entity => _inspector.SelectedEntity;

    protected override void AddCustomItems(StackContainer itemContainer)
    {
        this.SetWidth(700);
        base.AddCustomItems(itemContainer);
        StatusPanel statusInfo = AddStatusInfoPanel();
        statusInfo.SetStatusWorking();
        UpdaterBuilder updaterBuilder = UpdaterBuilder.Start();
        itemContainer.SetItemSpacing(5f);
        
        AddSectionTitle(itemContainer, "Control actions");

        var productButton = Builder
        .NewBtnPrimary("PHL Usage Button")
           .SetButtonStyle(Builder.Style.Global.ListMenuBtn)
           .SetText("Toggle Selection Tool")
           .SetTextAlignment(TextAnchor.MiddleLeft)
           .SetSize(new Vector2(175f, 30f))
           .OnClick(() =>
           {
               _inspector.setToolStatus(!_inspector.toolActive);
           });
        productButton.AppendTo(itemContainer);

        var toolStatusLabel = Builder.NewTxt("Default");
        toolStatusLabel.AppendTo(itemContainer);

        updaterBuilder.Observe<bool>((Func<bool>)(() => _inspector.toolActive))
            .Do((Action<bool>)(ts =>
            {
                toolStatusLabel.SetText($"tool is {ts}");
            }));


        var productButtonContainer = Builder
            .NewStackContainer("PHL Top")
            .SetStackingDirection(StackContainer.Direction.LeftToRight)
            .SetSizeMode(StackContainer.SizeMode.Dynamic)
            .SetSize(new Vector2(175f, 100f))
            .SetItemSpacing(3f);

        var productButton2 = Builder
            .NewBtnPrimary("PHL Usage Button")
            .SetButtonStyle(Builder.Style.Global.ListMenuBtn)
            .SetText("Hello button 2")
            .SetTextAlignment(TextAnchor.MiddleLeft)
            .SetSize(new Vector2(175f, 30f));
        productButton2.AppendTo(productButtonContainer);

        updaterBuilder.Observe<bool>((Func<bool>)(() => _inspector.toolActive))
    .Do((Action<bool>)(ts =>
    {
        productButton2.SetText($"tool is {ts}");
    }));

        var productButton3 = Builder
           .NewBtnPrimary("PHL Usage Button")
            .SetButtonStyle(Builder.Style.Global.ListMenuBtn)
            .SetText("Hello button 3")
            .SetTextAlignment(TextAnchor.MiddleLeft)
            .SetSize(new Vector2(175f, 30f));
        productButton3.AppendTo(productButtonContainer);

        updaterBuilder.Observe<EntityId>((Func<EntityId>)(() => lastClickedEntity))
        .Do((Action<EntityId>)(id =>
        {
            if (_entitiesManager.TryGetEntity(id, out Mafi.Core.Entities.Static.Layout.LayoutEntity entityOut))
            {
                Type entityType = entityOut.GetType();

                string iconPath = entityOut.Prototype.IconPath;
                Vector2? nullable = new Vector2?(64.Vector2());
                ColorRgba? color = new ColorRgba?();
                Vector2? size = nullable;
                IconStyle iconStyle = new IconStyle(iconPath, color, size);
                productButton3.SetIcon(iconStyle);
                productButton3.SetText("");
                if (entityType == typeof(Storage))
                {
                    var storage = entityOut as Storage;
                    if (storage.StoredProduct != Option<ProductProto>.None)
                    {
                        productButton3?.SetText(storage.StoredProduct.Value.Id.ToString());
                    }
                }
            }
        }));

        productButtonContainer.AppendTo(itemContainer);

        var inputGridContainer = Builder
            .NewGridContainer("PHL Top")
            .SetDynamicHeightMode(3)
            .SetCellSize(new Vector2(200, 50))
            .SetCellSpacing(5f);

        for (int i = 0; i < 5 ; i++)
        {
            var pButton = Builder
                .NewBtnPrimary("PHL Usage Button")
                .SetButtonStyle(Builder.Style.Global.ListMenuBtn)
                .SetText($"Hello button {i}")
                .SetTextAlignment(TextAnchor.MiddleLeft);
            pButton.AppendTo(inputGridContainer);
        }


        var productButton4 = Builder
            .NewBtnPrimary("PHL Usage Button")
            .SetButtonStyle(Builder.Style.Global.ListMenuBtn)
            .SetText("Hello button 4")
            .SetTextAlignment(TextAnchor.MiddleLeft);
        productButton4.AppendTo(inputGridContainer);

        var productButton5 = Builder
            .NewBtnPrimary("PHL Usage Button")
            .SetButtonStyle(Builder.Style.Global.ListMenuBtn)
            .SetText("Hello button 5")
            .SetTextAlignment(TextAnchor.MiddleLeft);
        productButton5.AppendTo(inputGridContainer);

        var productButton6 = Builder
            .NewBtnPrimary("PHL Usage Button")
            .SetButtonStyle(Builder.Style.Global.ListMenuBtn)
            .SetText("Hello button 6")
            .SetTextAlignment(TextAnchor.MiddleLeft);
        productButton6.AppendTo(inputGridContainer);

        var productButton7 = Builder
            .NewBtnPrimary("PHL Usage Button")
            .SetButtonStyle(Builder.Style.Global.ListMenuBtn)
            .SetText("Hello button 8")
            .SetTextAlignment(TextAnchor.MiddleLeft);

        productButton7.AppendTo(inputGridContainer);

        inputGridContainer.AppendTo(itemContainer);
        this.AddUpdater(updaterBuilder.Build());



    }





}
