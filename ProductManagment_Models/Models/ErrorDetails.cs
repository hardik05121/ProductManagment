using System.Text.Json;

namespace ProductManagment_Models.Models;

    public partial class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public override string ToString() 
        { 
            return JsonSerializer.Serialize(this); 
        }
    }
