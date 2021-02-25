using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FirstBot1.Dialogs.CellPhoneBuy
{
    public class CellPhoneBuyDialog:ComponentDialog
    {
        public CellPhoneBuyDialog()
        {
            //LuisService = _LuisService;
            //metodos secuenciales
            var watherfallSteps = new WaterfallStep[]
            {
                SetCellPhoneModel,
                SetCustomerName,
                SetShippingMethod,
                SetEmail,
                FinalProcess
                
            };
            // pasamos dialogo con los metodos secuenciales
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), watherfallSteps));
            //InitialDialogId = nameof(WaterfallDialog);
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            //InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> FinalProcess(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // ultima ejecucion
            var Email = stepContext.Context.Activity.Text;
            await stepContext.Context.SendActivityAsync("Tu compra ha sido registrada");
            // guardamos en base de datos
            return await stepContext.ContinueDialogAsync(cancellationToken);
        }

        private async Task<DialogTurnResult> SetEmail(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // se ejecuta cuarto
            // tomamos el dato ingresado anteriormente
            var ShippingMethod = stepContext.Context.Activity.Text;
            return await stepContext.PromptAsync(nameof(TextPrompt),
                new PromptOptions { Prompt = MessageFactory.Text("Ingrese su email") }, cancellationToken);
        }

        private async Task<DialogTurnResult> SetShippingMethod(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // se ejecuta tercero
            // tomamos el dato ingresado anteriormente
            var CustomerName = stepContext.Context.Activity.Text;
            return await stepContext.PromptAsync(nameof(TextPrompt),
                new PromptOptions { Prompt = MessageFactory.Text("Ingrese su direccion de envio") }, cancellationToken);
        }

        private async Task<DialogTurnResult> SetCustomerName(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // se ejecuta segundo
            // tomamos el dato ingresado anteriormente
            //var CellphoneModel = stepContext.Context.Activity.Text;
            return await stepContext.PromptAsync(nameof(TextPrompt),
                new PromptOptions { Prompt = MessageFactory.Text("Ingrese su nombre") }, cancellationToken);

        }

        private async Task<DialogTurnResult> SetCellPhoneModel(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // se ejecuta primero
            return await stepContext.PromptAsync(nameof(TextPrompt),
                new PromptOptions { Prompt = MessageFactory.Text("Ingrese el modelo del celular que busca") }, cancellationToken);
            //return await stepContext.Context.SendActivityAsync("modelo");
        }
    }
}
