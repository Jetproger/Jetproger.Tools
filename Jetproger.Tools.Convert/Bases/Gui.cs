using System;
using System.Drawing;
using Jetproger.Tools.Convert.Converts;
using Jetproger.Tools.Convert.Factories;

namespace Jetproger.Tools.Convert.Bases
{
    public static class GuiExtensions
    {
        public static Icon ImageAsIcon(this Je.IGuiExpander exp, Image image, int width, int height)
        {
            try
            {
                var bmp = new Bitmap(image, new Size(width, height));
                bmp.MakeTransparent(Color.Magenta);
                var hIcon = bmp.GetHicon();
                return Icon.FromHandle(hIcon);
            }
            catch
            {
                return DefaultIcon(exp);
            }
        }

        public static Image DefaultImage(this Je.IGuiExpander exp)
        {
            return Je<Image>.One(() => Je.str.To<Image>(_DefaultImage(exp)));
        }

        public static Icon DefaultIcon(this Je.IGuiExpander exp)
        {
            return Je<Icon>.One(() => Je.str.To<Icon>(_DefaultIcon(exp)));
        }

        public static string _DefaultIcon(this Je.IGuiExpander exp)
        {
            return @"AAABAAEAEBAAAAAAGABoAwAAFgAAACgAAAAQAAAAIAAAAAEAGAAAAAAAAAMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACyYzmARyl9RSh7RCd6RCd3QiZyQCVvPiRsPCNpOyJmOSFjNyBhNh9gNR8AAAAAAAC4ZjvYoIPYoIPQjmvHeE+4ZjueVzKGSit4QiZ3QiZ4QiZ3QiZbMh1hNh8AAAAAAAC/aj3oybjiuaTbqY7TlXTJfVW+aj2eVzKDSSp4QiZsPCOMTi17RCdjNyAAAAAAAADEcUXht6Hw29DiuaTZo4fQj2zHeE62ZTqSUS9jNyDCbD67aDwAAABmOSEAAAAAAADHeE/Mg13iuaTw29DiuaTYoIPOiWTDb0KSUS/Zo4fSknEAAADOiGNpOyIAAAAAAADLgVrz4djKf1fiuaPw2s/ht6HUl3egWTPhtqDjvKcAAADiuaPQjmtsPCMAAAAAAADOimb+/fzz4djFc0jiuaPlwK2HSyvbqY59RSh9RSjdrZTnxrXTlXRvPiQAAAAAAADRkW/+/fz+/fzw29DFc0jFc0ju1snz49rw29Dt1Mfrz8HpyrrWnH1yQCUAAAAAAADUmHj+/fz+/fz+/fz9+vj68/D47+rnxLKyYzmtYDfhtqDrz8HZo4Z3QiYAAAAAAADXn4H+/fz+/fz+/fz+/fz9+vj68/DDb0LbqY/FdEmtYDft1Mfcq5F6RCcAAAAAAADZo4b+/fz+/fz+/fz+/fz+/fz9+vjEcUXu1srbqY+yYznw29Dfspp7RCcAAAAAAADZo4b+/fz+/fz+/fz+/fz+/fz+/fzw29DEcUXDb0LnxLLz49rhtqB9RSgAAAAAAADZo4b+/fz+/fz+/fz+/fz+/fz+/fz+/fz9+vj68/D47+r16OHz49qARykAAAAAAADZo4bZo4bZo4bZo4bXn4HUmHjRkW/OimbLgVrHeE/EcUW/aj24ZjuyYzkAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAD//wAAgAEAAIABAACAAQAAgAEAAIABAACAAQAAgAEAAIABAACAAQAAgAEAAIABAACAAQAAgAEAAIABAAD//wAA";
        }

        public static string _DefaultImage(this Je.IGuiExpander exp)
        {
            return @"iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAIDSURBVDhPpZLrS5NhGMb3j4SWh0oRQVExD4gonkDpg4hGYKxG6WBogkMZKgPNCEVJFBGdGETEvgwyO9DJE5syZw3PIlPEE9pgBCLZ5XvdMB8Ew8gXbl54nuf63dd90OGSnwCahxbPRNPAPMw9Xpg6ZmF46kZZ0xSKzJPIrhpDWsVnpBhGkKx3nAX8Pv7z1zg8OoY/cITdn4fwbf/C0kYAN3Ma/w3gWfZL5kzTKBxjWyK2DftwI9tyMYCZKXbNHaD91bLYJrDXsYbrWfUKwJrPE9M2M1OcVzOOpHI7Jr376Hi9ogHqFIANO0/MmmmbmSmm9a8ze+I4MrNWAdjtoJgWcx+PSzg166yZZ8xM8XvXDix9c4jIqFYAjoriBV9AhEPv1mH/sonogha0afbZMMZz+yreTGyhpusHwtNNCsA5U1zS4BLxzJIfg299qO32Ir7UJtZfftyATqeT+8o2D8JSjQrAJblrncYL7ZJ2+bfaFnC/1S1NjL3diRat7qrO7wLRP3HjWsojBeComDEo5mNjuweFGvjWg2EBhCbpkW78htSHHwRyNdmgAFzPEee2iFkzayy2OLXzT4gr6UdUnlXrullsxxQ+kx0g8BTA3aZlButjSTyjODq/WcQcW/B/Je4OQhLvKQDnzN1mp0nnkvAhR8VuMzNrpm1mpjgkoVwB/v8DTgDQASA1MVpwzwAAAABJRU5ErkJggg==";
        }
    }
}