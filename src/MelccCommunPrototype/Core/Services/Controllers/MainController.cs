using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace Env.Commun.Core
{
    [ApiController]
    public abstract class MainController : Controller
    {
        protected ICollection<string> Erreurs = new List<string>();

        protected ActionResult CustomResponse(object result = null)
        {
            if (OperationInvalide())
            {
                return Ok(result);
            }

            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Messages", Erreurs.ToArray() }
            }));
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var erro in erros)
            {
                AjouterErreurTraitement(erro.ErrorMessage);
            }

            return CustomResponse();
        }

        protected ActionResult CustomResponse(ValidationResult validationResult)
        {
            foreach (var erro in validationResult.Errors)
            {
                AjouterErreurTraitement(erro.ErrorMessage);
            }

            return CustomResponse();
        }

        protected bool OperationInvalide()
        {
            return !Erreurs.Any();
        }

        protected void AjouterErreurTraitement(string erro)
        {
            Erreurs.Add(erro);
        }

        protected void ViderErreursTraitement()
        {
            Erreurs.Clear();
        }
    }
}