using System.Drawing;
using System.Resources;
using Jetproger.Tools.AppConfig;
using Jetproger.Tools.Convert.Bases;
using Jetproger.Tools.Convert.Converts;
using Jetproger.Tools.Convert.Settings;

namespace Jetproger.Tools.Convert.Settings
{  
    public abstract class ResourceDescriptionSetting : Setting
    {
        private static readonly ResourceManager Rm = t<ResourceManager>.one(() => f.sys.resxof("ResourceDescriptions", k<ResourceAssembly>.key()));
        public static void Parse() { Rm.IgnoreCase = true; }

        protected ResourceDescriptionSetting(string defaultValue)
        {
            var key = GetType().Name.ToLower();
            Value = GetValue(key, defaultValue);
            IsDeclared = GetDeclared(key, defaultValue);
        }

        private static bool GetDeclared(string key, string defaultValue)
        {
            if (Rm == null) return false;
            var value = f.sys.valueof(Rm, key.ToLower());
            return value != null;
        }

        private static string GetValue(string key, string defaultValue)
        {
            if (Rm == null) return defaultValue;
            var value = f.sys.valueof(Rm, key.ToLower());
            return value ?? defaultValue;
        }
    }

    public abstract class ResourceShortcutSetting : Setting
    {
        private static readonly ResourceManager Rm = t<ResourceManager>.one(() => f.sys.resxof("ResourceShortcuts", k<ResourceAssembly>.key()));
        public static void Parse() { Rm.IgnoreCase = true; }

        protected ResourceShortcutSetting(string defaultValue)
        {
            var key = GetType().Name.ToLower();
            Value = GetValue(key, defaultValue);
            IsDeclared = GetDeclared(key, defaultValue);
        }

        private static bool GetDeclared(string key, string defaultValue)
        {
            if (Rm == null) return false;
            var value = f.sys.valueof(Rm, key.ToLower());
            return value != null;
        }

        private static string GetValue(string key, string defaultValue)
        {
            if (Rm == null) return defaultValue;
            var value = f.sys.valueof(Rm, key.ToLower());
            return value ?? defaultValue;
        }
    }

    public abstract class ResourceSpecifySetting : Setting
    {
        private static readonly ResourceManager Rm = t<ResourceManager>.one(() => f.sys.resxof("ResourceSpecifies", k<ResourceAssembly>.key()));
        public static void Parse() { Rm.IgnoreCase = true; }

        protected ResourceSpecifySetting(string defaultValue)
        {
            var key = GetType().Name.ToLower();
            Value = GetValue(key, defaultValue);
            IsDeclared = GetDeclared(key, defaultValue);
        }

        private static bool GetDeclared(string key, string defaultValue)
        {
            if (Rm == null) return false;
            var value = f.sys.valueof(Rm, key.ToLower());
            return value != null;
        }

        private static string GetValue(string key, string defaultValue)
        {
            if (Rm == null) return defaultValue;
            var value = f.sys.valueof(Rm, key.ToLower());
            return value ?? defaultValue;
        }
    }

    public abstract class ResourcePictureSetting : Setting
    {
        private static readonly ResourceManager Rm = t<ResourceManager>.one(() => f.sys.resxof("ResourcePictures", k<ResourceAssembly>.key()));
        public static void Parse() { Rm.IgnoreCase = true; }

        protected ResourcePictureSetting(string defaultValue)
        {
            var key = GetType().Name.ToLower();
            Value = GetValue(key, defaultValue);
            IsDeclared = GetDeclared(key, defaultValue);
        }

        private static bool GetDeclared(string key, string defaultValue)
        {
            if (Rm == null) return false;
            var value = f.sys.valueof(Rm, key.ToLower());
            return value != null;
        }

        private static string GetValue(string key, string defaultValue)
        {
            if (Rm == null) return defaultValue;
            var value = f.sys.valueof(Rm, key.ToLower());
            return value ?? defaultValue;
        }
    }

    public abstract class ResourceNameSetting : Setting
    {
        private static readonly ResourceManager Rm = t<ResourceManager>.one(() => f.sys.resxof("ResourceNames", k<ResourceAssembly>.key()));
        public static void Parse() { Rm.IgnoreCase = true; }

        protected ResourceNameSetting(string defaultValue)
        {
            var key = GetType().Name.ToLower();
            Value = GetValue(key, defaultValue);
            IsDeclared = GetDeclared(key, defaultValue);
        }

        private static bool GetDeclared(string key, string defaultValue)
        {
            if (Rm == null) return false;
            var value = f.sys.valueof(Rm, key.ToLower());
            return value != null;
        }

        private static string GetValue(string key, string defaultValue)
        {
            if (Rm == null) return defaultValue;
            var value = f.sys.valueof(Rm, key.ToLower());
            return value ?? defaultValue;
        }
    }
}

namespace Jetproger.Tools.AppConfig
{
    public class ResourceAssembly : ConfigSetting { public ResourceAssembly() : base("Jetproger.Tools.Resource") { } }
}

namespace Jetproger.Tools.AppResource
{
    #region Names

