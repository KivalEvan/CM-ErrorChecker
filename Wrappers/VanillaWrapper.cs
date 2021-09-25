﻿using System;
using Jint;

abstract class VanillaWrapper<T> : Wrapper<T> where T : BeatmapObject
{
    private Lazy<JSONWraper> customData;
    private Action reconcile;

    public object _customData
    {
        get => wrapped.CustomData == null ? null : customData.Value;
        set
        {
            DeleteObject();
            wrapped.CustomData = JSONWraper.castObjToJSON(value);
            InitWrapper();
        }
    }

    protected VanillaWrapper(Engine engine, T wrapped, bool hasOriginal = true, bool? selected = null) : base(engine, wrapped, hasOriginal, selected)
    {
        InitWrapper();
    }

    private void InitWrapper()
    {
        reconcile = null;
        customData = new Lazy<JSONWraper>(() =>
            new JSONWraper(engine, ref reconcile, wrapped.CustomData, DeleteObject)
        );
    }

    internal override void Reconcile()
    {
        reconcile?.Invoke();

        if (wrapped.CustomData != null && wrapped.CustomData.Count == 0)
        {
            wrapped.CustomData = null;
        }
    }
}
