using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Commun
{
    public static class ExceptionExtensions
    {
        public static MessageErreur[] ObtenirMessages(this Exception exception)
        {
            var messages = exception.Hierarchie(ex => ex.InnerException)
                .Select(ex => new MessageErreur(null, ex.Message));
            return messages.ToArray();
        }

        private static IEnumerable<TSource> Hierarchie<TSource>(
            this TSource source,
            Func<TSource, TSource> nextItem,
            Func<TSource, bool> canContinue)
        {
            for (var current = source; canContinue(current); current = nextItem(current))
                yield return current;
        }

        private static IEnumerable<TSource> Hierarchie<TSource>(
            this TSource source,
            Func<TSource, TSource> nextItem)
            where TSource : class
        {
            return Hierarchie(source, nextItem, s => s != null);
        }
    }
}
