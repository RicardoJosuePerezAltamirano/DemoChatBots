// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FirstBot1
{
    public class EmptyBot<T> : ActivityHandler where T:Dialog// NUMERO 5 recibir un Dialog
    {
        
        private readonly BotState UserState;
        private readonly BotState ConversationState;
        private readonly Dialog Dialog;
        // NUMERO 2
        // injectamos los estados del bot 
        public EmptyBot(UserState _userState, ConversationState _ConversationState,T dialog)// NUMERO 5 recibir un Dialog
        {
            UserState = _userState;
            ConversationState = _ConversationState;
            Dialog = dialog; // NUMERO 5 recibir un Dialog
        }
        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text($"Hello world!"), cancellationToken);
                }
            }
        }
        // captura actividades del boot y del usuario
        //NUMERO 3
        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            await base.OnTurnAsync(turnContext, cancellationToken);
            await UserState.SaveChangesAsync(turnContext, false, cancellationToken);//guardar cambios , false solo cuando haya cambios
            await ConversationState.SaveChangesAsync(turnContext, false, cancellationToken);

        }
        // captura las actividades del usuario
        // NUMERO 4
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            // se manda lo que escribe el usuario
            // este metodo sirve como echo 
            //await base.OnMessageActivityAsync(turnContext, cancellationToken);
            //------------DEMO
            //var UserWrited = turnContext.Activity.Text;// lo que el usuario escribe
            //await turnContext.SendActivityAsync($"Usuario: {UserWrited}", cancellationToken: cancellationToken);

            //--NUMERO 6 AHORA REDIRECCIONAMOS AL DIALOGO
            await Dialog.RunAsync(
                turnContext,// contexto
                ConversationState.CreateProperty<DialogState>(nameof(DialogState)),// estado del dialogo
               cancellationToken: cancellationToken);
        }
    }
}
