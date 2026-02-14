using H5;
using H5.Core;

namespace H5.Core
{
    public static partial class dom
    {
        [CombinedClass]
        [FormerInterface]
        public class FileSystemHandle : IObject
        {
            public static dom.FileSystemHandle prototype { get; set; }

            public virtual dom.FileSystemHandleKind kind { get; }

            public virtual string name { get; }

            public virtual extern es5.Promise<bool> isSameEntry(dom.FileSystemHandle other);
        }

        [CombinedClass]
        [FormerInterface]
        public class FileSystemFileHandle : dom.FileSystemHandle
        {
            public static dom.FileSystemFileHandle prototype { get; set; }

            public virtual extern es5.Promise<dom.File> getFile();

            public virtual extern es5.Promise<dom.FileSystemWritableFileStream> createWritable();

            public virtual extern es5.Promise<dom.FileSystemWritableFileStream> createWritable(dom.FileSystemCreateWritableOptions options);
        }

        [CombinedClass]
        [FormerInterface]
        public class FileSystemDirectoryHandle : dom.FileSystemHandle
        {
            public static dom.FileSystemDirectoryHandle prototype { get; set; }

            public virtual extern es5.Promise<dom.FileSystemFileHandle> getFileHandle(string name);

            public virtual extern es5.Promise<dom.FileSystemFileHandle> getFileHandle(string name, dom.FileSystemGetFileOptions options);

            public virtual extern es5.Promise<dom.FileSystemDirectoryHandle> getDirectoryHandle(string name);

            public virtual extern es5.Promise<dom.FileSystemDirectoryHandle> getDirectoryHandle(string name, dom.FileSystemGetDirectoryOptions options);

            public virtual extern es5.Promise<H5.Core.Void> removeEntry(string name);

            public virtual extern es5.Promise<H5.Core.Void> removeEntry(string name, dom.FileSystemRemoveOptions options);

            public virtual extern es5.Promise<string[]> resolve(dom.FileSystemHandle possibleDescendant);
        }

        [CombinedClass]
        [FormerInterface]
        public class FileSystemWritableFileStream : dom.WritableStream
        {
            public static dom.FileSystemWritableFileStream prototype { get; set; }

            public virtual extern es5.Promise<H5.Core.Void> write(object data);

            public virtual extern es5.Promise<H5.Core.Void> seek(double position);

            public virtual extern es5.Promise<H5.Core.Void> truncate(double size);
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class FileSystemCreateWritableOptions : IObject
        {
            public bool? keepExistingData { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class FileSystemGetFileOptions : IObject
        {
            public bool? create { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class FileSystemGetDirectoryOptions : IObject
        {
            public bool? create { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class FileSystemRemoveOptions : IObject
        {
            public bool? recursive { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class OpenFilePickerOptions : IObject
        {
            public bool? multiple { get; set; }
            public bool? excludeAcceptAllOption { get; set; }
            public dom.FilePickerAcceptType[] types { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class SaveFilePickerOptions : IObject
        {
            public string suggestedName { get; set; }
            public bool? excludeAcceptAllOption { get; set; }
            public dom.FilePickerAcceptType[] types { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class DirectoryPickerOptions : IObject
        {
            public string id { get; set; }
            public string startIn { get; set; }
            public string mode { get; set; }
        }

        [IgnoreCast]
        [ObjectLiteral]
        [FormerInterface]
        public class FilePickerAcceptType : IObject
        {
            public string description { get; set; }
            public object accept { get; set; } // Record<string, string | string[]>
        }

        [Name("System.String")]
        public class FileSystemHandleKind : LiteralType<string>
        {
            [Template("<self>\"file\"")]
            public static readonly dom.FileSystemHandleKind file;

            [Template("<self>\"directory\"")]
            public static readonly dom.FileSystemHandleKind directory;

            private extern FileSystemHandleKind();

            public static extern implicit operator dom.FileSystemHandleKind(string value);
        }

        [Virtual]
        public abstract class FileSystemHandleTypeConfig : IObject
        {
             public virtual dom.FileSystemHandle prototype { get; set; }
        }

        [Virtual]
        public abstract class FileSystemFileHandleTypeConfig : IObject
        {
             public virtual dom.FileSystemFileHandle prototype { get; set; }
        }

        [Virtual]
        public abstract class FileSystemDirectoryHandleTypeConfig : IObject
        {
             public virtual dom.FileSystemDirectoryHandle prototype { get; set; }
        }

        [Virtual]
        public abstract class FileSystemWritableFileStreamTypeConfig : IObject
        {
             public virtual dom.FileSystemWritableFileStream prototype { get; set; }
        }
    }
}