    public class AppConfigAppHostName : ResourceNameSetting { public AppConfigAppHostName() : base("AppConfigAppHostName") { } }
    public class AboutName : ResourceNameSetting { public AboutName() : base("AboutName") { } }
    public class AskExitName : ResourceNameSetting { public AskExitName() : base("AskExitName") { } }
    public class AskTitleName : ResourceNameSetting { public AskTitleName() : base("AskTitleName") { } }
    public class CancelName : ResourceNameSetting { public CancelName() : base("CancelName") { } }
    public class CaptionName : ResourceNameSetting { public CaptionName() : base("CaptionName") { } }
    public class DialogName : ResourceNameSetting { public DialogName() : base("DialogName") { } }
    public class ErrorTitleName : ResourceNameSetting { public ErrorTitleName() : base("ErrorTitleName") { } }
    public class InfoTitleName : ResourceNameSetting { public InfoTitleName() : base("InfoTitleName") { } }
    public class InfoTraceName : ResourceNameSetting { public InfoTraceName() : base("InfoTraceName") { } }
    public class LoginName : ResourceNameSetting { public LoginName() : base("LoginName") { } }
    public class MessageLoadingName : ResourceNameSetting { public MessageLoadingName() : base("MessageLoadingName") { } }
    public class MessageNonConnectName : ResourceNameSetting { public MessageNonConnectName() : base("MessageNonConnectName") { } }
    public class MessageNonPasswordName : ResourceNameSetting { public MessageNonPasswordName() : base("MessageNonPasswordName") { } }
    public class MessageStartName : ResourceNameSetting { public MessageStartName() : base("MessageStartName") { } }
    public class MessageSuccessConnectName : ResourceNameSetting { public MessageSuccessConnectName() : base("MessageSuccessConnectName") { } }
    public class MessageSuccessPasswordName : ResourceNameSetting { public MessageSuccessPasswordName() : base("MessageSuccessPasswordName") { } }
    public class OkName : ResourceNameSetting { public OkName() : base("OkName") { } }
    public class PasswordName : ResourceNameSetting { public PasswordName() : base("PasswordName") { } }
    public class PasswordChangeName : ResourceNameSetting { public PasswordChangeName() : base("PasswordChangeName") { } }
    public class PasswordNewName : ResourceNameSetting { public PasswordNewName() : base("PasswordNewName") { } }
    public class PasswordOldName : ResourceNameSetting { public PasswordOldName() : base("PasswordOldName") { } }
    public class UserName : ResourceNameSetting { public UserName() : base("UserName") { } }
    public class AddName : ResourceNameSetting { public AddName() : base("AddName") { } }
    public class CopyName : ResourceNameSetting { public CopyName() : base("CopyName") { } }
    public class EditName : ResourceNameSetting { public EditName() : base("EditName") { } }
    public class RefreshName : ResourceNameSetting { public RefreshName() : base("RefreshName") { } }
    public class RemoveName : ResourceNameSetting { public RemoveName() : base("RemoveName") { } }
    public class RestoreName : ResourceNameSetting { public RestoreName() : base("RestoreName") { } }
    public class DescriptionName : ResourceNameSetting { public DescriptionName() : base("DescriptionName") { } }
    public class FolderName : ResourceNameSetting { public FolderName() : base("FolderName") { } }
    public class FolderOpenName : ResourceNameSetting { public FolderOpenName() : base("FolderOpenName") { } }
    public class NameName : ResourceNameSetting { public NameName() : base("NameName") { } }
    public class TypeName : ResourceNameSetting { public TypeName() : base("TypeName") { } }
    public class BinaryName : ResourceNameSetting { public BinaryName() : base("BinaryName") { } }
    public class BooleanName : ResourceNameSetting { public BooleanName() : base("BooleanName") { } }
    public class DateName : ResourceNameSetting { public DateName() : base("DateName") { } }
    public class GuidName : ResourceNameSetting { public GuidName() : base("GuidName") { } }
    public class IntegerName : ResourceNameSetting { public IntegerName() : base("IntegerName") { } }
    public class ListName : ResourceNameSetting { public ListName() : base("ListName") { } }
    public class NumericName : ResourceNameSetting { public NumericName() : base("NumericName") { } }
    public class RangeName : ResourceNameSetting { public RangeName() : base("RangeName") { } }
    public class ReferenceName : ResourceNameSetting { public ReferenceName() : base("ReferenceName") { } }
    public class StringName : ResourceNameSetting { public StringName() : base("StringName") { } }
    public class LoginInvalidName : ResourceNameSetting { public LoginInvalidName() : base("LoginInvalidName") { } }
    public class LoginLengthName : ResourceNameSetting { public LoginLengthName() : base("LoginLengthName") { } }
    public class MetaCreateTypeName : ResourceNameSetting { public MetaCreateTypeName() : base("MetaCreateTypeName") { } }
    public class MetaImplementInterfaceName : ResourceNameSetting { public MetaImplementInterfaceName() : base("MetaImplementInterfaceName") { } }
    public class NullCodeName : ResourceNameSetting { public NullCodeName() : base("NullCodeName") { } }
    public class NullLoginName : ResourceNameSetting { public NullLoginName() : base("NullLoginName") { } }
    public class NullNameName : ResourceNameSetting { public NullNameName() : base("NullNameName") { } }
    public class PasswordConfirmationInvalidName : ResourceNameSetting { public PasswordConfirmationInvalidName() : base("PasswordConfirmationInvalidName") { } }
    public class PasswordNewInvalidName : ResourceNameSetting { public PasswordNewInvalidName() : base("PasswordNewInvalidName") { } }
    public class ConfigurationName : ResourceNameSetting { public ConfigurationName() : base("ConfigurationName") { } }
    public class ConstantName : ResourceNameSetting { public ConstantName() : base("ConstantName") { } }
    public class ContextName : ResourceNameSetting { public ContextName() : base("ContextName") { } }
    public class PreviousName : ResourceNameSetting { public PreviousName() : base("PreviousName") { } }
    public class FullName : ResourceNameSetting { public FullName() : base("FullName") { } }
    public class NoneName : ResourceNameSetting { public NoneName() : base("NoneName") { } }
    public class ReadName : ResourceNameSetting { public ReadName() : base("ReadName") { } }
    public class LikeLoginName : ResourceNameSetting { public LikeLoginName() : base("LikeLoginName") { } }
    public class NoDigitName : ResourceNameSetting { public NoDigitName() : base("NoDigitName") { } }
    public class NoLowerName : ResourceNameSetting { public NoLowerName() : base("NoLowerName") { } }
    public class NoSymbolName : ResourceNameSetting { public NoSymbolName() : base("NoSymbolName") { } }
    public class NoUpperName : ResourceNameSetting { public NoUpperName() : base("NoUpperName") { } }
    public class SmallLengthName : ResourceNameSetting { public SmallLengthName() : base("SmallLengthName") { } }
    public class StrengthName : ResourceNameSetting { public StrengthName() : base("StrengthName") { } }
    public class DelName : ResourceNameSetting { public DelName() : base("DelName") { } }
    public class NewName : ResourceNameSetting { public NewName() : base("NewName") { } }
    public class ProcName : ResourceNameSetting { public ProcName() : base("ProcName") { } }
    public class AppMemoryName : ResourceNameSetting { public AppMemoryName() : base("AppMemoryName") { } }
    public class ComputerNameName : ResourceNameSetting { public ComputerNameName() : base("ComputerNameName") { } }
    public class DefaultUserName : ResourceNameSetting { public DefaultUserName() : base("DefaultUserName") { } }
    public class DotNetVersionName : ResourceNameSetting { public DotNetVersionName() : base("DotNetVersionName") { } }
    public class DrivesName : ResourceNameSetting { public DrivesName() : base("DrivesName") { } }
    public class OperatingSystemName : ResourceNameSetting { public OperatingSystemName() : base("OperatingSystemName") { } }
    public class ProcessorCountName : ResourceNameSetting { public ProcessorCountName() : base("ProcessorCountName") { } }
    public class TotalMemoryName : ResourceNameSetting { public TotalMemoryName() : base("TotalMemoryName") { } }
    public class WindowsUserName : ResourceNameSetting { public WindowsUserName() : base("WindowsUserName") { } }
    public class StartAppLangName : ResourceNameSetting { public StartAppLangName() : base("StartAppLangName") { } }
    public class StartCmdArgsName : ResourceNameSetting { public StartCmdArgsName() : base("StartCmdArgsName") { } }
    public class StartCmdPoolName : ResourceNameSetting { public StartCmdPoolName() : base("StartCmdPoolName") { } }
    public class StartDIName : ResourceNameSetting { public StartDIName() : base("StartDIName") { } }
    public class StartEndName : ResourceNameSetting { public StartEndName() : base("StartEndName") { } }
    public class StartNLogBaseName : ResourceNameSetting { public StartNLogBaseName() : base("StartNLogBaseName") { } }
    public class StartNLogTypedName : ResourceNameSetting { public StartNLogTypedName() : base("StartNLogTypedName") { } }
    public class StartReadConfName : ResourceNameSetting { public StartReadConfName() : base("StartReadConfName") { } }
    public class StartResName : ResourceNameSetting { public StartResName() : base("StartResName") { } }
    public class StartTraceName : ResourceNameSetting { public StartTraceName() : base("StartTraceName") { } }
    public class TypeNotFoundName : ResourceNameSetting { public TypeNotFoundName() : base("TypeNotFoundName {0}") { } }
    public class TypeNotSubtypeName : ResourceNameSetting { public TypeNotSubtypeName() : base("TypeNotSubtypeName {0} {1}") { } }
    public class CommandNotDefinedName : ResourceNameSetting { public CommandNotDefinedName() : base("CommandNotDefinedName {0}") { } }
    public class MssqlCommandName : ResourceNameSetting { public MssqlCommandName() : base("MssqlCommandName {0} {1}") { } }
    public class CertificateStoreName : ResourceNameSetting { public CertificateStoreName() : base("CertificateStoreName {0} {1}") { } }
    public class CertificateKeyName : ResourceNameSetting { public CertificateKeyName() : base("CertificateKeyName {0} {1}") { } }
    public class AppPortNotSpecifiedName : ResourceNameSetting { public AppPortNotSpecifiedName() : base("AppPortNotSpecifiedName") { } }
    public class ContainerNotFoundName : ResourceNameSetting { public ContainerNotFoundName() : base("ContainerNotFoundName {0}") { } }

