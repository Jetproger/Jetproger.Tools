using System;
using System.Resources;
using Jetproger.Tools.AppConfig;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Converts;

namespace Jetproger.Tools.Convert.Bases
{
    public abstract class ResourceSettingNames<T> : ResourceSetting<T>
    {
        private static readonly ResourceManager ResourceNamesManager = Je<ResourceManager>.One(() => Je.sys.GetResourceManager("ResourceNames", Je.Rs<ResourceAssembly>.Sz));
        protected ResourceSettingNames(T defaultValue) : base(defaultValue, "ResourceNames", Je.Rs<ResourceAssembly>.Sz) { }
        protected override ResourceManager GetResourceManager() { return ResourceNamesManager; }
    }

    public abstract class ResourceSettingDescriptions<T> : ResourceSetting<T>
    {
        private static readonly ResourceManager ResourceDescriptionsManager = Je<ResourceManager>.One(() => Je.sys.GetResourceManager("ResourceDescriptions", Je.Rs<ResourceAssembly>.Sz));
        protected ResourceSettingDescriptions(T defaultValue) : base(defaultValue, "ResourceDescriptions", Je.Rs<ResourceAssembly>.Sz) { }
        protected override ResourceManager GetResourceManager() { return ResourceDescriptionsManager; }
    }

    public abstract class ResourceSettingShortcuts<T> : ResourceSetting<T>
    {
        private static readonly ResourceManager ResourceShortcutsManager = Je<ResourceManager>.One(() => Je.sys.GetResourceManager("ResourceShortcuts", Je.Rs<ResourceAssembly>.Sz));
        protected ResourceSettingShortcuts(T defaultValue) : base(defaultValue, "ResourceShortcuts", Je.Rs<ResourceAssembly>.Sz) { }
        protected override ResourceManager GetResourceManager() { return ResourceShortcutsManager; }
    }

    public abstract class ResourceSettingSpecifies<T> : ResourceSetting<T>
    {
        private static readonly ResourceManager ResourceSpecifiesManager = Je<ResourceManager>.One(() => Je.sys.GetResourceManager("ResourceSpecifies", Je.Rs<ResourceAssembly>.Sz));
        protected ResourceSettingSpecifies(T defaultValue) : base(defaultValue, "ResourceSpecifies", Je.Rs<ResourceAssembly>.Sz) { }
        protected override ResourceManager GetResourceManager() { return ResourceSpecifiesManager; }
    }

    public abstract class ResourceSettingPictures<T> : ResourceSetting<T>
    {
        private static readonly ResourceManager ResourcePicturesManager = Je<ResourceManager>.One(() => Je.sys.GetResourceManager("ResourcePictures", Je.Rs<ResourceAssembly>.Sz));
        protected ResourceSettingPictures(T defaultValue) : base(defaultValue, "ResourcePictures", Je.Rs<ResourceAssembly>.Sz) { }
        protected override ResourceManager GetResourceManager() { return ResourcePicturesManager; }
    }

    public abstract class ResourceSetting<T> : MarshalByRefObject, ISetting
    {
        public bool IsDeclared { get; private set; }
        public T Value { get; private set; }
        private readonly string _typeName;

        protected ResourceSetting(T defaultValue, string resourceName, string assemblyName)
        {
            _typeName = GetType().Name;
            var rm = GetResourceManager();
            IsDeclared = rm != null;
            if (!IsDeclared) return;
            var stringValue = Je.sys.GetResourceValue(rm, _typeName);
            IsDeclared = stringValue != null;
            Value = IsDeclared ? stringValue.As<T>() : defaultValue;
        }

        protected virtual ResourceManager GetResourceManager()
        {
            return null;
        }

        protected virtual T GetValue(T defaultValue)
        {
            return Value;
        }

        bool ISetting.IsDeclared()
        {
            return IsDeclared;
        }

        string ISetting.GetValue()
        {
            return Value.As<string>();
        }
    }
}

