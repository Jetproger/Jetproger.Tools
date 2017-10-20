using Res = Tools.Resource;

namespace Jetproger.Tools.Resource.Bases
{
    public static class ResourceShortcutExtesions
    {
        public static string Cancel(this IResourceShortcuts resourceShortcuts) { return Res.GetResourceShortcut("Cancel").Value; }
        public static string Ok(this IResourceShortcuts resourceShortcuts) { return Res.GetResourceShortcut("Ok").Value; }
        public static string Password(this IResourceShortcuts resourceShortcuts) { return Res.GetResourceShortcut("Password").Value; }
        public static string Add(this IResourceShortcuts resourceShortcuts) { return Res.GetResourceShortcut("Add").Value; }
        public static string Copy(this IResourceShortcuts resourceShortcuts) { return Res.GetResourceShortcut("Copy").Value; }
        public static string Edit(this IResourceShortcuts resourceShortcuts) { return Res.GetResourceShortcut("Edit").Value; }
        public static string Refresh(this IResourceShortcuts resourceShortcuts) { return Res.GetResourceShortcut("Refresh").Value; }
        public static string Remove(this IResourceShortcuts resourceShortcuts) { return Res.GetResourceShortcut("Remove").Value; }
        public static string Restore(this IResourceShortcuts resourceShortcuts) { return Res.GetResourceShortcut("Restore").Value; }
        public static string FolderOpen(this IResourceShortcuts resourceShortcuts) { return Res.GetResourceShortcut("FolderOpen").Value; }
    }
}