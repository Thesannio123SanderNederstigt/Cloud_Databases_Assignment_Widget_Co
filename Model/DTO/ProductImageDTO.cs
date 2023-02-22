using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace Model.DTO;

public struct ProductImageDTO
{
    public byte[] productImage { get; set; }
}