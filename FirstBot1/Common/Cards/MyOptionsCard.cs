using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FirstBot1.Common.Cards
{
    public class MyOptionsCard
    {
        // pasamos el contexto para invocar las tarjetas con el contexto correspondiente 
        public static async Task ToShow(DialogContext stepContext,CancellationToken CT)
        {
            await stepContext.Context.SendActivityAsync(activity:CreateCarousel(), cancellationToken: CT);
            // la actividad puede ser una tarjeta o un carousel
        }
        private static Activity CreateCarousel()
        {
            var cardVentaCelulares = new HeroCard
            {
                Title="Venta de celulares",
                Subtitle="Ver opciones",
                Images=new List<CardImage>() { new CardImage("https://cnet1.cbsistatic.com/img/42V9Nx_YYLJE-FvhwHCEz005HWw=/940x0/2020/04/07/0c2d1786-5db8-425d-8758-651e01f1466d/galaxy-a51.jpg") },
                Buttons=new List<CardAction>() 
                {
                    new CardAction
                    {
                         Title="Comprar", Value="Comprar", Type=ActionTypes.ImBack
                    }
                }

            };
            var cardinformacionContacto = new HeroCard
            {
                Title = "Contactanos",
                Subtitle = "Ver opciones",
                Images = new List<CardImage>() { new CardImage("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSYwcgyV5YD-rTEp9bnqHzk-GLm9mgQjSspRQ&usqp=CAU") },
                Buttons = new List<CardAction>()
                {
                    new CardAction
                    {
                         Title="Centro de contacto", Value="Centro de contacto", Type=ActionTypes.ImBack,
                    },
                    new CardAction
                    {
                         Title="Sitio Web SolvereTI", Value="http://solvereti.com", Type=ActionTypes.OpenUrl,
                    }
                }

            };
            var cardRedes = new HeroCard
            {
                Title = "Mis Redes",
                Subtitle = "Ver opciones",
                Images = new List<CardImage>() { new CardImage("https://www.trecebits.com/wp-content/uploads/2019/08/Iconos-Redes-Sociales.jpg") },
                Buttons = new List<CardAction>()
                {
                    new CardAction
                    {
                         Title="Twitter", Value="https://twitter.com/home", Type=ActionTypes.OpenUrl,
                    },
                    //new CardAction
                    //{
                    //     Title="Facebook", Value="https://www.facebook.com/RicardoJosue04/", Type=ActionTypes.OpenUrl,
                    //},
                    
                    new CardAction
                    {
                         Title="Youtube", Value="https://www.youtube.com/channel/UC5Rh7fFBjc8_EBEeEXg67aw", Type=ActionTypes.OpenUrl,
                    }
                }

            };
            var optionAttachment = new List<Attachment>()
            {
                cardVentaCelulares.ToAttachment(),
                cardinformacionContacto.ToAttachment(),
                cardRedes.ToAttachment()

            };
            var reply = MessageFactory.Attachment(optionAttachment);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            return reply as Activity;
        }
    }
}
