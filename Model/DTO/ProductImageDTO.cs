using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Model.DTO;

public struct ProductImageDTO
{
    /*[OpenApiProperty(Default = "32698064-986w-98d1-dk8p-ef6587a4oye6", Description = "The id of a product that the image belongs to", Nullable = false)]
    public string ProductId { get; set; }*/ //USE THIS AS THE PATH PARAM FOR UPLOADING AN IMAGE SANDER!!! (in the actual controller)
    public byte[] UploadedProductImageFile { get; set; }
}
