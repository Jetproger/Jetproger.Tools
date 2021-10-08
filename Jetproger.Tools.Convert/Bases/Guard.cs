using System;
using System.Diagnostics;

namespace Jetproger.Tools.Convert.Bases
{
    public static class GuardExtensions
    {
        #region try

        public static void Try(this Je.IErrExpander exp, Action func)
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

        public static T Try<T>(this Je.IErrExpander exp, Func<T> func)
        {
            try
            {
                return func();
            }
            catch (Exception e)
            {
                return Catch(exp, e, default(T));
            }
        }

        public static T Try<T>(this Je.IErrExpander exp, T defaultValue, Func<T> func)
        {
            try
            {
                return func();
            }
            catch (Exception e)
            {
                return Catch(exp, e, defaultValue);
            }
        }

        #endregion

        #region throw

        public static void Throw(this Je.IErrExpander exp, Func<bool> condition)
        {
            if (condition()) throw Raise(exp);
        }

        public static void Throw(this Je.IErrExpander exp, Func<bool> condition, string errorMessage)
        {
            if (!condition()) return;
            Trace.WriteLine(new Je.Exception(errorMessage));
            throw Raise(exp);
        }

        public static void Throw(this Je.IErrExpander exp, Func<bool> condition, Ticket ticket)
        {
            if (!condition()) return;
            ticket.IsError = true;
            Trace.WriteLine(ticket);
            throw Raise(exp);
        }

        public static void Throw<T>(this Je.IErrExpander exp, Func<bool> condition) where T : Ticket, new()
        {
            if (!condition()) return;
            var ticket = new T();
            ticket.IsError = true;
            Trace.WriteLine(ticket);
            throw Raise(exp);
        }

        public static void Throw<T>(this Je.IErrExpander exp, Func<bool> condition, string errorMessage) where T : Ticket, new()
        {
            if (!condition()) return;
            var ticket = new T();
            ticket.IsError = true;
            ticket.Message = errorMessage;
            ticket.Comment = errorMessage;
            Trace.WriteLine(ticket);
            throw Raise(exp);
        }

        public static void Throw<T>(this Je.IErrExpander exp, Func<bool> condition, Ticket ticket) where T : Ticket, new()
        {
            if (!condition()) return;
            var newTicket = new T();
            newTicket.IsError = true;
            newTicket.Message = ticket.Message;
            newTicket.Comment = ticket.Comment;
            Trace.WriteLine(newTicket);
            throw Raise(exp);
        }

        public static void Throw(this Je.IErrExpander exp, Exception exception)
        {
            if (exception == null) return;
            if (exception is Je.Exception) throw exception;
            Trace.WriteLine(new Je.Exception(exception));
            throw Raise(exp);
        }

        public static void Throw(this Je.IErrExpander exp, Exception exception, Ticket ticket)
        {
            if (exception == null) return;
            if (exception is Je.Exception) throw exception;
            ticket.IsError = true;
            Trace.WriteLine(ticket);
            Trace.WriteLine(new Je.Exception(exception));
            throw Raise(exp);
        }

        public static void Throw<T>(this Je.IErrExpander exp, Exception exception) where T : Ticket, new()
        {
            if (exception == null) return;
            if (exception is Je.Exception) throw exception;
            var ex = new Je.Exception(exception);
            var ticket = new T();
            ticket.IsError = true;
            ticket.Message = ex.Text;
            ticket.Comment = ex.Description;
            Trace.WriteLine(ticket);
            throw Raise(exp);
        }

        public static void Throw<T>(this Je.IErrExpander exp, Exception exception, Ticket ticket) where T : Ticket, new()
        {
            if (exception == null) return;
            if (exception is Je.Exception) throw exception;
            ticket.IsError = true;
            Trace.WriteLine(ticket);
            var ex = new Je.Exception(exception);
            var newTicket = new T();
            newTicket.IsError = true;
            newTicket.Message = ex.Text;
            newTicket.Comment = ex.Description;
            Trace.WriteLine(newTicket);
            throw Raise(exp);
        }

        #endregion

        #region catch

        public static T Catch<T>(this Je.IErrExpander exp, Func<bool> condition, T result)
        {
            return !condition() ? result : default(T);
        }

