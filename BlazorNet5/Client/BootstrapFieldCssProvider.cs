using Microsoft.AspNetCore.Components.Forms;
using System.Linq;

namespace BlazorNet5.Client
{
    public class BootstrapFieldCssProvider : FieldCssClassProvider
    {
        public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
        {
            var isValid = !editContext.GetValidationMessages(fieldIdentifier).Any();

            return "form-control " + (isValid ? "is-valid" : "is-invalid");
        }
    }
}
