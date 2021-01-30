using Microsoft.Bot.Builder.AI.Luis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstBot1.Infrastructure.Luis
{
    public interface ILuisService
    {
        LuisRecognizer LuisRecognizer { get; set; }
    }
}
