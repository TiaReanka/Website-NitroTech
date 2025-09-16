using System;
using System.Collections.Generic;

namespace NitroTechWebsite
{
    //  Login log entry
    public class LoginLogEntry
    {
        public string Username { get; set; }
        public DateTime LoginTime { get; set; }
        public string Role { get; set; }
        public string IpAddress { get; set; }
    }

    //  Logoff log entry
    public class UserLogoffEntry
    {
        public string Username { get; set; }
        public DateTime LogoffTime { get; set; }
        public TimeSpan SessionDuration { get; set; } //pray Calvin remembers to do this
    }

    //  Add User log entry
    public class UserAddEntry
    {
        public string AdminUsername { get; set; }
        public string NewUser { get; set; }
        public DateTime AddTime { get; set; }
    }

    //  Delete User log entry
    public class UserDeleteEntry
    {
        public string AdminUsername { get; set; }
        public string DeletedUser { get; set; }
        public DateTime DeleteTime { get; set; }
    }

    //  Reset Password log entry
    public class UserResetPasswordEntry
    {
        public string AdminUsername { get; set; }
        public string TargetUser { get; set; }
        public DateTime ResetTime { get; set; }
    }

    public static class LogQueue
    {
        // Queues
        private static readonly Queue<LoginLogEntry> loginQueue = new Queue<LoginLogEntry>();
        private static readonly Queue<UserLogoffEntry> logoffQueue = new Queue<UserLogoffEntry>();
        private static readonly Queue<UserAddEntry> addUserQueue = new Queue<UserAddEntry>();
        private static readonly Queue<UserDeleteEntry> deleteUserQueue = new Queue<UserDeleteEntry>();
        private static readonly Queue<UserResetPasswordEntry> resetPasswordQueue = new Queue<UserResetPasswordEntry>();

        // Locks
        private static readonly object loginLock = new object();
        private static readonly object logoffLock = new object();
        private static readonly object addUserLock = new object();
        private static readonly object deleteUserLock = new object();
        private static readonly object resetPasswordLock = new object();

        // 🔹 Enqueue methods
        public static void EnqueueLogin(LoginLogEntry entry)
        {
            lock (loginLock) { loginQueue.Enqueue(entry); }
        }

        public static void EnqueueLogoff(UserLogoffEntry entry)
        {
            lock (logoffLock) { logoffQueue.Enqueue(entry); }
        }

        public static void EnqueueAddUser(UserAddEntry entry)
        {
            lock (addUserLock) { addUserQueue.Enqueue(entry); }
        }

        public static void EnqueueDeleteUser(UserDeleteEntry entry)
        {
            lock (deleteUserLock) { deleteUserQueue.Enqueue(entry); }
        }

        public static void EnqueueResetPassword(UserResetPasswordEntry entry)
        {
            lock (resetPasswordLock) { resetPasswordQueue.Enqueue(entry); }
        }

        // 🔹 Flush methods
        public static List<LoginLogEntry> FlushLoginQueue()
        {
            lock (loginLock)
            {
                var logs = new List<LoginLogEntry>();
                while (loginQueue.Count > 0) logs.Add(loginQueue.Dequeue());
                return logs;
            }
        }

        public static List<UserLogoffEntry> FlushLogoffQueue()
        {
            lock (logoffLock)
            {
                var logs = new List<UserLogoffEntry>();
                while (logoffQueue.Count > 0) logs.Add(logoffQueue.Dequeue());
                return logs;
            }
        }

        public static List<UserAddEntry> FlushAddUserQueue()
        {
            lock (addUserLock)
            {
                var logs = new List<UserAddEntry>();
                while (addUserQueue.Count > 0) logs.Add(addUserQueue.Dequeue());
                return logs;
            }
        }

        public static List<UserDeleteEntry> FlushDeleteUserQueue()
        {
            lock (deleteUserLock)
            {
                var logs = new List<UserDeleteEntry>();
                while (deleteUserQueue.Count > 0) logs.Add(deleteUserQueue.Dequeue());
                return logs;
            }
        }

        public static List<UserResetPasswordEntry> FlushResetPasswordQueue()
        {
            lock (resetPasswordLock)
            {
                var logs = new List<UserResetPasswordEntry>();
                while (resetPasswordQueue.Count > 0) logs.Add(resetPasswordQueue.Dequeue());
                return logs;
            }
        }

        // 🔹 Count properties
        public static int LoginCount { get { lock (loginLock) return loginQueue.Count; } }
        public static int LogoffCount { get { lock (logoffLock) return logoffQueue.Count; } }
        public static int AddUserCount { get { lock (addUserLock) return addUserQueue.Count; } }
        public static int DeleteUserCount { get { lock (deleteUserLock) return deleteUserQueue.Count; } }
        public static int ResetPasswordCount { get { lock (resetPasswordLock) return resetPasswordQueue.Count; } }
    }
}
