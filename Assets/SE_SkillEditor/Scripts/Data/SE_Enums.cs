using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModuleSubType
{
    Acceleration = 1,
    Attraction = 2,
    Camera = 3,
    Collision = 4,
    Color = 5,
    Event = 6,
    Kill = 7,
    Lifetime = 8,
    Location = 9,
    Material = 10,
    Orbit = 11,
    Orientation = 12,
    Parameter = 13,
    Rotation = 14,
    RotationRate = 15,
    Size = 16,
    Spawn = 17,
    StoreSpawnTime = 18,
    SubUV = 19,
    Uber = 20,
    Velocity = 21,
    WorldForces = 22,
    Homing = 23,
    Target = 24,
    Anchor = 25
}
public enum ModuleType
{
    Spawn = 0,

    OnCollision = 40,

    OnDestroy = 60,

    Lifetime = 80,

    Cone = 90,
    Circle = 91,
    Line = 92,
    AlignToVelocity = 120,

    InitialRotation = 140,

    InitialRotRate = 150,
    InitialAnchorRotRate = 151,

    InitialSize = 160,

    InitialVelocity = 210,
    VelocityScaleOverLife = 211,
    VelocityNoise = 212,

    Homing = 230,

    SelectTarget = 240,

    InitialAnchorOffset = 250,
    AnchorOffsetOverLife = 251

}
public enum EmitFrom
{
    Base,
    BaseSide,
    Volume,
    VolumeSide
}

public enum Axis
{
    NONE,
    X,
    Y,
    Z,
    NEGATIVE_X,
    NEGATIVE_Y,
    NEGATIVE_Z,
}
public enum SpaceType
{
    World = 0,
    Local = 1,
    Custom = 2
}
public enum ParentTo
{
    None,
    TriggerObject,
    TriggerObjectParent
}
public enum SpawnLocationMode
{
    Random,
    LoopByTimeRatio,
    LoopByCount,
    LoopByBrust
}
public enum ModifierType
{
    Override,
    Multiplier,
    Add,
    AddMultiplier
}