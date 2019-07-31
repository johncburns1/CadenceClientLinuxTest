using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CadenceClientLinux.Controllers
{
    
    /// <summary>
    /// <para>
    /// This class acts as an error accumulator for the <see cref="NotificationHubProxy" />class.
    /// Calls made to Azure's <see cref="Microsoft.Azure.NotificationHubs.NotificationHubClient"/> return results
    /// as errors or no errors.  These results accumulate in a <see cref="HubResponse"/> instance and the instance
    /// is returned by <see cref="NotificationHubProxy"/> to the Azure App Notifier <see cref="Controllers.ApiController"/>.
    /// The controller methods then check the returned <see cref="HubResponse"/> for any errors.  If there are no errors 
    /// then the request was successful.  If there are errors, then it was not successful.
    /// </para>
    /// <para>
    /// HubResponse{TResult} extends the default <see cref="HubResponse"/> class and it provides a method of
    /// wrapping a return object in a <see cref="HubResponse"/> instance.
    /// </para>
    /// </summary>
    /// <typeparam name="TResult">The result type.</typeparam>
    public sealed class HubResponse<TResult> : HubResponse
        where TResult : class
    {
        /// <summary>
        /// Result is a generic return value wrapped in a <see cref="HubResponse"/>.  This allows 
        /// various object types to be returned along with a <see cref="HubResponse"/>.
        /// The object type is specified with <see cref="HubResponse{TResult}"/>
        /// </summary>
        public TResult Result { get; set; }

        /// <summary>
        /// HubResponse constructor for a new empty <see cref="HubResponse"/>
        /// </summary>
        public HubResponse()
        {
        }

        /// <summary>
        /// SetAsFailureResponse indicates a forced failure <see cref="HubResponse"/>.
        /// This means that a request did not complete successfully.
        /// </summary>
        /// <returns><see cref="HubResponse{TResult}"/> with <see cref="HubResponse._forcedFailedResponse"/> to true</returns>
        public new HubResponse<TResult> SetAsFailureResponse()
        {
            base.SetAsFailureResponse();
            return this;
        }

        /// <summary>
        /// AddErrorMessage adds a custom error message to a <see cref="HubResponse{TResult}"/> instance.
        /// A single reponse can have as many of these as it wants.
        /// Having any amount of error messages > 0 in a <see cref="HubResponse{TResult}"/> means 
        /// that the request did not complete with success.
        /// </summary>
        /// <param name="errorMessage"> <see cref="string"/> is the custom error message to be added to 
        /// a <see cref="HubResponse{TResult}"/> instance. </param>
        /// <returns><see cref="HubResponse{TResult}"/>  with added error messages in its <see cref="HubResponse.ErrorMessages"/> list</returns>
        public new HubResponse<TResult> AddErrorMessage(string errorMessage)
        {
            base.AddErrorMessage(errorMessage);
            return this;
        }
    }

    /// <summary>
    /// HubResponse class acts as an error accumulator for the <see cref="NotificationHubProxy" />class.
    /// Calls made to Azure's <see cref="Microsoft.Azure.NotificationHubs.NotificationHubClient"/> return results
    /// as errors or no errors.  These results accumulate in a <see cref="HubResponse"/> instance and the instance
    /// is returned by <see cref="NotificationHubProxy"/> to the Azure App Notifier <see cref="Controllers.ApiController"/>.
    /// The controller methods then check the returned <see cref="HubResponse"/> for any errors.  If there are no errors then the request was
    /// successful.  If there are errors, then it was not successful.
    /// </summary>
    public class HubResponse
    {
        private bool _forcedFailedResponse;

        /// <summary>
        /// CompletedWithSuccess checks to see if a <see cref="HubResponse"/> instance has accumulated any 
        /// error messages (via <see cref="HubResponse.AddErrorMessage(string)"/>, and if it does not then
        /// CompletedWithSuccess is true.  Otherwise, the request did not complete with success and 
        /// CompletedWithSuccess is false.
        /// </summary>
        public bool CompletedWithSuccess => !ErrorMessages.Any() && !_forcedFailedResponse;

        /// <summary>
        /// ErrorMessages is a <see cref="HubResponse"/> internal list of accumulated error messages.
        /// These messages are added via <see cref="HubResponse.AddErrorMessage(string)"/>.  Checking this
        /// <see cref="IList{String}"/> for any existing error messages is what <see cref="HubResponse.CompletedWithSuccess"/>
        /// does to evaluate the success of a request returning a <see cref="HubResponse"/>
        /// </summary>
        public IList<string> ErrorMessages { get; private set; }

        /// <summary>
        /// HubResponse constructor that initializes an empty <see cref="List{String}"/> to accumulate error messages. 
        /// </summary>
        public HubResponse()
        {
            ErrorMessages = new List<string>();
        }

        /// <summary>
        /// SetAsFailureResponse indicates a forced failure <see cref="HubResponse"/>.
        /// This means that a request did not complete successfully.
        /// </summary>
        /// <returns><see cref="HubResponse"/> with <see cref="HubResponse._forcedFailedResponse"/> to true</returns>
        public HubResponse SetAsFailureResponse()
        {
            _forcedFailedResponse = true;
            return this;
        }

        /// <summary>
        /// AddErrorMessage adds a custom error message to a <see cref="HubResponse"/> instance.
        /// A single reponse can have as many of these as it wants.
        /// Having any amount of error messages > 0 in a <see cref="HubResponse"/> means 
        /// that the request did not complete with success.
        /// </summary>
        /// <param name="errorMessage"> <see cref="string"/> is the custom error message to be added to 
        /// a <see cref="HubResponse"/> instance.</param>
        /// <returns><see cref="HubResponse"/> with added error messages in its <see cref="HubResponse.ErrorMessages"/> list</returns>
        public HubResponse AddErrorMessage(string errorMessage)
        {
            ErrorMessages.Add(errorMessage);
            return this;
        }

        /// <summary>
        /// FormattedErrorMessage formats any accumulated error messages nicely 
        /// so that they are easily human readable.  If there are any error messages
        /// in a <see cref="HubResponse"/> <see cref="ErrorMessages"/> list, then format them 
        /// nicely to a string.
        /// </summary>
        /// <returns><see cref="string"/> of formatted error messages</returns>
        public string FormattedErrorMessages => ErrorMessages.Any()
            ? ErrorMessages.Aggregate((prev, current) => prev + Environment.NewLine + current)
            : string.Empty;
    }
}
