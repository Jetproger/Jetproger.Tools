using System;
using System.Drawing;
using Jetproger.Tools.Convert.Converts;
using Jetproger.Tools.Convert.Factories;

namespace Jetproger.Tools.Convert.Bases
{
    public static class GuiExtensions
    {
        public static Image DefaultImage(this f.IGuiExpander exp)
        {
            return t<Image>.one(() => f.sys.defof<Image>());
        }

        public static Icon DefaultIcon(this f.IGuiExpander exp)
        {
            return t<Icon>.one(() => f.sys.defof<Icon>());
        }
    }
}