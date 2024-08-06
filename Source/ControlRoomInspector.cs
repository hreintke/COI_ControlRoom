using Mafi.Unity.InputControl.Inspectors;
using Mafi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControllerMod;
using Mafi.Unity.InputControl;
using Mafi.Unity.InputControl.Cursors;
using Mafi.Core.GameLoop;
using Mafi.Unity.UiToolkit.Library;
using Mafi.Core;
using Mafi.Core.Input;
using Mafi.Core.Entities.Static;
using Mafi.Unity;
using Mafi.Core.Entities;
using UnityEngine;
using Mafi.Base;
using Mafi.Unity.UserInterface.Style;
using Mafi.Unity.Entities;

namespace ControlRoomMod;

[GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
public class ControlRoomInspector : EntityInspector<ControlRoom, ControlRoomView>
{
    private readonly ControlRoomView _windowView;
    public bool toolActive = false;
    private readonly ShortcutsManager _shortcutsManager;
    private readonly CursorPickingManager _cursorPickingManager;
    private readonly InspectorController _inspectorController;
    private readonly TerrainCursor _terrainCursor;
    private readonly Cursoor _assignCursor;
    private LinesFactory _linesFactory;
    private readonly LineMb _linePreview;
    private readonly Material _movingArrowsLineMaterialShared;
    private Option<Mafi.Core.Entities.Static.Layout.LayoutEntity> hoveredEntity;
    private readonly EntityHighlighter _entityHighlighter;

    public ControlRoomInspector(
        InspectorContext context,
        InspectorController ic,
        ShortcutsManager shortcutsManager,
        CursorPickingManager picker,
        InspectorController inspectorController,
        LinesFactory linesFactory,
        EntitiesManager em,
        TerrainCursor terrainCursor,
        CursorManager cursorManager,
        NewInstanceOf<EntityHighlighter> entityHighlighter,
        UiStyle style,
        AssetsDb assetsDb)
      : base(context)
    {
        _windowView = new ControlRoomView(this, ic, em);
        _shortcutsManager = shortcutsManager;
        _cursorPickingManager = picker;
        _inspectorController = inspectorController;
        _linesFactory = linesFactory;
        _terrainCursor = terrainCursor;
        _movingArrowsLineMaterialShared = assetsDb.GetSharedMaterial("Assets/Core/Materials/MovingArrowsLine.mat");
        _linePreview = linesFactory.CreateLine(Vector3.zero, Vector3.zero, 1.5f, Color.white, _movingArrowsLineMaterialShared);
        _linePreview.SetTextureMode(LineTextureMode.Tile);
        _linePreview.Hide();
        _assignCursor = cursorManager.RegisterCursor(style.Cursors.Assign);
        _entityHighlighter = entityHighlighter.Instance;
    }

    protected override ControlRoomView GetView() => _windowView;

    public void setToolStatus(bool newStatus)
    {
        _inspectorController.SetHoverCursorSuppression(newStatus);
        toolActive = newStatus;
        if (toolActive)
        {
            _assignCursor.Show();
            _linePreview.SetStartPoint(SelectedEntity.GetCenter().ToVector3());
            _linePreview.Show();
            _terrainCursor.Activate();
        }
        else
        {
            _linePreview.Hide();
            _assignCursor.Hide();
            _terrainCursor.Deactivate();
            clearHoveredEntityIfAny();
        }
    }

    protected override void OnActivated()
    {
        base.OnActivated();

    }

    protected override void OnDeactivated()
    {
        base.OnDeactivated();
        setToolStatus(false);

    }

    public override bool InputUpdate(IInputScheduler inputScheduler)
    {
        if (!toolActive)
        {
            return base.InputUpdate(InputScheduler);
        }
        if (_shortcutsManager.IsSecondaryActionUp)
        {
            LogWrite.Info("SecondaryActionUp");
            return true;
        }
        Vector3 vector3 = _terrainCursor.Tile3f.ToVector3();
        _linePreview.SetEndPoint(vector3);
        if (_shortcutsManager.IsPrimaryActionDown)
        {
            LogWrite.Info("PrimaryActionDown");
            ILayoutEntity pickedEntity = _cursorPickingManager.PickEntity<ILayoutEntity>().ValueOrNull;
            if (pickedEntity != null)
            {
                LogWrite.Info($"Picked entity {pickedEntity.Prototype.Id}");
                _windowView.lastClickedEntity = pickedEntity.Id;
            }
            return true;
        }
        return base.InputUpdate(InputScheduler);
    }

    public override void RenderUpdate(GameTime gameTime)
    {
        base.RenderUpdate(gameTime);
        if (!toolActive)
        {
            return;
        }
        Option<Mafi.Core.Entities.Static.Layout.LayoutEntity>  pickedEntity = _cursorPickingManager.PickEntity<Mafi.Core.Entities.Static.Layout.LayoutEntity>().ValueOrNull;
 
        if (pickedEntity.IsNone)
        {
            clearHoveredEntityIfAny();
            return;
        }
  
        if (hoveredEntity.HasValue)
        {

            if (pickedEntity.Value == hoveredEntity.Value)
            {
                return;
            }
        }
        clearHoveredEntityIfAny();
        setHoveredEntity(pickedEntity.Value);
    }

    private void clearHoveredEntityIfAny()
    {
        if (hoveredEntity.IsNone)
        {
            return;
        }
        _entityHighlighter.RemoveHighlight((IRenderedEntity)hoveredEntity.Value);
        hoveredEntity = (Option<Mafi.Core.Entities.Static.Layout.LayoutEntity>)Option.None;
    }

    private void setHoveredEntity(Mafi.Core.Entities.Static.Layout.LayoutEntity entity)
    {
        hoveredEntity = entity;
        _entityHighlighter.Highlight((IRenderedEntity)hoveredEntity.Value, ColorRgba.Yellow);
    }
}