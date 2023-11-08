using Web.ControllerApi.Template.Models;

namespace Web.ControllerApi.Template.Services.Interfaces;

public interface ILinkService
{
    Link GenerateLink(string endpointName, object? parameters, string rel, string methodName);
}