namespace Jetproger.Tools.AppResource
{
    public class AskExitName : ResourceSettingNames<string> { public AskExitName() : base("AskExitName") { } }
    public class AskTitleName : ResourceSettingNames<string> { public AskTitleName() : base("AskTitleName") { } }
    public class CancelName : ResourceSettingNames<string> { public CancelName() : base("CancelName") { } }
    public class CaptionName : ResourceSettingNames<string> { public CaptionName() : base("CaptionName") { } }
    public class DialogName : ResourceSettingNames<string> { public DialogName() : base("DialogName") { } }
    public class ErrorTitleName : ResourceSettingNames<string> { public ErrorTitleName() : base("ErrorTitleName") { } }
    public class InfoTitleName : ResourceSettingNames<string> { public InfoTitleName() : base("InfoTitleName") { } }
    public class InfoTraceName : ResourceSettingNames<string> { public InfoTraceName() : base("InfoTraceName") { } }
    public class LoginName : ResourceSettingNames<string> { public LoginName() : base("LoginName") { } }
    public class MessageLoadingName : ResourceSettingNames<string> { public MessageLoadingName() : base("MessageLoadingName") { } }
    public class MessageNonConnectName : ResourceSettingNames<string> { public MessageNonConnectName() : base("MessageNonConnectName") { } }
    public class MessageNonPasswordName : ResourceSettingNames<string> { public MessageNonPasswordName() : base("MessageNonPasswordName") { } }
    public class MessageStartName : ResourceSettingNames<string> { public MessageStartName() : base("MessageStartName") { } }
    public class MessageSuccessConnectName : ResourceSettingNames<string> { public MessageSuccessConnectName() : base("MessageSuccessConnectName") { } }
    public class MessageSuccessPasswordName : ResourceSettingNames<string> { public MessageSuccessPasswordName() : base("MessageSuccessPasswordName") { } }
    public class OkName : ResourceSettingNames<string> { public OkName() : base("OkName") { } }
    public class PasswordName : ResourceSettingNames<string> { public PasswordName() : base("PasswordName") { } }
    public class PasswordChangeName : ResourceSettingNames<string> { public PasswordChangeName() : base("PasswordChangeName") { } }
    public class PasswordNewName : ResourceSettingNames<string> { public PasswordNewName() : base("PasswordNewName") { } }
    public class PasswordOldName : ResourceSettingNames<string> { public PasswordOldName() : base("PasswordOldName") { } }
    public class UserName : ResourceSettingNames<string> { public UserName() : base("UserName") { } }
    public class AddName : ResourceSettingNames<string> { public AddName() : base("AddName") { } }
    public class CopyName : ResourceSettingNames<string> { public CopyName() : base("CopyName") { } }
    public class EditName : ResourceSettingNames<string> { public EditName() : base("EditName") { } }
    public class RefreshName : ResourceSettingNames<string> { public RefreshName() : base("RefreshName") { } }
    public class RemoveName : ResourceSettingNames<string> { public RemoveName() : base("RemoveName") { } }
    public class RestoreName : ResourceSettingNames<string> { public RestoreName() : base("RestoreName") { } }
    public class DescriptionName : ResourceSettingNames<string> { public DescriptionName() : base("DescriptionName") { } }
    public class FolderName : ResourceSettingNames<string> { public FolderName() : base("FolderName") { } }
    public class FolderOpenName : ResourceSettingNames<string> { public FolderOpenName() : base("FolderOpenName") { } }
    public class NameName : ResourceSettingNames<string> { public NameName() : base("NameName") { } }
    public class TypeName : ResourceSettingNames<string> { public TypeName() : base("TypeName") { } }
    public class BinaryName : ResourceSettingNames<string> { public BinaryName() : base("BinaryName") { } }
    public class BooleanName : ResourceSettingNames<string> { public BooleanName() : base("BooleanName") { } }
    public class DateName : ResourceSettingNames<string> { public DateName() : base("DateName") { } }
    public class GuidName : ResourceSettingNames<string> { public GuidName() : base("GuidName") { } }
    public class IntegerName : ResourceSettingNames<string> { public IntegerName() : base("IntegerName") { } }
    public class ListName : ResourceSettingNames<string> { public ListName() : base("ListName") { } }
    public class NumericName : ResourceSettingNames<string> { public NumericName() : base("NumericName") { } }
    public class RangeName : ResourceSettingNames<string> { public RangeName() : base("RangeName") { } }
    public class ReferenceName : ResourceSettingNames<string> { public ReferenceName() : base("ReferenceName") { } }
    public class StringName : ResourceSettingNames<string> { public StringName() : base("StringName") { } }
    public class LoginInvalidName : ResourceSettingNames<string> { public LoginInvalidName() : base("LoginInvalidName") { } }
    public class LoginLengthName : ResourceSettingNames<string> { public LoginLengthName() : base("LoginLengthName") { } }
    public class MetaCreateTypeName : ResourceSettingNames<string> { public MetaCreateTypeName() : base("MetaCreateTypeName") { } }
    public class MetaImplementInterfaceName : ResourceSettingNames<string> { public MetaImplementInterfaceName() : base("MetaImplementInterfaceName") { } }
    public class NullCodeName : ResourceSettingNames<string> { public NullCodeName() : base("NullCodeName") { } }
    public class NullLoginName : ResourceSettingNames<string> { public NullLoginName() : base("NullLoginName") { } }
    public class NullNameName : ResourceSettingNames<string> { public NullNameName() : base("NullNameName") { } }
    public class PasswordConfirmationInvalidName : ResourceSettingNames<string> { public PasswordConfirmationInvalidName() : base("PasswordConfirmationInvalidName") { } }
    public class PasswordNewInvalidName : ResourceSettingNames<string> { public PasswordNewInvalidName() : base("PasswordNewInvalidName") { } }
    public class ConfigurationName : ResourceSettingNames<string> { public ConfigurationName() : base("ConfigurationName") { } }
    public class ConstantName : ResourceSettingNames<string> { public ConstantName() : base("ConstantName") { } }
    public class ContextName : ResourceSettingNames<string> { public ContextName() : base("ContextName") { } }
    public class PreviousName : ResourceSettingNames<string> { public PreviousName() : base("PreviousName") { } }
    public class FullName : ResourceSettingNames<string> { public FullName() : base("FullName") { } }
    public class NoneName : ResourceSettingNames<string> { public NoneName() : base("NoneName") { } }
    public class ReadName : ResourceSettingNames<string> { public ReadName() : base("ReadName") { } }
    public class LikeLoginName : ResourceSettingNames<string> { public LikeLoginName() : base("LikeLoginName") { } }
    public class NoDigitName : ResourceSettingNames<string> { public NoDigitName() : base("NoDigitName") { } }
    public class NoLowerName : ResourceSettingNames<string> { public NoLowerName() : base("NoLowerName") { } }
    public class NoSymbolName : ResourceSettingNames<string> { public NoSymbolName() : base("NoSymbolName") { } }
    public class NoUpperName : ResourceSettingNames<string> { public NoUpperName() : base("NoUpperName") { } }
    public class SmallLengthName : ResourceSettingNames<string> { public SmallLengthName() : base("SmallLengthName") { } }
    public class StrengthName : ResourceSettingNames<string> { public StrengthName() : base("StrengthName") { } }
    public class DelName : ResourceSettingNames<string> { public DelName() : base("DelName") { } }
    public class NewName : ResourceSettingNames<string> { public NewName() : base("NewName") { } }
    public class ProcName : ResourceSettingNames<string> { public ProcName() : base("ProcName") { } }
    public class AppMemoryName : ResourceSettingNames<string> { public AppMemoryName() : base("AppMemoryName") { } }
    public class ComputerNameName : ResourceSettingNames<string> { public ComputerNameName() : base("ComputerNameName") { } }
    public class DefaultUserName : ResourceSettingNames<string> { public DefaultUserName() : base("DefaultUserName") { } }
    public class DotNetVersionName : ResourceSettingNames<string> { public DotNetVersionName() : base("DotNetVersionName") { } }
    public class DrivesName : ResourceSettingNames<string> { public DrivesName() : base("DrivesName") { } }
    public class OperatingSystemName : ResourceSettingNames<string> { public OperatingSystemName() : base("OperatingSystemName") { } }
    public class ProcessorCountName : ResourceSettingNames<string> { public ProcessorCountName() : base("ProcessorCountName") { } }
    public class TotalMemoryName : ResourceSettingNames<string> { public TotalMemoryName() : base("TotalMemoryName") { } }
    public class WindowsUserName : ResourceSettingNames<string> { public WindowsUserName() : base("WindowsUserName") { } }
    public class StartAppLangName : ResourceSettingNames<string> { public StartAppLangName() : base("StartAppLangName") { } }
    public class StartCmdArgsName : ResourceSettingNames<string> { public StartCmdArgsName() : base("StartCmdArgsName") { } }
    public class StartCmdPoolName : ResourceSettingNames<string> { public StartCmdPoolName() : base("StartCmdPoolName") { } }
    public class StartDIName : ResourceSettingNames<string> { public StartDIName() : base("StartDIName") { } }
    public class StartEndName : ResourceSettingNames<string> { public StartEndName() : base("StartEndName") { } }
    public class StartNLogBaseName : ResourceSettingNames<string> { public StartNLogBaseName() : base("StartNLogBaseName") { } }
    public class StartNLogTypedName : ResourceSettingNames<string> { public StartNLogTypedName() : base("StartNLogTypedName") { } }
    public class StartReadConfName : ResourceSettingNames<string> { public StartReadConfName() : base("StartReadConfName") { } }
    public class StartResName : ResourceSettingNames<string> { public StartResName() : base("StartResName") { } }
    public class StartTraceName : ResourceSettingNames<string> { public StartTraceName() : base("StartTraceName") { } }

