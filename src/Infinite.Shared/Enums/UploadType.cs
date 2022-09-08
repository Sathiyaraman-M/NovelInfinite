using System.ComponentModel;

namespace Infinite.Shared.Enums;

public enum UploadType
{
    [Description(@"Images\Assets")]
    Product,

    [Description(@"Images\ProfilePictures")]
    ProfilePicture,

    [Description(@"Documents")]
    Document
}