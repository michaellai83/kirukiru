using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using NSwag;
using NSwag.AspNet.Owin;
using NSwag.Generation.Processors.Security;
using Owin;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using testdatamodel.Secret;

[assembly: OwinStartup(typeof(testdatamodel.Startup))]

namespace testdatamodel
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            // 如需如何設定應用程式的詳細資訊，請瀏覽 https://go.microsoft.com/fwlink/?LinkID=316888
            var config = new HttpConfiguration();
            app.UseSwaggerUi3(typeof(Startup).Assembly, settings =>
            {
                //針對RPC-Style WebAPI，指定路由包含Action名稱
                settings.GeneratorSettings.DefaultUrlTemplate =
                    "api/{controller}/{action}/{id?}";
                //可加入客製化調整邏輯
                settings.PostProcess = document =>
                {
                    document.Info.Title = "切切的WebAPI ";
                };

                //加入Api Key定義
                settings.GeneratorSettings.DocumentProcessors.Add(new SecurityDefinitionAppender("Bearer", new OpenApiSecurityScheme()
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    Scheme = "Bearer",
                    Description = "請填入配發之 JWT",
                    In = OpenApiSecurityApiKeyLocation.Header,


                }));
                //REF: https://github.com/RicoSuter/NSwag/issues/1304
                settings.GeneratorSettings.OperationProcessors.Add(new OperationSecurityScopeProcessor("Bearer"));
            });
            app.UseWebApi(config);
            config.MapHttpAttributeRoutes();
            config.EnsureInitialized();

        }

        /// <summary>
        /// 啟用跨域及驗證配置
        /// </summary>
        /// <param name="app"></param>
        private void ConfigureAuth(IAppBuilder app)
        {
            // 建立 OAuth 配置
            var oAuthOptions = new OAuthAuthorizationServerOptions
            {
                Provider = new AuthorizationServerProvider()
            };

            // 啟用 OAuth2 bearer tokens 驗證並加入配置
            app.UseOAuthAuthorizationServer(oAuthOptions);
        }
    }
}
