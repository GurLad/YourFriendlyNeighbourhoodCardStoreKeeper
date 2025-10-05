using Godot;
using System;
using System.Collections.Generic;

public static class PathExtensions
{
    public static readonly Vector2I ENTRANCE_POS = new Vector2I(19, 3);
    public static readonly Vector2I QUEUE_END_POS = new Vector2I(13, 3);
    public static readonly Vector2I TOILET_POS = new Vector2I(9, 1);

    public static void Init()
    {
        // Wow I sure do love hardcoding & statics this LD huh
        Pathfinder.SetMap(
            new int[,]
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
            },
            new Vector2I(20, 11)
        );
    }

    public static void InterpolateMoveOnPath(this Interpolator interpolator, Node2D toMove, float moveSpeed, Vector2 startPos, Vector2 targetPos)
    {
        Vector2I start = startPos.ToV2I();
        Vector2I end = targetPos.ToV2I();
        List<Vector2> steps = Pathfinder.GetPath(start, end).ConvertAll(a => a.ToV2());
        steps[0] = startPos;
        steps[steps.Count - 1] = targetPos;
        float totalDist = steps.Sum((a, i) => steps[i].DistanceTo(steps[Mathf.Max(0, i - 1)]));
        interpolator.Interpolate(moveSpeed / totalDist,
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
                },
                0,
                totalDist
            ));
    }
}
