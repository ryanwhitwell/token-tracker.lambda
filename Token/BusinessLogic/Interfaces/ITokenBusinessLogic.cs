using System;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Response;
using Amazon.Lambda.Core;
using Token.DataAccess.Interfaces;

namespace Token.BusinessLogic
{
    public interface ITokenBusinessLogic
    {
        Task<SkillResponse> HandleSkillRequest(SkillRequest input, ILambdaContext context);
    }
}