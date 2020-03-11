using System;
using System.Diagnostics;

namespace Jetproger.Tools.Convert.Bases
{
    public static class GuardExtensions
    {
        #region try

        public static void Try(this Jc.IGuardExpander exp, Action func)
        {
            try
            {
                func();
            }
            catch (Exception e)
            {
                Catch(exp, e);
            }
        }

        public static T Try<T>(this Jc.IGuardExpander exp, Func<T> func)
        {
            try
            {
                return func();
            }
            catch (Exception e) {
                return Catch(exp, e, default(T));
            }
        }

        public static T Try<T>(this Jc.IGuardExpander exp, T defaultValue, Func<T> func)
        {
            try
            {
                return func();
            }
            catch (Exception e) {
                return Catch(exp, e, defaultValue);
            }
        }

        #endregion

        #region throw

        public static void Throw(this Jc.IGuardExpander exp, Func<bool> condition)
        {
            if (!condition()) return;
            throw Raise(exp);
        }

        public static void Throw(this Jc.IGuardExpander exp, Func<bool> condition, string errorMessage)
        {
            if (!condition()) return;
            Trace.WriteLine(new Jc.Exception(errorMessage));
            throw Raise(exp);
        }

        public static void Throw(this Jc.IGuardExpander exp, Func<bool> condition, Jc.Ticket ticket)
        {
            if (!condition()) return;
            ticket.IsException = true;
            Trace.WriteLine(ticket);
            throw Raise(exp);
        }

        public static void Throw<T>(this Jc.IGuardExpander exp, Func<bool> condition) where T : Jc.Ticket, new()
        {
            if (!condition()) return;
            var ticket = new T();
            ticket.IsException = true;
            Trace.WriteLine(ticket);
            throw Raise(exp);
        }

        public static void Throw<T>(this Jc.IGuardExpander exp, Func<bool> condition, string errorMessage) where T : Jc.Ticket, new()
        {
            if (!condition()) return;
            var ticket = new T();
            ticket.IsException = true;
            ticket.Text = errorMessage;
            ticket.Description = errorMessage;
            Trace.WriteLine(ticket);
            throw Raise(exp);
        }

        public static void Throw<T>(this Jc.IGuardExpander exp, Func<bool> condition, Jc.Ticket ticket) where T : Jc.Ticket, new()
        {
            if (!condition()) return;
            var newTicket = new T();
            newTicket.IsException = true;
            newTicket.Text = ticket.Text;
            newTicket.Description = ticket.Description;
            Trace.WriteLine(newTicket);
            throw Raise(exp);
        }

        public static void Throw(this Jc.IGuardExpander exp, Exception exception)
        {
            if (exception == null) return;
            if (exception is Jc.Exception) throw exception;
            Trace.WriteLine(new Jc.Exception(exception));
            throw Raise(exp);
        }

        public static void Throw(this Jc.IGuardExpander exp, Exception exception, Jc.Ticket ticket)
        {
            if (exception == null) return;
            if (exception is Jc.Exception) throw exception;
            ticket.IsException = true;
            Trace.WriteLine(ticket);
            Trace.WriteLine(new Jc.Exception(exception));
            throw Raise(exp);
        }

        public static void Throw<T>(this Jc.IGuardExpander exp, Exception exception) where T : Jc.Ticket, new()
        {
            if (exception == null) return;
            if (exception is Jc.Exception) throw exception;
            var ex = new Jc.Exception(exception);
            var ticket = new T();
            ticket.IsException = true;
            ticket.Text = ex.Text;
            ticket.Description = ex.Description;
            Trace.WriteLine(ticket);
            throw Raise(exp);
        }

        public static void Throw<T>(this Jc.IGuardExpander exp, Exception exception, Jc.Ticket ticket) where T : Jc.Ticket, new()
        {
            if (exception == null) return;
            if (exception is Jc.Exception) throw exception;
            ticket.IsException = true;
            Trace.WriteLine(ticket);
            var ex = new Jc.Exception(exception);
            var newTicket = new T();
            newTicket.IsException = true;
            newTicket.Text = ex.Text;
            newTicket.Description = ex.Description;
            Trace.WriteLine(newTicket);
            throw Raise(exp);
        }

        #endregion

        #region catch

        public static T Catch<T>(this Jc.IGuardExpander exp, Func<bool> condition, T result)
        {
            return !condition() ? result : default(T);
        }

