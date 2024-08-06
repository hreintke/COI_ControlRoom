using Mafi.Collections.ReadonlyCollections;
using Mafi.Collections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities;
using Mafi.Core.Factory.Transports;
using Mafi.Core.GameLoop;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Localization;
using Mafi.Unity.Entities;
using Mafi.Unity.InputControl.AreaTool;
using Mafi.Unity.InputControl.Cursors;
using Mafi.Unity.InputControl.Factory;
using Mafi.Unity.InputControl.Toolbar;
using Mafi.Unity.InputControl;
using Mafi.Unity.UiFramework.Styles;
using Mafi.Unity.UserInterface;
using Mafi.Unity;
using Mafi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Mafi.Collections.ImmutableCollections;

namespace ControlRoomMod;

[GlobalDependency(RegistrationMode.AsEverything)]
public class EntitySelectionController : Mafi.Unity.InputControl.Tools.BaseEntityCursorInputController<IStaticEntity>
{
    private readonly EntitiesIconRenderer _iconRenderer;
    private readonly ToolbarController _toolbarController;
    private readonly IEntitiesManager _entitiesManager;
    private readonly IUnityInputMgr _inputMgr;
    private CursorStyle _cursorStyle;
    private UiBuilder _uiBuilder;

    public EntitySelectionController(
        ProtosDb protosDb,
        UnlockedProtosDbForUi unlockedProtosDb,
        ShortcutsManager shortcutsManager,
        IUnityInputMgr inputManager,
        CursorPickingManager cursorPickingManager,
        CursorManager cursorManager,
        AreaSelectionToolFactory areaSelectionToolFactory,
        IEntitiesManager entitiesManager,
        NewInstanceOf<EntityHighlighter> highlighter,
        ToolbarController toolbarController,
        IGameLoopEvents gameLoopEvents,
        EntitiesIconRenderer iconRenderer,
        UiBuilder builder

    ) : base(protosDb,
              builder,
              unlockedProtosDb,
              shortcutsManager,
              inputManager,
              cursorPickingManager,
              cursorManager,
              areaSelectionToolFactory,
              entitiesManager,
              highlighter,
              (Option<NewInstanceOf<TransportTrajectoryHighlighter>>)Option.None,
              null)

    {
        _toolbarController = toolbarController;
        _entitiesManager = entitiesManager;
        _iconRenderer = iconRenderer;
        _uiBuilder = builder;
        _inputMgr = inputManager;
        //            gameLoopEvents.RegisterRendererInitState(this, InitState);
    }

    protected override bool OnFirstActivated(IStaticEntity hoveredEntity, Lyst<IStaticEntity> selectedEntities, Lyst<SubTransport> selectedPartialTransports)
    {
        LogWrite.Info("EntitySelection onFirstActivated");
        return false;
    }

    public void ActivateForSelection()
    {
        _inputMgr.ActivateNewController((IUnityInputController)this);
    }

    protected override void OnHoverChanged(IIndexable<IStaticEntity> hoveredEntities, IIndexable<SubTransport> hoveredPartialTransports, bool isLeftClick)
    {
        if (hoveredEntities.Count == 0) return;
        LogWrite.Info($"On Hover Changed {hoveredEntities.Count}");
        int i = 0;
        foreach (var e in hoveredEntities)
        {
            LogWrite.Info($"{i++} {e.Prototype.Id.Value.ToString()} : {e.GetType()}");
        }
    }

    protected override bool Matches(IStaticEntity entity, bool isAreaSelection, bool isLeftClick)
    {
        if (entity.IsDestroyed)
            return false;
        if (entity is IStaticEntity staticEntity && !staticEntity.IsConstructed)
            return false;

        return true;
    }

    protected override void RegisterToolbar(ToolbarController controller)
    {
        LogWrite.Info("Register UI");
        _toolbarController
            .AddLeftMenuButton("EntitySelection", this, "Assets/Unity/UserInterface/EntityIcons/Storage.svg", 1339f, manager => KeyBindings.FromKey(KbCategory.Tools, ShortcutMode.Game, KeyCode.F10))
            .AddTooltip(new LocStrFormatted("EntitySelection tooltip"));

        _cursorStyle = new CursorStyle("EntitySelectionStyle", "Assets/Unity/UserInterface/EntityIcons/Storage.svg", new Vector2(14f, 14f));
        InitializeUi(_cursorStyle, _uiBuilder.Audio.Assign, ColorRgba.White, ColorRgba.Green);
        LogWrite.Info("Register done UI");
        LogWrite.Info("Register Base done UI");

    }

    protected override void OnEntitiesSelected(IIndexable<IStaticEntity> selectedEntities, IIndexable<SubTransport> selectedPartialTransports, bool isAreaSelection, bool isLeftMouse, RectangleTerrainArea2i? area)
    {
        if (selectedEntities.Count == 0) return;
        LogWrite.Info($"On entities selected {selectedEntities.Count}");
        int i = 0;
        foreach (var e in selectedEntities)
        {
            LogWrite.Info($"{i++} {e.Prototype.Id.Value.ToString()} : {e.GetType()}");
        }
//        _inputMgr.DeactivateController((IUnityInputController)this);
    }
}