        public static void Catch<T>(this Je.IErrExpander exp, Func<bool> condition) where T : Ticket, new()
        {
            if (!condition()) return;
            var ticket = new T();
            ticket.IsError = true;
            Trace.WriteLine(ticket);
        }

        public static TResult Catch<TResult, TTicket>(this Je.IErrExpander exp, Func<bool> condition) where TTicket : Ticket, new()
        {
            return Catch<TResult, TTicket>(exp, condition, default(TResult));
        }

        public static TResult Catch<TResult, TTicket>(this Je.IErrExpander exp, Func<bool> condition, TResult result) where TTicket : Ticket, new()
        {
            if (!condition()) return result;
            var ticket = new TTicket();
            ticket.IsError = true;
            Trace.WriteLine(ticket);
            return default(TResult);
        }

        public static void Catch(this Je.IErrExpander exp, Func<bool> condition, string errorMessage)
        {
            if (condition()) Trace.WriteLine(new Je.Exception(errorMessage));
        }

        public static void Catch<T>(this Je.IErrExpander exp, string errorMessage, Func<bool> condition) where T : Ticket, new()
        {
            if (!condition()) return;
            var ticket = new T();
            ticket.IsError = true;
            ticket.Message = errorMessage;
            ticket.Comment = errorMessage;
            Trace.WriteLine(ticket);
        }

        public static T Catch<T>(this Je.IErrExpander exp, Func<bool> condition, string errorMessage)
        {
            return Catch<T>(exp, condition, errorMessage, default(T));
        }

        public static T Catch<T>(this Je.IErrExpander exp, Func<bool> condition, string errorMessage, T result)
        {
            if (!condition()) return result;
            Trace.WriteLine(new Je.Exception(errorMessage));
            return default(T);
        }

        public static TResult Catch<TTicket, TResult>(this Je.IErrExpander exp, Func<bool> condition, string errorMessage) where TTicket : Ticket, new()
        {
            return Catch<TTicket, TResult>(exp, condition, errorMessage, default(TResult));
        }

        public static TResult Catch<TTicket, TResult>(this Je.IErrExpander exp, Func<bool> condition, string errorMessage, TResult result) where TTicket : Ticket, new()
        {
            if (!condition()) return result;
            var ticket = new TTicket();
            ticket.IsError = true;
            ticket.Message = errorMessage;
            ticket.Comment = errorMessage;
            Trace.WriteLine(ticket);
            return default(TResult);
        }

        public static void Catch(this Je.IErrExpander exp, Func<bool> condition, Ticket ticket)
        {
            if (!condition()) return;
            ticket.IsError = true;
            Trace.WriteLine(ticket);
        }

        public static void Catch<T>(this Je.IErrExpander exp, Func<bool> condition, Ticket ticket) where T : Ticket, new()
        {
            if (!condition()) return;
            var newTicket = new T();
            newTicket.IsError = true;
            newTicket.Message = ticket.Message;
            newTicket.Comment = ticket.Comment;
            Trace.WriteLine(newTicket);
        }

        public static TResult Catch<TResult, TParam>(this Je.IErrExpander exp, Func<bool> condition, TParam ticket) where TParam : Ticket, new()
        {
            return Catch<TResult>(exp, condition, ticket, default(TResult));
        }

        public static T Catch<T>(this Je.IErrExpander exp, Func<bool> condition, Ticket ticket, T result)
        {
            if (!condition()) return result;
            ticket.IsError = true;
            Trace.WriteLine(ticket);
            return default(T);
        }

        public static TResult Catch<TTicket, TResult>(this Je.IErrExpander exp, Func<bool> condition, Ticket ticket) where TTicket : Ticket, new()
        {
            return Catch<TTicket, TResult>(exp, condition, ticket, default(TResult));
        }

        public static TResult Catch<TTicket, TResult>(this Je.IErrExpander exp, Func<bool> condition, Ticket ticket, TResult result) where TTicket : Ticket, new()
        {
            if (!condition()) return result;
            var newTicket = new TTicket();
            newTicket.IsError = true;
            newTicket.Message = ticket.Message;
            newTicket.Comment = ticket.Comment;
            Trace.WriteLine(newTicket);
            return default(TResult);
        }