        public static void Catch<T>(this Jc.IGuardExpander exp, Func<bool> condition) where T : Jc.Ticket, new()
        {
            if (!condition()) return;
            var ticket = new T();
            ticket.IsException = true;
            Trace.WriteLine(ticket);
        }

        public static TResult Catch<TResult, TTicket>(this Jc.IGuardExpander exp, Func<bool> condition) where TTicket : Jc.Ticket, new()
        {
            return Catch<TResult, TTicket>(exp, condition, default(TResult));
        }

        public static TResult Catch<TResult, TTicket>(this Jc.IGuardExpander exp, Func<bool> condition, TResult result) where TTicket : Jc.Ticket, new()
        {
            if (!condition()) return result;
            var ticket = new TTicket();
            ticket.IsException = true;
            Trace.WriteLine(ticket);
            return default(TResult);
        }

        public static void Catch(this Jc.IGuardExpander exp, Func<bool> condition, string errorMessage)
        {
            if (condition()) Trace.WriteLine(new Jc.Exception(errorMessage));
        }

        public static void Catch<T>(this Jc.IGuardExpander exp, string errorMessage, Func<bool> condition) where T : Jc.Ticket, new()
        {
            if (!condition()) return;
            var ticket = new T();
            ticket.IsException = true;
            ticket.Text = errorMessage;
            ticket.Description = errorMessage;
            Trace.WriteLine(ticket);
        }

        public static T Catch<T>(this Jc.IGuardExpander exp, Func<bool> condition, string errorMessage)
        {
            return Catch<T>(exp, condition, errorMessage, default(T));
        }

        public static T Catch<T>(this Jc.IGuardExpander exp, Func<bool> condition, string errorMessage, T result)
        {
            if (!condition()) return result;
            Trace.WriteLine(new Jc.Exception(errorMessage));
            return default(T);
        }

        public static TResult Catch<TTicket, TResult>(this Jc.IGuardExpander exp, Func<bool> condition, string errorMessage) where TTicket : Jc.Ticket, new()
        {
            return Catch<TTicket, TResult>(exp, condition, errorMessage, default(TResult));
        }

        public static TResult Catch<TTicket, TResult>(this Jc.IGuardExpander exp, Func<bool> condition, string errorMessage, TResult result) where TTicket : Jc.Ticket, new()
        {
            if (!condition()) return result;
            var ticket = new TTicket();
            ticket.IsException = true;
            ticket.Text = errorMessage;
            ticket.Description = errorMessage;
            Trace.WriteLine(ticket);
            return default(TResult);
        }

        public static void Catch(this Jc.IGuardExpander exp, Func<bool> condition, Jc.Ticket ticket)
        {
            if (!condition()) return;
            ticket.IsException = true;
            Trace.WriteLine(ticket);
        }

        public static void Catch<T>(this Jc.IGuardExpander exp, Func<bool> condition, Jc.Ticket ticket) where T : Jc.Ticket, new()
        {
            if (!condition()) return;
            var newTicket = new T();
            newTicket.IsException = true;
            newTicket.Text = ticket.Text;
            newTicket.Description = ticket.Description;
            Trace.WriteLine(newTicket);
        }

        public static TResult Catch<TResult, TParam>(this Jc.IGuardExpander exp, Func<bool> condition, TParam ticket) where TParam : Jc.Ticket, new()
        {
            return Catch<TResult>(exp, condition, ticket, default(TResult));
        }

        public static T Catch<T>(this Jc.IGuardExpander exp, Func<bool> condition, Jc.Ticket ticket, T result)
        {
            if (!condition()) return result;
            ticket.IsException = true;
            Trace.WriteLine(ticket);
            return default(T);
        }

        public static TResult Catch<TTicket, TResult>(this Jc.IGuardExpander exp, Func<bool> condition, Jc.Ticket ticket) where TTicket : Jc.Ticket, new()
        {
            return Catch<TTicket, TResult>(exp, condition, ticket, default(TResult));
        }

        public static TResult Catch<TTicket, TResult>(this Jc.IGuardExpander exp, Func<bool> condition, Jc.Ticket ticket, TResult result) where TTicket : Jc.Ticket, new()
        {
            if (!condition()) return result;
            var newTicket = new TTicket();
            newTicket.IsException = true;
            newTicket.Text = ticket.Text;
            newTicket.Description = ticket.Description;
            Trace.WriteLine(newTicket);
            return default(TResult);
        }

