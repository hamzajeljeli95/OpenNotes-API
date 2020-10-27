using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using AzureFunctions.Extensions.Swashbuckle;
using System.Net.Http;
using OpenNotes_API.Models;
using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.WebJobs.Host;

namespace OpenNotes_API
{
    public static class MainFunctions
    {
        static List<User> UsersList = new List<User>();

        [SwaggerIgnore]
        [FunctionName("Swagger")]
        public static async Task<HttpResponseMessage> Swagger(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Swagger/json")] HttpRequestMessage req,
        [SwashBuckleClient] ISwashBuckleClient swashBuckleClient)
        {
            return swashBuckleClient.CreateSwaggerDocumentResponse(req);
        }

        [SwaggerIgnore]
        [FunctionName("SwaggerUi")]
        public static async Task<HttpResponseMessage> SwaggerUi(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Swagger/ui")] HttpRequestMessage req,
            [SwashBuckleClient] ISwashBuckleClient swashBuckleClient)
        {
            return swashBuckleClient.CreateSwaggerUIResponse(req, "swagger/json");
        }

        [FunctionName("Login")]
        public static async Task<HttpResponseMessage> Login([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "User/Login")][RequestBodyType(typeof(User), "User")]HttpRequestMessage req, TraceWriter log)
        {
            bool isUserCredentialsMatch = false;
            /*
             * This method must return LoginResponse.CanConnect = TRUE along with USERUUID if credentials are valid
             * and return LoginResponse.CanConnect = FALSE along with USERUUID = EMPTY if credentials are invalid
             * Password Hash is created using SHA-256. 
             */
            User user = Converters.DeserializeFromJson<User>(req.Content.ReadAsStringAsync().Result);
            LoginResponse response = new LoginResponse();
            foreach (User u in UsersList)
            {
                if (u.Username.Equals(user.Username) && u.Password.Equals(user.Password))
                {
                    isUserCredentialsMatch = true;
                    response.CanConnect = true;
                    response.UserUUID = u.UUID;
                    break;
                }
            }
            if (isUserCredentialsMatch)
            {
                return req.CreateResponse(HttpStatusCode.OK, response);
            }
            else
            {
                response.CanConnect = false;
                return req.CreateResponse(HttpStatusCode.OK, response);
            }
        }

        [FunctionName("Register")]
        public static async Task<HttpResponseMessage> Register([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "User/Register")][RequestBodyType(typeof(User), "User")] HttpRequestMessage req,TraceWriter log)
        {
            User user = Converters.DeserializeFromJson<User>(req.Content.ReadAsStringAsync().Result);
            bool isUserAlreadyExists = false;
            foreach (User u in UsersList)
            {
                if (u.Username.Equals(user.Username))
                {
                    isUserAlreadyExists = true;
                    break;
                }
            }
            if (isUserAlreadyExists)
            {
                return req.CreateResponse(HttpStatusCode.OK, false);
            }
            else
            {
                UsersList.Add(new User()
                {
                    Username = user.Username,
                    Password = user.Password
                });
                return req.CreateResponse(HttpStatusCode.OK, true);
            }
        }

        [FunctionName("GetNotes")]
        public static async Task<HttpResponseMessage> GetNotes([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Note/List")][RequestBodyType(typeof(NoteBodyRequest), "NoteBodyRequest")] HttpRequestMessage req, TraceWriter log)
        {
            NoteBodyRequest notebodyreq = Converters.DeserializeFromJson<NoteBodyRequest>(req.Content.ReadAsStringAsync().Result);
            foreach (User u in UsersList)
            {
                if (u.UUID.Equals(notebodyreq.userUUID))
                {
                    return req.CreateResponse(HttpStatusCode.OK, u.Notes.ToArray());
                    break;
                }
            }
            return req.CreateResponse(HttpStatusCode.OK, new Note[1]);
        }

        [FunctionName("GetNote")]
        public static async Task<HttpResponseMessage> GetNote([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Note/List/{uuid}")] HttpRequestMessage req, string uuid, TraceWriter log)
        {
            /*
             * Return all notes for a specific NoteUUID in URL and UserUUID Passed in BODY Request
             */
            return req.CreateResponse(HttpStatusCode.OK, true);
        }

        [FunctionName("DeleteNote")]
        public static async Task<HttpResponseMessage> DeleteNote([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Note/Delete/{uuid}")] HttpRequestMessage req, string uuid, TraceWriter log)
        {
            /*
             * Delete note for a specific NoteUUID in URL and UserUUID Passed in BODY Request
             */
            return req.CreateResponse(HttpStatusCode.OK, true);
        }

        [FunctionName("EditNote")]
        public static async Task<HttpResponseMessage> EditNote([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Note/Edit/{uuid}")] HttpRequestMessage req, string uuid, TraceWriter log)
        {
            /*
             * Edit note for a specific NoteUUID in URL and UserUUID Passed in BODY Request, receives also new informations in body.
             */
            return req.CreateResponse(HttpStatusCode.OK, true);
        }

        [FunctionName("AddNote")]
        public static async Task<HttpResponseMessage> AddNote([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "Note/Add")][RequestBodyType(typeof(NoteBodyRequest), "NoteBodyRequest")] HttpRequestMessage req, TraceWriter log)
        {
            /*
             * Edit note for a specific NoteUUID in URL and UserUUID Passed in BODY Request, receives also new informations in body.
             */
            NoteBodyRequest notebodyreq = Converters.DeserializeFromJson<NoteBodyRequest>(req.Content.ReadAsStringAsync().Result);
            foreach (User u in UsersList)
            {
                if (u.UUID.Equals(notebodyreq.userUUID))
                {
                    Note n = new Note()
                    {
                        Content = notebodyreq.note.Content,
                        LastEditTimeDate = notebodyreq.note.LastEditTimeDate,
                    };
                    u.Notes.Add(n);
                    return req.CreateResponse(HttpStatusCode.OK, true);
                }
            }
            return req.CreateResponse(HttpStatusCode.OK, false);
        }
    }
}
