using FirstBot1.Common.Cards;
using FirstBot1.Infrastructure.Luis;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FirstBot1.Dialogs
{
    public class RootDialog:ComponentDialog
    {
        // aqui se incluyen todos los dialogos que se van a utilizar 
        private readonly ILuisService LuisService;// para invocar al servicio LUIS
        //dialogo principal 
        public RootDialog(ILuisService _LuisService)
        {
            LuisService = _LuisService;
            //metodos secuenciales
            var watherfallSteps = new WaterfallStep[]
            {
                InitialProcess,
                FinalProcess
            };
            // pasamos dialogo con los metodos secuenciales
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), watherfallSteps));
            AddDialog(new CellPhoneBuy.CellPhoneBuyDialog());// se registra el dialogo para poder iniciarlo
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> FinalProcess(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.EndDialogAsync(cancellationToken:cancellationToken);
        }

        private async Task<DialogTurnResult> InitialProcess(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // resultado de luis con lo que se va a mandar
            var LuisResult = await LuisService.LuisRecognizer.RecognizeAsync(stepContext.Context, cancellationToken);
            return await ManageIntentions(stepContext, LuisResult, cancellationToken);
        }

        private async Task<DialogTurnResult> ManageIntentions(WaterfallStepContext stepContext, Microsoft.Bot.Builder.RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            // capturar la mayor intencion con base a lo que escribe al usuario
            var topIntent = luisResult.GetTopScoringIntent();
            switch(topIntent.intent)
            {
                case "Agradecer":
                    await IntentAgradecer(stepContext, luisResult, cancellationToken);
                    break;
                case "Despedir":
                    await IntentDespedir(stepContext, luisResult, cancellationToken);
                    break;
                case "Saludar":
                    await IntentSaludar(stepContext, luisResult, cancellationToken);
                    break;
                case "None":
                    await IntentNone(stepContext, luisResult, cancellationToken);
                    break;
                case "VerOpciones":
                    await IntentVerOpciones(stepContext, luisResult, cancellationToken);
                    break;
                case "Comprar":
                    // AQUI RETORNAMOS POR QUE AL TRABAJAR CON DIALOGOS EXTERNOS Y ESTOS DEVUELVEN UNA ACTIVIDAD
                    return await IntentComprar(stepContext, luisResult, cancellationToken);
                    break;

                default:
                    break;

            }
            return await stepContext.NextAsync(cancellationToken: cancellationToken);// saltar al siguiente metodo 
        }

        private async Task<DialogTurnResult> IntentComprar(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            // usamos el nuevo dialogo creado ANTES SE DEBE AGREGAR EL DIALOGO AL DIALOGO ROOT
            return await stepContext.BeginDialogAsync(nameof(CellPhoneBuy.CellPhoneBuyDialog), cancellationToken: cancellationToken);
        }

        private async Task IntentVerOpciones(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync("aqui tengo mis opciones ", cancellationToken:cancellationToken);
            await MyOptionsCard.ToShow(stepContext, cancellationToken);
            // uso de carpeta common/cards
        }

        private async Task IntentNone(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync("No comprendo, me puedes contar mas", cancellationToken: cancellationToken);
        }

        private async  Task IntentSaludar(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            //si el bot detecta que esta saludando voy a responder con un saludo 
            await stepContext.Context.SendActivityAsync("Hola que gusto verte", cancellationToken: cancellationToken);
        }

        private async Task IntentDespedir(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync("Hasta pronto", cancellationToken: cancellationToken);
        }

        private async Task IntentAgradecer(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            //si el bot detecta que esta saludando voy a responder con un saludo 
            await stepContext.Context.SendActivityAsync("Un gusto ayudar", cancellationToken: cancellationToken);
        }
    }
}