        public static void Catch<T> (this Jc.IGuardExpander exp, T exception) where T : Exception
        {
            if (exception != null) Trace.WriteLine(new Jc.Exception(exception));
        }

        public static void Catch<T>(this Jc.IGuardExpander exp, Exception exception) where T : Jc.Ticket, new()
        {
            if (exception == null) return;
            var ex = new Jc.Exception(exception);
            var ticket = new T();
            ticket.IsException = true;
            ticket.Text = ex.Text;
            ticket.Description = ex.Description;
            Trace.WriteLine(ticket);
        }

        public static TResult Catch<TResult, TParam>(this Jc.IGuardExpander exp, TParam exception) where TParam : Exception
        {
            return Catch<TResult>(exp, exception, default(TResult));
        }

        public static T Catch<T>(this Jc.IGuardExpander exp, Exception exception, T result)
        {
            if (exception == null) return result;
            Trace.WriteLine(new Jc.Exception(exception));
            return result;
        }

        public static TResult Catch<TResult, TTicket>(this Jc.IGuardExpander exp, Exception exception) where TTicket : Jc.Ticket, new()
        {
            return Catch<TResult, TTicket>(exp, exception, default(TResult));
        }

        public static TResult Catch<TResult, TTicket>(this Jc.IGuardExpander exp, Exception exception, TResult result) where TTicket : Jc.Ticket, new()
        {
            if (exception == null) return result;
            var ticket = new TTicket();
            var ex = new Jc.Exception(exception);
            ticket.IsException = true;
            ticket.Text = ex.Text;
            ticket.Description = ex.Description;
            return default(TResult);
        }

        public static void Catch(this Jc.IGuardExpander exp, Exception exception, Jc.Ticket ticket)
        {
            if (exception == null) return;
            ticket.IsException = true;
            Trace.WriteLine(ticket);
            Trace.WriteLine(new Jc.Exception(exception));
        }

        public static void Catch<T>(this Jc.IGuardExpander exp, Exception exception, Jc.Ticket ticket) where T : Jc.Ticket, new()
        {
            if (exception == null) return;
            ticket.IsException = true;
            Trace.WriteLine(ticket);
            var ex = new Jc.Exception(exception);
            var newTicket = new T();
            newTicket.IsException = true;
            newTicket.Text = ex.Text;
            newTicket.Description = ex.Description;
            Trace.WriteLine(newTicket);
        }

        public static TResult Catch<TResult, TParam>(this Jc.IGuardExpander exp, Exception exception, TParam ticket) where TParam : Jc.Ticket, new()
        {
            return Catch<TResult>(exp, exception, ticket, default(TResult));
        }

        public static T Catch<T>(this Jc.IGuardExpander exp, Exception exception, Jc.Ticket ticket, T result)
        {
            if (exception == null) return result;
            ticket.IsException = true;
            Trace.WriteLine(ticket);
            Trace.WriteLine(new Jc.Exception(exception));
            return default(T);
        }

        public static TResult Catch<TTicket, TResult>(this Jc.IGuardExpander exp, Exception exception, Jc.Ticket ticket) where TTicket : Jc.Ticket, new()
        {
            return Catch<TTicket, TResult>(exp, exception, ticket, default(TResult));
        }

        public static TResult Catch<TTicket, TResult>(this Jc.IGuardExpander exp, Exception exception, Jc.Ticket ticket, TResult result) where TTicket : Jc.Ticket, new()
        {
            if (exception == null) return result;
            ticket.IsException = true;
            Trace.WriteLine(ticket);
            var ex = new Jc.Exception(exception);
            var newTicket = new TTicket();
            newTicket.IsException = true;
            newTicket.Text = ex.Text;
            newTicket.Description = ex.Description;
            Trace.WriteLine(newTicket);
            return default(TResult);
        }

        #endregion

        #region raise

        public static Exception Raise(this Jc.IGuardExpander exp, string errorMessage)
        {
            return new Exception(errorMessage);
        }

        public static Exception Raise(this Jc.IGuardExpander exp, Exception exception)
        {
            return new Jc.Exception(exception);
        }

        public static Exception Raise(this Jc.IGuardExpander exp, Jc.Ticket ticket)
        {
            return new Jc.Exception(ticket);
        }

        public static Exception Raise(this Jc.IGuardExpander expander)
        {
            return new Jc.Exception();
        }

        #endregion
    }
}