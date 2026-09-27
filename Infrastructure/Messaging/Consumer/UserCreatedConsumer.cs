using Domain.Events;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Messaging.Consumer
{
    public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
    {
        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            var message = context.Message;

            var email = $"Para: {message.Email}\nAssunto: Boas vindas!\nCorpo: Prezado {message.Nome}, o seu usuário foi criado com sucesso. Seja bem vindo!\n\n";

            Console.WriteLine(email);
        }
    }
}
