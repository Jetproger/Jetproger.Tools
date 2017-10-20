using System.Drawing;
using Res = Tools.Resource;

namespace Jetproger.Tools.Resource.Bases
{
    public static class ResourcePictureExtesions
    {
        public static Image AskExit(this IResourcePictures resourcePictures) { return Res.GetResourceImage("AskExit").Value; }
        public static Image Cancel(this IResourcePictures resourcePictures) { return Res.GetResourceImage("Cancel").Value; }
        public static Image Caption(this IResourcePictures resourcePictures) { return Res.GetResourceImage("Caption").Value; }
        public static Image InfoTrace(this IResourcePictures resourcePictures) { return Res.GetResourceImage("InfoTrace").Value; }
        public static Image Login(this IResourcePictures resourcePictures) { return Res.GetResourceImage("Login").Value; }
        public static Image Ok(this IResourcePictures resourcePictures) { return Res.GetResourceImage("Ok").Value; }
        public static Image Password(this IResourcePictures resourcePictures) { return Res.GetResourceImage("Password").Value; }
        public static Image Add(this IResourcePictures resourcePictures) { return Res.GetResourceImage("Add").Value; }
        public static Image Copy(this IResourcePictures resourcePictures) { return Res.GetResourceImage("Copy").Value; }
        public static Image Edit(this IResourcePictures resourcePictures) { return Res.GetResourceImage("Edit").Value; }
        public static Image Refresh(this IResourcePictures resourcePictures) { return Res.GetResourceImage("Refresh").Value; }
        public static Image Remove(this IResourcePictures resourcePictures) { return Res.GetResourceImage("Remove").Value; }
        public static Image Restore(this IResourcePictures resourcePictures) { return Res.GetResourceImage("Restore").Value; }
        public static Image Folder(this IResourcePictures resourcePictures) { return Res.GetResourceImage("Folder").Value; }
        public static Image FolderOpen(this IResourcePictures resourcePictures) { return Res.GetResourceImage("FolderOpen").Value; }
    }
}