        public static void Catch<T> (this Je.IErrExpander exp, T exception) where T : Exception
        {
            if (exception != null) Trace.WriteLine(new Je.Exception(exception));
        }

        public static void Catch<T>(this Je.IErrExpander exp, Exception exception) where T : Ticket, new()
        {
            if (exception == null) return;
            var ex = new Je.Exception(exception);
            var ticket = new T();
            ticket.IsError = true;
            ticket.Message = ex.Text;
            ticket.Comment = ex.Description;
            Trace.WriteLine(ticket);
        }

        public static TResult Catch<TResult, TParam>(this Je.IErrExpander exp, TParam exception) where TParam : Exception
        {
            return Catch<TResult>(exp, exception, default(TResult));
        }

        public static T Catch<T>(this Je.IErrExpander exp, Exception exception, T result)
        {
            if (exception == null) return result;
            Trace.WriteLine(new Je.Exception(exception));
            return result;
        }

        public static TResult Catch<TResult, TTicket>(this Je.IErrExpander exp, Exception exception) where TTicket : Ticket, new()
        {
            return Catch<TResult, TTicket>(exp, exception, default(TResult));
        }

        public static TResult Catch<TResult, TTicket>(this Je.IErrExpander exp, Exception exception, TResult result) where TTicket : Ticket, new()
        {
            if (exception == null) return result;
            var ticket = new TTicket();
            var ex = new Je.Exception(exception);
            ticket.IsError = true;
            ticket.Message = ex.Text;
            ticket.Comment = ex.Description;
            return default(TResult);
        }

        public static void Catch(this Je.IErrExpander exp, Exception exception, Ticket ticket)
        {
            if (exception == null) return;
            ticket.IsError = true;
            Trace.WriteLine(ticket);
            Trace.WriteLine(new Je.Exception(exception));
        }

        public static void Catch<T>(this Je.IErrExpander exp, Exception exception, Ticket ticket) where T : Ticket, new()
        {
            if (exception == null) return;
            ticket.IsError = true;
            Trace.WriteLine(ticket);
            var ex = new Je.Exception(exception);
            var newTicket = new T();
            newTicket.IsError = true;
            newTicket.Message = ex.Text;
            newTicket.Comment = ex.Description;
            Trace.WriteLine(newTicket);
        }

        public static TResult Catch<TResult, TParam>(this Je.IErrExpander exp, Exception exception, TParam ticket) where TParam : Ticket, new()
        {
            return Catch<TResult>(exp, exception, ticket, default(TResult));
        }

        public static T Catch<T>(this Je.IErrExpander exp, Exception exception, Ticket ticket, T result)
        {
            if (exception == null) return result;
            ticket.IsError = true;
            Trace.WriteLine(ticket);
            Trace.WriteLine(new Je.Exception(exception));
            return default(T);
        }

        public static TResult Catch<TTicket, TResult>(this Je.IErrExpander exp, Exception exception, Ticket ticket) where TTicket : Ticket, new()
        {
            return Catch<TTicket, TResult>(exp, exception, ticket, default(TResult));
        }

        public static TResult Catch<TTicket, TResult>(this Je.IErrExpander exp, Exception exception, Ticket ticket, TResult result) where TTicket : Ticket, new()
        {
            if (exception == null) return result;
            ticket.IsError = true;
            Trace.WriteLine(ticket);
            var ex = new Je.Exception(exception);
            var newTicket = new TTicket();
            newTicket.IsError = true;
            newTicket.Message = ex.Text;
            newTicket.Comment = ex.Description;
            Trace.WriteLine(newTicket);
            return default(TResult);
        }

        #endregion

        #region raise

        public static Exception Raise(this Je.IErrExpander exp, string errorMessage)
        {
            return new Exception(errorMessage);
        }

        public static Exception Raise(this Je.IErrExpander exp, Exception exception)
        {
            return new Je.Exception(exception);
        }

        public static Exception Raise(this Je.IErrExpander exp, Ticket ticket)
        {
            return new Je.Exception(ticket);
        }

        public static Exception Raise(this Je.IErrExpander expander)
        {
            return new Je.Exception();
        } 
        #endregion
    }
}