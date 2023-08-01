using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ProductManagment_Models.Models;

[ModelMetadataType(typeof(ErrorDetailsMetadata))]
public partial class ErrorDetails
{

}

public partial class ErrorDetailsMetadata
{
    public int StatusCode { get; set; }
    public string Message { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
