using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstBot1.Infrastructure.Luis
{
    public class LuisService: ILuisService
    {
        public LuisRecognizer LuisRecognizer { get; set; }

        public LuisService(IConfiguration Config)
        {
            var luisApp = new LuisApplication(
                Config["LUISAppId"],
                Config["LUISApiKey"],
                Config["LUISHostName"]);
            var recognizerOptions = new LuisRecognizerOptionsV3(luisApp)
            {
                PredictionOptions = new Microsoft.Bot.Builder.AI.LuisV3.LuisPredictionOptions()
                {
                    IncludeInstanceData = true
                }

            };
            LuisRecognizer = new LuisRecognizer(recognizerOptions);

        }
    }
}
