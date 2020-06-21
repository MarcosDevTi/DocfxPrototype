using FluentValidation.Results;
using System.Threading.Tasks;
// ReSharper disable All

namespace Env.Commun.Core
{
    /// <summary>
    /// L'interface de demande / réponse gère à la fois les scénarios de commande et de requête.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         MediatR est une bibliothèque à faible ambition qui tente de résoudre un problème simple - découplant l'envoi en cours de messages de la gestion des messages.
    ///         Multiplateforme, prenant en charge .NET Framework 4.6.1 et netstandard2.0 => <see cref="Evenement"/>
    ///     </para>
    ///     <para>
    ///         MediatR n'a pas de dépendances.
    ///         Vous devrez configurer un seul délégué d'usine, utilisé pour instancier tous les gestionnaires, comportements de pipeline et pré / post-processeurs.
    ///     </para>
    /// </remarks>
    public interface IMediatorHandler
    {
        /// <summary>
        /// This sample shows how to specify the <see cref="Evenement"/> constructor as a cref attribute.
        /// </summary>
        /// <param name="evenement">Événement à publier</param>
        /// <returns>A Task representing a point's location, in the form (x,y),
        ///    without any leading, trailing, or embedded whitespace.</returns>
        /// <typeparam name="T">Type générique événement</typeparam>
        Task PublierEvenement<T>(T evenement) where T : Evenement;

        /// <summary>
        /// The GetZero method.
        /// </summary>
        /// <example>
        /// This sample shows how to call the <see cref="Evenement"/> method.
        /// <code>
        /// public class Evenement
        /// {
        ///     static int Main()
        ///     {
        ///         return GetZero();
        ///     }
        /// }
        /// </code>
        /// </example>
        Task<ValidationResult> SoumettreCommand<T>(T command) where T : Command;
    }
}