    #endregion

    #region Descriptions

    public class AskExitNote : ResourceDescriptionSetting { public AskExitNote() : base("AskExitNote") { } }
    public class AskTitleNote : ResourceDescriptionSetting { public AskTitleNote() : base("AskTitleNote") { } }
    public class CancelNote : ResourceDescriptionSetting { public CancelNote() : base("CancelNote") { } }
    public class CaptionNote : ResourceDescriptionSetting { public CaptionNote() : base("CaptionNote") { } }
    public class DialogNote : ResourceDescriptionSetting { public DialogNote() : base("DialogNote") { } }
    public class ErrorTitleNote : ResourceDescriptionSetting { public ErrorTitleNote() : base("ErrorTitleNote") { } }
    public class InfoTitleNote : ResourceDescriptionSetting { public InfoTitleNote() : base("InfoTitleNote") { } }
    public class InfoTraceNote : ResourceDescriptionSetting { public InfoTraceNote() : base("InfoTraceNote") { } }
    public class LoginNote : ResourceDescriptionSetting { public LoginNote() : base("LoginNote") { } }
    public class MessageLoadingNote : ResourceDescriptionSetting { public MessageLoadingNote() : base("MessageLoadingNote") { } }
    public class MessageNonConnectNote : ResourceDescriptionSetting { public MessageNonConnectNote() : base("MessageNonConnectNote") { } }
    public class MessageNonPasswordNote : ResourceDescriptionSetting { public MessageNonPasswordNote() : base("MessageNonPasswordNote") { } }
    public class MessageStartNote : ResourceDescriptionSetting { public MessageStartNote() : base("MessageStartNote") { } }
    public class MessageSuccessConnectNote : ResourceDescriptionSetting { public MessageSuccessConnectNote() : base("MessageSuccessConnectNote") { } }
    public class MessageSuccessPasswordNote : ResourceDescriptionSetting { public MessageSuccessPasswordNote() : base("MessageSuccessPasswordNote") { } }
    public class OkNote : ResourceDescriptionSetting { public OkNote() : base("OkNote") { } }
    public class PasswordNote : ResourceDescriptionSetting { public PasswordNote() : base("PasswordNote") { } }
    public class PasswordChangeNote : ResourceDescriptionSetting { public PasswordChangeNote() : base("PasswordChangeNote") { } }
    public class PasswordNewNote : ResourceDescriptionSetting { public PasswordNewNote() : base("PasswordNewNote") { } }
    public class PasswordOldNote : ResourceDescriptionSetting { public PasswordOldNote() : base("PasswordOldNote") { } }
    public class UserNote : ResourceDescriptionSetting { public UserNote() : base("UserNote") { } }
    public class AddNote : ResourceDescriptionSetting { public AddNote() : base("AddNote") { } }
    public class CopyNote : ResourceDescriptionSetting { public CopyNote() : base("CopyNote") { } }
    public class EditNote : ResourceDescriptionSetting { public EditNote() : base("EditNote") { } }
    public class RefreshNote : ResourceDescriptionSetting { public RefreshNote() : base("RefreshNote") { } }
    public class RemoveNote : ResourceDescriptionSetting { public RemoveNote() : base("RemoveNote") { } }
    public class RestoreNote : ResourceDescriptionSetting { public RestoreNote() : base("RestoreNote") { } }
    public class DescriptionNote : ResourceDescriptionSetting { public DescriptionNote() : base("DescriptionNote") { } }
    public class FolderNote : ResourceDescriptionSetting { public FolderNote() : base("FolderNote") { } }
    public class FolderOpenNote : ResourceDescriptionSetting { public FolderOpenNote() : base("FolderOpenNote") { } }
    public class NoteNote : ResourceDescriptionSetting { public NoteNote() : base("NoteNote") { } }
    public class TypeNote : ResourceDescriptionSetting { public TypeNote() : base("TypeNote") { } }
    public class BinaryNote : ResourceDescriptionSetting { public BinaryNote() : base("BinaryNote") { } }
    public class BooleanNote : ResourceDescriptionSetting { public BooleanNote() : base("BooleanNote") { } }
    public class DateNote : ResourceDescriptionSetting { public DateNote() : base("DateNote") { } }
    public class GuidNote : ResourceDescriptionSetting { public GuidNote() : base("GuidNote") { } }
    public class IntegerNote : ResourceDescriptionSetting { public IntegerNote() : base("IntegerNote") { } }
    public class ListNote : ResourceDescriptionSetting { public ListNote() : base("ListNote") { } }
    public class NumericNote : ResourceDescriptionSetting { public NumericNote() : base("NumericNote") { } }
    public class RangeNote : ResourceDescriptionSetting { public RangeNote() : base("RangeNote") { } }
    public class ReferenceNote : ResourceDescriptionSetting { public ReferenceNote() : base("ReferenceNote") { } }
    public class StringNote : ResourceDescriptionSetting { public StringNote() : base("StringNote") { } }
    public class LoginInvalidNote : ResourceDescriptionSetting { public LoginInvalidNote() : base("LoginInvalidNote") { } }
    public class LoginLengthNote : ResourceDescriptionSetting { public LoginLengthNote() : base("LoginLengthNote") { } }
    public class MetaCreateTypeNote : ResourceDescriptionSetting { public MetaCreateTypeNote() : base("MetaCreateTypeNote") { } }
    public class MetaImplementInterfaceNote : ResourceDescriptionSetting { public MetaImplementInterfaceNote() : base("MetaImplementInterfaceNote") { } }
    public class NullCodeNote : ResourceDescriptionSetting { public NullCodeNote() : base("NullCodeNote") { } }
    public class NullLoginNote : ResourceDescriptionSetting { public NullLoginNote() : base("NullLoginNote") { } }
    public class NullNoteNote : ResourceDescriptionSetting { public NullNoteNote() : base("NullNoteNote") { } }
    public class PasswordConfirmationInvalidNote : ResourceDescriptionSetting { public PasswordConfirmationInvalidNote() : base("PasswordConfirmationInvalidNote") { } }
    public class PasswordNewInvalidNote : ResourceDescriptionSetting { public PasswordNewInvalidNote() : base("PasswordNewInvalidNote") { } }
    public class ConfigurationNote : ResourceDescriptionSetting { public ConfigurationNote() : base("ConfigurationNote") { } }
    public class ConstantNote : ResourceDescriptionSetting { public ConstantNote() : base("ConstantNote") { } }
    public class ContextNote : ResourceDescriptionSetting { public ContextNote() : base("ContextNote") { } }
    public class PreviousNote : ResourceDescriptionSetting { public PreviousNote() : base("PreviousNote") { } }
    public class FullNote : ResourceDescriptionSetting { public FullNote() : base("FullNote") { } }
    public class NoneNote : ResourceDescriptionSetting { public NoneNote() : base("NoneNote") { } }
    public class ReadNote : ResourceDescriptionSetting { public ReadNote() : base("ReadNote") { } }
    public class LikeLoginNote : ResourceDescriptionSetting { public LikeLoginNote() : base("LikeLoginNote") { } }
    public class NoDigitNote : ResourceDescriptionSetting { public NoDigitNote() : base("NoDigitNote") { } }
    public class NoLowerNote : ResourceDescriptionSetting { public NoLowerNote() : base("NoLowerNote") { } }
    public class NoSymbolNote : ResourceDescriptionSetting { public NoSymbolNote() : base("NoSymbolNote") { } }
    public class NoUpperNote : ResourceDescriptionSetting { public NoUpperNote() : base("NoUpperNote") { } }
    public class SmallLengthNote : ResourceDescriptionSetting { public SmallLengthNote() : base("SmallLengthNote") { } }
    public class StrengthNote : ResourceDescriptionSetting { public StrengthNote() : base("StrengthNote") { } }
    public class DelNote : ResourceDescriptionSetting { public DelNote() : base("DelNote") { } }
    public class NewNote : ResourceDescriptionSetting { public NewNote() : base("NewNote") { } }
    public class ProcNote : ResourceDescriptionSetting { public ProcNote() : base("ProcNote") { } }
    public class AppMemoryNote : ResourceDescriptionSetting { public AppMemoryNote() : base("AppMemoryNote") { } }
    public class ComputerNoteNote : ResourceDescriptionSetting { public ComputerNoteNote() : base("ComputerNoteNote") { } }
    public class DefaultUserNote : ResourceDescriptionSetting { public DefaultUserNote() : base("DefaultUserNote") { } }
    public class DotNetVersionNote : ResourceDescriptionSetting { public DotNetVersionNote() : base("DotNetVersionNote") { } }
    public class DrivesNote : ResourceDescriptionSetting { public DrivesNote() : base("DrivesNote") { } }
    public class OperatingSystemNote : ResourceDescriptionSetting { public OperatingSystemNote() : base("OperatingSystemNote") { } }
    public class ProcessorCountNote : ResourceDescriptionSetting { public ProcessorCountNote() : base("ProcessorCountNote") { } }
    public class TotalMemoryNote : ResourceDescriptionSetting { public TotalMemoryNote() : base("TotalMemoryNote") { } }
    public class WindowsUserNote : ResourceDescriptionSetting { public WindowsUserNote() : base("WindowsUserNote") { } }
    public class StartAppLangNote : ResourceDescriptionSetting { public StartAppLangNote() : base("StartAppLangNote") { } }
    public class StartCmdArgsNote : ResourceDescriptionSetting { public StartCmdArgsNote() : base("StartCmdArgsNote") { } }
    public class StartCmdPoolNote : ResourceDescriptionSetting { public StartCmdPoolNote() : base("StartCmdPoolNote") { } }
    public class StartDINote : ResourceDescriptionSetting { public StartDINote() : base("StartDINote") { } }
    public class StartEndNote : ResourceDescriptionSetting { public StartEndNote() : base("StartEndNote") { } }
    public class StartNLogBaseNote : ResourceDescriptionSetting { public StartNLogBaseNote() : base("StartNLogBaseNote") { } }
    public class StartNLogTypedNote : ResourceDescriptionSetting { public StartNLogTypedNote() : base("StartNLogTypedNote") { } }
    public class StartReadConfNote : ResourceDescriptionSetting { public StartReadConfNote() : base("StartReadConfNote") { } }
    public class StartResNote : ResourceDescriptionSetting { public StartResNote() : base("StartResNote") { } }
    public class StartTraceNote : ResourceDescriptionSetting { public StartTraceNote() : base("StartTraceNote") { } }

