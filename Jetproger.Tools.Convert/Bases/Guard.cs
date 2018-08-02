using System;
using System.Diagnostics;

namespace Jetproger.Tools.Convert.Bases
{
    public static class GuardExtensions
    {
        public static void Throw(this IGuardExpander expander, Func<bool> condition)
        {
            if (!condition()) return;
            throw Raise(expander);
        }

        public static void Throw(this IGuardExpander expander, Func<bool> condition, string errorMessage)
        {
            if (!condition()) return;
            Trace.WriteLine(new ExException(errorMessage));
            throw Raise(expander);
        }

        public static void Throw(this IGuardExpander expander, Func<bool> condition, ExTicket ticket)
        {
            if (!condition()) return;
            ticket.IsException = true;
            Trace.WriteLine(ticket);
            throw Raise(expander);
        }

        public static void Throw<T>(this IGuardExpander expander, Func<bool> condition) where T : ExTicket, new()
        {
            if (!condition()) return;
            var ticket = new T();
            ticket.IsException = true;
            Trace.WriteLine(ticket);
            throw Raise(expander);
        }

        public static void Throw<T>(this IGuardExpander expander, Func<bool> condition, string errorMessage) where T : ExTicket, new()
        {
            if (!condition()) return;
            var ticket = new T();
            ticket.IsException = true;
            ticket.Text = errorMessage;
            ticket.Description = errorMessage;
            Trace.WriteLine(ticket);
            throw Raise(expander);
        }

        public static void Throw<T>(this IGuardExpander expander, Func<bool> condition, ExTicket ticket) where T : ExTicket, new()
        {
            if (!condition()) return;
            var newTicket = new T();
            newTicket.IsException = true;
            newTicket.Text = ticket.Text;
            newTicket.Description = ticket.Description;
            Trace.WriteLine(newTicket);
            throw Raise(expander);
        }

        public static void Throw(this IGuardExpander expander, Exception exception)
        {
            if (exception == null) return;
            if (exception is ExException) throw exception;
            Trace.WriteLine(new ExException(exception));
            throw Raise(expander);
        }

        public static void Throw(this IGuardExpander expander, Exception exception, ExTicket ticket)
        {
            if (exception == null) return;
            if (exception is ExException) throw exception;
            ticket.IsException = true;
            Trace.WriteLine(ticket);
            Trace.WriteLine(new ExException(exception));
            throw Raise(expander);
        }

        public static void Throw<T>(this IGuardExpander expander, Exception exception) where T : ExTicket, new()
        {
            if (exception == null) return;
            if (exception is ExException) throw exception;
            var ex = new ExException(exception);
            var ticket = new T();
            ticket.IsException = true;
            ticket.Text = ex.Text;
            ticket.Description = ex.Description;
            Trace.WriteLine(ticket);
            throw Raise(expander);
        }

        public static void Throw<T>(this IGuardExpander expander, Exception exception, ExTicket ticket) where T : ExTicket, new()
        {
            if (exception == null) return;
            if (exception is ExException) throw exception;
            ticket.IsException = true;
            Trace.WriteLine(ticket);
            var ex = new ExException(exception);
            var newTicket = new T();
            newTicket.IsException = true;
            newTicket.Text = ex.Text;
            newTicket.Description = ex.Description;
            Trace.WriteLine(newTicket);
            throw Raise(expander);
        }

        public static T Catch<T>(this IGuardExpander expander, Func<bool> condition, T result)
        {
            return !condition() ? result : default(T);
        }

        public static void Catch<T>(this IGuardExpander expander, Func<bool> condition) where T : ExTicket, new()
        {
            if (!condition()) return;
            var ticket = new T();
            ticket.IsException = true;
            Trace.WriteLine(ticket);
        }

        public static TResult Catch<TTicket, TResult>(this IGuardExpander expander, Func<bool> condition) where TTicket : ExTicket, new()
        {
            return Catch<TTicket, TResult>(expander, condition, default(TResult));
        }

        public static TResult Catch<TTicket, TResult>(this IGuardExpander expander, Func<bool> condition, TResult result) where TTicket : ExTicket, new()
        {
            if (!condition()) return result;
            var ticket = new TTicket();
            ticket.IsException = true;
            Trace.WriteLine(ticket);
            return default(TResult);
        }

        public static void Catch(this IGuardExpander expander, Func<bool> condition, string errorMessage)
        {
            if (condition()) Trace.WriteLine(new ExException(errorMessage));
        }

        public static void CatchEx<T>(this IGuardExpander expander, Func<bool> condition, string errorMessage) where T : ExTicket, new()
        {
            if (!condition()) return;
            var ticket = new T();
            ticket.IsException = true;
            ticket.Text = errorMessage;
            ticket.Description = errorMessage;
            Trace.WriteLine(ticket);
        }

        public static T Catch<T>(this IGuardExpander expander, Func<bool> condition, string errorMessage)
        {
            return Catch<T>(expander, condition, errorMessage, default(T));
        }

        public static T Catch<T>(this IGuardExpander expander, Func<bool> condition, string errorMessage, T result)
        {
            if (!condition()) return result;
            Trace.WriteLine(new ExException(errorMessage));
            return default(T);
        }

        public static TResult Catch<TTicket, TResult>(this IGuardExpander expander, Func<bool> condition, string errorMessage) where TTicket : ExTicket, new()
        {
            return Catch<TTicket, TResult>(expander, condition, errorMessage, default(TResult));
        }

        public static TResult Catch<TTicket, TResult>(this IGuardExpander expander, Func<bool> condition, string errorMessage, TResult result) where TTicket : ExTicket, new()
        {
            if (!condition()) return result;
            var ticket = new TTicket();
            ticket.IsException = true;
            ticket.Text = errorMessage;
            ticket.Description = errorMessage;
            Trace.WriteLine(ticket);
            return default(TResult);
        }