    public class AskExitNote : ResourceSettingDescriptions<string> { public AskExitNote() : base("AskExitNote") { } }
    public class AskTitleNote : ResourceSettingDescriptions<string> { public AskTitleNote() : base("AskTitleNote") { } }
    public class CancelNote : ResourceSettingDescriptions<string> { public CancelNote() : base("CancelNote") { } }
    public class CaptionNote : ResourceSettingDescriptions<string> { public CaptionNote() : base("CaptionNote") { } }
    public class DialogNote : ResourceSettingDescriptions<string> { public DialogNote() : base("DialogNote") { } }
    public class ErrorTitleNote : ResourceSettingDescriptions<string> { public ErrorTitleNote() : base("ErrorTitleNote") { } }
    public class InfoTitleNote : ResourceSettingDescriptions<string> { public InfoTitleNote() : base("InfoTitleNote") { } }
    public class InfoTraceNote : ResourceSettingDescriptions<string> { public InfoTraceNote() : base("InfoTraceNote") { } }
    public class LoginNote : ResourceSettingDescriptions<string> { public LoginNote() : base("LoginNote") { } }
    public class MessageLoadingNote : ResourceSettingDescriptions<string> { public MessageLoadingNote() : base("MessageLoadingNote") { } }
    public class MessageNonConnectNote : ResourceSettingDescriptions<string> { public MessageNonConnectNote() : base("MessageNonConnectNote") { } }
    public class MessageNonPasswordNote : ResourceSettingDescriptions<string> { public MessageNonPasswordNote() : base("MessageNonPasswordNote") { } }
    public class MessageStartNote : ResourceSettingDescriptions<string> { public MessageStartNote() : base("MessageStartNote") { } }
    public class MessageSuccessConnectNote : ResourceSettingDescriptions<string> { public MessageSuccessConnectNote() : base("MessageSuccessConnectNote") { } }
    public class MessageSuccessPasswordNote : ResourceSettingDescriptions<string> { public MessageSuccessPasswordNote() : base("MessageSuccessPasswordNote") { } }
    public class OkNote : ResourceSettingDescriptions<string> { public OkNote() : base("OkNote") { } }
    public class PasswordNote : ResourceSettingDescriptions<string> { public PasswordNote() : base("PasswordNote") { } }
    public class PasswordChangeNote : ResourceSettingDescriptions<string> { public PasswordChangeNote() : base("PasswordChangeNote") { } }
    public class PasswordNewNote : ResourceSettingDescriptions<string> { public PasswordNewNote() : base("PasswordNewNote") { } }
    public class PasswordOldNote : ResourceSettingDescriptions<string> { public PasswordOldNote() : base("PasswordOldNote") { } }
    public class UserNote : ResourceSettingDescriptions<string> { public UserNote() : base("UserNote") { } }
    public class AddNote : ResourceSettingDescriptions<string> { public AddNote() : base("AddNote") { } }
    public class CopyNote : ResourceSettingDescriptions<string> { public CopyNote() : base("CopyNote") { } }
    public class EditNote : ResourceSettingDescriptions<string> { public EditNote() : base("EditNote") { } }
    public class RefreshNote : ResourceSettingDescriptions<string> { public RefreshNote() : base("RefreshNote") { } }
    public class RemoveNote : ResourceSettingDescriptions<string> { public RemoveNote() : base("RemoveNote") { } }
    public class RestoreNote : ResourceSettingDescriptions<string> { public RestoreNote() : base("RestoreNote") { } }
    public class DescriptionNote : ResourceSettingDescriptions<string> { public DescriptionNote() : base("DescriptionNote") { } }
    public class FolderNote : ResourceSettingDescriptions<string> { public FolderNote() : base("FolderNote") { } }
    public class FolderOpenNote : ResourceSettingDescriptions<string> { public FolderOpenNote() : base("FolderOpenNote") { } }
    public class NoteNote : ResourceSettingDescriptions<string> { public NoteNote() : base("NoteNote") { } }
    public class TypeNote : ResourceSettingDescriptions<string> { public TypeNote() : base("TypeNote") { } }
    public class BinaryNote : ResourceSettingDescriptions<string> { public BinaryNote() : base("BinaryNote") { } }
    public class BooleanNote : ResourceSettingDescriptions<string> { public BooleanNote() : base("BooleanNote") { } }
    public class DateNote : ResourceSettingDescriptions<string> { public DateNote() : base("DateNote") { } }
    public class GuidNote : ResourceSettingDescriptions<string> { public GuidNote() : base("GuidNote") { } }
    public class IntegerNote : ResourceSettingDescriptions<string> { public IntegerNote() : base("IntegerNote") { } }
    public class ListNote : ResourceSettingDescriptions<string> { public ListNote() : base("ListNote") { } }
    public class NumericNote : ResourceSettingDescriptions<string> { public NumericNote() : base("NumericNote") { } }
    public class RangeNote : ResourceSettingDescriptions<string> { public RangeNote() : base("RangeNote") { } }
    public class ReferenceNote : ResourceSettingDescriptions<string> { public ReferenceNote() : base("ReferenceNote") { } }
    public class StringNote : ResourceSettingDescriptions<string> { public StringNote() : base("StringNote") { } }
    public class LoginInvalidNote : ResourceSettingDescriptions<string> { public LoginInvalidNote() : base("LoginInvalidNote") { } }
    public class LoginLengthNote : ResourceSettingDescriptions<string> { public LoginLengthNote() : base("LoginLengthNote") { } }
    public class MetaCreateTypeNote : ResourceSettingDescriptions<string> { public MetaCreateTypeNote() : base("MetaCreateTypeNote") { } }
    public class MetaImplementInterfaceNote : ResourceSettingDescriptions<string> { public MetaImplementInterfaceNote() : base("MetaImplementInterfaceNote") { } }
    public class NullCodeNote : ResourceSettingDescriptions<string> { public NullCodeNote() : base("NullCodeNote") { } }
    public class NullLoginNote : ResourceSettingDescriptions<string> { public NullLoginNote() : base("NullLoginNote") { } }
    public class NullNoteNote : ResourceSettingDescriptions<string> { public NullNoteNote() : base("NullNoteNote") { } }
    public class PasswordConfirmationInvalidNote : ResourceSettingDescriptions<string> { public PasswordConfirmationInvalidNote() : base("PasswordConfirmationInvalidNote") { } }
    public class PasswordNewInvalidNote : ResourceSettingDescriptions<string> { public PasswordNewInvalidNote() : base("PasswordNewInvalidNote") { } }
    public class ConfigurationNote : ResourceSettingDescriptions<string> { public ConfigurationNote() : base("ConfigurationNote") { } }
    public class ConstantNote : ResourceSettingDescriptions<string> { public ConstantNote() : base("ConstantNote") { } }
    public class ContextNote : ResourceSettingDescriptions<string> { public ContextNote() : base("ContextNote") { } }
    public class PreviousNote : ResourceSettingDescriptions<string> { public PreviousNote() : base("PreviousNote") { } }
    public class FullNote : ResourceSettingDescriptions<string> { public FullNote() : base("FullNote") { } }
    public class NoneNote : ResourceSettingDescriptions<string> { public NoneNote() : base("NoneNote") { } }
    public class ReadNote : ResourceSettingDescriptions<string> { public ReadNote() : base("ReadNote") { } }
    public class LikeLoginNote : ResourceSettingDescriptions<string> { public LikeLoginNote() : base("LikeLoginNote") { } }
    public class NoDigitNote : ResourceSettingDescriptions<string> { public NoDigitNote() : base("NoDigitNote") { } }
    public class NoLowerNote : ResourceSettingDescriptions<string> { public NoLowerNote() : base("NoLowerNote") { } }
    public class NoSymbolNote : ResourceSettingDescriptions<string> { public NoSymbolNote() : base("NoSymbolNote") { } }
    public class NoUpperNote : ResourceSettingDescriptions<string> { public NoUpperNote() : base("NoUpperNote") { } }
    public class SmallLengthNote : ResourceSettingDescriptions<string> { public SmallLengthNote() : base("SmallLengthNote") { } }
    public class StrengthNote : ResourceSettingDescriptions<string> { public StrengthNote() : base("StrengthNote") { } }
    public class DelNote : ResourceSettingDescriptions<string> { public DelNote() : base("DelNote") { } }
    public class NewNote : ResourceSettingDescriptions<string> { public NewNote() : base("NewNote") { } }
    public class ProcNote : ResourceSettingDescriptions<string> { public ProcNote() : base("ProcNote") { } }
    public class AppMemoryNote : ResourceSettingDescriptions<string> { public AppMemoryNote() : base("AppMemoryNote") { } }
    public class ComputerNoteNote : ResourceSettingDescriptions<string> { public ComputerNoteNote() : base("ComputerNoteNote") { } }
    public class DefaultUserNote : ResourceSettingDescriptions<string> { public DefaultUserNote() : base("DefaultUserNote") { } }
    public class DotNetVersionNote : ResourceSettingDescriptions<string> { public DotNetVersionNote() : base("DotNetVersionNote") { } }
    public class DrivesNote : ResourceSettingDescriptions<string> { public DrivesNote() : base("DrivesNote") { } }
    public class OperatingSystemNote : ResourceSettingDescriptions<string> { public OperatingSystemNote() : base("OperatingSystemNote") { } }
    public class ProcessorCountNote : ResourceSettingDescriptions<string> { public ProcessorCountNote() : base("ProcessorCountNote") { } }
    public class TotalMemoryNote : ResourceSettingDescriptions<string> { public TotalMemoryNote() : base("TotalMemoryNote") { } }
    public class WindowsUserNote : ResourceSettingDescriptions<string> { public WindowsUserNote() : base("WindowsUserNote") { } }
    public class StartAppLangNote : ResourceSettingDescriptions<string> { public StartAppLangNote() : base("StartAppLangNote") { } }
    public class StartCmdArgsNote : ResourceSettingDescriptions<string> { public StartCmdArgsNote() : base("StartCmdArgsNote") { } }
    public class StartCmdPoolNote : ResourceSettingDescriptions<string> { public StartCmdPoolNote() : base("StartCmdPoolNote") { } }
    public class StartDINote : ResourceSettingDescriptions<string> { public StartDINote() : base("StartDINote") { } }
    public class StartEndNote : ResourceSettingDescriptions<string> { public StartEndNote() : base("StartEndNote") { } }
    public class StartNLogBaseNote : ResourceSettingDescriptions<string> { public StartNLogBaseNote() : base("StartNLogBaseNote") { } }
    public class StartNLogTypedNote : ResourceSettingDescriptions<string> { public StartNLogTypedNote() : base("StartNLogTypedNote") { } }
    public class StartReadConfNote : ResourceSettingDescriptions<string> { public StartReadConfNote() : base("StartReadConfNote") { } }
    public class StartResNote : ResourceSettingDescriptions<string> { public StartResNote() : base("StartResNote") { } }
    public class StartTraceNote : ResourceSettingDescriptions<string> { public StartTraceNote() : base("StartTraceNote") { } }