    #endregion

    #region Shortcuts

    public class AskExitKeys : ResourceShortcutSetting { public AskExitKeys() : base("AskExitKeys") { } }
    public class AskTitleKeys : ResourceShortcutSetting { public AskTitleKeys() : base("AskTitleKeys") { } }
    public class CancelKeys : ResourceShortcutSetting { public CancelKeys() : base("CancelKeys") { } }
    public class CaptionKeys : ResourceShortcutSetting { public CaptionKeys() : base("CaptionKeys") { } }
    public class DialogKeys : ResourceShortcutSetting { public DialogKeys() : base("DialogKeys") { } }
    public class ErrorTitleKeys : ResourceShortcutSetting { public ErrorTitleKeys() : base("ErrorTitleKeys") { } }
    public class InfoTitleKeys : ResourceShortcutSetting { public InfoTitleKeys() : base("InfoTitleKeys") { } }
    public class InfoTraceKeys : ResourceShortcutSetting { public InfoTraceKeys() : base("InfoTraceKeys") { } }
    public class LoginKeys : ResourceShortcutSetting { public LoginKeys() : base("LoginKeys") { } }
    public class MessageLoadingKeys : ResourceShortcutSetting { public MessageLoadingKeys() : base("MessageLoadingKeys") { } }
    public class MessageNonConnectKeys : ResourceShortcutSetting { public MessageNonConnectKeys() : base("MessageNonConnectKeys") { } }
    public class MessageNonPasswordKeys : ResourceShortcutSetting { public MessageNonPasswordKeys() : base("MessageNonPasswordKeys") { } }
    public class MessageStartKeys : ResourceShortcutSetting { public MessageStartKeys() : base("MessageStartKeys") { } }
    public class MessageSuccessConnectKeys : ResourceShortcutSetting { public MessageSuccessConnectKeys() : base("MessageSuccessConnectKeys") { } }
    public class MessageSuccessPasswordKeys : ResourceShortcutSetting { public MessageSuccessPasswordKeys() : base("MessageSuccessPasswordKeys") { } }
    public class OkKeys : ResourceShortcutSetting { public OkKeys() : base("OkKeys") { } }
    public class PasswordKeys : ResourceShortcutSetting { public PasswordKeys() : base("PasswordKeys") { } }
    public class PasswordChangeKeys : ResourceShortcutSetting { public PasswordChangeKeys() : base("PasswordChangeKeys") { } }
    public class PasswordNewKeys : ResourceShortcutSetting { public PasswordNewKeys() : base("PasswordNewKeys") { } }
    public class PasswordOldKeys : ResourceShortcutSetting { public PasswordOldKeys() : base("PasswordOldKeys") { } }
    public class UserKeys : ResourceShortcutSetting { public UserKeys() : base("UserKeys") { } }
    public class AddKeys : ResourceShortcutSetting { public AddKeys() : base("AddKeys") { } }
    public class CopyKeys : ResourceShortcutSetting { public CopyKeys() : base("CopyKeys") { } }
    public class EditKeys : ResourceShortcutSetting { public EditKeys() : base("EditKeys") { } }
    public class RefreshKeys : ResourceShortcutSetting { public RefreshKeys() : base("RefreshKeys") { } }
    public class RemoveKeys : ResourceShortcutSetting { public RemoveKeys() : base("RemoveKeys") { } }
    public class RestoreKeys : ResourceShortcutSetting { public RestoreKeys() : base("RestoreKeys") { } }
    public class DescriptionKeys : ResourceShortcutSetting { public DescriptionKeys() : base("DescriptionKeys") { } }
    public class FolderKeys : ResourceShortcutSetting { public FolderKeys() : base("FolderKeys") { } }
    public class FolderOpenKeys : ResourceShortcutSetting { public FolderOpenKeys() : base("FolderOpenKeys") { } }
    public class KeysKeys : ResourceShortcutSetting { public KeysKeys() : base("KeysKeys") { } }
    public class TypeKeys : ResourceShortcutSetting { public TypeKeys() : base("TypeKeys") { } }
    public class BinaryKeys : ResourceShortcutSetting { public BinaryKeys() : base("BinaryKeys") { } }
    public class BooleanKeys : ResourceShortcutSetting { public BooleanKeys() : base("BooleanKeys") { } }
    public class DateKeys : ResourceShortcutSetting { public DateKeys() : base("DateKeys") { } }
    public class GuidKeys : ResourceShortcutSetting { public GuidKeys() : base("GuidKeys") { } }
    public class IntegerKeys : ResourceShortcutSetting { public IntegerKeys() : base("IntegerKeys") { } }
    public class ListKeys : ResourceShortcutSetting { public ListKeys() : base("ListKeys") { } }
    public class NumericKeys : ResourceShortcutSetting { public NumericKeys() : base("NumericKeys") { } }
    public class RangeKeys : ResourceShortcutSetting { public RangeKeys() : base("RangeKeys") { } }
    public class ReferenceKeys : ResourceShortcutSetting { public ReferenceKeys() : base("ReferenceKeys") { } }
    public class StringKeys : ResourceShortcutSetting { public StringKeys() : base("StringKeys") { } }
    public class LoginInvalidKeys : ResourceShortcutSetting { public LoginInvalidKeys() : base("LoginInvalidKeys") { } }
    public class LoginLengthKeys : ResourceShortcutSetting { public LoginLengthKeys() : base("LoginLengthKeys") { } }
    public class MetaCreateTypeKeys : ResourceShortcutSetting { public MetaCreateTypeKeys() : base("MetaCreateTypeKeys") { } }
    public class MetaImplementInterfaceKeys : ResourceShortcutSetting { public MetaImplementInterfaceKeys() : base("MetaImplementInterfaceKeys") { } }
    public class NullCodeKeys : ResourceShortcutSetting { public NullCodeKeys() : base("NullCodeKeys") { } }
    public class NullLoginKeys : ResourceShortcutSetting { public NullLoginKeys() : base("NullLoginKeys") { } }
    public class NullKeysKeys : ResourceShortcutSetting { public NullKeysKeys() : base("NullKeysKeys") { } }
    public class PasswordConfirmationInvalidKeys : ResourceShortcutSetting { public PasswordConfirmationInvalidKeys() : base("PasswordConfirmationInvalidKeys") { } }
    public class PasswordNewInvalidKeys : ResourceShortcutSetting { public PasswordNewInvalidKeys() : base("PasswordNewInvalidKeys") { } }
    public class ConfigurationKeys : ResourceShortcutSetting { public ConfigurationKeys() : base("ConfigurationKeys") { } }
    public class ConstantKeys : ResourceShortcutSetting { public ConstantKeys() : base("ConstantKeys") { } }
    public class ContextKeys : ResourceShortcutSetting { public ContextKeys() : base("ContextKeys") { } }
    public class PreviousKeys : ResourceShortcutSetting { public PreviousKeys() : base("PreviousKeys") { } }
    public class FullKeys : ResourceShortcutSetting { public FullKeys() : base("FullKeys") { } }
    public class NoneKeys : ResourceShortcutSetting { public NoneKeys() : base("NoneKeys") { } }
    public class ReadKeys : ResourceShortcutSetting { public ReadKeys() : base("ReadKeys") { } }
    public class LikeLoginKeys : ResourceShortcutSetting { public LikeLoginKeys() : base("LikeLoginKeys") { } }
    public class NoDigitKeys : ResourceShortcutSetting { public NoDigitKeys() : base("NoDigitKeys") { } }
    public class NoLowerKeys : ResourceShortcutSetting { public NoLowerKeys() : base("NoLowerKeys") { } }
    public class NoSymbolKeys : ResourceShortcutSetting { public NoSymbolKeys() : base("NoSymbolKeys") { } }
    public class NoUpperKeys : ResourceShortcutSetting { public NoUpperKeys() : base("NoUpperKeys") { } }
    public class SmallLengthKeys : ResourceShortcutSetting { public SmallLengthKeys() : base("SmallLengthKeys") { } }
    public class StrengthKeys : ResourceShortcutSetting { public StrengthKeys() : base("StrengthKeys") { } }
    public class DelKeys : ResourceShortcutSetting { public DelKeys() : base("DelKeys") { } }
    public class NewKeys : ResourceShortcutSetting { public NewKeys() : base("NewKeys") { } }
    public class ProcKeys : ResourceShortcutSetting { public ProcKeys() : base("ProcKeys") { } }
    public class AppMemoryKeys : ResourceShortcutSetting { public AppMemoryKeys() : base("AppMemoryKeys") { } }
    public class ComputerKeysKeys : ResourceShortcutSetting { public ComputerKeysKeys() : base("ComputerKeysKeys") { } }
    public class DefaultUserKeys : ResourceShortcutSetting { public DefaultUserKeys() : base("DefaultUserKeys") { } }
    public class DotNetVersionKeys : ResourceShortcutSetting { public DotNetVersionKeys() : base("DotNetVersionKeys") { } }
    public class DrivesKeys : ResourceShortcutSetting { public DrivesKeys() : base("DrivesKeys") { } }
    public class OperatingSystemKeys : ResourceShortcutSetting { public OperatingSystemKeys() : base("OperatingSystemKeys") { } }
    public class ProcessorCountKeys : ResourceShortcutSetting { public ProcessorCountKeys() : base("ProcessorCountKeys") { } }
    public class TotalMemoryKeys : ResourceShortcutSetting { public TotalMemoryKeys() : base("TotalMemoryKeys") { } }
    public class WindowsUserKeys : ResourceShortcutSetting { public WindowsUserKeys() : base("WindowsUserKeys") { } }
    public class StartAppLangKeys : ResourceShortcutSetting { public StartAppLangKeys() : base("StartAppLangKeys") { } }
    public class StartCmdArgsKeys : ResourceShortcutSetting { public StartCmdArgsKeys() : base("StartCmdArgsKeys") { } }
    public class StartCmdPoolKeys : ResourceShortcutSetting { public StartCmdPoolKeys() : base("StartCmdPoolKeys") { } }
    public class StartDIKeys : ResourceShortcutSetting { public StartDIKeys() : base("StartDIKeys") { } }
    public class StartEndKeys : ResourceShortcutSetting { public StartEndKeys() : base("StartEndKeys") { } }
    public class StartNLogBaseKeys : ResourceShortcutSetting { public StartNLogBaseKeys() : base("StartNLogBaseKeys") { } }
    public class StartNLogTypedKeys : ResourceShortcutSetting { public StartNLogTypedKeys() : base("StartNLogTypedKeys") { } }
    public class StartReadConfKeys : ResourceShortcutSetting { public StartReadConfKeys() : base("StartReadConfKeys") { } }
    public class StartResKeys : ResourceShortcutSetting { public StartResKeys() : base("StartResKeys") { } }
    public class StartTraceKeys : ResourceShortcutSetting { public StartTraceKeys() : base("StartTraceKeys") { } }