        public static void Catch(this IGuardExpander expander, Func<bool> condition, ExTicket ticket)
        {
            if (!condition()) return;
            ticket.IsException = true;
            Trace.WriteLine(ticket);
        }

        public static void CatchEx<T>(this IGuardExpander expander, Func<bool> condition, ExTicket ticket) where T : ExTicket, new()
        {
            if (!condition()) return;
            var newTicket = new T();
            newTicket.IsException = true;
            newTicket.Text = ticket.Text;
            newTicket.Description = ticket.Description;
            Trace.WriteLine(newTicket);
        }

        public static T Catch<T>(this IGuardExpander expander, Func<bool> condition, ExTicket ticket)
        {
            return Catch<T>(expander, condition, ticket, default(T));
        }

        public static T Catch<T>(this IGuardExpander expander, Func<bool> condition, ExTicket ticket, T result)
        {
            if (!condition()) return result;
            ticket.IsException = true;
            Trace.WriteLine(ticket);
            return default(T);
        }

        public static TResult Catch<TTicket, TResult>(this IGuardExpander expander, Func<bool> condition, ExTicket ticket) where TTicket : ExTicket, new()
        {
            return Catch<TTicket, TResult>(expander, condition, ticket, default(TResult));
        }

        public static TResult Catch<TTicket, TResult>(this IGuardExpander expander, Func<bool> condition, ExTicket ticket, TResult result) where TTicket : ExTicket, new()
        {
            if (!condition()) return result;
            var newTicket = new TTicket();
            newTicket.IsException = true;
            newTicket.Text = ticket.Text;
            newTicket.Description = ticket.Description;
            Trace.WriteLine(newTicket);
            return default(TResult);
        }

        public static void Catch(this IGuardExpander expander, Exception exception)
        {
            if (exception != null) Trace.WriteLine(new ExException(exception));
        }

        public static void CatchEx<T>(this IGuardExpander expander, Exception exception) where T : ExTicket, new()
        {
            if (exception == null) return;
            var ex = new ExException(exception);
            var ticket = new T();
            ticket.IsException = true;
            ticket.Text = ex.Text;
            ticket.Description = ex.Description;
            Trace.WriteLine(ticket);
        }

        public static T Catch<T>(this IGuardExpander expander, Exception exception)
        {
            return Catch<T>(expander, exception, default(T));
        }

        public static T Catch<T>(this IGuardExpander expander, Exception exception, T result)
        {
            if (exception == null) return result;
            Trace.WriteLine(new ExException(exception));
            return default(T);
        }

        public static TResult Catch<TTicket, TResult>(this IGuardExpander expander, Exception exception) where TTicket : ExTicket, new()
        {
            return Catch<TTicket, TResult>(expander, exception, default(TResult));
        }

        public static TResult Catch<TTicket, TResult>(this IGuardExpander expander, Exception exception, TResult result) where TTicket : ExTicket, new()
        {
            if (exception == null) return result;
            var ticket = new TTicket();
            var ex = new ExException(exception);
            ticket.IsException = true;
            ticket.Text = ex.Text;
            ticket.Description = ex.Description;
            return default(TResult);
        }

        public static void Catch(this IGuardExpander expander, Exception exception, ExTicket ticket)
        {
            if (exception == null) return;
            ticket.IsException = true;
            Trace.WriteLine(ticket);
            Trace.WriteLine(new ExException(exception));
        }

        public static void CatchEx<T>(this IGuardExpander expander, Exception exception, ExTicket ticket) where T : ExTicket, new()
        {
            if (exception == null) return;
            ticket.IsException = true;
            Trace.WriteLine(ticket);
            var ex = new ExException(exception);
            var newTicket = new T();
            newTicket.IsException = true;
            newTicket.Text = ex.Text;
            newTicket.Description = ex.Description;
            Trace.WriteLine(newTicket);
        }

        public static T Catch<T>(this IGuardExpander expander, Exception exception, ExTicket ticket)
        {
            return Catch<T>(expander, exception, ticket, default(T));
        }

        public static T Catch<T>(this IGuardExpander expander, Exception exception, ExTicket ticket, T result)
        {
            if (exception == null) return result;
            ticket.IsException = true;
            Trace.WriteLine(ticket);
            Trace.WriteLine(new ExException(exception));
            return default(T);
        }

        public static TResult Catch<TTicket, TResult>(this IGuardExpander expander, Exception exception, ExTicket ticket) where TTicket : ExTicket, new()
        {
            return Catch<TTicket, TResult>(expander, exception, ticket, default(TResult));
        }

        public static TResult Catch<TTicket, TResult>(this IGuardExpander expander, Exception exception, ExTicket ticket, TResult result) where TTicket : ExTicket, new()
        {
            if (exception == null) return result;
            ticket.IsException = true;
            Trace.WriteLine(ticket);
            var ex = new ExException(exception);
            var newTicket = new TTicket();
            newTicket.IsException = true;
            newTicket.Text = ex.Text;
            newTicket.Description = ex.Description;
            Trace.WriteLine(newTicket);
            return default(TResult);
        }

        public static Exception Raise(this IGuardExpander expander, string errorMessage)
        {
            return new Exception(errorMessage);
        }

        public static Exception Raise(this IGuardExpander expander, Exception exception)
        {
            return new ExException(exception);
        }

        public static Exception Raise(this IGuardExpander expander, ExTicket ticket)
        {
            return new ExException(ticket);
        }

        public static Exception Raise(this IGuardExpander expander)
        {
            return new ExException();
        }
    }
}