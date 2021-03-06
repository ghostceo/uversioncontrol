﻿// Copyright (c) <2017> <Playdead>
// This file is subject to the MIT License as seen in the trunk of this repository
// Maintained by: <Kristian Kjems> <kristian.kjems+UnityVC@gmail.com>
using System.Collections.Generic;
using UnityEngine;

namespace VersionControl.UserInterface
{
    using Logging;
    public static class IconUtils
    {
        // Const
        public const int borderSize = 1;
        public static readonly RubyIcon rubyIcon = new RubyIcon();
        public static readonly ChildIcon childIcon = new ChildIcon();
        public static readonly CircleIcon circleIcon = new CircleIcon();
        public static readonly SquareIcon squareIcon = new SquareIcon();
        public static readonly TriangleIcon triangleIcon = new TriangleIcon();
        public static readonly BoxIcon boxIcon = new BoxIcon();

        public abstract class Icon
        {
            private static readonly Dictionary<int, Texture2D> iconDatabase = new Dictionary<int, Texture2D>();
            public Texture2D GetTexture(Color color)
            {
                int hashCode = color.GetHashCode() ^ GetType().GetHashCode();
                Texture2D texture;
                if (!iconDatabase.TryGetValue(hashCode, out texture))
                {
                    texture = LoadTexture(color);
                    iconDatabase.Add(hashCode, texture);
                }
                return texture;
            }
            public abstract int Size { get; }
            protected abstract Texture2D LoadTexture(Color color);
        }
        public class RubyIcon : Icon
        {
            protected override Texture2D LoadTexture(Color color)
            {
                return LoadTextureFromFile(ruby, Size, color);
            }
            public override int Size { get { return 16; } }
        }
        public class ChildIcon : Icon
        {
            protected override Texture2D LoadTexture(Color color)
            {
                return LoadTextureFromFile(child, Size, color);
            }
            public override int Size { get { return 20; } }
        }
        public class CircleIcon : Icon
        {
            protected override Texture2D LoadTexture(Color color)
            {
                return LoadTextureFromFile(circle, Size, color);
            }
            public override int Size { get { return 16; } }
        }
        public class SquareIcon : Icon
        {
            protected override Texture2D LoadTexture(Color color)
            {
                return LoadTextureFromFile(square, Size, color);
            }
            public override int Size { get { return 16; } }
        }
        public class TriangleIcon : Icon
        {
            protected override Texture2D LoadTexture(Color color)
            {
                return LoadTextureFromFile(triangle, Size, color);
            }
            public override int Size { get { return 12; } }
        }
        public class BoxIcon : Icon
        {
            protected override Texture2D LoadTexture(Color color)
            {
                return CreateSquareTextureWithBorder(Size, borderSize, color, Color.black);
            }
            public override int Size { get { return 12; } }
        }

        private static Texture2D LoadTextureFromFile(byte[] bytes, int size, Color color)
        {
            var texture = new Texture2D(size, size, TextureFormat.ARGB32, false, true) { hideFlags = HideFlags.HideAndDontSave };
            texture.LoadRawTextureData(bytes);
            for (int x = 0; x < size; ++x)
            {
                for (int y = 0; y < size; ++y)
                {
                    var resourceColor = texture.GetPixel(x, y);
                    bool resourcePixelIsWhite = resourceColor.r == 1 && resourceColor.g == 1 && resourceColor.b == 1;
                    var newColor = resourcePixelIsWhite
                                       ? new Color(color.r, color.g, color.b, Mathf.Min(resourceColor.a, color.a))
                                       : resourceColor;
                    texture.SetPixel(x, y, newColor);
                }
            }

            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Point;
            texture.Apply();
            return texture;
        }


        public static Texture2D CreateBorderedTexture(Color border, Color body)
        {
            var backgroundTexture = new Texture2D(3, 3, TextureFormat.ARGB32, false, true) { hideFlags = HideFlags.HideAndDontSave };

            backgroundTexture.SetPixels(new[]
            {
                border, border, border,
                border, body, border,
                border, border, border,
            });
            backgroundTexture.wrapMode = TextureWrapMode.Clamp;
            backgroundTexture.filterMode = FilterMode.Point;
            backgroundTexture.Apply();
            return backgroundTexture;
        }

