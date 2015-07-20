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
using DC = GinTub.Services.DataContracts;


namespace GinTub
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            TypeAdapterConfig<Repository.Entities.Noun, DC.Responses.WordData>
                .NewConfig()
                .MapFrom(dest => dest.NounId, src => src.Id); ;
            TypeAdapterConfig<Repository.Entities.ParagraphState, DC.Responses.ParagraphStateData>
                .NewConfig()
                .MapFrom<IEnumerable<DC.Responses.WordData>>
                (
                    dest => dest.Words,
                    src => (from x in Regex.Split(src.Text, "(\\s|\\.|,|;|\\?|!|\")")
                            join n in src.Nouns on x equals n.Text into nx
                            where !string.IsNullOrWhiteSpace(x)
                            from nn in nx.DefaultIfEmpty()
                            select new DC.Responses.WordData() { Text = x, NounId = (nn != null) ? (int?)nn.Id : null })
                            .ToList()
                );

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