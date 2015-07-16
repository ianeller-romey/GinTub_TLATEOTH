using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

using FastMapper;

using GinTub;
using GinTub.Services.DataContracts;


namespace GinTub
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            TypeAdapterConfig<Repository.Entities.Noun, WordData>
                .NewConfig()
                .MapFrom(dest => dest.NounId, src => src.Id); ;
            TypeAdapterConfig<Repository.Entities.ParagraphState, ParagraphStateData>
                .NewConfig()
                .MapFrom<IEnumerable<WordData>>
                (
                    dest => dest.Words,
                    src => (from x in Regex.Split(src.Text, "(\\s|\\.|,|;|\\?|!|\")")
                            join n in src.Nouns on x equals n.Text into nx
                            where !string.IsNullOrWhiteSpace(x)
                            from nn in nx.DefaultIfEmpty()
                            select new WordData() { Text = x, NounId = (nn != null) ? (int?)nn.Id : null })
                            .ToList()
                );

            /*
            // Create Json.Net formatter serializing DateTime using the ISO 8601 format
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.Converters.Add(new IsoDateTimeConverter());

            var config = HttpHostConfiguration.Create().Configuration;
            config.OperationHandlerFactory.Formatters.Clear();
            config.OperationHandlerFactory.Formatters.Insert(0, new JsonNetMediaTypeFormatter(serializerSettings));

            var httpServiceFactory = new HttpServiceHostFactory
                                    {
                                        OperationHandlerFactory = config.OperationHandlerFactory,
                                        MessageHandlerFactory = config.MessageHandlerFactory
                                    };
            RouteTable.Routes.Add(new ServiceRoute("Service1", httpServiceFactory, typeof(Service1)));
            */

            RouteTable.Routes.Add(new ServiceRoute("", new WebServiceHostFactory(), typeof(GinTub.Services.GinTubService)));
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}