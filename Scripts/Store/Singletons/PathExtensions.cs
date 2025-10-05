using Godot;
using System;
using System.Collections.Generic;

public static class PathExtensions
{
    public static readonly Vector2I ENTRANCE_POS = new Vector2I(19, 3);
    public static readonly Vector2I QUEUE_START_POS = new Vector2I(13, 3);
    public static readonly Vector2I TOILET_POS = new Vector2I(9, 1);

    public static void Init()
    {
        // Wow I sure do love hardcoding & statics this LD huh
        int[,] humanReadable = new int[,]
            {
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
                { 1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1 },
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1 },
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1 },
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1 },
                { 0,0,1,1,0,0,0,0,1,1,0,0,0,1,1,1,1,1,1,1 },
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1 },
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1 },
                { 0,0,1,1,0,0,0,0,1,1,0,0,0,1,1,1,1,1,1,1 },
                { 0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1 },
            };
        int[,] res = new int[20, 11];
        for (int x = 0; x < 20; x++)
        {
            for (int y = 0; y < 11; y++)
            {
                res[x, y] = humanReadable[y, x];
            }
        }
        Pathfinder.SetMap(
            res,
            new Vector2I(20, 11)
        );
    }

    public static void InterpolateMoveOnPath(this Interpolator interpolator, Node2D toMove, float moveSpeed, Vector2 startPos, Vector2 targetPos)
    {
        Vector2I start = startPos.ToTile();
        Vector2I end = targetPos.ToTile();
        List<Vector2> steps;
        if (start != end)
        {
            steps = Pathfinder.GetPath(start, end).ConvertAll(a => a.ToPos());
            steps[0] = startPos;
            steps[steps.Count - 1] = targetPos;
        }
        else
        {
            if (startPos.DistanceTo(targetPos) >= 0.01f)
            {
                steps = new List<Vector2>() { startPos, targetPos };
            }
            else
            {
                GD.PushWarning("[PathExtensions]: Empty path!");
                return;
            }
        }
        float totalDist = steps.Sum((a, i) => steps[i].DistanceTo(steps[Mathf.Max(0, i - 1)]));
        moveSpeed *= 32;
        interpolator.Interpolate(totalDist / moveSpeed,
            new Interpolator.InterpolateObject(
                a =>
                {
                    int targetStep = 1;
                    while (targetStep < steps.Count - 1 && a > steps[targetStep - 1].DistanceTo(steps[targetStep]))
                    {
                        a -= steps[targetStep - 1].DistanceTo(steps[targetStep]);
                        targetStep++;
                    }
                    a /= steps[targetStep - 1].DistanceTo(steps[targetStep]);
                    toMove.Position = (1 - a) * steps[targetStep - 1] + a * steps[targetStep];
                    if (toMove.Position.DistanceTo(steps[targetStep]) > 0.01f)
                    {
                        toMove.LookAt(steps[targetStep]);
                        toMove.RotationDegrees += 180;
                    }
                },
                0,
                totalDist
            ));
    }
}
