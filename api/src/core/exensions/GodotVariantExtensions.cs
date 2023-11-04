using System;
using System.Collections.Generic;
using Godot;

namespace GdUnit4
{
    /// <summary>
    /// A extension box/unbox Godot Variants
    /// </summary>
    public static class GodotVariantExtensions
    {
        public static dynamic? UnboxVariant<T>(this T? value)
        {
            if (value is Variant v)
                return v.UnboxVariant();
            if (value is StringName sn)
                return sn.ToString();
            if (value is Godot.Collections.Dictionary gd)
                return gd.UnboxVariant();
            if (value is Godot.Collections.Array ga)
                return ga.UnboxVariant();
            if (value is Variant[] parameters)
                return parameters.UnboxVariant();
            return value;
        }

        private static IDictionary<object, object?> UnboxVariant(this Godot.Collections.Dictionary dict)
        {
            var unboxed = new Dictionary<object, object?>();
            foreach (KeyValuePair<Variant, Variant> kvp in dict)
                unboxed.Add(kvp.Key.UnboxVariant()!, kvp.Value.UnboxVariant());
            return unboxed;
        }

        private static ICollection<object?> UnboxVariant(this Godot.Collections.Array values)
        {
            var unboxed = new List<object?>();
            foreach (Variant value in values)
                unboxed.Add(value.UnboxVariant());
            return unboxed;
        }

        private static ICollection<object?> UnboxVariant(this Variant[] values)
        {
            var unboxed = new List<object?>();
            foreach (Variant value in values)
                unboxed.Add(value.UnboxVariant());
            return unboxed;
        }

        internal static Variant ToVariant(this Object? obj) =>
            Type.GetTypeCode(obj?.GetType()) switch
            {
                TypeCode.Empty => new Variant(),
                TypeCode.String => Variant.CreateFrom((String)obj!),
                TypeCode.Boolean => Variant.CreateFrom((Boolean)obj!),
                TypeCode.Char => Variant.CreateFrom(0 + (Char)obj!),
                TypeCode.SByte => Variant.CreateFrom((SByte)obj!),
                TypeCode.Byte => Variant.CreateFrom((Byte)obj!),
                TypeCode.Int16 => Variant.CreateFrom((Int16)obj!),
                TypeCode.UInt16 => Variant.CreateFrom((UInt16)obj!),
                TypeCode.Int32 => Variant.CreateFrom((Int32)obj!),
                TypeCode.UInt32 => Variant.CreateFrom((UInt32)obj!),
                TypeCode.Int64 => Variant.CreateFrom((Int64)obj!),
                TypeCode.UInt64 => Variant.CreateFrom((UInt64)obj!),
                TypeCode.Single => Variant.CreateFrom((Single)obj!),
                TypeCode.Double => Variant.CreateFrom((Double)obj!),
                TypeCode.Decimal => Variant.CreateFrom((UInt64)obj!),
                _ => ToVariantByType(obj!)
            };

        private static Variant ToVariantByType(Object obj)
        {
            if (obj is System.Collections.IList list)
                return list.ToGodotArray();

            if (obj is IDictionary<string, object> dict)
                return dict.ToGodotDictionary();

            throw new NotImplementedException($"Cannot convert '{obj?.GetType()}' to Variant!");
        }

        private static dynamic? UnboxVariant(this Variant v) => v.VariantType switch
        {
            Variant.Type.Nil => null,
            Variant.Type.Bool => v.AsBool(),
            Variant.Type.Int => v.AsInt64(),
            Variant.Type.Float => v.AsDouble(),
            Variant.Type.String => v.AsString(),
            Variant.Type.Vector2 => v.AsVector2(),
            Variant.Type.Vector2I => v.AsVector2I(),
            Variant.Type.Rect2 => v.AsRect2(),
            Variant.Type.Rect2I => v.AsRect2I(),
            Variant.Type.Vector3 => v.AsVector3(),
            Variant.Type.Vector3I => v.AsVector3I(),
            Variant.Type.Transform2D => v.AsTransform2D(),
            Variant.Type.Vector4 => v.AsVector4(),
            Variant.Type.Vector4I => v.AsVector4I(),
            Variant.Type.Plane => v.AsPlane(),
            Variant.Type.Quaternion => v.AsQuaternion(),
            Variant.Type.Aabb => v.AsAabb(),
            Variant.Type.Basis => v.AsBasis(),
            Variant.Type.Transform3D => v.AsTransform3D(),
            Variant.Type.Projection => v.AsProjection(),
            Variant.Type.Color => v.AsColor(),
            Variant.Type.StringName => v.AsStringName(),
            Variant.Type.NodePath => v.AsNodePath(),
            Variant.Type.Rid => v.AsRid(),
            Variant.Type.Object => v.AsGodotObject(),
            Variant.Type.Callable => v.AsCallable(),
            Variant.Type.Signal => v.AsSignal(),
            Variant.Type.Dictionary => v.AsGodotDictionary(),
            Variant.Type.Array => v.AsGodotArray(),
            Variant.Type.PackedByteArray => v.AsByteArray(),
            Variant.Type.PackedInt32Array => v.AsInt32Array(),
            Variant.Type.PackedInt64Array => v.AsInt64Array(),
            Variant.Type.PackedFloat32Array => v.AsFloat32Array(),
            Variant.Type.PackedFloat64Array => v.AsFloat64Array(),
            Variant.Type.PackedStringArray => v.AsStringArray(),
            Variant.Type.PackedVector2Array => v.AsVector2Array(),
            Variant.Type.PackedVector3Array => v.AsVector3Array(),
            Variant.Type.PackedColorArray => v.AsColorArray(),
            _ => throw new ArgumentOutOfRangeException(nameof(v))
        };
    }
}