    public class AskExitKeys : ResourceSettingShortcuts<string> { public AskExitKeys() : base("AskExitKeys") { } }
    public class AskTitleKeys : ResourceSettingShortcuts<string> { public AskTitleKeys() : base("AskTitleKeys") { } }
    public class CancelKeys : ResourceSettingShortcuts<string> { public CancelKeys() : base("CancelKeys") { } }
    public class CaptionKeys : ResourceSettingShortcuts<string> { public CaptionKeys() : base("CaptionKeys") { } }
    public class DialogKeys : ResourceSettingShortcuts<string> { public DialogKeys() : base("DialogKeys") { } }
    public class ErrorTitleKeys : ResourceSettingShortcuts<string> { public ErrorTitleKeys() : base("ErrorTitleKeys") { } }
    public class InfoTitleKeys : ResourceSettingShortcuts<string> { public InfoTitleKeys() : base("InfoTitleKeys") { } }
    public class InfoTraceKeys : ResourceSettingShortcuts<string> { public InfoTraceKeys() : base("InfoTraceKeys") { } }
    public class LoginKeys : ResourceSettingShortcuts<string> { public LoginKeys() : base("LoginKeys") { } }
    public class MessageLoadingKeys : ResourceSettingShortcuts<string> { public MessageLoadingKeys() : base("MessageLoadingKeys") { } }
    public class MessageNonConnectKeys : ResourceSettingShortcuts<string> { public MessageNonConnectKeys() : base("MessageNonConnectKeys") { } }
    public class MessageNonPasswordKeys : ResourceSettingShortcuts<string> { public MessageNonPasswordKeys() : base("MessageNonPasswordKeys") { } }
    public class MessageStartKeys : ResourceSettingShortcuts<string> { public MessageStartKeys() : base("MessageStartKeys") { } }
    public class MessageSuccessConnectKeys : ResourceSettingShortcuts<string> { public MessageSuccessConnectKeys() : base("MessageSuccessConnectKeys") { } }
    public class MessageSuccessPasswordKeys : ResourceSettingShortcuts<string> { public MessageSuccessPasswordKeys() : base("MessageSuccessPasswordKeys") { } }
    public class OkKeys : ResourceSettingShortcuts<string> { public OkKeys() : base("OkKeys") { } }
    public class PasswordKeys : ResourceSettingShortcuts<string> { public PasswordKeys() : base("PasswordKeys") { } }
    public class PasswordChangeKeys : ResourceSettingShortcuts<string> { public PasswordChangeKeys() : base("PasswordChangeKeys") { } }
    public class PasswordNewKeys : ResourceSettingShortcuts<string> { public PasswordNewKeys() : base("PasswordNewKeys") { } }
    public class PasswordOldKeys : ResourceSettingShortcuts<string> { public PasswordOldKeys() : base("PasswordOldKeys") { } }
    public class UserKeys : ResourceSettingShortcuts<string> { public UserKeys() : base("UserKeys") { } }
    public class AddKeys : ResourceSettingShortcuts<string> { public AddKeys() : base("AddKeys") { } }
    public class CopyKeys : ResourceSettingShortcuts<string> { public CopyKeys() : base("CopyKeys") { } }
    public class EditKeys : ResourceSettingShortcuts<string> { public EditKeys() : base("EditKeys") { } }
    public class RefreshKeys : ResourceSettingShortcuts<string> { public RefreshKeys() : base("RefreshKeys") { } }
    public class RemoveKeys : ResourceSettingShortcuts<string> { public RemoveKeys() : base("RemoveKeys") { } }
    public class RestoreKeys : ResourceSettingShortcuts<string> { public RestoreKeys() : base("RestoreKeys") { } }
    public class DescriptionKeys : ResourceSettingShortcuts<string> { public DescriptionKeys() : base("DescriptionKeys") { } }
    public class FolderKeys : ResourceSettingShortcuts<string> { public FolderKeys() : base("FolderKeys") { } }
    public class FolderOpenKeys : ResourceSettingShortcuts<string> { public FolderOpenKeys() : base("FolderOpenKeys") { } }
    public class KeysKeys : ResourceSettingShortcuts<string> { public KeysKeys() : base("KeysKeys") { } }
    public class TypeKeys : ResourceSettingShortcuts<string> { public TypeKeys() : base("TypeKeys") { } }
    public class BinaryKeys : ResourceSettingShortcuts<string> { public BinaryKeys() : base("BinaryKeys") { } }
    public class BooleanKeys : ResourceSettingShortcuts<string> { public BooleanKeys() : base("BooleanKeys") { } }
    public class DateKeys : ResourceSettingShortcuts<string> { public DateKeys() : base("DateKeys") { } }
    public class GuidKeys : ResourceSettingShortcuts<string> { public GuidKeys() : base("GuidKeys") { } }
    public class IntegerKeys : ResourceSettingShortcuts<string> { public IntegerKeys() : base("IntegerKeys") { } }
    public class ListKeys : ResourceSettingShortcuts<string> { public ListKeys() : base("ListKeys") { } }
    public class NumericKeys : ResourceSettingShortcuts<string> { public NumericKeys() : base("NumericKeys") { } }
    public class RangeKeys : ResourceSettingShortcuts<string> { public RangeKeys() : base("RangeKeys") { } }
    public class ReferenceKeys : ResourceSettingShortcuts<string> { public ReferenceKeys() : base("ReferenceKeys") { } }
    public class StringKeys : ResourceSettingShortcuts<string> { public StringKeys() : base("StringKeys") { } }
    public class LoginInvalidKeys : ResourceSettingShortcuts<string> { public LoginInvalidKeys() : base("LoginInvalidKeys") { } }
    public class LoginLengthKeys : ResourceSettingShortcuts<string> { public LoginLengthKeys() : base("LoginLengthKeys") { } }
    public class MetaCreateTypeKeys : ResourceSettingShortcuts<string> { public MetaCreateTypeKeys() : base("MetaCreateTypeKeys") { } }
    public class MetaImplementInterfaceKeys : ResourceSettingShortcuts<string> { public MetaImplementInterfaceKeys() : base("MetaImplementInterfaceKeys") { } }
    public class NullCodeKeys : ResourceSettingShortcuts<string> { public NullCodeKeys() : base("NullCodeKeys") { } }
    public class NullLoginKeys : ResourceSettingShortcuts<string> { public NullLoginKeys() : base("NullLoginKeys") { } }
    public class NullKeysKeys : ResourceSettingShortcuts<string> { public NullKeysKeys() : base("NullKeysKeys") { } }
    public class PasswordConfirmationInvalidKeys : ResourceSettingShortcuts<string> { public PasswordConfirmationInvalidKeys() : base("PasswordConfirmationInvalidKeys") { } }
    public class PasswordNewInvalidKeys : ResourceSettingShortcuts<string> { public PasswordNewInvalidKeys() : base("PasswordNewInvalidKeys") { } }
    public class ConfigurationKeys : ResourceSettingShortcuts<string> { public ConfigurationKeys() : base("ConfigurationKeys") { } }
    public class ConstantKeys : ResourceSettingShortcuts<string> { public ConstantKeys() : base("ConstantKeys") { } }
    public class ContextKeys : ResourceSettingShortcuts<string> { public ContextKeys() : base("ContextKeys") { } }
    public class PreviousKeys : ResourceSettingShortcuts<string> { public PreviousKeys() : base("PreviousKeys") { } }
    public class FullKeys : ResourceSettingShortcuts<string> { public FullKeys() : base("FullKeys") { } }
    public class NoneKeys : ResourceSettingShortcuts<string> { public NoneKeys() : base("NoneKeys") { } }
    public class ReadKeys : ResourceSettingShortcuts<string> { public ReadKeys() : base("ReadKeys") { } }
    public class LikeLoginKeys : ResourceSettingShortcuts<string> { public LikeLoginKeys() : base("LikeLoginKeys") { } }
    public class NoDigitKeys : ResourceSettingShortcuts<string> { public NoDigitKeys() : base("NoDigitKeys") { } }
    public class NoLowerKeys : ResourceSettingShortcuts<string> { public NoLowerKeys() : base("NoLowerKeys") { } }
    public class NoSymbolKeys : ResourceSettingShortcuts<string> { public NoSymbolKeys() : base("NoSymbolKeys") { } }
    public class NoUpperKeys : ResourceSettingShortcuts<string> { public NoUpperKeys() : base("NoUpperKeys") { } }
    public class SmallLengthKeys : ResourceSettingShortcuts<string> { public SmallLengthKeys() : base("SmallLengthKeys") { } }
    public class StrengthKeys : ResourceSettingShortcuts<string> { public StrengthKeys() : base("StrengthKeys") { } }
    public class DelKeys : ResourceSettingShortcuts<string> { public DelKeys() : base("DelKeys") { } }
    public class NewKeys : ResourceSettingShortcuts<string> { public NewKeys() : base("NewKeys") { } }
    public class ProcKeys : ResourceSettingShortcuts<string> { public ProcKeys() : base("ProcKeys") { } }
    public class AppMemoryKeys : ResourceSettingShortcuts<string> { public AppMemoryKeys() : base("AppMemoryKeys") { } }
    public class ComputerKeysKeys : ResourceSettingShortcuts<string> { public ComputerKeysKeys() : base("ComputerKeysKeys") { } }
    public class DefaultUserKeys : ResourceSettingShortcuts<string> { public DefaultUserKeys() : base("DefaultUserKeys") { } }
    public class DotNetVersionKeys : ResourceSettingShortcuts<string> { public DotNetVersionKeys() : base("DotNetVersionKeys") { } }
    public class DrivesKeys : ResourceSettingShortcuts<string> { public DrivesKeys() : base("DrivesKeys") { } }
    public class OperatingSystemKeys : ResourceSettingShortcuts<string> { public OperatingSystemKeys() : base("OperatingSystemKeys") { } }
    public class ProcessorCountKeys : ResourceSettingShortcuts<string> { public ProcessorCountKeys() : base("ProcessorCountKeys") { } }
    public class TotalMemoryKeys : ResourceSettingShortcuts<string> { public TotalMemoryKeys() : base("TotalMemoryKeys") { } }
    public class WindowsUserKeys : ResourceSettingShortcuts<string> { public WindowsUserKeys() : base("WindowsUserKeys") { } }
    public class StartAppLangKeys : ResourceSettingShortcuts<string> { public StartAppLangKeys() : base("StartAppLangKeys") { } }
    public class StartCmdArgsKeys : ResourceSettingShortcuts<string> { public StartCmdArgsKeys() : base("StartCmdArgsKeys") { } }
    public class StartCmdPoolKeys : ResourceSettingShortcuts<string> { public StartCmdPoolKeys() : base("StartCmdPoolKeys") { } }
    public class StartDIKeys : ResourceSettingShortcuts<string> { public StartDIKeys() : base("StartDIKeys") { } }
    public class StartEndKeys : ResourceSettingShortcuts<string> { public StartEndKeys() : base("StartEndKeys") { } }
    public class StartNLogBaseKeys : ResourceSettingShortcuts<string> { public StartNLogBaseKeys() : base("StartNLogBaseKeys") { } }
    public class StartNLogTypedKeys : ResourceSettingShortcuts<string> { public StartNLogTypedKeys() : base("StartNLogTypedKeys") { } }
    public class StartReadConfKeys : ResourceSettingShortcuts<string> { public StartReadConfKeys() : base("StartReadConfKeys") { } }
    public class StartResKeys : ResourceSettingShortcuts<string> { public StartResKeys() : base("StartResKeys") { } }
    public class StartTraceKeys : ResourceSettingShortcuts<string> { public StartTraceKeys() : base("StartTraceKeys") { } }