        public static Texture2D CreateSquareTexture(int size, int borderSize, Color color)
        {
            return CreateSquareTextureWithBorder(size, borderSize, color, color);
        }

        public static Texture2D CreateSquareTextureWithBorder(int size, int borderSize, Color inner, Color border)
        {
            var colors = new Color[size * size];
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    bool onborder = (x < borderSize || x >= size - borderSize || y < borderSize || y >= size - borderSize);
                    colors[x + y * size] = onborder ? border : inner;
                }
            }

            var iconTexture = new Texture2D(size, size, TextureFormat.ARGB32, false, true) { hideFlags = HideFlags.HideAndDontSave };
            iconTexture.SetPixels(colors);
            iconTexture.wrapMode = TextureWrapMode.Clamp;
            iconTexture.filterMode = FilterMode.Point;
            iconTexture.Apply();
            return iconTexture;
        }
        //string base64 = System.Convert.ToBase64String(texture.GetRawTextureData());
        static byte[] child     = System.Convert.FromBase64String("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA////AP///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD///8A////AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP///wD///8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA////AP///wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP///////////wAAAP8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA////////////AAAA/wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD///////////8AAAD/AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP///////////wAAAP8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA////////////AAAA/wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD///////////8AAAD/AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP///////////wAAAP8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA////////////AAAA/wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD///////////8AAAD/AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP///////////wAAAP8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA////////////AAAA/wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD///////////8AAAD/AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP///////////wAAAP8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA////////////AAAA/wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD///////////8AAAD/AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP///////////wAAAP8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA==");
        static byte[] circle    = System.Convert.FromBase64String("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA/wAAAP8AAAD/AAAA/wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////////////////8AAAD/AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA/////////////////////////////////wAAAP8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAJAAAA////////////////////////////////////////////AAAA/wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP///////////////////////////////////////////wAAAP8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD///////////////////////////////////////////8AAAD/AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA////////////////////////////////////////////AAAA/wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEQAAAAAAAAD/////////////////////////////////AAAA/wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//////////////////////AAAA/wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA/wAAAP8AAAD/AAAA/wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAADAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA==");
        static byte[] ruby      = System.Convert.FromBase64String("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD/AAAA/wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD///////////8AAAD/AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////////////////wAAAP8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD/////////////////////////////////AAAA/wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD///////////////////////////////////////////8AAAD/AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//////////////////////////////////////////////////////wAAAP8AAAAAAAAAAAAAAAAAAAAAAAAA//////////////////////////////////////////////////////8AAAD/AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD///////////////////////////////////////////8AAAD/AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP////////////////////////////////8AAAD/AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////////////////8AAAD/AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD///////////8AAAD/AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP8AAAD/AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA==");
        static byte[] square    = System.Convert.FromBase64String("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACzAAAAswAAALMAAACzAAAAswAAALMAAACzAAAAswAAALMAAACzAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAs///////////////////////////////////////////AAAAswAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAALP//////////////////////////////////////////wAAALMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACz//////////////////////////////////////////8AAACzAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAs///////////////////////////////////////////AAAAswAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAALP//////////////////////////////////////////wAAALMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACz//////////////////////////////////////////8AAACzAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAs///////////////////////////////////////////AAAAswAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAALP//////////////////////////////////////////wAAALMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACzAAAAswAAALMAAACzAAAAswAAALMAAACzAAAAswAAALMAAACzAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA==");
        static byte[] triangle  = System.Convert.FromBase64String("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAA/wAAAP8AAAD/AAAAv/////////////////////////////////////////////////////8AAAC/AAAAAAAAAP///////////////////////////////////////////wAAAP8AAAAAAAAAAAAAAL///////////////////////////////////////////wAAAL8AAAAAAAAAAAAAAAAAAAD/////////////////////////////////AAAA/wAAAAAAAAAAAAAAAAAAAAAAAAC/////////////////////////////////AAAAvwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA//////////////////////8AAAD/AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAv/////////////////////8AAAC/AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP///////////wAAAP8AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD/AAAA/wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAC/AAAAvwAAAAAAAAAAAAAAAAAAAAAAAAAA");
    }

}
