using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concorrencia
{
    class Program
    {
        static void Main(string[] args)
        {
            ClientWins();
        }

        private static void StoreWins()
        {
            using (ClarifyDBEntities ctx = new ClarifyDBEntities())
            {
                var pessoa = ctx.Pessoas.First(x => x.ID == 12);
                pessoa.UltimoNome = "Schimt";

                try
                {
                    int num = ctx.SaveChanges();
                    Console.WriteLine("Sem conflitos. " + num.ToString() + " Atualizações Realizadas.");
                }
                catch (OptimisticConcurrencyException)
                {
                    var ctxMutavel = ((IObjectContextAdapter)ctx).ObjectContext;
                    ctxMutavel.Refresh(RefreshMode.StoreWins, pessoa);

                    // Save changes.
                    ctxMutavel.SaveChanges();
                    Console.WriteLine("Alguem tentou salvar a mesma coisa, o que você fez foi desfeito.");
                }
            }
        }

        private static void ClientWins()
        {
            using (ClarifyDBEntities ctx = new ClarifyDBEntities())
            {
                var pessoa = ctx.Pessoas.First(x => x.ID == 12);
                pessoa.UltimoNome = "Schimt";

                try
                {
                    int num = ctx.SaveChanges();
                    Console.WriteLine("Sem conflitos. " + num.ToString() + " Atualizações Realizadas.");
                }
                catch (OptimisticConcurrencyException)
                {
                    var ctxMutavel = ((IObjectContextAdapter)ctx).ObjectContext;
                    ctxMutavel.Refresh(RefreshMode.ClientWins, pessoa);

                    // Save changes.
                    ctxMutavel.SaveChanges();
                    Console.WriteLine("Alguem tentou salvar a mesma coisa, porém o que você salvou sobrescreveu a mudança que haviam feito ");
                }
            }
        }
    }
}