    public class AskExitSpec : ResourceSettingSpecifies<string> { public AskExitSpec() : base("AskExitSpec") { } }
    public class AskTitleSpec : ResourceSettingSpecifies<string> { public AskTitleSpec() : base("AskTitleSpec") { } }
    public class CancelSpec : ResourceSettingSpecifies<string> { public CancelSpec() : base("CancelSpec") { } }
    public class CaptionSpec : ResourceSettingSpecifies<string> { public CaptionSpec() : base("CaptionSpec") { } }
    public class DialogSpec : ResourceSettingSpecifies<string> { public DialogSpec() : base("DialogSpec") { } }
    public class ErrorTitleSpec : ResourceSettingSpecifies<string> { public ErrorTitleSpec() : base("ErrorTitleSpec") { } }
    public class InfoTitleSpec : ResourceSettingSpecifies<string> { public InfoTitleSpec() : base("InfoTitleSpec") { } }
    public class InfoTraceSpec : ResourceSettingSpecifies<string> { public InfoTraceSpec() : base("InfoTraceSpec") { } }
    public class LoginSpec : ResourceSettingSpecifies<string> { public LoginSpec() : base("LoginSpec") { } }
    public class MessageLoadingSpec : ResourceSettingSpecifies<string> { public MessageLoadingSpec() : base("MessageLoadingSpec") { } }
    public class MessageNonConnectSpec : ResourceSettingSpecifies<string> { public MessageNonConnectSpec() : base("MessageNonConnectSpec") { } }
    public class MessageNonPasswordSpec : ResourceSettingSpecifies<string> { public MessageNonPasswordSpec() : base("MessageNonPasswordSpec") { } }
    public class MessageStartSpec : ResourceSettingSpecifies<string> { public MessageStartSpec() : base("MessageStartSpec") { } }
    public class MessageSuccessConnectSpec : ResourceSettingSpecifies<string> { public MessageSuccessConnectSpec() : base("MessageSuccessConnectSpec") { } }
    public class MessageSuccessPasswordSpec : ResourceSettingSpecifies<string> { public MessageSuccessPasswordSpec() : base("MessageSuccessPasswordSpec") { } }
    public class OkSpec : ResourceSettingSpecifies<string> { public OkSpec() : base("OkSpec") { } }
    public class PasswordSpec : ResourceSettingSpecifies<string> { public PasswordSpec() : base("PasswordSpec") { } }
    public class PasswordChangeSpec : ResourceSettingSpecifies<string> { public PasswordChangeSpec() : base("PasswordChangeSpec") { } }
    public class PasswordNewSpec : ResourceSettingSpecifies<string> { public PasswordNewSpec() : base("PasswordNewSpec") { } }
    public class PasswordOldSpec : ResourceSettingSpecifies<string> { public PasswordOldSpec() : base("PasswordOldSpec") { } }
    public class UserSpec : ResourceSettingSpecifies<string> { public UserSpec() : base("UserSpec") { } }
    public class AddSpec : ResourceSettingSpecifies<string> { public AddSpec() : base("AddSpec") { } }
    public class CopySpec : ResourceSettingSpecifies<string> { public CopySpec() : base("CopySpec") { } }
    public class EditSpec : ResourceSettingSpecifies<string> { public EditSpec() : base("EditSpec") { } }
    public class RefreshSpec : ResourceSettingSpecifies<string> { public RefreshSpec() : base("RefreshSpec") { } }
    public class RemoveSpec : ResourceSettingSpecifies<string> { public RemoveSpec() : base("RemoveSpec") { } }
    public class RestoreSpec : ResourceSettingSpecifies<string> { public RestoreSpec() : base("RestoreSpec") { } }
    public class DescriptionSpec : ResourceSettingSpecifies<string> { public DescriptionSpec() : base("DescriptionSpec") { } }
    public class FolderSpec : ResourceSettingSpecifies<string> { public FolderSpec() : base("FolderSpec") { } }
    public class FolderOpenSpec : ResourceSettingSpecifies<string> { public FolderOpenSpec() : base("FolderOpenSpec") { } }
    public class SpecSpec : ResourceSettingSpecifies<string> { public SpecSpec() : base("SpecSpec") { } }
    public class TypeSpec : ResourceSettingSpecifies<string> { public TypeSpec() : base("TypeSpec") { } }
    public class BinarySpec : ResourceSettingSpecifies<string> { public BinarySpec() : base("BinarySpec") { } }
    public class BooleanSpec : ResourceSettingSpecifies<string> { public BooleanSpec() : base("BooleanSpec") { } }
    public class DateSpec : ResourceSettingSpecifies<string> { public DateSpec() : base("DateSpec") { } }
    public class GuidSpec : ResourceSettingSpecifies<string> { public GuidSpec() : base("GuidSpec") { } }
    public class IntegerSpec : ResourceSettingSpecifies<string> { public IntegerSpec() : base("IntegerSpec") { } }
    public class ListSpec : ResourceSettingSpecifies<string> { public ListSpec() : base("ListSpec") { } }
    public class NumericSpec : ResourceSettingSpecifies<string> { public NumericSpec() : base("NumericSpec") { } }
    public class RangeSpec : ResourceSettingSpecifies<string> { public RangeSpec() : base("RangeSpec") { } }
    public class ReferenceSpec : ResourceSettingSpecifies<string> { public ReferenceSpec() : base("ReferenceSpec") { } }
    public class StringSpec : ResourceSettingSpecifies<string> { public StringSpec() : base("StringSpec") { } }
    public class LoginInvalidSpec : ResourceSettingSpecifies<string> { public LoginInvalidSpec() : base("LoginInvalidSpec") { } }
    public class LoginLengthSpec : ResourceSettingSpecifies<string> { public LoginLengthSpec() : base("LoginLengthSpec") { } }
    public class MetaCreateTypeSpec : ResourceSettingSpecifies<string> { public MetaCreateTypeSpec() : base("MetaCreateTypeSpec") { } }
    public class MetaImplementInterfaceSpec : ResourceSettingSpecifies<string> { public MetaImplementInterfaceSpec() : base("MetaImplementInterfaceSpec") { } }
    public class NullCodeSpec : ResourceSettingSpecifies<string> { public NullCodeSpec() : base("NullCodeSpec") { } }
    public class NullLoginSpec : ResourceSettingSpecifies<string> { public NullLoginSpec() : base("NullLoginSpec") { } }
    public class NullSpecSpec : ResourceSettingSpecifies<string> { public NullSpecSpec() : base("NullSpecSpec") { } }
    public class PasswordConfirmationInvalidSpec : ResourceSettingSpecifies<string> { public PasswordConfirmationInvalidSpec() : base("PasswordConfirmationInvalidSpec") { } }
    public class PasswordNewInvalidSpec : ResourceSettingSpecifies<string> { public PasswordNewInvalidSpec() : base("PasswordNewInvalidSpec") { } }
    public class ConfigurationSpec : ResourceSettingSpecifies<string> { public ConfigurationSpec() : base("ConfigurationSpec") { } }
    public class ConstantSpec : ResourceSettingSpecifies<string> { public ConstantSpec() : base("ConstantSpec") { } }
    public class ContextSpec : ResourceSettingSpecifies<string> { public ContextSpec() : base("ContextSpec") { } }
    public class PreviousSpec : ResourceSettingSpecifies<string> { public PreviousSpec() : base("PreviousSpec") { } }
    public class FullSpec : ResourceSettingSpecifies<string> { public FullSpec() : base("FullSpec") { } }
    public class NoneSpec : ResourceSettingSpecifies<string> { public NoneSpec() : base("NoneSpec") { } }
    public class ReadSpec : ResourceSettingSpecifies<string> { public ReadSpec() : base("ReadSpec") { } }
    public class LikeLoginSpec : ResourceSettingSpecifies<string> { public LikeLoginSpec() : base("LikeLoginSpec") { } }
    public class NoDigitSpec : ResourceSettingSpecifies<string> { public NoDigitSpec() : base("NoDigitSpec") { } }
    public class NoLowerSpec : ResourceSettingSpecifies<string> { public NoLowerSpec() : base("NoLowerSpec") { } }
    public class NoSymbolSpec : ResourceSettingSpecifies<string> { public NoSymbolSpec() : base("NoSymbolSpec") { } }
    public class NoUpperSpec : ResourceSettingSpecifies<string> { public NoUpperSpec() : base("NoUpperSpec") { } }
    public class SmallLengthSpec : ResourceSettingSpecifies<string> { public SmallLengthSpec() : base("SmallLengthSpec") { } }
    public class StrengthSpec : ResourceSettingSpecifies<string> { public StrengthSpec() : base("StrengthSpec") { } }
    public class DelSpec : ResourceSettingSpecifies<string> { public DelSpec() : base("DelSpec") { } }
    public class NewSpec : ResourceSettingSpecifies<string> { public NewSpec() : base("NewSpec") { } }
    public class ProcSpec : ResourceSettingSpecifies<string> { public ProcSpec() : base("ProcSpec") { } }
    public class AppMemorySpec : ResourceSettingSpecifies<string> { public AppMemorySpec() : base("AppMemorySpec") { } }
    public class ComputerSpecSpec : ResourceSettingSpecifies<string> { public ComputerSpecSpec() : base("ComputerSpecSpec") { } }
    public class DefaultUserSpec : ResourceSettingSpecifies<string> { public DefaultUserSpec() : base("DefaultUserSpec") { } }
    public class DotNetVersionSpec : ResourceSettingSpecifies<string> { public DotNetVersionSpec() : base("DotNetVersionSpec") { } }
    public class DrivesSpec : ResourceSettingSpecifies<string> { public DrivesSpec() : base("DrivesSpec") { } }
    public class OperatingSystemSpec : ResourceSettingSpecifies<string> { public OperatingSystemSpec() : base("OperatingSystemSpec") { } }
    public class ProcessorCountSpec : ResourceSettingSpecifies<string> { public ProcessorCountSpec() : base("ProcessorCountSpec") { } }
    public class TotalMemorySpec : ResourceSettingSpecifies<string> { public TotalMemorySpec() : base("TotalMemorySpec") { } }
    public class WindowsUserSpec : ResourceSettingSpecifies<string> { public WindowsUserSpec() : base("WindowsUserSpec") { } }
    public class StartAppLangSpec : ResourceSettingSpecifies<string> { public StartAppLangSpec() : base("StartAppLangSpec") { } }
    public class StartCmdArgsSpec : ResourceSettingSpecifies<string> { public StartCmdArgsSpec() : base("StartCmdArgsSpec") { } }
    public class StartCmdPoolSpec : ResourceSettingSpecifies<string> { public StartCmdPoolSpec() : base("StartCmdPoolSpec") { } }
    public class StartDISpec : ResourceSettingSpecifies<string> { public StartDISpec() : base("StartDISpec") { } }
    public class StartEndSpec : ResourceSettingSpecifies<string> { public StartEndSpec() : base("StartEndSpec") { } }
    public class StartNLogBaseSpec : ResourceSettingSpecifies<string> { public StartNLogBaseSpec() : base("StartNLogBaseSpec") { } }
    public class StartNLogTypedSpec : ResourceSettingSpecifies<string> { public StartNLogTypedSpec() : base("StartNLogTypedSpec") { } }
    public class StartReadConfSpec : ResourceSettingSpecifies<string> { public StartReadConfSpec() : base("StartReadConfSpec") { } }
    public class StartResSpec : ResourceSettingSpecifies<string> { public StartResSpec() : base("StartResSpec") { } }
    public class StartTraceSpec : ResourceSettingSpecifies<string> { public StartTraceSpec() : base("StartTraceSpec") { } }