    #endregion

    #region Specifies

    public class AskExitSpec : ResourceSpecifySetting { public AskExitSpec() : base("AskExitSpec") { } }
    public class AskTitleSpec : ResourceSpecifySetting { public AskTitleSpec() : base("AskTitleSpec") { } }
    public class CancelSpec : ResourceSpecifySetting { public CancelSpec() : base("CancelSpec") { } }
    public class CaptionSpec : ResourceSpecifySetting { public CaptionSpec() : base("CaptionSpec") { } }
    public class DialogSpec : ResourceSpecifySetting { public DialogSpec() : base("DialogSpec") { } }
    public class ErrorTitleSpec : ResourceSpecifySetting { public ErrorTitleSpec() : base("ErrorTitleSpec") { } }
    public class InfoTitleSpec : ResourceSpecifySetting { public InfoTitleSpec() : base("InfoTitleSpec") { } }
    public class InfoTraceSpec : ResourceSpecifySetting { public InfoTraceSpec() : base("InfoTraceSpec") { } }
    public class LoginSpec : ResourceSpecifySetting { public LoginSpec() : base("LoginSpec") { } }
    public class MessageLoadingSpec : ResourceSpecifySetting { public MessageLoadingSpec() : base("MessageLoadingSpec") { } }
    public class MessageNonConnectSpec : ResourceSpecifySetting { public MessageNonConnectSpec() : base("MessageNonConnectSpec") { } }
    public class MessageNonPasswordSpec : ResourceSpecifySetting { public MessageNonPasswordSpec() : base("MessageNonPasswordSpec") { } }
    public class MessageStartSpec : ResourceSpecifySetting { public MessageStartSpec() : base("MessageStartSpec") { } }
    public class MessageSuccessConnectSpec : ResourceSpecifySetting { public MessageSuccessConnectSpec() : base("MessageSuccessConnectSpec") { } }
    public class MessageSuccessPasswordSpec : ResourceSpecifySetting { public MessageSuccessPasswordSpec() : base("MessageSuccessPasswordSpec") { } }
    public class OkSpec : ResourceSpecifySetting { public OkSpec() : base("OkSpec") { } }
    public class PasswordSpec : ResourceSpecifySetting { public PasswordSpec() : base("PasswordSpec") { } }
    public class PasswordChangeSpec : ResourceSpecifySetting { public PasswordChangeSpec() : base("PasswordChangeSpec") { } }
    public class PasswordNewSpec : ResourceSpecifySetting { public PasswordNewSpec() : base("PasswordNewSpec") { } }
    public class PasswordOldSpec : ResourceSpecifySetting { public PasswordOldSpec() : base("PasswordOldSpec") { } }
    public class UserSpec : ResourceSpecifySetting { public UserSpec() : base("UserSpec") { } }
    public class AddSpec : ResourceSpecifySetting { public AddSpec() : base("AddSpec") { } }
    public class CopySpec : ResourceSpecifySetting { public CopySpec() : base("CopySpec") { } }
    public class EditSpec : ResourceSpecifySetting { public EditSpec() : base("EditSpec") { } }
    public class RefreshSpec : ResourceSpecifySetting { public RefreshSpec() : base("RefreshSpec") { } }
    public class RemoveSpec : ResourceSpecifySetting { public RemoveSpec() : base("RemoveSpec") { } }
    public class RestoreSpec : ResourceSpecifySetting { public RestoreSpec() : base("RestoreSpec") { } }
    public class DescriptionSpec : ResourceSpecifySetting { public DescriptionSpec() : base("DescriptionSpec") { } }
    public class FolderSpec : ResourceSpecifySetting { public FolderSpec() : base("FolderSpec") { } }
    public class FolderOpenSpec : ResourceSpecifySetting { public FolderOpenSpec() : base("FolderOpenSpec") { } }
    public class SpecSpec : ResourceSpecifySetting { public SpecSpec() : base("SpecSpec") { } }
    public class TypeSpec : ResourceSpecifySetting { public TypeSpec() : base("TypeSpec") { } }
    public class BinarySpec : ResourceSpecifySetting { public BinarySpec() : base("BinarySpec") { } }
    public class BooleanSpec : ResourceSpecifySetting { public BooleanSpec() : base("BooleanSpec") { } }
    public class DateSpec : ResourceSpecifySetting { public DateSpec() : base("DateSpec") { } }
    public class GuidSpec : ResourceSpecifySetting { public GuidSpec() : base("GuidSpec") { } }
    public class IntegerSpec : ResourceSpecifySetting { public IntegerSpec() : base("IntegerSpec") { } }
    public class ListSpec : ResourceSpecifySetting { public ListSpec() : base("ListSpec") { } }
    public class NumericSpec : ResourceSpecifySetting { public NumericSpec() : base("NumericSpec") { } }
    public class RangeSpec : ResourceSpecifySetting { public RangeSpec() : base("RangeSpec") { } }
    public class ReferenceSpec : ResourceSpecifySetting { public ReferenceSpec() : base("ReferenceSpec") { } }
    public class StringSpec : ResourceSpecifySetting { public StringSpec() : base("StringSpec") { } }
    public class LoginInvalidSpec : ResourceSpecifySetting { public LoginInvalidSpec() : base("LoginInvalidSpec") { } }
    public class LoginLengthSpec : ResourceSpecifySetting { public LoginLengthSpec() : base("LoginLengthSpec") { } }
    public class MetaCreateTypeSpec : ResourceSpecifySetting { public MetaCreateTypeSpec() : base("MetaCreateTypeSpec") { } }
    public class MetaImplementInterfaceSpec : ResourceSpecifySetting { public MetaImplementInterfaceSpec() : base("MetaImplementInterfaceSpec") { } }
    public class NullCodeSpec : ResourceSpecifySetting { public NullCodeSpec() : base("NullCodeSpec") { } }
    public class NullLoginSpec : ResourceSpecifySetting { public NullLoginSpec() : base("NullLoginSpec") { } }
    public class NullSpecSpec : ResourceSpecifySetting { public NullSpecSpec() : base("NullSpecSpec") { } }
    public class PasswordConfirmationInvalidSpec : ResourceSpecifySetting { public PasswordConfirmationInvalidSpec() : base("PasswordConfirmationInvalidSpec") { } }
    public class PasswordNewInvalidSpec : ResourceSpecifySetting { public PasswordNewInvalidSpec() : base("PasswordNewInvalidSpec") { } }
    public class ConfigurationSpec : ResourceSpecifySetting { public ConfigurationSpec() : base("ConfigurationSpec") { } }
    public class ConstantSpec : ResourceSpecifySetting { public ConstantSpec() : base("ConstantSpec") { } }
    public class ContextSpec : ResourceSpecifySetting { public ContextSpec() : base("ContextSpec") { } }
    public class PreviousSpec : ResourceSpecifySetting { public PreviousSpec() : base("PreviousSpec") { } }
    public class FullSpec : ResourceSpecifySetting { public FullSpec() : base("FullSpec") { } }
    public class NoneSpec : ResourceSpecifySetting { public NoneSpec() : base("NoneSpec") { } }
    public class ReadSpec : ResourceSpecifySetting { public ReadSpec() : base("ReadSpec") { } }
    public class LikeLoginSpec : ResourceSpecifySetting { public LikeLoginSpec() : base("LikeLoginSpec") { } }
    public class NoDigitSpec : ResourceSpecifySetting { public NoDigitSpec() : base("NoDigitSpec") { } }
    public class NoLowerSpec : ResourceSpecifySetting { public NoLowerSpec() : base("NoLowerSpec") { } }
    public class NoSymbolSpec : ResourceSpecifySetting { public NoSymbolSpec() : base("NoSymbolSpec") { } }
    public class NoUpperSpec : ResourceSpecifySetting { public NoUpperSpec() : base("NoUpperSpec") { } }
    public class SmallLengthSpec : ResourceSpecifySetting { public SmallLengthSpec() : base("SmallLengthSpec") { } }
    public class StrengthSpec : ResourceSpecifySetting { public StrengthSpec() : base("StrengthSpec") { } }
    public class DelSpec : ResourceSpecifySetting { public DelSpec() : base("DelSpec") { } }
    public class NewSpec : ResourceSpecifySetting { public NewSpec() : base("NewSpec") { } }
    public class ProcSpec : ResourceSpecifySetting { public ProcSpec() : base("ProcSpec") { } }
    public class AppMemorySpec : ResourceSpecifySetting { public AppMemorySpec() : base("AppMemorySpec") { } }
    public class ComputerSpecSpec : ResourceSpecifySetting { public ComputerSpecSpec() : base("ComputerSpecSpec") { } }
    public class DefaultUserSpec : ResourceSpecifySetting { public DefaultUserSpec() : base("DefaultUserSpec") { } }
    public class DotNetVersionSpec : ResourceSpecifySetting { public DotNetVersionSpec() : base("DotNetVersionSpec") { } }
    public class DrivesSpec : ResourceSpecifySetting { public DrivesSpec() : base("DrivesSpec") { } }
    public class OperatingSystemSpec : ResourceSpecifySetting { public OperatingSystemSpec() : base("OperatingSystemSpec") { } }
    public class ProcessorCountSpec : ResourceSpecifySetting { public ProcessorCountSpec() : base("ProcessorCountSpec") { } }
    public class TotalMemorySpec : ResourceSpecifySetting { public TotalMemorySpec() : base("TotalMemorySpec") { } }
    public class WindowsUserSpec : ResourceSpecifySetting { public WindowsUserSpec() : base("WindowsUserSpec") { } }
    public class StartAppLangSpec : ResourceSpecifySetting { public StartAppLangSpec() : base("StartAppLangSpec") { } }
    public class StartCmdArgsSpec : ResourceSpecifySetting { public StartCmdArgsSpec() : base("StartCmdArgsSpec") { } }
    public class StartCmdPoolSpec : ResourceSpecifySetting { public StartCmdPoolSpec() : base("StartCmdPoolSpec") { } }
    public class StartDISpec : ResourceSpecifySetting { public StartDISpec() : base("StartDISpec") { } }
    public class StartEndSpec : ResourceSpecifySetting { public StartEndSpec() : base("StartEndSpec") { } }
    public class StartNLogBaseSpec : ResourceSpecifySetting { public StartNLogBaseSpec() : base("StartNLogBaseSpec") { } }
    public class StartNLogTypedSpec : ResourceSpecifySetting { public StartNLogTypedSpec() : base("StartNLogTypedSpec") { } }
    public class StartReadConfSpec : ResourceSpecifySetting { public StartReadConfSpec() : base("StartReadConfSpec") { } }
    public class StartResSpec : ResourceSpecifySetting { public StartResSpec() : base("StartResSpec") { } }
    public class StartTraceSpec : ResourceSpecifySetting { public StartTraceSpec() : base("StartTraceSpec") { } }

