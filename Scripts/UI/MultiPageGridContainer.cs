using Godot;
using System;
using System.Collections.Generic;

public partial class MultiPageGridContainer : Control
{
    [ExportCategory("Nodes")]
    [Export] private GridContainer baseGrid;
    [Export] private Container gridsContainer;
    [Export] private BaseButton nextButton;
    [Export] private BaseButton prevButton;

    [ExportCategory("Vars")]
    [Export] private int maxItemsPerPage = 8;
    [Export] private float movePageTime = 0.2f;

    private Interpolator interpolator { get; } = new Interpolator();

    private List<Control> items { get; } = new List<Control>();
    private List<GridContainer> grids { get; } = new List<GridContainer>();
    private int currentGrid = 0;

    private int lastGridIndex => grids.Count - 1;

    public override void _Ready()
    {
        base._Ready();
        AddChild(interpolator);
        interpolator.InterruptMode = Interpolator.Mode.Error;
        CreateGrid().Modulate = Colors.White;
    }

    private GridContainer CreateGrid()
    {
        GridContainer grid = (GridContainer)baseGrid.Duplicate();
        grid.Visible = true;
        grid.Modulate = Colors.Transparent;
        grids.Add(grid);
        gridsContainer.AddChild(grid);
        return grid;
    }

    private void RemoveLastGrid()
    {
        if (currentGrid == lastGridIndex)
        {
            TotalMouseBlock.Block();
            currentGrid--;
            interpolator.Interpolate(movePageTime,
                Interpolator.InterpolateObject.ModulateFadeInterpolate(
                    grids[currentGrid],
                    Colors.White,
                    Easing.EaseInOutSin
                ));
            interpolator.OnFinish = () =>
            {
                grids[lastGridIndex].QueueFree();
                grids.RemoveAt(lastGridIndex);
                EnableDisableNextPrevButtons();
                TotalMouseBlock.Unblock();
            };
        }
        else
        {
            grids[lastGridIndex].QueueFree();
            grids.RemoveAt(lastGridIndex);
            EnableDisableNextPrevButtons();
        }
    }

    private void NextGrid() => MoveGrid(currentGrid + 1);

    private void PrevGrid() => MoveGrid(currentGrid - 1);

    private void MoveGrid(int target)
    {
        TotalMouseBlock.Block();
        interpolator.Interpolate(movePageTime,
            Interpolator.InterpolateObject.ModulateFadeInterpolate(
                grids[currentGrid],
                Colors.Transparent,
                Easing.EaseInOutSin
            ));
        interpolator.OnFinish = () =>
        {
            currentGrid = target;
            interpolator.Interpolate(movePageTime,
                Interpolator.InterpolateObject.ModulateFadeInterpolate(
                    grids[currentGrid],
                    Colors.White,
                    Easing.EaseInOutSin
                ));
            interpolator.OnFinish = TotalMouseBlock.Unblock;
        };
    }

    public void AddItem(Control item)
    {
        items.Add(item);
        if (items.Count <= grids.Count * maxItemsPerPage)
        {
            grids[lastGridIndex].AddChild(item);
        }
        else
        {
            CreateGrid().AddChild(item);
            EnableDisableNextPrevButtons();
        }
    }

    public void RemoveItem(Control item)
    {
        if (items.Contains(item))
        {
            int containerIndex = grids.FindIndex(a => a.GetChildren().Contains(item));
            if (containerIndex >= 0)
            {
                item.QueueFree();
                for (int i = containerIndex + 1; i < lastGridIndex; i++)
                {
                    if (grids[i].GetChildren().Count > 0)
                    {
                        grids[i].GetChildren()[0].Reparent(grids[i - 1]);
                    }
                    else
                    {
                        GD.PushError("[MultiPageGridContainer]: Empty grid!");
                    }
                }
                if (grids[lastGridIndex].GetChildren().Count <= 0)
                {
                    RemoveLastGrid();
                }
            }
            else
            {
                GD.PushError("[MultiPageGridContainer]: Removing a corrupted item! " + item + " in " + ToString());
            }
        }
        else
        {
            GD.PushError("[MultiPageGridContainer]: Removing a nonexistent item! " + item + " in " + ToString());
        }
    }

    public void EnableDisableNextPrevButtons()
    {
        if (grids.Count <= 1)
        {
            nextButton.Visible = prevButton.Visible = false;
        }
        else
        {
            nextButton.Visible = prevButton.Visible = true;
            if (currentGrid >= lastGridIndex) nextButton.Visible = false;
            if (currentGrid <= 0) prevButton.Visible = false;
        }
    }
}
