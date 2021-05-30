// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EmptyBot v4.11.1

using FirstBot1.Dialogs;
using FirstBot1.Infrastructure.Luis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FirstBot1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // preguntas cual es la diferencia entre recurso de creacion y recurso de preduccion de LUIS?
        public void ConfigureServices(IServiceCollection services)
        {
            //NUMERO 1
            var storage = new AzureBlobStorage(
                Configuration.GetSection("Storage").Value,
                Configuration.GetSection("StorageContainer").Value);
            //-----estado de usuario
            var userState = new UserState(storage);

            services.AddSingleton(userState);
            //-----estado de conversacion 
            var ConversationState = new ConversationState(storage);
            services.AddSingleton(ConversationState);

            services.AddControllers().AddNewtonsoftJson();

            // Create the Bot Framework Adapter with error handling enabled.
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

            //NUMERO 8 
            // Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
            services.AddSingleton<RootDialog>();// registramos dialogo
            services.AddSingleton<ILuisService, LuisService>();
            services.AddTransient<IBot, EmptyBot<RootDialog>>();// pasamos dialogo principal 
            //services.AddScoped();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles()
                .UseStaticFiles()
                .UseWebSockets()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

            // app.UseHttpsRedirection();
        }
    }
}