    #endregion

    #region Pictures 
    public class AskExitIcon : ResourcePictureSetting { public AskExitIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class AskTitleIcon : ResourcePictureSetting { public AskTitleIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class CancelIcon : ResourcePictureSetting { public CancelIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class CaptionIcon : ResourcePictureSetting { public CaptionIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class DialogIcon : ResourcePictureSetting { public DialogIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class ErrorTitleIcon : ResourcePictureSetting { public ErrorTitleIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class InfoTitleIcon : ResourcePictureSetting { public InfoTitleIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class InfoTraceIcon : ResourcePictureSetting { public InfoTraceIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class LoginIcon : ResourcePictureSetting { public LoginIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class MessageLoadingIcon : ResourcePictureSetting { public MessageLoadingIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class MessageNonConnectIcon : ResourcePictureSetting { public MessageNonConnectIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class MessageNonPasswordIcon : ResourcePictureSetting { public MessageNonPasswordIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class MessageStartIcon : ResourcePictureSetting { public MessageStartIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class MessageSuccessConnectIcon : ResourcePictureSetting { public MessageSuccessConnectIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class MessageSuccessPasswordIcon : ResourcePictureSetting { public MessageSuccessPasswordIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class OkIcon : ResourcePictureSetting { public OkIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class PasswordIcon : ResourcePictureSetting { public PasswordIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class PasswordChangeIcon : ResourcePictureSetting { public PasswordChangeIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class PasswordNewIcon : ResourcePictureSetting { public PasswordNewIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class PasswordOldIcon : ResourcePictureSetting { public PasswordOldIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class UserIcon : ResourcePictureSetting { public UserIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class AddIcon : ResourcePictureSetting { public AddIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class CopyIcon : ResourcePictureSetting { public CopyIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class EditIcon : ResourcePictureSetting { public EditIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class RefreshIcon : ResourcePictureSetting { public RefreshIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class RemoveIcon : ResourcePictureSetting { public RemoveIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class RestoreIcon : ResourcePictureSetting { public RestoreIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class DescriptionIcon : ResourcePictureSetting { public DescriptionIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class FolderIcon : ResourcePictureSetting { public FolderIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class FolderOpenIcon : ResourcePictureSetting { public FolderOpenIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class IconIcon : ResourcePictureSetting { public IconIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class TypeIcon : ResourcePictureSetting { public TypeIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class BinaryIcon : ResourcePictureSetting { public BinaryIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class BooleanIcon : ResourcePictureSetting { public BooleanIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class DateIcon : ResourcePictureSetting { public DateIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class GuidIcon : ResourcePictureSetting { public GuidIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class IntegerIcon : ResourcePictureSetting { public IntegerIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class ListIcon : ResourcePictureSetting { public ListIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class NumericIcon : ResourcePictureSetting { public NumericIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class RangeIcon : ResourcePictureSetting { public RangeIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class ReferenceIcon : ResourcePictureSetting { public ReferenceIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class StringIcon : ResourcePictureSetting { public StringIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class LoginInvalidIcon : ResourcePictureSetting { public LoginInvalidIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class LoginLengthIcon : ResourcePictureSetting { public LoginLengthIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class MetaCreateTypeIcon : ResourcePictureSetting { public MetaCreateTypeIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class MetaImplementInterfaceIcon : ResourcePictureSetting { public MetaImplementInterfaceIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class NullCodeIcon : ResourcePictureSetting { public NullCodeIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class NullLoginIcon : ResourcePictureSetting { public NullLoginIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class NullIconIcon : ResourcePictureSetting { public NullIconIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class PasswordConfirmationInvalidIcon : ResourcePictureSetting { public PasswordConfirmationInvalidIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class PasswordNewInvalidIcon : ResourcePictureSetting { public PasswordNewInvalidIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class ConfigurationIcon : ResourcePictureSetting { public ConfigurationIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class ConstantIcon : ResourcePictureSetting { public ConstantIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class ContextIcon : ResourcePictureSetting { public ContextIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class PreviousIcon : ResourcePictureSetting { public PreviousIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class FullIcon : ResourcePictureSetting { public FullIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class NoneIcon : ResourcePictureSetting { public NoneIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class ReadIcon : ResourcePictureSetting { public ReadIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class LikeLoginIcon : ResourcePictureSetting { public LikeLoginIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class NoDigitIcon : ResourcePictureSetting { public NoDigitIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class NoLowerIcon : ResourcePictureSetting { public NoLowerIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class NoSymbolIcon : ResourcePictureSetting { public NoSymbolIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class NoUpperIcon : ResourcePictureSetting { public NoUpperIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class SmallLengthIcon : ResourcePictureSetting { public SmallLengthIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class StrengthIcon : ResourcePictureSetting { public StrengthIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class DelIcon : ResourcePictureSetting { public DelIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class NewIcon : ResourcePictureSetting { public NewIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class ProcIcon : ResourcePictureSetting { public ProcIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class AppMemoryIcon : ResourcePictureSetting { public AppMemoryIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class ComputerIconIcon : ResourcePictureSetting { public ComputerIconIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class DefaultUserIcon : ResourcePictureSetting { public DefaultUserIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class DotNetVersionIcon : ResourcePictureSetting { public DotNetVersionIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class DrivesIcon : ResourcePictureSetting { public DrivesIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class OperatingSystemIcon : ResourcePictureSetting { public OperatingSystemIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class ProcessorCountIcon : ResourcePictureSetting { public ProcessorCountIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class TotalMemoryIcon : ResourcePictureSetting { public TotalMemoryIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class WindowsUserIcon : ResourcePictureSetting { public WindowsUserIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class StartAppLangIcon : ResourcePictureSetting { public StartAppLangIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class StartCmdArgsIcon : ResourcePictureSetting { public StartCmdArgsIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class StartCmdPoolIcon : ResourcePictureSetting { public StartCmdPoolIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class StartDIIcon : ResourcePictureSetting { public StartDIIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class StartEndIcon : ResourcePictureSetting { public StartEndIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class StartNLogBaseIcon : ResourcePictureSetting { public StartNLogBaseIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class StartNLogTypedIcon : ResourcePictureSetting { public StartNLogTypedIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class StartReadConfIcon : ResourcePictureSetting { public StartReadConfIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class StartResIcon : ResourcePictureSetting { public StartResIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    public class StartTraceIcon : ResourcePictureSetting { public StartTraceIcon() : base(f.sys.defof<Image>().As<string>()) { } }
    #endregion
}