    public class AskExitIcon : ResourceSettingPictures<string> { public AskExitIcon() : base("AskExitIcon") { } }
    public class AskTitleIcon : ResourceSettingPictures<string> { public AskTitleIcon() : base("AskTitleIcon") { } }
    public class CancelIcon : ResourceSettingPictures<string> { public CancelIcon() : base("CancelIcon") { } }
    public class CaptionIcon : ResourceSettingPictures<string> { public CaptionIcon() : base("CaptionIcon") { } }
    public class DialogIcon : ResourceSettingPictures<string> { public DialogIcon() : base("DialogIcon") { } }
    public class ErrorTitleIcon : ResourceSettingPictures<string> { public ErrorTitleIcon() : base("ErrorTitleIcon") { } }
    public class InfoTitleIcon : ResourceSettingPictures<string> { public InfoTitleIcon() : base("InfoTitleIcon") { } }
    public class InfoTraceIcon : ResourceSettingPictures<string> { public InfoTraceIcon() : base("InfoTraceIcon") { } }
    public class LoginIcon : ResourceSettingPictures<string> { public LoginIcon() : base("LoginIcon") { } }
    public class MessageLoadingIcon : ResourceSettingPictures<string> { public MessageLoadingIcon() : base("MessageLoadingIcon") { } }
    public class MessageNonConnectIcon : ResourceSettingPictures<string> { public MessageNonConnectIcon() : base("MessageNonConnectIcon") { } }
    public class MessageNonPasswordIcon : ResourceSettingPictures<string> { public MessageNonPasswordIcon() : base("MessageNonPasswordIcon") { } }
    public class MessageStartIcon : ResourceSettingPictures<string> { public MessageStartIcon() : base("MessageStartIcon") { } }
    public class MessageSuccessConnectIcon : ResourceSettingPictures<string> { public MessageSuccessConnectIcon() : base("MessageSuccessConnectIcon") { } }
    public class MessageSuccessPasswordIcon : ResourceSettingPictures<string> { public MessageSuccessPasswordIcon() : base("MessageSuccessPasswordIcon") { } }
    public class OkIcon : ResourceSettingPictures<string> { public OkIcon() : base("OkIcon") { } }
    public class PasswordIcon : ResourceSettingPictures<string> { public PasswordIcon() : base("PasswordIcon") { } }
    public class PasswordChangeIcon : ResourceSettingPictures<string> { public PasswordChangeIcon() : base("PasswordChangeIcon") { } }
    public class PasswordNewIcon : ResourceSettingPictures<string> { public PasswordNewIcon() : base("PasswordNewIcon") { } }
    public class PasswordOldIcon : ResourceSettingPictures<string> { public PasswordOldIcon() : base("PasswordOldIcon") { } }
    public class UserIcon : ResourceSettingPictures<string> { public UserIcon() : base("UserIcon") { } }
    public class AddIcon : ResourceSettingPictures<string> { public AddIcon() : base("AddIcon") { } }
    public class CopyIcon : ResourceSettingPictures<string> { public CopyIcon() : base("CopyIcon") { } }
    public class EditIcon : ResourceSettingPictures<string> { public EditIcon() : base("EditIcon") { } }
    public class RefreshIcon : ResourceSettingPictures<string> { public RefreshIcon() : base("RefreshIcon") { } }
    public class RemoveIcon : ResourceSettingPictures<string> { public RemoveIcon() : base("RemoveIcon") { } }
    public class RestoreIcon : ResourceSettingPictures<string> { public RestoreIcon() : base("RestoreIcon") { } }
    public class DescriptionIcon : ResourceSettingPictures<string> { public DescriptionIcon() : base("DescriptionIcon") { } }
    public class FolderIcon : ResourceSettingPictures<string> { public FolderIcon() : base("FolderIcon") { } }
    public class FolderOpenIcon : ResourceSettingPictures<string> { public FolderOpenIcon() : base("FolderOpenIcon") { } }
    public class IconIcon : ResourceSettingPictures<string> { public IconIcon() : base("IconIcon") { } }
    public class TypeIcon : ResourceSettingPictures<string> { public TypeIcon() : base("TypeIcon") { } }
    public class BinaryIcon : ResourceSettingPictures<string> { public BinaryIcon() : base("BinaryIcon") { } }
    public class BooleanIcon : ResourceSettingPictures<string> { public BooleanIcon() : base("BooleanIcon") { } }
    public class DateIcon : ResourceSettingPictures<string> { public DateIcon() : base("DateIcon") { } }
    public class GuidIcon : ResourceSettingPictures<string> { public GuidIcon() : base("GuidIcon") { } }
    public class IntegerIcon : ResourceSettingPictures<string> { public IntegerIcon() : base("IntegerIcon") { } }
    public class ListIcon : ResourceSettingPictures<string> { public ListIcon() : base("ListIcon") { } }
    public class NumericIcon : ResourceSettingPictures<string> { public NumericIcon() : base("NumericIcon") { } }
    public class RangeIcon : ResourceSettingPictures<string> { public RangeIcon() : base("RangeIcon") { } }
    public class ReferenceIcon : ResourceSettingPictures<string> { public ReferenceIcon() : base("ReferenceIcon") { } }
    public class StringIcon : ResourceSettingPictures<string> { public StringIcon() : base("StringIcon") { } }
    public class LoginInvalidIcon : ResourceSettingPictures<string> { public LoginInvalidIcon() : base("LoginInvalidIcon") { } }
    public class LoginLengthIcon : ResourceSettingPictures<string> { public LoginLengthIcon() : base("LoginLengthIcon") { } }
    public class MetaCreateTypeIcon : ResourceSettingPictures<string> { public MetaCreateTypeIcon() : base("MetaCreateTypeIcon") { } }
    public class MetaImplementInterfaceIcon : ResourceSettingPictures<string> { public MetaImplementInterfaceIcon() : base("MetaImplementInterfaceIcon") { } }
    public class NullCodeIcon : ResourceSettingPictures<string> { public NullCodeIcon() : base("NullCodeIcon") { } }
    public class NullLoginIcon : ResourceSettingPictures<string> { public NullLoginIcon() : base("NullLoginIcon") { } }
    public class NullIconIcon : ResourceSettingPictures<string> { public NullIconIcon() : base("NullIconIcon") { } }
    public class PasswordConfirmationInvalidIcon : ResourceSettingPictures<string> { public PasswordConfirmationInvalidIcon() : base("PasswordConfirmationInvalidIcon") { } }
    public class PasswordNewInvalidIcon : ResourceSettingPictures<string> { public PasswordNewInvalidIcon() : base("PasswordNewInvalidIcon") { } }
    public class ConfigurationIcon : ResourceSettingPictures<string> { public ConfigurationIcon() : base("ConfigurationIcon") { } }
    public class ConstantIcon : ResourceSettingPictures<string> { public ConstantIcon() : base("ConstantIcon") { } }
    public class ContextIcon : ResourceSettingPictures<string> { public ContextIcon() : base("ContextIcon") { } }
    public class PreviousIcon : ResourceSettingPictures<string> { public PreviousIcon() : base("PreviousIcon") { } }
    public class FullIcon : ResourceSettingPictures<string> { public FullIcon() : base("FullIcon") { } }
    public class NoneIcon : ResourceSettingPictures<string> { public NoneIcon() : base("NoneIcon") { } }
    public class ReadIcon : ResourceSettingPictures<string> { public ReadIcon() : base("ReadIcon") { } }
    public class LikeLoginIcon : ResourceSettingPictures<string> { public LikeLoginIcon() : base("LikeLoginIcon") { } }
    public class NoDigitIcon : ResourceSettingPictures<string> { public NoDigitIcon() : base("NoDigitIcon") { } }
    public class NoLowerIcon : ResourceSettingPictures<string> { public NoLowerIcon() : base("NoLowerIcon") { } }
    public class NoSymbolIcon : ResourceSettingPictures<string> { public NoSymbolIcon() : base("NoSymbolIcon") { } }
    public class NoUpperIcon : ResourceSettingPictures<string> { public NoUpperIcon() : base("NoUpperIcon") { } }
    public class SmallLengthIcon : ResourceSettingPictures<string> { public SmallLengthIcon() : base("SmallLengthIcon") { } }
    public class StrengthIcon : ResourceSettingPictures<string> { public StrengthIcon() : base("StrengthIcon") { } }
    public class DelIcon : ResourceSettingPictures<string> { public DelIcon() : base("DelIcon") { } }
    public class NewIcon : ResourceSettingPictures<string> { public NewIcon() : base("NewIcon") { } }
    public class ProcIcon : ResourceSettingPictures<string> { public ProcIcon() : base("ProcIcon") { } }
    public class AppMemoryIcon : ResourceSettingPictures<string> { public AppMemoryIcon() : base("AppMemoryIcon") { } }
    public class ComputerIconIcon : ResourceSettingPictures<string> { public ComputerIconIcon() : base("ComputerIconIcon") { } }
    public class DefaultUserIcon : ResourceSettingPictures<string> { public DefaultUserIcon() : base("DefaultUserIcon") { } }
    public class DotNetVersionIcon : ResourceSettingPictures<string> { public DotNetVersionIcon() : base("DotNetVersionIcon") { } }
    public class DrivesIcon : ResourceSettingPictures<string> { public DrivesIcon() : base("DrivesIcon") { } }
    public class OperatingSystemIcon : ResourceSettingPictures<string> { public OperatingSystemIcon() : base("OperatingSystemIcon") { } }
    public class ProcessorCountIcon : ResourceSettingPictures<string> { public ProcessorCountIcon() : base("ProcessorCountIcon") { } }
    public class TotalMemoryIcon : ResourceSettingPictures<string> { public TotalMemoryIcon() : base("TotalMemoryIcon") { } }
    public class WindowsUserIcon : ResourceSettingPictures<string> { public WindowsUserIcon() : base("WindowsUserIcon") { } }
    public class StartAppLangIcon : ResourceSettingPictures<string> { public StartAppLangIcon() : base("StartAppLangIcon") { } }
    public class StartCmdArgsIcon : ResourceSettingPictures<string> { public StartCmdArgsIcon() : base("StartCmdArgsIcon") { } }
    public class StartCmdPoolIcon : ResourceSettingPictures<string> { public StartCmdPoolIcon() : base("StartCmdPoolIcon") { } }
    public class StartDIIcon : ResourceSettingPictures<string> { public StartDIIcon() : base("StartDIIcon") { } }
    public class StartEndIcon : ResourceSettingPictures<string> { public StartEndIcon() : base("StartEndIcon") { } }
    public class StartNLogBaseIcon : ResourceSettingPictures<string> { public StartNLogBaseIcon() : base("StartNLogBaseIcon") { } }
    public class StartNLogTypedIcon : ResourceSettingPictures<string> { public StartNLogTypedIcon() : base("StartNLogTypedIcon") { } }
    public class StartReadConfIcon : ResourceSettingPictures<string> { public StartReadConfIcon() : base("StartReadConfIcon") { } }
    public class StartResIcon : ResourceSettingPictures<string> { public StartResIcon() : base("StartResIcon") { } }
    public class StartTraceIcon : ResourceSettingPictures<string> { public StartTraceIcon() : base("StartTraceIcon") { } }

}